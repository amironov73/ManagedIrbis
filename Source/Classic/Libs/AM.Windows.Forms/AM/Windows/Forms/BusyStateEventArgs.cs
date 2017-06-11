// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BusyStateEventArgs.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BusyStateEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// State.
        /// </summary>
        [NotNull]
        public BusyState State { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyStateEventArgs
            (
                [NotNull] BusyState state
            )
        {
            Code.NotNull(state, "state");

            State = state;
        }

        #endregion
    }
}
