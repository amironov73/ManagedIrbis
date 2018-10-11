// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp4Socket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простейшее подключение через TCP/IP v4.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp4Socket
        : IrbisServerSocket
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public TcpClient Client { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public Tcp4Socket
            (
                [NotNull] TcpClient client,
                CancellationToken token
            )
        {
            Code.NotNull(client, "client");

            client.Client.SetSocketOption
                (
                    SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive,
                    true
                );
            Client = client;
            _token = token;
        }

        #endregion

        #region Private members

        private CancellationToken _token;

        #endregion

        #region IrbisServerSocket members

        /// <inheritdoc cref="IrbisServerSocket.GetRemoteAddress" />
        public override string GetRemoteAddress()
        {
            EndPoint endPoint = Client.Client.RemoteEndPoint;
            IPEndPoint ip = endPoint as IPEndPoint;
            if (!ReferenceEquals(ip, null))
            {
                return ip.Address.ToString();
            }

            return endPoint.ToString();
        }

        /// <inheritdoc cref="IrbisServerSocket.ReceiveAll" />
        public override MemoryStream ReceiveAll()
        {
            MemoryStream result = new MemoryStream();
            NetworkStream stream = Client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[50 * 1024];
                int read = stream.Read(buffer, 0, buffer.Length);
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
        public override void Send
            (
                byte[][] data
            )
        {
            Socket socket = Client.Client;

            foreach (byte[] bytes in data)
            {
                socket.Send(bytes);
            }
        }

        /// <inheritdoc cref="IrbisServerSocket.Dispose"/>
        public override void Dispose()
        {
#if UAP

            Client.Dispose();

#else

            Client.Close();

#endif
        }

        #endregion
    }
}
