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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Configuration;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IsisUtilsNet
{
    public partial class MainForm
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        private void CheckFileExist
            (
                [NotNull] string fileName
            )
        {
            string exeDirectory = Path.GetDirectoryName
                (
                    Assembly.GetEntryAssembly()
                        .Location
                )
                .ThrowIfNull("can't determine exeDirectory");

            string fullPath = Path.Combine
                (
                    exeDirectory,
                    fileName
                );

            if (!File.Exists(fullPath))
            {
                ExceptionUtility.Throw
                    (
                        string.Format
                        (
                            "Can't find file {0}",
                            fileName
                        )
                    );
            }
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            CheckFileExist("irbis64.dll");
            CheckFileExist("irbis65.dll");
            CheckFileExist("borlndmm.dll");

            string serverIniPath
                = ConfigurationUtility.GetString
                (
                    "server-ini"
                );
            if (string.IsNullOrEmpty(serverIniPath))
            {
                ExceptionUtility.Throw
                    (
                        "server-ini parameter is empty"
                    );
            }
        }
    }
}
