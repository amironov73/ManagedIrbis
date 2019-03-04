// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OffPanel.cs --
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
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;

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

namespace WriteOffER
{
    public partial class OffPanel
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

        public SpreadsheetControl Spreadsheet { get; set; }

        // [NotNull] private BeriManager BeriMan { get; set; }

        [NotNull]
        public IIrbisConnection Connection => GetConnection();

        //[NotNull]
        //BeriInfo[] SelectedBooks { get; set; }

        //[NotNull]
        //private string[] SelectedReaders { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable NotNullMemberIsNotInitialized
        protected OffPanel()
            // ReSharper restore NotNullMemberIsNotInitialized
            : base(null)
        {
            // Constructor for WinForms Designer only.
        }

        public OffPanel
        (
            MainForm mainForm
        )
            : base(mainForm)
        {
            InitializeComponent();

            //IrbisConnection connection = (IrbisConnection)Connection.ThrowIfNull("Connection");
            //BeriMan = new BeriManager(connection);

            _toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top
            };
            Controls.Add(_toolStrip);
            var prefixes = new PrefixInfo[]
            {
            };
            var prefixBox = new ToolStripComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items =
                {
                    new PrefixInfo{Prefix="NS=", Description = "Карточка комплектования"},
                    new PrefixInfo{Prefix="IN=", Description = "Инвентарный номер"},
                    new PrefixInfo{Prefix="NKSU=", Description = "Номер КСУ"}
                },
                SelectedIndex = 0
            };
            _toolStrip.Items.Add(prefixBox);
            var clearButton = new ToolStripButton("Очистить");
            _toolStrip.Items.Add(clearButton);
            var goButton = new ToolStripButton("Рассчивать");
            _toolStrip.Items.Add(goButton);
            var saveButton = new ToolStripButton("Сохранить");
            _toolStrip.Items.Add(saveButton);
            var loadButton = new ToolStripButton("Загрузить");
            _toolStrip.Items.Add(loadButton);

            Spreadsheet = new SpreadsheetControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Spreadsheet);
            ClearTable();
        }

        #endregion

        #region Private members

        private ToolStrip _toolStrip;

        [NotNull]
        private IIrbisConnection GetConnection()
        {
            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.GetIrbisProvider();
            IIrbisConnection result = mainForm.Connection
                .ThrowIfNull("connection");

            return result;
        }

        #endregion

        #region Public methods

        public void ClearTable()
        {
            byte[] template = Properties.Resources.Template;
            Spreadsheet.LoadDocument(template, DocumentFormat.Xlsx);
        }

        #endregion
    }
}
