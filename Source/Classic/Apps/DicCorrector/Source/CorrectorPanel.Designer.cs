namespace DicCorrector
{
    partial class CorrectorPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            IrbisUI.Grid.SiberianPalette siberianPalette1 = new IrbisUI.Grid.SiberianPalette();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._upperStrip = new System.Windows.Forms.ToolStrip();
            this._lowerStrip = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.siberianTermGrid1 = new IrbisUI.Grid.SiberianTermGrid();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 550);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 600);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._upperStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._lowerStrip);
            // 
            // _upperStrip
            // 
            this._upperStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._upperStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._upperStrip.Location = new System.Drawing.Point(0, 0);
            this._upperStrip.Name = "_upperStrip";
            this._upperStrip.Size = new System.Drawing.Size(800, 25);
            this._upperStrip.Stretch = true;
            this._upperStrip.TabIndex = 0;
            // 
            // _lowerStrip
            // 
            this._lowerStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._lowerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._lowerStrip.Location = new System.Drawing.Point(3, 25);
            this._lowerStrip.Name = "_lowerStrip";
            this._lowerStrip.Size = new System.Drawing.Size(34, 25);
            this._lowerStrip.Stretch = true;
            this._lowerStrip.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.siberianTermGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 550);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // siberianTermGrid1
            // 
            this.siberianTermGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.siberianTermGrid1.HeaderHeight = 26;
            this.siberianTermGrid1.Location = new System.Drawing.Point(0, 0);
            this.siberianTermGrid1.Name = "siberianTermGrid1";
            siberianPalette1.BackColor = System.Drawing.Color.DarkGray;
            siberianPalette1.DisabledBackColor = System.Drawing.Color.White;
            siberianPalette1.DisabledForeColor = System.Drawing.Color.DarkGray;
            siberianPalette1.ForeColor = System.Drawing.Color.Black;
            siberianPalette1.HeaderBackColor = System.Drawing.Color.LightGray;
            siberianPalette1.HeaderForeColor = System.Drawing.Color.Black;
            siberianPalette1.LineColor = System.Drawing.Color.Gray;
            siberianPalette1.Name = "Default";
            siberianPalette1.SelectedBackColor = System.Drawing.Color.Blue;
            siberianPalette1.SelectedForeColor = System.Drawing.Color.White;
            this.siberianTermGrid1.Palette = siberianPalette1;
            this.siberianTermGrid1.Size = new System.Drawing.Size(266, 550);
            this.siberianTermGrid1.TabIndex = 0;
            this.siberianTermGrid1.Text = "siberianTermGrid1";
            // 
            // CorrectorPanel
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "CorrectorPanel";
            this.Size = new System.Drawing.Size(800, 600);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip _upperStrip;
        private System.Windows.Forms.ToolStrip _lowerStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private IrbisUI.Grid.SiberianTermGrid siberianTermGrid1;
    }
}
