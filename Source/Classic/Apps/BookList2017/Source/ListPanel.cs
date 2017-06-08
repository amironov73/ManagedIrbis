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

namespace BookList2017
{
    public partial class ListPanel
        : UniversalCentralControl
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ListPanel()
            : base(null)
        {
            // Constructor for WinForms Designer only.
        }

        public ListPanel
            (
                MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            if (!ReferenceEquals(mainForm, null))
            {
                mainForm.Icon = Properties.Resources.Document;
            }
            WriteLine("BookList2017 ready");
        }

        #endregion

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
        }
    }
}
