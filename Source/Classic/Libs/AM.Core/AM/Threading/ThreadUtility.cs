// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ThreadUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ThreadUtility
    {
        #region Properties

        /// <summary>
        /// Thread.CurrentThread.ManagedThreadId
        /// </summary>
        public static int ThreadId
        {
            get
            {
#if PORTABLE || SILVERLIGHT || UAP

                return 1;

#else

                return Thread.CurrentThread.ManagedThreadId;

#endif
            }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

#if CLASSIC

#if FW45

        /// <summary>
        /// Sleep for specified milliseconds.
        /// </summary>
        public static async void Sleep
            (
                int milliseconds
            )
        {
            // Let other tasks use this thread.

            if (milliseconds > 0)
            {
                await Task.Delay(milliseconds);
            }
        }

#else

        /// <summary>
        /// Sleep for specified milliseconds.
        /// </summary>
        public static void Sleep
            (
                int milliseconds
            )
        {
            System.Threading.Thread.Sleep (milliseconds);
        }

#endif

#else

        /// <summary>
        /// Sleep for specified milliseconds.
        /// </summary>
        public static async void Sleep
            (
                int milliseconds
            )
        {
            // Let other tasks use this thread.

            if (milliseconds > 0)
            {
                await Task.Delay(milliseconds);
            }
        }

#endif

        #endregion
    }
}
