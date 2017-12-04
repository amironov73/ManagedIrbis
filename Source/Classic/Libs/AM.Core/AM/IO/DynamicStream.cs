// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DynamicStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DynamicStream
        : Stream
    {
        #region Properties

        /// <summary>
        /// Handles Flush method.
        /// </summary>
        [CanBeNull]
        public Action FlushHandler;

        /// <summary>
        /// Handles Seek method.
        /// </summary>
        [CanBeNull]
        public Func<long, SeekOrigin, long> SeekHandler;

        /// <summary>
        /// Handles SetLength method.
        /// </summary>
        [CanBeNull]
        public Action<long> SetLengthHandler;

        /// <summary>
        /// Handles Read method.
        /// </summary>
        [CanBeNull]
        public Func<byte[], int, int, int> ReadHandler;

        /// <summary>
        /// Handles Write method.
        /// </summary>
        [CanBeNull]
        public Action<byte[], int, int> WriteHandler;

        /// <summary>
        /// Handles CanRead property.
        /// </summary>
        [CanBeNull]
        public Func<bool> CanReadHandler;

        /// <summary>
        /// Handles CanSeek property.
        /// </summary>
        [CanBeNull]
        public Func<bool> CanSeekHandler;

        /// <summary>
        /// Handles CanWrite property.
        /// </summary>
        [CanBeNull]
        public Func<bool> CanWriteHandler;

        /// <summary>
        /// Handles Length property.
        /// </summary>
        [CanBeNull]
        public Func<long> GetLengthHandler;

        /// <summary>
        /// Handles Position property getter.
        /// </summary>
        [CanBeNull]
        public Func<long> GetPositionHandler;

        /// <summary>
        /// Handles Position property setter.
        /// </summary>
        [CanBeNull]
        public Action<long> SetPositionHandler;

        /// <summary>
        /// Handles Dispose method.
        /// </summary>
        [CanBeNull]
        public Action DisposeHandler;

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            Action handler = FlushHandler;
            if (!ReferenceEquals(handler, null))
            {
                handler();
            }
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek
            (
                long offset,
                SeekOrigin origin
            )
        {
            Func<long, SeekOrigin, long> handler = SeekHandler;
            long result = 0;
            if (!ReferenceEquals(handler, null))
            {
                result = handler(offset, origin);
            }

            return result;
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength
            (
                long value
            )
        {
            Action<long> handler = SetLengthHandler;
            if (!ReferenceEquals(handler, null))
            {
                handler(value);
            }
        }

        /// <inheritdoc cref="Stream.Read" />
        public override int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Func<byte[], int, int, int> handler = ReadHandler;
            int result = 0;
            if (!ReferenceEquals(handler, null))
            {
                result = handler(buffer, offset, count);
            }

            return result;
        }

        /// <inheritdoc cref="Stream.Write" />
        public override void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Action<byte[], int, int> handler = WriteHandler;
            if (!ReferenceEquals(handler, null))
            {
                handler(buffer, offset, count);
            }
        }

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead
        {
            get
            {
                bool result = false;
                Func<bool> handler = CanReadHandler;
                if (!ReferenceEquals(handler, null))
                {
                    result = handler();
                }

                return result;
            }
        }

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek
        {
            get
            {
                bool result = false;
                Func<bool> handler = CanSeekHandler;
                if (!ReferenceEquals(handler, null))
                {
                    result = handler();
                }

                return result;
            }
        }

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite
        {
            get
            {
                bool result = false;
                Func<bool> handler = CanWriteHandler;
                if (!ReferenceEquals(handler, null))
                {
                    result = handler();
                }

                return result;
            }
        }

        /// <inheritdoc cref="Stream.Length" />
        public override long Length
        {
            get
            {
                Func<long> handler = GetLengthHandler;
                long result = 0;
                if (!ReferenceEquals(handler, null))
                {
                    result = handler();
                }

                return result;
            }
        }

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get
            {
                Func<long> handler = GetPositionHandler;
                long result = 0;
                if (!ReferenceEquals(handler, null))
                {
                    result = handler();
                }

                return result;
            }
            set
            {
                Action<long> handler = SetPositionHandler;
                if (!ReferenceEquals(handler, null))
                {
                    handler(value);
                }
            }
        }

        /// <inheritdoc cref="Stream.Dispose(bool)" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            Action handler = DisposeHandler;
            if (!ReferenceEquals(handler, null))
            {
                handler();
            }
        }

        #endregion
    }
}
