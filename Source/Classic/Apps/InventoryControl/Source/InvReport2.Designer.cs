namespace InventoryControl
{
    partial class InvReport2
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this._ksuCell = new DevExpress.XtraReports.UI.XRTableCell();
            this._numberCell = new DevExpress.XtraReports.UI.XRTableCell();
            this._descriptionCell = new DevExpress.XtraReports.UI.XRTableCell();
            this._yearCell = new DevExpress.XtraReports.UI.XRTableCell();
            this._indexCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.MainTitle = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this._indexTitleCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageNumberInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageNumber = new DevExpress.XtraReports.Parameters.Parameter();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.SumPrice = new DevExpress.XtraReports.Parameters.Parameter();
            this.NBooks = new DevExpress.XtraReports.Parameters.Parameter();
            this.NTitles = new DevExpress.XtraReports.Parameters.Parameter();
            this.ordinary = new DevExpress.XtraReports.UI.XRControlStyle();
            this.underlined = new DevExpress.XtraReports.UI.XRControlStyle();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this._issueCell = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //
            // Detail
            //
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Detail.HeightF = 10F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            // xrTable2
            //
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 0, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(599F, 10F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            //
            // xrTableRow2
            //
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this._ksuCell,
            this._numberCell,
            this._descriptionCell,
            this._yearCell,
            this._issueCell,
            this._indexCell,
            this.xrTableCell12});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.88235294117647056D;
            //
            // _ksuCell
            //
            this._ksuCell.CanGrow = false;
            this._ksuCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Ordinal")});
            this._ksuCell.Name = "_ksuCell";
            this._ksuCell.StylePriority.UseTextAlignment = false;
            this._ksuCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this._ksuCell.Weight = 0.23214274455741335D;
            this._ksuCell.WordWrap = false;
            //
            // _numberCell
            //
            this._numberCell.CanGrow = false;
            this._numberCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Number")});
            this._numberCell.Name = "_numberCell";
            this._numberCell.StylePriority.UseTextAlignment = false;
            this._numberCell.Text = "_numberCell";
            this._numberCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this._numberCell.Weight = 0.23803615590217928D;
            this._numberCell.WordWrap = false;
            //
            // _descriptionCell
            //
            this._descriptionCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Description")});
            this._descriptionCell.Name = "_descriptionCell";
            this._descriptionCell.StylePriority.UseTextAlignment = false;
            this._descriptionCell.Text = "_descriptionCell";
            this._descriptionCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this._descriptionCell.TextTrimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this._descriptionCell.Weight = 1.2879008452003004D;
            this._descriptionCell.WordWrap = false;
            //
            // _yearCell
            //
            this._yearCell.CanGrow = false;
            this._yearCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Year")});
            this._yearCell.Name = "_yearCell";
            this._yearCell.StylePriority.UseTextAlignment = false;
            this._yearCell.Text = "_yearCell";
            this._yearCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._yearCell.Weight = 0.21072918663803292D;
            this._yearCell.WordWrap = false;
            //
            // _issueCell
            //
            this._issueCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, "Issue")});
            this._issueCell.Multiline = false;
            this._issueCell.Name = "_issueCell";
            this._issueCell.StylePriority.UseTextAlignment = false;
            this._issueCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this._issueCell.Weight = 0.17945071744518842D;
            //
            // _indexCell
            //
            this._indexCell.CanGrow = false;
            this._indexCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Bbk")});
            this._indexCell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._indexCell.Name = "_indexCell";
            this._indexCell.StylePriority.UseFont = false;
            this._indexCell.Weight = 0.21693422461758485D;
            this._indexCell.WordWrap = false;
            //
            // xrTableCell12
            //
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Remark")});
            this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell12.Weight = 0.41267849386035416D;
            //
            // TopMargin
            //
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrTable1,
            this.PageNumberInfo});
            this.TopMargin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TopMargin.HeightF = 53.00001F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseFont = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            // xrLabel1
            //
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.MainTitle, "Text", "")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(498.9584F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // MainTitle
            //
            this.MainTitle.Description = "Main Title";
            this.MainTitle.Name = "MainTitle";
            this.MainTitle.Visible = false;
            //
            // xrTable1
            //
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(599.0001F, 20F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrTableRow1
            //
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell5,
            this.xrTableCell2,
            this.xrTableCell6,
            this.xrTableCell4,
            this._indexTitleCell,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 0.8D;
            //
            // xrTableCell1
            //
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "№ п/п";
            this.xrTableCell1.Weight = 0.23214270732187936D;
            //
            // xrTableCell5
            //
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Номер";
            this.xrTableCell5.Weight = 0.23803601143910345D;
            //
            // xrTableCell2
            //
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Автор и заглавие";
            this.xrTableCell2.Weight = 1.2879003841364035D;
            //
            // xrTableCell6
            //
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Год";
            this.xrTableCell6.Weight = 0.2107286739464862D;
            //
            // _indexTitleCell
            //
            this._indexTitleCell.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._indexTitleCell.Name = "_indexTitleCell";
            this._indexTitleCell.StylePriority.UseFont = false;
            this._indexTitleCell.Text = "Шифр";
            this._indexTitleCell.Weight = 0.21800592621793702D;
            //
            // xrTableCell3
            //
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Отметки";
            this.xrTableCell3.Weight = 0.4116072998541046D;
            //
            // PageNumberInfo
            //
            this.PageNumberInfo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.PageNumberInfo.LocationFloat = new DevExpress.Utils.PointFloat(498.9584F, 10.00001F);
            this.PageNumberInfo.Name = "PageNumberInfo";
            this.PageNumberInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PageNumberInfo.PageInfo = DevExpress.XtraPrinting.PageInfo.Number;
            this.PageNumberInfo.SizeF = new System.Drawing.SizeF(97.91669F, 23F);
            this.PageNumberInfo.StylePriority.UseFont = false;
            this.PageNumberInfo.StylePriority.UseTextAlignment = false;
            this.PageNumberInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.PageNumberInfo.Format = "Стр. {0}";
            //
            // PageNumber
            //
            this.PageNumber.Description = "Page Number";
            this.PageNumber.Name = "PageNumber";
            this.PageNumber.Type = typeof(int);
            this.PageNumber.ValueInfo = "0";
            this.PageNumber.Visible = false;
            //
            // BottomMargin
            //
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPageInfo1});
            this.BottomMargin.HeightF = 40.04164F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            // xrLabel2
            //
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(176.25F, 2F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(422.75F, 19.24999F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Заведующий отделом _____________________________";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            //
            // xrPageInfo1
            //
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(149.7084F, 19.25F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.Format = "{0:d MMMM yyyy \'г.\'}";
            //
            // SumPrice
            //
            this.SumPrice.Description = "Sum of Price";
            this.SumPrice.Name = "SumPrice";
            this.SumPrice.Type = typeof(decimal);
            this.SumPrice.ValueInfo = "0";
            this.SumPrice.Visible = false;
            //
            // NBooks
            //
            this.NBooks.Name = "NBooks";
            this.NBooks.Type = typeof(int);
            this.NBooks.ValueInfo = "0";
            this.NBooks.Visible = false;
            //
            // NTitles
            //
            this.NTitles.Name = "NTitles";
            this.NTitles.Type = typeof(int);
            this.NTitles.ValueInfo = "0";
            this.NTitles.Visible = false;
            //
            // ordinary
            //
            this.ordinary.Name = "ordinary";
            //
            // underlined
            //
            this.underlined.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.underlined.Name = "underlined";
            //
            // _bindingSource
            //
            this._bindingSource.DataSource = typeof(InventoryControl.BookInfo);
            //
            // xrTableCell4
            //
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Вып";
            this.xrTableCell4.Weight = 0.17945064382872789D;
            //
            // InvReport
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataSource = this._bindingSource;
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margins = new System.Drawing.Printing.Margins(110, 141, 53, 40);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PageNumber,
            this.SumPrice,
            this.NTitles,
            this.NBooks,
            this.MainTitle});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.ordinary,
            this.underlined});
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource _bindingSource;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell _ksuCell;
        private DevExpress.XtraReports.UI.XRTableCell _numberCell;
        private DevExpress.XtraReports.UI.XRTableCell _descriptionCell;
        private DevExpress.XtraReports.UI.XRTableCell _yearCell;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.Parameters.Parameter PageNumber;
        private DevExpress.XtraReports.Parameters.Parameter SumPrice;
        private DevExpress.XtraReports.Parameters.Parameter NTitles;
        private DevExpress.XtraReports.Parameters.Parameter NBooks;
        private DevExpress.XtraReports.UI.XRControlStyle ordinary;
        private DevExpress.XtraReports.UI.XRControlStyle underlined;
        public DevExpress.XtraReports.UI.XRPageInfo PageNumberInfo;
        private DevExpress.XtraReports.Parameters.Parameter MainTitle;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRTableCell _indexCell;
        private DevExpress.XtraReports.UI.XRTableCell _indexTitleCell;
        private DevExpress.XtraReports.UI.XRTableCell _issueCell;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
    }
}
