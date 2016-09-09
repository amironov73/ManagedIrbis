namespace AM.Windows.Forms
{
	partial class PlainTextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlainTextEditor));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._textBox = new System.Windows.Forms.TextBox();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
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
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(481, 27);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _newToolStripButton
            // 
            this._newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_newToolStripButton.Image")));
            this._newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newToolStripButton.Name = "_newToolStripButton";
            this._newToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._newToolStripButton.Text = "&New";
            this._newToolStripButton.Click += new System.EventHandler(this._newToolStripButton_Click);
            // 
            // _openToolStripButton
            // 
            this._openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_openToolStripButton.Image")));
            this._openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openToolStripButton.Name = "_openToolStripButton";
            this._openToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._openToolStripButton.Text = "&Open";
            this._openToolStripButton.Click += new System.EventHandler(this._openToolStripButton_Click);
            // 
            // _saveToolStripButton
            // 
            this._saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveToolStripButton.Image")));
            this._saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveToolStripButton.Name = "_saveToolStripButton";
            this._saveToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._saveToolStripButton.Text = "&Save";
            this._saveToolStripButton.Click += new System.EventHandler(this._saveToolStripButton_Click);
            // 
            // _printToolStripButton
            // 
            this._printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_printToolStripButton.Image")));
            this._printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printToolStripButton.Name = "_printToolStripButton";
            this._printToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._printToolStripButton.Text = "&Print";
            this._printToolStripButton.Click += new System.EventHandler(this._printToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // _cutToolStripButton
            // 
            this._cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_cutToolStripButton.Image")));
            this._cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._cutToolStripButton.Name = "_cutToolStripButton";
            this._cutToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._cutToolStripButton.Text = "C&ut";
            this._cutToolStripButton.Click += new System.EventHandler(this._cutToolStripButton_Click);
            // 
            // _copyToolStripButton
            // 
            this._copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_copyToolStripButton.Image")));
            this._copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._copyToolStripButton.Name = "_copyToolStripButton";
            this._copyToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._copyToolStripButton.Text = "&Copy";
            this._copyToolStripButton.Click += new System.EventHandler(this._copyToolStripButton_Click);
            // 
            // _pasteToolStripButton
            // 
            this._pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_pasteToolStripButton.Image")));
            this._pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._pasteToolStripButton.Name = "_pasteToolStripButton";
            this._pasteToolStripButton.Size = new System.Drawing.Size(24, 24);
            this._pasteToolStripButton.Text = "&Paste";
            this._pasteToolStripButton.Click += new System.EventHandler(this._pasteToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // _textBox
            // 
            this._textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textBox.Location = new System.Drawing.Point(0, 27);
            this._textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(481, 334);
            this._textBox.TabIndex = 1;
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "Text files|*.txt|All files|*.*";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            // 
            // PlainTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._toolStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PlainTextEditor";
            this.Size = new System.Drawing.Size(481, 361);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.ToolStripButton _newToolStripButton;
		private System.Windows.Forms.ToolStripButton _openToolStripButton;
		private System.Windows.Forms.ToolStripButton _saveToolStripButton;
		private System.Windows.Forms.ToolStripButton _printToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton _cutToolStripButton;
		private System.Windows.Forms.ToolStripButton _copyToolStripButton;
		private System.Windows.Forms.ToolStripButton _pasteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}