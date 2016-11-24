/* FileObject.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    sealed class FileObject
        : IDisposable
    {
        #region Properties

        public bool AppendMode { get; private set; }

        public string FileName { get; private set; }

        public bool WriteMode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileObject
            (
                [NotNull] string fileName,
                bool writeMode,
                bool appendMode
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
            WriteMode = writeMode;
            AppendMode = appendMode;

            if (writeMode)
            {
                _writer = new StreamWriter(fileName, appendMode);
            }
            else
            {
                _reader = new StreamReader(fileName);
            }
        }

        #endregion

        #region Private members

        private readonly StreamReader _reader;

        private readonly StreamWriter _writer;

        #endregion

        #region Public methods

        public string ReadAll()
        {
            string result = _reader.ReadToEnd();

            return result;
        }

        public string ReadLine()
        {
            string result = _reader.ReadLine();

            return result;
        }

        public void Write
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            _writer.Write(text);
        }

        public void WriteLine
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            _writer.WriteLine(text);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (!ReferenceEquals(_reader, null))
            {
                _reader.Dispose();
            }

            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
