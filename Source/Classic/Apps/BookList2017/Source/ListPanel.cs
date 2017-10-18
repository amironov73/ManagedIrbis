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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace BookList2017
{
    public partial class ListPanel
        : UniversalCentralControl
    {
        #region Properties

        /// <summary>
        /// Busy state controller.
        /// </summary>
        [NotNull]
        public BusyController Controller
        {
            get
            {
                return MainForm
                    .ThrowIfNull("MainForm")
                    .Controller
                    .ThrowIfNull("MainForm.Controller");
            }
        }

        /// <summary>
        /// List of exemplars.
        /// </summary>
        [NotNull]
        public BindingList<ExemplarInfo> ExemplarList { get; private set; }

        [NotNull]
        public IIrbisConnection Connection
        {
            get { return GetConnection(); }
        }

        public ExemplarManager Manager
        {
            get
            {
                if (ReferenceEquals(_manager, null))
                {
                    _manager = new ExemplarManager
                        (
                            Connection,
                            Output
                        );
                }

                return _manager;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ListPanel()
            : base(null)
        {
            // Constructor for WinForms Designer only.

            ExemplarList = new BindingList<ExemplarInfo>();
        }

        public ListPanel
            (
                MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            ExemplarList = new BindingList<ExemplarInfo>();

            if (!ReferenceEquals(mainForm, null))
            {
                mainForm.Icon = Properties.Resources.Document;
            }

            _bindingSource.DataSource = ExemplarList;

            Controller.Controls.AddRange
                (
                    new Control[]
                    {
                        _addButton, _buildButton,
                        _clearButton, _deleteButton,
                        _numberBox
                    }
                );
            Controller.DisableControls();

            JObject config = JsonUtility.ReadObjectFromFile("config.json");

            DataColumnInfo[] columns = config.SelectToken("grid")
                .ToObject<DataColumnInfo[]>();
            DataGridViewUtility.ApplyColumns(_grid, columns);

            ListFormat[] formats = config.SelectToken("format")
                .ToObject<ListFormat[]>();
            _formatBox.DataSource = formats;
            _formatBox.DisplayMember = "Description";

            ListSort[] sort = config.SelectToken("sort")
                .ToObject<ListSort[]>();
            _sortBox.DataSource = sort;
            _sortBox.DisplayMember = "Description";

            ListVariant[] variants = config.SelectToken("list")
                .ToObject<ListVariant[]>();
            _variantBox.DataSource = variants;
            _variantBox.DisplayMember = "Title";

            _header = config.SelectToken("header")
                .ToObject<MoonExcelData[]>();
            _footer = config.SelectToken("footer")
                .ToObject<MoonExcelData[]>();
        }

        #endregion

        private ExemplarManager _manager;

        private MoonExcelData[] _header, _footer;

        [NotNull]
        private IIrbisConnection GetConnection()
        {
            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.GetIrbisProvider();
            IIrbisConnection result = mainForm.Connection
                .ThrowIfNull("connection");

            return result;
        }

        private bool AlreadyHave
            (
                [NotNull] string number
            )
        {
            foreach (ExemplarInfo exemplar in ExemplarList)
            {
                if (exemplar.Number.SameString(number)
                    || exemplar.Barcode.SameString(number))
                {
                    return true;
                }
            }

            return false;
        }

        private void _addButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            UniversalForm mainForm = MainForm;
            if (ReferenceEquals(mainForm, null)
                || !mainForm.Active)
            {
                WriteLine("Приложение не активно");

                return;
            }

            string number = _numberBox.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            _numberBox.Clear();
            _numberBox.Focus();
            Application.DoEvents();

            if (AlreadyHave(number))
            {
                WriteLine("Уже есть экземпляр: {0}", number);

                return;
            }

            ExemplarInfo exemplar = null;
            Manager.Format = ((ListFormat) _formatBox.SelectedItem)
                .Format
                .ThrowIfNull("Manager.Format");

            try
            {
                Run
                    (
                        () =>
                        {
                            exemplar = Manager.ReadExtend(number);
                        }
                    );
            }
            catch (Exception exception)
            {
                WriteLine("Ошибка: {0}", exception.Message);

                return;
            }

            if (ReferenceEquals(exemplar, null))
            {
                WriteLine("Не удалось найти экземпляр: {0}", number);

                return;
            }

            if (!string.IsNullOrEmpty(exemplar.Description))
            {
                exemplar.Description = exemplar.Description.Replace
                    (
                        "[Текст] ",
                        string.Empty
                    );
            }

            MarcRecord record = exemplar.Record.ThrowIfNull("exemplar.Record");
            RecordField field = exemplar.Field.ThrowIfNull("exemplar.Field");

            if (_statusBox.Checked
                && exemplar.Status != "0")
            {
                exemplar.Status = "0";
                field.SetSubField('A', "0");
                WriteLine("Статус экземпляра {0} изменён на 0", number);
            }

            if (_exhibitionBox.Checked)
            {
                field.SetSubField('9', "Выставка новых поступлений");
                WriteLine("Экземпляр {0} передан на выставку", number);
            }

            if (record.Modified)
            {
                Run
                    (
                        () =>
                        {
                            MainForm.Provider.WriteRecord(record);
                            WriteLine("Изменения сохранены на сервере");
                        }
                    );
            }

            ExemplarList.Add(exemplar);
            _bindingSource.Position = ExemplarList.Count - 1;

            SaveExemplars();
            _numberBox.Focus();

            WriteLine
                (
                    "Добавлено: {0} {1}",
                    exemplar.Number,
                    exemplar.Description.ToVisibleString()
                );
        }

        private void LoadExemplars()
        {
            if (!File.Exists("list.sav"))
            {
                return;
            }

            ExemplarInfo[] array = null;
            Run
                (
                    () =>
                    {

                        array = SerializationUtility
                            .RestoreArrayFromFile<ExemplarInfo>("list.sav");
                    }
                );

            if (ReferenceEquals(array, null))
            {
                return;
            }

            ExemplarList.Clear();
            _bindingSource.SuspendBinding();
            foreach (ExemplarInfo exemplar in array)
            {
                ExemplarList.Add(exemplar);
            }
            _bindingSource.ResumeBinding();
        }

        private void SaveExemplars()
        {
            Run
                (
                    () =>
                    {
                        ExemplarInfo[] array = ExemplarList.ToArray();

                        FileUtility.DeleteIfExists("list.tmp");
                        array.SaveToFile("list.tmp");
                        FileUtility.DeleteIfExists("list.sav");
                        File.Move("list.tmp", "list.sav");
                    }
                );
        }

        private void _clearButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ExemplarList.Clear();
            SaveExemplars();
        }

        private void _firstTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            _firstTimer.Enabled = false;
            if (!ReferenceEquals(MainForm, null))
            {
                LoadExemplars();
            }
        }

        private void _numberBox_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.Enter)
            {
                _addButton_Click(sender, e);
                e.IsInputKey = false;
            }
        }

        private void _deleteButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_bindingSource.Count == 0)
            {
                return;
            }

            int index = _bindingSource.Position;
            if (index < 0 || index >= _bindingSource.Count)
            {
                return;
            }

            _bindingSource.RemoveCurrent();
            SaveExemplars();
        }

        /// <inheritdoc />
        public override void SetDefaultFocus()
        {
            _numberBox.Focus();
        }

        private void _buildButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ListVariant currentVariant
                = (ListVariant)_variantBox.SelectedItem;

            try
            {
                Controller.DisableControls();

                List<object[]> books = null;

                if (!Run
                    (
                        () =>
                        {
                            books = BuildBookList(currentVariant);
                        }
                    ))
                {
                    return;
                }

                IDictionary<string, object> dictionary
                    = new Dictionary<string, object>();
                dictionary.Add("books", books);

                using (ExcelForm excelForm = new ExcelForm())
                {
                    excelForm.ShowBooks
                        (
                            Connection,
                            currentVariant.FileName.ThrowIfNull(),
                            currentVariant.Columns.ThrowIfNull(),
                            dictionary,
                            _header,
                            books,
                            _footer,
                            currentVariant.FirstLine
                        );
                    excelForm.ShowDialog(this);
                }
            }
            finally
            {
                Controller.EnableControls();
            }
        }

        private List<object[]> BuildBookList
            (
                ListVariant currentVariant
            )
        {
            IComparer<ExemplarInfo> comparer;
            ListSort sort = null;

            this.InvokeIfRequired
                (
                    () =>
                    {
                        sort = (ListSort) _sortBox.SelectedItem;
                    }
                );

            switch (sort.Field)
            {
                case "Description":
                    comparer = ExemplarInfoComparer.ByDescription();
                    break;

                case "Number":
                    comparer = ExemplarInfoComparer.ByNumber();
                    break;

                default:
                    throw new ApplicationException
                        (
                            "Unknown field: "
                            + sort.Field.ToVisibleString()
                        );
            }

            ExemplarInfo[] array = ExemplarList
                .OrderBy(book => book, comparer)
                .ToArray();

            int firstNumber = Convert.ToInt32(_firstNumberBox.Value);
            foreach (ExemplarInfo item in array)
            {
                item.SequentialNumber = firstNumber;
                firstNumber++;
            }

            List<object[]> books = new List<object[]>(array.Length);

            foreach (ExemplarInfo exemplar in array)
            {
                List<object> list = new List<object>();
                foreach (ExcelColumn column in currentVariant.Columns)
                {
                    object o = ReflectionUtility.GetPropertyValue
                        (
                            exemplar,
                            column.Expression
                        );
                    list.Add(o);
                }

                books.Add(list.ToArray());
            }

            ExcelForm.DummyMethod();

            return books;
        }
    }
}
