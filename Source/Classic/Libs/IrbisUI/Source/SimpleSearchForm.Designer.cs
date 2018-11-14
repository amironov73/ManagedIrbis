namespace IrbisUI.Source
{
    partial class SimpleSearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleSearchForm));
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._termPanel = new IrbisUI.DictionaryPanel();
            this._prefixBox = new System.Windows.Forms.ComboBox();
            this._goButton = new System.Windows.Forms.Button();
            this._resultButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._closeButton);
            this._bottomPanel.Controls.Add(this._resultButton);
            this._bottomPanel.Controls.Add(this._goButton);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 415);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(594, 56);
            this._bottomPanel.TabIndex = 0;
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._termPanel);
            this._splitContainer.Panel1.Controls.Add(this._prefixBox);
            this._splitContainer.Size = new System.Drawing.Size(594, 415);
            this._splitContainer.SplitterDistance = 270;
            this._splitContainer.TabIndex = 1;
            // 
            // _termPanel
            // 
            this._termPanel.Adapter = null;
            this._termPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._termPanel.Location = new System.Drawing.Point(0, 21);
            this._termPanel.Margin = new System.Windows.Forms.Padding(2);
            this._termPanel.Name = "_termPanel";
            this._termPanel.Size = new System.Drawing.Size(270, 394);
            this._termPanel.TabIndex = 0;
            // 
            // _prefixBox
            // 
            this._prefixBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._prefixBox.FormattingEnabled = true;
            this._prefixBox.Location = new System.Drawing.Point(0, 0);
            this._prefixBox.Name = "_prefixBox";
            this._prefixBox.Size = new System.Drawing.Size(270, 21);
            this._prefixBox.TabIndex = 1;
            // 
            // _goButton
            // 
            this._goButton.Location = new System.Drawing.Point(12, 21);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(111, 23);
            this._goButton.TabIndex = 0;
            this._goButton.Text = "Search";
            this._goButton.UseVisualStyleBackColor = true;
            // 
            // _resultButton
            // 
            this._resultButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._resultButton.Location = new System.Drawing.Point(315, 21);
            this._resultButton.Name = "_resultButton";
            this._resultButton.Size = new System.Drawing.Size(141, 23);
            this._resultButton.TabIndex = 1;
            this._resultButton.Text = "View result";
            this._resultButton.UseVisualStyleBackColor = true;
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(462, 21);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(120, 23);
            this._closeButton.TabIndex = 2;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            // 
            // SimpleSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 471);
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._bottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "SimpleSearchForm";
            this.ShowInTaskbar = false;
            this.Text = "Поиск по словарю/рубрикатору";
            this._bottomPanel.ResumeLayout(false);
            this._splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _bottomPanel;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private DictionaryPanel _termPanel;
        private System.Windows.Forms.ComboBox _prefixBox;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _resultButton;
        private System.Windows.Forms.Button _goButton;
    }
}