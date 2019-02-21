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
        private Worksheet _worksheet;
        private int _currentRow;

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

        public void Dispose()
        {
            Workbook.Dispose();
        }

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

        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            WriteCell(0, string.Format(format, args));
            NewLine();
        }

        public void NewLine()
        {
            _currentRow++;
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
