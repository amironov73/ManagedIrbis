// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using UnsafeAM.IO;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Logging
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
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
            Code.NotNullNorEmpty(fileName, nameof(fileName));

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
                using (StreamWriter writer = TextWriterUtility.Append
                    (
                        FileName,
                        Encoding.UTF8
                    ))
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
