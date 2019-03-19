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
            TimeSpan T = this.CurrentSimulator.RunTime;
            string Duration = $"Run time: {T.Hours}h: {T.Minutes}m: {T.Seconds}s";

            Font F = DefaultFont;
            Brush B = Brushes.Black;

            g.DrawString(Duration, F, B, 5, 5);

            // draw sights, sounds, etc.
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            Output("Stopping simulation...");
            CurrentSimulator?.StopSimulation();

            // NOTE: because we need to wait until all the parts are shut down, we need to wait for the other threads...

            Output("Simulation stopped.");
        }
    }
}
