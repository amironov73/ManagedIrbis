namespace BookTerminator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._mfnBox = new System.Windows.Forms.TextBox();
            this._findButton = new System.Windows.Forms.Button();
            this._deleteButton = new System.Windows.Forms.Button();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._busyStripe = new IrbisUI.IrbisBusyStripe();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mfnBox
            // 
            this._mfnBox.Location = new System.Drawing.Point(12, 28);
            this._mfnBox.Name = "_mfnBox";
            this._mfnBox.Size = new System.Drawing.Size(230, 22);
            this._mfnBox.TabIndex = 0;
            this._mfnBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._mfnBox_KeyDown);
            // 
            // _findButton
            // 
            this._findButton.Location = new System.Drawing.Point(260, 20);
            this._findButton.Name = "_findButton";
            this._findButton.Size = new System.Drawing.Size(167, 39);
            this._findButton.TabIndex = 1;
            this._findButton.Text = "Find";
            this._findButton.UseVisualStyleBackColor = true;
            this._findButton.Click += new System.EventHandler(this._findButton_Click);
            // 
            // _deleteButton
            // 
            this._deleteButton.Location = new System.Drawing.Point(433, 20);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(237, 39);
            this._deleteButton.TabIndex = 2;
            this._deleteButton.Text = "Delete";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Location = new System.Drawing.Point(0, 17);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(690, 123);
            this._logBox.TabIndex = 4;
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Top;
            this._busyStripe.ForeColor = System.Drawing.Color.Lime;
            this._busyStripe.Location = new System.Drawing.Point(0, 0);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(690, 17);
            this._busyStripe.TabIndex = 5;
            // 
            // _browser
            // 
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(690, 144);
            this._browser.TabIndex = 3;
            // 
            // _splitContainer
            // 
            this._splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._splitContainer.Location = new System.Drawing.Point(0, 70);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._browser);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._logBox);
            this._splitContainer.Panel2.Controls.Add(this._busyStripe);
            this._splitContainer.Size = new System.Drawing.Size(690, 288);
            this._splitContainer.SplitterDistance = 144;
            this._splitContainer.TabIndex = 6;
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Interval = 60000;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // MainForm
            // 
            this.AcceptButton = this._findButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 353);
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._deleteButton);
            this.Controls.Add(this._findButton);
            this.Controls.Add(this._mfnBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "MainForm";
            this.Text = "BookTerminator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _mfnBox;
        private System.Windows.Forms.Button _findButton;
        private System.Windows.Forms.Button _deleteButton;
        private AM.Windows.Forms.LogBox _logBox;
        private IrbisUI.IrbisBusyStripe _busyStripe;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.Timer _timer;
    }
}

