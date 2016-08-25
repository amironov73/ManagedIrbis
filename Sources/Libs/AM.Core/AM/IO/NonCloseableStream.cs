/* NonCloseableStream.cs -- stream that likes to be non-closed.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
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
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NonCloseableStream"/> class.
        /// </summary>
        public NonCloseableStream
            (
                [NotNull] Stream innerStream
            )
        {
            Code.NotNull(innerStream, "innerStream");

            _innerStream = innerStream;
        }

        #endregion

        #region Private members

        private readonly Stream _innerStream;

        #endregion

        #region Public methods

        /// <summary>
        /// Really closes the stream.
        /// </summary>
        public virtual void ReallyClose()
        {
            _innerStream.Dispose();
        }

        #endregion

        #region Stream members

        /// <summary>
        /// Gets a value indicating whether the current
        /// stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            [DebuggerStepThrough]
            get
            {
                return _innerStream.CanRead;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if the stream supports seeking; 
        /// otherwise, <c>false</c>.</returns>
        public override bool CanSeek
        {
            [DebuggerStepThrough]
            get
            {
                return _innerStream.CanSeek;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if the stream supports writing; 
        /// otherwise, <c>false</c>.</returns>
        public override bool CanWrite
        {
            [DebuggerStepThrough]
            get
            {
                return _innerStream.CanWrite;
            }
        }

#if !NETCORE

        /// <summary>
        /// NOT closes the current stream and releases any resources 
        /// (such as sockets and file handles) associated with the current stream.
        /// </summary>
        /// <seealso cref="M:AM.IO.NonCloseable.NonCloseableStream.ReallyClose"/>
        [DebuggerStepThrough]
        public override void Close()
        {
            // Nothing to do actually
        }

#endif

        /// <summary>
        /// Clears all buffers for this stream and causes any 
        /// buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs. </exception>
        [DebuggerStepThrough]
        public override void Flush()
        {
            _innerStream.Flush();
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the 
        /// length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// A class derived from Stream does not support seeking. 
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed. </exception>
        public override long Length
        {
            [DebuggerStepThrough]
            get
            {
                return _innerStream.Length;
            }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O 
        /// error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream 
        /// does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods 
        /// were called after the stream was closed. </exception>
        public override long Position
        {
            [DebuggerStepThrough]
            get
            {
                return _innerStream.Position;
            }
            [DebuggerStepThrough]
            set
            {
                _innerStream.Position = value;
            }
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream 
        /// and advances the position within the stream by the 
        /// number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When 
        /// this method returns, the buffer contains the specified 
        /// byte array with the values between offset and 
        /// (offset + count - 1) replaced by the bytes read from 
        /// the current source.</param>
        /// <param name="offset">The zero-based byte offset 
        /// in buffer at which to begin storing the data read from 
        /// the current stream.</param>
        /// <param name="count">The maximum number of 
        /// bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can 
        /// be less than the number of bytes requested if that many 
        /// bytes are not currently available, or zero (0) if the end of 
        /// the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The sum 
        /// of offset and count is larger than the buffer length. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods 
        /// were called after the stream was closed. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream 
        /// does not support reading. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer 
        /// is <c>null</c>. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error 
        /// occurs. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// offset or count is negative. </exception>
        [DebuggerStepThrough]
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.
        /// </param>
        /// <param name="origin">A value of type 
        /// <see cref="T:System.IO.SeekOrigin"></see> indicating the reference 
        /// point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The stream 
        /// does not support seeking, such as if the stream is constructed from 
        /// a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods 
        /// were called after the stream was closed. </exception>
        [DebuggerStepThrough]
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of 
        /// the current stream in bytes.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support both writing and seeking, such as 
        /// if the stream is constructed from a pipe or console output. 
        /// </exception>
        /// <exception cref="T:System.IO.IOException">An I/O 
        /// error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed. </exception>
        [DebuggerStepThrough]
        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances 
        /// the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method 
        /// copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset 
        /// in buffer at which to begin copying bytes to the current stream.
        /// </param>
        /// <param name="count">The number of bytes to be written 
        /// to the current stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The stream 
        /// does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods 
        /// were called after the stream was closed. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer 
        /// is <c>null</c>. </exception>
        /// <exception cref="T:System.ArgumentException">The sum 
        /// of offset and count is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// offset or count is negative. </exception>
        [DebuggerStepThrough]
        public override void Write(byte[] buffer, int offset, int count)
        {
            _innerStream.Write(buffer, offset, count);
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Releases all resources used by the 
        /// <see cref="T:System.IO.Stream"></see>.
        /// </summary>
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
