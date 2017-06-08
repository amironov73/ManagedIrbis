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
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text.Output;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

#endregion

namespace BookList2017
{
    public partial class ListPanel
        : UniversalCentralControl
    {
        #region Properties

        /// <summary>
        /// List of exemplars.
        /// </summary>
        [NotNull]
        public BindingList<ExemplarInfo> ExemplarList { get; private set; }

        [NotNull]
        public IrbisConnection Connection
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
                        )
                    {
                        Format = ConfigurationUtility.GetString
                            (
                                "format",
                                "@brief"
                            )
                            .ThrowIfNull("_manager.Format")
                    };
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

            WriteLine("BookList2017 ready");
        }

        #endregion

        private ExemplarManager _manager;

        [NotNull]
        private IrbisConnection GetConnection()
        {
            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.GetIrbisProvider();
            IrbisConnection result = mainForm.Connection
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
            string number = _numberBox.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            _numberBox.Clear();

            if (AlreadyHave(number))
            {
                WriteLine("Уже есть экземпляр: {0}", number);

                return;
            }

            ExemplarInfo exemplar;

            try
            {
                exemplar = Manager.ReadExtend(number);
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

            exemplar.ShelfIndex = exemplar.Record
                .ThrowIfNull("exemplar.Record")
                .FM("906");

            if (!string.IsNullOrEmpty(exemplar.Description))
            {
                exemplar.Description = exemplar.Description.Replace
                    (
                        "[Текст] ",
                        string.Empty
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

        private async void LoadExemplars()
        {
            if (!File.Exists("list.sav"))
            {
                return;
            }

            Task<ExemplarInfo[]> task 
                = Task<ExemplarInfo[]>.Factory.StartNew
                (
                    () =>
                    {

                        ExemplarInfo[] result = SerializationUtility
                            .RestoreArrayFromFile<ExemplarInfo>("list.sav");

                        return result;
                    }
                );

            ExemplarInfo[] array = await task;
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
            ExemplarInfo[] array = ExemplarList.ToArray();

            FileUtility.DeleteIfExists("list.tmp");
            array.SaveToFile("list.tmp");
            FileUtility.DeleteIfExists("list.sav");
            File.Move("list.tmp", "list.sav");
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
    }
}
