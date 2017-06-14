namespace BookList2017
{
    partial class ListPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._tableLayout1 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayout2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._numberBox = new System.Windows.Forms.TextBox();
            this._addButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._tableLayout3 = new System.Windows.Forms.TableLayoutPanel();
            this._clearButton = new System.Windows.Forms.Button();
            this._deleteButton = new System.Windows.Forms.Button();
            this._buildButton = new System.Windows.Forms.Button();
            this._firstNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this._firstTimer = new System.Windows.Forms.Timer(this.components);
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Issue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shelfIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.placeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this._variantBox = new System.Windows.Forms.ComboBox();
            this._tableLayout1.SuspendLayout();
            this._tableLayout2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this._tableLayout3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._firstNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _tableLayout1
            // 
            this._tableLayout1.ColumnCount = 3;
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.Controls.Add(this._tableLayout2, 1, 1);
            this._tableLayout1.Controls.Add(this.dataGridView1, 1, 3);
            this._tableLayout1.Controls.Add(this._tableLayout3, 1, 2);
            this._tableLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayout1.Location = new System.Drawing.Point(0, 0);
            this._tableLayout1.Name = "_tableLayout1";
            this._tableLayout1.RowCount = 5;
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.Size = new System.Drawing.Size(800, 600);
            this._tableLayout1.TabIndex = 0;
            // 
            // _tableLayout2
            // 
            this._tableLayout2.AutoSize = true;
            this._tableLayout2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayout2.ColumnCount = 3;
            this._tableLayout2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout2.Controls.Add(this.label1, 0, 0);
            this._tableLayout2.Controls.Add(this._numberBox, 1, 0);
            this._tableLayout2.Controls.Add(this._addButton, 2, 0);
            this._tableLayout2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayout2.Location = new System.Drawing.Point(23, 23);
            this._tableLayout2.Name = "_tableLayout2";
            this._tableLayout2.RowCount = 1;
            this._tableLayout2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout2.Size = new System.Drawing.Size(754, 29);
            this._tableLayout2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Инв. номер или штрих-код";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _numberBox
            // 
            this._numberBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._numberBox.Location = new System.Drawing.Point(149, 3);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(521, 20);
            this._numberBox.TabIndex = 1;
            this._numberBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._numberBox_PreviewKeyDown);
            // 
            // _addButton
            // 
            this._addButton.Location = new System.Drawing.Point(676, 3);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(75, 23);
            this._addButton.TabIndex = 2;
            this._addButton.Text = "Добавить";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.yearDataGridViewTextBoxColumn,
            this.Issue,
            this.priceDataGridViewTextBoxColumn,
            this.shelfIndexDataGridViewTextBoxColumn,
            this.placeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this._bindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(23, 93);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(754, 484);
            this.dataGridView1.TabIndex = 1;
            // 
            // _bindingSource
            // 
            this._bindingSource.DataSource = typeof(ManagedIrbis.Fields.ExemplarInfo);
            // 
            // _tableLayout3
            // 
            this._tableLayout3.AutoSize = true;
            this._tableLayout3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayout3.ColumnCount = 8;
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._tableLayout3.Controls.Add(this._clearButton, 0, 0);
            this._tableLayout3.Controls.Add(this._deleteButton, 1, 0);
            this._tableLayout3.Controls.Add(this._buildButton, 2, 0);
            this._tableLayout3.Controls.Add(this.label2, 6, 0);
            this._tableLayout3.Controls.Add(this._firstNumberBox, 7, 0);
            this._tableLayout3.Controls.Add(this.label3, 4, 0);
            this._tableLayout3.Controls.Add(this._variantBox, 5, 0);
            this._tableLayout3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayout3.Location = new System.Drawing.Point(23, 58);
            this._tableLayout3.Name = "_tableLayout3";
            this._tableLayout3.RowCount = 1;
            this._tableLayout3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout3.Size = new System.Drawing.Size(754, 29);
            this._tableLayout3.TabIndex = 2;
            // 
            // _clearButton
            // 
            this._clearButton.AutoSize = true;
            this._clearButton.Location = new System.Drawing.Point(3, 3);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(103, 23);
            this._clearButton.TabIndex = 0;
            this._clearButton.Text = "Очистить список";
            this._clearButton.UseVisualStyleBackColor = true;
            this._clearButton.Click += new System.EventHandler(this._clearButton_Click);
            // 
            // _deleteButton
            // 
            this._deleteButton.AutoSize = true;
            this._deleteButton.Location = new System.Drawing.Point(112, 3);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(75, 23);
            this._deleteButton.TabIndex = 1;
            this._deleteButton.Text = "Удалить";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // _buildButton
            // 
            this._buildButton.AutoSize = true;
            this._buildButton.Location = new System.Drawing.Point(193, 3);
            this._buildButton.Name = "_buildButton";
            this._buildButton.Size = new System.Drawing.Size(110, 23);
            this._buildButton.TabIndex = 2;
            this._buildButton.Text = "Построить список";
            this._buildButton.UseVisualStyleBackColor = true;
            this._buildButton.Click += new System.EventHandler(this._buildButton_Click);
            // 
            // _firstNumberBox
            // 
            this._firstNumberBox.Location = new System.Drawing.Point(667, 3);
            this._firstNumberBox.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this._firstNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._firstNumberBox.Name = "_firstNumberBox";
            this._firstNumberBox.Size = new System.Drawing.Size(84, 20);
            this._firstNumberBox.TabIndex = 4;
            this._firstNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(556, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Начинать с номера";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _firstTimer
            // 
            this._firstTimer.Enabled = true;
            this._firstTimer.Tick += new System.EventHandler(this._firstTimer_Tick);
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Инв. №";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            this.numberDataGridViewTextBoxColumn.ReadOnly = true;
            this.numberDataGridViewTextBoxColumn.Width = 70;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Автор, заглавие";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // yearDataGridViewTextBoxColumn
            // 
            this.yearDataGridViewTextBoxColumn.DataPropertyName = "Year";
            this.yearDataGridViewTextBoxColumn.HeaderText = "Год";
            this.yearDataGridViewTextBoxColumn.Name = "yearDataGridViewTextBoxColumn";
            this.yearDataGridViewTextBoxColumn.ReadOnly = true;
            this.yearDataGridViewTextBoxColumn.Width = 50;
            // 
            // Issue
            // 
            this.Issue.DataPropertyName = "Issue";
            this.Issue.HeaderText = "Вып.";
            this.Issue.Name = "Issue";
            this.Issue.ReadOnly = true;
            this.Issue.Width = 50;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Цена";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn.Width = 50;
            // 
            // shelfIndexDataGridViewTextBoxColumn
            // 
            this.shelfIndexDataGridViewTextBoxColumn.DataPropertyName = "ShelfIndex";
            this.shelfIndexDataGridViewTextBoxColumn.HeaderText = "Шифр";
            this.shelfIndexDataGridViewTextBoxColumn.Name = "shelfIndexDataGridViewTextBoxColumn";
            this.shelfIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // placeDataGridViewTextBoxColumn
            // 
            this.placeDataGridViewTextBoxColumn.DataPropertyName = "Place";
            this.placeDataGridViewTextBoxColumn.HeaderText = "Фонд";
            this.placeDataGridViewTextBoxColumn.Name = "placeDataGridViewTextBoxColumn";
            this.placeDataGridViewTextBoxColumn.ReadOnly = true;
            this.placeDataGridViewTextBoxColumn.Width = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(329, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Список";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _variantBox
            // 
            this._variantBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._variantBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._variantBox.FormattingEnabled = true;
            this._variantBox.Location = new System.Drawing.Point(379, 3);
            this._variantBox.Name = "_variantBox";
            this._variantBox.Size = new System.Drawing.Size(171, 21);
            this._variantBox.TabIndex = 6;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayout1);
            this.Name = "ListPanel";
            this.Size = new System.Drawing.Size(800, 600);
            this._tableLayout1.ResumeLayout(false);
            this._tableLayout1.PerformLayout();
            this._tableLayout2.ResumeLayout(false);
            this._tableLayout2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this._tableLayout3.ResumeLayout(false);
            this._tableLayout3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._firstNumberBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout1;
        private System.Windows.Forms.TableLayoutPanel _tableLayout2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _numberBox;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel _tableLayout3;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Button _buildButton;
        private System.Windows.Forms.Timer _firstTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _firstNumberBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Issue;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shelfIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn placeDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _variantBox;
    }
}
