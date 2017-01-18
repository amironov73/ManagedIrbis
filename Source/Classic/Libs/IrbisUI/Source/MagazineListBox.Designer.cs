namespace IrbisUI
{
    partial class MagazineListBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._listBox = new System.Windows.Forms.ListBox();
            this._textBox = new AM.Windows.Forms.EventedTextBox();
            this.SuspendLayout();
            // 
            // _listBox
            // 
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(0, 0);
            this._listBox.Name = "_listBox";
            this._listBox.ScrollAlwaysVisible = true;
            this._listBox.Size = new System.Drawing.Size(301, 467);
            this._listBox.TabIndex = 0;
            // 
            // _textBox
            // 
            this._textBox.Delay = 750;
            this._textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._textBox.Location = new System.Drawing.Point(0, 467);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(301, 20);
            this._textBox.TabIndex = 1;
            // 
            // MagazineListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._listBox);
            this.Controls.Add(this._textBox);
            this.Name = "MagazineListBox";
            this.Size = new System.Drawing.Size(301, 487);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _listBox;
        private AM.Windows.Forms.EventedTextBox _textBox;
    }
}
