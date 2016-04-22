/* FileOutput.cs -- файловый вывод
 */

#region

using System;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Файловый вывод.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FileOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get { return _fileName; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FileOutput()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FileOutput
            (
                string fileName
            )
        {
            Open
                (
                    fileName
                );
        }

        #endregion

        #region Private members

        private string _fileName;

        private TextWriter _writer;

        #endregion

        #region Public methods

        /// <summary>
        /// Закрытие файла.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName,
                bool append
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            Close();
            _fileName = fileName;
            _writer = new StreamWriter
                (
                    fileName,
                    append
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName
            )
        {
            Open
                (
                    fileName, 
                    false
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName,
                bool append,
                Encoding encoding
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (ReferenceEquals(encoding, null))
            {
                throw new ArgumentNullException("encoding");
            }
            Close();
            _fileName = fileName;
            _writer = new StreamWriter
                (
                    fileName,
                    append,
                    encoding
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName, 
                Encoding encoding
            )
        {
            Open
                (
                    fileName,
                    false,
                    encoding
                );
        }

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }

        public override AbstractOutput Clear()
        {
            // TODO: implement properly
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            Open(configuration);
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            if (!ReferenceEquals(_writer, null))
            {
                _writer.Write(text);
                _writer.Flush();
            }
            return this;
        }

        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;
            if (!ReferenceEquals(_writer, null))
            {
                _writer.Write(text);
                _writer.Flush();
            }
            return this;
        }

        #endregion

        #region IDisposable members

        public override void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
