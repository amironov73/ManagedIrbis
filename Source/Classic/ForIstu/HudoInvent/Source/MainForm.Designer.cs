
namespace HudoInvent
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._barcodeBox = new System.Windows.Forms.TextBox();
            this._checkButton = new System.Windows.Forms.Button();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._bookBrowser = new System.Windows.Forms.WebBrowser();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._counterBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this._confirmButton = new System.Windows.Forms.Button();
            this._reportButton = new System.Windows.Forms.Button();
            this._missingButton = new System.Windows.Forms.Button();
            this._idleTimer = new System.Windows.Forms.Timer(this.components);
            this._saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._logBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._bookBrowser, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._barcodeBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._checkButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(794, 29);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // _barcodeBox
            // 
            this._barcodeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._barcodeBox.Location = new System.Drawing.Point(3, 3);
            this._barcodeBox.Name = "_barcodeBox";
            this._barcodeBox.Size = new System.Drawing.Size(707, 20);
            this._barcodeBox.TabIndex = 0;
            this._barcodeBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._barcodeBox_PreviewKeyDown);
            // 
            // _checkButton
            // 
            this._checkButton.Location = new System.Drawing.Point(716, 3);
            this._checkButton.Name = "_checkButton";
            this._checkButton.Size = new System.Drawing.Size(75, 23);
            this._checkButton.TabIndex = 1;
            this._checkButton.Text = "Проверить";
            this._checkButton.UseVisualStyleBackColor = true;
            this._checkButton.Click += new System.EventHandler(this._checkButton_Click);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Location = new System.Drawing.Point(3, 249);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(794, 122);
            this._logBox.TabIndex = 3;
            // 
            // _bookBrowser
            // 
            this._bookBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bookBrowser.Location = new System.Drawing.Point(3, 70);
            this._bookBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._bookBrowser.Name = "_bookBrowser";
            this._bookBrowser.Size = new System.Drawing.Size(794, 173);
            this._bookBrowser.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this._busyStripe, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._counterBox, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(794, 26);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Fill;
            this._busyStripe.ForeColor = System.Drawing.Color.Blue;
            this._busyStripe.Location = new System.Drawing.Point(3, 3);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(682, 20);
            this._busyStripe.TabIndex = 3;
            this._busyStripe.Visible = false;
            // 
            // _counterBox
            // 
            this._counterBox.Location = new System.Drawing.Point(691, 3);
            this._counterBox.Name = "_counterBox";
            this._counterBox.ReadOnly = true;
            this._counterBox.Size = new System.Drawing.Size(100, 20);
            this._counterBox.TabIndex = 4;
            this._counterBox.Text = "0";
            this._counterBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this._confirmButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this._reportButton, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this._missingButton, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 377);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(794, 70);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // _confirmButton
            // 
            this._confirmButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._confirmButton.Location = new System.Drawing.Point(3, 3);
            this._confirmButton.Name = "_confirmButton";
            this._confirmButton.Size = new System.Drawing.Size(604, 64);
            this._confirmButton.TabIndex = 2;
            this._confirmButton.Text = "Потдвердить = F2";
            this._confirmButton.UseVisualStyleBackColor = true;
            this._confirmButton.Click += new System.EventHandler(this._confirmButton_Click);
            // 
            // _reportButton
            // 
            this._reportButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._reportButton.Location = new System.Drawing.Point(613, 3);
            this._reportButton.Name = "_reportButton";
            this._reportButton.Size = new System.Drawing.Size(75, 64);
            this._reportButton.TabIndex = 3;
            this._reportButton.Text = "За сегодня";
            this._reportButton.UseVisualStyleBackColor = true;
            this._reportButton.Click += new System.EventHandler(this._reportButton_Click);
            // 
            // _missingButton
            // 
            this._missingButton.AutoSize = true;
            this._missingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._missingButton.Location = new System.Drawing.Point(694, 3);
            this._missingButton.Name = "_missingButton";
            this._missingButton.Size = new System.Drawing.Size(97, 64);
            this._missingButton.TabIndex = 4;
            this._missingButton.Text = "Отсутствующие";
            this._missingButton.UseVisualStyleBackColor = true;
            // 
            // _idleTimer
            // 
            this._idleTimer.Enabled = true;
            this._idleTimer.Interval = 360000;
            this._idleTimer.Tick += new System.EventHandler(this._idleTimer_Tick);
            // 
            // _saveFileDialog1
            // 
            this._saveFileDialog1.DefaultExt = "txt";
            this._saveFileDialog1.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Инвентаризация художественного фонда";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox _barcodeBox;
        private System.Windows.Forms.Button _checkButton;
        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.Timer _idleTimer;
        private System.Windows.Forms.WebBrowser _bookBrowser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private AM.Windows.Forms.BusyStripe _busyStripe;
        private System.Windows.Forms.TextBox _counterBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button _confirmButton;
        private System.Windows.Forms.Button _reportButton;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog1;
        private System.Windows.Forms.Button _missingButton;
    }
}

