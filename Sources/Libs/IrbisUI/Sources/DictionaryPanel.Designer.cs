namespace IrbisUI
{
    partial class DictionaryPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._keyLabel = new System.Windows.Forms.Label();
            this._keyBox = new System.Windows.Forms.TextBox();
            this._grid = new System.Windows.Forms.DataGridView();
            this._scrollBar = new System.Windows.Forms.VScrollBar();
            this._countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._termColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._keyBox);
            this.panel1.Controls.Add(this._keyLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 330);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 46);
            this.panel1.TabIndex = 0;
            // 
            // _keyLabel
            // 
            this._keyLabel.Location = new System.Drawing.Point(14, 14);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(59, 23);
            this._keyLabel.TabIndex = 0;
            this._keyLabel.Text = "Key";
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Location = new System.Drawing.Point(79, 15);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(297, 22);
            this._keyBox.TabIndex = 1;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._countColumn,
            this._termColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 24;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(358, 330);
            this._grid.TabIndex = 1;
            // 
            // _scrollBar
            // 
            this._scrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this._scrollBar.Location = new System.Drawing.Point(358, 0);
            this._scrollBar.Name = "_scrollBar";
            this._scrollBar.Size = new System.Drawing.Size(21, 330);
            this._scrollBar.TabIndex = 2;
            // 
            // _countColumn
            // 
            this._countColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._countColumn.DataPropertyName = "Count";
            this._countColumn.FillWeight = 30F;
            this._countColumn.HeaderText = "Count";
            this._countColumn.Name = "_countColumn";
            this._countColumn.ReadOnly = true;
            // 
            // _termColumn
            // 
            this._termColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._termColumn.DataPropertyName = "Text";
            this._termColumn.HeaderText = "Terms";
            this._termColumn.Name = "_termColumn";
            this._termColumn.ReadOnly = true;
            // 
            // DictionaryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._scrollBar);
            this.Controls.Add(this.panel1);
            this.Name = "DictionaryPanel";
            this.Size = new System.Drawing.Size(379, 376);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.VScrollBar _scrollBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn _countColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _termColumn;
    }
}
