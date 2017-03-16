namespace Shortage
{
    partial class PlainTextViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlainTextViewer));
            this._editor = new AM.Windows.Forms.PlainTextEditor();
            this.SuspendLayout();
            // 
            // _editor
            // 
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.Location = new System.Drawing.Point(0, 0);
            this._editor.Name = "_editor";
            this._editor.Size = new System.Drawing.Size(684, 362);
            this._editor.TabIndex = 0;
            // 
            // 
            // 
            this._editor.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.TextBox.Location = new System.Drawing.Point(0, 27);
            this._editor.TextBox.Multiline = true;
            this._editor.TextBox.Name = "_textBox";
            this._editor.TextBox.Size = new System.Drawing.Size(684, 335);
            this._editor.TextBox.TabIndex = 1;
            // 
            // PlainTextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 362);
            this.Controls.Add(this._editor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "PlainTextViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Просмотр списка";
            this.ResumeLayout(false);

        }

        #endregion

        private AM.Windows.Forms.PlainTextEditor _editor;
    }
}