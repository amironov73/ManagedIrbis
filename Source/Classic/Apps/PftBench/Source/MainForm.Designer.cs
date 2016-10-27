namespace PftBench
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._pftBox = new System.Windows.Forms.TextBox();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._astPage = new System.Windows.Forms.TabPage();
            this._pftTreeView = new IrbisUI.PftTreeView();
            this._tokenPage = new System.Windows.Forms.TabPage();
            this._tokenGrid = new IrbisUI.PftTokenGrid();
            this._recordPage = new System.Windows.Forms.TabPage();
            this._recordGrid = new IrbisUI.RecordViewGrid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._recordBox = new System.Windows.Forms.TextBox();
            this._resutlBox = new System.Windows.Forms.TextBox();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._parseButton = new System.Windows.Forms.ToolStripButton();
            this._goButton = new System.Windows.Forms.ToolStripButton();
            this._varsPage = new System.Windows.Forms.TabPage();
            this._varsGrid = new IrbisUI.PftVariableGrid();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._astPage.SuspendLayout();
            this._tokenPage.SuspendLayout();
            this._recordPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._varsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(784, 513);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(784, 562);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(784, 513);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._pftBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._tabControl);
            this.splitContainer3.Size = new System.Drawing.Size(784, 175);
            this.splitContainer3.SplitterDistance = 412;
            this.splitContainer3.TabIndex = 1;
            // 
            // _pftBox
            // 
            this._pftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pftBox.Location = new System.Drawing.Point(0, 0);
            this._pftBox.Multiline = true;
            this._pftBox.Name = "_pftBox";
            this._pftBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._pftBox.Size = new System.Drawing.Size(412, 175);
            this._pftBox.TabIndex = 0;
            this._pftBox.Text = "v200^a, \" : \"v200^e, \" / \"v200^f";
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._astPage);
            this._tabControl.Controls.Add(this._tokenPage);
            this._tabControl.Controls.Add(this._recordPage);
            this._tabControl.Controls.Add(this._varsPage);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(368, 175);
            this._tabControl.TabIndex = 1;
            // 
            // _astPage
            // 
            this._astPage.Controls.Add(this._pftTreeView);
            this._astPage.Location = new System.Drawing.Point(4, 22);
            this._astPage.Name = "_astPage";
            this._astPage.Padding = new System.Windows.Forms.Padding(3);
            this._astPage.Size = new System.Drawing.Size(360, 149);
            this._astPage.TabIndex = 0;
            this._astPage.Text = "AST";
            this._astPage.UseVisualStyleBackColor = true;
            // 
            // _pftTreeView
            // 
            this._pftTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pftTreeView.Location = new System.Drawing.Point(3, 3);
            this._pftTreeView.Name = "_pftTreeView";
            this._pftTreeView.Size = new System.Drawing.Size(354, 143);
            this._pftTreeView.TabIndex = 0;
            // 
            // _tokenPage
            // 
            this._tokenPage.Controls.Add(this._tokenGrid);
            this._tokenPage.Location = new System.Drawing.Point(4, 22);
            this._tokenPage.Name = "_tokenPage";
            this._tokenPage.Padding = new System.Windows.Forms.Padding(3);
            this._tokenPage.Size = new System.Drawing.Size(360, 149);
            this._tokenPage.TabIndex = 1;
            this._tokenPage.Text = "Tokens";
            this._tokenPage.UseVisualStyleBackColor = true;
            // 
            // _tokenGrid
            // 
            this._tokenGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tokenGrid.Location = new System.Drawing.Point(3, 3);
            this._tokenGrid.Name = "_tokenGrid";
            this._tokenGrid.Size = new System.Drawing.Size(354, 143);
            this._tokenGrid.TabIndex = 0;
            // 
            // _recordPage
            // 
            this._recordPage.Controls.Add(this._recordGrid);
            this._recordPage.Location = new System.Drawing.Point(4, 22);
            this._recordPage.Name = "_recordPage";
            this._recordPage.Padding = new System.Windows.Forms.Padding(3);
            this._recordPage.Size = new System.Drawing.Size(360, 149);
            this._recordPage.TabIndex = 2;
            this._recordPage.Text = "Record";
            this._recordPage.UseVisualStyleBackColor = true;
            // 
            // _recordGrid
            // 
            this._recordGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordGrid.Location = new System.Drawing.Point(3, 3);
            this._recordGrid.Name = "_recordGrid";
            this._recordGrid.Size = new System.Drawing.Size(354, 143);
            this._recordGrid.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._recordBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._resutlBox);
            this.splitContainer2.Size = new System.Drawing.Size(784, 334);
            this.splitContainer2.SplitterDistance = 407;
            this.splitContainer2.TabIndex = 0;
            // 
            // _recordBox
            // 
            this._recordBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordBox.Location = new System.Drawing.Point(0, 0);
            this._recordBox.Multiline = true;
            this._recordBox.Name = "_recordBox";
            this._recordBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._recordBox.Size = new System.Drawing.Size(407, 334);
            this._recordBox.TabIndex = 0;
            this._recordBox.Text = resources.GetString("_recordBox.Text");
            // 
            // _resutlBox
            // 
            this._resutlBox.BackColor = System.Drawing.SystemColors.Window;
            this._resutlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._resutlBox.Location = new System.Drawing.Point(0, 0);
            this._resutlBox.Multiline = true;
            this._resutlBox.Name = "_resutlBox";
            this._resutlBox.ReadOnly = true;
            this._resutlBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._resutlBox.Size = new System.Drawing.Size(373, 334);
            this._resutlBox.TabIndex = 0;
            // 
            // _menuStrip
            // 
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(784, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._parseButton,
            this._goButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 24);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(784, 25);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _parseButton
            // 
            this._parseButton.Image = ((System.Drawing.Image)(resources.GetObject("_parseButton.Image")));
            this._parseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._parseButton.Name = "_parseButton";
            this._parseButton.Size = new System.Drawing.Size(78, 22);
            this._parseButton.Text = "Parse (F4)";
            this._parseButton.Click += new System.EventHandler(this._parseButton_Click);
            // 
            // _goButton
            // 
            this._goButton.Image = ((System.Drawing.Image)(resources.GetObject("_goButton.Image")));
            this._goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(68, 22);
            this._goButton.Text = "Go! (F5)";
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // _varsPage
            // 
            this._varsPage.Controls.Add(this._varsGrid);
            this._varsPage.Location = new System.Drawing.Point(4, 22);
            this._varsPage.Name = "_varsPage";
            this._varsPage.Padding = new System.Windows.Forms.Padding(3);
            this._varsPage.Size = new System.Drawing.Size(360, 149);
            this._varsPage.TabIndex = 3;
            this._varsPage.Text = "Variables";
            this._varsPage.UseVisualStyleBackColor = true;
            // 
            // _varsGrid
            // 
            this._varsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._varsGrid.Location = new System.Drawing.Point(3, 3);
            this._varsGrid.Name = "_varsGrid";
            this._varsGrid.Size = new System.Drawing.Size(354, 143);
            this._varsGrid.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this._menuStrip;
            this.Name = "MainForm";
            this.Text = "PFT Bench";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this._tabControl.ResumeLayout(false);
            this._astPage.ResumeLayout(false);
            this._tokenPage.ResumeLayout(false);
            this._recordPage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._varsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox _recordBox;
        private System.Windows.Forms.TextBox _resutlBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton _goButton;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox _pftBox;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private IrbisUI.PftTreeView _pftTreeView;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _astPage;
        private System.Windows.Forms.TabPage _tokenPage;
        private IrbisUI.PftTokenGrid _tokenGrid;
        private System.Windows.Forms.TabPage _recordPage;
        private IrbisUI.RecordViewGrid _recordGrid;
        private System.Windows.Forms.ToolStripButton _parseButton;
        private System.Windows.Forms.TabPage _varsPage;
        private IrbisUI.PftVariableGrid _varsGrid;

    }
}