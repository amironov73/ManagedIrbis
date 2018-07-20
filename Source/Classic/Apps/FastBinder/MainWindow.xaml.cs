using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

using AM.Text;
using AM.Text.Ranges;

using ManagedIrbis;
using ManagedIrbis.Magazines;
using CM = System.Configuration.ConfigurationManager;

namespace FastBinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private IrbisConnection _connection;
        private MarcRecord _mainRecord;
        private string _index, _year, _issues, _month,
            _number, _inventory, _fond, _complect,
            _description, _reference;

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            _index = IndexBox.Text.Trim();
            _year = YearBox.Text.Trim();
            _issues = IssueBox.Text.Trim();
            _month = MonthBox.Text.Trim();
            _number = NumberBox.Text.Trim();
            _inventory = InventoryBox.Text.Trim();
            _fond = FondBox.Text.Trim();
            _complect = ComplectBox.Text.Trim();
            if (string.IsNullOrEmpty(_index)
                || string.IsNullOrEmpty(_year)
                || string.IsNullOrEmpty(_issues)
                || string.IsNullOrEmpty(_month)
                || string.IsNullOrEmpty(_number)
                || string.IsNullOrEmpty(_inventory)
                || string.IsNullOrEmpty(_fond)
                || string.IsNullOrEmpty(_complect))
            {
                WriteLog("Empty data");
                return;
            }

            var collection = NumberRangeCollection.Parse(_issues);

            _description = $"Подшивка N{_number} {_month} ({_issues})";
            _reference = $"{_index}/{_year}/{_description}";

            string connectionString = CM.AppSettings["connectionString"];
            using (_connection = new IrbisConnection(connectionString))
            {
                WriteLog("Connected");

                _mainRecord = _connection.SearchReadOneRecord("\"I={0}\"", _index);
                if (_mainRecord == null)
                {
                    WriteLog("Main record not found");
                    return;
                }

                MagazineInfo magazine = MagazineInfo.Parse(_mainRecord);
                WriteLog("Main: {0}", magazine.ExtendedTitle);

                foreach (NumberText number in collection)
                {
                    MarcRecord issue = new MarcRecord
                    {
                        Database = _connection.Database
                    };

                    string issueIndex = $"{_index}/{_year}/{number}";
                    issue
                        .AddField(933, _index)
                        .AddField(903, issueIndex)
                        .AddField(934, _year)
                        .AddField(936, number)
                        .AddField(920, "NJP")
                        .AddField
                            (
                                new RecordField(910)
                                    .AddSubField('a', "0")
                                    .AddSubField('b', _complect)
                                    .AddSubField('c', "?")
                                    .AddSubField('d', _fond)
                                    .AddSubField('p', _reference)
                                    .AddSubField('i', _inventory)
                            )
                        .AddField
                            (
                                new RecordField(463)
                                    .AddSubField('w', _reference)
                            );

                    _connection.WriteRecord(issue);
                    WriteLog("Issue record created: N={0}, MFN={1}", number, issue.Mfn);
                }

                MarcRecord binding = new MarcRecord
                {
                    Database = _connection.Database
                };
                binding
                    .AddField(933, _index)
                    .AddField(903, _reference)
                    .AddField(904, _year)
                    .AddField(936, _description)
                    .AddField(931, _issues)
                    .AddField(920, "NJK")
                    .AddField
                    (
                        new RecordField(910)
                            .AddSubField('a', "0")
                            .AddSubField('b', _inventory)
                            .AddSubField('c', "?")
                            .AddSubField('d', _fond)
                    );

                _connection.WriteRecord(binding);
                WriteLog("Binding record created: MFN={0}", binding.Mfn);

                _mainRecord.AddField
                    (
                        new RecordField(909)
                            .AddSubField('q', _year)
                            .AddSubField('d', _fond)
                            .AddSubField('k', _complect)
                            .AddSubField('h', _issues)
                    );
                _connection.WriteRecord(_mainRecord);
                WriteLog("Cumulation updated");
            }

            WriteLog("Disconnected");
            WriteLog("==========================================");
        }

        private void WriteLog(string format, params object[] args)
        {
            string text = string.Format(format, args);
            int index = LogList.Items.Add(text);
            object justInserted = LogList.Items[index];
            LogList.ScrollIntoView(justInserted);
            DoEvents();
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate { }));
        }
    }
}
