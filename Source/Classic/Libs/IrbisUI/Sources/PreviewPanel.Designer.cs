namespace IrbisUI
{
    partial class PreviewPanel
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
            this._tabControl = new System.Windows.Forms.TabControl();
            this._viewPage = new System.Windows.Forms.TabPage();
            this._relatedPage = new System.Windows.Forms.TabPage();
            this._tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._viewPage);
            this._tabControl.Controls.Add(this._relatedPage);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(600, 300);
            this._tabControl.TabIndex = 0;
            // 
            // _viewPage
            // 
            this._viewPage.Location = new System.Drawing.Point(4, 25);
            this._viewPage.Name = "_viewPage";
            this._viewPage.Padding = new System.Windows.Forms.Padding(3);
            this._viewPage.Size = new System.Drawing.Size(592, 271);
            this._viewPage.TabIndex = 0;
            this._viewPage.Text = "Description";
            this._viewPage.UseVisualStyleBackColor = true;
            // 
            // _relatedPage
            // 
            this._relatedPage.Location = new System.Drawing.Point(4, 25);
            this._relatedPage.Name = "_relatedPage";
            this._relatedPage.Padding = new System.Windows.Forms.Padding(3);
            this._relatedPage.Size = new System.Drawing.Size(592, 271);
            this._relatedPage.TabIndex = 1;
            this._relatedPage.Text = "Related documents";
            this._relatedPage.UseVisualStyleBackColor = true;
            // 
            // PreviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tabControl);
            this.Name = "PreviewPanel";
            this.Size = new System.Drawing.Size(600, 300);
            this._tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _viewPage;
        private System.Windows.Forms.TabPage _relatedPage;
    }
}
