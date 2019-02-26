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

            IrbisConnection connection = (IrbisConnection)Connection.ThrowIfNull("Connection");
            //BeriMan = new BeriManager(connection);
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

        #endregion

        #region Public methods



        #endregion
    }
}
