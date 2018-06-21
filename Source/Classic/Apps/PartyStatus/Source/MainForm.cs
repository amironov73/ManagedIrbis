// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

// ReSharper disable UseNameofExpression
// ReSharper disable ConvertToLocalFunction
// ReSharper disable CoVariantArrayConversion

namespace PartyStatus
{
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private MenuFile _menu;

        // Количество обработанных названий книг.
        private int _titleCount;

        // Количество обработанных экземпляров.
        private int _exemplarCount;

        // Количество реально измененных экземпляров.
        private int _changeCount;

        public IrbisConnection GetConnection()
        {
            IrbisConnection result
                = IrbisConnectionUtility.GetClientFromConfig();

            return result;
        }

        private IrbisProvider GetProvider()
        {
            return ProviderManager.GetPreconfiguredProvider();
        }

        private bool TestConnection()
        {
            bool result = false;

            Run(() =>
            {
                using (IrbisProvider provider = GetProvider())
                {
                    FileSpecification specification = new FileSpecification
                        (
                            IrbisPath.MasterFile,
                            provider.Database,
                            "ste.mnu"
                        );
                    _menu = provider.ReadMenuFile(specification);
                    result = true;
                }
            });

            return result;
        }

        private void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            string text = string.Format(format, args)
                + Environment.NewLine;

            _logBox.InvokeIfRequired(() =>
            {
                TextBoxOutput output = (TextBoxOutput) _logBox.Output;
                output.AppendText(text);
            });
        }

        private void Run
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            try
            {
                _busyStripe.Visible = true;
                _busyStripe.Moving = true;
                PseudoAsync.Run(action);
            }
            catch (Exception exception)
            {
                WriteLine("Exception: {0}", exception);
            }
            finally
            {
                _busyStripe.Moving = false;
                _busyStripe.Visible = false;
            }
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();
            bool testResult = TestConnection();
            WriteLine
                (
                    testResult
                    ? "Соединение с сервером успешно установлено"
                    : "Невозможно установить соединение с сервером"
                );

            if (!ReferenceEquals(_menu, null))
            {
                _statusBox.Items.AddRange(_menu.Entries.ToArray());
                if (_menu.Entries.Count != 0)
                {
                    _statusBox.SelectedIndex = 0;
                }
            }
        }

        private void ProcessMfn
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string number,
                int mfn,
                [NotNull] string newStatus
            )
        {
            WriteLine(string.Empty);
            WriteLine("MFN={0}", mfn);

            MarcRecord record = provider.ReadRecord(mfn);
            if (ReferenceEquals(record, null))
            {
                return;
            }

            _titleCount++;

            RecordField[] fields = record.Fields
                .GetField(910)
                .GetField('u', number);
            if (fields.Length == 0)
            {
                return;
            }

            string description = provider.FormatRecord(record, "@sbrief");
            if (!string.IsNullOrEmpty(description))
            {
                WriteLine("{0}", description);
            }

            foreach (RecordField field in fields)
            {
                _exemplarCount++;
                string oldStatus = field.GetFirstSubFieldValue('a');
                string inventory = field.GetFirstSubFieldValue('b');
                bool flag = false;
                if (!oldStatus.SameString(newStatus))
                {
                    _changeCount++;
                    field.SetSubField('a', newStatus);
                    flag = true;
                }
                WriteLine
                    (
                        "{0}: {1} меняем",
                        inventory,
                        flag ? "[" + oldStatus + "]" : "НЕ"
                    );
            }

            if (record.Modified)
            {
                WriteLine("Сохраняем запись");
                provider.WriteRecord(record);
            }
        }

        private void ProcessNumber
            (
                [NotNull] string number,
                [NotNull] string status
            )
        {
            _titleCount = 0;
            _exemplarCount = 0;
            _changeCount = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (IrbisProvider provider = GetProvider())
            {
                WriteLine("КСУ {0}", number);

                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        "NKSU=",
                        number
                    );
                int[] found = provider.Search(expression);
                WriteLine("Найдено: {0}", found.Length);

                foreach (int mfn in found)
                {
                    ProcessMfn(provider, number, mfn, status);
                }
            }

            WriteLine(new string('=', 70));
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            WriteLine("Затрачено: {0}", elapsed.ToMinuteString());
            WriteLine("обработано названий: {0}", _titleCount);
            WriteLine("экземпляров: {0}", _exemplarCount);
            WriteLine("изменён статус: {0}", _changeCount);
            WriteLine(string.Empty);
        }

        private void _goButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            string number = _numberBox.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            MenuEntry entry = (MenuEntry) _statusBox.SelectedItem;
            string status = entry.Code.ThrowIfNull("entry.Code");

            Run(() =>
                {
                    ProcessNumber(number, status);
                });
        }

        #endregion
    }
}
