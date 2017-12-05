// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NotifyStream.cs -- stream with write notifications
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using CodeJam;

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
        public NotifyStream
            (
                [NotNull] Stream innerStream
            )
        {
            Code.NotNull(innerStream, "innerStream");

            InnerStream = innerStream;
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Called when stream content is changed.
        /// </summary>
        protected virtual void OnStreamChanged()
        {
            StreamChanged.Raise(this);
        }

        #endregion

        #region Stream members

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

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            InnerStream.Flush();
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
            OnStreamChanged();
        }

        /// <inheritdoc cref="Stream.Write" />
        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
            OnStreamChanged();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        void IDisposable.Dispose()
        {
            InnerStream.Dispose();
        }

        #endregion
    }
}
