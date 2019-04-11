namespace Dundee.Source
{
    partial class SearchBox
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
            this._prefixBox = new System.Windows.Forms.ComboBox();
            this._termBox = new System.Windows.Forms.ListBox();
            this._inputBox = new System.Windows.Forms.TextBox();
            this._searchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _prefixBox
            // 
            this._prefixBox.DisplayMember = "Name";
            this._prefixBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._prefixBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._prefixBox.FormattingEnabled = true;
            this._prefixBox.Location = new System.Drawing.Point(0, 0);
            this._prefixBox.Name = "_prefixBox";
            this._prefixBox.Size = new System.Drawing.Size(302, 21);
            this._prefixBox.TabIndex = 0;
            this._prefixBox.SelectedIndexChanged += new System.EventHandler(this._prefixBox_SelectedIndexChanged);
            // 
            // _termBox
            // 
            this._termBox.DisplayMember = "Text";
            this._termBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._termBox.FormattingEnabled = true;
            this._termBox.Location = new System.Drawing.Point(0, 21);
            this._termBox.Name = "_termBox";
            this._termBox.ScrollAlwaysVisible = true;
            this._termBox.Size = new System.Drawing.Size(302, 298);
            this._termBox.TabIndex = 1;
            this._termBox.DoubleClick += new System.EventHandler(this._termBox_DoubleClick);
            // 
            // _inputBox
            // 
            this._inputBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._inputBox.Location = new System.Drawing.Point(0, 319);
            this._inputBox.Name = "_inputBox";
            this._inputBox.Size = new System.Drawing.Size(302, 20);
            this._inputBox.TabIndex = 2;
            this._inputBox.TextChanged += new System.EventHandler(this._inputBox_TextChanged);
            // 
            // _searchButton
            // 
            this._searchButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._searchButton.Location = new System.Drawing.Point(0, 339);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(302, 23);
            this._searchButton.TabIndex = 3;
            this._searchButton.Text = "Поиск";
            this._searchButton.UseVisualStyleBackColor = true;
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // SearchBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._termBox);
            this.Controls.Add(this._inputBox);
            this.Controls.Add(this._searchButton);
            this.Controls.Add(this._prefixBox);
            this.Name = "SearchBox";
            this.Size = new System.Drawing.Size(302, 362);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _prefixBox;
        private System.Windows.Forms.ListBox _termBox;
        private System.Windows.Forms.TextBox _inputBox;
        private System.Windows.Forms.Button _searchButton;
    }
}
