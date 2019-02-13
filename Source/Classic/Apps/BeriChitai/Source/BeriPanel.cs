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

        [NotNull]
        public IIrbisConnection Connection
        {
            get { return GetConnection(); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BeriPanel()
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
    }
}
