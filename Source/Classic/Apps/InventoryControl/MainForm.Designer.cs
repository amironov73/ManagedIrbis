namespace InventoryControl
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
            this._logBox = new AM.Windows.Forms.LogBox();
            this._dbBox = new System.Windows.Forms.ComboBox();
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this._mainTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this._patternBox = new System.Windows.Forms.ComboBox();
            this._startNumber = new System.Windows.Forms.NumericUpDown();
            this._SeenPlusHandsButton = new System.Windows.Forms.Button();
            this._booksOnHandsButton = new System.Windows.Forms.Button();
            this._numberIndexBox = new System.Windows.Forms.CheckBox();
            this._wrapDescriptionBox = new System.Windows.Forms.CheckBox();
            this._sortByTitleBox = new System.Windows.Forms.CheckBox();
            this._dayStatButton = new System.Windows.Forms.Button();
            this._nmhrButton = new System.Windows.Forms.Button();
            this._showDefectButton = new System.Windows.Forms.Button();
            this._showMissingButton = new System.Windows.Forms.Button();
            this._showSeenButton = new System.Windows.Forms.Button();
            this._showAllButton = new System.Windows.Forms.Button();
            this._createDbButton = new System.Windows.Forms.Button();
            this._extractButton = new System.Windows.Forms.Button();
            this._tabBox = new System.Windows.Forms.TabControl();
            this._taskbarAssistant = new DevExpress.Utils.Taskbar.TaskbarAssistant();
            this._mainTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._startNumber)).BeginInit();
            this._tabBox.SuspendLayout();
            this.SuspendLayout();
            //
            // _logBox
            //
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Location = new System.Drawing.Point(0, 296);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(784, 266);
            this._logBox.TabIndex = 1;
            //
            // _dbBox
            //
            this._dbBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._dbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._dbBox.FormattingEnabled = true;
            this._dbBox.Location = new System.Drawing.Point(0, 0);
            this._dbBox.Name = "_dbBox";
            this._dbBox.Size = new System.Drawing.Size(784, 21);
            this._dbBox.TabIndex = 0;
            this._dbBox.SelectedIndexChanged += new System.EventHandler(this._dbBox_SelectedIndexChanged);
            //
            // _timer1
            //
            this._timer1.Enabled = true;
            this._timer1.Interval = 60000;
            this._timer1.Tick += new System.EventHandler(this._timer1_Tick);
            //
            // _mainTab
            //
            this._mainTab.Controls.Add(this.label1);
            this._mainTab.Controls.Add(this._patternBox);
            this._mainTab.Controls.Add(this._startNumber);
            this._mainTab.Controls.Add(this._SeenPlusHandsButton);
            this._mainTab.Controls.Add(this._booksOnHandsButton);
            this._mainTab.Controls.Add(this._numberIndexBox);
            this._mainTab.Controls.Add(this._wrapDescriptionBox);
            this._mainTab.Controls.Add(this._sortByTitleBox);
            this._mainTab.Controls.Add(this._dayStatButton);
            this._mainTab.Controls.Add(this._nmhrButton);
            this._mainTab.Controls.Add(this._showDefectButton);
            this._mainTab.Controls.Add(this._showMissingButton);
            this._mainTab.Controls.Add(this._showSeenButton);
            this._mainTab.Controls.Add(this._showAllButton);
            this._mainTab.Controls.Add(this._createDbButton);
            this._mainTab.Controls.Add(this._extractButton);
            this._mainTab.Location = new System.Drawing.Point(4, 22);
            this._mainTab.Name = "_mainTab";
            this._mainTab.Padding = new System.Windows.Forms.Padding(3);
            this._mainTab.Size = new System.Drawing.Size(776, 249);
            this._mainTab.TabIndex = 0;
            this._mainTab.Text = "Основные операции";
            this._mainTab.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Шаблон инв. номера";
            //
            // _patternBox
            //
            this._patternBox.FormattingEnabled = true;
            this._patternBox.Items.AddRange(new object[] {
            "^[0123456789CАБИКМНФ][^н]{2,}$",
            "^(\\d|\\d\\d|[ГЖП]\\d+|б/н)$"});
            this._patternBox.Location = new System.Drawing.Point(191, 130);
            this._patternBox.Name = "_patternBox";
            this._patternBox.Size = new System.Drawing.Size(161, 21);
            this._patternBox.TabIndex = 13;
            //
            // _startNumber
            //
            this._startNumber.Location = new System.Drawing.Point(191, 159);
            this._startNumber.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this._startNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._startNumber.Name = "_startNumber";
            this._startNumber.Size = new System.Drawing.Size(157, 21);
            this._startNumber.TabIndex = 12;
            this._startNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // _SeenPlusHandsButton
            //
            this._SeenPlusHandsButton.Location = new System.Drawing.Point(191, 188);
            this._SeenPlusHandsButton.Name = "_SeenPlusHandsButton";
            this._SeenPlusHandsButton.Size = new System.Drawing.Size(161, 23);
            this._SeenPlusHandsButton.TabIndex = 11;
            this._SeenPlusHandsButton.Text = "Проверенные плюс на руках";
            this._SeenPlusHandsButton.UseVisualStyleBackColor = true;
            this._SeenPlusHandsButton.Click += new System.EventHandler(this._SeenPlusHandsButton_Click);
            //
            // _booksOnHandsButton
            //
            this._booksOnHandsButton.Location = new System.Drawing.Point(191, 217);
            this._booksOnHandsButton.Name = "_booksOnHandsButton";
            this._booksOnHandsButton.Size = new System.Drawing.Size(161, 23);
            this._booksOnHandsButton.TabIndex = 11;
            this._booksOnHandsButton.Text = "Книги на руках";
            this._booksOnHandsButton.UseVisualStyleBackColor = true;
            this._booksOnHandsButton.Click += new System.EventHandler(this._booksOnHandsButton_Click);
            //
            // _numberIndexBox
            //
            this._numberIndexBox.AutoSize = true;
            this._numberIndexBox.Location = new System.Drawing.Point(191, 66);
            this._numberIndexBox.Name = "_numberIndexBox";
            this._numberIndexBox.Size = new System.Drawing.Size(131, 17);
            this._numberIndexBox.TabIndex = 10;
            this._numberIndexBox.Text = "Вместо шифра номер";
            this._numberIndexBox.UseVisualStyleBackColor = true;
            //
            // _wrapDescriptionBox
            //
            this._wrapDescriptionBox.AutoSize = true;
            this._wrapDescriptionBox.Location = new System.Drawing.Point(191, 42);
            this._wrapDescriptionBox.Name = "_wrapDescriptionBox";
            this._wrapDescriptionBox.Size = new System.Drawing.Size(161, 17);
            this._wrapDescriptionBox.TabIndex = 9;
            this._wrapDescriptionBox.Text = "Переносить биб. описания";
            this._wrapDescriptionBox.UseVisualStyleBackColor = true;
            //
            // _sortByTitleBox
            //
            this._sortByTitleBox.AutoSize = true;
            this._sortByTitleBox.Location = new System.Drawing.Point(191, 18);
            this._sortByTitleBox.Name = "_sortByTitleBox";
            this._sortByTitleBox.Size = new System.Drawing.Size(157, 17);
            this._sortByTitleBox.TabIndex = 8;
            this._sortByTitleBox.Text = "Сортировка по заглавиям";
            this._sortByTitleBox.UseVisualStyleBackColor = true;
            //
            // _dayStatButton
            //
            this._dayStatButton.Location = new System.Drawing.Point(8, 217);
            this._dayStatButton.Name = "_dayStatButton";
            this._dayStatButton.Size = new System.Drawing.Size(167, 23);
            this._dayStatButton.TabIndex = 7;
            this._dayStatButton.Text = "Статистика по дням";
            this._dayStatButton.UseVisualStyleBackColor = true;
            this._dayStatButton.Click += new System.EventHandler(this._dayStatButton_Click);
            //
            // _nmhrButton
            //
            this._nmhrButton.Location = new System.Drawing.Point(8, 188);
            this._nmhrButton.Name = "_nmhrButton";
            this._nmhrButton.Size = new System.Drawing.Size(167, 23);
            this._nmhrButton.TabIndex = 6;
            this._nmhrButton.Text = "Передать в НМХР";
            this._nmhrButton.UseVisualStyleBackColor = true;
            this._nmhrButton.Click += new System.EventHandler(this._nmhrButton_Click);
            //
            // _showDefectButton
            //
            this._showDefectButton.Location = new System.Drawing.Point(8, 159);
            this._showDefectButton.Name = "_showDefectButton";
            this._showDefectButton.Size = new System.Drawing.Size(167, 23);
            this._showDefectButton.TabIndex = 5;
            this._showDefectButton.Text = "Дефектные описания";
            this._showDefectButton.UseVisualStyleBackColor = true;
            this._showDefectButton.Click += new System.EventHandler(this._showDefectButton_Click);
            //
            // _showMissingButton
            //
            this._showMissingButton.Location = new System.Drawing.Point(8, 130);
            this._showMissingButton.Name = "_showMissingButton";
            this._showMissingButton.Size = new System.Drawing.Size(167, 23);
            this._showMissingButton.TabIndex = 4;
            this._showMissingButton.Text = "Отсутствующие книги";
            this._showMissingButton.UseVisualStyleBackColor = true;
            this._showMissingButton.Click += new System.EventHandler(this._showMissingButton_Click);
            //
            // _showSeenButton
            //
            this._showSeenButton.Location = new System.Drawing.Point(8, 101);
            this._showSeenButton.Name = "_showSeenButton";
            this._showSeenButton.Size = new System.Drawing.Size(167, 23);
            this._showSeenButton.TabIndex = 3;
            this._showSeenButton.Text = "Проверенные книги";
            this._showSeenButton.UseVisualStyleBackColor = true;
            this._showSeenButton.Click += new System.EventHandler(this._showSeenButton_Click);
            //
            // _showAllButton
            //
            this._showAllButton.Location = new System.Drawing.Point(8, 72);
            this._showAllButton.Name = "_showAllButton";
            this._showAllButton.Size = new System.Drawing.Size(167, 23);
            this._showAllButton.TabIndex = 2;
            this._showAllButton.Text = "Показать все книги";
            this._showAllButton.UseVisualStyleBackColor = true;
            this._showAllButton.Click += new System.EventHandler(this._showAllButton_Click);
            //
            // _createDbButton
            //
            this._createDbButton.Location = new System.Drawing.Point(8, 14);
            this._createDbButton.Name = "_createDbButton";
            this._createDbButton.Size = new System.Drawing.Size(167, 23);
            this._createDbButton.TabIndex = 1;
            this._createDbButton.Text = "Пересоздать базу данных";
            this._createDbButton.UseVisualStyleBackColor = true;
            this._createDbButton.Click += new System.EventHandler(this._createDbButton_Click);
            //
            // _extractButton
            //
            this._extractButton.Location = new System.Drawing.Point(8, 43);
            this._extractButton.Name = "_extractButton";
            this._extractButton.Size = new System.Drawing.Size(167, 23);
            this._extractButton.TabIndex = 0;
            this._extractButton.Text = "Извлечь экземпляры";
            this._extractButton.UseVisualStyleBackColor = true;
            this._extractButton.Click += new System.EventHandler(this._extractButton_Click);
            //
            // _tabBox
            //
            this._tabBox.Controls.Add(this._mainTab);
            this._tabBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._tabBox.Location = new System.Drawing.Point(0, 21);
            this._tabBox.Name = "_tabBox";
            this._tabBox.SelectedIndex = 0;
            this._tabBox.Size = new System.Drawing.Size(784, 275);
            this._tabBox.TabIndex = 0;
            //
            // _taskbarAssistant
            //
            this._taskbarAssistant.ParentControl = this;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this._logBox);
            this.Controls.Add(this._tabBox);
            this.Controls.Add(this._dbBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Обработка результатов инвентаризации фонда";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._mainTab.ResumeLayout(false);
            this._mainTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._startNumber)).EndInit();
            this._tabBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.ComboBox _dbBox;
        private System.Windows.Forms.Timer _timer1;
        private System.Windows.Forms.TabPage _mainTab;
        private System.Windows.Forms.Button _showSeenButton;
        private System.Windows.Forms.Button _showAllButton;
        private System.Windows.Forms.Button _createDbButton;
        private System.Windows.Forms.Button _extractButton;
        private System.Windows.Forms.TabControl _tabBox;
        private System.Windows.Forms.Button _showMissingButton;
        private System.Windows.Forms.Button _showDefectButton;
        private DevExpress.Utils.Taskbar.TaskbarAssistant _taskbarAssistant;
        private System.Windows.Forms.Button _nmhrButton;
        private System.Windows.Forms.Button _dayStatButton;
        private System.Windows.Forms.CheckBox _sortByTitleBox;
        private System.Windows.Forms.CheckBox _wrapDescriptionBox;
        private System.Windows.Forms.CheckBox _numberIndexBox;
        private System.Windows.Forms.Button _booksOnHandsButton;
        private System.Windows.Forms.Button _SeenPlusHandsButton;
        private System.Windows.Forms.NumericUpDown _startNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _patternBox;

    }
}

