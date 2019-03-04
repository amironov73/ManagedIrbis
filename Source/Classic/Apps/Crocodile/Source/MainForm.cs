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

using IrbisUI;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using CM = System.Configuration.ConfigurationManager;
using DataGridViewRow = System.Windows.Forms.DataGridViewRow;

#endregion

// ReSharper disable StringLiteralTypo

namespace Crocodile
{
    public partial class MainForm : Form
    {
        private const string Nksu = "NKSU=";
        private Reference<bool> stopFlag = null;

        public AbstractOutput Log => _logBox.Output;
        public SpreadsheetControl Spreadsheet { get; set; }
        public bool Busy { get; private set; } = false;
        private string _connectionString;

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

        public string GetConnectionString()
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                return _connectionString;
            }

            string result = ConfigurationUtility
                .GetString("connectionString")
                .ThrowIfNull("connectionString");

            return result;
        }

        public IrbisConnection GetConnection()
        {
            string connectionString = GetConnectionString();

            return new IrbisConnection(connectionString);
        }

        private bool SetupLogin()
        {
            string connectionString = GetConnectionString()
                .ThrowIfNull("connectionString");
            IrbisConnection client = new IrbisConnection();
            client.ParseConnectionString(connectionString);

            if ((string.IsNullOrEmpty(client.Username)
                 || string.IsNullOrEmpty(client.Password)))
            {
                if (!IrbisLoginCenter.Login(client, null))
                {
                    client.Dispose();

                    return false;
                }

                ConnectionSettings settings
                    = ConnectionSettings.FromConnection(client);
                _connectionString = settings.Encode();
            }

            client.Dispose();

            connectionString = GetConnectionString();
            client = new IrbisConnection();
            client.ParseConnectionString(connectionString);

            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                        ex.ToString(),
                        "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                client.Dispose();

                return false;
            }

            client.Dispose();

            return true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!SetupLogin())
            {
                Application.Exit();
                return;
            }

            ComboBox.ObjectCollection items = _yearBox.Items;
            items.Clear();
            int year = DateTime.Today.Year;
            for (int i = 0; i < 10; i++)
            {
                items.Add(year.ToInvariantString());
                year--;
            }

            _yearBox.SelectedIndex = 0;

            MenuFile menu = new MenuFile();
            using (IrbisConnection connection = GetConnection())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        connection.Database,
                        "mhr.mnu"
                    );
                menu = connection.ReadMenu(specification);
            }

            _fondBox.Items.Add(new MenuEntry() { Code = "*", Comment = "Все фонды" });
            foreach (MenuEntry entry in menu.Entries)
            {
                _fondBox.Items.Add(entry);
            }

            _fondBox.SelectedIndex = 0;
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
            foreach (DataGridViewRow row in rows)
            {
                TermInfo term = row.DataBoundItem as TermInfo;
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

            string fond = "*";
            MenuEntry entry = _fondBox.SelectedItem as MenuEntry;
            if (!ReferenceEquals(entry, null))
            {
                fond = entry.Code;
            }

            if (ReferenceEquals(stopFlag, null))
            {
                stopFlag = new Reference<bool>(false);
            }

            stopFlag.Target = false;

            EffectiveEngine engine = new EffectiveEngine
            {
                StopFlag = stopFlag,
                Log = Log,
                Fond = fond,
                OutputBooks = _booksBox.CheckBox.Checked,
                Selected = NumberText.Sort(list.ToArray()).ToArray(),
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
            if (Busy)
            {
                return;
            }

            bool fast = _fastBox.CheckBox.Checked;

            try
            {
                Busy = true;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                WriteLine("СТАРТ");

                if (fast)
                {
                    var sheet = Spreadsheet.Document.Worksheets[0];
                    sheet.Clear(sheet.Cells);
                    Application.DoEvents();

                    Spreadsheet.BeginUpdate();
                }

                await Run();

                WriteLine("СТОП");
                stopwatch.Stop();
                WriteLine("Затрачено времени: "
                          + stopwatch.Elapsed.TotalMinutes.ToString("F2")
                          + " мин.");
            }
            finally
            {
                if (fast)
                {
                    Spreadsheet.EndUpdate();
                }

                Busy = false;
            }
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

        private void _stopButton_Click(object sender, EventArgs e)
        {
            if (stopFlag != null)
            {
                stopFlag.Target = true;
            }
        }
    }
}
