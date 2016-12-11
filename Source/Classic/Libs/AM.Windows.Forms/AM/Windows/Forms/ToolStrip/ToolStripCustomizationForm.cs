// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ToolStripCustomizationForm.cs --
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
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Customization form for ToolStrip.
    /// </summary>
    [PublicAPI]
    partial class ToolStripCustomizationForm
        : Form
    {
        #region Properties

        /// <summary>
        /// ToolStrip.
        /// </summary>
        [NotNull]
        public ToolStrip ToolStrip { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripCustomizationForm
            (
                [NotNull] ToolStrip toolStrip
            )
        {
            Code.NotNull(toolStrip, "toolStrip");

            ToolStrip = toolStrip;

            InitializeComponent();
        }

        #endregion

        #region Private members

        private void ToolStripCustomizationForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                _listBox.BeginUpdate();
                _listBox.Items.Clear();
                foreach (ToolStripItem item in ToolStrip.Items)
                {
                    _listBox.Items.Add(item, item.Available);
                }
            }
            finally
            {
                _listBox.EndUpdate();
            }
        }

        private void _applyButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            foreach (ToolStripItem item in _listBox.Items)
            {
                int index = _listBox.Items.IndexOf(item);
                item.Available = _listBox.GetItemChecked(index);
            }
        }

        #endregion
    }
}