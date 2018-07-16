// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleClientSocket.cs -- naive client socket implementation
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Net;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Naive client socket implementation.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SimpleClientSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleClientSocket
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

            //if (ReferenceEquals(_address, null))
            //{
            //    throw new IrbisNetworkException
            //        (
            //            "Can't resolve host " + host
            //        );
            //}
        }

        private TcpClient _GetTcpClient()
        {
            TcpClient result = new TcpClient();

            // TODO some setup

#if UAP

            Task task = result.ConnectAsync(_address, Connection.Port);
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
