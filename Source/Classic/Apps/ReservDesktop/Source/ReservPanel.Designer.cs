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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._comboBox = new System.Windows.Forms.ComboBox();
            this._gridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(639, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = false;
            // 
            // _comboBox
            // 
            this._comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._comboBox.FormattingEnabled = true;
            this._comboBox.Location = new System.Drawing.Point(0, 25);
            this._comboBox.Name = "_comboBox";
            this._comboBox.Size = new System.Drawing.Size(639, 21);
            this._comboBox.TabIndex = 1;
            // 
            // _gridView
            // 
            this._gridView.AllowUserToAddRows = false;
            this._gridView.AllowUserToDeleteRows = false;
            this._gridView.AutoGenerateColumns = false;
            this._gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridView.DataSource = this._bindingSource;
            this._gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridView.Location = new System.Drawing.Point(0, 46);
            this._gridView.Name = "_gridView";
            this._gridView.ReadOnly = true;
            this._gridView.Size = new System.Drawing.Size(639, 425);
            this._gridView.TabIndex = 2;
            // 
            // ReservPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gridView);
            this.Controls.Add(this._comboBox);
            this.Controls.Add(this._toolStrip);
            this.Name = "ReservPanel";
            this.Size = new System.Drawing.Size(639, 471);
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.ComboBox _comboBox;
        private System.Windows.Forms.DataGridView _gridView;
    }
}
