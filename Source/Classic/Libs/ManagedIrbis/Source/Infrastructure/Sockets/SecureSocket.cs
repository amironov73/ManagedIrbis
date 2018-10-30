// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SecureSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using AM.Net;
using AM.Security;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// SSL/TLS over TCP/IP v4
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SecureSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecureSocket
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
                _address = SocketUtility.ResolveAddressIPv4(host);
                if (_address.AddressFamily
                    != AddressFamily.InterNetwork)
                {
                    throw new IrbisNetworkException
                        (
                            "Address must be IPv4 only!"
                        );
                }
            }
        }

        private TcpClient _GetTcpClient()
        {
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

#if UAP

            Task task = result.ConnectAsync(_address, Connection.Port);
            task.Wait();

#else

            result.Connect(_address, Connection.Port);

#endif

            return result;
        }

        private bool _ValidateServerCertificate
            (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            return false;
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
                    X509Certificate certificate
                        = SecurityUtility.GetSslCertificate();
                    X509CertificateCollection collection
                        = new X509CertificateCollection { certificate };
                    SslStream sslStream = new SslStream
                        (
                            client.GetStream(),
                            false,
                            _ValidateServerCertificate,
                            null
                        );
                    sslStream.AuthenticateAsClient
                        (
                            "ArsMagnaSslSocket",
                            collection,
                            SslProtocols.Tls12,
                            false
                        );

                    Socket socket = client.Client;
                    foreach (byte[] bytes in request)
                    {
                        socket.Send(bytes);
                    }
                    socket.Shutdown(SocketShutdown.Send);

                    MemoryStream memory = Connection.Executive
                        .GetMemoryStream(GetType());
                    byte[] result = socket.ReceiveToEnd(memory);
                    Connection.Executive.ReportMemoryUsage
                        (
                            GetType(),
                            result.Length
                        );
                    Connection.Executive.RecycleMemoryStream(memory);

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