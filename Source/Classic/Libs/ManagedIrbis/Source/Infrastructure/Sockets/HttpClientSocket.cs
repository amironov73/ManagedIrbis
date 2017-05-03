// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HttpClientSocket.cs -- socket for Web CGI mode
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
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
    //
    // Sample Over-HTTP request
    //
    // POST /cgi-bin/irbis64r_01/WebToIrbisServer.exe HTTP/1.1
    // User-Agent: GPNTB/Irbis64
    // Host: 127.0.0.1:6666
    // Accept: *.*
    // Content-length: 72
    //                           // empty line
    // A
    // C
    // A
    // 904625
    // 1
    // password
    // username
    //                           // empty line
    //                           // empty line
    //                           // empty line
    // username
    // password
    // IRBIS_END_REQUEST
    //

    //
    // Sample ordinary request
    //
    // 53
    // A
    // C
    // A
    // 322813
    // 1
    // password
    // username
    //                           // empty line
    //                           // empty line
    //                           // empty line
    // username
    // password
    //

    /// <summary>
    /// Socket for Web CGI mode.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class HttpClientSocket
        : AbstractClientSocket
    {
        #region Constants

        /// <summary>
        /// Request boundary marker.
        /// </summary>
        public const string RequestBoundaryMarker = "IRBIS_END_REQUEST";

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public IPAddress ServerAddress { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerPort { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HttpClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] IPAddress serverAddress
            )
            : base(connection)
        {
            Code.NotNull(serverAddress, "serverAddress");

            ServerAddress = serverAddress;
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
                _address = SocketUtility.ResolveAddress(host);
            }

            if (ReferenceEquals(_address, null))
            {
                throw new IrbisNetworkException
                    (
                        "Can't resolve host " + host
                    );
            }
        }

        private TcpClient _GetTcpClient()
        {
            TcpClient result = new TcpClient();

            // TODO some setup

            result.Connect(_address, Connection.Port);

            return result;
        }

        private byte[] _TransformRequest
            (
                [NotNull] byte[] request
            )
        {
            throw new NotImplementedException();
        }

        private byte[] _TranswormAnswer
            (
                [NotNull] byte[] answer
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            Connection.RawClientRequest = request;

            _ResolveHostAddress(Connection.Host);

            using (new BusyGuard(Busy))
            {
                using (TcpClient client = _GetTcpClient())
                {
                    Socket socket = client.Client;
                    byte[] transformedRequest
                        = _TransformRequest(request);

                    socket.Send(transformedRequest);

                    byte[] answer = socket.ReceiveToEnd();
                    byte[] result = _TranswormAnswer(answer);

                    Connection.RawServerResponse = result;

                    return result;
                }
            }
        }

        #endregion
    }
}

#endif
