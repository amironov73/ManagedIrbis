// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextViewer.cs -- 
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

#endregion

namespace Shortage
{
    partial class PlainTextViewer 
        : Form
    {
        #region Construction

        public PlainTextViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        public void SetText
            (
                string text
            )
        {
            _editor.Text = text;
        }

        #endregion
    }
}
