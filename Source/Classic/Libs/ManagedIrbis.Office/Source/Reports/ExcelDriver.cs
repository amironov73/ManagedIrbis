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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Drawing;

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
        /// Name of the template file.
        /// </summary>
        [CanBeNull]
        public string InputFile { get; set; }

        /// <summary>
        /// Name of the result file.
        /// </summary>
        [CanBeNull]
        public string OutputFile { get; set; }

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

            foreach (var pair in cell.Attributes)
            {
                int intValue;
                double dblValue;
                bool enabled;
                Color color;
                string strValue;

                Cell curCell = _worksheet.Cells[_row, _column];
                switch (pair.Key)
                {
                    case ReportAttribute.BackColor:
                        color = ((string)pair.Value).ToColor();
                        curCell.FillColor = color;
                        break;

                    case ReportAttribute.Bold:
                        enabled = (bool)pair.Value;
                        curCell.Font.Bold = enabled;
                        break;

                    case ReportAttribute.Borders:
                        enabled = (bool)pair.Value;
                        if (enabled)
                        {
                            curCell.Borders.SetAllBorders
                                (
                                    Color.Black,
                                    BorderLineStyle.Medium
                                );
                        }
                        else
                        {
                            curCell.Borders.RemoveBorders();
                        }
                        break;

                    case ReportAttribute.Column:
                        intValue = Convert.ToInt32(pair.Value);
                        _column += intValue;
                        break;

                    case ReportAttribute.FontName:
                        strValue = (string)pair.Value;
                        curCell.Font.Name = strValue;
                        break;

                    case ReportAttribute.FontSize:
                        dblValue = (double)pair.Value;
                        curCell.Font.Size = dblValue;
                        break;

                    case ReportAttribute.ForeColor:
                        color = ((string)pair.Value).ToColor();
                        curCell.Font.Color = color;
                        break;

                    case ReportAttribute.HorizontalAlign:
                        strValue = (string)pair.Value;
                        SpreadsheetHorizontalAlignment horizontal
                            = SpreadsheetHorizontalAlignment.Left;
                        switch (strValue)
                        {
                            case "Center":
                                horizontal = SpreadsheetHorizontalAlignment.Center;
                                break;

                            case "Right":
                                horizontal = SpreadsheetHorizontalAlignment.Right;
                                break;
                        }
                        curCell.Alignment.Horizontal = horizontal;
                        break;

                    case ReportAttribute.Italic:
                        enabled = (bool)pair.Value;
                        curCell.Font.Italic = enabled;
                        break;

                    case ReportAttribute.Number:
                        // TODO: implement
                        break;

                    case ReportAttribute.Span:
                        // TODO: implement
                        break;

                    case ReportAttribute.Underline:
                        enabled = (bool)pair.Value;
                        curCell.Font.UnderlineType = enabled
                            ? UnderlineType.Single
                            : UnderlineType.None;
                        break;

                    case ReportAttribute.VerticalAlign:
                        strValue = (string)pair.Value;
                        SpreadsheetVerticalAlignment vertical
                            = SpreadsheetVerticalAlignment.Top;
                        switch (strValue)
                        {
                            case "Bottom":
                                vertical = SpreadsheetVerticalAlignment.Bottom;
                                break;

                            case "Center":
                                vertical = SpreadsheetVerticalAlignment.Center;
                                break;
                        }
                        curCell.Alignment.Vertical = vertical;
                        break;

                    case ReportAttribute.Width:
                        dblValue = Convert.ToDouble(pair.Value);
                        curCell.ColumnWidth = dblValue;
                        break;

                    case ReportAttribute.WrapText:
                        enabled = (bool)pair.Value;
                        curCell.Alignment.WrapText = enabled;
                        break;
                }
            }
        }

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            _workbook = new Workbook();

            string inputFile = InputFile;
            if (string.IsNullOrEmpty(inputFile))
            {
                _workbook.CreateNewDocument();
            }
            else
            {
                _workbook.LoadDocument(inputFile);
            }
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

            foreach (var pair in band.Attributes)
            {
                int offset;

                switch (pair.Key)
                {
                    case ReportAttribute.Row:
                        offset = Convert.ToInt32(pair.Value);
                        _row += offset;
                        break;

                    case ReportAttribute.Column:
                        offset = Convert.ToInt32(pair.Value);
                        _column += offset;
                        break;
                }
            }
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
            string fileName = OutputFile
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
