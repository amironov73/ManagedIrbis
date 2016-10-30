namespace IrbisUI
{
    partial class RecordViewGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._grid = new System.Windows.Forms.DataGridView();
            this._tagColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._repeatColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._textColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._buttonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
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
            this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._tagColumn,
            this._repeatColumn,
            this._textColumn,
            this._buttonColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.Size = new System.Drawing.Size(398, 261);
            this._grid.TabIndex = 0;
            // 
            // _tagColumn
            // 
            this._tagColumn.DataPropertyName = "Tag";
            this._tagColumn.HeaderText = "Tag";
            this._tagColumn.MinimumWidth = 40;
            this._tagColumn.Name = "_tagColumn";
            this._tagColumn.ReadOnly = true;
            this._tagColumn.Width = 40;
            // 
            // _repeatColumn
            // 
            this._repeatColumn.DataPropertyName = "Repeat";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this._repeatColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._repeatColumn.HeaderText = "Rep";
            this._repeatColumn.MinimumWidth = 30;
            this._repeatColumn.Name = "_repeatColumn";
            this._repeatColumn.ReadOnly = true;
            this._repeatColumn.Width = 30;
            // 
            // _textColumn
            // 
            this._textColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._textColumn.DataPropertyName = "Text";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._textColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this._textColumn.HeaderText = "Text";
            this._textColumn.MinimumWidth = 100;
            this._textColumn.Name = "_textColumn";
            this._textColumn.ReadOnly = true;
            // 
            // _buttonColumn
            // 
            this._buttonColumn.HeaderText = "Edit";
            this._buttonColumn.MinimumWidth = 40;
            this._buttonColumn.Name = "_buttonColumn";
            this._buttonColumn.ReadOnly = true;
            this._buttonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this._buttonColumn.Text = "...";
            this._buttonColumn.Width = 40;
            // 
            // RecordViewGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Name = "RecordViewGrid";
            this.Size = new System.Drawing.Size(398, 261);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _tagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _repeatColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _textColumn;
        private System.Windows.Forms.DataGridViewButtonColumn _buttonColumn;
    }
}
