namespace OsmiRegistration
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._logBox = new AM.Windows.Forms.LogBox();
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._ticketBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._searchButton = new System.Windows.Forms.Button();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._createButton = new System.Windows.Forms.Button();
            this._sendEmailButton = new System.Windows.Forms.Button();
            this._deleteButton = new System.Windows.Forms.Button();
            this._clearButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._emailBox = new System.Windows.Forms.TextBox();
            this._configButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 236);
            this._logBox.Margin = new System.Windows.Forms.Padding(2);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(584, 206);
            this._logBox.TabIndex = 9;
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._busyStripe.Location = new System.Drawing.Point(0, 223);
            this._busyStripe.Margin = new System.Windows.Forms.Padding(2);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(584, 13);
            this._busyStripe.TabIndex = 1;
            // 
            // _ticketBox
            // 
            this._ticketBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ticketBox.Location = new System.Drawing.Point(12, 37);
            this._ticketBox.Name = "_ticketBox";
            this._ticketBox.Size = new System.Drawing.Size(372, 20);
            this._ticketBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Номер читательского билета";
            // 
            // _searchButton
            // 
            this._searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._searchButton.Location = new System.Drawing.Point(390, 34);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(182, 23);
            this._searchButton.TabIndex = 1;
            this._searchButton.Text = "Найти читателя по билету";
            this._searchButton.UseVisualStyleBackColor = true;
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // _browser
            // 
            this._browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._browser.Location = new System.Drawing.Point(12, 105);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(372, 113);
            this._browser.TabIndex = 8;
            // 
            // _createButton
            // 
            this._createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._createButton.Location = new System.Drawing.Point(390, 63);
            this._createButton.Name = "_createButton";
            this._createButton.Size = new System.Drawing.Size(182, 23);
            this._createButton.TabIndex = 2;
            this._createButton.Text = "Создать карту";
            this._createButton.UseVisualStyleBackColor = true;
            this._createButton.Click += new System.EventHandler(this._createButton_Click);
            // 
            // _sendEmailButton
            // 
            this._sendEmailButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._sendEmailButton.Location = new System.Drawing.Point(390, 92);
            this._sendEmailButton.Name = "_sendEmailButton";
            this._sendEmailButton.Size = new System.Drawing.Size(182, 23);
            this._sendEmailButton.TabIndex = 4;
            this._sendEmailButton.Text = "Послать email";
            this._sendEmailButton.UseVisualStyleBackColor = true;
            this._sendEmailButton.Click += new System.EventHandler(this._sendEmailButton_Click);
            // 
            // _deleteButton
            // 
            this._deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteButton.Enabled = false;
            this._deleteButton.Location = new System.Drawing.Point(390, 121);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(182, 23);
            this._deleteButton.TabIndex = 5;
            this._deleteButton.Text = "Удалить карту";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // _clearButton
            // 
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(390, 150);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(182, 23);
            this._clearButton.TabIndex = 6;
            this._clearButton.Text = "Очистить поля";
            this._clearButton.UseVisualStyleBackColor = true;
            this._clearButton.Click += new System.EventHandler(this._clearButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "E-mail";
            // 
            // _emailBox
            // 
            this._emailBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._emailBox.Location = new System.Drawing.Point(12, 79);
            this._emailBox.Name = "_emailBox";
            this._emailBox.Size = new System.Drawing.Size(372, 20);
            this._emailBox.TabIndex = 3;
            // 
            // _configButton
            // 
            this._configButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._configButton.Location = new System.Drawing.Point(390, 179);
            this._configButton.Name = "_configButton";
            this._configButton.Size = new System.Drawing.Size(182, 23);
            this._configButton.TabIndex = 7;
            this._configButton.Text = "Настройки...";
            this._configButton.UseVisualStyleBackColor = true;
            this._configButton.Click += new System.EventHandler(this._configButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 442);
            this.Controls.Add(this._configButton);
            this.Controls.Add(this._emailBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._deleteButton);
            this.Controls.Add(this._sendEmailButton);
            this.Controls.Add(this._createButton);
            this.Controls.Add(this._browser);
            this.Controls.Add(this._searchButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._ticketBox);
            this.Controls.Add(this._busyStripe);
            this.Controls.Add(this._logBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "MainForm";
            this.Text = "Работа с картами OSMI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AM.Windows.Forms.LogBox _logBox;
        private AM.Windows.Forms.BusyStripe _busyStripe;
        private System.Windows.Forms.TextBox _ticketBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _searchButton;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.Button _createButton;
        private System.Windows.Forms.Button _sendEmailButton;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _emailBox;
        private System.Windows.Forms.Button _configButton;
    }
}

