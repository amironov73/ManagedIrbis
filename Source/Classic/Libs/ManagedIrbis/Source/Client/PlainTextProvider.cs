// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextProvider.cs -- client for plain text file.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Client for plain text file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PlainTextProvider
        : IrbisProvider
    {
        #region Constants

        /// <summary>
        /// Record separator.
        /// </summary>
        public const string RecordSeparator = "*****";

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

        private StreamReader _reader;

        /// <summary>
        /// Смещения до записей.
        /// </summary>
        private long[] _layout;

        private void _ScanFile()
        {
            Stream stream;

            string layoutPath = LayoutPath;
            if (File.Exists(layoutPath))
            {
                using (stream = File.OpenRead(layoutPath))
                {
                    int length = checked((int) (stream.Length / sizeof(long)));
                    _layout = new long[length];
                    for (int i = 0; i < length; i++)
                    {
                        _layout[i] = stream.ReadInt64Host();
                    }
                }

                return;
            }

            stream = _reader.BaseStream;
            _reader.DiscardBufferedData();
            stream.Seek(0, SeekOrigin.Begin);

            Encoding encoding = _reader.CurrentEncoding;
            List<long> offsets = new List<long> ();
            string line;
            long start = 0, position = 0;
            while ((line = _reader.ReadLine()) != null)
            {
                position += encoding.GetByteCount(line) + 2;
                if (line == RecordSeparator)
                {
                    offsets.Add(start);
                    start = position;
                }
            }

            _layout = offsets.ToArray();
        }

        #endregion

        #region Contruction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextProvider
            (
                [NotNull] string filePath,
                [NotNull] Encoding encoding
            )
        {
            Code.FileExists(filePath, "filePath");
            Code.NotNull(encoding, "encoding");

            FilePath = filePath;
            Database = Path.GetFileNameWithoutExtension(filePath);
            _reader = new StreamReader(filePath, encoding);
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
                for (int i = 0; i < _layout.Length; i++)
                {
                    stream.WriteInt64Network(_layout[i]);
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

            long offset = _layout[mfn-1];
            _reader.DiscardBufferedData();
            _reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            MarcRecord result = new MarcRecord()
            {
                Mfn = mfn,
                Database = Database
            };
            while (true)
            {
                string line = _reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    throw new IrbisException();
                }

                if (line == RecordSeparator)
                {
                    break;
                }

                if (line[0] != '#')
                {
                    throw new IrbisException();
                }

                int pos = line.IndexOf(':') + 1;
                if (pos <= 1 || line[pos] != ' ')
                {
                    throw new IrbisException();
                }

                int tag = FastNumber.ParseInt32(line, 1, pos - 2);
                RecordField field = new RecordField(tag);
                result.Fields.Add(field);
                int start = ++pos, length = line.Length;
                while (pos < length)
                {
                    if (line[pos] == '^')
                    {
                        break;
                    }
                    pos++;
                }

                if (pos != start)
                {
                    field.Value = line.Substring(start, pos - start);
                    start = pos;
                }

                while (start < length - 1)
                {
                    char code = line[++start];
                    pos = ++start;
                    while (pos < length)
                    {
                        if (line[pos] == '^')
                        {
                            break;
                        }
                        pos++;
                    }

                    SubField sub = new SubField
                        (
                            code,
                            line.Substring(start, pos - start)
                        );
                    field.SubFields.Add(sub);
                    start = pos;
                }
            }

            result.Modified = false;
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
            if (!ReferenceEquals(_reader, null))
            {
                _reader.Dispose();
                _reader = null;
            }

            base.Dispose();
        }

        #endregion
    }
}
