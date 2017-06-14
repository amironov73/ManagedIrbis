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
                [NotNull] string template,
                [NotNull] IEnumerable<object[]> books,
                int startRow
            )
        {
            _spreadsheet.LoadDocument(template);
            _spreadsheet.Options.Save.CurrentFileName = string.Format
                (
                    "Документ {0:dd MMMM yyyy}.xslx",
                    DateTime.Today
                );

            int row = startRow;
            Worksheet sheet = _spreadsheet.ActiveWorksheet;

            foreach (object[] book in books)
            {
                int col = 0;

                foreach (object val in book)
                {
                    Cell cell = sheet.Cells[row, col++];
                    cell.Value = val.NullableToString();
                    cell.Alignment.Horizontal
                        = SpreadsheetHorizontalAlignment.Left;
                    cell.Alignment.Vertical
                        = SpreadsheetVerticalAlignment.Top;
                    SetBorders(cell);
                }

                row++;
            }
        }

        #endregion
    }
}
