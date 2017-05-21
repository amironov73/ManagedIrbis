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
using AM.Logging;

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

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            foreach (Stream stream in Streams)
            {
                stream.Flush();
            }
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek
            (
                long offset, 
                SeekOrigin origin
            )
        {
            long result = -1;
            foreach (Stream stream in Streams)
            {
                result = stream.Seek(offset, origin);
            }

            return result;
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength
            (
                long value
            )
        {
            foreach (Stream stream in Streams)
            {
                stream.SetLength(value);
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
            Log.Error
                (
                    "MultiStream::Read: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="Stream.Write" />
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

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek
        {
            get
            {
                return Streams.All(stream => stream.CanSeek);
            }
        }

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc cref="Stream.Length" />
        public override long Length
        {
            get
            {
                Stream first = Streams.FirstOrDefault();
                long result = ReferenceEquals(first, null)
                    ? -1L
                    : first.Length;

                return result;
            }
        }

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get
            {
                Stream first = Streams.FirstOrDefault();
                long result = ReferenceEquals(first, null)
                    ? -1L
                    : first.Position;

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

        /// <inheritdoc cref="Stream.Close" />
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
