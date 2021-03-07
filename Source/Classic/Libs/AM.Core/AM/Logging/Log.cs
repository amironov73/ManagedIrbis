// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Log.cs -- common logging point
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
    //
    // NLog log levels
    //
    // * Fatal - something bad happened; application is going down
    // * Error - something failed; application may or may not continue
    // * Warn  - something unexpected; application will continue
    // * Info  - normal behavior like mail sent, user updated profile etc.
    // * Debug - for debugging; executed query, user authenticated, session expired
    // * Trace - for trace debugging; begin method X, end method X
    //

    /// <summary>
    /// Common logging point for all the ArsMagna subrpojects.
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

        #region Private members

        private static readonly object SyncRoot = new object();

        #endregion

        #region Public methods

        /// <summary>
        /// Apply defaults for console application.
        /// </summary>
        public static void ApplyDefaultsForConsoleApplication()
        {
#if CLASSIC || DESKTOP || NETCORE

            TeeLogger root = new TeeLogger();
            Logger = root;
            root.Loggers.AddRange
                (
                    new IAmLogger[]
                    {
                        new ConsoleLogger(),
                        new TraceLogger()
                    }
                );

#endif
        }

        /// <summary>
        /// Apply defaults for console application.
        /// </summary>
        public static void ApplyDefaultsForServiceApplication()
        {
#if CLASSIC || DESKTOP

            TeeLogger root = new TeeLogger();
            Logger = root;
            root.Loggers.AddRange
                (
                    new IAmLogger[]
                    {
                        new TraceLogger()
                    }
                );

#endif
        }

        /// <summary>
        /// Apply defaults for console application.
        /// </summary>
        public static void ApplyDefaultsForWindowedApplication()
        {
#if CLASSIC || DESKTOP

            TeeLogger root = new TeeLogger();
            Logger = root;
            root.Loggers.AddRange
                (
                    new IAmLogger[]
                    {
                        new TraceLogger()
                    }
                );

#endif
        }

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
