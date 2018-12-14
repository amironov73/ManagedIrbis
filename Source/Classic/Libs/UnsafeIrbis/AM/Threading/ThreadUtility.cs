// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ThreadUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Threading;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Threading
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public static class ThreadUtility
    {
        #region Properties

        /// <summary>
        /// Thread.CurrentThread.ManagedThreadId
        /// </summary>
        public static int ThreadId => Thread.CurrentThread.ManagedThreadId;

        #endregion

        #region Public methods

        /// <summary>
        /// Sleep for specified milliseconds.
        /// </summary>
        public static void Sleep
            (
                int milliseconds
            )
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        #endregion
    }
}
