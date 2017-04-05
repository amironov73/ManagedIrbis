namespace MagnaRfid
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._readerGroup = new AM.Windows.Forms.RadioGroup();
            this._connectionGroup = new AM.Windows.Forms.RadioGroup();
            this._nameBox = new AM.Windows.Forms.LabeledTextBox();
            this._crlfBox = new System.Windows.Forms.CheckBox();
            this._repeatBox = new System.Windows.Forms.CheckBox();
            this._hotkeyBox = new System.Windows.Forms.CheckBox();
            this._openButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._startButton = new System.Windows.Forms.Button();
            this._stopButton = new System.Windows.Forms.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _readerGroup
            // 
            this._readerGroup.Current = 1;
            this._readerGroup.Lines = new string[] {
        "FEIG",
        "CardMan"};
            this._readerGroup.Location = new System.Drawing.Point(12, 12);
            this._readerGroup.Name = "_readerGroup";
            this._readerGroup.Size = new System.Drawing.Size(200, 102);
            this._readerGroup.TabIndex = 0;
            this._readerGroup.TabStop = false;
            this._readerGroup.Text = "RFID reader";
            // 
            // _connectionGroup
            // 
            this._connectionGroup.Current = 2;
            this._connectionGroup.Lines = new string[] {
        "COM",
        "LAN",
        "USB"};
            this._connectionGroup.Location = new System.Drawing.Point(218, 12);
            this._connectionGroup.Name = "_connectionGroup";
            this._connectionGroup.Size = new System.Drawing.Size(97, 102);
            this._connectionGroup.TabIndex = 1;
            this._connectionGroup.TabStop = false;
            this._connectionGroup.Text = "Connection";
            // 
            // _nameBox
            // 
            // 
            // 
            // 
            this._nameBox.Label.Dock = System.Windows.Forms.DockStyle.Top;
            this._nameBox.Label.Location = new System.Drawing.Point(0, 0);
            this._nameBox.Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._nameBox.Label.Name = "_label";
            this._nameBox.Label.Size = new System.Drawing.Size(200, 26);
            this._nameBox.Label.TabIndex = 0;
            this._nameBox.Label.Text = "Device name";
            this._nameBox.Location = new System.Drawing.Point(321, 12);
            this._nameBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._nameBox.Name = "_nameBox";
            this._nameBox.Size = new System.Drawing.Size(200, 71);
            this._nameBox.TabIndex = 2;
            // 
            // 
            // 
            this._nameBox.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nameBox.TextBox.Location = new System.Drawing.Point(0, 26);
            this._nameBox.TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._nameBox.TextBox.Name = "_textBox";
            this._nameBox.TextBox.Size = new System.Drawing.Size(200, 22);
            this._nameBox.TextBox.TabIndex = 1;
            // 
            // _crlfBox
            // 
            this._crlfBox.Checked = true;
            this._crlfBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._crlfBox.Location = new System.Drawing.Point(12, 129);
            this._crlfBox.Name = "_crlfBox";
            this._crlfBox.Size = new System.Drawing.Size(200, 24);
            this._crlfBox.TabIndex = 3;
            this._crlfBox.Text = "CRLF";
            this._crlfBox.UseVisualStyleBackColor = true;
            // 
            // _repeatBox
            // 
            this._repeatBox.Location = new System.Drawing.Point(322, 90);
            this._repeatBox.Name = "_repeatBox";
            this._repeatBox.Size = new System.Drawing.Size(200, 24);
            this._repeatBox.TabIndex = 3;
            this._repeatBox.Text = "Repeat";
            this._repeatBox.UseVisualStyleBackColor = true;
            // 
            // _hotkeyBox
            // 
            this._hotkeyBox.Location = new System.Drawing.Point(321, 129);
            this._hotkeyBox.Name = "_hotkeyBox";
            this._hotkeyBox.Size = new System.Drawing.Size(200, 24);
            this._hotkeyBox.TabIndex = 3;
            this._hotkeyBox.Text = "Hotkey";
            this._hotkeyBox.UseVisualStyleBackColor = true;
            // 
            // _openButton
            // 
            this._openButton.Location = new System.Drawing.Point(12, 159);
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(303, 32);
            this._openButton.TabIndex = 4;
            this._openButton.Text = "Open";
            this._openButton.UseVisualStyleBackColor = true;
            this._openButton.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(12, 197);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(303, 32);
            this._closeButton.TabIndex = 5;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            // 
            // _startButton
            // 
            this._startButton.Location = new System.Drawing.Point(321, 159);
            this._startButton.Name = "_startButton";
            this._startButton.Size = new System.Drawing.Size(201, 32);
            this._startButton.TabIndex = 6;
            this._startButton.Text = "Start";
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this._startButton_Click);
            // 
            // _stopButton
            // 
            this._stopButton.Location = new System.Drawing.Point(321, 197);
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(201, 32);
            this._stopButton.TabIndex = 7;
            this._stopButton.Text = "Stop";
            this._stopButton.UseVisualStyleBackColor = true;
            this._stopButton.Click += new System.EventHandler(this._stopButton_Click);
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 232);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._startButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._openButton);
            this.Controls.Add(this._hotkeyBox);
            this.Controls.Add(this._repeatBox);
            this.Controls.Add(this._crlfBox);
            this.Controls.Add(this._nameBox);
            this.Controls.Add(this._connectionGroup);
            this.Controls.Add(this._readerGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Magna RFID reader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private AM.Windows.Forms.RadioGroup _readerGroup;
        private AM.Windows.Forms.RadioGroup _connectionGroup;
        private AM.Windows.Forms.LabeledTextBox _nameBox;
        private System.Windows.Forms.CheckBox _crlfBox;
        private System.Windows.Forms.CheckBox _repeatBox;
        private System.Windows.Forms.CheckBox _hotkeyBox;
        private System.Windows.Forms.Button _openButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.Button _stopButton;
        private System.Windows.Forms.Timer _timer;
    }
}

