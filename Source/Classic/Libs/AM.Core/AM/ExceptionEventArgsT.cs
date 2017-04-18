// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionEventArgsT.cs -- information about exception
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Information about exception.
    /// </summary>
    [PublicAPI]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Exception} {Handled}")]
#endif
    public sealed class ExceptionEventArgs<T>
        : EventArgs
        where T: Exception
    {
        #region Properties

        /// <summary>
        /// Exception.
        /// </summary>
        [NotNull]
        public T Exception { get; private set; }

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
                [NotNull] T exception
            )
        {
            Code.NotNull(exception, "exception");

            Exception = exception;
        }

        #endregion
    }
}
