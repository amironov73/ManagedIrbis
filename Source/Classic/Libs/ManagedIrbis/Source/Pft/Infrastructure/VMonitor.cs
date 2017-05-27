// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VMonitor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Отслеживает, был ли вывод из поля с помощью vXXX.
    /// </summary>
    [DebuggerDisplay("{Output}")]
    internal sealed class VMonitor
    {
        #region Properties

        /// <summary>
        /// V command output has been seen.
        /// </summary>
        public bool Output { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
