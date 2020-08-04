namespace OsmiRegistration
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this._writeButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._checkButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._databaseBox = new System.Windows.Forms.TextBox();
            this._loginBox = new System.Windows.Forms.TextBox();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._portBox = new System.Windows.Forms.TextBox();
            this._irbisBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._urlBox = new System.Windows.Forms.TextBox();
            this._idBox = new System.Windows.Forms.TextBox();
            this._keyBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.logBox1 = new AM.Windows.Forms.LogBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _writeButton
            // 
            this._writeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._writeButton.Location = new System.Drawing.Point(577, 16);
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new System.Drawing.Size(145, 23);
            this._writeButton.TabIndex = 3;
            this._writeButton.Text = "Записать";
            this._writeButton.UseVisualStyleBackColor = true;
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Location = new System.Drawing.Point(445, 45);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(277, 23);
            this._closeButton.TabIndex = 4;
            this._closeButton.Text = "Отменить";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _checkButton
            // 
            this._checkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkButton.Location = new System.Drawing.Point(445, 16);
            this._checkButton.Name = "_checkButton";
            this._checkButton.Size = new System.Drawing.Size(127, 23);
            this._checkButton.TabIndex = 2;
            this._checkButton.Text = "Проверить";
            this._checkButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._databaseBox);
            this.groupBox1.Controls.Add(this._loginBox);
            this.groupBox1.Controls.Add(this._passwordBox);
            this.groupBox1.Controls.Add(this._portBox);
            this.groupBox1.Controls.Add(this._irbisBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ИРБИС";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "База данных";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(230, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Логин ИРБИС";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Порт";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP-адрес сервера ИРБИС";
            // 
            // _databaseBox
            // 
            this._databaseBox.Location = new System.Drawing.Point(16, 139);
            this._databaseBox.Name = "_databaseBox";
            this._databaseBox.Size = new System.Drawing.Size(195, 20);
            this._databaseBox.TabIndex = 4;
            this._databaseBox.Text = "RDR";
            // 
            // _loginBox
            // 
            this._loginBox.Location = new System.Drawing.Point(16, 89);
            this._loginBox.Name = "_loginBox";
            this._loginBox.Size = new System.Drawing.Size(195, 20);
            this._loginBox.TabIndex = 2;
            // 
            // _passwordBox
            // 
            this._passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._passwordBox.Location = new System.Drawing.Point(233, 90);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.Size = new System.Drawing.Size(161, 20);
            this._passwordBox.TabIndex = 3;
            // 
            // _portBox
            // 
            this._portBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._portBox.Location = new System.Drawing.Point(233, 40);
            this._portBox.Name = "_portBox";
            this._portBox.Size = new System.Drawing.Size(161, 20);
            this._portBox.TabIndex = 1;
            this._portBox.Text = "6666";
            // 
            // _irbisBox
            // 
            this._irbisBox.Location = new System.Drawing.Point(16, 40);
            this._irbisBox.Name = "_irbisBox";
            this._irbisBox.Size = new System.Drawing.Size(195, 20);
            this._irbisBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this._keyBox);
            this.groupBox2.Controls.Add(this._idBox);
            this.groupBox2.Controls.Add(this._urlBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 200);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 167);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "API";
            // 
            // _urlBox
            // 
            this._urlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._urlBox.Location = new System.Drawing.Point(16, 44);
            this._urlBox.Name = "_urlBox";
            this._urlBox.Size = new System.Drawing.Size(378, 20);
            this._urlBox.TabIndex = 0;
            // 
            // _idBox
            // 
            this._idBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._idBox.Location = new System.Drawing.Point(16, 86);
            this._idBox.Name = "_idBox";
            this._idBox.Size = new System.Drawing.Size(378, 20);
            this._idBox.TabIndex = 1;
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Location = new System.Drawing.Point(13, 125);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(378, 20);
            this._keyBox.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "URL";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "ID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Key";
            // 
            // logBox1
            // 
            this.logBox1.BackColor = System.Drawing.SystemColors.Window;
            this.logBox1.Location = new System.Drawing.Point(445, 87);
            this.logBox1.Multiline = true;
            this.logBox1.Name = "logBox1";
            this.logBox1.ReadOnly = true;
            this.logBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox1.Size = new System.Drawing.Size(277, 280);
            this.logBox1.TabIndex = 5;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size(734, 379);
            this.ControlBox = false;
            this.Controls.Add(this.logBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._checkButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._writeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка приложения";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _checkButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _databaseBox;
        private System.Windows.Forms.TextBox _loginBox;
        private System.Windows.Forms.TextBox _passwordBox;
        private System.Windows.Forms.TextBox _portBox;
        private System.Windows.Forms.TextBox _irbisBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.TextBox _idBox;
        private System.Windows.Forms.TextBox _urlBox;
        private AM.Windows.Forms.LogBox logBox1;
    }
}