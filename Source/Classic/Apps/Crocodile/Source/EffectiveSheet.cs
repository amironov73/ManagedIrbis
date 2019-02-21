// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Windows.Forms;

using AM.Windows.Forms;

using DevExpress.Spreadsheet;

using JetBrains.Annotations;

#endregion

namespace Crocodile
{
    class EffectiveSheet
        : IDisposable
    {
        public Control Control;
        public IWorkbook Workbook;
        private readonly Worksheet _worksheet;
        private int _currentRow;

        public int CurrentRow => _currentRow;

        public EffectiveSheet()
        {
            Workbook = new Workbook();
            _worksheet = Workbook.Worksheets[0];
            _currentRow = 0;
        }

        public EffectiveSheet
            (
                [NotNull] IWorkbook workbook
            )
        {
            Workbook = workbook;
            _worksheet = workbook.Worksheets[0];
            _currentRow = 0;
        }

        public void Clear()
        {
            _worksheet.Clear(_worksheet.Cells);
        }

        [NotNull]
        public Row CurrentLine()
        {
            return _worksheet.Rows[_currentRow];
        }

        public void Dispose()
        {
            Workbook.Dispose();
        }

        [NotNull]
        public Cell WriteCell(int column, string text)
        {
            Cell result = null;
            Control.InvokeIfRequired(() =>
            {
                result = _worksheet.Cells[_currentRow, column];
                result.Value = text;
            });

            return result;
        }

        [NotNull]
        public Cell WriteCell(int column, int value)
        {
            Cell result = null;
            Control.InvokeIfRequired(() =>
            {
                result = _worksheet.Cells[_currentRow, column];
                result.Value = value;
            });

            return result;
        }

        [NotNull]
        public Cell WriteCell(int column, double value, string format)
        {
            Cell result = null;
            Control.InvokeIfRequired(() =>
            {
                result = _worksheet.Cells[_currentRow, column];
                result.Value = value;
                result.NumberFormat = format;
            });

            return result;
        }

        [NotNull]
        public Cell WriteCell(int column, decimal value, string format)
        {
            Cell result = null;
            Control.InvokeIfRequired(() =>
            {
                result = _worksheet.Cells[_currentRow, column];
                result.Value = (double)value;
                result.NumberFormat = format;
            });

            return result;
        }

        [NotNull]
        public Row WriteLine
            (
                string format,
                params object[] args
            )
        {
            WriteCell(0, string.Format(format, args));
            var result = CurrentLine();
            NewLine();

            return result;
        }

        [NotNull]
        public EffectiveSheet Invoke
            (
                [NotNull] MethodInvoker action
            )
        {
            Control.InvokeIfRequired(action);

            return this;
        }

        public void NewLine()
        {
            _currentRow++;
        }

        public Range GetRange(int column, int topRow, int bottomRow)
        {
            return _worksheet.Range.FromLTRB(column, topRow, column, bottomRow);
        }

        public void SaveDocument
            (
                string fileName
            )
        {
            Workbook.SaveDocument(fileName);
        }
    }
}
