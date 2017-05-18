// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Log.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class Log
    {
        #region Properties

        /// <summary>
        /// Logger.
        /// </summary>
        [CanBeNull]
        public static IAmLogger Logger { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static readonly object SyncRoot = new object();

        #endregion

        #region Public methods

        /// <summary>
        /// Debug message.
        /// </summary>
        public static void Debug
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Debug(text);
                }
            }
        }

        /// <summary>
        /// Error message.
        /// </summary>
        public static void Error
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Error(text);
                }
            }
        }

        /// <summary>
        /// Fatal error message.
        /// </summary>
        public static void Fatal
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Fatal(text);
                }
            }
        }

        /// <summary>
        /// Information message.
        /// </summary>
        public static void Info
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Info(text);
                }
            }
        }

        /// <summary>
        /// Set new logger.
        /// </summary>
        /// <returns>Previous logger</returns>
        [CanBeNull]
        public static IAmLogger SetLogger
            (
                [CanBeNull] IAmLogger logger
            )
        {
            lock (SyncRoot)
            {
                IAmLogger result = Logger;

                Logger = logger;

                return result;
            }
        }

        /// <summary>
        /// Trace message.
        /// </summary>
        public static void Trace
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Trace(text);
                }
            }
        }

        /// <summary>
        /// Trace the exception.
        /// </summary>
        public static void TraceException
            (
                [NotNull] string text,
                [NotNull] Exception exception
            )
        {
            string fullText = string.Format
                (
                    "{0}: {1}: {2}",
                    text,
                    exception.GetType().Name,
                    exception.Message
                );

            Trace(fullText);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        public static void Warn
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!ReferenceEquals(Logger, null))
                {
                    Logger.Warn(text);
                }
            }
        }

        #endregion
    }
}
