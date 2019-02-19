// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListPanel.cs --
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
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;
using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Timer = System.Windows.Forms.Timer;

#endregion

// ReSharper disable StringLiteralTypo

namespace BeriChitai
{
    public partial class BeriPanel
        : UniversalCentralControl
    {
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

        [NotNull] private BeriManager BeriMan { get; set; }

        [NotNull]
        public IIrbisConnection Connection => GetConnection();

        [NotNull]
        BeriInfo[] SelectedBooks { get; set; }

        [NotNull]
        private string[] SelectedReaders { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable NotNullMemberIsNotInitialized
        protected BeriPanel()
        // ReSharper restore NotNullMemberIsNotInitialized
            : base(null)
        {
            // Constructor for WinForms Designer only.
        }

        public BeriPanel
            (
                MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            IrbisConnection connection = (IrbisConnection)Connection.ThrowIfNull("Connection");
            BeriMan = new BeriManager(connection);
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

        private void InitializeReaders()
        {
            Run(() =>
            {
                SelectedBooks = BeriMan.GetReservedBooks();
            });

            SelectedReaders = EmptyArray<string>.Value;

            WriteLine
                (
                    "Поиск заказанных книг выдал: {0} шт.",
                    SelectedBooks.Length
                );

            SelectedReaders = SelectedBooks
                .Select(b => b.Reader)
                .NonNullItems()
                .Select(r => r.FullName)
                .NonEmptyLines()
                .OrderBy(name => name)
                .Distinct()
                .ToArray();
            _readerSource.DataSource = SelectedReaders;
            _readerList.DataSource = _readerSource;

            if (_readerSource.Count != 0)
            {
                _readerSource.Position = 0;
            }

            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.ReleaseProvider();
        }

        private void _timer1_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer) sender;
            timer.Enabled = false;
            InitializeReaders();
        }

        #endregion

        #region Public methods

        public void Phase2()
        {
            _timer1.Enabled = true;
        }

        [CanBeNull]
        public ReaderInfo FindReader
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, nameof(name));

            foreach (BeriInfo book in SelectedBooks)
            {
                ReaderInfo reader = book.Reader;
                if (!ReferenceEquals(reader, null))
                {
                    if (reader.FullName.SameString(name))
                    {
                        return reader;
                    }
                }
            }

            return null;
        }

        [NotNull]
        BeriInfo[] FindBooks
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, nameof(reader));

            LocalList<BeriInfo> result = new LocalList<BeriInfo>();
            foreach (BeriInfo book in SelectedBooks)
            {
                ReaderInfo candidate = book.Reader;
                if (!ReferenceEquals(candidate, null)
                   && candidate.Ticket.SameString(reader.Ticket))
                {
                    result.Add(book);
                }
            }

            return result.ToArray();
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

        #endregion

        private void _readerList_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            _bookList.DataSource = null;
            if (!PrepareBrowser(_readerBrowser)
             || !PrepareBrowser(_bookBrowser))
            {
                return;
            }

            string name = _readerSource.Current as string;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ReaderInfo reader = FindReader(name);
            if (ReferenceEquals(reader, null))
            {
                return;
            }

            _readerBrowser.DocumentText = reader.Description;

            BeriInfo[] books = FindBooks(reader)
                .OrderBy(b => b.ShortDescription)
                .ToArray();

            _bookSource.DataSource = books;
            _bookList.DataSource = _bookSource;
            _bookList.DisplayMember = "ShortDescription";
            if (_bookSource.Count != 0)
            {
                _bookSource.Position = 0;
            }
        }

        private void _bookList_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            if (!PrepareBrowser(_bookBrowser))
            {
                return;
            }

            BeriInfo book = _bookSource.Current as BeriInfo;
            if (ReferenceEquals(book, null))
            {
                return;
            }

            _bookBrowser.DocumentText = book.Description;
        }

        private void _refreshButton_Click(object sender, EventArgs e)
        {
            WriteLine("Обновляем данные...");

            if (!PrepareBrowser(_readerBrowser)
                || !PrepareBrowser(_bookBrowser))
            {
                return;
            }

            SelectedBooks = EmptyArray<BeriInfo>.Value;
            SelectedReaders = EmptyArray<string>.Value;
            _readerSource.DataSource = SelectedReaders;
            _readerList.DataSource = _readerSource;
            Application.DoEvents();

            InitializeReaders();

            if (_readerSource.Count != 0)
            {
                _readerSource.Position = 0;
            }

            WriteLine("Обновление данных завершено");
        }

        private void _giveButton_Click(object sender, EventArgs e)
        {
            BeriInfo book = _bookSource.Current as BeriInfo;
            if (ReferenceEquals(book, null))
            {
                return;
            }

            WriteLine($"КНИГА: {book.ShortDescription}");

            WriteLine("Регистрируем выдачу книги");
            if (!Run(() =>
            {
                if (!BeriMan.GiveBook(book))
                {
                    throw new IrbisException("Ошибка при выдаче книги");
                }
            }))
            {
                WriteLine("Что-то пошло не так");
                return;
            }

            WriteLine("Книга успешно выдана");

            _refreshButton_Click(this, EventArgs.Empty);
        }

        private void _rejectButton_Click(object sender, EventArgs e)
        {
            BeriInfo book = _bookSource.Current as BeriInfo;
            if (ReferenceEquals(book, null))
            {
                return;
            }

            ReaderInfo reader = book.Reader.ThrowIfNull("book.Reader");

            WriteLine($"КНИГА: {book.ShortDescription}");
            WriteLine($"ЧИТАТЕЛЬ: {reader.FullName}");

            WriteLine("Отменяем заказ книги");
            if (!Run(() =>
            {
                if (!BeriMan.GiveBook(book))
                {
                    throw new IrbisException("Ошибка при отмене заказа");
                }
            }))
            {
                WriteLine("Что-то пошло не так");
                return;
            }

            WriteLine("Заказ отменён, книга вновь доступна для заказа");

            _refreshButton_Click(this, EventArgs.Empty);

        }

        private void _readerSource_CurrentChanged(object sender, EventArgs e)
        {
            _mailBox.Clear();
            _phoneBox.Clear();

            string name = _readerSource.Current as string;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ReaderInfo reader = FindReader(name);
            if (ReferenceEquals(reader, null))
            {
                return;
            }

            _mailBox.Text = reader.Email;
            _phoneBox.Text = reader.HomePhone;
        }

        private void _copyPhoneButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_phoneBox.Text);
        }

        private void _copyMailButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_mailBox.Text);
        }
    }
}
