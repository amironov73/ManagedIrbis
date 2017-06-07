namespace CniInvent
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._rfidBox = new System.Windows.Forms.TextBox();
            this._grid = new System.Windows.Forms.DataGridView();
            this.inventoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._descriptionBox = new System.Windows.Forms.TextBox();
            this._indicatorPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._placeBox = new System.Windows.Forms.TextBox();
            this._confirmButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._irbisBusyStripe = new IrbisUI.IrbisBusyStripe();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._reportButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._logBox);
            this.splitContainer1.Panel2.Controls.Add(this._irbisBusyStripe);
            this.splitContainer1.Size = new System.Drawing.Size(784, 536);
            this.splitContainer1.SplitterDistance = 389;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this._rfidBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._grid, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 389);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _rfidBox
            // 
            this._rfidBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rfidBox.Location = new System.Drawing.Point(23, 59);
            this._rfidBox.Name = "_rfidBox";
            this._rfidBox.Size = new System.Drawing.Size(738, 20);
            this._rfidBox.TabIndex = 0;
            this._rfidBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._rfidBox_KeyDown);
            this._rfidBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._rfidBox_PreviewKeyDown);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.AutoGenerateColumns = false;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inventoryDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this._grid.DataSource = this._bindingSource;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(23, 225);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.Size = new System.Drawing.Size(738, 141);
            this._grid.TabIndex = 2;
            // 
            // inventoryDataGridViewTextBoxColumn
            // 
            this.inventoryDataGridViewTextBoxColumn.DataPropertyName = "Inventory";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.inventoryDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.inventoryDataGridViewTextBoxColumn.HeaderText = "Инв. №";
            this.inventoryDataGridViewTextBoxColumn.Name = "inventoryDataGridViewTextBoxColumn";
            this.inventoryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Библиографическое описание";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = false;
            this._bindingSource.DataSource = typeof(CniInvent.StatusRecord);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this._descriptionBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._indicatorPanel, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(23, 105);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(738, 94);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // _descriptionBox
            // 
            this._descriptionBox.BackColor = System.Drawing.SystemColors.Window;
            this._descriptionBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._descriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._descriptionBox.Location = new System.Drawing.Point(3, 3);
            this._descriptionBox.Multiline = true;
            this._descriptionBox.Name = "_descriptionBox";
            this._descriptionBox.ReadOnly = true;
            this._descriptionBox.Size = new System.Drawing.Size(612, 91);
            this._descriptionBox.TabIndex = 1;
            // 
            // _indicatorPanel
            // 
            this._indicatorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._indicatorPanel.Location = new System.Drawing.Point(641, 3);
            this._indicatorPanel.Name = "_indicatorPanel";
            this._indicatorPanel.Size = new System.Drawing.Size(94, 91);
            this._indicatorPanel.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this._placeBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this._confirmButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(23, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(738, 30);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // _placeBox
            // 
            this._placeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._placeBox.Location = new System.Drawing.Point(117, 3);
            this._placeBox.Name = "_placeBox";
            this._placeBox.Size = new System.Drawing.Size(306, 20);
            this._placeBox.TabIndex = 0;
            // 
            // _confirmButton
            // 
            this._confirmButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._confirmButton.Location = new System.Drawing.Point(429, 3);
            this._confirmButton.Name = "_confirmButton";
            this._confirmButton.Size = new System.Drawing.Size(306, 24);
            this._confirmButton.TabIndex = 1;
            this._confirmButton.Text = "Подтвердить";
            this._confirmButton.UseVisualStyleBackColor = true;
            this._confirmButton.Click += new System.EventHandler(this._confirmButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Проверяемый фонд";
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Location = new System.Drawing.Point(0, 20);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(784, 123);
            this._logBox.TabIndex = 1;
            // 
            // _irbisBusyStripe
            // 
            this._irbisBusyStripe.Dock = System.Windows.Forms.DockStyle.Top;
            this._irbisBusyStripe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._irbisBusyStripe.Location = new System.Drawing.Point(0, 0);
            this._irbisBusyStripe.Moving = false;
            this._irbisBusyStripe.Name = "_irbisBusyStripe";
            this._irbisBusyStripe.Size = new System.Drawing.Size(784, 20);
            this._irbisBusyStripe.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(784, 536);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(784, 561);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._reportButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(784, 25);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 0;
            // 
            // _reportButton
            // 
            this._reportButton.Image = ((System.Drawing.Image)(resources.GetObject("_reportButton.Image")));
            this._reportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._reportButton.Name = "_reportButton";
            this._reportButton.Size = new System.Drawing.Size(147, 22);
            this._reportButton.Text = "Список недостающих";
            this._reportButton.Click += new System.EventHandler(this._reportButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Инвентаризация фонда";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private IrbisUI.IrbisBusyStripe _irbisBusyStripe;
        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _rfidBox;
        private System.Windows.Forms.TextBox _descriptionBox;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel _indicatorPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox _placeBox;
        private System.Windows.Forms.Button _confirmButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn inventoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _reportButton;
    }
}

