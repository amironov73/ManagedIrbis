namespace AM.Deployment
{
	partial class UpdateProgressForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( UpdateProgressForm ) );
			this.pictureBox1 = new System.Windows.Forms.PictureBox ();
			this._sourceLabel = new System.Windows.Forms.Label ();
			this._fileNameLabel = new System.Windows.Forms.Label ();
			this._backgroundWorker = new System.ComponentModel.BackgroundWorker ();
			( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).BeginInit ();
			this.SuspendLayout ();
			// 
			// pictureBox1
			// 
			resources.ApplyResources ( this.pictureBox1, "pictureBox1" );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// _sourceLabel
			// 
			resources.ApplyResources ( this._sourceLabel, "_sourceLabel" );
			this._sourceLabel.Name = "_sourceLabel";
			// 
			// _fileNameLabel
			// 
			resources.ApplyResources ( this._fileNameLabel, "_fileNameLabel" );
			this._fileNameLabel.Name = "_fileNameLabel";
			// 
			// _backgroundWorker
			// 
			this._backgroundWorker.WorkerReportsProgress = true;
			this._backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler ( this._backgroundWorker_DoWork );
			this._backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler ( this._backgroundWorker_RunWorkerCompleted );
			this._backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler ( this._backgroundWorker_ProgressChanged );
			// 
			// UpdateProgressForm
			// 
			resources.ApplyResources ( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add ( this._fileNameLabel );
			this.Controls.Add ( this._sourceLabel );
			this.Controls.Add ( this.pictureBox1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "UpdateProgressForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler ( this.UpdateProgressForm_FormClosing );
			this.Load += new System.EventHandler ( this._Form_Load );
			( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).EndInit ();
			this.ResumeLayout ( false );

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label _sourceLabel;
		private System.Windows.Forms.Label _fileNameLabel;
		private System.ComponentModel.BackgroundWorker _backgroundWorker;
	}
}