namespace LitresTicket
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._ticketBox = new System.Windows.Forms.TextBox();
            this._searchButton = new System.Windows.Forms.Button();
            this._resultBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._loginBox = new System.Windows.Forms.TextBox();
            this._bindButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._startupTimer = new System.Windows.Forms.Timer(this.components);
            this._table.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this.label1, 0, 0);
            this._table.Controls.Add(this._ticketBox, 1, 0);
            this._table.Controls.Add(this._searchButton, 0, 1);
            this._table.Controls.Add(this._resultBox, 0, 2);
            this._table.Controls.Add(this.label2, 0, 3);
            this._table.Controls.Add(this._loginBox, 1, 3);
            this._table.Controls.Add(this._bindButton, 0, 5);
            this._table.Controls.Add(this.label3, 0, 4);
            this._table.Controls.Add(this._passwordBox, 1, 4);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 6;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(384, 261);
            this._table.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Билет ИОГУНБ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ticketBox
            // 
            this._ticketBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._ticketBox.Location = new System.Drawing.Point(101, 3);
            this._ticketBox.Name = "_ticketBox";
            this._ticketBox.Size = new System.Drawing.Size(280, 20);
            this._ticketBox.TabIndex = 1;
            this._ticketBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._ticketBox_KeyDown);
            // 
            // _searchButton
            // 
            this._table.SetColumnSpan(this._searchButton, 2);
            this._searchButton.Dock = System.Windows.Forms.DockStyle.Top;
            this._searchButton.Location = new System.Drawing.Point(3, 29);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(378, 23);
            this._searchButton.TabIndex = 2;
            this._searchButton.Text = "Найти читателя";
            this._searchButton.UseVisualStyleBackColor = true;
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // _resultBox
            // 
            this._table.SetColumnSpan(this._resultBox, 2);
            this._resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._resultBox.Location = new System.Drawing.Point(3, 58);
            this._resultBox.Multiline = true;
            this._resultBox.Name = "_resultBox";
            this._resultBox.ReadOnly = true;
            this._resultBox.Size = new System.Drawing.Size(378, 119);
            this._resultBox.TabIndex = 1;
            this._resultBox.TabStop = false;
            this._resultBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Логин ЛИТРЕС";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _loginBox
            // 
            this._loginBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._loginBox.Location = new System.Drawing.Point(101, 183);
            this._loginBox.Name = "_loginBox";
            this._loginBox.Size = new System.Drawing.Size(280, 20);
            this._loginBox.TabIndex = 4;
            // 
            // _bindButton
            // 
            this._table.SetColumnSpan(this._bindButton, 2);
            this._bindButton.Dock = System.Windows.Forms.DockStyle.Top;
            this._bindButton.Location = new System.Drawing.Point(3, 235);
            this._bindButton.Name = "_bindButton";
            this._bindButton.Size = new System.Drawing.Size(378, 23);
            this._bindButton.TabIndex = 7;
            this._bindButton.Text = "Привязать к ЛИТРЕС";
            this._bindButton.UseVisualStyleBackColor = true;
            this._bindButton.Click += new System.EventHandler(this._bindButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Пароль ЛИТРЕС";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _passwordBox
            // 
            this._passwordBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._passwordBox.Location = new System.Drawing.Point(101, 209);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.Size = new System.Drawing.Size(280, 20);
            this._passwordBox.TabIndex = 6;
            // 
            // _startupTimer
            // 
            this._startupTimer.Enabled = true;
            this._startupTimer.Tick += new System.EventHandler(this._startupTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this._table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.Text = "Привязка читателей к ЛИТРЕС";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _ticketBox;
        private System.Windows.Forms.Button _searchButton;
        private System.Windows.Forms.TextBox _resultBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _loginBox;
        private System.Windows.Forms.Button _bindButton;
        private System.Windows.Forms.Timer _startupTimer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _passwordBox;
    }
}

