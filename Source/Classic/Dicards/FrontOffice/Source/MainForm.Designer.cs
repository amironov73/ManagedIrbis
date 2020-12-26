namespace FrontOffice
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
            this._layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._emailBox = new DevExpress.XtraEditors.TextEdit();
            this._ticketBox = new DevExpress.XtraEditors.TextEdit();
            this._configButton = new DevExpress.XtraEditors.SimpleButton();
            this._sendEmailButton = new DevExpress.XtraEditors.SimpleButton();
            this._createButton = new DevExpress.XtraEditors.SimpleButton();
            this._searchButton = new DevExpress.XtraEditors.SimpleButton();
            this._logBox = new AM.Windows.Forms.LogBox();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem7 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).BeginInit();
            this._layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._emailBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ticketBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // _layoutControl
            // 
            this._layoutControl.Controls.Add(this._browser);
            this._layoutControl.Controls.Add(this._emailBox);
            this._layoutControl.Controls.Add(this._ticketBox);
            this._layoutControl.Controls.Add(this._configButton);
            this._layoutControl.Controls.Add(this._sendEmailButton);
            this._layoutControl.Controls.Add(this._createButton);
            this._layoutControl.Controls.Add(this._searchButton);
            this._layoutControl.Controls.Add(this._logBox);
            this._layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutControl.Location = new System.Drawing.Point(0, 0);
            this._layoutControl.Name = "_layoutControl";
            this._layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(906, 258, 650, 400);
            this._layoutControl.Root = this.Root;
            this._layoutControl.Size = new System.Drawing.Size(598, 448);
            this._layoutControl.TabIndex = 0;
            this._layoutControl.Text = "layoutControl1";
            // 
            // _browser
            // 
            this._browser.Location = new System.Drawing.Point(12, 102);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(346, 165);
            this._browser.TabIndex = 11;
            // 
            // _emailBox
            // 
            this._emailBox.Location = new System.Drawing.Point(12, 68);
            this._emailBox.Name = "_emailBox";
            this._emailBox.Size = new System.Drawing.Size(346, 20);
            this._emailBox.StyleController = this._layoutControl;
            this._emailBox.TabIndex = 10;
            // 
            // _ticketBox
            // 
            this._ticketBox.Location = new System.Drawing.Point(12, 28);
            this._ticketBox.Name = "_ticketBox";
            this._ticketBox.Size = new System.Drawing.Size(346, 20);
            this._ticketBox.StyleController = this._layoutControl;
            this._ticketBox.TabIndex = 9;
            // 
            // _configButton
            // 
            this._configButton.Location = new System.Drawing.Point(374, 123);
            this._configButton.Name = "_configButton";
            this._configButton.Size = new System.Drawing.Size(212, 22);
            this._configButton.StyleController = this._layoutControl;
            this._configButton.TabIndex = 8;
            this._configButton.Text = "Настройки...";
            this._configButton.Click += new System.EventHandler(this._configButton_Click);
            // 
            // _sendEmailButton
            // 
            this._sendEmailButton.Location = new System.Drawing.Point(374, 86);
            this._sendEmailButton.Name = "_sendEmailButton";
            this._sendEmailButton.Size = new System.Drawing.Size(212, 22);
            this._sendEmailButton.StyleController = this._layoutControl;
            this._sendEmailButton.TabIndex = 7;
            this._sendEmailButton.Text = "Послать e-mail";
            this._sendEmailButton.Click += new System.EventHandler(this._sendEmailButton_Click);
            // 
            // _createButton
            // 
            this._createButton.Location = new System.Drawing.Point(374, 49);
            this._createButton.Name = "_createButton";
            this._createButton.Size = new System.Drawing.Size(212, 22);
            this._createButton.StyleController = this._layoutControl;
            this._createButton.TabIndex = 6;
            this._createButton.Text = "Создать карту";
            this._createButton.Click += new System.EventHandler(this._createButton_Click);
            // 
            // _searchButton
            // 
            this._searchButton.Location = new System.Drawing.Point(374, 12);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(212, 22);
            this._searchButton.StyleController = this._layoutControl;
            this._searchButton.TabIndex = 5;
            this._searchButton.Text = "Найти читателя по билету";
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Location = new System.Drawing.Point(12, 282);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(574, 154);
            this._logBox.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem1,
            this.emptySpaceItem5,
            this.emptySpaceItem6,
            this.emptySpaceItem7});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(598, 448);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._logBox;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 270);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(578, 158);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._searchButton;
            this.layoutControlItem2.Location = new System.Drawing.Point(362, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._createButton;
            this.layoutControlItem3.Location = new System.Drawing.Point(362, 37);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._sendEmailButton;
            this.layoutControlItem4.Location = new System.Drawing.Point(362, 74);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._configButton;
            this.layoutControlItem5.Location = new System.Drawing.Point(362, 111);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._ticketBox;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(350, 40);
            this.layoutControlItem6.Text = "Номер читательского билета";
            this.layoutControlItem6.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(149, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._emailBox;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 40);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(350, 40);
            this.layoutControlItem7.Text = "E-mail";
            this.layoutControlItem7.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(149, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._browser;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(129, 24);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(350, 169);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(350, 0);
            this.emptySpaceItem2.MaxSize = new System.Drawing.Size(12, 0);
            this.emptySpaceItem2.MinSize = new System.Drawing.Size(12, 10);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(12, 259);
            this.emptySpaceItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(362, 137);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(216, 122);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 80);
            this.emptySpaceItem4.MaxSize = new System.Drawing.Size(0, 10);
            this.emptySpaceItem4.MinSize = new System.Drawing.Size(10, 10);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(350, 10);
            this.emptySpaceItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 259);
            this.emptySpaceItem1.MaxSize = new System.Drawing.Size(0, 11);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(10, 11);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(578, 11);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(362, 26);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(216, 11);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem6
            // 
            this.emptySpaceItem6.AllowHotTrack = false;
            this.emptySpaceItem6.Location = new System.Drawing.Point(362, 63);
            this.emptySpaceItem6.Name = "emptySpaceItem6";
            this.emptySpaceItem6.Size = new System.Drawing.Size(216, 11);
            this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem7
            // 
            this.emptySpaceItem7.AllowHotTrack = false;
            this.emptySpaceItem7.Location = new System.Drawing.Point(362, 100);
            this.emptySpaceItem7.Name = "emptySpaceItem7";
            this.emptySpaceItem7.Size = new System.Drawing.Size(216, 11);
            this.emptySpaceItem7.TextSize = new System.Drawing.Size(0, 0);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 448);
            this.Controls.Add(this._layoutControl);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("MainForm.IconOptions.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "MainForm";
            this.Text = "Работа с картами DiCARDS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).EndInit();
            this._layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._emailBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ticketBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl _layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private System.Windows.Forms.WebBrowser _browser;
        private DevExpress.XtraEditors.TextEdit _emailBox;
        private DevExpress.XtraEditors.TextEdit _ticketBox;
        private DevExpress.XtraEditors.SimpleButton _configButton;
        private DevExpress.XtraEditors.SimpleButton _sendEmailButton;
        private DevExpress.XtraEditors.SimpleButton _createButton;
        private DevExpress.XtraEditors.SimpleButton _searchButton;
        private AM.Windows.Forms.LogBox _logBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem7;
    }
}

