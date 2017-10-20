namespace PartyStatus
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._numberBox = new System.Windows.Forms.TextBox();
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._logBox = new AM.Windows.Forms.LogBox();
            this.label2 = new System.Windows.Forms.Label();
            this._statusBox = new System.Windows.Forms.ComboBox();
            this._goButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._numberBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._busyStripe, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._logBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this._statusBox, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this._goButton, 5, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер партии (номер КСУ)";
            // 
            // _numberBox
            // 
            this._numberBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._numberBox.Location = new System.Drawing.Point(23, 36);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(290, 20);
            this._numberBox.TabIndex = 1;
            // 
            // _busyStripe
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._busyStripe, 7);
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Fill;
            this._busyStripe.ForeColor = System.Drawing.Color.Lime;
            this._busyStripe.Location = new System.Drawing.Point(3, 65);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(578, 14);
            this._busyStripe.TabIndex = 3;
            this._busyStripe.Text = "Выполнение операции на сервере";
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this._logBox, 5);
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._logBox.Location = new System.Drawing.Point(23, 85);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(538, 253);
            this._logBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Статус";
            // 
            // _statusBox
            // 
            this._statusBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._statusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._statusBox.FormattingEnabled = true;
            this._statusBox.Location = new System.Drawing.Point(339, 36);
            this._statusBox.Name = "_statusBox";
            this._statusBox.Size = new System.Drawing.Size(121, 21);
            this._statusBox.TabIndex = 6;
            // 
            // _goButton
            // 
            this._goButton.Location = new System.Drawing.Point(486, 36);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(75, 23);
            this._goButton.TabIndex = 2;
            this._goButton.Text = "Установить";
            this._goButton.UseVisualStyleBackColor = true;
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this._goButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Статус партии";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _numberBox;
        private System.Windows.Forms.Button _goButton;
        private AM.Windows.Forms.BusyStripe _busyStripe;
        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _statusBox;
    }
}

