// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncClientSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if (CLASSIC && (FW45 || FW46)) || NETCORE || ANDROID || UAP

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

        private byte[] _result;

        private void __Execute
            (
                byte[] request
            )
        {
            Connection.RawClientRequest = request;

            Debug.WriteLine("AsyncClientSocket: entering");

            string host = Connection.Host
                .ThrowIfNull("Connection.Host not specified");

            using (new BusyGuard(Busy))
            {
                if (ReferenceEquals(_address, null))
                {
                    try
                    {
                        Debug.WriteLine("AsyncClientSocket: before Parse");

                        _address = IPAddress.Parse(host);

                        Debug.WriteLine("AsyncClientSocket: after Parse");
                    }
                    catch
                    {
                        // Nothing to do here
                    }

                    if (ReferenceEquals(_address, null))
                    {
#if NETCORE

                        IPHostEntry ipHostEntry
                            = Dns.GetHostEntryAsync(host).Result;

#else

                        IPHostEntry ipHostEntry
                            = Dns.GetHostEntry(host);

#endif

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

                    Debug.WriteLine("AsyncClientSocket: before Connect");

#if NETCORE

                    client.ConnectAsync
                        (
                            _address,
                            Connection.Port
                        ).Wait ();

#else

                    client.Connect
                        (
                            _address,
                            Connection.Port
                        );

#endif

                    Debug.WriteLine("AsyncClientSocket: after Connectc");

                    NetworkStream stream = client.GetStream();

                    Debug.WriteLine("AsyncClientSocket: before Write");

                    stream.Write(request, 0, request.Length);

                    Debug.WriteLine("AsyncClientSocket: after Write");

                    using (MemoryStream memory = new MemoryStream())
                    {
                        byte[] buffer = new byte[32 * 1024];

                        while (true)
                        {
                            Debug.WriteLine("AsyncClientSocket: before Read");

                            int readed = stream.Read
                                (
                                    buffer, 0, buffer.Length
                                );

                            Debug.WriteLine("AsyncClientSocket: after Read");

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

                        _result = memory.ToArray();
                    }

                    Debug.WriteLine("AsyncClientSocket: exiting");

                    Connection.RawServerResponse = _result;
                }
            }
        }

        private async void _Execute
            (
                byte[] request
            )
        {
            Task task = Task.Factory.StartNew
                (
                    () => __Execute(request)
                );
            task.Wait(); // ???
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

            _result = null;
            _Execute(request);

            return _result;
        }

        #endregion
    }
}

#endif
