namespace IrbisUI
{
    partial class IrbisLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IrbisLoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this._nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Имя пользователя:";
            // 
            // _nameBox
            // 
            this._nameBox.Location = new System.Drawing.Point(185, 31);
            this._nameBox.Name = "_nameBox";
            this._nameBox.Size = new System.Drawing.Size(215, 20);
            this._nameBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Пароль:";
            // 
            // _passwordBox
            // 
            this._passwordBox.Location = new System.Drawing.Point(185, 70);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.Size = new System.Drawing.Size(215, 20);
            this._passwordBox.TabIndex = 2;
            this._passwordBox.UseSystemPasswordChar = true;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(185, 114);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(97, 23);
            this._okButton.TabIndex = 3;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(300, 114);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 23);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "О&тмена";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // IrbisLoginForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(412, 149);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._passwordBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._nameBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IrbisLoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подключение к серверу";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _passwordBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;

    }
}