namespace NedoCat
{
    partial class NedoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NedoForm));
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._logBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBox.Image")));
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(601, 496);
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.Color.DarkBlue;
            this._logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._logBox.ForeColor = System.Drawing.Color.White;
            this._logBox.Location = new System.Drawing.Point(0, 496);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.Size = new System.Drawing.Size(601, 73);
            this._logBox.TabIndex = 1;
            // 
            // NedoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 569);
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this._logBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NedoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NedoForm";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.TextBox _logBox;
    }
}