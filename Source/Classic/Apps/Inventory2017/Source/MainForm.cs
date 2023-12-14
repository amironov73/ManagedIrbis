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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text;
using AM.Windows.Forms;

using DevExpress.XtraEditors;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;
using ManagedIrbis.Statistics;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable CoVariantArrayConversion

namespace Inventory2017
{
    public partial class MainForm
        : Form
    {
        #region Properties

        [CanBeNull]
        public string CurrentFond
        {
            get
            {
                if (_fondBox.DroppedDown)
                {
                    return null;
                }

                MenuEntry entry = _fondBox.SelectedItem as MenuEntry;

                return entry?.Code;
            }
        }

        [CanBeNull]
        public string CurrentCollection
        {
            get
            {
                if (_collectionBox.DroppedDown)
                {
                    return null;
                }

                MenuEntry entry = _collectionBox.SelectedItem as MenuEntry;

                return entry?.Code;
            }
        }

        public List<BookInfo> ConfirmedBooks = new List<BookInfo>();

        public string CurrentShelf
        {
            get { return _shelfBox.Text.Trim().ToUpperInvariant(); }
        }

        public string FromNumber
        {
            get { return _fromBox.Text.Trim(); }
        }

        public string ToNumber
        {
            get { return _toBox.Text.Trim(); }
        }

        public string CurrentDate
        {
            get { return DateTime.Today.ToString("yyyyMMdd"); }
        }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private IrbisConnection _client;
        //private RfidReader _rfid;
        //private List<string> _emulation;
        //private int _currentEmulation;

        private MarcRecord _currentRecord;
        private ExemplarInfo _currentExemplar;

        private void _InitClient()
        {
            _client = IrbisConnectionUtility.GetClientFromConfig();
            _logBox.Output.WriteLine
                (
                    "Подключились к серверу {0}, база {1}",
                    _client.Host,
                    _client.Database
                );

            string mhr = CM.AppSettings["mhr"];
            MenuFile menu = _client.ReadMenu(mhr);
            MenuEntry[] entries = menu.SortEntries(MenuSort.ByCode);
            _fondBox.Items.AddRange(entries);

            string coll = CM.AppSettings["coll"];
            menu = _client.ReadMenu(coll);
            entries = menu.SortEntries(MenuSort.ByCode);
            _collectionBox.Items.AddRange(entries);
        }

        private void _SetHtml
            (
                string format,
                params object[] args
            )
        {
            _browser.DocumentText = string.Format
                (
                    format,
                    args
                );
            Application.DoEvents();
        }

        private void _HandleRfid
            (
                string rfid
            )
        {
            _currentRecord = null;
            _currentExemplar = null;
            _SetHtml(string.Empty);

            _logBox.Output.WriteLine
                (
                    "Считана метка: {0}",
                    rfid
                );

            string prefix = CM.AppSettings["prefix"];
            MarcRecord[] found = _client.SearchRead
                (
                    "\"{0}{1}\"",
                    prefix,
                    rfid
                );
            if (found.Length == 0)
            {
                _SetHtml
                    (
                        "Не найдена метка {0}",
                        rfid
                    );
                return;
            }
            if (found.Length != 1)
            {
                _SetHtml
                    (
                        "Много записей для метки {0}",
                        rfid
                    );
                return;
            }

            MarcRecord record = found[0];
            RecordField[] fields = record.Fields
                .GetField(910)
                .GetField('h', rfid);

            if (fields.Length == 0)
            {
                _SetHtml
                    (
                        "Не найдено поле с меткой {0}",
                        rfid
                    );
                return;
            }
            if (fields.Length != 1)
            {
                _SetHtml
                    (
                        "Много полей с меткой {0}",
                        rfid
                    );
                return;
            }

            ExemplarInfo exemplar = ExemplarInfo.Parse(fields[0]);
            exemplar.UserData = fields[0];

            StringBuilder diagnosis = new StringBuilder();
            diagnosis.AppendFormat
                (
                    "{0} =&gt; <b>{1}</b><br/>",
                    rfid,
                    exemplar.Number
                );
            if (!string.IsNullOrEmpty(exemplar.RealPlace))
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Экземпляр уже был проверен "
                        + "в фонде</font> <b>{0}</b> ({1})<br/>",
                        exemplar.RealPlace,
                        exemplar.CheckedDate
                    );
            }
            if (exemplar.Status != "0")
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Неверный статус "
                        + "экземпляра: </font><b>{0}</b><br/>",
                        exemplar.Status
                    );
            }
            if (!exemplar.Place.SameString(CurrentFond))
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Неверное место хранения "
                        + "экземпляра: </font><b>{0}</b><br/>",
                        exemplar.Place
                    );
            }
            string currentShelf = CurrentShelf;
            if (!string.IsNullOrEmpty(currentShelf))
            {
                string[] items = currentShelf
                    .Split(',', ';')
                    .Select(item => item.Trim())
                    .NonEmptyLines()
                    .Select(item => item.ToUpperInvariant())
                    .ToArray();

                string shelf = record.FM(906);
                if (string.IsNullOrEmpty(shelf))
                {
                    diagnosis.AppendFormat
                        (
                            "<font color='red'>Для книги не указан "
                            + "расстановочный шифр</font><br/>"
                        );
                }
                else if (items.Length != 0)
                {
                    shelf = shelf.ToUpperInvariant();
                    bool good = false;
                    foreach (string item in items)
                    {
                        if (shelf.StartsWith(item))
                        {
                            good = true;
                            break;
                        }
                    }
                    if (!good)
                    {
                        diagnosis.AppendFormat
                            (
                                "<font color='red'>Неверный шифр: "
                                + "</font><b>{0}</b><br/>",
                                shelf
                            );
                    }
                }
            }
            string fromNumber = FromNumber,
                toNumber = ToNumber;
            if (!string.IsNullOrEmpty(fromNumber)
                && !string.IsNullOrEmpty(toNumber))
            {
                string number = exemplar.Number;
                if (!string.IsNullOrEmpty(number))
                {
                    NumberText fn = fromNumber,
                        tn = toNumber,
                        n = number;
                    if (fn > n || tn < n)
                    {
                        diagnosis.AppendFormat
                            (
                                "<font color='red'>Номер за пределами "
                                + "интервала</font>: <b>{0}</b><br/>",
                                 number
                            );
                    }
                }
            }
            diagnosis.AppendLine("<br/>");

            string cardFormat = CM.AppSettings["card-format"];
            string briefDescription = _client.FormatRecord
                (
                    cardFormat,
                    record.Mfn
                ).Trim();
            diagnosis.Append(briefDescription);
            record.UserData = briefDescription;

            _SetHtml
                (
                    "{0}",
                    diagnosis.ToString()
                );

            _currentRecord = record;
            _currentExemplar = exemplar;

            //if (_emulation != null)
            //{
            //    _okButton_Click
            //        (
            //            this,
            //            EventArgs.Empty
            //        );
            //}
        }

        private void _ReadRfid()
        {
            //string rfid = null;

            //if (_emulation != null)
            //{
            //    if (!string.IsNullOrEmpty(CurrentFond))
            //    {
            //        if (_currentEmulation < _emulation.Count)
            //        {
            //            rfid = _emulation[_currentEmulation];
            //            _currentEmulation++;
            //        }
            //    }
            //}

            //if (_rfid != null)
            //{
            //    rfid = _rfid.ReadRfid();
            //}
            //if (string.IsNullOrEmpty(rfid))
            //{
            //    return;
            //}
            //try
            //{
            //    _idleTimer.Enabled = false;
            //    _HandleRfid(rfid);
            //}
            //finally
            //{
            //    _idleTimer.Enabled = true;
            //}
        }

        private void _idleTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                //    if (_emulation == null)
                //    {
                //        string emulationFile = CM.AppSettings["emulation"];
                //        if (!string.IsNullOrEmpty(emulationFile))
                //        {
                //            _emulation = File.ReadAllLines(emulationFile)
                //                .ToList();
                //            _logBox.Output.WriteLine
                //                (
                //                    "Включена эмуляция RFID из файла {0}",
                //                    emulationFile
                //                );
                //        }
                //        else
                //        {
                //            if (_rfid == null)
                //            {
                //                string readerName = CM.AppSettings["rfid-reader"];

                //                _rfid = new RfidReader(readerName);
                //                _logBox.Output.WriteLine
                //                    (
                //                        "Открыт RFID-считыватель: {0}",
                //                        readerName
                //                    );
                //            }
                //        }
                //    }

                if (_client == null)
                {
                    _InitClient();
                }
                else
                {
                    try
                    {
                        _client.NoOp();

                        _ReadRfid();
                    }
                    catch (Exception ex)
                    {
                        _logBox.Output.WriteErrorLine
                            (
                                "Ошибка: {0}",
                                ex.Message
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                _logBox.Output.WriteErrorLine
                    (
                        "Функционирование остановлено из-за ошибки: {0}",
                        ex.ToString()
                    );
                _SetHtml
                    (
                        "Функционирование остановлено из-за ошибки: {0}",
                        ex.ToString()
                    );
                _idleTimer.Enabled = false;
            }
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            //if (_rfid != null)
            //{
            //    _rfid.Close();
            //    _rfid = null;
            //}
        }

        private void _okButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_currentRecord == null
                || _currentExemplar == null)
            {
                XtraMessageBox.Show
                    (
                        "Невозможно подтвердить книгу"
                    );
                return;
            }

            RecordField field = _currentExemplar.Field;
            if (field == null)
            {
                return;
            }
            field.SetSubField('!', CurrentFond);
            field.SetSubField('s', CurrentDate);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _client.WriteRecord
                (
                    _currentRecord,
                    false,
                    true
                );
            stopwatch.Stop();
            _logBox.Output.WriteLine
                (
                    "Подтвержден номер {0}: {1} ({2})",
                    _currentExemplar.Number,
                    _currentRecord.UserData,
                    stopwatch.Elapsed
                );

            BookInfo book = new BookInfo
            {
                Number = _currentExemplar.Number,
                Description = (string)_currentRecord.UserData
            };
            ConfirmedBooks.Add(book);
            _counterLabel.Text = string.Format
                (
                    "Подтверждено: {0}",
                    ConfirmedBooks.Count
                );

            _SetHtml(string.Empty);

            _currentRecord = null;
            _currentExemplar = null;

            if (_numberBox.Enabled)
            {
                _numberBox.Focus();
            }
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            //this.ShowVersionInfoInTitle();
        }

        private void _viewButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            using (ViewForm form = new ViewForm())
            {
                form.SetDataSource(ConfirmedBooks);
                form.ShowDialog(this);
            }
        }

        private void _fixPlaceButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_currentRecord == null
                || _currentExemplar == null)
            {
                XtraMessageBox.Show
                    (
                        "Нечего исправлять!"
                    );
                return;
            }

            RecordField field = _currentExemplar.Field;
            if (field == null)
            {
                return;
            }
            field.SetSubField('a', "0");
            field.SetSubField('d', CurrentFond);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _client.WriteRecord
                (
                    _currentRecord,
                    false,
                    true
                );
            stopwatch.Stop();
            _logBox.Output.WriteLine
                (
                    "Исправлено место/статус для номера {0}: {1} ({2})",
                    _currentExemplar.Number,
                    _currentRecord.UserData,
                    stopwatch.Elapsed
                );

            _SetHtml(string.Empty);

            _currentRecord = null;
            _currentExemplar = null;
        }

        private void _fixShelfButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_currentRecord == null
                || _currentExemplar == null)
            {
                XtraMessageBox.Show
                    (
                        "Нечего исправлять!"
                    );
                return;
            }

            string currentShelf = CurrentShelf;
            if (string.IsNullOrEmpty(currentShelf))
            {
                XtraMessageBox.Show
                    (
                        "Не задан раздел!"
                    );
                return;
            }

            if (currentShelf.ContainsAnySymbol
                (
                    ' ',
                    ',',
                    ';'
                ))
            {
                XtraMessageBox.Show
                    (
                        "Раздел содержит недопустимые символы!"
                    );
                return;
            }

            string shelf = _currentRecord.FM(906);
            if (!string.IsNullOrEmpty(shelf))
            {
                XtraMessageBox.Show
                    (
                        "В записи уже установлен "
                        + "систематический шифр, "
                        + "его нельзя менять!"
                    );
                return;
            }

            _currentRecord.SetField
                (
                    906,
                    currentShelf
                );

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _client.WriteRecord
                (
                    _currentRecord,
                    false,
                    true
                );
            stopwatch.Stop();
            _logBox.Output.WriteLine
                (
                    "Исправлен раздел для метки {0}: {1} ({2})",
                    _currentExemplar.Barcode,
                    _currentRecord.UserData,
                    stopwatch.Elapsed
                );

            _SetHtml(string.Empty);

            _currentRecord = null;
            _currentExemplar = null;
        }

        #endregion

        private void _badButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_currentRecord == null
                || _currentExemplar == null)
            {
                XtraMessageBox.Show
                    (
                        "Нечего откладывать!"
                    );
                return;
            }

            string processing = CM.AppSettings["processing"];
            RecordField field = _currentExemplar.Field;
            if (field == null)
            {
                return;
            }
            field.SetSubField('a', "5");
            field.SetSubField('d', processing);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _client.WriteRecord
                (
                    _currentRecord,
                    false,
                    true
                );
            stopwatch.Stop();
            _logBox.Output.WriteLine
                (
                    "Отложен номер {0}: {1} ({2})",
                    _currentExemplar.Number,
                    _currentRecord.UserData,
                    stopwatch.Elapsed
                );

            _SetHtml(string.Empty);

            _currentRecord = null;
            _currentExemplar = null;

        }

        private void _HandleNumber
            (
                [CanBeNull] string number
            )
        {
            _currentRecord = null;
            _currentExemplar = null;
            _SetHtml(string.Empty);

            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            _logBox.Output.WriteLine
                (
                    "Введен номер: {0}",
                    number
                );

            string prefix = CM.AppSettings["prefix"];
            int[] found = _client.Search
                (
                    "\"{0}{1}\"",
                    prefix,
                    number
                );
            if (found.Length == 0)
            {
                _SetHtml
                    (
                        "Не найден номер {0}",
                        number
                    );
                return;
            }
            if (found.Length != 1)
            {
                _SetHtml
                    (
                        "Много записей для номера {0}",
                        number
                    );
                return;
            }

            MarcRecord record = _client.ReadRecord(found[0]);
            RecordField[] fields = record.Fields
                .GetField(910)
                .GetField(new char[] {'b', 'h'}, number);

            if (fields.Length == 0)
            {
                _SetHtml
                    (
                        "Не найдено поле с номером {0}",
                        number
                    );
                return;
            }
            if (fields.Length != 1)
            {
                _SetHtml
                    (
                        "Много полей с номером {0}",
                        number
                    );
                return;
            }

            ExemplarInfo exemplar = ExemplarInfo.Parse(fields[0]);

            StringBuilder diagnosis = new StringBuilder();
            var field = fields[0];
            diagnosis.AppendFormat
                (
                    "Номер: <b>{0}</b><br/>Штрих-код: <b>{1}</b><br/>",
                    field.GetFirstSubFieldValue ('b'),
                    field.GetFirstSubFieldValue ('h')
                );
            if (!string.IsNullOrEmpty(exemplar.RealPlace))
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Экземпляр уже был проверен "
                        + "в фонде</font> <b>{0}</b> ({1})<br/>",
                        exemplar.RealPlace,
                        exemplar.CheckedDate
                    );
            }
            if (exemplar.Status != "0")
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Неверный статус "
                        + "экземпляра: </font><b>{0}</b><br/>",
                        exemplar.Status
                    );
            }
            if (!exemplar.Place.SameString(CurrentFond))
            {
                diagnosis.AppendFormat
                    (
                        "<font color='red'>Неверное место хранения "
                        + "экземпляра: </font><b>{0}</b><br/>",
                        exemplar.Place
                    );
            }
            string currentShelf = CurrentShelf;
            if (!string.IsNullOrEmpty(currentShelf))
            {
                string[] items = currentShelf
                    .Split(',', ';')
                    .Select(item => item.Trim())
                    .NonEmptyLines()
                    .Select(item => item.ToUpperInvariant())
                    .ToArray();

                string shelf = record.FM(906);
                if (string.IsNullOrEmpty(shelf))
                {
                    diagnosis.AppendFormat
                        (
                            "<font color='red'>Для книги не указан "
                            + "расстановочный шифр</font><br/>"
                        );
                }
                else if (items.Length != 0)
                {
                    shelf = shelf.ToUpperInvariant();
                    bool good = false;
                    foreach (string item in items)
                    {
                        if (shelf.StartsWith(item))
                        {
                            good = true;
                            break;
                        }
                    }
                    if (!good)
                    {
                        diagnosis.AppendFormat
                            (
                                "<font color='red'>Неверный шифр: "
                                + "</font><b>{0}</b><br/>",
                                shelf
                            );
                    }
                }
            }

            string currentCollection = CurrentCollection;
            if (!string.IsNullOrEmpty(currentCollection))
            {
                string collection = exemplar.Collection;
                if (currentCollection != collection)
                {
                    if (string.IsNullOrEmpty(collection))
                    {
                        diagnosis.AppendFormat
                            (
                                "<font color='red'>Для книги не указана "
                                + "коллекция"
                            );
                    }
                    else
                    {
                        diagnosis.AppendFormat
                            (
                                "<font color='red'>Неверная коллекция: "
                                + "</font><b>{0}</b><br/>",
                                collection
                            );
                    }
                }
            }

            diagnosis.AppendLine("<br/>");

            string cardFormat = CM.AppSettings["card-format"];
            string briefDescription = _client.FormatRecord
                (
                    cardFormat,
                    record.Mfn
                ).Trim();
            diagnosis.Append(briefDescription);
            record.UserData = briefDescription;

            _SetHtml
                (
                    "{0}",
                    diagnosis.ToString()
                );

            _currentRecord = record;
            _currentExemplar = exemplar;
        }

        private void _numberBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                string number = _numberBox.Text.Trim();
                _numberBox.Clear();
                _HandleNumber(number);
                _numberBox.Focus();
            }
        }

        private void _clearCollectionButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _collectionBox.SelectedItem = null;
        }
    }
}
