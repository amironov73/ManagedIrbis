namespace Inventory2017
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this._collectionBox = new System.Windows.Forms.ComboBox();
            this._numberBox = new System.Windows.Forms.TextBox();
            this._badButton = new System.Windows.Forms.Button();
            this._fixShelfButton = new System.Windows.Forms.Button();
            this._fixPlaceButton = new System.Windows.Forms.Button();
            this._counterLabel = new System.Windows.Forms.Label();
            this._viewButton = new System.Windows.Forms.Button();
            this._toBox = new System.Windows.Forms.TextBox();
            this._fromBox = new System.Windows.Forms.TextBox();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._okButton = new System.Windows.Forms.Button();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._shelfBox = new System.Windows.Forms.TextBox();
            this._fondBox = new System.Windows.Forms.ComboBox();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this._idleTimer = new System.Windows.Forms.Timer(this.components);
            this._clearCollectionButton = new System.Windows.Forms.Button();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._clearCollectionButton);
            this.layoutControl1.Controls.Add(this._collectionBox);
            this.layoutControl1.Controls.Add(this._numberBox);
            this.layoutControl1.Controls.Add(this._badButton);
            this.layoutControl1.Controls.Add(this._fixShelfButton);
            this.layoutControl1.Controls.Add(this._fixPlaceButton);
            this.layoutControl1.Controls.Add(this._counterLabel);
            this.layoutControl1.Controls.Add(this._viewButton);
            this.layoutControl1.Controls.Add(this._toBox);
            this.layoutControl1.Controls.Add(this._fromBox);
            this.layoutControl1.Controls.Add(this._logBox);
            this.layoutControl1.Controls.Add(this._okButton);
            this.layoutControl1.Controls.Add(this._browser);
            this.layoutControl1.Controls.Add(this._shelfBox);
            this.layoutControl1.Controls.Add(this._fondBox);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(784, 562);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // _collectionBox
            // 
            this._collectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._collectionBox.FormattingEnabled = true;
            this._collectionBox.Location = new System.Drawing.Point(536, 37);
            this._collectionBox.Name = "_collectionBox";
            this._collectionBox.Size = new System.Drawing.Size(204, 21);
            this._collectionBox.TabIndex = 18;
            // 
            // _numberBox
            // 
            this._numberBox.Location = new System.Drawing.Point(154, 37);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(236, 20);
            this._numberBox.TabIndex = 17;
            this._numberBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._numberBox_KeyDown);
            // 
            // _badButton
            // 
            this._badButton.Enabled = false;
            this._badButton.Location = new System.Drawing.Point(544, 86);
            this._badButton.Name = "_badButton";
            this._badButton.Size = new System.Drawing.Size(228, 20);
            this._badButton.TabIndex = 16;
            this._badButton.Text = "Отложить и передать в обработку";
            this._badButton.UseVisualStyleBackColor = true;
            this._badButton.Click += new System.EventHandler(this._badButton_Click);
            // 
            // _fixShelfButton
            // 
            this._fixShelfButton.Location = new System.Drawing.Point(289, 86);
            this._fixShelfButton.Name = "_fixShelfButton";
            this._fixShelfButton.Size = new System.Drawing.Size(251, 20);
            this._fixShelfButton.TabIndex = 15;
            this._fixShelfButton.Text = "Исправить шифр";
            this._fixShelfButton.UseVisualStyleBackColor = true;
            this._fixShelfButton.Click += new System.EventHandler(this._fixShelfButton_Click);
            // 
            // _fixPlaceButton
            // 
            this._fixPlaceButton.Location = new System.Drawing.Point(12, 86);
            this._fixPlaceButton.Name = "_fixPlaceButton";
            this._fixPlaceButton.Size = new System.Drawing.Size(273, 20);
            this._fixPlaceButton.TabIndex = 14;
            this._fixPlaceButton.Text = "Исправить место/статус";
            this._fixPlaceButton.UseVisualStyleBackColor = true;
            this._fixPlaceButton.Click += new System.EventHandler(this._fixPlaceButton_Click);
            // 
            // _counterLabel
            // 
            this._counterLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this._counterLabel.Location = new System.Drawing.Point(301, 62);
            this._counterLabel.Name = "_counterLabel";
            this._counterLabel.Size = new System.Drawing.Size(182, 20);
            this._counterLabel.TabIndex = 13;
            this._counterLabel.Text = "Подтверждено: 0";
            this._counterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _viewButton
            // 
            this._viewButton.Location = new System.Drawing.Point(12, 62);
            this._viewButton.Name = "_viewButton";
            this._viewButton.Size = new System.Drawing.Size(285, 20);
            this._viewButton.TabIndex = 12;
            this._viewButton.Text = "Перечень подтверждённых книг";
            this._viewButton.UseVisualStyleBackColor = true;
            this._viewButton.Click += new System.EventHandler(this._viewButton_Click);
            // 
            // _toBox
            // 
            this._toBox.Location = new System.Drawing.Point(660, 12);
            this._toBox.Name = "_toBox";
            this._toBox.Size = new System.Drawing.Size(112, 20);
            this._toBox.TabIndex = 11;
            // 
            // _fromBox
            // 
            this._fromBox.Location = new System.Drawing.Point(495, 12);
            this._fromBox.Name = "_fromBox";
            this._fromBox.Size = new System.Drawing.Size(111, 20);
            this._fromBox.TabIndex = 10;
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Location = new System.Drawing.Point(12, 420);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(760, 130);
            this._logBox.TabIndex = 8;
            // 
            // _okButton
            // 
            this._okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._okButton.Location = new System.Drawing.Point(12, 375);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(760, 25);
            this._okButton.TabIndex = 7;
            this._okButton.Text = "Подтверждаю, что библиографическое описание совпадает с титульным листом книги";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _browser
            // 
            this._browser.Location = new System.Drawing.Point(12, 126);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(760, 245);
            this._browser.TabIndex = 6;
            // 
            // _shelfBox
            // 
            this._shelfBox.Location = new System.Drawing.Point(319, 12);
            this._shelfBox.Name = "_shelfBox";
            this._shelfBox.Size = new System.Drawing.Size(121, 20);
            this._shelfBox.TabIndex = 5;
            // 
            // _fondBox
            // 
            this._fondBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._fondBox.FormattingEnabled = true;
            this._fondBox.Location = new System.Drawing.Point(44, 12);
            this._fondBox.Name = "_fondBox";
            this._fondBox.Size = new System.Drawing.Size(230, 21);
            this._fondBox.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem6,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem15});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(784, 562);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._fondBox;
            this.layoutControlItem1.CustomizationFormText = "Фонд";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 25);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(56, 25);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(266, 25);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "Фонд";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(27, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._shelfBox;
            this.layoutControlItem2.CustomizationFormText = "Раздел";
            this.layoutControlItem2.Location = new System.Drawing.Point(266, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(166, 25);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "Раздел";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(36, 13);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._browser;
            this.layoutControlItem3.CustomizationFormText = "Описание найденной книги";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 98);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(764, 265);
            this.layoutControlItem3.Text = "Описание найденной книги";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(139, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._okButton;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 363);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(0, 29);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(24, 29);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(764, 29);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._logBox;
            this.layoutControlItem5.CustomizationFormText = "Выполнение операций";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 392);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(0, 240);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(143, 40);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(764, 150);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "Выполнение операций";
            this.layoutControlItem5.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(139, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(475, 50);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(289, 24);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._fromBox;
            this.layoutControlItem7.CustomizationFormText = "С номера";
            this.layoutControlItem7.Location = new System.Drawing.Point(432, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(166, 25);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "С номера";
            this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(46, 13);
            this.layoutControlItem7.TextToControlDistance = 5;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._toBox;
            this.layoutControlItem8.CustomizationFormText = "по номер";
            this.layoutControlItem8.Location = new System.Drawing.Point(598, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(166, 24);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(166, 25);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "по номер";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(45, 13);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._viewButton;
            this.layoutControlItem9.CustomizationFormText = "layoutControlItem9";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 50);
            this.layoutControlItem9.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem9.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(289, 24);
            this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._counterLabel;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(289, 50);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(186, 24);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._fixPlaceButton;
            this.layoutControlItem10.CustomizationFormText = "layoutControlItem10";
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 74);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._fixShelfButton;
            this.layoutControlItem11.CustomizationFormText = "layoutControlItem11";
            this.layoutControlItem11.Location = new System.Drawing.Point(277, 74);
            this.layoutControlItem11.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem11.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(255, 24);
            this.layoutControlItem11.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this._badButton;
            this.layoutControlItem12.CustomizationFormText = "layoutControlItem12";
            this.layoutControlItem12.Location = new System.Drawing.Point(532, 74);
            this.layoutControlItem12.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(232, 24);
            this.layoutControlItem12.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._numberBox;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(382, 25);
            this.layoutControlItem13.Text = "Инвентарный номер";
            this.layoutControlItem13.TextSize = new System.Drawing.Size(139, 13);
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this._collectionBox;
            this.layoutControlItem14.Location = new System.Drawing.Point(382, 25);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(350, 25);
            this.layoutControlItem14.Text = "Коллекция";
            this.layoutControlItem14.TextSize = new System.Drawing.Size(139, 13);
            // 
            // _idleTimer
            // 
            this._idleTimer.Enabled = true;
            this._idleTimer.Interval = 1000;
            this._idleTimer.Tick += new System.EventHandler(this._idleTimer_Tick);
            // 
            // _clearCollectionButton
            // 
            this._clearCollectionButton.Location = new System.Drawing.Point(744, 37);
            this._clearCollectionButton.Name = "_clearCollectionButton";
            this._clearCollectionButton.Size = new System.Drawing.Size(28, 21);
            this._clearCollectionButton.TabIndex = 19;
            this._clearCollectionButton.Text = "x";
            this._clearCollectionButton.UseVisualStyleBackColor = true;
            this._clearCollectionButton.Click += new System.EventHandler(this._clearCollectionButton_Click);
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this._clearCollectionButton;
            this.layoutControlItem15.Location = new System.Drawing.Point(732, 25);
            this.layoutControlItem15.MaxSize = new System.Drawing.Size(32, 25);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(32, 25);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(32, 25);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.layoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Инвентаризация фонда";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private System.Windows.Forms.TextBox _shelfBox;
        private System.Windows.Forms.ComboBox _fondBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.WebBrowser _browser;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Timer _idleTimer;
        private AM.Windows.Forms.LogBox _logBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private System.Windows.Forms.TextBox _toBox;
        private System.Windows.Forms.TextBox _fromBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.Button _viewButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.Label _counterLabel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.Button _fixShelfButton;
        private System.Windows.Forms.Button _fixPlaceButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.Button _badButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private System.Windows.Forms.TextBox _numberBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private System.Windows.Forms.ComboBox _collectionBox;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private System.Windows.Forms.Button _clearCollectionButton;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
    }
}

