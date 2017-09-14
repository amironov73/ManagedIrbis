// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservPanel.cs -- 
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
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ReservDesktop
{
    public partial class ReservPanel
        : UniversalCentralControl
    {
        #region Properties

        [NotNull]
        public BusyController Controller
        {
            get { return MainForm.Controller; }
        }

        [NotNull]
        public string Prefix { get; set; }

        [CanBeNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        public ReservPanel()
        {
            InitializeComponent();
        }

        public ReservPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();
        }

        #endregion

        #region Private members



        #endregion

        #region Public methods

        [NotNull]
        public IrbisProvider GetProvider()
        {
            //return MainForm.Provider;
            return MainForm.GetIrbisProvider();
        }

        public void ReleaseProvider()
        {
            MainForm.ReleaseProvider();
            Provider = null;
        }

        #endregion

    }
}
