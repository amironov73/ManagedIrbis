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
            this._grid = new System.Windows.Forms.DataGridView();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._tableLayout3 = new System.Windows.Forms.TableLayoutPanel();
            this._clearButton = new System.Windows.Forms.Button();
            this._deleteButton = new System.Windows.Forms.Button();
            this._buildButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._firstNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this._variantBox = new System.Windows.Forms.ComboBox();
            this.tableLayout4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this._formatBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this._sortBox = new System.Windows.Forms.ComboBox();
            this._firstTimer = new System.Windows.Forms.Timer(this.components);
            this._tableLayout1.SuspendLayout();
            this._tableLayout2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this._tableLayout3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._firstNumberBox)).BeginInit();
            this.tableLayout4.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayout1
            // 
            this._tableLayout1.ColumnCount = 3;
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.Controls.Add(this._tableLayout2, 1, 1);
            this._tableLayout1.Controls.Add(this._grid, 1, 4);
            this._tableLayout1.Controls.Add(this._tableLayout3, 1, 2);
            this._tableLayout1.Controls.Add(this.tableLayout4, 1, 3);
            this._tableLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayout1.Location = new System.Drawing.Point(0, 0);
            this._tableLayout1.Name = "_tableLayout1";
            this._tableLayout1.RowCount = 6;
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
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
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.AutoGenerateColumns = false;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.DataSource = this._bindingSource;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(23, 126);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(754, 451);
            this._grid.TabIndex = 1;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(557, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Начинать с номера";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _firstNumberBox
            // 
            this._firstNumberBox.Location = new System.Drawing.Point(668, 3);
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
            this._firstNumberBox.Size = new System.Drawing.Size(83, 20);
            this._firstNumberBox.TabIndex = 4;
            this._firstNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            this._variantBox.Size = new System.Drawing.Size(172, 21);
            this._variantBox.TabIndex = 6;
            // 
            // tableLayout4
            // 
            this.tableLayout4.AutoSize = true;
            this.tableLayout4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout4.ColumnCount = 6;
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayout4.Controls.Add(this.label4, 0, 0);
            this.tableLayout4.Controls.Add(this._formatBox, 1, 0);
            this.tableLayout4.Controls.Add(this.label5, 3, 0);
            this.tableLayout4.Controls.Add(this._sortBox, 4, 0);
            this.tableLayout4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout4.Location = new System.Drawing.Point(23, 93);
            this.tableLayout4.Name = "tableLayout4";
            this.tableLayout4.RowCount = 1;
            this.tableLayout4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout4.Size = new System.Drawing.Size(754, 27);
            this.tableLayout4.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Формат";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _formatBox
            // 
            this._formatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._formatBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._formatBox.FormattingEnabled = true;
            this._formatBox.Location = new System.Drawing.Point(58, 3);
            this._formatBox.Name = "_formatBox";
            this._formatBox.Size = new System.Drawing.Size(286, 21);
            this._formatBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(370, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 27);
            this.label5.TabIndex = 2;
            this.label5.Text = "Сортировка";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _sortBox
            // 
            this._sortBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sortBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sortBox.FormattingEnabled = true;
            this._sortBox.Location = new System.Drawing.Point(443, 3);
            this._sortBox.Name = "_sortBox";
            this._sortBox.Size = new System.Drawing.Size(286, 21);
            this._sortBox.TabIndex = 3;
            // 
            // _firstTimer
            // 
            this._firstTimer.Enabled = true;
            this._firstTimer.Tick += new System.EventHandler(this._firstTimer_Tick);
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
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this._tableLayout3.ResumeLayout(false);
            this._tableLayout3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._firstNumberBox)).EndInit();
            this.tableLayout4.ResumeLayout(false);
            this.tableLayout4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout1;
        private System.Windows.Forms.TableLayoutPanel _tableLayout2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _numberBox;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.TableLayoutPanel _tableLayout3;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Button _buildButton;
        private System.Windows.Forms.Timer _firstTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _firstNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _variantBox;
        private System.Windows.Forms.TableLayoutPanel tableLayout4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _formatBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _sortBox;
    }
}
