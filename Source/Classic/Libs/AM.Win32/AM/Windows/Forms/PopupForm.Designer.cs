namespace AM.Windows.Forms
{
	partial class PopupForm
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
			this.components = new System.ComponentModel.Container ();
			this._titlePanel = new System.Windows.Forms.Panel ();
			this._titleLabel = new System.Windows.Forms.Label ();
			this._appearanceTimer = new System.Windows.Forms.Timer ( this.components );
			this._timeoutTimer = new System.Windows.Forms.Timer ( this.components );
			this._browser = new System.Windows.Forms.WebBrowser ();
			this._titlePanel.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// _titlePanel
			// 
			this._titlePanel.BackColor = System.Drawing.Color.FromArgb ( ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ) );
			this._titlePanel.Controls.Add ( this._titleLabel );
			this._titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this._titlePanel.Location = new System.Drawing.Point ( 0, 0 );
			this._titlePanel.Name = "_titlePanel";
			this._titlePanel.Size = new System.Drawing.Size ( 375, 26 );
			this._titlePanel.TabIndex = 0;
			// 
			// _titleLabel
			// 
			this._titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._titleLabel.Font = new System.Drawing.Font ( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte) ( 204 ) ) );
			this._titleLabel.Location = new System.Drawing.Point ( 0, 0 );
			this._titleLabel.Name = "_titleLabel";
			this._titleLabel.Size = new System.Drawing.Size ( 375, 26 );
			this._titleLabel.TabIndex = 0;
			this._titleLabel.Text = "Form title";
			this._titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _appearanceTimer
			// 
			this._appearanceTimer.Enabled = true;
			this._appearanceTimer.Tick += new System.EventHandler ( this._appearanceTimer_Tick );
			// 
			// _timeoutTimer
			// 
			this._timeoutTimer.Tick += new System.EventHandler ( this._timeoutTimer_Tick );
			// 
			// _browser
			// 
			this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._browser.Location = new System.Drawing.Point ( 0, 26 );
			this._browser.MinimumSize = new System.Drawing.Size ( 20, 20 );
			this._browser.Name = "_browser";
			this._browser.ScrollBarsEnabled = false;
			this._browser.Size = new System.Drawing.Size ( 375, 70 );
			this._browser.TabIndex = 1;
			this._browser.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler ( this._browser_PreviewKeyDown );
			// 
			// PopupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size ( 375, 96 );
			this.ControlBox = false;
			this.Controls.Add ( this._browser );
			this.Controls.Add ( this._titlePanel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.Name = "PopupForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Form title here";
			this._titlePanel.ResumeLayout ( false );
			this.ResumeLayout ( false );

		}

		#endregion

		private System.Windows.Forms.Panel _titlePanel;
		private System.Windows.Forms.Label _titleLabel;
		private System.Windows.Forms.Timer _appearanceTimer;
		private System.Windows.Forms.Timer _timeoutTimer;
		private System.Windows.Forms.WebBrowser _browser;
	}
}