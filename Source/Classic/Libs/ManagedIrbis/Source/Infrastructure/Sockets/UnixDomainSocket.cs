// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UnixDomainSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE_VNEXT

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

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UnixDomainSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Socket name.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnixDomainSocket
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            UnixDomainSocketEndPoint endPoint = new UnixDomainSocketEndPoint(Name);
            using (Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream,
                ProtocolType.Unspecified))
            {
                socket.Connect(endPoint);
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

                return result;
            }
        }

        #endregion
    }
}

#endif
