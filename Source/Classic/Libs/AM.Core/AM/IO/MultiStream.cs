// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MultiStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AM.Collections;

using CodeJam;

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
    public class MultiStream
        : Stream
    {
        #region Properties

        private NonNullCollection<Stream> _streams;

        /// <summary>
        /// Gets the streams.
        /// </summary>
        /// <value>The streams.</value>
        public NonNullCollection<Stream> Streams
        {
            [DebuggerStepThrough]
            get
            {
                return _streams;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiStream"/> class.
        /// </summary>
        public MultiStream()
        {
            _streams = new NonNullCollection<Stream>();
        }

        #endregion

        #region Stream members

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream 
        /// and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public override void Flush()
        {
            foreach (Stream stream in Streams)
            {
                stream.Flush();
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the 
        /// current stream.
        /// </summary>
        /// <returns>The new position within the current stream.</returns>
        /// <param name="offset">A byte offset relative to the origin parameter.
        /// </param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/>
        /// indicating the reference point used to obtain the new position.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">The stream does 
        /// not support seeking, such as if the stream is constructed from 
        /// a pipe or console output. </exception>
        /// <exception cref="ObjectDisposedException">Methods were 
        /// called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long result = -1;
            foreach (Stream stream in Streams)
            {
                result = stream.Seek(offset, origin);
            }
            return result;
        }

        /// <summary>
        /// When overridden in a derived class, sets the length 
        /// of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current 
        /// stream in bytes.</param>
        /// <exception cref="T:System.NotSupportedException">The stream 
        /// does not support both writing and seeking, such as if the stream 
        /// is constructed from a pipe or console output.</exception>
        /// <exception cref="IOException">An I/O error occurs.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods 
        /// were called after the stream was closed.</exception>
        public override void SetLength(long value)
        {
            foreach (Stream stream in Streams)
            {
                stream.SetLength(value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes 
        /// from the current stream and advances the position within the stream 
        /// by the number of bytes read.
        /// </summary>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than 
        /// the number of bytes requested if that many bytes are not currently 
        /// available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <param name="offset">The zero-based byte offset in buffer at 
        /// which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read 
        /// from the current stream.</param>
        /// <param name="buffer">An array of bytes. When this method 
        /// returns, the buffer contains the specified byte array with the values 
        /// between offset and (offset + count - 1) replaced by the bytes read 
        /// from the current source.</param>
        /// <exception cref="ArgumentException">The sum of offset and 
        /// count is larger than the buffer length.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called 
        /// after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does 
        /// not support reading.</exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count 
        /// is negative.</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes 
        /// to the current stream and advances the current position within 
        /// this stream by the number of bytes written.
        /// </summary>
        /// <param name="offset">The zero-based byte offset in buffer at which 
        /// to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the 
        /// current stream.</param>
        /// <param name="buffer">An array of bytes. This method copies count 
        /// bytes from buffer to the current stream. </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The stream does not 
        /// support writing.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called 
        /// after the stream was closed. </exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="ArgumentException">The sum of offset and count 
        /// is greater than the buffer length.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count 
        /// is negative.</exception>
        public override void Write
            (
                byte[] buffer, 
                int offset, 
                int count
            )
        {
            Code.NotNull(buffer, "buffer");

            foreach (Stream stream in Streams)
            {
                stream.Write(buffer, offset, count);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating 
        /// whether the current stream supports reading.
        /// </summary>
        /// <returns>
        /// true if the stream supports reading; otherwise, false.
        /// </returns>
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating 
        /// whether the current stream supports seeking.
        /// </summary>
        /// <returns>
        /// true if the stream supports seeking; otherwise, false.
        /// </returns>
        public override bool CanSeek
        {
            get
            {
                return Streams.All(stream => stream.CanSeek);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating 
        /// whether the current stream supports writing.
        /// </summary>
        /// <returns>
        /// true if the stream supports writing; otherwise, false.
        /// </returns>
        public override bool CanWrite
        {
            get
            {
                return Streams.All(stream => stream.CanSeek);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length 
        /// in bytes of the stream.
        /// </summary>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        /// </returns>
        /// <exception cref="NotSupportedException">A class derived from 
        /// Stream does not support seeking.</exception>
        ///<exception cref="ObjectDisposedException">Methods were called 
        /// after the stream was closed.</exception>
        public override long Length
        {
            get
            {
                Stream stream = Streams.FirstOrDefault();
                long result = (stream == null)
                    ? -1L
                    : stream.Length;

                return result;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position 
        /// within the current stream.
        /// </summary>
        /// <returns>
        /// The current position within the stream.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The stream does 
        /// not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called 
        /// after the stream was closed.</exception>
        public override long Position
        {
            get
            {
                Stream stream = Streams.FirstOrDefault();
                long result = (stream == null)
                    ? -1L
                    : stream.Position;

                return result;
            }
            set
            {
                foreach (Stream stream in Streams)
                {
                    stream.Position = value;
                }
            }
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets 
        /// and file handles) associated with the current stream.
        /// </summary>
        public
#if !NETCORE && !UAP && !WIN81 && !PORTABLE
            override 
#endif
            void Close()
        {

#if PORTABLE

            Dispose();

#elif !NETCORE && !UAP && !WIN81

            base.Close();

#endif

            foreach (Stream stream in Streams)
            {
                stream.Dispose();
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);

            foreach (Stream stream in Streams)
            {
                stream.Dispose();
            }
        }

        #endregion
    }
}
