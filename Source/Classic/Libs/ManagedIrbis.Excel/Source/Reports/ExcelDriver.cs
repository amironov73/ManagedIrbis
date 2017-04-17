// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExcelDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using CodeJam;

using DevExpress.Spreadsheet;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExcelDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Name of the result file.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private int _row, _column;

        private StringBuilder _accumulatedText;

        private Workbook _workbook;

        private DevExpress.Spreadsheet.Worksheet _worksheet;

        #endregion

        #region Public methods

        /// <inheritdoc cref="ReportDriver.BeginCell"/>
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            _accumulatedText = new StringBuilder();
        }

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            _workbook = new Workbook();
            _workbook.CreateNewDocument();
            _worksheet = _workbook.Worksheets[0];
            _row = 0;
            _column = 0;
        }

        /// <inheritdoc cref="ReportDriver.BeginRow"/>
        public override void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            _column = 0;
        }

        /// <inheritdoc cref="ReportDriver.EndCell"/>
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            _worksheet.Cells[_row, _column].Value
                = _accumulatedText.ToString();

            _column++;
        }

        /// <inheritdoc cref="ReportDriver.EndDocument"/>
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            string fileName = FileName
                .ThrowIfNull("File name not set");
            _workbook.SaveDocument(fileName);
        }

        /// <inheritdoc cref="ReportDriver.EndRow"/>
        public override void EndRow
            (
                ReportContext context,
            ReportBand band
            )
        {
            _row++;
        }

        /// <inheritdoc cref="ReportDriver.Write"/>
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _accumulatedText.Append(text);
            }
        }

        #endregion
    }
}
