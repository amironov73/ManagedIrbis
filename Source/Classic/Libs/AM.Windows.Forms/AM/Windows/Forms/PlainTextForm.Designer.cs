namespace AM.Windows.Forms
{
    partial class PlainTextForm
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
            this._textControl = new AM.Windows.Forms.PlainTextEditor();
            this.SuspendLayout();
            // 
            // _textControl
            // 
            this._textControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textControl.Location = new System.Drawing.Point(0, 0);
            this._textControl.Name = "_textControl";
            this._textControl.Size = new System.Drawing.Size(784, 561);
            this._textControl.TabIndex = 0;
            // 
            // 
            // 
            this._textControl.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textControl.TextBox.Location = new System.Drawing.Point(0, 27);
            this._textControl.TextBox.Multiline = true;
            this._textControl.TextBox.Name = "_textBox";
            this._textControl.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textControl.TextBox.Size = new System.Drawing.Size(784, 534);
            this._textControl.TextBox.TabIndex = 1;
            // 
            // PlainTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._textControl);
            this.Name = "PlainTextForm";
            this.Text = "Text editor";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.PlainTextEditor _textControl;
    }
}