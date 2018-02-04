// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MappedMstFile64.cs -- super-fast MST-file accessor using memory-mapped files
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !FW35 && !UAP && !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Super-fast MST-file accessor using memory-mapped files.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MappedMstFile64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Control record.
        /// </summary>
        [NotNull]
        public MstControlRecord64 ControlRecord { get; private set; }

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
        public MappedMstFile64
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;

            _lockObject = new object();
            _mapping = DirectUtility.OpenMemoryMappedFile(fileName);
            _stream = _mapping.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            ControlRecord = MstControlRecord64.Read(_stream);
        }

        #endregion

        #region Private members

        private readonly object _lockObject;

        private readonly MemoryMappedFile _mapping;

        private readonly MemoryMappedViewStream _stream;

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record (with preload optimization).
        /// </summary>
        [NotNull]
        public MstRecord64 ReadRecord
            (
                long position
            )
        {
            lock (_lockObject)
            {
                Encoding encoding = IrbisEncoding.Utf8;
                List<MstDictionaryEntry64> dictionary
                    = new List<MstDictionaryEntry64>();

                _stream.Seek(position, SeekOrigin.Begin);
                MstRecordLeader64 leader = MstRecordLeader64.Read(_stream);

                for (int i = 0; i < leader.Nvf; i++)
                {
                    MstDictionaryEntry64 entry = new MstDictionaryEntry64
                    {
                        Tag = _stream.ReadInt32Network(),
                        Position = _stream.ReadInt32Network(),
                        Length = _stream.ReadInt32Network()
                    };
                    dictionary.Add(entry);
                }

                foreach (MstDictionaryEntry64 entry in dictionary)
                {
                    long endOffset = leader.Base + entry.Position;
                    _stream.Seek(position + endOffset, SeekOrigin.Begin);
                    entry.Bytes = StreamUtility.ReadBytes(_stream, entry.Length);
                    if (!ReferenceEquals(entry.Bytes, null))
                    {
                        byte[] buffer = entry.Bytes;
                        entry.Text = encoding.GetString(buffer, 0, buffer.Length);
                    }
                }

                MstRecord64 result = new MstRecord64
                {
                    Leader = leader,
                    Dictionary = dictionary
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
