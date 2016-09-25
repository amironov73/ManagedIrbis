namespace AM.Windows.Forms
{
	partial class LabeledComboBox
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
			this._comboBox = new System.Windows.Forms.ComboBox ();
			this.SuspendLayout ();
			// 
			// _label
			// 
			this._label.Dock = System.Windows.Forms.DockStyle.Top;
			this._label.Location = new System.Drawing.Point ( 0, 0 );
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size ( 150, 18 );
			this._label.TabIndex = 0;
			this._label.Text = "_label";
			// 
			// _comboBox
			// 
			this._comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._comboBox.FormattingEnabled = true;
			this._comboBox.Location = new System.Drawing.Point ( 0, 18 );
			this._comboBox.Name = "_comboBox";
			this._comboBox.Size = new System.Drawing.Size ( 150, 21 );
			this._comboBox.TabIndex = 1;
			// 
			// LabeledComboBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add ( this._comboBox );
			this.Controls.Add ( this._label );
			this.Name = "LabeledComboBox";
			this.Size = new System.Drawing.Size ( 150, 44 );
			this.ResumeLayout ( false );

		}

		#endregion

		private System.Windows.Forms.Label _label;
		private System.Windows.Forms.ComboBox _comboBox;
	}
}