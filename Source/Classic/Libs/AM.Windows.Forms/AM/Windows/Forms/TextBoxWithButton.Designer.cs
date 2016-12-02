namespace AM.Windows.Forms
{
    partial class TextBoxWithButton
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
            this._textBox = new System.Windows.Forms.TextBox();
            this._button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this._textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textBox.Location = new System.Drawing.Point(0, 0);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(228, 20);
            this._textBox.TabIndex = 0;
            // 
            // _button
            // 
            this._button.Dock = System.Windows.Forms.DockStyle.Right;
            this._button.Location = new System.Drawing.Point(228, 0);
            this._button.Name = "_button";
            this._button.Size = new System.Drawing.Size(23, 21);
            this._button.TabIndex = 1;
            this._button.UseVisualStyleBackColor = true;
            // 
            // TextBoxWithButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._button);
            this.Name = "TextBoxWithButton";
            this.Size = new System.Drawing.Size(251, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBox;
        private System.Windows.Forms.Button _button;
    }
}
