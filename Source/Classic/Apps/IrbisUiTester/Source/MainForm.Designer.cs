namespace IrbisUiTester
{
    partial class MainForm
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._logBox = new AM.Windows.Forms.LogBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._actionsItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pingItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._slowBox = new AM.Windows.Forms.ToolStripCheckBox();
            this._brokenBox = new AM.Windows.Forms.ToolStripCheckBox();
            this._busyBox = new AM.Windows.Forms.ToolStripCheckBox();
            this._busyStripe = new IrbisUI.IrbisBusyStripe();
            this._retryBox = new AM.Windows.Forms.ToolStripCheckBox();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._busyStripe);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._logBox);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(782, 498);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(782, 553);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 345);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(782, 153);
            this._logBox.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._actionsItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _actionsItem
            // 
            this._actionsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pingItem});
            this._actionsItem.Name = "_actionsItem";
            this._actionsItem.Size = new System.Drawing.Size(70, 24);
            this._actionsItem.Text = "&Actions";
            // 
            // _pingItem
            // 
            this._pingItem.Name = "_pingItem";
            this._pingItem.Size = new System.Drawing.Size(113, 26);
            this._pingItem.Text = "&Ping";
            this._pingItem.Click += new System.EventHandler(this._pingItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._slowBox,
            this._brokenBox,
            this._busyBox,
            this._retryBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(782, 27);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 1;
            // 
            // _slowBox
            // 
            this._slowBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _slowBox
            // 
            this._slowBox.CheckBox.AccessibleName = "_slowBox";
            this._slowBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._slowBox.CheckBox.Checked = true;
            this._slowBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._slowBox.CheckBox.Location = new System.Drawing.Point(0, 1);
            this._slowBox.CheckBox.Name = "_slowBox";
            this._slowBox.CheckBox.Size = new System.Drawing.Size(63, 24);
            this._slowBox.CheckBox.TabIndex = 1;
            this._slowBox.CheckBox.Text = "Slow";
            this._slowBox.CheckBox.UseVisualStyleBackColor = false;
            this._slowBox.Name = "_slowBox";
            this._slowBox.Size = new System.Drawing.Size(63, 24);
            this._slowBox.Text = "Slow";
            // 
            // _brokenBox
            // 
            this._brokenBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _brokenBox
            // 
            this._brokenBox.CheckBox.AccessibleName = "_brokenBox";
            this._brokenBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._brokenBox.CheckBox.Checked = true;
            this._brokenBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._brokenBox.CheckBox.Location = new System.Drawing.Point(63, 1);
            this._brokenBox.CheckBox.Name = "_brokenBox";
            this._brokenBox.CheckBox.Size = new System.Drawing.Size(77, 24);
            this._brokenBox.CheckBox.TabIndex = 2;
            this._brokenBox.CheckBox.Text = "Broken";
            this._brokenBox.CheckBox.UseVisualStyleBackColor = false;
            this._brokenBox.Name = "_brokenBox";
            this._brokenBox.Size = new System.Drawing.Size(77, 24);
            this._brokenBox.Text = "Broken";
            // 
            // _busyBox
            // 
            this._busyBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _busyBox
            // 
            this._busyBox.CheckBox.AccessibleName = "_busyBox";
            this._busyBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._busyBox.CheckBox.Checked = true;
            this._busyBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._busyBox.CheckBox.Location = new System.Drawing.Point(140, 1);
            this._busyBox.CheckBox.Name = "_busyBox";
            this._busyBox.CheckBox.Size = new System.Drawing.Size(61, 24);
            this._busyBox.CheckBox.TabIndex = 3;
            this._busyBox.CheckBox.Text = "Busy";
            this._busyBox.CheckBox.UseVisualStyleBackColor = false;
            this._busyBox.Name = "_busyBox";
            this._busyBox.Size = new System.Drawing.Size(61, 24);
            this._busyBox.Text = "Busy";
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._busyStripe.ForeColor = System.Drawing.Color.Lime;
            this._busyStripe.Location = new System.Drawing.Point(0, 341);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(782, 4);
            this._busyStripe.TabIndex = 1;
            // 
            // _retryBox
            // 
            this._retryBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _retryBox
            // 
            this._retryBox.CheckBox.AccessibleName = "_retryBox";
            this._retryBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._retryBox.CheckBox.Checked = true;
            this._retryBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._retryBox.CheckBox.Location = new System.Drawing.Point(201, 1);
            this._retryBox.CheckBox.Name = "_retryBox";
            this._retryBox.CheckBox.Size = new System.Drawing.Size(65, 24);
            this._retryBox.CheckBox.TabIndex = 4;
            this._retryBox.CheckBox.Text = "Retry";
            this._retryBox.CheckBox.UseVisualStyleBackColor = false;
            this._retryBox.Name = "_retryBox";
            this._retryBox.Size = new System.Drawing.Size(65, 24);
            this._retryBox.Text = "Retry";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "IRBIS UI Tester";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private IrbisUI.IrbisBusyStripe _busyStripe;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _actionsItem;
        private System.Windows.Forms.ToolStripMenuItem _pingItem;
        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private AM.Windows.Forms.ToolStripCheckBox _slowBox;
        private AM.Windows.Forms.ToolStripCheckBox _brokenBox;
        private AM.Windows.Forms.ToolStripCheckBox _busyBox;
        private AM.Windows.Forms.ToolStripCheckBox _retryBox;
    }
}

