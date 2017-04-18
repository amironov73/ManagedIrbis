namespace AM.Windows.Forms
{
    partial class RetryForm
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
            this._messageLabel = new System.Windows.Forms.Label();
            this._retryLabel = new System.Windows.Forms.Label();
            this._yesButton = new System.Windows.Forms.Button();
            this._noButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _messageLabel
            // 
            this._messageLabel.Location = new System.Drawing.Point(12, 29);
            this._messageLabel.Name = "_messageLabel";
            this._messageLabel.Size = new System.Drawing.Size(714, 91);
            this._messageLabel.TabIndex = 0;
            this._messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _retryLabel
            // 
            this._retryLabel.Location = new System.Drawing.Point(13, 137);
            this._retryLabel.Name = "_retryLabel";
            this._retryLabel.Size = new System.Drawing.Size(712, 31);
            this._retryLabel.TabIndex = 1;
            this._retryLabel.Text = "Retry? Click \"Yes\" or \"No\"";
            this._retryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _yesButton
            // 
            this._yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this._yesButton.Location = new System.Drawing.Point(205, 184);
            this._yesButton.Name = "_yesButton";
            this._yesButton.Size = new System.Drawing.Size(162, 46);
            this._yesButton.TabIndex = 2;
            this._yesButton.Text = "&Yes";
            this._yesButton.UseVisualStyleBackColor = true;
            // 
            // _noButton
            // 
            this._noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this._noButton.Location = new System.Drawing.Point(390, 184);
            this._noButton.Name = "_noButton";
            this._noButton.Size = new System.Drawing.Size(162, 46);
            this._noButton.TabIndex = 3;
            this._noButton.Text = "&No";
            this._noButton.UseVisualStyleBackColor = true;
            // 
            // RetryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 253);
            this.ControlBox = false;
            this.Controls.Add(this._noButton);
            this.Controls.Add(this._yesButton);
            this.Controls.Add(this._retryLabel);
            this.Controls.Add(this._messageLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RetryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error occured. Retry?";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _messageLabel;
        private System.Windows.Forms.Label _retryLabel;
        private System.Windows.Forms.Button _yesButton;
        private System.Windows.Forms.Button _noButton;
    }
}