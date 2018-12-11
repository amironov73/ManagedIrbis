// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Log.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Logging
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class TimeStampLogger
        : IAmLogger
    {
        #region Properties

        /// <summary>
        /// Inner logger.
        /// </summary>
        [NotNull]
        public IAmLogger Inner { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TimeStampLogger
            (
                [NotNull] IAmLogger inner
            )
        {
            Code.NotNull(inner, nameof(inner));

            Inner = inner;
        }

        #endregion

        #region Private members

        [NotNull]
        private static string _Prepend
            (
                [CanBeNull] string text
            )
        {
            return DateTime.Now.ToLongUniformString() + ": " + text;
        }

        #endregion

        #region IAmLogger members

        /// <inheritdoc cref="IAmLogger.Debug" />
        public void Debug
            (
                string text
            )
        {
            Inner.Debug(_Prepend(text));
        }

        /// <inheritdoc cref="IAmLogger.Error" />
        public void Error
            (
                string text
            )
        {
            Inner.Error(_Prepend(text));
        }

        /// <inheritdoc cref="IAmLogger.Fatal" />
        public void Fatal
            (
                string text
            )
        {
            Inner.Fatal(_Prepend(text));
        }

        /// <inheritdoc cref="IAmLogger.Info" />
        public void Info
            (
                string text
            )
        {
            Inner.Info(_Prepend(text));
        }

        /// <inheritdoc cref="IAmLogger.Trace" />
        public void Trace
            (
                string text
            )
        {
            Inner.Trace(_Prepend(text));
        }

        /// <inheritdoc cref="IAmLogger.Warn" />
        public void Warn
            (
                string text
            )
        {
            Inner.Warn(_Prepend(text));
        }

        #endregion
    }
}
