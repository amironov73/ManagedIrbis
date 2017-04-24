namespace AsyncSocketTester
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
            this._pressMeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _pressMeButton
            // 
            this._pressMeButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pressMeButton.Location = new System.Drawing.Point(0, 386);
            this._pressMeButton.Name = "_pressMeButton";
            this._pressMeButton.Size = new System.Drawing.Size(677, 40);
            this._pressMeButton.TabIndex = 1;
            this._pressMeButton.Text = "Press me!";
            this._pressMeButton.UseVisualStyleBackColor = true;
            this._pressMeButton.Click += new System.EventHandler(this._pressMeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 426);
            this.Controls.Add(this._pressMeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Async Socket Tester";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _pressMeButton;
    }
}

