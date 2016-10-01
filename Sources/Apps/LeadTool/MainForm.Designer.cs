namespace LeadTrial
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
            this._datePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this._goButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _datePicker
            // 
            this._datePicker.Location = new System.Drawing.Point(13, 29);
            this._datePicker.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this._datePicker.MinDate = new System.DateTime(2014, 8, 1, 0, 0, 0, 0);
            this._datePicker.Name = "_datePicker";
            this._datePicker.Size = new System.Drawing.Size(160, 20);
            this._datePicker.TabIndex = 0;
            this._datePicker.Value = new System.DateTime(2050, 1, 1, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Продлить триал до";
            // 
            // _goButton
            // 
            this._goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._goButton.Location = new System.Drawing.Point(186, 10);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(173, 38);
            this._goButton.TabIndex = 2;
            this._goButton.Text = "Записать в реестр";
            this._goButton.UseVisualStyleBackColor = true;
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 57);
            this.Controls.Add(this._goButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._datePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Продление триала LeadTools 18";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _datePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _goButton;
    }
}

