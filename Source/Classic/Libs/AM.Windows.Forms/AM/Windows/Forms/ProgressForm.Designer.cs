// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace AM.Windows.Forms
{
	partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._label = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _progressBar
            // 
            resources.ApplyResources(this._progressBar, "_progressBar");
            this._progressBar.Name = "_progressBar";
            // 
            // _label
            // 
            resources.ApplyResources(this._label, "_label");
            this._label.Name = "_label";
            // 
            // _cancelButton
            // 
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // ProgressForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._label);
            this.Controls.Add(this._progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _label;
        private System.Windows.Forms.Button _cancelButton;
    }
}