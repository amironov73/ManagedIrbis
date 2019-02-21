namespace Crocodile
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._yearBox = new System.Windows.Forms.ToolStripComboBox();
            this._booksBox = new AM.Windows.Forms.ToolStripCheckBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._goButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._termGrid = new System.Windows.Forms.DataGridView();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.termInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._termSource = new System.Windows.Forms.BindingSource(this.components);
            this._logBox = new AM.Windows.Forms.LogBox();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this._fondBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._stopButton = new System.Windows.Forms.ToolStripButton();
            this._fastBox = new AM.Windows.Forms.ToolStripCheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._termGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.termInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._termSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._yearBox,
            this.toolStripLabel2,
            this._fondBox,
            this._booksBox,
            this._fastBox,
            this.toolStripSeparator1,
            this._goButton,
            this._stopButton,
            this.toolStripSeparator2,
            this._saveButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(908, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(26, 22);
            this.toolStripLabel1.Text = "Год";
            // 
            // _yearBox
            // 
            this._yearBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._yearBox.Name = "_yearBox";
            this._yearBox.Size = new System.Drawing.Size(121, 25);
            this._yearBox.SelectedIndexChanged += new System.EventHandler(this._yearBox_SelectedIndexChanged);
            // 
            // _booksBox
            // 
            this._booksBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _booksBox
            // 
            this._booksBox.CheckBox.AccessibleName = "_booksBox";
            this._booksBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._booksBox.CheckBox.Checked = true;
            this._booksBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._booksBox.CheckBox.Location = new System.Drawing.Point(317, 1);
            this._booksBox.CheckBox.Name = "_booksBox";
            this._booksBox.CheckBox.Size = new System.Drawing.Size(84, 22);
            this._booksBox.CheckBox.TabIndex = 1;
            this._booksBox.CheckBox.Text = "С книгами";
            this._booksBox.CheckBox.UseVisualStyleBackColor = false;
            this._booksBox.Name = "_booksBox";
            this._booksBox.Size = new System.Drawing.Size(84, 22);
            this._booksBox.Text = "С книгами";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _goButton
            // 
            this._goButton.Image = ((System.Drawing.Image)(resources.GetObject("_goButton.Image")));
            this._goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(54, 22);
            this._goButton.Text = "Пуск";
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // _saveButton
            // 
            this._saveButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveButton.Image")));
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(85, 22);
            this._saveButton.Text = "Сохранить";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Location = new System.Drawing.Point(0, 25);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._termGrid);
            this._splitContainer.Size = new System.Drawing.Size(908, 409);
            this._splitContainer.SplitterDistance = 155;
            this._splitContainer.TabIndex = 1;
            // 
            // _termGrid
            // 
            this._termGrid.AllowUserToAddRows = false;
            this._termGrid.AllowUserToDeleteRows = false;
            this._termGrid.AllowUserToResizeRows = false;
            this._termGrid.AutoGenerateColumns = false;
            this._termGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._termGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.countDataGridViewTextBoxColumn,
            this.textDataGridViewTextBoxColumn});
            this._termGrid.DataSource = this.termInfoBindingSource;
            this._termGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._termGrid.Location = new System.Drawing.Point(0, 0);
            this._termGrid.Name = "_termGrid";
            this._termGrid.ReadOnly = true;
            this._termGrid.RowHeadersVisible = false;
            this._termGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._termGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._termGrid.Size = new System.Drawing.Size(155, 409);
            this._termGrid.TabIndex = 0;
            this._termGrid.DoubleClick += new System.EventHandler(this._termGrid_DoubleClick);
            // 
            // countDataGridViewTextBoxColumn
            // 
            this.countDataGridViewTextBoxColumn.DataPropertyName = "Count";
            this.countDataGridViewTextBoxColumn.HeaderText = "#";
            this.countDataGridViewTextBoxColumn.Name = "countDataGridViewTextBoxColumn";
            this.countDataGridViewTextBoxColumn.ReadOnly = true;
            this.countDataGridViewTextBoxColumn.Width = 50;
            // 
            // textDataGridViewTextBoxColumn
            // 
            this.textDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textDataGridViewTextBoxColumn.DataPropertyName = "Text";
            this.textDataGridViewTextBoxColumn.HeaderText = "Номер КСУ";
            this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
            this.textDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // termInfoBindingSource
            // 
            this.termInfoBindingSource.DataSource = typeof(ManagedIrbis.Search.TermInfo);
            // 
            // _termSource
            // 
            this._termSource.AllowNew = false;
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 434);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(908, 108);
            this._logBox.TabIndex = 2;
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "XLSX|*.xlsx|All|*.*";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel2.Text = "Фонд";
            // 
            // _fondBox
            // 
            this._fondBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._fondBox.Name = "_fondBox";
            this._fondBox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // _stopButton
            // 
            this._stopButton.Image = ((System.Drawing.Image)(resources.GetObject("_stopButton.Image")));
            this._stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(54, 22);
            this._stopButton.Text = "Стоп";
            this._stopButton.Click += new System.EventHandler(this._stopButton_Click);
            // 
            // _fastBox
            // 
            this._fastBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _fastBox
            // 
            this._fastBox.CheckBox.AccessibleName = "_fastBox";
            this._fastBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._fastBox.CheckBox.Location = new System.Drawing.Point(401, 1);
            this._fastBox.CheckBox.Name = "_fastBox";
            this._fastBox.CheckBox.Size = new System.Drawing.Size(67, 22);
            this._fastBox.CheckBox.TabIndex = 3;
            this._fastBox.CheckBox.Text = "Быстро";
            this._fastBox.CheckBox.UseVisualStyleBackColor = false;
            this._fastBox.Name = "_fastBox";
            this._fastBox.Size = new System.Drawing.Size(67, 22);
            this._fastBox.Text = "Быстро";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 542);
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this._logBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "КРОКОДИЛ — анализ эффективности комплектования";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this._splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._termGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.termInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._termSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.DataGridView _termGrid;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox _yearBox;
        private System.Windows.Forms.ToolStripButton _goButton;
        private System.Windows.Forms.BindingSource _termSource;
        private System.Windows.Forms.BindingSource termInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private AM.Windows.Forms.LogBox _logBox;
        private AM.Windows.Forms.ToolStripCheckBox _booksBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _saveButton;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox _fondBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _stopButton;
        private AM.Windows.Forms.ToolStripCheckBox _fastBox;
    }
}

