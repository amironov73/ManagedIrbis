// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExceptionEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Exception.
        /// </summary>
        [NotNull]
        public Exception Exception { get; private set; }

        /// <summary>
        /// Handled?
        /// </summary>
        public bool Handled { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionEventArgs
            (
                [NotNull] Exception exception
            )
        {
            Exception = exception;
        }

        #endregion
    }
}
