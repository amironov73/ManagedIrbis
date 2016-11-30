// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XrfFile64.cs -- cross-reference file reading
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81

#region Using directives

using System;
using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Файл перекрестных ссылок XRF представляет собой
    /// таблицу ссылок на записи файла документов.
    /// Первая ссылка соответствует записи файла документов
    /// с номером 1, вторая – 2  и тд.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class XrfFile64
        : IDisposable
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        /// <summary>
        /// XRF located in memory?
        /// </summary>
        public bool InMemory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="inMemory"></param>
        public XrfFile64
            (
                [NotNull] string fileName,
                bool inMemory
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
            InMemory = inMemory;

            _stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                   FileShare.ReadWrite
                );

            if (InMemory)
            {
                byte[] buffer = new byte[(int)_stream.Length];
                Stream memory = new MemoryStream(buffer);
                _stream.Read(buffer, 0, buffer.Length);
                _stream.Dispose();
                _stream = memory;
            }
        }

        #endregion

        #region Private members

        private Stream _stream;

        private long _GetOffset
            (
                int mfn
            )
        {
            long result = unchecked(XrfRecord64.RecordSize * (mfn - 1));
            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record.
        /// </summary>
        [NotNull]
        public XrfRecord64 ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            long offset = _GetOffset(mfn);
            if (offset >= _stream.Length)
            {
                throw new ArgumentOutOfRangeException("mfn");
            }

            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            long ofs = _stream.ReadInt64Network();
            int flags = _stream.ReadInt32Network();

            XrfRecord64 result = new XrfRecord64
                {
                    Mfn = mfn,
                    Offset = ofs,
                    Status = (RecordStatus)flags
                };

            return result;
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                [NotNull] XrfRecord64 record
            )
        {
            Code.NotNull(record, "record");

            throw new NotImplementedException("WriteRecord");
        }

        /// <summary>
        /// Lock the record.
        /// </summary>
        public void LockRecord
            (
                int mfn,
                bool flag
            )
        {
            XrfRecord64 record = ReadRecord(mfn);
            if (flag != record.Locked)
            {
                WriteRecord(record);
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        #endregion
    }
}

#endif

