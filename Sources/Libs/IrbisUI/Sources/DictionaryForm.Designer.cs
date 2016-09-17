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
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._keyLabel = new System.Windows.Forms.Label();
            this._keyBox = new System.Windows.Forms.TextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._grid = new System.Windows.Forms.DataGridView();
            this._countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._termColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancelButton);
            this.panel1.Controls.Add(this._keyLabel);
            this.panel1.Controls.Add(this._keyBox);
            this.panel1.Controls.Add(this._okButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 49);
            this.panel1.TabIndex = 0;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(494, 9);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(116, 30);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _keyLabel
            // 
            this._keyLabel.AutoSize = true;
            this._keyLabel.Location = new System.Drawing.Point(151, 20);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(32, 17);
            this._keyLabel.TabIndex = 2;
            this._keyLabel.Text = "Key";
            // 
            // _keyBox
            // 
            this._keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyBox.Location = new System.Drawing.Point(219, 13);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(269, 22);
            this._keyBox.TabIndex = 1;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(12, 10);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(116, 29);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
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
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(622, 304);
            this._grid.TabIndex = 1;
            // 
            // _countColumn
            // 
            this._countColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._countColumn.DataPropertyName = "Count";
            this._countColumn.FillWeight = 30F;
            this._countColumn.HeaderText = "Count";
            this._countColumn.MinimumWidth = 30;
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
            // DictionaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 353);
            this.Controls.Add(this._grid);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "DictionaryForm";
            this.Text = "Dictionary";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _countColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _termColumn;
    }
}