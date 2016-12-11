// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ModalForm.cs -- simple blank for modal form
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Simple blank for modal form.
    /// </summary>
    [PublicAPI]
    public class ModalForm
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModalForm()
        {
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion
    }
}
