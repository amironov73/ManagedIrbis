// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using ManagedIrbis;

#endregion

namespace HudoInvent
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    public sealed partial class MainForm
        : Form
    {
        private HudoInfo _currentBook;
        private int _counter;
        private readonly List<string> _report;

        public MainForm()
        {
            _report = new List<string>();
            InitializeComponent();
        }

        private void ShowBusy()
        {
            _busyStripe.Moving = true;
            _busyStripe.Visible = true;
        }

        private void HideBusy()
        {
            _busyStripe.Moving = false;
            _busyStripe.Visible = false;
        }

        public void ClearBrowser
            (
                WebBrowser browser
            )
        {
            if (browser.Disposing || browser.IsDisposed)
            {
                return;
            }

            browser.Navigate("about:blank");
        }

        public void SetBrowserText
            (
                WebBrowser browser,
                string html
            )
        {
            if (browser.Disposing || browser.IsDisposed)
            {
                return;
            }

            browser.DocumentText = html;
        }

        public bool PrepareBrowser
            (
                WebBrowser browser
            )
        {
            if (browser.Disposing || browser.IsDisposed)
            {
                return false;
            }

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    browser.Navigate("about:blank");
                    while (browser.IsBusy)
                    {
                        Application.DoEvents();
                    }

                    browser.DocumentText = "&nbsp;";

                    PseudoAsync.SleepALittle();
                }
                catch (Exception exception)
                {
                    WriteLog(exception.Message);
                    return false;
                }
            }

            return true;
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                ShowBusy();
                ControlCenter.Initialize(_logBox.Output);
                PrepareBrowser(_bookBrowser);
                WriteLog("Готовы к работе");
            }
            catch
            {
                WriteLog("НЕ ГОТОВЫ К РАБОТЕ");
            }

            HideBusy();
        }

        private void WriteLog
            (
                string format,
                params object[] args
            )
        {
            _logBox.Output.WriteLine(format, args);
        }

        private static string Error(string text)
            => "<h1><font color='red'>" + text + "</font></h1>";

        private async void _checkButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _currentBook = null;

            var barcode = _barcodeBox.Text.Trim();
            if (string.IsNullOrEmpty(barcode))
            {
                return;
            }

            _barcodeBox.Clear();
            ClearBrowser(_bookBrowser);

            ShowBusy();
            try
            {
                _currentBook = await Task.Run
                    (
                        () => ControlCenter.GetBook(barcode)
                    );
            }
            finally
            {
                HideBusy();
            }

            if (_currentBook == null)
            {
                SetBrowserText(_bookBrowser, Error("Книга не найдена"));
                return;
            }

            var builder = new StringBuilder(1024);
            if (_currentBook.CurrentExemplar == null)
            {
                SetBrowserText(_bookBrowser, Error("Непонятки с экземплярами"));
                return;
            }

            var checkedDate = _currentBook.CurrentExemplar.CheckedDate;
            if (checkedDate != null)
            {
                WriteLog("{0}: уже проверена", barcode);
                builder.AppendFormat
                    (
                        Error("Уже проверена {0}"),
                        new IrbisDate(checkedDate).Date.ToShortDateString()
                    );
                builder.AppendLine();
            }

            if (_currentBook.Ticket != null)
            {
                WriteLog("{0}: на руках", barcode);
                builder.AppendFormat(Error("На руках: {0}"), _currentBook.Ticket);
                builder.AppendLine();
            }

            if (_currentBook.Number != null && _currentBook.CurrentExemplar != null)
            {
                var place = _currentBook.CurrentExemplar.Place
                            ?? "<font color='red'>БЕЗ МЕСТА</font>";
                builder.AppendFormat
                    (
                        "<h1>{0}: {1}</h1>",
                        place,
                        _currentBook.Number
                    );
                builder.AppendLine();
            }
            else
            {
                WriteLog("{0}: непонятки со штрих-кодом");
                builder.AppendFormat(Error("Непонятки со штрих-кодом!"));
                builder.AppendLine();
            }

            builder.AppendLine(_currentBook.Record.Description);

            SetBrowserText(_bookBrowser, builder.ToString());
            WriteLog
                (
                    "{0} => {1}: {2}",
                    barcode,
                    _currentBook.Number,
                    _currentBook.Description
                );
        }

        private async void _idleTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                ShowBusy();
                await Task.Run(ControlCenter.IdleAction);
            }
            finally
            {
                HideBusy();
            }
        }

        private void _barcodeBox_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.Enter)
            {
                _checkButton_Click(sender, e);
                e.IsInputKey = false;
            }

            if (e.KeyCode == Keys.F2)
            {
                _confirmButton_Click(sender, e);
                e.IsInputKey = false;
            }
        }

        private async void _confirmButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ClearBrowser(_bookBrowser);
            if (_currentBook == null)
            {
                return;
            }

            try
            {
                ShowBusy();
                var book = _currentBook;
                bool result = await Task.Run
                    (
                        () => ControlCenter.ConfirmBook(book)
                    );

                if (!result)
                {
                    SetBrowserText(_bookBrowser, Error("ОШИБКА при сохранении записи"));
                    return;
                }
            }
            catch (Exception exception)
            {
                SetBrowserText(_bookBrowser, "<pre>" + exception + "</pre>");
                return;
            }
            finally
            {
                HideBusy();
            }

            _report.Add($"{_currentBook.Number}: {_currentBook.Description}");

            ++_counter;
            _counterBox.Text = _counter.ToInvariantString();
            _barcodeBox.Focus();
        }

        private void MainForm_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.F2)
            {
                _confirmButton_Click(sender, e);
                e.IsInputKey = false;
            }
        }

        private void _reportButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            var reportText = string.Join
                (
                    Environment.NewLine,
                    _report
                );

            if (string.IsNullOrWhiteSpace(reportText))
            {
                MessageBox.Show("Отчет пуст");
            }
            else
            {
                if (_saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText
                        (
                            _saveFileDialog1.FileName,
                            reportText
                        );
                }
            }
        }
    }
}
