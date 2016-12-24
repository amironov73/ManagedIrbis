namespace AM.Windows.Forms
{
    partial class AttachmentBox
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
            this._listBox = new System.Windows.Forms.ListBox();
            this._saveButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // _listBox
            // 
            this._listBox.FormattingEnabled = true;
            this._listBox.ItemHeight = 16;
            this._listBox.Location = new System.Drawing.Point(12, 12);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(437, 228);
            this._listBox.TabIndex = 0;
            // 
            // _saveButton
            // 
            this._saveButton.Location = new System.Drawing.Point(455, 12);
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(115, 34);
            this._saveButton.TabIndex = 1;
            this._saveButton.Text = "Save file";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(455, 52);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(115, 34);
            this._closeButton.TabIndex = 1;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            // 
            // AttachmentBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 253);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._saveButton);
            this.Controls.Add(this._listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttachmentBox";
            this.ShowInTaskbar = false;
            this.Text = "Attachments";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _listBox;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}