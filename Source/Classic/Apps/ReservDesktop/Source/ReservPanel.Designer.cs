namespace ReservDesktop
{
    partial class ReservPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReservPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._connectButton = new System.Windows.Forms.ToolStripButton();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._roomBox = new System.Windows.Forms.ComboBox();
            this._grid = new System.Windows.Forms.DataGridView();
            this._numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._connectButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(639, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _connectButton
            // 
            this._connectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._connectButton.Image = ((System.Drawing.Image)(resources.GetObject("_connectButton.Image")));
            this._connectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new System.Drawing.Size(120, 22);
            this._connectButton.Text = "Подключиться к БД";
            this._connectButton.Click += new System.EventHandler(this._connectButton_Click);
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = false;
            // 
            // _roomBox
            // 
            this._roomBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._roomBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._roomBox.FormattingEnabled = true;
            this._roomBox.Location = new System.Drawing.Point(0, 25);
            this._roomBox.Name = "_roomBox";
            this._roomBox.Size = new System.Drawing.Size(639, 21);
            this._roomBox.TabIndex = 1;
            this._roomBox.SelectedIndexChanged += new System.EventHandler(this._roomBox_SelectedIndexChanged);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.AutoGenerateColumns = false;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._numberColumn,
            this._statusColumn,
            this._descriptionColumn});
            this._grid.DataSource = this._bindingSource;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 46);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(639, 425);
            this._grid.TabIndex = 2;
            this._grid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this._grid_RowPrePaint);
            // 
            // _numberColumn
            // 
            this._numberColumn.DataPropertyName = "Number";
            this._numberColumn.HeaderText = "№";
            this._numberColumn.Name = "_numberColumn";
            this._numberColumn.ReadOnly = true;
            this._numberColumn.Width = 30;
            // 
            // _statusColumn
            // 
            this._statusColumn.DataPropertyName = "Status";
            this._statusColumn.HeaderText = "Статус";
            this._statusColumn.Name = "_statusColumn";
            this._statusColumn.ReadOnly = true;
            this._statusColumn.Width = 50;
            // 
            // _descriptionColumn
            // 
            this._descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._descriptionColumn.DataPropertyName = "Description";
            this._descriptionColumn.HeaderText = "Описание";
            this._descriptionColumn.Name = "_descriptionColumn";
            this._descriptionColumn.ReadOnly = true;
            // 
            // ReservPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._roomBox);
            this.Controls.Add(this._toolStrip);
            this.Name = "ReservPanel";
            this.Size = new System.Drawing.Size(639, 471);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.ComboBox _roomBox;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _numberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _statusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _descriptionColumn;
        private System.Windows.Forms.ToolStripButton _connectButton;
    }
}
