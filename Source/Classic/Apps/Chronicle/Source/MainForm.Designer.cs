﻿namespace Chronicle
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._yearBox = new System.Windows.Forms.ToolStripComboBox();
            this._withMfn = new AM.Windows.Forms.ToolStripCheckBox();
            this._additionalBox = new AM.Windows.Forms.ToolStripCheckBox();
            this._goButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this._linksBox = new AM.Windows.Forms.ToolStripCheckBox();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 425);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._yearBox,
            this._withMfn,
            this._linksBox,
            this._additionalBox,
            this._goButton,
            this.toolStripSeparator1,
            this._saveButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(26, 22);
            this.toolStripLabel1.Text = "Год";
            // 
            // _yearBox
            // 
            this._yearBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._yearBox.Name = "_yearBox";
            this._yearBox.Size = new System.Drawing.Size(121, 25);
            // 
            // _withMfn
            // 
            this._withMfn.BackColor = System.Drawing.Color.Transparent;
            // 
            // _withMfn
            // 
            this._withMfn.CheckBox.AccessibleName = "_withMfn";
            this._withMfn.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._withMfn.CheckBox.Location = new System.Drawing.Point(158, 1);
            this._withMfn.CheckBox.Name = "_withMfn";
            this._withMfn.CheckBox.Size = new System.Drawing.Size(52, 22);
            this._withMfn.CheckBox.TabIndex = 1;
            this._withMfn.CheckBox.Text = "MFN";
            this._withMfn.CheckBox.UseVisualStyleBackColor = false;
            this._withMfn.Name = "_withMfn";
            this._withMfn.Size = new System.Drawing.Size(52, 22);
            this._withMfn.Text = "MFN";
            // 
            // _additionalBox
            // 
            this._additionalBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _additionalBox
            // 
            this._additionalBox.CheckBox.AccessibleName = "_additionalBox";
            this._additionalBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._additionalBox.CheckBox.Location = new System.Drawing.Point(310, 1);
            this._additionalBox.CheckBox.Name = "_additionalBox";
            this._additionalBox.CheckBox.Size = new System.Drawing.Size(122, 22);
            this._additionalBox.CheckBox.TabIndex = 2;
            this._additionalBox.CheckBox.Text = "Дополнительные";
            this._additionalBox.CheckBox.UseVisualStyleBackColor = false;
            this._additionalBox.Name = "_additionalBox";
            this._additionalBox.Size = new System.Drawing.Size(122, 22);
            this._additionalBox.Text = "Дополнительные";
            // 
            // _goButton
            // 
            this._goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._goButton.Image = ((System.Drawing.Image)(resources.GetObject("_goButton.Image")));
            this._goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(52, 22);
            this._goButton.Text = "Запуск!";
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _saveButton
            // 
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._saveButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveButton.Image")));
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(79, 22);
            this._saveButton.Text = "Сохранить...";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // _linksBox
            // 
            this._linksBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _linksBox
            // 
            this._linksBox.CheckBox.AccessibleName = "_linksBox";
            this._linksBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._linksBox.CheckBox.Location = new System.Drawing.Point(210, 1);
            this._linksBox.CheckBox.Name = "_linksBox";
            this._linksBox.CheckBox.Size = new System.Drawing.Size(100, 22);
            this._linksBox.CheckBox.TabIndex = 3;
            this._linksBox.CheckBox.Text = "Гиперссылки";
            this._linksBox.CheckBox.UseVisualStyleBackColor = false;
            this._linksBox.Name = "_linksBox";
            this._linksBox.Size = new System.Drawing.Size(100, 22);
            this._linksBox.Text = "Гиперссылки";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Книжная летопись";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox _yearBox;
        private System.Windows.Forms.ToolStripButton _goButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton _saveButton;
        private AM.Windows.Forms.ToolStripCheckBox _withMfn;
        private AM.Windows.Forms.ToolStripCheckBox _additionalBox;
        private AM.Windows.Forms.ToolStripCheckBox _linksBox;
    }
}

