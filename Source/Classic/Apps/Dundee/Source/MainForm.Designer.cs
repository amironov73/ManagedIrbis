namespace Dundee
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._foundBox = new System.Windows.Forms.ListBox();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._searchBox = new Dundee.Source.SearchBox();
            this._chart = new Dundee.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this._foundBox);
            this.splitContainer1.Panel2.Controls.Add(this._busyStripe);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 561);
            this.splitContainer1.SplitterDistance = 336;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._searchBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._chart);
            this.splitContainer2.Size = new System.Drawing.Size(1008, 336);
            this.splitContainer2.SplitterDistance = 347;
            this.splitContainer2.TabIndex = 0;
            // 
            // _foundBox
            // 
            this._foundBox.DisplayMember = "Text";
            this._foundBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._foundBox.FormattingEnabled = true;
            this._foundBox.Location = new System.Drawing.Point(0, 10);
            this._foundBox.Name = "_foundBox";
            this._foundBox.ScrollAlwaysVisible = true;
            this._foundBox.Size = new System.Drawing.Size(1008, 211);
            this._foundBox.TabIndex = 0;
            this._foundBox.DoubleClick += new System.EventHandler(this._foundBox_DoubleClick);
            // 
            // _timer
            // 
            this._timer.Interval = 10000;
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Top;
            this._busyStripe.ForeColor = System.Drawing.Color.Lime;
            this._busyStripe.Location = new System.Drawing.Point(0, 0);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(1008, 10);
            this._busyStripe.TabIndex = 1;
            this._busyStripe.Visible = false;
            // 
            // _searchBox
            // 
            this._searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._searchBox.Location = new System.Drawing.Point(0, 0);
            this._searchBox.Name = "_searchBox";
            this._searchBox.Size = new System.Drawing.Size(347, 336);
            this._searchBox.TabIndex = 0;
            // 
            // _chart
            // 
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.Enabled = false;
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(657, 336);
            this._chart.TabIndex = 0;
            this._chart.Text = "chartControl1";
            this._chart.Values = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Timer _timer;
        private Source.SearchBox _searchBox;
        private System.Windows.Forms.ListBox _foundBox;
        private ChartControl _chart;
        private AM.Windows.Forms.BusyStripe _busyStripe;
    }
}

