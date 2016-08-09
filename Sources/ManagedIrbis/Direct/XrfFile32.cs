/* XrfFile32.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM.IO;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Файл перекрестных ссылок XRF представляет собой
    /// таблицу ссылок на записи файла документов.
    /// Первая ссылка соответствует записи файла документов
    /// с номером 1, вторая – 2  и тд.
    /// </summary>
    public sealed class XrfFile32
        : IDisposable
    {
        #region Constants

        public const int XrfBlockSize = 512;
        public const int XrfBlockCapacity = 127;

        #endregion

        #region Properties

        public string FileName { get; private set; }

        #endregion

        #region Construction

        public XrfFile32
            (
                string fileName
            )
        {
            FileName = fileName;

            _stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
        }

        #endregion

        #region Private members

        private Stream _stream;

        private long _GetOffset
            (
                int mfn
            )
        {
            unchecked
            {
                int blockNumber = (mfn - 1) / XrfBlockCapacity;
                int blockOffset = ((mfn - 1) % XrfBlockCapacity) * 4;
                long result = blockNumber * XrfBlockSize + blockOffset + 4;
                
                return result;
            }
        }

        #endregion

        #region Public methods

        public XrfRecord32 Decode
            (
                int value
            )
        {
            unchecked
            {
                RecordStatus status = 0;
                if ((value & 0xFFFFF800) == 0xFFFFF800)
                {
                    status = RecordStatus.PhysicallyDeleted;
                }
                else if (value < 0)
                {
                    status = RecordStatus.LogicallyDeleted;
                }

                int blockNumber = (value < 0)
                    ? (-(int)(value & 0xFFFFF800) >> 11) - 1
                    : ((value & 0x7FFFF800) >> 11);
                int blockOffset = (value < 0)
                    ? ((-value) & 0x7FF)
                    : (value & 0x7FF);

                XrfRecord32 result = new XrfRecord32
                {
                    BlockNumber = blockNumber,
                    BlockOffset = blockOffset,
                    AbsoluteOffset = MstRecord32.MstBlockSize * (blockNumber - 1) + blockOffset,
                    Status = status
                };

                return result;
            }            
        }

        public XrfRecord32 ReadRecord
            (
                int mfn
            )
        {
            if (mfn <= 0)
            {
                throw new ArgumentOutOfRangeException("mfn");
            }

            long offset = _GetOffset(mfn);
            if (offset >= _stream.Length)
            {
                throw new ArgumentOutOfRangeException("mfn");
            }

            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            //int coded = _stream.ReadInt32Network();
            int coded = _stream.ReadInt32Host();

            return Decode(coded);
        }

        public void WriteRecord
            (
                XrfRecord32 record
            )
        {
            if (record == null)
            {
                throw new ArgumentNullException("record");
            }

        }

        public void LockRecord
            (
                int mfn,
                bool flag
            )
        {
            XrfRecord32 record = ReadRecord(mfn);
            if (flag != record.Locked)
            {
                WriteRecord(record);
            }
        }

        #endregion

        #region IDisposable members

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
