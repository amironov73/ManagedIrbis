namespace IrbisUI
{
    partial class PftTokenGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._grid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._kindColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._lineColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._columnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._kindColumn,
            this._valueColumn,
            this._lineColumn,
            this._columnColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(403, 232);
            this._grid.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Kind";
            this.dataGridViewTextBoxColumn1.HeaderText = "Kind";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Text";
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Line";
            this.dataGridViewTextBoxColumn3.HeaderText = "Line";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 40;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Column";
            this.dataGridViewTextBoxColumn4.HeaderText = "Col";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 40;
            // 
            // _kindColumn
            // 
            this._kindColumn.DataPropertyName = "Kind";
            this._kindColumn.HeaderText = "Kind";
            this._kindColumn.Name = "_kindColumn";
            this._kindColumn.ReadOnly = true;
            // 
            // _valueColumn
            // 
            this._valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._valueColumn.DataPropertyName = "Text";
            this._valueColumn.HeaderText = "Value";
            this._valueColumn.Name = "_valueColumn";
            this._valueColumn.ReadOnly = true;
            // 
            // _lineColumn
            // 
            this._lineColumn.DataPropertyName = "Line";
            this._lineColumn.HeaderText = "Line";
            this._lineColumn.MinimumWidth = 40;
            this._lineColumn.Name = "_lineColumn";
            this._lineColumn.ReadOnly = true;
            this._lineColumn.Width = 40;
            // 
            // _columnColumn
            // 
            this._columnColumn.DataPropertyName = "Column";
            this._columnColumn.HeaderText = "Col";
            this._columnColumn.MinimumWidth = 40;
            this._columnColumn.Name = "_columnColumn";
            this._columnColumn.ReadOnly = true;
            this._columnColumn.Width = 40;
            // 
            // PftTokenGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Name = "PftTokenGrid";
            this.Size = new System.Drawing.Size(403, 232);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _kindColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _lineColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _columnColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
