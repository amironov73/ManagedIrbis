namespace AM.Windows.Forms
{
    partial class RichTextEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ();
            }
            base.Dispose ( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextEditor));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._richTextBox = new System.Windows.Forms.RichTextBox();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            resources.ApplyResources(this._toolStrip, "_toolStrip");
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newToolStripButton,
            this._openToolStripButton,
            this._saveToolStripButton,
            this._printToolStripButton,
            this.toolStripSeparator,
            this._cutToolStripButton,
            this._copyToolStripButton,
            this._pasteToolStripButton,
            this.toolStripSeparator1});
            this._toolStrip.Name = "_toolStrip";
            // 
            // toolStripSeparator
            // 
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            this.toolStripSeparator.Name = "toolStripSeparator";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // _richTextBox
            // 
            resources.ApplyResources(this._richTextBox, "_richTextBox");
            this._richTextBox.Name = "_richTextBox";
            // 
            // _openFileDialog
            // 
            resources.ApplyResources(this._openFileDialog, "_openFileDialog");
            // 
            // _saveFileDialog
            // 
            resources.ApplyResources(this._saveFileDialog, "_saveFileDialog");
            // 
            // _newToolStripButton
            // 
            resources.ApplyResources(this._newToolStripButton, "_newToolStripButton");
            this._newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newToolStripButton.Name = "_newToolStripButton";
            this._newToolStripButton.Click += new System.EventHandler(this._newToolStripButton_Click);
            // 
            // _openToolStripButton
            // 
            resources.ApplyResources(this._openToolStripButton, "_openToolStripButton");
            this._openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openToolStripButton.Name = "_openToolStripButton";
            this._openToolStripButton.Click += new System.EventHandler(this._openToolStripButton_Click);
            // 
            // _saveToolStripButton
            // 
            resources.ApplyResources(this._saveToolStripButton, "_saveToolStripButton");
            this._saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveToolStripButton.Name = "_saveToolStripButton";
            this._saveToolStripButton.Click += new System.EventHandler(this._saveToolStripButton_Click);
            // 
            // _printToolStripButton
            // 
            resources.ApplyResources(this._printToolStripButton, "_printToolStripButton");
            this._printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printToolStripButton.Name = "_printToolStripButton";
            // 
            // _cutToolStripButton
            // 
            resources.ApplyResources(this._cutToolStripButton, "_cutToolStripButton");
            this._cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._cutToolStripButton.Name = "_cutToolStripButton";
            this._cutToolStripButton.Click += new System.EventHandler(this._cutToolStripButton_Click);
            // 
            // _copyToolStripButton
            // 
            resources.ApplyResources(this._copyToolStripButton, "_copyToolStripButton");
            this._copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyToolStripButton.Name = "_copyToolStripButton";
            this._copyToolStripButton.Click += new System.EventHandler(this._copyToolStripButton_Click);
            // 
            // _pasteToolStripButton
            // 
            resources.ApplyResources(this._pasteToolStripButton, "_pasteToolStripButton");
            this._pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._pasteToolStripButton.Name = "_pasteToolStripButton";
            this._pasteToolStripButton.Click += new System.EventHandler(this._pasteToolStripButton_Click);
            // 
            // RichTextEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._richTextBox);
            this.Controls.Add(this._toolStrip);
            this.Name = "RichTextEditor";
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _newToolStripButton;
        private System.Windows.Forms.ToolStripButton _openToolStripButton;
        private System.Windows.Forms.ToolStripButton _saveToolStripButton;
        private System.Windows.Forms.ToolStripButton _printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton _cutToolStripButton;
        private System.Windows.Forms.ToolStripButton _copyToolStripButton;
        private System.Windows.Forms.ToolStripButton _pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.RichTextBox _richTextBox;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}