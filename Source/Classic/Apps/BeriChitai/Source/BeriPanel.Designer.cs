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
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._bookList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this._bookBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._readerSource = new System.Windows.Forms.BindingSource(this.components);
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this._bookSource = new System.Windows.Forms.BindingSource(this.components);
            this._giveButton = new System.Windows.Forms.ToolStripButton();
            this._rejectButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(808, 547);
            this.splitContainer1.SplitterDistance = 269;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer2.Size = new System.Drawing.Size(808, 269);
            this.splitContainer2.SplitterDistance = 358;
            this.splitContainer2.TabIndex = 0;
            // 
            // _readerList
            // 
            this._readerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._readerList.FormattingEnabled = true;
            this._readerList.Location = new System.Drawing.Point(0, 16);
            this._readerList.Name = "_readerList";
            this._readerList.ScrollAlwaysVisible = true;
            this._readerList.Size = new System.Drawing.Size(356, 251);
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
            this.label1.Size = new System.Drawing.Size(356, 16);
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
            this._readerBrowser.Size = new System.Drawing.Size(444, 251);
            this._readerBrowser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(444, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Сведения о читателе";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._bookList);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._bookBrowser);
            this.splitContainer3.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer3.Size = new System.Drawing.Size(808, 274);
            this.splitContainer3.SplitterDistance = 358;
            this.splitContainer3.TabIndex = 0;
            // 
            // _bookList
            // 
            this._bookList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bookList.FormattingEnabled = true;
            this._bookList.Location = new System.Drawing.Point(0, 16);
            this._bookList.Name = "_bookList";
            this._bookList.ScrollAlwaysVisible = true;
            this._bookList.Size = new System.Drawing.Size(356, 256);
            this._bookList.TabIndex = 1;
            this._bookList.SelectedIndexChanged += new System.EventHandler(this._bookList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(356, 16);
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
            this._bookBrowser.Size = new System.Drawing.Size(444, 247);
            this._bookBrowser.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._giveButton,
            this._rejectButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(444, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _readerSource
            // 
            this._readerSource.AllowNew = false;
            // 
            // _timer1
            // 
            this._timer1.Tick += new System.EventHandler(this._timer1_Tick);
            // 
            // _bookSource
            // 
            this._bookSource.AllowNew = false;
            // 
            // _giveButton
            // 
            this._giveButton.Image = ((System.Drawing.Image)(resources.GetObject("_giveButton.Image")));
            this._giveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._giveButton.Name = "_giveButton";
            this._giveButton.Size = new System.Drawing.Size(66, 22);
            this._giveButton.Text = "Выдать";
            // 
            // _rejectButton
            // 
            this._rejectButton.Image = ((System.Drawing.Image)(resources.GetObject("_rejectButton.Image")));
            this._rejectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._rejectButton.Name = "_rejectButton";
            this._rejectButton.Size = new System.Drawing.Size(112, 22);
            this._rejectButton.Text = "Отменить заказ";
            // 
            // BeriPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BeriPanel";
            this.Size = new System.Drawing.Size(808, 547);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
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
    }
}
