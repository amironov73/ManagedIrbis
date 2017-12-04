// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonBufferedStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

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
    public sealed class NonBufferedStream
        : Stream
    {
        #region Properties

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
        public NonBufferedStream
            (
                [NotNull] Stream innerStream
            )
        {
            Code.NotNull(innerStream, "innerStream");

            InnerStream = innerStream;
        }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            InnerStream.Flush();
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek
            (
                long offset,
                SeekOrigin origin
            )
        {
            return InnerStream.Seek(offset, origin);
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength
            (
                long value
            )
        {
            InnerStream.SetLength(value);
        }

        /// <inheritdoc cref="Stream.Read" />
        public override int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Flush();

            return InnerStream.Read(buffer, offset, count);
        }

        /// <inheritdoc cref="Stream.Write" />
        public override void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            InnerStream.Write(buffer, offset, count);
            Flush();
        }

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead
        {
            get { return InnerStream.CanRead; }
        }

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek
        {
            get { return InnerStream.CanSeek; }
        }

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite
        {
            get { return InnerStream.CanWrite; }
        }

        /// <inheritdoc cref="Stream.Length" />
        public override long Length
        {
            get { return InnerStream.Length; }
        }

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get { return InnerStream.Position; }
            set { InnerStream.Position = value; }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return InnerStream.ToString();
        }

        #endregion
    }
}
