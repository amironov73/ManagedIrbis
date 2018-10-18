// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* KeepAliveSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if DESKTOP

#region Using directives

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using AM.Net;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class KeepAliveSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public KeepAliveSocket
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        private IPAddress _address;
        private TcpClient _client;

        private void _ResolveHostAddress
            (
                string host
            )
        {
            Code.NotNullNorEmpty(host, "host");

            if (ReferenceEquals(_address, null))
            {
                _address = SocketUtility.ResolveAddressIPv4(host);
            }
        }

        private TcpClient _GetTcpClient()
        {
            if (!ReferenceEquals(_client, null))
            {
                return _client;
            }

            TcpClient result = new TcpClient();

            // TODO some setup
            result.Client.SetSocketOption
                (
                    SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive,
                    true
                );
            result.NoDelay = true;
            result.LingerState = new LingerOption(false, 0);

            result.Connect(_address, Connection.Port);
            _client = result;

            return result;
        }

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest"/>
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            _ResolveHostAddress(Connection.Host);

            using (new BusyGuard(Busy))
            {
                TcpClient client = _GetTcpClient();
                Socket socket = client.Client;
                foreach (byte[] bytes in request)
                {
                    socket.Send(bytes);
                }

                MemoryStream stream = Connection.Executive
                    .GetMemoryStream(GetType());
                byte[] result = socket.ReceiveToEnd(stream);
                Connection.Executive.ReportMemoryUsage
                    (
                        GetType(),
                        result.Length
                    );
                Connection.Executive.RecycleMemoryStream(stream);

                return result;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public override void Dispose()
        {
            base.Dispose();
            if (!ReferenceEquals(_client, null))
            {
                ((IDisposable)_client).Dispose();
                _client = null;
            }
        }

        #endregion
    }
}

#endif
