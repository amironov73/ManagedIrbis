/* NotifyStream.cs -- stream with write notifications
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// <see cref="Stream"/> with write notifications.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class NotifyStream
        : Stream,
          IDisposable
    {
        #region Events

        /// <summary>
        /// Issued when stream content is changed.
        /// </summary>
        public event EventHandler StreamChanged;

        #endregion

        #region Properties

        private readonly Stream _baseStream;

        /// <summary>
        /// Base stream.
        /// </summary>
        public Stream BaseStream
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        public NotifyStream(Stream baseStream)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException();
            }
            _baseStream = baseStream;
        }

        /// <summary>
        /// Releases unmanaged resources and performs 
        /// other cleanup operations before the
        /// <see cref="T:AM.IO.NotifyStream"/> 
        /// is reclaimed by garbage collection.
        /// </summary>
        ~NotifyStream()
        {
            Dispose();
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Called when stream content is changed.
        /// </summary>
        protected virtual void OnStreamChanged()
        {
            if (StreamChanged != null)
            {
                StreamChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Stream members

        /// <summary>
        /// When overridden in a derived class, gets
        /// a value indicating whether the current
        /// stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading;
        /// otherwise, false.</returns>
        public override bool CanRead
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanRead;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets
        /// a value indicating whether the current
        /// stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports seeking;
        /// otherwise, false.</returns>
        public override bool CanSeek
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanSeek;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanWrite;
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Flush()
        {
            _baseStream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.Length;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Position
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.Position;
            }
            [DebuggerStepThrough]
            set
            {
                _baseStream.Position = value;
            }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
            OnStreamChanged();
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
        public override void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            _baseStream.Write(buffer, offset, count);
            OnStreamChanged();
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.IO.Stream"></see>.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (_baseStream != null)
            {
                _baseStream.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}