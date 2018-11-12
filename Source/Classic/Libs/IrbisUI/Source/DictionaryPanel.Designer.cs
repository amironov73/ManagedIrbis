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
            this._keyBox = new System.Windows.Forms.TextBox();
            this._keyLabel = new System.Windows.Forms.Label();
            this._grid = new System.Windows.Forms.DataGridView();
            this._countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._termColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._scrollControl = new AM.Windows.Forms.ScrollControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._keyBox);
            this.panel1.Controls.Add(this._keyLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 37);
            this.panel1.TabIndex = 0;
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Location = new System.Drawing.Point(59, 12);
            this._keyBox.Margin = new System.Windows.Forms.Padding(2);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(224, 20);
            this._keyBox.TabIndex = 1;
            // 
            // _keyLabel
            // 
            this._keyLabel.Location = new System.Drawing.Point(10, 11);
            this._keyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(44, 19);
            this._keyLabel.TabIndex = 0;
            this._keyLabel.Text = "Key";
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._countColumn,
            this._termColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Margin = new System.Windows.Forms.Padding(2);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 24;
            this._grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(268, 269);
            this._grid.TabIndex = 1;
            // 
            // _countColumn
            // 
            this._countColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._countColumn.DataPropertyName = "Count";
            this._countColumn.FillWeight = 30F;
            this._countColumn.HeaderText = "Count";
            this._countColumn.MinimumWidth = 50;
            this._countColumn.Name = "_countColumn";
            this._countColumn.ReadOnly = true;
            this._countColumn.Width = 50;
            // 
            // _termColumn
            // 
            this._termColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._termColumn.DataPropertyName = "Text";
            this._termColumn.HeaderText = "Terms";
            this._termColumn.Name = "_termColumn";
            this._termColumn.ReadOnly = true;
            // 
            // _scrollControl
            // 
            this._scrollControl.Dock = System.Windows.Forms.DockStyle.Right;
            this._scrollControl.Location = new System.Drawing.Point(268, 0);
            this._scrollControl.Name = "_scrollControl";
            this._scrollControl.Size = new System.Drawing.Size(16, 269);
            this._scrollControl.TabIndex = 3;
            this._scrollControl.Text = "scrollControl1";
            // 
            // DictionaryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._scrollControl);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DictionaryPanel";
            this.Size = new System.Drawing.Size(284, 306);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn _countColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _termColumn;
        private AM.Windows.Forms.ScrollControl _scrollControl;
    }
}
