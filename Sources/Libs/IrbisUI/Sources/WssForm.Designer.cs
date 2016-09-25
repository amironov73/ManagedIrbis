namespace IrbisUI
{
    partial class WssForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WssForm));
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._hintPanel = new System.Windows.Forms.Panel();
            this._hintLabel = new System.Windows.Forms.Label();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButto = new System.Windows.Forms.Button();
            this._grid = new System.Windows.Forms.DataGridView();
            this._titleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bottomPanel.SuspendLayout();
            this._hintPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _bottomPanel
            // 
            this._bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._bottomPanel.Controls.Add(this._cancelButto);
            this._bottomPanel.Controls.Add(this._okButton);
            this._bottomPanel.Controls.Add(this._hintPanel);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 283);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(622, 70);
            this._bottomPanel.TabIndex = 0;
            // 
            // _hintPanel
            // 
            this._hintPanel.Controls.Add(this._hintLabel);
            this._hintPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._hintPanel.Location = new System.Drawing.Point(0, 0);
            this._hintPanel.Name = "_hintPanel";
            this._hintPanel.Size = new System.Drawing.Size(620, 27);
            this._hintPanel.TabIndex = 0;
            // 
            // _hintLabel
            // 
            this._hintLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._hintLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hintLabel.Location = new System.Drawing.Point(0, 0);
            this._hintLabel.Name = "_hintLabel";
            this._hintLabel.Size = new System.Drawing.Size(620, 27);
            this._hintLabel.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(11, 33);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(100, 30);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButto
            // 
            this._cancelButto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButto.Location = new System.Drawing.Point(509, 33);
            this._cancelButto.Name = "_cancelButto";
            this._cancelButto.Size = new System.Drawing.Size(100, 30);
            this._cancelButto.TabIndex = 1;
            this._cancelButto.Text = "&Cancel";
            this._cancelButto.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._titleColumn,
            this._valueColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.DefaultCellStyle = dataGridViewCellStyle2;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 24;
            this._grid.Size = new System.Drawing.Size(622, 283);
            this._grid.TabIndex = 1;
            // 
            // _titleColumn
            // 
            this._titleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._titleColumn.DataPropertyName = "Title";
            this._titleColumn.HeaderText = "Subfield";
            this._titleColumn.Name = "_titleColumn";
            this._titleColumn.ReadOnly = true;
            // 
            // _valueColumn
            // 
            this._valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._valueColumn.DataPropertyName = "Value";
            this._valueColumn.HeaderText = "Value";
            this._valueColumn.Name = "_valueColumn";
            // 
            // WssForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 353);
            this.Controls.Add(this._grid);
            this.Controls.Add(this._bottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "WssForm";
            this.Text = "Field";
            this._bottomPanel.ResumeLayout(false);
            this._hintPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _bottomPanel;
        private System.Windows.Forms.Panel _hintPanel;
        private System.Windows.Forms.Label _hintLabel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButto;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _titleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
    }
}