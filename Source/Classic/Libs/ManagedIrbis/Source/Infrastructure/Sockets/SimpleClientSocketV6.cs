// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleClientSocketV6.cs -- client socket implementation for TCP/IP v6
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Net;
using System.Net.Sockets;

using AM.Net;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Client socket implementation for TCP/IP v6.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SimpleClientSocketV6
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleClientSocketV6
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        private IPAddress _address;

        private void _ResolveHostAddress
            (
                string host
            )
        {
            Code.NotNullNorEmpty(host, "host");

            if (ReferenceEquals(_address, null))
            {
                _address = SocketUtility.ResolveAddressIPv6(host);
                if (_address.AddressFamily
                    != AddressFamily.InterNetworkV6)
                {
                    throw new IrbisNetworkException
                        (
                            "Address must be IPv6 only!"
                        );
                }
            }
        }

        private TcpClient _GetTcpClient()
        {
            TcpClient result = new TcpClient(AddressFamily.InterNetworkV6);

            // TODO some setup

#if UAP

            System.Threading.Tasks.Task task
                = result.ConnectAsync(_address, Connection.Port);
            task.Wait();

#else

            result.Connect(_address, Connection.Port);

#endif

            return result;
        }

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest"/>
        public override void AbortRequest()
        {
            // TODO do something?
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest"/>
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            Code.NotNull(request, "request");

            IrbisConnection connection = Connection as IrbisConnection;

            if (!ReferenceEquals(connection, null))
            {
                connection.RawClientRequest = request;
            }

            _ResolveHostAddress(Connection.Host);

            using (new BusyGuard(Busy))
            {
                using (TcpClient client = _GetTcpClient())
                {
                    Socket socket = client.Client;
                    foreach (byte[] bytes in request)
                    {
                        socket.Send(bytes);
                    }
                    socket.Shutdown(SocketShutdown.Send);

                    MemoryStream stream = Connection.Executive
                        .GetMemoryStream(GetType());
                    byte[] result = socket.ReceiveToEnd(stream);
                    Connection.Executive.ReportMemoryUsage
                        (
                            GetType(),
                            result.Length
                        );

                    if (!ReferenceEquals(connection, null))
                    {
                        connection.RawServerResponse = result;
                    }

                    return result;
                }
            }
        }

        #endregion
    }
}
