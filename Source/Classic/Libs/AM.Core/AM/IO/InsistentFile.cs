// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InsistentFile.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !PORTABLE && !SILVERLIGHT && !WIN81

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM.Logging;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class InsistentFile
    {
        #region Constants

        /// <summary>
        /// Default value for <see cref="Timeout"/> property.
        /// </summary>
        public const int DefaultTimeout = 10 * 1000;

        /// <summary>
        /// Default value for <see cref="SleepInterval"/> property.
        /// </summary>
        public const int DefaultSleep = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Interval to sleep between subsequential attempts,
        /// milliseconds.
        /// </summary>
        public static int SleepInterval { get; set; }

        /// <summary>
        /// Timeout for operations, milliseconds.
        /// </summary>
        public static int Timeout { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        static InsistentFile()
        {
            SleepInterval = DefaultSleep;
            Timeout = DefaultTimeout;
        }

        #endregion

        #region Private members

        private static T _Evaluate<T>
            (
                [NotNull] Func<T> function
            )
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                try
                {
                    T result = function();

                    return result;
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "InistentFile::_Evaluate",
                            exception
                        );
                }

                int elapsed = (int) stopwatch.ElapsedMilliseconds;
                if (elapsed > Timeout)
                {
                    Log.Error
                        (
                            "InsistentFile::_Evaluate: "
                            + "timeout is exhausted"
                        );

                    throw new ArsMagnaException
                        (
                            "InsistentFile: timeout is exhausted"
                        );
                }

                ThreadUtility.Sleep(SleepInterval);
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc
        /// cref="File.Open(string,FileMode,FileAccess,FileShare)"/>.
        [NotNull]
        public static FileStream Open
            (
                [NotNull] string path,
                FileMode mode,
                FileAccess access,
                FileShare share
            )
        {
            Code.NotNullNorEmpty(path, "path");

            FileStream result = _Evaluate
                (
                    () => File.Open(path, mode, access, share)
                );

            return result;
        }

        /// <summary>
        /// Open specified file for exclusive reading.
        /// </summary>
        [NotNull]
        public static FileStream OpenForExclusiveRead
            (
                [NotNull] string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
        }

        /// <summary>
        /// Open specified file for exclusive writing.
        /// </summary>
        [NotNull]
        public static FileStream OpenForExclusiveWrite
            (
                [NotNull] string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
        }

        /// <summary>
        /// Open specified file for shared reading.
        /// </summary>
        [NotNull]
        public static FileStream OpenForSharedRead
            (
                [NotNull] string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );
        }

        #endregion
    }
}

#endif
