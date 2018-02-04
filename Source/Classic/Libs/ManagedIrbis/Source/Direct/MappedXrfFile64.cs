// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MappedXrfFile64.cs -- super-fast XRF-file accessor using memory-mapped files
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !FW35 && !UAP && !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.IO;
using System.IO.MemoryMappedFiles;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Super-fast XRF-file accessor using memory-mapped files.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MappedXrfFile64
        : IDisposable
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
        public MappedXrfFile64
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
            _lockObject = new object();
            //_mapping = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open);
            _mapping = DirectUtility.OpenMemoryMappedFile(fileName);
            _stream = _mapping.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
        }

        #endregion

        #region Private members

        private readonly object _lockObject;

        private readonly MemoryMappedFile _mapping;

        private readonly MemoryMappedViewStream _stream;

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

            lock (_lockObject)
            {
                long position = (long) XrfRecord64.RecordSize * (mfn - 1);
                _stream.Seek(position, SeekOrigin.Begin);
                XrfRecord64 result = new XrfRecord64
                {
                    Mfn = mfn,
                    Offset = _stream.ReadNetworkInt64(),
                    Status = (RecordStatus) _stream.ReadNetworkInt32()
                };

                return result;
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _stream.Dispose();
            _mapping.Dispose();
        }

        #endregion
    }
}


#endif
