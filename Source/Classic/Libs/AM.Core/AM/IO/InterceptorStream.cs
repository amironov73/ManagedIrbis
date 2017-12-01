// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InterceptorStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

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
    public sealed class InterceptorStream
        : Stream
    {
        #region Properties

        /// <summary>
        /// Encoding.
        /// </summary>
        [NotNull]
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Writer.
        /// </summary>
        [NotNull]
        public TextWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InterceptorStream
            (
                [NotNull] TextWriter writer,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(encoding, "encoding");

            Writer = writer;
            Encoding = encoding;
        }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            Writer.Flush();
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="Stream.Read" />
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="Stream.Write" />
        public override void Write(byte[] buffer, int offset, int count)
        {
            Writer.Write(Encoding.GetString(buffer, offset, count));
        }

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead
        {
            get { return false; }
        }

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <inheritdoc cref="Stream.Length" />
        public override long Length
        {
            get { return 0; }
        }

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get { return 0; }
            set
            {
                // Nothing to do here
            }
        }

        #endregion
    }
}
