// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonCloseableStream.cs -- stream that likes to be non-closed.
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

namespace AM.IO
{
    /// <summary>
    /// Stream that likes to be non-closed.
    /// To close the stream call 
    /// <see cref="M:AM.IO.NonCloseable.NonCloseableStream.ReallyClose"/>.
    /// </summary>
    [PublicAPI]
    public class NonCloseableStream
        : Stream,
        IDisposable
    {
        #region Porperties

        /// <summary>
        /// Inner stream.
        /// </summary>
        [NotNull]
        public Stream InnerStream { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStream
            (
                [NotNull] Stream innerStream
            )
        {
            Code.NotNull(innerStream, "innerStream");

            InnerStream = innerStream;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Really closes the stream.
        /// </summary>
        public virtual void ReallyClose()
        {
            InnerStream.Dispose();
        }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead
        {
            get
            {
                return InnerStream.CanRead;
            }
        }

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek
        {
            get
            {
                return InnerStream.CanSeek;
            }
        }

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite
        {
            get
            {
                return InnerStream.CanWrite;
            }
        }

        /// <summary>
        /// NOT closes the current stream and releases any resources 
        /// (such as sockets and file handles) associated with the current stream.
        /// </summary>
        /// <seealso cref="M:AM.IO.NonCloseable.NonCloseableStream.ReallyClose"/>
        public
#if !NETCORE && !UAP && !WIN81 && !PORTABLE
            override
#endif
            void Close()
        {
            // Nothing to do actually
        }

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            InnerStream.Flush();
        }

        /// <inheritdoc cref="Stream.Length" />
        public override long Length
        {
            get
            {
                return InnerStream.Length;
            }
        }

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get
            {
                return InnerStream.Position;
            }
            set
            {
                InnerStream.Position = value;
            }
        }

        /// <inheritdoc cref="Stream.Read" />
        public override int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength(long value)
        {
            InnerStream.SetLength(value);
        }

        /// <inheritdoc cref="Stream.Write" />
        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
