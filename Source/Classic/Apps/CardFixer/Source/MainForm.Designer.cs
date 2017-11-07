namespace CardFixer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._listBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this._boxNumberBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._goodButton = new System.Windows.Forms.Button();
            this._badButton = new System.Windows.Forms.Button();
            this._numberBox = new System.Windows.Forms.TextBox();
            this._logBox = new AM.Windows.Forms.LogBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._listBox);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this._boxNumberBox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._pictureBox);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this._numberBox);
            this.splitContainer1.Panel2.Controls.Add(this._logBox);
            this.splitContainer1.Size = new System.Drawing.Size(784, 561);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.TabIndex = 0;
            // 
            // _listBox
            // 
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(0, 47);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(261, 514);
            this._listBox.TabIndex = 3;
            this._listBox.SelectedIndexChanged += new System.EventHandler(this._listBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Список карточек";
            // 
            // _boxNumberBox
            // 
            this._boxNumberBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._boxNumberBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._boxNumberBox.FormattingEnabled = true;
            this._boxNumberBox.Location = new System.Drawing.Point(0, 13);
            this._boxNumberBox.Name = "_boxNumberBox";
            this._boxNumberBox.Size = new System.Drawing.Size(261, 21);
            this._boxNumberBox.TabIndex = 1;
            this._boxNumberBox.SelectedIndexChanged += new System.EventHandler(this._boxNumberBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер ящика";
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(519, 330);
            this._pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._goodButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._badButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 330);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(519, 29);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // _goodButton
            // 
            this._goodButton.Dock = System.Windows.Forms.DockStyle.Top;
            this._goodButton.Location = new System.Drawing.Point(3, 3);
            this._goodButton.Name = "_goodButton";
            this._goodButton.Size = new System.Drawing.Size(253, 23);
            this._goodButton.TabIndex = 0;
            this._goodButton.Text = "На ввод [F2]";
            this._goodButton.UseVisualStyleBackColor = true;
            // 
            // _badButton
            // 
            this._badButton.Dock = System.Windows.Forms.DockStyle.Top;
            this._badButton.Location = new System.Drawing.Point(262, 3);
            this._badButton.Name = "_badButton";
            this._badButton.Size = new System.Drawing.Size(254, 23);
            this._badButton.TabIndex = 1;
            this._badButton.Text = "Пропустить [F6]";
            this._badButton.UseVisualStyleBackColor = true;
            // 
            // _numberBox
            // 
            this._numberBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._numberBox.Location = new System.Drawing.Point(0, 359);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(519, 20);
            this._numberBox.TabIndex = 1;
            this._numberBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._numberBox_KeyDown);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 379);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(519, 182);
            this._logBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Коррекция карточек";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox _listBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _boxNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.TextBox _numberBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _goodButton;
        private System.Windows.Forms.Button _badButton;
        private AM.Windows.Forms.LogBox _logBox;
    }
}

