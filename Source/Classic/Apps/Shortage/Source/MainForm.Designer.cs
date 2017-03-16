namespace Shortage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._ksuGrid = new System.Windows.Forms.DataGridView();
            this._amountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._registerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._newPartyBox = new System.Windows.Forms.Button();
            this._yearBox = new System.Windows.Forms.NumericUpDown();
            this._bookGrid = new System.Windows.Forms.DataGridView();
            this._inventoryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._barcodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._markedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._barcodeBox = new System.Windows.Forms.TextBox();
            this._topLabel = new System.Windows.Forms.Label();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._unmarkedButton = new System.Windows.Forms.ToolStripButton();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ksuGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._yearBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bookGrid)).BeginInit();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._ksuGrid);
            this.splitContainer1.Panel1.Controls.Add(this._newPartyBox);
            this.splitContainer1.Panel1.Controls.Add(this._yearBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._bookGrid);
            this.splitContainer1.Panel2.Controls.Add(this._barcodeBox);
            this.splitContainer1.Panel2.Controls.Add(this._topLabel);
            this.splitContainer1.Size = new System.Drawing.Size(744, 349);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 1;
            // 
            // _ksuGrid
            // 
            this._ksuGrid.AllowUserToAddRows = false;
            this._ksuGrid.AllowUserToDeleteRows = false;
            this._ksuGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._ksuGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._ksuGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ksuGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._amountColumn,
            this._registerColumn});
            this._ksuGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ksuGrid.Location = new System.Drawing.Point(0, 20);
            this._ksuGrid.MultiSelect = false;
            this._ksuGrid.Name = "_ksuGrid";
            this._ksuGrid.ReadOnly = true;
            this._ksuGrid.RowHeadersVisible = false;
            this._ksuGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._ksuGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._ksuGrid.Size = new System.Drawing.Size(165, 306);
            this._ksuGrid.TabIndex = 4;
            this._ksuGrid.DoubleClick += new System.EventHandler(this._ksuGrid_DoubleClick);
            // 
            // _amountColumn
            // 
            this._amountColumn.DataPropertyName = "Count";
            this._amountColumn.HeaderText = "Кол-во";
            this._amountColumn.MinimumWidth = 45;
            this._amountColumn.Name = "_amountColumn";
            this._amountColumn.ReadOnly = true;
            this._amountColumn.Width = 45;
            // 
            // _registerColumn
            // 
            this._registerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._registerColumn.DataPropertyName = "Text";
            this._registerColumn.HeaderText = "Регистр";
            this._registerColumn.Name = "_registerColumn";
            this._registerColumn.ReadOnly = true;
            // 
            // _newPartyBox
            // 
            this._newPartyBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._newPartyBox.Location = new System.Drawing.Point(0, 326);
            this._newPartyBox.Name = "_newPartyBox";
            this._newPartyBox.Size = new System.Drawing.Size(165, 23);
            this._newPartyBox.TabIndex = 1;
            this._newPartyBox.Text = "Выбрать партию";
            this._newPartyBox.UseVisualStyleBackColor = true;
            this._newPartyBox.Click += new System.EventHandler(this._newPartyBox_Click);
            // 
            // _yearBox
            // 
            this._yearBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._yearBox.Location = new System.Drawing.Point(0, 0);
            this._yearBox.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this._yearBox.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this._yearBox.Name = "_yearBox";
            this._yearBox.Size = new System.Drawing.Size(165, 20);
            this._yearBox.TabIndex = 0;
            this._yearBox.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this._yearBox.ValueChanged += new System.EventHandler(this._yearBox_ValueChanged);
            // 
            // _bookGrid
            // 
            this._bookGrid.AllowUserToAddRows = false;
            this._bookGrid.AllowUserToDeleteRows = false;
            this._bookGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._bookGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this._bookGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._bookGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._inventoryColumn,
            this._barcodeColumn,
            this._descriptionColumn,
            this._markedColumn});
            this._bookGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bookGrid.Location = new System.Drawing.Point(0, 38);
            this._bookGrid.MultiSelect = false;
            this._bookGrid.Name = "_bookGrid";
            this._bookGrid.ReadOnly = true;
            this._bookGrid.RowHeadersVisible = false;
            this._bookGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._bookGrid.Size = new System.Drawing.Size(575, 291);
            this._bookGrid.TabIndex = 1;
            // 
            // _inventoryColumn
            // 
            this._inventoryColumn.DataPropertyName = "Number";
            this._inventoryColumn.HeaderText = "Инв. №";
            this._inventoryColumn.Name = "_inventoryColumn";
            this._inventoryColumn.ReadOnly = true;
            this._inventoryColumn.Width = 80;
            // 
            // _barcodeColumn
            // 
            this._barcodeColumn.DataPropertyName = "Barcode";
            this._barcodeColumn.HeaderText = "Радиометка";
            this._barcodeColumn.Name = "_barcodeColumn";
            this._barcodeColumn.ReadOnly = true;
            this._barcodeColumn.Width = 120;
            // 
            // _descriptionColumn
            // 
            this._descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._descriptionColumn.DataPropertyName = "Description";
            this._descriptionColumn.HeaderText = "Библиографическое описание";
            this._descriptionColumn.Name = "_descriptionColumn";
            this._descriptionColumn.ReadOnly = true;
            // 
            // _markedColumn
            // 
            this._markedColumn.DataPropertyName = "Marked";
            this._markedColumn.HeaderText = "Отм.";
            this._markedColumn.MinimumWidth = 40;
            this._markedColumn.Name = "_markedColumn";
            this._markedColumn.ReadOnly = true;
            this._markedColumn.Width = 40;
            // 
            // _barcodeBox
            // 
            this._barcodeBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._barcodeBox.Location = new System.Drawing.Point(0, 329);
            this._barcodeBox.Name = "_barcodeBox";
            this._barcodeBox.Size = new System.Drawing.Size(575, 20);
            this._barcodeBox.TabIndex = 2;
            this._barcodeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._barcodeBox_KeyDown);
            // 
            // _topLabel
            // 
            this._topLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topLabel.Location = new System.Drawing.Point(0, 0);
            this._topLabel.Name = "_topLabel";
            this._topLabel.Size = new System.Drawing.Size(575, 38);
            this._topLabel.TabIndex = 0;
            this._topLabel.Text = "Нет записей";
            this._topLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._saveButton,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this._unmarkedButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(744, 25);
            this._toolStrip.TabIndex = 3;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _saveButton
            // 
            this._saveButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveButton.Image")));
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(130, 22);
            this._saveButton.Text = "Сохранить партию";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(126, 22);
            this.toolStripButton1.Text = "Загрузить партию";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _unmarkedButton
            // 
            this._unmarkedButton.Image = ((System.Drawing.Image)(resources.GetObject("_unmarkedButton.Image")));
            this._unmarkedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._unmarkedButton.Name = "_unmarkedButton";
            this._unmarkedButton.Size = new System.Drawing.Size(177, 22);
            this._unmarkedButton.Text = "Список неподтверждённых";
            this._unmarkedButton.Click += new System.EventHandler(this._unmarkedButton_Click);
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "json";
            this._saveFileDialog.Filter = "JSON|*.json|ALL|*.*";
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "JSON|*.json|ALL|*.*";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Count";
            this.dataGridViewTextBoxColumn1.HeaderText = "Кол-во";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 45;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 45;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Text";
            this.dataGridViewTextBoxColumn2.HeaderText = "Регистр";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Number";
            this.dataGridViewTextBoxColumn3.HeaderText = "Инв. №";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Barcode";
            this.dataGridViewTextBoxColumn4.HeaderText = "Радиометка";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn5.HeaderText = "Библиографическое описание";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 384);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(744, 118);
            this._logBox.TabIndex = 0;
            // 
            // _busyStripe
            // 
            this._busyStripe.BackColor = System.Drawing.Color.White;
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._busyStripe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this._busyStripe.Location = new System.Drawing.Point(0, 374);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(744, 10);
            this._busyStripe.TabIndex = 4;
            this._busyStripe.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 502);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._busyStripe);
            this.Controls.Add(this._logBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.Text = "Нет недостачам!";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ksuGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._yearBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bookGrid)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button _newPartyBox;
        private System.Windows.Forms.NumericUpDown _yearBox;
        private System.Windows.Forms.DataGridView _ksuGrid;
        private System.Windows.Forms.Label _topLabel;
        private System.Windows.Forms.DataGridView _bookGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _amountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _registerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox _barcodeBox;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.ToolStripButton _saveButton;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _unmarkedButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn _inventoryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _barcodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _descriptionColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _markedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private AM.Windows.Forms.BusyStripe _busyStripe;
    }
}

