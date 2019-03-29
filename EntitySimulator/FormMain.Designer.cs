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
            this.btnPlaceSensoryInput = new System.Windows.Forms.Button();
            this.cbSoundType = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnEntityStats = new System.Windows.Forms.Button();
            this.cbSightType = new System.Windows.Forms.ComboBox();
            this.btnPlaceSight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(122, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New Simulation";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 41);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(122, 23);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run Simulation";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.BackColor = System.Drawing.Color.Black;
            this.tbOutput.ForeColor = System.Drawing.Color.White;
            this.tbOutput.Location = new System.Drawing.Point(3, 3);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(654, 161);
            this.tbOutput.TabIndex = 2;
            // 
            // EnvironmentPanel
            // 
            this.EnvironmentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EnvironmentPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.EnvironmentPanel.Location = new System.Drawing.Point(0, 0);
            this.EnvironmentPanel.Name = "EnvironmentPanel";
            this.EnvironmentPanel.Size = new System.Drawing.Size(654, 270);
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
            this.btnStop.Size = new System.Drawing.Size(122, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop Simulation";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPlaceSensoryInput
            // 
            this.btnPlaceSensoryInput.Location = new System.Drawing.Point(12, 162);
            this.btnPlaceSensoryInput.Name = "btnPlaceSensoryInput";
            this.btnPlaceSensoryInput.Size = new System.Drawing.Size(122, 23);
            this.btnPlaceSensoryInput.TabIndex = 5;
            this.btnPlaceSensoryInput.Text = "Place Sound";
            this.btnPlaceSensoryInput.UseVisualStyleBackColor = true;
            this.btnPlaceSensoryInput.Click += new System.EventHandler(this.btnPlaceSound_Click);
            // 
            // cbSoundType
            // 
            this.cbSoundType.FormattingEnabled = true;
            this.cbSoundType.Items.AddRange(new object[] {
            "Water"});
            this.cbSoundType.Location = new System.Drawing.Point(13, 135);
            this.cbSoundType.Name = "cbSoundType";
            this.cbSoundType.Size = new System.Drawing.Size(121, 21);
            this.cbSoundType.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(140, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.EnvironmentPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbOutput);
            this.splitContainer1.Size = new System.Drawing.Size(657, 444);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.TabIndex = 7;
            // 
            // btnEntityStats
            // 
            this.btnEntityStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEntityStats.Location = new System.Drawing.Point(12, 415);
            this.btnEntityStats.Name = "btnEntityStats";
            this.btnEntityStats.Size = new System.Drawing.Size(122, 23);
            this.btnEntityStats.TabIndex = 8;
            this.btnEntityStats.Text = "Entity Stats";
            this.btnEntityStats.UseVisualStyleBackColor = true;
            this.btnEntityStats.Click += new System.EventHandler(this.btnEntityStats_Click);
            // 
            // cbSightType
            // 
            this.cbSightType.FormattingEnabled = true;
            this.cbSightType.Items.AddRange(new object[] {
            "Water"});
            this.cbSightType.Location = new System.Drawing.Point(14, 212);
            this.cbSightType.Name = "cbSightType";
            this.cbSightType.Size = new System.Drawing.Size(121, 21);
            this.cbSightType.TabIndex = 10;
            // 
            // btnPlaceSight
            // 
            this.btnPlaceSight.Location = new System.Drawing.Point(13, 239);
            this.btnPlaceSight.Name = "btnPlaceSight";
            this.btnPlaceSight.Size = new System.Drawing.Size(122, 23);
            this.btnPlaceSight.TabIndex = 9;
            this.btnPlaceSight.Text = "Place Sight";
            this.btnPlaceSight.UseVisualStyleBackColor = true;
            this.btnPlaceSight.Click += new System.EventHandler(this.btnPlaceSight_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbSightType);
            this.Controls.Add(this.btnPlaceSight);
            this.Controls.Add(this.btnEntityStats);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cbSoundType);
            this.Controls.Add(this.btnPlaceSensoryInput);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnNew);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Panel EnvironmentPanel;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlaceSensoryInput;
        private System.Windows.Forms.ComboBox cbSoundType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnEntityStats;
        private System.Windows.Forms.ComboBox cbSightType;
        private System.Windows.Forms.Button btnPlaceSight;
    }
}

