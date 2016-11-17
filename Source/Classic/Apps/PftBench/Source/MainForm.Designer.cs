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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._maxMfnLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._recordBox = new System.Windows.Forms.TextBox();
            this._outputTabControl = new System.Windows.Forms.TabControl();
            this._plainTextPage = new System.Windows.Forms.TabPage();
            this._resutlBox = new System.Windows.Forms.TextBox();
            this._rtfPage = new System.Windows.Forms.TabPage();
            this._rtfBox = new System.Windows.Forms.RichTextBox();
            this._htmlPage = new System.Windows.Forms.TabPage();
            this._htmlBox = new System.Windows.Forms.WebBrowser();
            this._warningPage = new System.Windows.Forms.TabPage();
            this._warningBox = new System.Windows.Forms.TextBox();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._newButton = new System.Windows.Forms.ToolStripButton();
            this._openButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._parseButton = new System.Windows.Forms.ToolStripButton();
            this._goButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._databaseBox = new System.Windows.Forms.ToolStripComboBox();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._pftBox = new IrbisUI.PftEditorControl();
            this._astTabControl = new System.Windows.Forms.TabControl();
            this._astPage = new System.Windows.Forms.TabPage();
            this._pftTreeView = new IrbisUI.PftTreeView();
            this._tokenPage = new System.Windows.Forms.TabPage();
            this._tokenGrid = new IrbisUI.PftTokenGrid();
            this._recordPage = new System.Windows.Forms.TabPage();
            this._recordGrid = new IrbisUI.RecordViewGrid();
            this._varsPage = new System.Windows.Forms.TabPage();
            this._varsGrid = new IrbisUI.PftVariableGrid();
            this._globalsPage = new System.Windows.Forms.TabPage();
            this._globalsGrid = new IrbisUI.PftGlobalGrid();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer2)).BeginInit();
            this._splitContainer2.Panel1.SuspendLayout();
            this._splitContainer2.Panel2.SuspendLayout();
            this._splitContainer2.SuspendLayout();
            this._outputTabControl.SuspendLayout();
            this._plainTextPage.SuspendLayout();
            this._rtfPage.SuspendLayout();
            this._htmlPage.SuspendLayout();
            this._warningPage.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer3)).BeginInit();
            this._splitContainer3.Panel1.SuspendLayout();
            this._splitContainer3.Panel2.SuspendLayout();
            this._splitContainer3.SuspendLayout();
            this._astTabControl.SuspendLayout();
            this._astPage.SuspendLayout();
            this._tokenPage.SuspendLayout();
            this._recordPage.SuspendLayout();
            this._varsPage.SuspendLayout();
            this._globalsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(754, 458);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(754, 531);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._maxMfnLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(754, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // _maxMfnLabel
            // 
            this._maxMfnLabel.Name = "_maxMfnLabel";
            this._maxMfnLabel.Size = new System.Drawing.Size(12, 17);
            this._maxMfnLabel.Text = "_";
            // 
            // _splitContainer1
            // 
            this._splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer1.Location = new System.Drawing.Point(0, 0);
            this._splitContainer1.Name = "_splitContainer1";
            this._splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._splitContainer3);
            // 
            // _splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._splitContainer2);
            this._splitContainer1.Size = new System.Drawing.Size(754, 458);
            this._splitContainer1.SplitterDistance = 155;
            this._splitContainer1.TabIndex = 0;
            // 
            // _splitContainer2
            // 
            this._splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer2.Location = new System.Drawing.Point(0, 0);
            this._splitContainer2.Name = "_splitContainer2";
            // 
            // _splitContainer2.Panel1
            // 
            this._splitContainer2.Panel1.Controls.Add(this._recordBox);
            // 
            // _splitContainer2.Panel2
            // 
            this._splitContainer2.Panel2.Controls.Add(this._outputTabControl);
            this._splitContainer2.Size = new System.Drawing.Size(754, 299);
            this._splitContainer2.SplitterDistance = 371;
            this._splitContainer2.TabIndex = 0;
            // 
            // _recordBox
            // 
            this._recordBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordBox.Location = new System.Drawing.Point(0, 0);
            this._recordBox.Multiline = true;
            this._recordBox.Name = "_recordBox";
            this._recordBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._recordBox.Size = new System.Drawing.Size(371, 299);
            this._recordBox.TabIndex = 0;
            this._recordBox.Text = resources.GetString("_recordBox.Text");
            // 
            // _outputTabControl
            // 
            this._outputTabControl.Controls.Add(this._plainTextPage);
            this._outputTabControl.Controls.Add(this._rtfPage);
            this._outputTabControl.Controls.Add(this._htmlPage);
            this._outputTabControl.Controls.Add(this._warningPage);
            this._outputTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outputTabControl.Location = new System.Drawing.Point(0, 0);
            this._outputTabControl.Margin = new System.Windows.Forms.Padding(2);
            this._outputTabControl.Name = "_outputTabControl";
            this._outputTabControl.SelectedIndex = 0;
            this._outputTabControl.Size = new System.Drawing.Size(379, 299);
            this._outputTabControl.TabIndex = 1;
            // 
            // _plainTextPage
            // 
            this._plainTextPage.Controls.Add(this._resutlBox);
            this._plainTextPage.Location = new System.Drawing.Point(4, 22);
            this._plainTextPage.Margin = new System.Windows.Forms.Padding(2);
            this._plainTextPage.Name = "_plainTextPage";
            this._plainTextPage.Padding = new System.Windows.Forms.Padding(2);
            this._plainTextPage.Size = new System.Drawing.Size(371, 273);
            this._plainTextPage.TabIndex = 0;
            this._plainTextPage.Text = "Plain text";
            this._plainTextPage.UseVisualStyleBackColor = true;
            // 
            // _resutlBox
            // 
            this._resutlBox.BackColor = System.Drawing.SystemColors.Window;
            this._resutlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._resutlBox.Location = new System.Drawing.Point(2, 2);
            this._resutlBox.Multiline = true;
            this._resutlBox.Name = "_resutlBox";
            this._resutlBox.ReadOnly = true;
            this._resutlBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._resutlBox.Size = new System.Drawing.Size(367, 269);
            this._resutlBox.TabIndex = 0;
            // 
            // _rtfPage
            // 
            this._rtfPage.Controls.Add(this._rtfBox);
            this._rtfPage.Location = new System.Drawing.Point(4, 22);
            this._rtfPage.Margin = new System.Windows.Forms.Padding(2);
            this._rtfPage.Name = "_rtfPage";
            this._rtfPage.Padding = new System.Windows.Forms.Padding(2);
            this._rtfPage.Size = new System.Drawing.Size(371, 273);
            this._rtfPage.TabIndex = 1;
            this._rtfPage.Text = "RTF";
            this._rtfPage.UseVisualStyleBackColor = true;
            // 
            // _rtfBox
            // 
            this._rtfBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtfBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtfBox.Location = new System.Drawing.Point(2, 2);
            this._rtfBox.Margin = new System.Windows.Forms.Padding(2);
            this._rtfBox.Name = "_rtfBox";
            this._rtfBox.Size = new System.Drawing.Size(367, 269);
            this._rtfBox.TabIndex = 0;
            this._rtfBox.Text = "";
            // 
            // _htmlPage
            // 
            this._htmlPage.Controls.Add(this._htmlBox);
            this._htmlPage.Location = new System.Drawing.Point(4, 22);
            this._htmlPage.Margin = new System.Windows.Forms.Padding(2);
            this._htmlPage.Name = "_htmlPage";
            this._htmlPage.Padding = new System.Windows.Forms.Padding(2);
            this._htmlPage.Size = new System.Drawing.Size(371, 273);
            this._htmlPage.TabIndex = 2;
            this._htmlPage.Text = "HTML";
            this._htmlPage.UseVisualStyleBackColor = true;
            // 
            // _htmlBox
            // 
            this._htmlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._htmlBox.Location = new System.Drawing.Point(2, 2);
            this._htmlBox.Margin = new System.Windows.Forms.Padding(2);
            this._htmlBox.MinimumSize = new System.Drawing.Size(15, 16);
            this._htmlBox.Name = "_htmlBox";
            this._htmlBox.Size = new System.Drawing.Size(367, 269);
            this._htmlBox.TabIndex = 0;
            // 
            // _warningPage
            // 
            this._warningPage.Controls.Add(this._warningBox);
            this._warningPage.Location = new System.Drawing.Point(4, 22);
            this._warningPage.Margin = new System.Windows.Forms.Padding(2);
            this._warningPage.Name = "_warningPage";
            this._warningPage.Padding = new System.Windows.Forms.Padding(2);
            this._warningPage.Size = new System.Drawing.Size(371, 273);
            this._warningPage.TabIndex = 3;
            this._warningPage.Text = "Warnings";
            this._warningPage.UseVisualStyleBackColor = true;
            // 
            // _warningBox
            // 
            this._warningBox.BackColor = System.Drawing.SystemColors.Window;
            this._warningBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._warningBox.Location = new System.Drawing.Point(2, 2);
            this._warningBox.Margin = new System.Windows.Forms.Padding(2);
            this._warningBox.Multiline = true;
            this._warningBox.Name = "_warningBox";
            this._warningBox.ReadOnly = true;
            this._warningBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._warningBox.Size = new System.Drawing.Size(367, 269);
            this._warningBox.TabIndex = 0;
            // 
            // _menuStrip
            // 
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(754, 24);
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
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newButton,
            this._openButton,
            this._saveButton,
            this.toolStripSeparator2,
            this._parseButton,
            this._goButton,
            this.toolStripSeparator1,
            this._databaseBox});
            this._toolStrip.Location = new System.Drawing.Point(0, 24);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(754, 27);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _newButton
            // 
            this._newButton.Image = ((System.Drawing.Image)(resources.GetObject("_newButton.Image")));
            this._newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newButton.Name = "_newButton";
            this._newButton.Size = new System.Drawing.Size(55, 24);
            this._newButton.Text = "New";
            this._newButton.Click += new System.EventHandler(this._newButton_Click);
            // 
            // _openButton
            // 
            this._openButton.Image = ((System.Drawing.Image)(resources.GetObject("_openButton.Image")));
            this._openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(60, 24);
            this._openButton.Text = "Open";
            this._openButton.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _saveButton
            // 
            this._saveButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveButton.Image")));
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(55, 24);
            this._saveButton.Text = "Save";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // _parseButton
            // 
            this._parseButton.Image = ((System.Drawing.Image)(resources.GetObject("_parseButton.Image")));
            this._parseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._parseButton.Name = "_parseButton";
            this._parseButton.Size = new System.Drawing.Size(82, 24);
            this._parseButton.Text = "Parse (F4)";
            this._parseButton.Click += new System.EventHandler(this._parseButton_Click);
            // 
            // _goButton
            // 
            this._goButton.Image = ((System.Drawing.Image)(resources.GetObject("_goButton.Image")));
            this._goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(72, 24);
            this._goButton.Text = "Go! (F5)";
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // _databaseBox
            // 
            this._databaseBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._databaseBox.Name = "_databaseBox";
            this._databaseBox.Size = new System.Drawing.Size(121, 27);
            this._databaseBox.SelectedIndexChanged += new System.EventHandler(this._databaseBox_SelectedIndexChanged);
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "PFT files|*.pft|All files|*.*";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "PFT files|*.pft|All files|*.*";
            // 
            // _splitContainer3
            // 
            this._splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer3.Location = new System.Drawing.Point(0, 0);
            this._splitContainer3.Name = "_splitContainer3";
            // 
            // _splitContainer3.Panel1
            // 
            this._splitContainer3.Panel1.Controls.Add(this._pftBox);
            // 
            // _splitContainer3.Panel2
            // 
            this._splitContainer3.Panel2.Controls.Add(this._astTabControl);
            this._splitContainer3.Size = new System.Drawing.Size(754, 155);
            this._splitContainer3.SplitterDistance = 369;
            this._splitContainer3.TabIndex = 1;
            // 
            // _pftBox
            // 
            this._pftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pftBox.IsReadOnly = false;
            this._pftBox.Location = new System.Drawing.Point(0, 0);
            this._pftBox.Margin = new System.Windows.Forms.Padding(2);
            this._pftBox.Name = "_pftBox";
            this._pftBox.Size = new System.Drawing.Size(369, 155);
            this._pftBox.TabIndex = 0;
            this._pftBox.Text = "/* My first PFT script\r\n\'Some text\' /\r\nv200^a, \" : \"v200^e, \" / \"v200^f";
            // 
            // _astTabControl
            // 
            this._astTabControl.Controls.Add(this._astPage);
            this._astTabControl.Controls.Add(this._tokenPage);
            this._astTabControl.Controls.Add(this._recordPage);
            this._astTabControl.Controls.Add(this._varsPage);
            this._astTabControl.Controls.Add(this._globalsPage);
            this._astTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._astTabControl.Location = new System.Drawing.Point(0, 0);
            this._astTabControl.Name = "_astTabControl";
            this._astTabControl.SelectedIndex = 0;
            this._astTabControl.Size = new System.Drawing.Size(381, 155);
            this._astTabControl.TabIndex = 1;
            // 
            // _astPage
            // 
            this._astPage.Controls.Add(this._pftTreeView);
            this._astPage.Location = new System.Drawing.Point(4, 22);
            this._astPage.Name = "_astPage";
            this._astPage.Padding = new System.Windows.Forms.Padding(3);
            this._astPage.Size = new System.Drawing.Size(373, 129);
            this._astPage.TabIndex = 0;
            this._astPage.Text = "AST";
            this._astPage.UseVisualStyleBackColor = true;
            // 
            // _pftTreeView
            // 
            this._pftTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pftTreeView.Location = new System.Drawing.Point(3, 3);
            this._pftTreeView.Margin = new System.Windows.Forms.Padding(4);
            this._pftTreeView.Name = "_pftTreeView";
            this._pftTreeView.Size = new System.Drawing.Size(367, 123);
            this._pftTreeView.TabIndex = 0;
            // 
            // _tokenPage
            // 
            this._tokenPage.Controls.Add(this._tokenGrid);
            this._tokenPage.Location = new System.Drawing.Point(4, 22);
            this._tokenPage.Name = "_tokenPage";
            this._tokenPage.Padding = new System.Windows.Forms.Padding(3);
            this._tokenPage.Size = new System.Drawing.Size(373, 129);
            this._tokenPage.TabIndex = 1;
            this._tokenPage.Text = "Tokens";
            this._tokenPage.UseVisualStyleBackColor = true;
            // 
            // _tokenGrid
            // 
            this._tokenGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tokenGrid.Location = new System.Drawing.Point(3, 3);
            this._tokenGrid.Margin = new System.Windows.Forms.Padding(4);
            this._tokenGrid.Name = "_tokenGrid";
            this._tokenGrid.Size = new System.Drawing.Size(367, 123);
            this._tokenGrid.TabIndex = 0;
            this._tokenGrid.CellDoubleClick += new System.EventHandler(this._tokenGrid_CellDoubleClick);
            // 
            // _recordPage
            // 
            this._recordPage.Controls.Add(this._recordGrid);
            this._recordPage.Location = new System.Drawing.Point(4, 22);
            this._recordPage.Name = "_recordPage";
            this._recordPage.Padding = new System.Windows.Forms.Padding(3);
            this._recordPage.Size = new System.Drawing.Size(373, 129);
            this._recordPage.TabIndex = 2;
            this._recordPage.Text = "Record";
            this._recordPage.UseVisualStyleBackColor = true;
            // 
            // _recordGrid
            // 
            this._recordGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._recordGrid.Location = new System.Drawing.Point(3, 3);
            this._recordGrid.Margin = new System.Windows.Forms.Padding(4);
            this._recordGrid.Name = "_recordGrid";
            this._recordGrid.Size = new System.Drawing.Size(367, 123);
            this._recordGrid.TabIndex = 0;
            // 
            // _varsPage
            // 
            this._varsPage.Controls.Add(this._varsGrid);
            this._varsPage.Location = new System.Drawing.Point(4, 22);
            this._varsPage.Name = "_varsPage";
            this._varsPage.Padding = new System.Windows.Forms.Padding(3);
            this._varsPage.Size = new System.Drawing.Size(373, 129);
            this._varsPage.TabIndex = 3;
            this._varsPage.Text = "Variables";
            this._varsPage.UseVisualStyleBackColor = true;
            // 
            // _varsGrid
            // 
            this._varsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._varsGrid.Location = new System.Drawing.Point(3, 3);
            this._varsGrid.Margin = new System.Windows.Forms.Padding(4);
            this._varsGrid.Name = "_varsGrid";
            this._varsGrid.Size = new System.Drawing.Size(367, 123);
            this._varsGrid.TabIndex = 0;
            // 
            // _globalsPage
            // 
            this._globalsPage.Controls.Add(this._globalsGrid);
            this._globalsPage.Location = new System.Drawing.Point(4, 22);
            this._globalsPage.Name = "_globalsPage";
            this._globalsPage.Padding = new System.Windows.Forms.Padding(3);
            this._globalsPage.Size = new System.Drawing.Size(373, 129);
            this._globalsPage.TabIndex = 4;
            this._globalsPage.Text = "Globals";
            this._globalsPage.UseVisualStyleBackColor = true;
            // 
            // _globalsGrid
            // 
            this._globalsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._globalsGrid.Location = new System.Drawing.Point(3, 3);
            this._globalsGrid.Margin = new System.Windows.Forms.Padding(4);
            this._globalsGrid.Name = "_globalsGrid";
            this._globalsGrid.Size = new System.Drawing.Size(367, 123);
            this._globalsGrid.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 531);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this._menuStrip;
            this.Name = "MainForm";
            this.Text = "PFT Bench";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).EndInit();
            this._splitContainer1.ResumeLayout(false);
            this._splitContainer2.Panel1.ResumeLayout(false);
            this._splitContainer2.Panel1.PerformLayout();
            this._splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer2)).EndInit();
            this._splitContainer2.ResumeLayout(false);
            this._outputTabControl.ResumeLayout(false);
            this._plainTextPage.ResumeLayout(false);
            this._plainTextPage.PerformLayout();
            this._rtfPage.ResumeLayout(false);
            this._htmlPage.ResumeLayout(false);
            this._warningPage.ResumeLayout(false);
            this._warningPage.PerformLayout();
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._splitContainer3.Panel1.ResumeLayout(false);
            this._splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer3)).EndInit();
            this._splitContainer3.ResumeLayout(false);
            this._astTabControl.ResumeLayout(false);
            this._astPage.ResumeLayout(false);
            this._tokenPage.ResumeLayout(false);
            this._recordPage.ResumeLayout(false);
            this._varsPage.ResumeLayout(false);
            this._globalsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.SplitContainer _splitContainer1;
        private System.Windows.Forms.SplitContainer _splitContainer2;
        private System.Windows.Forms.TextBox _recordBox;
        private System.Windows.Forms.TextBox _resutlBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton _goButton;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer _splitContainer3;
        private IrbisUI.PftTreeView _pftTreeView;
        private System.Windows.Forms.TabControl _astTabControl;
        private System.Windows.Forms.TabPage _astPage;
        private System.Windows.Forms.TabPage _tokenPage;
        private IrbisUI.PftTokenGrid _tokenGrid;
        private System.Windows.Forms.TabPage _recordPage;
        private IrbisUI.RecordViewGrid _recordGrid;
        private System.Windows.Forms.ToolStripButton _parseButton;
        private System.Windows.Forms.TabPage _varsPage;
        private IrbisUI.PftVariableGrid _varsGrid;
        private System.Windows.Forms.TabPage _globalsPage;
        private IrbisUI.PftGlobalGrid _globalsGrid;
        private System.Windows.Forms.TabControl _outputTabControl;
        private System.Windows.Forms.TabPage _plainTextPage;
        private System.Windows.Forms.TabPage _rtfPage;
        private System.Windows.Forms.TabPage _htmlPage;
        private System.Windows.Forms.TabPage _warningPage;
        private System.Windows.Forms.TextBox _warningBox;
        private System.Windows.Forms.WebBrowser _htmlBox;
        private System.Windows.Forms.RichTextBox _rtfBox;
        private IrbisUI.PftEditorControl _pftBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox _databaseBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _maxMfnLabel;
        private System.Windows.Forms.ToolStripButton _newButton;
        private System.Windows.Forms.ToolStripButton _openButton;
        private System.Windows.Forms.ToolStripButton _saveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}