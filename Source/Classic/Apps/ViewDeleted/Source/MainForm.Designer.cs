namespace ViewDeleted
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
            this._databaseBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._modeBox = new System.Windows.Forms.CheckBox();
            this._foundBox = new System.Windows.Forms.ListBox();
            this._allBox = new System.Windows.Forms.ListBox();
            this._valueBox = new System.Windows.Forms.TextBox();
            this._searchButton = new System.Windows.Forms.Button();
            this._web = new System.Windows.Forms.WebBrowser();
            this._undeleteButton = new System.Windows.Forms.Button();
            this._formatBox = new System.Windows.Forms.ComboBox();
            this._statusBar = new System.Windows.Forms.StatusStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this._statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // _databaseBox
            // 
            this._databaseBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._databaseBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._databaseBox.FormattingEnabled = true;
            this._databaseBox.Location = new System.Drawing.Point(12, 12);
            this._databaseBox.Name = "_databaseBox";
            this._databaseBox.Size = new System.Drawing.Size(864, 21);
            this._databaseBox.TabIndex = 0;
            this._databaseBox.SelectedIndexChanged += new System.EventHandler(this._databaseBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._modeBox);
            this.groupBox1.Controls.Add(this._foundBox);
            this.groupBox1.Controls.Add(this._allBox);
            this.groupBox1.Location = new System.Drawing.Point(18, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 519);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MFN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Найденные";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Все удаленные";
            // 
            // _modeBox
            // 
            this._modeBox.AutoSize = true;
            this._modeBox.Location = new System.Drawing.Point(6, 17);
            this._modeBox.Name = "_modeBox";
            this._modeBox.Size = new System.Drawing.Size(187, 17);
            this._modeBox.TabIndex = 2;
            this._modeBox.Text = "Показывать найденные записи";
            this._modeBox.UseVisualStyleBackColor = true;
            this._modeBox.CheckedChanged += new System.EventHandler(this._modeBox_CheckedChanged);
            // 
            // _foundBox
            // 
            this._foundBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._foundBox.FormattingEnabled = true;
            this._foundBox.Location = new System.Drawing.Point(132, 66);
            this._foundBox.Name = "_foundBox";
            this._foundBox.Size = new System.Drawing.Size(120, 433);
            this._foundBox.TabIndex = 1;
            this._foundBox.SelectedIndexChanged += new System.EventHandler(this._foundBox_SelectedIndexChanged);
            // 
            // _allBox
            // 
            this._allBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._allBox.FormattingEnabled = true;
            this._allBox.Location = new System.Drawing.Point(6, 66);
            this._allBox.Name = "_allBox";
            this._allBox.Size = new System.Drawing.Size(120, 433);
            this._allBox.TabIndex = 0;
            this._allBox.SelectedIndexChanged += new System.EventHandler(this._allBox_SelectedIndexChanged);
            // 
            // _valueBox
            // 
            this._valueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._valueBox.Location = new System.Drawing.Point(12, 45);
            this._valueBox.Name = "_valueBox";
            this._valueBox.Size = new System.Drawing.Size(720, 20);
            this._valueBox.TabIndex = 2;
            // 
            // _searchButton
            // 
            this._searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._searchButton.Location = new System.Drawing.Point(738, 43);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(138, 23);
            this._searchButton.TabIndex = 3;
            this._searchButton.Text = "Искать";
            this._searchButton.UseVisualStyleBackColor = true;
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // _web
            // 
            this._web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._web.Location = new System.Drawing.Point(289, 117);
            this._web.MinimumSize = new System.Drawing.Size(20, 20);
            this._web.Name = "_web";
            this._web.Size = new System.Drawing.Size(587, 447);
            this._web.TabIndex = 4;
            // 
            // _undeleteButton
            // 
            this._undeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._undeleteButton.Location = new System.Drawing.Point(289, 570);
            this._undeleteButton.Name = "_undeleteButton";
            this._undeleteButton.Size = new System.Drawing.Size(587, 23);
            this._undeleteButton.TabIndex = 5;
            this._undeleteButton.Text = "Восстановить";
            this._undeleteButton.UseVisualStyleBackColor = true;
            this._undeleteButton.Click += new System.EventHandler(this._undeleteButton_Click);
            // 
            // _formatBox
            // 
            this._formatBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._formatBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._formatBox.FormattingEnabled = true;
            this._formatBox.Items.AddRange(new object[] {
            "&uf(\'+9I!\\b!!<b>!\',&uf(\'+9I!\\b0!!</b>!\',&uf(\'+9I!\\par!/<br>/\'&uf(\'0\'))))",
            "@BRIEF",
            "@ALL",
            "@",
            "@KN_H",
            "@MN_H",
            "@ASP_H",
            "@JW_H"});
            this._formatBox.Location = new System.Drawing.Point(289, 90);
            this._formatBox.Name = "_formatBox";
            this._formatBox.Size = new System.Drawing.Size(587, 21);
            this._formatBox.TabIndex = 6;
            this._formatBox.SelectedIndexChanged += new System.EventHandler(this._formatBox_SelectedIndexChanged);
            // 
            // _statusBar
            // 
            this._statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._progressBar});
            this._statusBar.Location = new System.Drawing.Point(0, 599);
            this._statusBar.Name = "_statusBar";
            this._statusBar.Size = new System.Drawing.Size(888, 22);
            this._statusBar.TabIndex = 7;
            // 
            // _statusLabel
            // 
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(12, 17);
            this._statusLabel.Text = "_";
            // 
            // _progressBar
            // 
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(100, 16);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // _backgroundWorker
            // 
            this._backgroundWorker.WorkerReportsProgress = true;
            this._backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._backgroundWorker_DoWork);
            this._backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this._backgroundWorker_ProgressChanged);
            this._backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._backgroundWorker_RunWorkerCompleted);
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 621);
            this.Controls.Add(this._statusBar);
            this.Controls.Add(this._formatBox);
            this.Controls.Add(this._undeleteButton);
            this.Controls.Add(this._web);
            this.Controls.Add(this._searchButton);
            this.Controls.Add(this._valueBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._databaseBox);
            this.Name = "MainForm";
            this.Text = "Поиск по удалённым записям";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._statusBar.ResumeLayout(false);
            this._statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _databaseBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _modeBox;
        private System.Windows.Forms.ListBox _foundBox;
        private System.Windows.Forms.ListBox _allBox;
        private System.Windows.Forms.TextBox _valueBox;
        private System.Windows.Forms.Button _searchButton;
        private System.Windows.Forms.WebBrowser _web;
        private System.Windows.Forms.Button _undeleteButton;
        private System.Windows.Forms.ComboBox _formatBox;
        private System.Windows.Forms.StatusStrip _statusBar;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.ComponentModel.BackgroundWorker _backgroundWorker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer _timer;
    }
}

