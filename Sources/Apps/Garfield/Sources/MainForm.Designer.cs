namespace Garfield
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._horizontalSplit = new System.Windows.Forms.SplitContainer();
            this._bottomSplit = new System.Windows.Forms.SplitContainer();
            this._topSplit = new System.Windows.Forms.SplitContainer();
            this.foundPanel1 = new IrbisUI.FoundPanel();
            this.previewPanel1 = new IrbisUI.PreviewPanel();
            this.dictionaryPanel1 = new IrbisUI.DictionaryPanel();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._horizontalSplit)).BeginInit();
            this._horizontalSplit.Panel1.SuspendLayout();
            this._horizontalSplit.Panel2.SuspendLayout();
            this._horizontalSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bottomSplit)).BeginInit();
            this._bottomSplit.Panel1.SuspendLayout();
            this._bottomSplit.Panel2.SuspendLayout();
            this._bottomSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._topSplit)).BeginInit();
            this._topSplit.Panel1.SuspendLayout();
            this._topSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._horizontalSplit);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(782, 500);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(782, 553);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(3, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(111, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // _horizontalSplit
            // 
            this._horizontalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._horizontalSplit.Location = new System.Drawing.Point(0, 0);
            this._horizontalSplit.Name = "_horizontalSplit";
            this._horizontalSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _horizontalSplit.Panel1
            // 
            this._horizontalSplit.Panel1.Controls.Add(this._topSplit);
            // 
            // _horizontalSplit.Panel2
            // 
            this._horizontalSplit.Panel2.Controls.Add(this._bottomSplit);
            this._horizontalSplit.Size = new System.Drawing.Size(782, 500);
            this._horizontalSplit.SplitterDistance = 312;
            this._horizontalSplit.TabIndex = 0;
            // 
            // _bottomSplit
            // 
            this._bottomSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bottomSplit.Location = new System.Drawing.Point(0, 0);
            this._bottomSplit.Name = "_bottomSplit";
            // 
            // _bottomSplit.Panel1
            // 
            this._bottomSplit.Panel1.Controls.Add(this.foundPanel1);
            // 
            // _bottomSplit.Panel2
            // 
            this._bottomSplit.Panel2.Controls.Add(this.previewPanel1);
            this._bottomSplit.Size = new System.Drawing.Size(782, 184);
            this._bottomSplit.SplitterDistance = 381;
            this._bottomSplit.TabIndex = 0;
            // 
            // _topSplit
            // 
            this._topSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._topSplit.Location = new System.Drawing.Point(0, 0);
            this._topSplit.Name = "_topSplit";
            // 
            // _topSplit.Panel1
            // 
            this._topSplit.Panel1.Controls.Add(this.dictionaryPanel1);
            this._topSplit.Size = new System.Drawing.Size(782, 312);
            this._topSplit.SplitterDistance = 260;
            this._topSplit.TabIndex = 0;
            // 
            // foundPanel1
            // 
            this.foundPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.foundPanel1.Location = new System.Drawing.Point(0, 0);
            this.foundPanel1.Name = "foundPanel1";
            this.foundPanel1.Size = new System.Drawing.Size(381, 184);
            this.foundPanel1.TabIndex = 0;
            // 
            // previewPanel1
            // 
            this.previewPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel1.Location = new System.Drawing.Point(0, 0);
            this.previewPanel1.Name = "previewPanel1";
            this.previewPanel1.Size = new System.Drawing.Size(397, 184);
            this.previewPanel1.TabIndex = 0;
            // 
            // dictionaryPanel1
            // 
            this.dictionaryPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dictionaryPanel1.Location = new System.Drawing.Point(0, 0);
            this.dictionaryPanel1.Name = "dictionaryPanel1";
            this.dictionaryPanel1.Size = new System.Drawing.Size(260, 312);
            this.dictionaryPanel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Garfield";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this._horizontalSplit.Panel1.ResumeLayout(false);
            this._horizontalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._horizontalSplit)).EndInit();
            this._horizontalSplit.ResumeLayout(false);
            this._bottomSplit.Panel1.ResumeLayout(false);
            this._bottomSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._bottomSplit)).EndInit();
            this._bottomSplit.ResumeLayout(false);
            this._topSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._topSplit)).EndInit();
            this._topSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer _horizontalSplit;
        private System.Windows.Forms.SplitContainer _bottomSplit;
        private System.Windows.Forms.SplitContainer _topSplit;
        private IrbisUI.FoundPanel foundPanel1;
        private IrbisUI.PreviewPanel previewPanel1;
        private IrbisUI.DictionaryPanel dictionaryPanel1;
    }
}

