// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ApplicationUtility.cs -- helper methods for Apllication class
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Helper methods for <see cref="Application"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ApplicationUtility
    {
        #region Properties

        #endregion

        #region Public methods

        /// <summary>
        /// (Almost) non-blocking Delay.
        /// </summary>
        public static void IdleDelay
            (
                int milliseconds
            )
        {
            Code.Positive(milliseconds, "milliseconds");

            DateTime moment
                = DateTime.Now.AddMilliseconds(milliseconds);

            PseudoAsync.WaitFor
                (
                    () => DateTime.Now < moment
                );
        }

        #endregion
    }
}
