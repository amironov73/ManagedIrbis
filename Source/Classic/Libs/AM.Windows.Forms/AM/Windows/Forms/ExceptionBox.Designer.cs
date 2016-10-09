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
            resources.ApplyResources(this._textBox, "_textBox");
            this._textBox.BackColor = System.Drawing.SystemColors.Window;
            this._textBox.Name = "_textBox";
            this._textBox.ReadOnly = true;
            // 
            // _printButton
            // 
            resources.ApplyResources(this._printButton, "_printButton");
            this._printButton.Name = "_printButton";
            this._printButton.UseVisualStyleBackColor = true;
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _saveButton
            // 
            resources.ApplyResources(this._saveButton, "_saveButton");
            this._saveButton.Name = "_saveButton";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _closeButton
            // 
            resources.ApplyResources(this._closeButton, "_closeButton");
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Name = "_closeButton";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _copyButton
            // 
            resources.ApplyResources(this._copyButton, "_copyButton");
            this._copyButton.Name = "_copyButton";
            this._copyButton.UseVisualStyleBackColor = true;
            this._copyButton.Click += new System.EventHandler(this._copyButton_Click);
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.FileName = "exception.txt";
            resources.ApplyResources(this._saveFileDialog, "_saveFileDialog");
            // 
            // _abortButton
            // 
            resources.ApplyResources(this._abortButton, "_abortButton");
            this._abortButton.Name = "_abortButton";
            this._abortButton.UseVisualStyleBackColor = true;
            this._abortButton.Click += new System.EventHandler(this._abortButton_Click);
            // 
            // _typeLabel
            // 
            resources.ApplyResources(this._typeLabel, "_typeLabel");
            this._typeLabel.Name = "_typeLabel";
            // 
            // _messageLabel
            // 
            resources.ApplyResources(this._messageLabel, "_messageLabel");
            this._messageLabel.Name = "_messageLabel";
            // 
            // ExceptionBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._closeButton;
            this.Controls.Add(this._messageLabel);
            this.Controls.Add(this._typeLabel);
            this.Controls.Add(this._abortButton);
            this.Controls.Add(this._copyButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._saveButton);
            this.Controls.Add(this._printButton);
            this.Controls.Add(this._textBox);
            this.Name = "ExceptionBox";
            this.ShowIcon = false;
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