﻿/* SimpleClientSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
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

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SimpleClientSocket
        : IrbisClientSocket
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

            if (_address == null)
            {
                //try
                //{
                    _address = IPAddress.Parse(Connection.Host);
                //}
                //catch
                //{
                //    // Not supported in .NET Core
                //    IPHostEntry ipHostEntry = Dns.GetHostEntry(Connection.Host);
                //    _address = ipHostEntry.AddressList[0];
                //}
            }

            if (_address == null)
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

#if FW35

            // Not supported in .NET Core
            result.Connect(_address, Connection.Port);

#endif

#if FW45

            Task task = result.ConnectAsync(_address, Connection.Port);
            task.Wait();

#else

            // FW40 doesn't contain ConnectAsync
            result.Connect(_address, Connection.Port);

#endif

            return result;
        }

        #endregion

        #region IrbisClientSocket members

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            _ResolveHostAddress(Connection.Host);

            using (TcpClient client = _GetTcpClient())
            {
                NetworkStream stream = client.GetStream();

                stream.Write
                    (
                        request,
                        0,
                        request.Length
                    );

                byte[] result = stream.ReadToEnd();

                return result;
            }
        }

        #endregion
    }
}
