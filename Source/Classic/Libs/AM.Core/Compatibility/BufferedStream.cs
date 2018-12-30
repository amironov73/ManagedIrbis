// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BufferedStream.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NOTDEF

//using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    public class BufferedStream
        : Stream
    {
        #region Construction

        public BufferedStream
            (
                Stream stream
            )
        {
            _stream = stream;
        }

        #endregion

        #region Private members

        private readonly Stream _stream;

        #endregion

        #region Stream members

        //public object GetLifetimeService()
        //{
        //    return _stream.GetLifetimeService();
        //}

        //public object InitializeLifetimeService()
        //{
        //    return _stream.InitializeLifetimeService();
        //}

        //public ObjRef CreateObjRef(Type requestedType)
        //{
        //    return _stream.CreateObjRef(requestedType);
        //}

        public Task CopyToAsync(Stream destination)
        {
            return _stream.CopyToAsync(destination);
        }

        public Task CopyToAsync(Stream destination, int bufferSize)
        {
            return _stream.CopyToAsync(destination, bufferSize);
        }

        public Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _stream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public void CopyTo(Stream destination)
        {
            _stream.CopyTo(destination);
        }

        public void CopyTo(Stream destination, int bufferSize)
        {
            _stream.CopyTo(destination, bufferSize);
        }

        //public void Close()
        //{
        //    _stream.Close();
        //}

        public void Dispose()
        {
            _stream.Dispose();
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public Task FlushAsync()
        {
            return _stream.FlushAsync();
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return _stream.FlushAsync(cancellationToken);
        }

        //public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        //{
        //    return _stream.BeginRead(buffer, offset, count, callback, state);
        //}

        //public int EndRead(IAsyncResult asyncResult)
        //{
        //    return _stream.EndRead(asyncResult);
        //}

        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return _stream.ReadAsync(buffer, offset, count);
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        //public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        //{
        //    return _stream.BeginWrite(buffer, offset, count, callback, state);
        //}

        //public void EndWrite(IAsyncResult asyncResult)
        //{
        //    _stream.EndWrite(asyncResult);
        //}

        public Task WriteAsync(byte[] buffer, int offset, int count)
        {
            return _stream.WriteAsync(buffer, offset, count);
        }

        public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public bool CanTimeout
        {
            get { return _stream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public int ReadTimeout
        {
            get { return _stream.ReadTimeout; }
            set { _stream.ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return _stream.WriteTimeout; }
            set { _stream.WriteTimeout = value; }
        }

        #endregion
    }
}

#endif