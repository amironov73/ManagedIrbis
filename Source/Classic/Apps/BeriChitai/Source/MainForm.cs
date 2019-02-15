// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace BeriChitai
{
    public partial class MainForm
        : UniversalForm
    {
        #region Properties

        [NotNull]
        public BeriPanel Panel { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            Initialize += _Initialize;

            InitializeComponent();

            HideMainMenu();
            HideToolStrip();
            HideStatusStrip();
            Panel = new BeriPanel(this);
            SetupCentralControl(Panel);
        }

        #endregion

        #region Private members

        private void _Initialize
            (
                object sender,
                EventArgs e
            )
        {
            Icon = Properties.Resources.Garbage;

            if (TestProviderConnection())
            {
                WriteLine("Connection OK");
                Active = true;
                Controller.EnableControls();

                UniversalCentralControl universal = CentralControl
                    as UniversalCentralControl;
                if (!ReferenceEquals(universal, null))
                {
                    universal.SetDefaultFocus();
                }
            }
            else
            {
                Controller.DisableControls();
                return;
            }

            WriteLine("BeriChitai ready");
            Panel.Phase2();
        }

        #endregion

    }
}
