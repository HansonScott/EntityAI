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
        }

        #region Event Handlers
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
            Output("Starting simulation...");
            CurrentSimulator?.RunSimulation();
            Output("Simulation started.");
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            Output("Stopping simulation...");
            CurrentSimulator?.StopSimulation();

            // NOTE: because we need to wait until all the parts are shut down, we need to wait for the other threads...

            Output("Simulation stopped.");
        }
        private void EnvironmentPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentState == UIState.Placing_Sound)
            {
                CurrentSimulator.CurrentEnvironment.Sounds.Add(
                    new Sound(Sound.RecognitionFootPrint.Water, 50, new Position(e.X, e.Y, 0)));
            }
            else if (CurrentState == UIState.Placing_Sight)
            {
                EntityObject eo = new EntityObject();
                // set properties of eo

                // add to environment
                CurrentSimulator.CurrentEnvironment.Sights.Add(
                    new Sight(eo));
            }

            // and finally, set the state back to normal
            CurrentState = UIState.normal;
        }
        private void btnPlaceSound_Click(object sender, EventArgs e)
        {
            // get what type of input was chosen from the comboBox
            switch(cbSensoryType.SelectedItem)
            {
                case "Sound":
                    CurrentState = UIState.Placing_Sound;
                    Output("Placing sound...");
                    break;
                default:
                    break;
            }
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
        #endregion

        private void Output(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(Output), new object[] { msg});
                return;
            }

            string s = DateTime.Now.ToString("HH:mm:ss: ") + msg + Environment.NewLine;
            tbOutput.AppendText(s);
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
                g.DrawString("*", DefaultFont, Brushes.DarkBlue, (float)s.Origin.X, (float)s.Origin.Y);
            }

            foreach (Sight s in currentEnvironment.Sights)
            {
                //g.DrawString("@", DefaultFont, Brushes.Gold, (float)s.X, (float)s.Y);
            }

        }
        #endregion

        private void btnEntityStats_Click(object sender, EventArgs e)
        {
            Form_Entity_Viewer frm = new Form_Entity_Viewer();
            frm.LoadEntity(this.CurrentSimulator.Protagonist);
            frm.Show(this);
        }
    }
}
