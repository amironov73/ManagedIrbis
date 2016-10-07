using AM.Drawing.Printing;

namespace AM.Windows.Forms
{
	partial class PictureViewForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PictureViewForm));
            this._toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this._printDocument = new System.Drawing.Printing.PrintDocument();
            this._picturePrinter = new AM.Drawing.Printing.PicturePrinter();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._openButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this._copyButton = new System.Windows.Forms.ToolStripButton();
            this._pasteButton = new System.Windows.Forms.ToolStripButton();
            this._printSetupButton = new System.Windows.Forms.ToolStripButton();
            this._printPreviewButton = new System.Windows.Forms.ToolStripButton();
            this._printButton = new System.Windows.Forms.ToolStripButton();
            this._zoomInButton = new System.Windows.Forms.ToolStripButton();
            this._zoomOutButton = new System.Windows.Forms.ToolStripButton();
            this._resetZoomButton = new System.Windows.Forms.ToolStripButton();
            this._toolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolBar
            // 
            resources.ApplyResources(this._toolBar, "_toolBar");
            this._toolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openButton,
            this._saveButton,
            this.toolStripSeparator1,
            this._copyButton,
            this._pasteButton,
            this.toolStripSeparator3,
            this._printSetupButton,
            this._printPreviewButton,
            this._printButton,
            this.toolStripSeparator2,
            this._zoomInButton,
            this._zoomOutButton,
            this._resetZoomButton});
            this._toolBar.Name = "_toolBar";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // _openFileDialog
            // 
            resources.ApplyResources(this._openFileDialog, "_openFileDialog");
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "bmp";
            resources.ApplyResources(this._saveFileDialog, "_saveFileDialog");
            // 
            // _pageSetupDialog
            // 
            this._pageSetupDialog.Document = this._printDocument;
            // 
            // _picturePrinter
            // 
            this._picturePrinter.Document = this._printDocument;
            this._picturePrinter.Image = null;
            this._picturePrinter.ImagePosition = AM.Drawing.Printing.ImagePosition.PageCenter;
            this._picturePrinter.Title = null;
            // 
            // _pictureBox
            // 
            resources.ApplyResources(this._pictureBox, "_pictureBox");
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.TabStop = false;
            // 
            // _openButton
            // 
            resources.ApplyResources(this._openButton, "_openButton");
            this._openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openButton.Name = "_openButton";
            this._openButton.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _saveButton
            // 
            resources.ApplyResources(this._saveButton, "_saveButton");
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _copyButton
            // 
            resources.ApplyResources(this._copyButton, "_copyButton");
            this._copyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyButton.Name = "_copyButton";
            this._copyButton.Click += new System.EventHandler(this._copyButton_Click);
            // 
            // _pasteButton
            // 
            resources.ApplyResources(this._pasteButton, "_pasteButton");
            this._pasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._pasteButton.Name = "_pasteButton";
            this._pasteButton.Click += new System.EventHandler(this._pasteButton_Click);
            // 
            // _printSetupButton
            // 
            resources.ApplyResources(this._printSetupButton, "_printSetupButton");
            this._printSetupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printSetupButton.Name = "_printSetupButton";
            this._printSetupButton.Click += new System.EventHandler(this._printSetupButton_Click);
            // 
            // _printPreviewButton
            // 
            resources.ApplyResources(this._printPreviewButton, "_printPreviewButton");
            this._printPreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printPreviewButton.Name = "_printPreviewButton";
            this._printPreviewButton.Click += new System.EventHandler(this._printPreviewButton_Click);
            // 
            // _printButton
            // 
            resources.ApplyResources(this._printButton, "_printButton");
            this._printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printButton.Name = "_printButton";
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _zoomInButton
            // 
            resources.ApplyResources(this._zoomInButton, "_zoomInButton");
            this._zoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._zoomInButton.Name = "_zoomInButton";
            // 
            // _zoomOutButton
            // 
            resources.ApplyResources(this._zoomOutButton, "_zoomOutButton");
            this._zoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._zoomOutButton.Name = "_zoomOutButton";
            // 
            // _resetZoomButton
            // 
            resources.ApplyResources(this._resetZoomButton, "_resetZoomButton");
            this._resetZoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._resetZoomButton.Name = "_resetZoomButton";
            // 
            // PictureViewForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this._toolBar);
            this.Name = "PictureViewForm";
            this._toolBar.ResumeLayout(false);
            this._toolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolBar;
		private System.Windows.Forms.ToolStripButton _openButton;
		private System.Windows.Forms.ToolStripButton _saveButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton _printButton;
		private System.Windows.Forms.ToolStripButton _zoomInButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton _zoomOutButton;
		private System.Windows.Forms.ToolStripButton _resetZoomButton;
		private System.Windows.Forms.ToolStripButton _printSetupButton;
		private System.Windows.Forms.ToolStripButton _copyButton;
		private System.Windows.Forms.ToolStripButton _pasteButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton _printPreviewButton;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.PageSetupDialog _pageSetupDialog;
		private System.Windows.Forms.PictureBox _pictureBox;
		private System.Drawing.Printing.PrintDocument _printDocument;
		private global::AM.Drawing.Printing.PicturePrinter _picturePrinter;

	}
}