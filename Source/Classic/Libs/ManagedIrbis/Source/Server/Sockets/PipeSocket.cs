// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PipeSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW46 || NETCORE || ANDROID

#region Using directives

using System.IO;
using System.IO.Pipes;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простейшее подключение через System.IO.Pipes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PipeSocket
        : IrbisServerSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PipeSocket
            (
                [NotNull] PipeStream stream,
                CancellationToken token
            )
        {
            Code.NotNull(stream, "stream");

            _stream = stream;
            _token = token;
        }

        #endregion

        #region Private members

        private readonly CancellationToken _token;
        private readonly PipeStream _stream;

        #endregion

        #region IrbisServerSocket members

        /// <inheritdoc cref="IrbisServerSocket.GetRemoteAddress" />
        public override string GetRemoteAddress()
        {
            // TODO implement

            return "(unknown)";
        }

        /// <inheritdoc cref="IrbisServerSocket.ReceiveAll" />
        public override MemoryStream ReceiveAll()
        {
            MemoryStream result = new MemoryStream();

            while (true)
            {
                byte[] buffer = new byte[50 * 1024];
                int read = _stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write(buffer, 0, read);
            }

            result.Position = 0;

            return result;
        }

        /// <inheritdoc cref="IrbisServerSocket.Send" />
        public override void Send(byte[][] data)
        {
            foreach (byte[] bytes in data)
            {
                _stream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <inheritdoc cref="IrbisServerSocket.Dispose" />
        public override void Dispose()
        {
            _stream.Dispose();
        }

        #endregion
    }
}

#endif
