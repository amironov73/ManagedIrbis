/* PaginDataGridViewEventArgs.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// EventArgs for <see cref="PagingDataGridView"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PagingDataGridViewEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Current row.
        /// </summary>
        public DataGridViewRow CurrentRow { get; set; }

        /// <summary>
        /// Initial call?
        /// </summary>
        public bool InitialCall { get; set; }

        /// <summary>
        /// Scroll direction.
        /// </summary>
        public bool ScrollDown { get; set; }

        /// <summary>
        /// Success.
        /// </summary>
        public bool Success { get; set; }

        #endregion
    }
}
