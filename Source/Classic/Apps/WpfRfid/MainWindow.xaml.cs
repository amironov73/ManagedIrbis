using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AM.Configuration;
using AM.Rfid;
using AM.Windows;

using ManagedIrbis;

using CM = System.Configuration.ConfigurationManager;

namespace WpfRfid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnLoad();
        }

        private void ShowMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);
            errorBox.Text = message;
        }

        private IrbisConnection GetClient()
        {
            IrbisConnection result = new IrbisConnection();
            string connectionString = CM.AppSettings["connection-string"];
            result.ParseConnectionString(connectionString);
            result.Connect();
            return result;
        }

        private RfidDriver GetDriver()
        {
            RfidCardmanDriver result = new RfidCardmanDriver();
            //string[] readers = result.GetReaders();

            return result;
        }

        private bool IsRfidRunning()
        {
            try
            {
                GetDriver();
                //rfid.Connect("OMNIKEY CardMan 5x21 0");
                //rfid.Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private string ReadRfidLabel()
        {
            try
            {
                using (RfidDriver rfid = GetDriver())
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    long rfidTimeout = ConfigurationUtility
                        .GetInt64("rfid-timeout", 5000);
                    while (watch.ElapsedMilliseconds < rfidTimeout)
                    {
                        string[] labels = rfid.Inventory();
                        if (labels.Length == 1)
                        {
                            return labels[0];
                        }

                        if (labels.Length > 1)
                        {
                            ShowMessage
                                (
                                    "Слишком много меток: {0}",
                                    string.Join("; ", labels.ToArray())
                                );

                            return null;
                        }

                        WpfUtility.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString());

                return null;
            }

            ShowMessage("Закончилось время ожидания");
            return null;
        }

        private bool IsChecked(CheckBox checkBox)
        {
            return checkBox.IsChecked ?? false;
        }

        private string[] SearchFormat
            (
                IrbisConnection client,
                string expression,
                string format
            )
        {
            int[] found = client.Search(expression);
            string[] result = client.FormatRecords(client.Database, format, found);

            return result;
        }

        private string GetBiblioDescription(string number)
        {
            using (IrbisConnection client = GetClient())
            {
                string expression;
                string[] found;

                if (!IsChecked(offBox))
                {
                    expression = string.Format
                        (
                            "\"INS={0}\"",
                            number
                        );

                    found = SearchFormat(client, expression, "@brief");

                    if ((found != null)
                        && (found.Length != 0))
                    {
                        if (!IsChecked(offBox))
                        {
                            ShowMessage("Книга списана\r\n" + found[0]);
                            return null;
                        }
                    }
                }

                expression = string.Format
                    (
                        "\"IN={0}\" + \"INS={0}\"",
                        number
                    );

                found = SearchFormat(client, expression, "@brief");

                if (found.Length > 1)
                {
                    ShowMessage("Много книг с инвентарным номером {0}", number);

                    return null;
                }

                return found.FirstOrDefault();
            }
        }

        private string FindRfidLabel(string rfid)
        {
            using (IrbisConnection client = GetClient())
            {
                string expression = string.Format
                    (
                        "\"IN={0}\"",
                        rfid
                    );

                string[] found = SearchFormat(client, expression, "@brief");

                if (found.Length > 1)
                {
                    ShowMessage("Много книг привязано к метке {0}", rfid);
                }

                return found.FirstOrDefault();
            }
        }

        private void Clear()
        {
            numberBox.Text = string.Empty;
            resultBox.Text = string.Empty;
            errorBox.Text = string.Empty;
            numberBox.Focus();
            _bookChecked = false;
        }

        private bool _bookChecked = false;

        private void SaveProtocol()
        {
            string number = numberBox.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            string description = GetBiblioDescription(number);
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            string contents = $"{number}\t{description}{Environment.NewLine}";
            Encoding encoding = Encoding.UTF8;
            File.AppendAllText
                (
                    "protocol.txt",
                    contents,
                    encoding
                );
        }

        private void OnLoad()
        {
            if (!IsRfidRunning())
            {
                MessageBox.Show("Не удалось открыть RFID!");
                App.Current.Shutdown();
            }

            string sigla = CM.AppSettings["sigla"];
            if (!string.IsNullOrEmpty(sigla))
            {
                siglaBox.Text = sigla.Trim();
            }

            string enableWrittenOff = CM.AppSettings["enable-written-off"];
            if (string.Compare(enableWrittenOff, "yes",
                StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                offBox.IsEnabled = true;
                offBox.IsChecked = true;
            }

            string enableRewrite = CM.AppSettings["enable-rewrite"];
            if (string.Compare(enableRewrite, "yes",
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                rewriteBox.IsEnabled = true;
                rewriteBox.IsChecked = true;
            }

            string lockSigla = CM.AppSettings["lock-sigla"];
            if (string.Compare(lockSigla, "yes",
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                siglaBox.IsReadOnly = true;
            }

            string eas = CM.AppSettings["eas"];
            if (string.Compare(eas, "yes",
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                easBox.IsChecked = true;
            }

            string protocol = CM.AppSettings["protocol"];
            if (string.Compare(protocol, "yes",
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                protocolBox.IsChecked = true;
            }

            string lockCheckboxes = CM.AppSettings["lock-checkboxes"];
            if (string.Compare(lockCheckboxes, "yes",
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                offBox.IsEnabled = false;
                rewriteBox.IsEnabled = false;
                protocolBox.IsEnabled = false;
                easBox.IsEnabled = false;
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            _bookChecked = false;
            string number = numberBox.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                ShowMessage("Инвентарный номер не введен!");
            }
            else
            {
                string description = GetBiblioDescription(number);
                resultBox.Text = description ?? "Книга не найдена";
                if (!string.IsNullOrEmpty(description))
                {
                    _bookChecked = true;
                }
            }
        }

        private bool _busy = false;

        private void BindButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_bookChecked)
            {
                ShowMessage("Книга не проверена!");
                return;
            }

            if (_busy)
            {
                return;
            }

            try
            {
                _busy = true;

                string number = numberBox.Text.Trim();
                if (string.IsNullOrEmpty(number))
                {
                    ShowMessage("Не введен инвентарный номер");
                    return;
                }

                string description = GetBiblioDescription(number);
                if (string.IsNullOrEmpty(description))
                {
                    ShowMessage("Книга не найдена");
                    return;
                }

                string rfid = ReadRfidLabel();
                if (string.IsNullOrEmpty(rfid))
                {
                    return;
                }

                if (!IsChecked(rewriteBox))
                {
                    description = FindRfidLabel(rfid);
                    if (!string.IsNullOrEmpty(description))
                    {
                        ShowMessage("Метка привязана к книге:\r\n{0}", description);
                        return;
                    }
                }

                bool aes = IsChecked(easBox);
                if (aes)
                {
                    //using (IRfidManager manager = new RfidManager())
                    //{
                    //    manager.Open = true;
                    //    manager.SetEasBit(rfid, true);
                    //}
                }

                using (IrbisConnection client = GetClient())
                {
                    string expression = string.Format
                        (
                            "\"INS={0}\"",
                            number
                        );

                    int[] mfns = client.Search(expression);
                    if ((mfns != null)
                        && (mfns.Length != 0))
                    {
                        if (!IsChecked(offBox))
                        {
                            ShowMessage("Книга списана");
                            return;
                        }
                    }

                    expression = string.Format
                        (
                            "\"IN={0}\" + \"INS={0}\"",
                            number
                        );

                    mfns = client.Search(expression);

                    if (mfns.Length == 0)
                    {
                        ShowMessage("Книга не найдена");
                        return;
                    }

                    if (mfns.Length > 1)
                    {
                        ShowMessage("Много книг с инвентарным номером {0}", number);
                        return;
                    }

                    string sigla = siglaBox.Text.Trim();

                    if (sigla.Length > 0)
                    {
                        if ((sigla[0] != 'Ф') || (sigla.Length > 4)
                            || sigla.Contains(' '))
                        {
                            ShowMessage("Плохая сигла!");
                            return;
                        }
                    }

                    MarcRecord record = client.ReadRecord(mfns[0]);

                    RecordField field = record.Fields
                                              .GetField(910)
                                              .GetField('b', number)
                                              .FirstOrDefault();

                    if (field == null)
                    {
                        ShowMessage("Не найдено поле 910^b={0}", number);
                        return;
                    }

                    field
                        .SetSubField('h', rfid) // Радиометка
                        .SetSubField('a', "0")  // Статус
                        .RemoveSubField('v')    // Номер акта списания
                        ;

                    if (!string.IsNullOrEmpty(sigla))
                    {
                        field.SetSubField('d', sigla);
                    }

                    client.WriteRecord(record, false, true);

                    if (IsChecked(protocolBox))
                    {
                        SaveProtocol();
                    }
                }

                Clear();
            }
            catch (Exception exception)
            {
                ShowMessage("Ошибка:\r\n{0}", exception);
            }
            finally
            {
                _busy = false;
                _bookChecked = false;
            }
        }

        private void NumberBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _bookChecked = false;
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }
    }
}
