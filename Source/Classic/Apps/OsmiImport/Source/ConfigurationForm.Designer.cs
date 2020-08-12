namespace OsmiImport
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
            this._databaseLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._loginLabel = new System.Windows.Forms.Label();
            this._portLabel = new System.Windows.Forms.Label();
            this._hostLabel = new System.Windows.Forms.Label();
            this._databaseBox = new System.Windows.Forms.TextBox();
            this._loginBox = new System.Windows.Forms.TextBox();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._portBox = new System.Windows.Forms.TextBox();
            this._hostBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._templateLabel = new System.Windows.Forms.Label();
            this._templateBox = new System.Windows.Forms.TextBox();
            this._keyLabel = new System.Windows.Forms.Label();
            this._idLabel = new System.Windows.Forms.Label();
            this._urlLabel = new System.Windows.Forms.Label();
            this._keyBox = new System.Windows.Forms.TextBox();
            this._idBox = new System.Windows.Forms.TextBox();
            this._urlBox = new System.Windows.Forms.TextBox();
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
            this._writeButton.Text = "Применить";
            this._writeButton.UseVisualStyleBackColor = true;
            this._writeButton.Click += new System.EventHandler(this._writeButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Location = new System.Drawing.Point(445, 45);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(277, 23);
            this._closeButton.TabIndex = 4;
            this._closeButton.Text = "Закрыть окно конфигуратора";
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
            this._checkButton.Click += new System.EventHandler(this._checkButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._databaseLabel);
            this.groupBox1.Controls.Add(this._passwordLabel);
            this.groupBox1.Controls.Add(this._loginLabel);
            this.groupBox1.Controls.Add(this._portLabel);
            this.groupBox1.Controls.Add(this._hostLabel);
            this.groupBox1.Controls.Add(this._databaseBox);
            this.groupBox1.Controls.Add(this._loginBox);
            this.groupBox1.Controls.Add(this._passwordBox);
            this.groupBox1.Controls.Add(this._portBox);
            this.groupBox1.Controls.Add(this._hostBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ИРБИС";
            // 
            // _databaseLabel
            // 
            this._databaseLabel.AutoSize = true;
            this._databaseLabel.Location = new System.Drawing.Point(13, 123);
            this._databaseLabel.Name = "_databaseLabel";
            this._databaseLabel.Size = new System.Drawing.Size(72, 13);
            this._databaseLabel.TabIndex = 15;
            this._databaseLabel.Text = "База данных";
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(230, 74);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(45, 13);
            this._passwordLabel.TabIndex = 14;
            this._passwordLabel.Text = "Пароль";
            // 
            // _loginLabel
            // 
            this._loginLabel.AutoSize = true;
            this._loginLabel.Location = new System.Drawing.Point(13, 73);
            this._loginLabel.Name = "_loginLabel";
            this._loginLabel.Size = new System.Drawing.Size(78, 13);
            this._loginLabel.TabIndex = 13;
            this._loginLabel.Text = "Логин ИРБИС";
            // 
            // _portLabel
            // 
            this._portLabel.AutoSize = true;
            this._portLabel.Location = new System.Drawing.Point(230, 25);
            this._portLabel.Name = "_portLabel";
            this._portLabel.Size = new System.Drawing.Size(32, 13);
            this._portLabel.TabIndex = 12;
            this._portLabel.Text = "Порт";
            // 
            // _hostLabel
            // 
            this._hostLabel.AutoSize = true;
            this._hostLabel.Location = new System.Drawing.Point(13, 24);
            this._hostLabel.Name = "_hostLabel";
            this._hostLabel.Size = new System.Drawing.Size(135, 13);
            this._hostLabel.TabIndex = 11;
            this._hostLabel.Text = "IP-адрес сервера ИРБИС";
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
            this._passwordBox.PasswordChar = '*';
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
            // _hostBox
            // 
            this._hostBox.Location = new System.Drawing.Point(16, 40);
            this._hostBox.Name = "_hostBox";
            this._hostBox.Size = new System.Drawing.Size(195, 20);
            this._hostBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._templateLabel);
            this.groupBox2.Controls.Add(this._templateBox);
            this.groupBox2.Controls.Add(this._keyLabel);
            this.groupBox2.Controls.Add(this._idLabel);
            this.groupBox2.Controls.Add(this._urlLabel);
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
            // _templateLabel
            // 
            this._templateLabel.AutoSize = true;
            this._templateLabel.Location = new System.Drawing.Point(230, 70);
            this._templateLabel.Name = "_templateLabel";
            this._templateLabel.Size = new System.Drawing.Size(46, 13);
            this._templateLabel.TabIndex = 5;
            this._templateLabel.Text = "Шаблон";
            // 
            // _templateBox
            // 
            this._templateBox.Location = new System.Drawing.Point(233, 86);
            this._templateBox.Name = "_templateBox";
            this._templateBox.Size = new System.Drawing.Size(161, 20);
            this._templateBox.TabIndex = 4;
            this._templateBox.Text = "chb";
            // 
            // _keyLabel
            // 
            this._keyLabel.AutoSize = true;
            this._keyLabel.Location = new System.Drawing.Point(13, 109);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(25, 13);
            this._keyLabel.TabIndex = 3;
            this._keyLabel.Text = "Key";
            // 
            // _idLabel
            // 
            this._idLabel.AutoSize = true;
            this._idLabel.Location = new System.Drawing.Point(16, 70);
            this._idLabel.Name = "_idLabel";
            this._idLabel.Size = new System.Drawing.Size(18, 13);
            this._idLabel.TabIndex = 2;
            this._idLabel.Text = "ID";
            // 
            // _urlLabel
            // 
            this._urlLabel.AutoSize = true;
            this._urlLabel.Location = new System.Drawing.Point(16, 28);
            this._urlLabel.Name = "_urlLabel";
            this._urlLabel.Size = new System.Drawing.Size(29, 13);
            this._urlLabel.TabIndex = 1;
            this._urlLabel.Text = "URL";
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Location = new System.Drawing.Point(13, 125);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(381, 20);
            this._keyBox.TabIndex = 2;
            // 
            // _idBox
            // 
            this._idBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._idBox.Location = new System.Drawing.Point(16, 86);
            this._idBox.Name = "_idBox";
            this._idBox.Size = new System.Drawing.Size(195, 20);
            this._idBox.TabIndex = 1;
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
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
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
        private System.Windows.Forms.Label _databaseLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _loginLabel;
        private System.Windows.Forms.Label _portLabel;
        private System.Windows.Forms.Label _hostLabel;
        private System.Windows.Forms.TextBox _databaseBox;
        private System.Windows.Forms.TextBox _loginBox;
        private System.Windows.Forms.TextBox _passwordBox;
        private System.Windows.Forms.TextBox _portBox;
        private System.Windows.Forms.TextBox _hostBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.Label _idLabel;
        private System.Windows.Forms.Label _urlLabel;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.TextBox _idBox;
        private System.Windows.Forms.TextBox _urlBox;
        private AM.Windows.Forms.LogBox logBox1;
        private System.Windows.Forms.Label _templateLabel;
        private System.Windows.Forms.TextBox _templateBox;
    }
}