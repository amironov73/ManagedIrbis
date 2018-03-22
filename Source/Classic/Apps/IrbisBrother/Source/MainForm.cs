// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
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
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Logging;
using AM.Windows.Forms;

using ManagedIrbis;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace IrbisBrother
{
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
            _Clear();
        }

        #endregion

        #region Private members

        private string[] _lines;

        private void _Clear()
        {
            _browser.Navigate("about:blank");
        }

        public void PrintCodes()
        {
            using (PrintDocument document = new PrintDocument())
            {
                document.PrinterSettings.PrinterName = CM.AppSettings["printer"];
                document.PrinterSettings.Copies = (short) _amountBox.NumericUpDown.Value;
                document.DefaultPageSettings.Margins = new Margins(2, 2, 2, 2);
                document.PrintPage += _PrintPage;
                document.Print();
            }
        }

        private void _PrintPage(object sender, PrintPageEventArgs e)
        {
            string fontName = CM.AppSettings["font-name"];
            float fontSize = float.Parse(CM.AppSettings["font-size"]);

            Graphics g = e.Graphics;
            string s = string.Join(Environment.NewLine, _lines);
            using (Brush brush = new SolidBrush(Color.Black))
            using (StringFormat format = new StringFormat())
            using (Font font = new Font(fontName, fontSize))
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Near;
                format.HotkeyPrefix = HotkeyPrefix.None;
                format.Trimming = StringTrimming.None;
                RectangleF rect = e.PageBounds;
                g.DrawString(s, font, brush, rect, format);
                e.HasMorePages = false;
            }
        }

        #endregion

        private void _printButton_Click(object sender, EventArgs e)
        {
            if (!ReferenceEquals(_lines, null))
            {
                PrintCodes();
                _barcodeBox.Focus();
            }
        }

        private void _barcodeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            _browser.DocumentText = string.Empty;
            _lines = null;

            string barcode = _barcodeBox.Text.Trim();
            if (string.IsNullOrEmpty(barcode))
            {
                return;
            }

            _barcodeBox.Clear();

            try
            {
                string connectionString = CM.AppSettings["connectionString"];
                using (IrbisConnection connection = new IrbisConnection(connectionString))
                {
                    MarcRecord record = connection.SearchReadOneRecord("\"IN={0}\"", barcode);
                    if (ReferenceEquals(record, null))
                    {
                        _browser.DocumentText = "Не найдена книга";
                        _barcodeBox.Focus();
                        return;
                    }

                    string formatted = connection.FormatRecord("@", record.Mfn);
                    _browser.DocumentText = formatted;

                    string firstLine = record.FM(906), secondLine = record.FM(908);
                    if (string.IsNullOrEmpty(firstLine))
                    {
                        _browser.DocumentText = "Не введен систематический шифр (поле 906)";
                        _barcodeBox.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(secondLine))
                    {
                        _browser.DocumentText = "Не введен авторский знак (поле 908)";
                        _barcodeBox.Focus();
                        return;
                    }

                    _lines = new[] {firstLine, secondLine};
                }
            }
            catch (Exception exception)
            {
                _browser.DocumentText = exception.ToString();
            }
        }
    }
}
