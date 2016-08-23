namespace AM.Windows.Forms
{
    partial class ToolStripCustomizationForm
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
            this._groupBox = new System.Windows.Forms.GroupBox();
            this._downButton = new System.Windows.Forms.Button();
            this._upButton = new System.Windows.Forms.Button();
            this._listBox = new System.Windows.Forms.CheckedListBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._applyButton = new System.Windows.Forms.Button();
            this._groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupBox
            // 
            this._groupBox.Controls.Add(this._downButton);
            this._groupBox.Controls.Add(this._upButton);
            this._groupBox.Controls.Add(this._listBox);
            this._groupBox.Location = new System.Drawing.Point(12, 12);
            this._groupBox.Name = "_groupBox";
            this._groupBox.Size = new System.Drawing.Size(200, 242);
            this._groupBox.TabIndex = 0;
            this._groupBox.TabStop = false;
            this._groupBox.Text = "ToolStrip items";
            // 
            // _downButton
            // 
            this._downButton.Location = new System.Drawing.Point(168, 124);
            this._downButton.Name = "_downButton";
            this._downButton.Size = new System.Drawing.Size(26, 23);
            this._downButton.TabIndex = 2;
            this._downButton.Text = "down";
            this._downButton.UseVisualStyleBackColor = true;
            // 
            // _upButton
            // 
            this._upButton.Location = new System.Drawing.Point(168, 87);
            this._upButton.Name = "_upButton";
            this._upButton.Size = new System.Drawing.Size(26, 23);
            this._upButton.TabIndex = 1;
            this._upButton.Text = "up";
            this._upButton.UseVisualStyleBackColor = true;
            // 
            // _listBox
            // 
            this._listBox.CheckOnClick = true;
            this._listBox.Location = new System.Drawing.Point(6, 19);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(156, 214);
            this._listBox.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(223, 12);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "&OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._applyButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(223, 41);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _applyButton
            // 
            this._applyButton.Location = new System.Drawing.Point(223, 70);
            this._applyButton.Name = "_applyButton";
            this._applyButton.Size = new System.Drawing.Size(75, 23);
            this._applyButton.TabIndex = 3;
            this._applyButton.Text = "&Apply";
            this._applyButton.UseVisualStyleBackColor = true;
            this._applyButton.Click += new System.EventHandler(this._applyButton_Click);
            // 
            // ToolStripCustomizationForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(310, 266);
            this.Controls.Add(this._applyButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._groupBox);
            this.Name = "ToolStripCustomizationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ToolStrip customization";
            this.Load += new System.EventHandler(this.ToolStripCustomizationForm_Load);
            this._groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupBox;
        private System.Windows.Forms.CheckedListBox _listBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _applyButton;
        private System.Windows.Forms.Button _upButton;
        private System.Windows.Forms.Button _downButton;
    }
}