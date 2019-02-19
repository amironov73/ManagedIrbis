namespace BeriChitai
{
    partial class BeriPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BeriPanel));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._readerList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this._readerBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._phoneBox = new System.Windows.Forms.ToolStripTextBox();
            this._copyPhoneButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this._mailBox = new System.Windows.Forms.ToolStripTextBox();
            this._copyMailButton = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._bookList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this._bookBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._giveButton = new System.Windows.Forms.ToolStripButton();
            this._rejectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._refreshButton = new System.Windows.Forms.ToolStripButton();
            this._readerSource = new System.Windows.Forms.BindingSource(this.components);
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this._bookSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._readerSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bookSource)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(884, 547);
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._readerList);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._readerBrowser);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(884, 366);
            this.splitContainer2.SplitterDistance = 468;
            this.splitContainer2.TabIndex = 0;
            // 
            // _readerList
            // 
            this._readerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._readerList.FormattingEnabled = true;
            this._readerList.Location = new System.Drawing.Point(0, 16);
            this._readerList.Name = "_readerList";
            this._readerList.ScrollAlwaysVisible = true;
            this._readerList.Size = new System.Drawing.Size(466, 348);
            this._readerList.TabIndex = 0;
            this._readerList.SelectedIndexChanged += new System.EventHandler(this._readerList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(466, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Читатели, оставившие заказы";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _readerBrowser
            // 
            this._readerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._readerBrowser.Location = new System.Drawing.Point(0, 16);
            this._readerBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._readerBrowser.Name = "_readerBrowser";
            this._readerBrowser.Size = new System.Drawing.Size(410, 348);
            this._readerBrowser.TabIndex = 1;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._phoneBox,
            this._copyPhoneButton,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this._mailBox,
            this._copyMailButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 150);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(466, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "Тел.";
            // 
            // _phoneBox
            // 
            this._phoneBox.Name = "_phoneBox";
            this._phoneBox.ReadOnly = true;
            this._phoneBox.Size = new System.Drawing.Size(80, 25);
            // 
            // _copyPhoneButton
            // 
            this._copyPhoneButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyPhoneButton.Image = ((System.Drawing.Image)(resources.GetObject("_copyPhoneButton.Image")));
            this._copyPhoneButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._copyPhoneButton.Name = "_copyPhoneButton";
            this._copyPhoneButton.Size = new System.Drawing.Size(23, 22);
            this._copyPhoneButton.Text = "Копировать";
            this._copyPhoneButton.Click += new System.EventHandler(this._copyPhoneButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel2.Text = "Mail";
            // 
            // _mailBox
            // 
            this._mailBox.Name = "_mailBox";
            this._mailBox.ReadOnly = true;
            this._mailBox.Size = new System.Drawing.Size(200, 25);
            // 
            // _copyMailButton
            // 
            this._copyMailButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyMailButton.Image = ((System.Drawing.Image)(resources.GetObject("_copyMailButton.Image")));
            this._copyMailButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._copyMailButton.Name = "_copyMailButton";
            this._copyMailButton.Size = new System.Drawing.Size(23, 22);
            this._copyMailButton.Text = "Копировать";
            this._copyMailButton.Click += new System.EventHandler(this._copyMailButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(410, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Сведения о читателе";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._bookList);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            this.splitContainer3.Panel1.Controls.Add(this.toolStrip2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._bookBrowser);
            this.splitContainer3.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer3.Size = new System.Drawing.Size(884, 177);
            this.splitContainer3.SplitterDistance = 468;
            this.splitContainer3.TabIndex = 0;
            // 
            // _bookList
            // 
            this._bookList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bookList.FormattingEnabled = true;
            this._bookList.Location = new System.Drawing.Point(0, 16);
            this._bookList.Name = "_bookList";
            this._bookList.ScrollAlwaysVisible = true;
            this._bookList.Size = new System.Drawing.Size(466, 134);
            this._bookList.TabIndex = 1;
            this._bookList.SelectedIndexChanged += new System.EventHandler(this._bookList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(466, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Книги, заказанные читателем";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _bookBrowser
            // 
            this._bookBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bookBrowser.Location = new System.Drawing.Point(0, 25);
            this._bookBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._bookBrowser.Name = "_bookBrowser";
            this._bookBrowser.Size = new System.Drawing.Size(410, 150);
            this._bookBrowser.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._giveButton,
            this._rejectButton,
            this.toolStripSeparator1,
            this._refreshButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(410, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _giveButton
            // 
            this._giveButton.Image = ((System.Drawing.Image)(resources.GetObject("_giveButton.Image")));
            this._giveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._giveButton.Name = "_giveButton";
            this._giveButton.Size = new System.Drawing.Size(66, 22);
            this._giveButton.Text = "Выдать";
            this._giveButton.Click += new System.EventHandler(this._giveButton_Click);
            // 
            // _rejectButton
            // 
            this._rejectButton.Image = ((System.Drawing.Image)(resources.GetObject("_rejectButton.Image")));
            this._rejectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._rejectButton.Name = "_rejectButton";
            this._rejectButton.Size = new System.Drawing.Size(112, 22);
            this._rejectButton.Text = "Отменить заказ";
            this._rejectButton.Click += new System.EventHandler(this._rejectButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _refreshButton
            // 
            this._refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("_refreshButton.Image")));
            this._refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._refreshButton.Name = "_refreshButton";
            this._refreshButton.Size = new System.Drawing.Size(125, 22);
            this._refreshButton.Text = "Обновить данные";
            this._refreshButton.Click += new System.EventHandler(this._refreshButton_Click);
            // 
            // _readerSource
            // 
            this._readerSource.AllowNew = false;
            this._readerSource.CurrentChanged += new System.EventHandler(this._readerSource_CurrentChanged);
            // 
            // _timer1
            // 
            this._timer1.Tick += new System.EventHandler(this._timer1_Tick);
            // 
            // _bookSource
            // 
            this._bookSource.AllowNew = false;
            // 
            // BeriPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BeriPanel";
            this.Size = new System.Drawing.Size(884, 547);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._readerSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bookSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.BindingSource _readerSource;
        private System.Windows.Forms.ListBox _readerList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.WebBrowser _readerBrowser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox _bookList;
        private System.Windows.Forms.WebBrowser _bookBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Timer _timer1;
        private System.Windows.Forms.BindingSource _bookSource;
        private System.Windows.Forms.ToolStripButton _giveButton;
        private System.Windows.Forms.ToolStripButton _rejectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _refreshButton;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox _phoneBox;
        private System.Windows.Forms.ToolStripButton _copyPhoneButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox _mailBox;
        private System.Windows.Forms.ToolStripButton _copyMailButton;
    }
}
