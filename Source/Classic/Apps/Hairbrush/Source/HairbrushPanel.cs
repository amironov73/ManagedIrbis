// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HairbrushPanel.cs -- 
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
using ManagedIrbis.Search;
using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Hairbrush
{
    public partial class HairbrushPanel
        : UniversalCentralControl
    {
        #region Properties

        [NotNull]
        public string Prefix { get; set; }

        [CanBeNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        public HairbrushPanel()
            : base(null)
        {
            InitializeComponent();

            Prefix = "A=";
        }

        public HairbrushPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            Prefix = "A=";
        }

        #endregion

        #region Private members



        #endregion

        #region Public methods

        [NotNull]
        public IrbisProvider GetProvider()
        {
            return MainForm.GetIrbisProvider();
        }

        public void ReleaseProvider()
        {
            MainForm.ReleaseProvider();
        }

        #endregion

        private void _keyBox_ButtonClick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Provider = GetProvider();

                string startTerm = Prefix + _keyBox.Text;
                TermParameters parameters = new TermParameters
                {
                    Database = Provider.Database,
                    StartTerm = startTerm,
                    NumberOfTerms = 100
                };
                TermInfo[] rawTerms = Provider.ReadTerms(parameters);
                TermData[] termData = TermData.FromRawTerms
                    (
                        rawTerms,
                        Prefix
                    );
                _termDataBindingSource.DataSource = termData;
            }
            finally
            {
                ReleaseProvider();
            }
        }

        private void _keyBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                _keyBox_ButtonClick(sender, e);
            }
        }
    }
}
