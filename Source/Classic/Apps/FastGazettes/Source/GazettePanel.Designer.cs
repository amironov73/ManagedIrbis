namespace FastGazettes
{
    partial class GazettePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GazettePanel));
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._magazineBox = new System.Windows.Forms.ListBox();
            this._magazineSource = new System.Windows.Forms.BindingSource(this.components);
            this._keyStrip = new System.Windows.Forms.ToolStrip();
            this._keyBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this._fondBox = new System.Windows.Forms.ToolStripComboBox();
            this._richTextBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._yearBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this._numberBox = new System.Windows.Forms.ToolStripTextBox();
            this._goButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this._statusBox = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._magazineSource)).BeginInit();
            this._keyStrip.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _timer1
            // 
            this._timer1.Tick += new System.EventHandler(this._timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._magazineBox);
            this.splitContainer1.Panel1.Controls.Add(this._keyStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Panel2.Controls.Add(this._richTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(771, 384);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 0;
            // 
            // _magazineBox
            // 
            this._magazineBox.DataSource = this._magazineSource;
            this._magazineBox.DisplayMember = "Title";
            this._magazineBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._magazineBox.FormattingEnabled = true;
            this._magazineBox.IntegralHeight = false;
            this._magazineBox.Location = new System.Drawing.Point(0, 0);
            this._magazineBox.Name = "_magazineBox";
            this._magazineBox.Size = new System.Drawing.Size(243, 359);
            this._magazineBox.TabIndex = 0;
            this._magazineBox.SelectedIndexChanged += new System.EventHandler(this._magazineBox_SelectedIndexChanged);
            this._magazineBox.SizeChanged += new System.EventHandler(this._magazineBox_SizeChanged);
            // 
            // _magazineSource
            // 
            this._magazineSource.AllowNew = false;
            this._magazineSource.DataSource = typeof(ManagedIrbis.Magazines.MagazineInfo);
            // 
            // _keyStrip
            // 
            this._keyStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._keyStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._keyStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._keyBox});
            this._keyStrip.Location = new System.Drawing.Point(0, 359);
            this._keyStrip.Name = "_keyStrip";
            this._keyStrip.Size = new System.Drawing.Size(243, 25);
            this._keyStrip.Stretch = true;
            this._keyStrip.TabIndex = 1;
            this._keyStrip.Text = "toolStrip2";
            // 
            // _keyBox
            // 
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(200, 25);
            this._keyBox.TextChanged += new System.EventHandler(this._keyBox_TextChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this._fondBox,
            this.toolStripLabel4,
            this._statusBox});
            this.toolStrip2.Location = new System.Drawing.Point(0, 334);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(524, 25);
            this.toolStrip2.Stretch = true;
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel3.Text = "Фонд";
            // 
            // _fondBox
            // 
            this._fondBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._fondBox.Name = "_fondBox";
            this._fondBox.Size = new System.Drawing.Size(300, 25);
            // 
            // _richTextBox
            // 
            this._richTextBox.BackColor = System.Drawing.SystemColors.Window;
            this._richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richTextBox.Location = new System.Drawing.Point(0, 0);
            this._richTextBox.Name = "_richTextBox";
            this._richTextBox.ReadOnly = true;
            this._richTextBox.Size = new System.Drawing.Size(524, 359);
            this._richTextBox.TabIndex = 1;
            this._richTextBox.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._yearBox,
            this.toolStripLabel2,
            this._numberBox,
            this._goButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 359);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(524, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(26, 22);
            this.toolStripLabel1.Text = "Год";
            // 
            // _yearBox
            // 
            this._yearBox.Name = "_yearBox";
            this._yearBox.Size = new System.Drawing.Size(50, 25);
            this._yearBox.Text = "1979";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(51, 22);
            this.toolStripLabel2.Text = "Номера";
            // 
            // _numberBox
            // 
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(150, 25);
            this._numberBox.Text = "1-5,10-20";
            // 
            // _goButton
            // 
            this._goButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._goButton.Image = ((System.Drawing.Image)(resources.GetObject("_goButton.Image")));
            this._goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(111, 22);
            this._goButton.Text = "Зарегистрировать";
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel4.Text = "Статус";
            // 
            // _statusBox
            // 
            this._statusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._statusBox.Name = "_statusBox";
            this._statusBox.Size = new System.Drawing.Size(121, 25);
            // 
            // GazettePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GazettePanel";
            this.Size = new System.Drawing.Size(771, 384);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._magazineSource)).EndInit();
            this._keyStrip.ResumeLayout(false);
            this._keyStrip.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer _timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource _magazineSource;
        private System.Windows.Forms.ListBox _magazineBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.RichTextBox _richTextBox;
        private System.Windows.Forms.ToolStrip _keyStrip;
        private System.Windows.Forms.ToolStripTextBox _keyBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox _yearBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox _numberBox;
        private System.Windows.Forms.ToolStripButton _goButton;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox _fondBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox _statusBox;
    }
}
