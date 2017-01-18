// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NotifyStream.cs -- stream with write notifications
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
        public NotifyStream
            (
                [NotNull] Stream baseStream
            )
        {
            Code.NotNull(baseStream, "baseStream");

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
            StreamChanged.Raise(this);
        }

        #endregion

        #region Stream members

        /// <inheritdoc />
        public override bool CanRead
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanRead;
            }
        }

        /// <inheritdoc />
        public override bool CanSeek
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanSeek;
            }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.CanWrite;
            }
        }

        /// <inheritdoc />
        public override void Flush()
        {
            _baseStream.Flush();
        }

        /// <inheritdoc />
        public override long Length
        {
            [DebuggerStepThrough]
            get
            {
                return _baseStream.Length;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            return _baseStream.Read
                (
                    buffer,
                    offset,
                    count
                );
        }

        /// <inheritdoc />
        public override long Seek
            (
                long offset,
                SeekOrigin origin
            )
        {
            return _baseStream.Seek
                (
                    offset,
                    origin
                );
        }

        /// <inheritdoc />
        public override void SetLength
            (
                long value
            )
        {
            _baseStream.SetLength
                (
                    value
                );

            OnStreamChanged();
        }

        /// <inheritdoc />
        public override void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            _baseStream.Write
                (
                    buffer,
                    offset,
                    count
                );

            OnStreamChanged();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            if (!ReferenceEquals(_baseStream, null))
            {
                _baseStream.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
