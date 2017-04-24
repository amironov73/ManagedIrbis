// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncClientSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if (CLASSIC && FW45) || NETCORE || ANDROID || UAP

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Async version of <see cref="SimpleClientSocket"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AsyncClientSocket
        : AbstractClientSocket
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsyncClientSocket
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        private IPAddress _address;

        private async Task<byte[]> _Execute
            (
                byte[] request
            )
        {
            string host = Connection.Host
                .ThrowIfNull("Connection.Host not specified");

            if (ReferenceEquals(_address, null))
            {
                try
                {
                    _address = IPAddress.Parse(host);
                }
                catch
                {
                    // Nothing to do here
                }

                if (ReferenceEquals(_address, null))
                {
                    IPHostEntry ipHostEntry
                        = await Dns.GetHostEntryAsync(host);
                    if (!ReferenceEquals(ipHostEntry, null)
                        && !ReferenceEquals
                        (
                            ipHostEntry.AddressList,
                            null
                        )
                        && ipHostEntry.AddressList.Length != 0)
                    {
                        _address = ipHostEntry.AddressList[0];
                    }
                }

                if (ReferenceEquals(_address, null))
                {
                    throw new IrbisNetworkException
                    (
                        "Can't resolve host " + host
                    );
                }
            }

            using (TcpClient client = new TcpClient())
            {
                // TODO some setup?

                await client.ConnectAsync
                    (
                        _address,
                        Connection.Port
                    );

                NetworkStream stream = client.GetStream();

                await stream.WriteAsync(request, 0, request.Length);

                byte[] result;

                using (MemoryStream memory = new MemoryStream())
                {
                    byte[] buffer = new byte[32 * 1024];

                    while (true)
                    {
                        int readed = await stream.ReadAsync
                        (
                            buffer, 0, buffer.Length
                        );

                        if (readed < 0)
                        {
                            throw new ArsMagnaException
                            (
                                "Socket reading error"
                            );
                        }

                        if (readed == 0)
                        {
                            break;
                        }

                        memory.Write(buffer, 0, readed);
                    }

                    result = memory.ToArray();
                }

                Connection.RawServerResponse = result;

                return result;
            }
        }

#endregion

#region Public methods

#endregion

#region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            // TODO do something?
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

            using (new BusyGuard(Busy))
            {
                byte[] result = _Execute(request).Result;

                return result;
            }

        }

#endregion
    }
}

#endif
