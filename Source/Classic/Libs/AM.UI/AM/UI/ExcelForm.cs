// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExcelForm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using DevExpress.CodeParser;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

#endregion

namespace AM.UI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class ExcelForm 
        : XtraForm
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set cell borders.
        /// </summary>
        public void SetBorders
            (
                [NotNull] Cell cell
            )
        {
            cell.Borders.SetAllBorders
                (
                    Color.Black,
                    BorderLineStyle.Thin
                );
        }

        /// <summary>
        /// Show books
        /// </summary>
        public void ShowBooks
            (
                [NotNull] IEnumerable<ExemplarInfo> books,
                [NotNull] string startNumber
            )
        {
            _spreadsheet.LoadDocument("Template.xlsx");
            _spreadsheet.Options.Save.CurrentFileName = string.Format
                (
                    "Документ {0:dd MMMM yyyy}.xslx",
                    DateTime.Today
                );

            int count;
            int.TryParse(startNumber, out count);

            int row = 7;
            Worksheet sheet = _spreadsheet.ActiveWorksheet;
            int col;

            foreach (ExemplarInfo book in books)
            {
                col = 0;

                Cell countCell = sheet.Cells[row, col++];
                countCell.Value = ++count;
                countCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                countCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                SetBorders(countCell);

                Cell numberCell = sheet.Cells[row, col++];
                numberCell.Value = book.Number;
                numberCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                numberCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                SetBorders(numberCell);

                Cell descrCell = sheet.Cells[row, col++];
                descrCell.Value = book.Description;
                descrCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                descrCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                descrCell.Alignment.WrapText = true;
                SetBorders(descrCell);

                Cell yearCell = sheet.Cells[row, col++];
                yearCell.Value = book.Year;
                yearCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                yearCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                SetBorders(yearCell);

                Cell priceCell = sheet.Cells[row, col++];
                priceCell.Value = book.Price;
                priceCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                priceCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                SetBorders(priceCell);

                Cell indexCell = sheet.Cells[row, col++];
                indexCell.Value = book.ShelfIndex;
                indexCell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                indexCell.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                SetBorders(indexCell);

                Cell signCell = sheet.Cells[row, col];
                signCell.Value = " ";
                SetBorders(signCell);

                row++;
            }

            row += 2;
            col = 2;

            Cell cell = sheet.Cells[row, col];
            cell.Value = "Всего экземпляров: " + count;
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
        }

        #endregion
    }
}
