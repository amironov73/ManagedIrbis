// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || DESKTOP || NETCORE || DROID || UWP

#region Using directives

using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FileLogger
        : IAmLogger
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileLogger
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Write one line.
        /// </summary>
        [NotNull]
        public FileLogger WriteLine
            (
                [CanBeNull] string line
            )
        {
            if (!ReferenceEquals(line, null))
            {
                using (StreamWriter writer
#if NETCORE
                    = new StreamWriter
                    (
                        new FileStream (FileName, FileMode.Append),
                        Encoding.UTF8
                    )
#else
                    = new StreamWriter(FileName, true)
#endif
                    )
                {
                    writer.WriteLine(line);
                }
            }

            return this;
        }

#endregion

        #region IAMLogger members

        /// <inheritdoc cref="IAmLogger.Debug" />
        public void Debug
            (
                string text
            )
        {
            WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Error" />
        public void Error
            (
                string text
            )
        {
            WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Fatal" />
        public void Fatal
            (
                string text
            )
        {
            WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Info" />
        public void Info
            (
                string text
            )
        {
            WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Trace" />
        public void Trace
            (
                string text
            )
        {
            WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Warn" />
        public void Warn
            (
                string text
            )
        {
            WriteLine(text);
        }

        #endregion
    }
}

#endif
