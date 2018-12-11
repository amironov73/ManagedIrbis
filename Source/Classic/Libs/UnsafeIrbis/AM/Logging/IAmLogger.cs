// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IAmLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Logging
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public interface IAmLogger
    {
        /// <summary>
        /// Debug.
        /// </summary>
        void Debug
            (
                [NotNull] string text
            );

        /// <summary>
        /// Error.
        /// </summary>
        void Error
            (
                [NotNull] string text
            );

        /// <summary>
        /// Fatal error.
        /// </summary>
        void Fatal
            (
                [NotNull] string text
            );

        /// <summary>
        /// Information message.
        /// </summary>
        void Info
            (
                [NotNull] string text
            );

        /// <summary>
        /// Trace.
        /// </summary>
        void Trace
            (
                [NotNull] string text
            );

        /// <summary>
        /// Warning.
        /// </summary>
        void Warn
            (
                [NotNull] string text
            );
    }
}
