// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsoFileProvider.cs -- client for ISO 2709 file.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Client for ISO 2709 file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IsoFileProvider
        : IrbisProvider
    {
        #region Nested classes

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Layout
        {
            public long Offset;

            public int Length;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Extension of the layout file.
        /// </summary>
        public const string LayoutExtension = ".layout";

        #endregion

        #region Properties

        /// <summary>
        /// Text file path.
        /// </summary>
        [NotNull]
        public string FilePath { get; private set; }

        /// <summary>
        /// Encoding
        /// </summary>
        [NotNull]
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Layout file path.
        /// </summary>
        [NotNull]
        public string LayoutPath
        {
            get
            {
                return Path.ChangeExtension(FilePath, LayoutExtension);
            }
        }

        #endregion

        #region Private members

        private Layout[] _layout;

        private Stream _stream;

        private void _ScanFile()
        {
            string layoutPath = LayoutPath;
            if (File.Exists(layoutPath))
            {
                using (Stream stream = File.OpenRead(layoutPath))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    int itemCount = checked((int)
                        (
                            stream.Length / Marshal.SizeOf(typeof(Layout))
                        ));
                    _layout = new Layout[itemCount];
                    for (int i = 0; i < itemCount; i++)
                    {
                        Layout item = new Layout
                        {
                            Offset = reader.ReadInt64(),
                            Length = reader.ReadInt32()
                        };
                        _layout[i] = item;
                    }
                }

                return;
            }

            List<Layout> list = new List<Layout>();
            _stream.Seek(0, SeekOrigin.Begin);
            int bufferLength = 5;
            byte[] buffer = new byte[bufferLength];
            long streamLength = _stream.Length, position = 0;
            while (position < streamLength)
            {
                int readed = _stream.Read(buffer, 0, bufferLength);
                if (readed != bufferLength)
                {
                    throw new IrbisException();
                }

                int recordLength = FastNumber.ParseInt32(buffer, 0, bufferLength);
                if (recordLength <= 0)
                {
                    throw new IrbisException();
                }

                Layout item = new Layout
                {
                    Offset = position,
                    Length = recordLength
                };
                list.Add(item);
                position += recordLength;
                _stream.Seek(position, SeekOrigin.Begin);
            }

            _layout = list.ToArray();
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsoFileProvider
            (
                [NotNull] string filePath,
                [NotNull] Encoding encoding
            )
        {
            Code.FileExists(filePath, "filePath");
            Code.NotNull(encoding, "encoding");

            FilePath = filePath;
            Encoding = encoding;
            Database = Path.GetFileNameWithoutExtension(filePath);
            _stream = File.OpenRead(filePath);
            _ScanFile();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Delete the layout file.
        /// </summary>
        public void DeleteLayout()
        {
            string layoutPath = LayoutPath;
            FileUtility.DeleteIfExists(layoutPath);
        }

        /// <summary>
        /// Save the layout file.
        /// </summary>
        public void SaveLayout()
        {
            string layoutPath = LayoutPath;
            FileUtility.DeleteIfExists(layoutPath);
            using (Stream stream = File.Create(layoutPath))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                for (int i = 0; i < _layout.Length; i++)
                {
                    Layout item = _layout[i];
                    writer.Write(item.Offset);
                    writer.Write(item.Length);
                }
            }
        }

        #endregion

        #region IrbisProvider members

        /// <inheritdoc cref="IrbisProvider.GetMaxMfn" />
        public override int GetMaxMfn()
        {
            return _layout.Length;
        }

        /// <inheritdoc cref="IrbisProvider.ReadRecord" />
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            if (mfn > _layout.Length)
            {
                return null;
            }

            Layout item = _layout[mfn - 1];
            long offset = item.Offset;
            int length = item.Length;

            _stream.Seek(offset, SeekOrigin.Begin);
            byte[] buffer = new byte[length];
            int readed = _stream.Read(buffer, 0, length);
            if (readed != length)
            {
                throw new IrbisException();
            }

            MemoryStream memory = new MemoryStream(buffer);
            MarcRecord result = Iso2709.ReadRecord(memory, Encoding);
            if (!ReferenceEquals(result, null))
            {
                result.Database = Database;
                result.Mfn = mfn;
            }

            return result;
        }

        /// <inheritdoc cref="IrbisProvider.ReadRecordVersion"/>
        [ExcludeFromCodeCoverage]
        public override MarcRecord ReadRecordVersion
            (
                int mfn,
                int version
            )
        {
            return ReadRecord(mfn);
        }

        /// <inheritdoc cref="IrbisProvider.WriteRecord"/>
        [ExcludeFromCodeCoverage]
        public override void WriteRecord
            (
                MarcRecord record
            )
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            if (!ReferenceEquals(_stream, null))
            {
                _stream.Dispose();
                _stream = null;
            }

            base.Dispose();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return FilePath.ToVisibleString();
        }

        #endregion
    }
}
