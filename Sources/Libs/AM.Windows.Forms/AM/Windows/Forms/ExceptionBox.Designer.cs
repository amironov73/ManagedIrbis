namespace AM.Windows.Forms
{
    partial class ExceptionBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionBox));
            this._textBox = new System.Windows.Forms.TextBox();
            this._printButton = new System.Windows.Forms.Button();
            this._saveButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._copyButton = new System.Windows.Forms.Button();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._abortButton = new System.Windows.Forms.Button();
            this._typeLabel = new System.Windows.Forms.Label();
            this._messageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this._textBox.BackColor = System.Drawing.SystemColors.Window;
            this._textBox.Location = new System.Drawing.Point(0, 83);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.ReadOnly = true;
            this._textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox.Size = new System.Drawing.Size(784, 213);
            this._textBox.TabIndex = 2;
            // 
            // _printButton
            // 
            this._printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._printButton.Location = new System.Drawing.Point(160, 302);
            this._printButton.Name = "_printButton";
            this._printButton.Size = new System.Drawing.Size(147, 39);
            this._printButton.TabIndex = 4;
            this._printButton.Text = "Print text";
            this._printButton.UseVisualStyleBackColor = true;
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _saveButton
            // 
            this._saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._saveButton.Location = new System.Drawing.Point(316, 302);
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(147, 39);
            this._saveButton.TabIndex = 5;
            this._saveButton.Text = "Save to file";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Location = new System.Drawing.Point(622, 302);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(147, 39);
            this._closeButton.TabIndex = 7;
            this._closeButton.Text = "Close the window";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _copyButton
            // 
            this._copyButton.Location = new System.Drawing.Point(7, 302);
            this._copyButton.Name = "_copyButton";
            this._copyButton.Size = new System.Drawing.Size(147, 39);
            this._copyButton.TabIndex = 3;
            this._copyButton.Text = "Copy to clipboard";
            this._copyButton.UseVisualStyleBackColor = true;
            this._copyButton.Click += new System.EventHandler(this._copyButton_Click);
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.FileName = "exception.txt";
            this._saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            // 
            // _abortButton
            // 
            this._abortButton.Location = new System.Drawing.Point(469, 302);
            this._abortButton.Name = "_abortButton";
            this._abortButton.Size = new System.Drawing.Size(147, 39);
            this._abortButton.TabIndex = 6;
            this._abortButton.Text = "Abort the program";
            this._abortButton.UseVisualStyleBackColor = true;
            this._abortButton.Click += new System.EventHandler(this._abortButton_Click);
            // 
            // _typeLabel
            // 
            this._typeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._typeLabel.Location = new System.Drawing.Point(12, 9);
            this._typeLabel.Name = "_typeLabel";
            this._typeLabel.Size = new System.Drawing.Size(757, 23);
            this._typeLabel.TabIndex = 0;
            this._typeLabel.Text = "Exception type";
            // 
            // _messageLabel
            // 
            this._messageLabel.Location = new System.Drawing.Point(12, 32);
            this._messageLabel.Name = "_messageLabel";
            this._messageLabel.Size = new System.Drawing.Size(758, 37);
            this._messageLabel.TabIndex = 1;
            this._messageLabel.Text = "Exception message";
            // 
            // ExceptionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size(782, 353);
            this.Controls.Add(this._messageLabel);
            this.Controls.Add(this._typeLabel);
            this.Controls.Add(this._abortButton);
            this.Controls.Add(this._copyButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._saveButton);
            this.Controls.Add(this._printButton);
            this.Controls.Add(this._textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExceptionBox";
            this.ShowIcon = false;
            this.Text = "Exception occurred";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBox;
        private System.Windows.Forms.Button _printButton;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _copyButton;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.Button _abortButton;
        private System.Windows.Forms.Label _typeLabel;
        private System.Windows.Forms.Label _messageLabel;
    }
}