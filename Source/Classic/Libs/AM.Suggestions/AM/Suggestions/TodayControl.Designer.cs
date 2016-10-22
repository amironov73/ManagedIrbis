namespace AM.Suggestions
{
    partial class TodayControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed
        /// resources should be disposed; otherwise, false.
        /// </param>
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
            this._calendar = new System.Windows.Forms.MonthCalendar();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _calendar
            // 
            this._calendar.Location = new System.Drawing.Point(12, 7);
            this._calendar.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this._calendar.Name = "_calendar";
            this._calendar.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(10, 229);
            this._okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(100, 28);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "O&K";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(129, 229);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 28);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // TodayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._calendar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TodayControl";
            this.Size = new System.Drawing.Size(245, 266);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar _calendar;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}