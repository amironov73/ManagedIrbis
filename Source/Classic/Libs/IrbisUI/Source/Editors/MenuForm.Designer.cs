namespace IrbisUI
{
    partial class MenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._grid = new IrbisUI.Grid.SiberianMenuGrid();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._cancelButton);
            this._bottomPanel.Controls.Add(this._okButton);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 253);
            this._bottomPanel.Margin = new System.Windows.Forms.Padding(2);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(468, 41);
            this._bottomPanel.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(370, 6);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(90, 24);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(8, 8);
            this._okButton.Margin = new System.Windows.Forms.Padding(2);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(90, 24);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.DarkGray;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.HeaderHeight = 26;
            this._grid.LineColor = System.Drawing.Color.Gray;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(468, 253);
            this._grid.TabIndex = 0;
            this._grid.Text = "siberianMenuGrid1";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 294);
            this.Controls.Add(this._grid);
            this.Controls.Add(this._bottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(484, 332);
            this.Name = "MenuForm";
            this.Text = "Menu";
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _bottomPanel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private Grid.SiberianMenuGrid _grid;
    }
}