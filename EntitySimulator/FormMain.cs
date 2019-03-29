using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityAI;

namespace EntitySimulator
{
    public partial class FormMain : Form
    {
        #region Main
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
        #endregion

        Simulator CurrentSimulator;
        UIState CurrentState;

        private enum UIState
        {
            normal = 0,
            Placing_Sound = 1,
            Placing_Sight = 2,
        }

        public FormMain()
        {
            InitializeComponent();

            PopulateSoundCombo();
            PopulateSightCombo();
        }

        private void PopulateSoundCombo()
        {
            cbSoundType.DataSource = Enum.GetNames(typeof(Sound.RecognitionFootPrint));
        }
        private void PopulateSightCombo()
        {
            cbSightType.DataSource = Enum.GetNames(typeof(Sight.RecognitionFootPrint));
        }

        #region Event Handlers
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CurrentSimulator?.StopSimulation();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            // a new simulation calls for new textbox
            tbOutput.Clear();

            Output("Creating a new simulation...");
            CurrentSimulator = new Simulator();
            CurrentSimulator.CurrentEnvironment.OnTick += CurrentEnvironment_OnTick;
            CurrentSimulator.CurrentEnvironment.OnLog += CurrentEnvironment_OnLog;
            CurrentSimulator.Protagonist.OnLog += Protagonist_OnLog;
            Output("Simulation created.");
        }
        private void CurrentEnvironment_OnTick(object sender, EntityLogging.EntityLoggingEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, EntityLogging.EntityLoggingEventArgs>(CurrentEnvironment_OnTick), new object[] { sender, e });
                return;
            }

            // call the panel to repaint
            this.EnvironmentPanel.Refresh();
        }
        private void CurrentEnvironment_OnLog(object sender, EntityLogging.EntityLoggingEventArgs e)
        {
            string msg = $"Environment - [{e.Log.Severity.ToString()}] {e.Log.Message}";
            Output(msg);
        }
        private void Protagonist_OnLog(object sender, EntityLogging.EntityLoggingEventArgs e)
        {
            string msg = $"Entity - [{e.Log.Severity.ToString()}] {e.Log.Message}";
            Output(msg);
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            if(CurrentSimulator == null)
            {
                Output("Run failed, need to create a new simulation first.");
                return;
            }

            Output("Starting simulation...");
            bool result = CurrentSimulator.RunSimulation();
            if(result)
            {
                Output("Simulation started.");
            }
            else
            {
                Output("Simulation unable to start.");
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (CurrentSimulator == null)
            {
                Output("Stop failed, need to create a new simulation first.");
                return;
            }

            Output("Stopping simulation...");
            CurrentSimulator.StopSimulation();

            // NOTE: because we need to wait until all the parts are shut down, we need to wait for the other threads...

            Output("Simulation stopped.");
        }
        private void btnPlaceSound_Click(object sender, EventArgs e)
        {
            CurrentState = UIState.Placing_Sound;
            Output("Placing sound of " + cbSoundType.SelectedItem);
        }
        private void btnPlaceSight_Click(object sender, EventArgs e)
        {
            CurrentState = UIState.Placing_Sight;
            Output("Placing sight of " + cbSightType.SelectedItem);
        }
        private void EnvironmentPanel_MouseEnter(object sender, EventArgs e)
        {
            if (CurrentState != UIState.normal)
            {
                Cursor.Current = Cursors.Cross;
            }
        }
        private void EnvironmentPanel_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
        private void EnvironmentPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentState == UIState.Placing_Sound)
            {
                string item = cbSoundType.SelectedItem.ToString();
                Sound.RecognitionFootPrint snd = (Sound.RecognitionFootPrint)Enum.Parse(typeof(Sound.RecognitionFootPrint), item);
                CurrentSimulator.CurrentEnvironment.Sounds.Add(
                    new Sound(snd, 50, new Position(e.X, e.Y, 0)));
            }
            else if (CurrentState == UIState.Placing_Sight)
            {
                string item = cbSightType.SelectedItem.ToString();
                Sight.RecognitionFootPrint sght = (Sight.RecognitionFootPrint)Enum.Parse(typeof(Sight.RecognitionFootPrint), item);
                CurrentSimulator.CurrentEnvironment.Sights.Add(
                    new Sight(new EntityObject(sght, Sound.RecognitionFootPrint.Unknown, new Position(e.X, e.Y, 0))));
            }

            // and finally, set the state back to normal
            CurrentState = UIState.normal;
        }
        private void btnEntityStats_Click(object sender, EventArgs e)
        {
            if (this.CurrentSimulator == null || this.CurrentSimulator.Protagonist == null)
            {
                Output("create a new simulation before opening the entity stat viewer.");
                return;
            }

            Form_Entity_Viewer frm = new Form_Entity_Viewer();
            frm.LoadEntity(this.CurrentSimulator.Protagonist);
            frm.Show(this);
        }

        #endregion

        private void Output(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(Output), new object[] { msg});
                return;
            }

            string s = DateTime.Now.ToString("HH:mm:ss: ") + msg + Environment.NewLine;

            try
            {
                tbOutput.AppendText(s);
            }
            catch { }// in this case, just don't print.
        }

        #region Drawing Environment
        private void EnvironmentPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if(this.CurrentSimulator != null &&
                this.CurrentSimulator.CurrentEnvironment != null)
            {
                DrawEnvironment(this.CurrentSimulator.CurrentEnvironment, g);
            }
        }

        private void DrawEnvironment(EntityEnvironment currentEnvironment, Graphics g)
        {
            #region Draw simulation timer
            TimeSpan T = this.CurrentSimulator.RunTime;
            string Duration = $"Run time: {T.Hours}h: {T.Minutes}m: {T.Seconds}s";
            Font F = DefaultFont;
            Brush B = Brushes.Black;
            g.DrawString(Duration, F, B, 5, 5);
            #endregion

            // draw entity
            g.DrawString("@", DefaultFont, Brushes.Goldenrod, 
                (float)CurrentSimulator.Protagonist.PositionCurrent.X, 
                (float)CurrentSimulator.Protagonist.PositionCurrent.Y);

            // draw sights, sounds, etc.
            foreach(Sound s in currentEnvironment.Sounds)
            {
                string snd = Enum.GetName(typeof(Sound.RecognitionFootPrint), s.FootPrint);
                g.DrawString(snd.Substring(0,1), DefaultFont, Brushes.LightBlue, (float)s.Origin.X, (float)s.Origin.Y);
            }

            foreach (Sight s in currentEnvironment.Sights)
            {
                string sght = Enum.GetName(typeof(Sight.RecognitionFootPrint), s.FootPrint);
                g.DrawString(sght.Substring(0,1), DefaultFont, Brushes.Blue, (float)s.Origin.X, (float)s.Origin.Y);
            }

        }
        #endregion
    }
}
