using System.Drawing;
using IrbisUI;

namespace EasyBinder
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
            this._magazineList = new IrbisUI.MagazineListBox();
            this._logBox = new AM.Windows.Forms.LogBox();
            this.SuspendLayout();
            // 
            // _magazineList
            // 
            this._magazineList.Dock = System.Windows.Forms.DockStyle.Left;
            this._magazineList.Location = new System.Drawing.Point(0, 0);
            this._magazineList.Name = "_magazineList";
            this._magazineList.Size = new System.Drawing.Size(301, 487);
            this._magazineList.TabIndex = 0;
            // 
            // _logBox
            // 
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 487);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.Size = new System.Drawing.Size(784, 75);
            this._logBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this._magazineList);
            this.Controls.Add(this._logBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "RFID-метки для периодики";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MagazineListBox _magazineList;
        private AM.Windows.Forms.LogBox _logBox;
    }
}

