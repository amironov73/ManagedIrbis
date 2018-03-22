namespace IrbisBrother
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._amountBox = new AM.Windows.Forms.ToolStripNumericUpDown();
            this._printButton = new System.Windows.Forms.ToolStripButton();
            this._barcodeBox = new System.Windows.Forms.TextBox();
            this._browser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._amountBox.NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._amountBox,
            this._printButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 26);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 23);
            this.toolStripLabel1.Text = "Тираж";
            // 
            // _amountBox
            // 
            this._amountBox.Name = "_amountBox";
            // 
            // _amountBox
            // 
            this._amountBox.NumericUpDown.AccessibleName = "_amountBox";
            this._amountBox.NumericUpDown.Location = new System.Drawing.Point(52, 1);
            this._amountBox.NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._amountBox.NumericUpDown.Name = "_amountBox";
            this._amountBox.NumericUpDown.Size = new System.Drawing.Size(41, 23);
            this._amountBox.NumericUpDown.TabIndex = 1;
            this._amountBox.NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._amountBox.Size = new System.Drawing.Size(41, 23);
            this._amountBox.Text = "1";
            // 
            // _printButton
            // 
            this._printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._printButton.Image = ((System.Drawing.Image)(resources.GetObject("_printButton.Image")));
            this._printButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printButton.Name = "_printButton";
            this._printButton.Size = new System.Drawing.Size(50, 23);
            this._printButton.Text = "Печать";
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _barcodeBox
            // 
            this._barcodeBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._barcodeBox.Location = new System.Drawing.Point(0, 26);
            this._barcodeBox.Name = "_barcodeBox";
            this._barcodeBox.Size = new System.Drawing.Size(800, 20);
            this._barcodeBox.TabIndex = 1;
            this._barcodeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._barcodeBox_KeyDown);
            // 
            // _browser
            // 
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.Location = new System.Drawing.Point(0, 46);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(800, 404);
            this._browser.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._browser);
            this.Controls.Add(this._barcodeBox);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Печать расстановочного шифра";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._amountBox.NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private AM.Windows.Forms.ToolStripNumericUpDown _amountBox;
        private System.Windows.Forms.ToolStripButton _printButton;
        private System.Windows.Forms.TextBox _barcodeBox;
        private System.Windows.Forms.WebBrowser _browser;
    }
}

