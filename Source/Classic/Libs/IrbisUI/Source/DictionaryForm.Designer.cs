namespace IrbisUI
{
    partial class DictionaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DictionaryForm));
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._keyLabel = new System.Windows.Forms.Label();
            this._keyBox = new AM.Windows.Forms.EventedTextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._grid = new System.Windows.Forms.DataGridView();
            this._countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._termColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._scroll = new AM.Windows.Forms.ScrollControl();
            this._bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this._bottomPanel.Controls.Add(this._cancelButton);
            this._bottomPanel.Controls.Add(this._keyLabel);
            this._bottomPanel.Controls.Add(this._keyBox);
            this._bottomPanel.Controls.Add(this._okButton);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 253);
            this._bottomPanel.Margin = new System.Windows.Forms.Padding(2);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(468, 40);
            this._bottomPanel.TabIndex = 0;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(372, 7);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(87, 24);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _keyLabel
            // 
            this._keyLabel.AutoSize = true;
            this._keyLabel.Location = new System.Drawing.Point(113, 16);
            this._keyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(25, 13);
            this._keyLabel.TabIndex = 2;
            this._keyLabel.Text = "Key";
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Delay = 750;
            this._keyBox.Location = new System.Drawing.Point(164, 11);
            this._keyBox.Margin = new System.Windows.Forms.Padding(2);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(205, 20);
            this._keyBox.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(9, 8);
            this._okButton.Margin = new System.Windows.Forms.Padding(2);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(87, 24);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._countColumn,
            this._termColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Margin = new System.Windows.Forms.Padding(2);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(452, 253);
            this._grid.StandardTab = true;
            this._grid.TabIndex = 0;
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
            // _scroll
            // 
            this._scroll.Dock = System.Windows.Forms.DockStyle.Right;
            this._scroll.Location = new System.Drawing.Point(452, 0);
            this._scroll.Name = "_scroll";
            this._scroll.Size = new System.Drawing.Size(16, 253);
            this._scroll.TabIndex = 2;
            this._scroll.Text = "scrollControl1";
            // 
            // DictionaryForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(468, 293);
            this.Controls.Add(this._grid);
            this.Controls.Add(this._scroll);
            this.Controls.Add(this._bottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(484, 332);
            this.Name = "DictionaryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Dictionary";
            this._bottomPanel.ResumeLayout(false);
            this._bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _bottomPanel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Label _keyLabel;
        private AM.Windows.Forms.EventedTextBox _keyBox;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _countColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _termColumn;
        private AM.Windows.Forms.ScrollControl _scroll;
    }
}