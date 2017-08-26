namespace Hairbrush
{
    partial class HairbrushPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._termGrid = new System.Windows.Forms.DataGridView();
            this._countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._textColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._selectedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._buttonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this._termDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._keyBox = new AM.Windows.Forms.TextBoxWithButton();
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this._applyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._termGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._termDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // _splitContainer1
            // 
            this._splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer1.Location = new System.Drawing.Point(0, 0);
            this._splitContainer1.Name = "_splitContainer1";
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._termGrid);
            this._splitContainer1.Panel1.Controls.Add(this._keyBox);
            // 
            // _splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._propertyGrid);
            this._splitContainer1.Panel2.Controls.Add(this._applyButton);
            this._splitContainer1.Size = new System.Drawing.Size(800, 600);
            this._splitContainer1.SplitterDistance = 289;
            this._splitContainer1.TabIndex = 0;
            // 
            // _termGrid
            // 
            this._termGrid.AllowUserToAddRows = false;
            this._termGrid.AllowUserToDeleteRows = false;
            this._termGrid.AllowUserToResizeColumns = false;
            this._termGrid.AllowUserToResizeRows = false;
            this._termGrid.AutoGenerateColumns = false;
            this._termGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._termGrid.ColumnHeadersVisible = false;
            this._termGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._countColumn,
            this._textColumn,
            this._selectedColumn,
            this._buttonColumn});
            this._termGrid.DataSource = this._termDataBindingSource;
            this._termGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._termGrid.Location = new System.Drawing.Point(0, 0);
            this._termGrid.MultiSelect = false;
            this._termGrid.Name = "_termGrid";
            this._termGrid.RowHeadersVisible = false;
            this._termGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._termGrid.Size = new System.Drawing.Size(289, 579);
            this._termGrid.TabIndex = 0;
            this._termGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._termGrid_CellContentClick);
            // 
            // _countColumn
            // 
            this._countColumn.DataPropertyName = "Count";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._countColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this._countColumn.HeaderText = "Count";
            this._countColumn.Name = "_countColumn";
            this._countColumn.ReadOnly = true;
            this._countColumn.Width = 50;
            // 
            // _textColumn
            // 
            this._textColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._textColumn.DataPropertyName = "Text";
            this._textColumn.HeaderText = "Text";
            this._textColumn.Name = "_textColumn";
            this._textColumn.ReadOnly = true;
            // 
            // _selectedColumn
            // 
            this._selectedColumn.DataPropertyName = "Selected";
            this._selectedColumn.HeaderText = "Selected";
            this._selectedColumn.Name = "_selectedColumn";
            this._selectedColumn.Width = 25;
            // 
            // _buttonColumn
            // 
            this._buttonColumn.HeaderText = "Button";
            this._buttonColumn.Name = "_buttonColumn";
            this._buttonColumn.ReadOnly = true;
            this._buttonColumn.Text = "->";
            this._buttonColumn.Width = 25;
            // 
            // _termDataBindingSource
            // 
            this._termDataBindingSource.AllowNew = false;
            this._termDataBindingSource.DataSource = typeof(Hairbrush.TermData);
            // 
            // _keyBox
            // 
            // 
            // 
            // 
            this._keyBox.Button.Dock = System.Windows.Forms.DockStyle.Right;
            this._keyBox.Button.Location = new System.Drawing.Point(269, 0);
            this._keyBox.Button.Name = "_button";
            this._keyBox.Button.Size = new System.Drawing.Size(20, 21);
            this._keyBox.Button.TabIndex = 1;
            this._keyBox.Button.UseVisualStyleBackColor = true;
            this._keyBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._keyBox.Location = new System.Drawing.Point(0, 579);
            this._keyBox.Name = "_keyBox";
            this._keyBox.SelectionStart = 0;
            this._keyBox.Size = new System.Drawing.Size(289, 21);
            this._keyBox.TabIndex = 1;
            // 
            // 
            // 
            this._keyBox.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._keyBox.TextBox.Location = new System.Drawing.Point(0, 0);
            this._keyBox.TextBox.Name = "_textBox";
            this._keyBox.TextBox.Size = new System.Drawing.Size(269, 20);
            this._keyBox.TextBox.TabIndex = 0;
            this._keyBox.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._keyBox_KeyDown);
            this._keyBox.ButtonClick += new System.EventHandler(this._keyBox_ButtonClick);
            this._keyBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._keyBox_KeyDown);
            // 
            // _propertyGrid
            // 
            this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this._propertyGrid.Location = new System.Drawing.Point(0, 0);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this._propertyGrid.Size = new System.Drawing.Size(507, 577);
            this._propertyGrid.TabIndex = 0;
            this._propertyGrid.ToolbarVisible = false;
            // 
            // _applyButton
            // 
            this._applyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._applyButton.Location = new System.Drawing.Point(0, 577);
            this._applyButton.Name = "_applyButton";
            this._applyButton.Size = new System.Drawing.Size(507, 23);
            this._applyButton.TabIndex = 1;
            this._applyButton.Text = "Применить к выбранным записям";
            this._applyButton.UseVisualStyleBackColor = true;
            this._applyButton.Click += new System.EventHandler(this._applyButton_Click);
            // 
            // HairbrushPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer1);
            this.Name = "HairbrushPanel";
            this.Size = new System.Drawing.Size(800, 600);
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).EndInit();
            this._splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._termGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._termDataBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer1;
        private System.Windows.Forms.DataGridView _termGrid;
        private System.Windows.Forms.BindingSource _termDataBindingSource;
        private AM.Windows.Forms.TextBoxWithButton _keyBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn _countColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _textColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _selectedColumn;
        private System.Windows.Forms.DataGridViewButtonColumn _buttonColumn;
        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private System.Windows.Forms.Button _applyButton;
    }
}
