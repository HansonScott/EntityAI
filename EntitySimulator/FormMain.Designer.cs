namespace EntitySimulator
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.EnvironmentPanel = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPlaceSound = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(95, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New Simulation";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 41);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(95, 23);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run Simulation";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.BackColor = System.Drawing.Color.Black;
            this.tbOutput.ForeColor = System.Drawing.Color.White;
            this.tbOutput.Location = new System.Drawing.Point(7, 296);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(792, 152);
            this.tbOutput.TabIndex = 2;
            // 
            // EnvironmentPanel
            // 
            this.EnvironmentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EnvironmentPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.EnvironmentPanel.Location = new System.Drawing.Point(113, 12);
            this.EnvironmentPanel.Name = "EnvironmentPanel";
            this.EnvironmentPanel.Size = new System.Drawing.Size(675, 278);
            this.EnvironmentPanel.TabIndex = 3;
            this.EnvironmentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.EnvironmentPanel_Paint);
            this.EnvironmentPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EnvironmentPanel_MouseClick);
            this.EnvironmentPanel.MouseEnter += new System.EventHandler(this.EnvironmentPanel_MouseEnter);
            this.EnvironmentPanel.MouseLeave += new System.EventHandler(this.EnvironmentPanel_MouseLeave);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(12, 70);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(95, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop Simulation";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPlaceSound
            // 
            this.btnPlaceSound.Location = new System.Drawing.Point(12, 162);
            this.btnPlaceSound.Name = "btnPlaceSound";
            this.btnPlaceSound.Size = new System.Drawing.Size(95, 23);
            this.btnPlaceSound.TabIndex = 5;
            this.btnPlaceSound.Text = "Place Sound";
            this.btnPlaceSound.UseVisualStyleBackColor = true;
            this.btnPlaceSound.Click += new System.EventHandler(this.btnPlaceSound_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnPlaceSound);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.EnvironmentPanel);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnNew);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Panel EnvironmentPanel;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlaceSound;
    }
}

