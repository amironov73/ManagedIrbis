namespace IrbisUI
{
    partial class FoundPanel
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
            this._topPanel = new System.Windows.Forms.Panel();
            this._sortBox = new System.Windows.Forms.ComboBox();
            this._grid = new System.Windows.Forms.DataGridView();
            this._mfnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._selectionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this._descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _topPanel
            // 
            this._topPanel.Controls.Add(this._sortBox);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(500, 24);
            this._topPanel.TabIndex = 0;
            // 
            // _sortBox
            // 
            this._sortBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sortBox.FormattingEnabled = true;
            this._sortBox.Location = new System.Drawing.Point(0, 0);
            this._sortBox.Name = "_sortBox";
            this._sortBox.Size = new System.Drawing.Size(178, 24);
            this._sortBox.TabIndex = 0;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._mfnColumn,
            this._selectionColumn,
            this._iconColumn,
            this._descriptionColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 24);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 24;
            this._grid.Size = new System.Drawing.Size(500, 176);
            this._grid.TabIndex = 1;
            // 
            // _mfnColumn
            // 
            this._mfnColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._mfnColumn.DataPropertyName = "Mfn";
            this._mfnColumn.HeaderText = "MFN";
            this._mfnColumn.MinimumWidth = 70;
            this._mfnColumn.Name = "_mfnColumn";
            this._mfnColumn.ReadOnly = true;
            this._mfnColumn.Width = 70;
            // 
            // _selectionColumn
            // 
            this._selectionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._selectionColumn.DataPropertyName = "Selected";
            this._selectionColumn.HeaderText = "";
            this._selectionColumn.MinimumWidth = 30;
            this._selectionColumn.Name = "_selectionColumn";
            this._selectionColumn.ReadOnly = true;
            this._selectionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this._selectionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._selectionColumn.Width = 30;
            // 
            // _iconColumn
            // 
            this._iconColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._iconColumn.DataPropertyName = "Icon";
            this._iconColumn.HeaderText = "";
            this._iconColumn.MinimumWidth = 30;
            this._iconColumn.Name = "_iconColumn";
            this._iconColumn.ReadOnly = true;
            this._iconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this._iconColumn.Width = 30;
            // 
            // _descriptionColumn
            // 
            this._descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._descriptionColumn.DataPropertyName = "Description";
            this._descriptionColumn.HeaderText = "Description";
            this._descriptionColumn.Name = "_descriptionColumn";
            this._descriptionColumn.ReadOnly = true;
            this._descriptionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._descriptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FoundPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._topPanel);
            this.Name = "FoundPanel";
            this.Size = new System.Drawing.Size(500, 200);
            this._topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.ComboBox _sortBox;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _mfnColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _selectionColumn;
        private System.Windows.Forms.DataGridViewImageColumn _iconColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _descriptionColumn;
    }
}
