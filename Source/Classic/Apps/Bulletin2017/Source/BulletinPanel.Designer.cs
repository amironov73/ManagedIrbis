namespace Bulletin2017
{
    partial class BulletinPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BulletinPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._prevMonthButton = new System.Windows.Forms.ToolStripButton();
            this._periodLabel = new System.Windows.Forms.ToolStripLabel();
            this._nextMonthButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._prevMonthButton,
            this._periodLabel,
            this._nextMonthButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(860, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _prevMonthButton
            // 
            this._prevMonthButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._prevMonthButton.Image = ((System.Drawing.Image)(resources.GetObject("_prevMonthButton.Image")));
            this._prevMonthButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._prevMonthButton.Name = "_prevMonthButton";
            this._prevMonthButton.Size = new System.Drawing.Size(23, 22);
            this._prevMonthButton.Text = "toolStripButton1";
            this._prevMonthButton.Click += new System.EventHandler(this._prevMonthButton_Click);
            // 
            // _periodLabel
            // 
            this._periodLabel.Name = "_periodLabel";
            this._periodLabel.Size = new System.Drawing.Size(86, 22);
            this._periodLabel.Text = "toolStripLabel1";
            // 
            // _nextMonthButton
            // 
            this._nextMonthButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._nextMonthButton.Image = ((System.Drawing.Image)(resources.GetObject("_nextMonthButton.Image")));
            this._nextMonthButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._nextMonthButton.Name = "_nextMonthButton";
            this._nextMonthButton.Size = new System.Drawing.Size(23, 22);
            this._nextMonthButton.Text = "toolStripButton2";
            this._nextMonthButton.Click += new System.EventHandler(this._nextMonthButton_Click);
            // 
            // BulletinPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "BulletinPanel";
            this.Size = new System.Drawing.Size(860, 511);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _prevMonthButton;
        private System.Windows.Forms.ToolStripLabel _periodLabel;
        private System.Windows.Forms.ToolStripButton _nextMonthButton;
    }
}
