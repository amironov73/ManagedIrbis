// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.Text;
using AM.Text.Output;

using DevExpress.XtraSpreadsheet;

using ManagedIrbis;
using ManagedIrbis.Search;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo

namespace Crocodile
{
    public partial class MainForm : Form
    {
        private const string Nksu = "NKSU=";

        public AbstractOutput Log => _logBox.Output;
        public SpreadsheetControl Spreadsheet { get; set; }

        public MainForm()
        {
            InitializeComponent();

            Spreadsheet = new SpreadsheetControl();
            _splitContainer.Panel2.Controls.Add(Spreadsheet);
            Spreadsheet.Dock = DockStyle.Fill;
        }

        public string CurrentYear
        {
            get
            {
                string result = _yearBox.SelectedItem as string
                                ?? string.Empty;

                return result;
            }
        }

        public IrbisConnection GetConnection()
        {
            string connectionString = ConfigurationUtility
                .GetString("connectionString")
                .ThrowIfNull("connectionString");

            return new IrbisConnection(connectionString);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ComboBox.ObjectCollection items = _yearBox.Items;
            items.Clear();
            int year = DateTime.Today.Year;
            for (int i = 0; i < 10; i++)
            {
                items.Add(year.ToInvariantString());
                year--;
            }

            _yearBox.SelectedIndex = 0;
        }

        public TermInfo[] ReadTerms()
        {
            string prefix = Nksu + CurrentYear;
            LocalList<TermInfo> list = new LocalList<TermInfo>();
            string startTerm = prefix;
            using (IrbisConnection connection = GetConnection())
            {
                // TODO skip duplicates
                bool flag = true;
                while (flag)
                {
                    TermParameters parameters = new TermParameters
                    {
                        Database = connection.Database,
                        StartTerm = startTerm,
                        NumberOfTerms = 200
                    };
                    TermInfo[] terms = connection.ReadTerms(parameters);
                    if (terms.Length == 0)
                    {
                        break;
                    }

                    foreach (TermInfo term in terms)
                    {
                        if (!term.Text.SafeStarts(prefix))
                        {
                            flag = false;
                            break;
                        }
                        list.Add(term);
                    }

                    startTerm = terms.Last().Text;
                }
            }

            var comparison = new Comparison<TermInfo>
                (
                    (TermInfo left, TermInfo right) => NumberText.Compare(left.Text, right.Text)
                );
            var result = TermInfo.TrimPrefix(list.ToArray(), Nksu);
            Array.Sort<TermInfo>(result, comparison);

            return result;
        }

        private void _yearBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            _termGrid.AutoGenerateColumns = false;
            _termGrid.DataSource = _termSource;
            _termSource.DataSource = ReadTerms();
        }

        public void WriteLine(string text) => Log.WriteLine(text);

        public async Task Run()
        {
            LocalList<string> list = new LocalList<string>();
            var rows = _termGrid.SelectedRows;
            foreach (var row in rows)
            {
                TermInfo term = row as TermInfo;
                if (!ReferenceEquals(term, null))
                {
                    list.Add(term.Text);
                }
            }

            if (list.Count == 0)
            {
                TermInfo term = _termSource.Current as TermInfo;
                if (ReferenceEquals(term, null))
                {
                    return;
                }

                list.Add(term.Text);
            }

            if (list.Count == 0)
            {
                return;
            }

            if (File.Exists("Template.xlsx"))
            {
                Spreadsheet.Document.LoadDocument("Template.xlsx");
            }
            else
            {
                var worksheet = Spreadsheet.Document.Worksheets[0];
                worksheet.Clear(worksheet.Cells);
            }

            EffectiveEngine engine = new EffectiveEngine
            {
                Log = Log,
                OutputBooks = _booksBox.CheckBox.Checked,
                Selected = list.ToArray(),
                Connection = GetConnection(),
                Sheet = new EffectiveSheet(Spreadsheet.Document)
                {
                    Control = this
                }
            };

            engine.Sheet.NewLine();

            Task task = Task.Run(() =>
            {
                engine.Process(engine.Selected);
            });

            await task;
        }

        private async void _goButton_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WriteLine("СТАРТ");

            await Run();

            WriteLine("СТОП");
            stopwatch.Stop();
            WriteLine("Затрачено времени: "
                      + stopwatch.Elapsed.TotalMinutes.ToString("F2")
                      + " мин.");
        }

        private void _termGrid_DoubleClick(object sender, EventArgs e)
        {
            _goButton_Click(sender, e);
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Spreadsheet.SaveDocument(_saveFileDialog.FileName);
            }
        }
    }
}
