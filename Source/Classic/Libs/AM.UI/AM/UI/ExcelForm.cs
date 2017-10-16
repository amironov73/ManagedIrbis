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

using ManagedIrbis;

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
        /// Speedup initialization.
        /// </summary>
        public static void DummyMethod()
        {
            // Do nothing
        }

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
        /// Set height of the cell.
        /// </summary>
        public void SetHeight
            (
                [NotNull] Cell cell,
                double height
            )
        {
            cell.Worksheet.Rows[cell.RowIndex].Height = height;
        }

        /// <summary>
        /// Set width of the cell.
        /// </summary>
        public void SetWidht
            (
                [NotNull] Cell cell,
                double width
            )
        {
            cell.Worksheet.Columns[cell.ColumnIndex].Width = width;
        }

        /// <summary>
        /// Wrap text in the cell.
        /// </summary>
        public void SetWrap
            (
                [NotNull] Cell cell
            )
        {
            cell.Alignment.WrapText = true;
        }

        /// <summary>
        /// Show books
        /// </summary>
        public void ShowBooks
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string template,
                [NotNull][ItemNotNull] ExcelColumn[] columns,
                [NotNull] IDictionary<string, object> dictionary,
                [NotNull][ItemNotNull] MoonExcelData[] header,
                [NotNull][ItemNotNull] IEnumerable<object[]> books,
                [NotNull][ItemNotNull] MoonExcelData[] footer,
                int startRow
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(template, "template");
            Code.NotNull(columns, "columns");
            Code.NotNull(dictionary, "dictionary");
            Code.NotNull(header, "header");
            Code.NotNull(books, "books");
            Code.NotNull(footer, "footer");

            _spreadsheet.LoadDocument(template);
            _spreadsheet.Options.Save.CurrentFileName = string.Format
                (
                    "Документ {0:dd MMMM yyyy}.xslx",
                    DateTime.Today
                );

            Worksheet sheet = _spreadsheet.ActiveWorksheet;
            int row = startRow, col = 0;
            Cell cell;
            foreach (MoonExcelData moonData in header)
            {
                int ourRow = moonData.Row;
                int ourCol = moonData.Column;
                cell = sheet.Cells[ourRow, ourCol];
                string text = moonData.Execute(connection, dictionary);
                cell.Value = text;
            }

            foreach (object[] book in books)
            {
                col = 0;

                foreach (object val in book)
                {
                    ExcelColumn column = columns[col];

                    cell = sheet.Cells[row, col++];
                    cell.Value = val.NullableToString();
                    cell.Alignment.Horizontal
                        = SpreadsheetHorizontalAlignment.Left;
                    cell.Alignment.Vertical
                        = SpreadsheetVerticalAlignment.Top;

                    if (column.Border)
                    {
                        SetBorders(cell);
                    }
                    if (column.Wrap)
                    {
                        SetWrap(cell);
                    }
                    if (column.Height > 0.0)
                    {
                        SetHeight(cell, column.Height);
                    }
                    if (column.Width > 0.0)
                    {
                        SetWidht(cell, column.Width);
                    }
                }

                row++;
            }

            col = 0;

            foreach (MoonExcelData moonData in footer)
            {
                int ourRow = moonData.Row;
                if (moonData.RowRelative)
                {
                    ourRow += row;
                    row = ourRow;
                }
                int ourCol = moonData.Column;
                if (moonData.ColumnRelative)
                {
                    ourCol += col;
                    col = ourCol;
                }
                cell = sheet.Cells[ourRow, ourCol];
                string text = moonData.Execute(connection, dictionary);
                cell.Value = text;
            }

        }

        #endregion
    }
}
