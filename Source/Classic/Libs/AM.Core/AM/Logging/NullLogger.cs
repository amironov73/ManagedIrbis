// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// Null logger.
    /// </summary>
    [PublicAPI]
    public sealed class NullLogger
        : IAmLogger
    {
        #region IAmLogger members

        /// <inheritdoc/>
        public void Debug
            (
                string text
            )
        {
        }

        /// <inheritdoc/>
        public void Error
            (
                string text
            )
        {
        }

        /// <inheritdoc/>
        public void Fatal
            (
                string text
            )
        {
        }

        /// <inheritdoc/>
        public void Info
            (
                string text
            )
        {
        }

        /// <inheritdoc/>
        public void Trace
            (
                string text
            )
        {
        }

        /// <inheritdoc/>
        public void Warn
            (
                string text
            )
        {
        }

        #endregion
    }
}
