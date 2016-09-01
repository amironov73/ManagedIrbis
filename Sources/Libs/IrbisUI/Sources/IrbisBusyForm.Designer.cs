namespace IrbisUI
{
    partial class IrbisBusyForm
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
            this._breakButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _messageLabel
            // 
            this._messageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._messageLabel.Location = new System.Drawing.Point(0, 0);
            this._messageLabel.Name = "_messageLabel";
            this._messageLabel.Size = new System.Drawing.Size(280, 68);
            this._messageLabel.TabIndex = 0;
            this._messageLabel.Text = "Происходит обработка данных на сервере. Подождите, пожалуйста";
            this._messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _breakButton
            // 
            this._breakButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._breakButton.Location = new System.Drawing.Point(0, 68);
            this._breakButton.Name = "_breakButton";
            this._breakButton.Size = new System.Drawing.Size(280, 23);
            this._breakButton.TabIndex = 1;
            this._breakButton.Text = "Прервать";
            this._breakButton.UseVisualStyleBackColor = true;
            this._breakButton.Click += new System.EventHandler(this._breakButton_Click);
            // 
            // IrbisBusyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 91);
            this.ControlBox = false;
            this.Controls.Add(this._messageLabel);
            this.Controls.Add(this._breakButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "IrbisBusyForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ИРБИС64";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _messageLabel;
        private System.Windows.Forms.Button _breakButton;
    }
}