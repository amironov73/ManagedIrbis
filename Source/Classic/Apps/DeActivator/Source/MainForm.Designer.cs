namespace DeActivator
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
            this._deactivateButton = new System.Windows.Forms.Button();
            this._activateButton = new System.Windows.Forms.Button();
            this._logBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _deactivateButton
            // 
            this._deactivateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._deactivateButton.Location = new System.Drawing.Point(12, 189);
            this._deactivateButton.Name = "_deactivateButton";
            this._deactivateButton.Size = new System.Drawing.Size(558, 23);
            this._deactivateButton.TabIndex = 1;
            this._deactivateButton.Text = "Можно выносить";
            this._deactivateButton.UseVisualStyleBackColor = true;
            this._deactivateButton.Click += new System.EventHandler(this._deactivateButton_Click);
            // 
            // _activateButton
            // 
            this._activateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._activateButton.Location = new System.Drawing.Point(12, 218);
            this._activateButton.Name = "_activateButton";
            this._activateButton.Size = new System.Drawing.Size(558, 23);
            this._activateButton.TabIndex = 2;
            this._activateButton.Text = "Нельзя выносить";
            this._activateButton.UseVisualStyleBackColor = true;
            this._activateButton.Click += new System.EventHandler(this._activateButton_Click);
            // 
            // _logBox
            // 
            this._logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Location = new System.Drawing.Point(12, 12);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(558, 171);
            this._logBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 253);
            this.Controls.Add(this._logBox);
            this.Controls.Add(this._activateButton);
            this.Controls.Add(this._deactivateButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.Text = "Деактивация меток";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button _deactivateButton;
        private System.Windows.Forms.Button _activateButton;
        private System.Windows.Forms.TextBox _logBox;
    }
}

