// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// Null logger.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public sealed class NullLogger
        : IAmLogger
    {
        #region IAmLogger members

        /// <inheritdoc cref="IAmLogger.Debug" />
        public void Debug
            (
                string text
            )
        {
        }

        /// <inheritdoc cref="IAmLogger.Error" />
        public void Error
            (
                string text
            )
        {
        }

        /// <inheritdoc cref="IAmLogger.Fatal" />
        public void Fatal
            (
                string text
            )
        {
        }

        /// <inheritdoc cref="IAmLogger.Info" />
        public void Info
            (
                string text
            )
        {
        }

        /// <inheritdoc cref="IAmLogger.Trace" />
        public void Trace
            (
                string text
            )
        {
        }

        /// <inheritdoc cref="IAmLogger.Warn" />
        public void Warn
            (
                string text
            )
        {
        }

        #endregion
    }
}
