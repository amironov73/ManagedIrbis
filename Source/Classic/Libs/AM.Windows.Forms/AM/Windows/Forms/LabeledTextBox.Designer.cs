// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace AM.Windows.Forms
{
	partial class LabeledTextBox
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
			this._label = new System.Windows.Forms.Label ();
			this._textBox = new System.Windows.Forms.TextBox ();
			this.SuspendLayout ();
			// 
			// _label
			// 
			this._label.Dock = System.Windows.Forms.DockStyle.Top;
			this._label.Location = new System.Drawing.Point ( 0, 0 );
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size ( 150, 21 );
			this._label.TabIndex = 0;
			this._label.Text = "_label";
			// 
			// _textBox
			// 
			this._textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._textBox.Location = new System.Drawing.Point ( 0, 21 );
			this._textBox.Name = "_textBox";
			this._textBox.Size = new System.Drawing.Size ( 150, 20 );
			this._textBox.TabIndex = 1;
			// 
			// LabeledTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add ( this._textBox );
			this.Controls.Add ( this._label );
			this.Name = "LabeledTextBox";
			this.Size = new System.Drawing.Size ( 150, 45 );
			this.ResumeLayout ( false );
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.Label _label;
		private System.Windows.Forms.TextBox _textBox;
	}
}