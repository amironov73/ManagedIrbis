namespace SiberianGrider
{
    partial class Form1
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
            this._grid = new IrbisUI.Grid.SiberianGrid();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.DarkGray;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.HeaderHeight = 20;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Margin = new System.Windows.Forms.Padding(4);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(1175, 565);
            this._grid.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 565);
            this.Controls.Add(this._grid);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private IrbisUI.Grid.SiberianGrid _grid;
    }
}

