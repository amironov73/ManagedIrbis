// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GazettesPanel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.Data;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Reflection;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;
using AM.UI;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Magazines;
using ManagedIrbis.Menus;

using Timer = System.Windows.Forms.Timer;

#endregion

// ReSharper disable StringLiteralTypo

namespace FastGazettes
{
    public partial class GazettePanel
        : UniversalCentralControl
    {
        #region Constants

        public static string RussianPrologue
            = @"{\rtf1\ansi\ansicpg1251\deff0\deflang1049"
              + @"{\fonttbl{\f0\fnil\fcharset204 Times New Roman;}"
              + @"{\f1\fnil\fcharset238 Times New Roman;}}"
              + @"{\stylesheet{\s0\f0\fs24\snext0 Normal;}"
              + @"{\s1\f1\fs40\b\snext0 Heading;}}"
              + @"\viewkind4\uc1\pard\f0\fs24 ";


        #endregion

        #region Properties

        /// <summary>
        /// Busy state controller.
        /// </summary>
        [NotNull]
        public BusyController Controller =>
            MainForm
                .ThrowIfNull("MainForm")
                .Controller
                .ThrowIfNull("MainForm.Controller");

        [NotNull]
        public MagazineManager MagMan
        {
            get
            {
                return new MagazineManager(Connection);
            }
        }

        [NotNull]
        public IIrbisConnection Connection => GetConnection();

        public MagazineInfo[] Magazines { get; set; }

        [CanBeNull]
        public MagazineInfo CurrentMagazine => _magazineSource.Current as MagazineInfo;

        public MenuEntry[] Siglas { get; set; }

        [CanBeNull]
        public string CurrentSigla
        {
            get
            {
                MenuEntry entry = _fondBox.SelectedItem as MenuEntry;

                return entry?.Code;
            }
        }

        public MenuEntry[] Statuses { get; set; }

        public string CurrentStatus
        {
            get
            {
                MenuEntry entry = _statusBox.SelectedItem as MenuEntry;

                return entry?.Code;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable NotNullMemberIsNotInitialized
        protected GazettePanel()
            // ReSharper restore NotNullMemberIsNotInitialized
            : base(null)
        {
            // Constructor for WinForms Designer only.
        }

        public GazettePanel
            (
                MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();
            _magazineBox_SizeChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Private members

        [NotNull]
        private IIrbisConnection GetConnection()
        {
            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.GetIrbisProvider();
            IIrbisConnection result = mainForm.Connection
                .ThrowIfNull("connection");

            return result;
        }

        private void InitializeMagazines()
        {
            MenuFile siglaMenu = new MenuFile();
            MenuFile statusMenu = new MenuFile();
            Run(() =>
            {
                Magazines = MagMan.GetAllMagazines();
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        Connection.Database,
                        "mhr.mnu"
                    );
                siglaMenu = Connection.ReadMenu(specification);

                specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        Connection.Database,
                        "ste.mnu"
                    );
                statusMenu = Connection.ReadMenu(specification);
            });

            Siglas = siglaMenu.SortEntries(MenuSort.ByCode);
            foreach (MenuEntry sigla in Siglas)
            {
                _fondBox.Items.Add(sigla);
            }

            Statuses = statusMenu.SortEntries(MenuSort.ByCode);
            foreach (MenuEntry status in Statuses)
            {
                _statusBox.Items.Add(status);
            }

            if (Statuses.Length != 0)
            {
                _statusBox.SelectedIndex = 0;
            }

            Array.Sort(Magazines, new MagazineComparer.ByTitle());
            _magazineSource.DataSource = Magazines;

            WriteLine
                (
                    "Список журналов и газет: {0} шт.",
                    Magazines.Length
                );

            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.ReleaseProvider();
        }

        private void _timer1_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            timer.Enabled = false;
            InitializeMagazines();
        }

        #endregion

        #region Public methods

        public void WriteLine()
        {
            MainForm.ThrowIfNull("MainForm").WriteLine(string.Empty);
        }

        public void Phase2()
        {
            _timer1.Enabled = true;
        }

        public bool PrepareBrowser(WebBrowser browser)
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
                    Debug.WriteLine(exception.Message);
                    return false;
                }
            }

            return true;
        }

        [NotNull]
        public string GetFormat()
        {
            string result = ConfigurationUtility.GetString("format", "@")
                .ThrowIfNull("format");

            return result;
        }

        #endregion

        private void _magazineBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            MagazineInfo magazine = CurrentMagazine;
            if (ReferenceEquals(magazine, null))
            {
                return;
            }

            if (string.IsNullOrEmpty(magazine.Description))
            {
                string format = GetFormat();
                Run(() =>
                {
                    magazine.Description = Connection.FormatRecord
                        (
                            format,
                            magazine.Mfn
                        );
                });
            }

            try
            {
                _richTextBox.Rtf = RussianPrologue
                    + magazine.Description + "}";
            }
            catch
            {
                _richTextBox.Text = magazine.Description;
            }
        }

        private void _keyBox_TextChanged
            (
                object sender,
                EventArgs e
            )
        {
            string key = _keyBox.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            MagazineInfo found = Magazines.FirstOrDefault();
            foreach (MagazineInfo magazine in Magazines)
            {
                if (string.Compare(magazine.Title, key,
                        StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    break;
                }

                found = magazine;
            }

            int index = Array.IndexOf(Magazines, found);
            if (index >= 0)
            {
                _magazineSource.Position = index;
            }
        }

        private void _magazineBox_SizeChanged
            (
                object sender,
                EventArgs e
            )
        {
            // TODO
            // _keyBox.Width = _magazineBox.Width - 50;
        }

        private void _CreateNumbersAndCumulate
            (
                [NotNull] MagazineInfo magazine,
                [NotNull] string year,
                [NotNull] string cumulated,
                [NotNull] string[] decumulated,
                [NotNull] string fond
            )
        {
            foreach (string number in decumulated)
            {
                MagazineIssueInfo found = MagMan.GetIssue(magazine, year, number);
                if (!ReferenceEquals(found, null))
                {
                    WriteLine("Выпуск {0} за {1} год уже есть", number, year);
                    return;
                }
            }

            foreach (string number in decumulated)
            {
                ExemplarInfo exemplar = new ExemplarInfo
                {
                    Status = ExemplarStatus.Free,
                    Number = "1",
                    Date = "?",
                    Place = fond
                };
                ExemplarInfo[] exemplars = { exemplar };
                MagazineIssueInfo created = MagMan.CreateMagazine
                    (
                        magazine,
                        year,
                        number,
                        exemplars
                    );
                MarcRecord record = created.ToRecord();
                Connection.WriteRecord(record);
                WriteLine("\tСоздан выпуск {0}", number);
            }

            // Добавляем строку кумуляции
            // TODO проверять, если такая строка уже есть
            MarcRecord summary = Connection.ReadRecord(magazine.Mfn);
            RecordField field = new RecordField(909)
                .AddSubField('q', year)
                .AddSubField('d', fond)
                .AddSubField('h', cumulated)
                .AddSubField('k', "1");
            summary.Fields.Add(field);
            Connection.WriteRecord(summary);
            WriteLine("\tДобавлена строка кумуляции");

            // Обновляем биб. описание
            string format = GetFormat();
            magazine.Description = Connection.FormatRecord(format, magazine.Mfn);
        }

        private void _goButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            MagazineInfo magazine = CurrentMagazine;
            if (ReferenceEquals(magazine, null))
            {
                WriteLine("Не выбран журнал!");

                return;
            }

            string title = magazine.Title;
            string fond = CurrentSigla;
            if (string.IsNullOrEmpty(fond))
            {
                WriteLine("Не выбран фонд!");

                return;
            }

            string year = _yearBox.Text.Trim();
            if (!Regex.IsMatch(year, "^\\d{4}$"))
            {
                WriteLine("Задан неверный год!");

                return;
            }

            string status = CurrentStatus;
            if (string.IsNullOrEmpty(status))
            {
                WriteLine("Не задан статус");

                return;
            }

            string cumulated = _numberBox.Text.Trim();
            if (string.IsNullOrEmpty(cumulated))
            {
                WriteLine("Не заданы номера!");

                return;
            }

            while (cumulated.Contains(" "))
            {
                cumulated = cumulated.Replace(" ", string.Empty);
            }

            if (string.IsNullOrEmpty(cumulated))
            {
                WriteLine("Не заданы номера!");

                return;
            }

            WriteLine("{0} {1} {2}", title, year, cumulated);
            string[] decumultated = NumberText.ParseRanges(cumulated)
                .Select(n => n.ToString())
                .ToArray();

            Run(() =>
            {
                _CreateNumbersAndCumulate(magazine, year, cumulated, decumultated, fond);
            });

            // Обновляем биб. описание на экране
            _magazineBox_SelectedIndexChanged(this, EventArgs.Empty);

            stopwatch.Stop();
            WriteLine("Выполнено за {0:0.00} с.", stopwatch.Elapsed.TotalSeconds);
            WriteLine();
        }
    }
}
