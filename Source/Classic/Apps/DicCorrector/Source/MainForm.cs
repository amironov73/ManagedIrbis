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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Logging;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace DicCorrector
{
    public sealed partial class MainForm
        : UniversalForm
    {
        #region Properties

        [NotNull]
        public CorrectorPanel CorrectorPanel { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            Initialize += _Initialize;

            InitializeComponent();

            HideMainMenu();
            HideToolStrip();
            HideStatusStrip();
            CorrectorPanel = new CorrectorPanel(this);
            SetupCentralControl(CorrectorPanel);
        }

        #endregion

        #region Private members

        private void _Initialize
            (
                object sender,
                EventArgs e
            )
        {
            Icon = Properties.Resources.Correct;

            if (TestProviderConnection())
            {
                WriteLine("Connection OK");
            }
            else
            {
                return;
            }

            WriteLine("DicCorrector ready");
        }

        #endregion

        #region Public methods



        #endregion

        #region UniversalForm members

        #endregion
    }
}
