// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CorrectorPanel.cs -- 
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
using AM.Text.Output;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace DicCorrector
{
    public partial class CorrectorPanel 
        : UniversalCentralControl
    {
        #region Properties


        #endregion

        #region Construction

        public CorrectorPanel
            (
                MainForm mainForm
            )
            : base (mainForm)
        {
            InitializeComponent();

            MainForm.Icon = Properties.Resources.Correct;
            WriteLine("DicCorrector ready");
        }

        #endregion

        #region Private members


        #endregion

        #region Public methods


        #endregion
    }
}
