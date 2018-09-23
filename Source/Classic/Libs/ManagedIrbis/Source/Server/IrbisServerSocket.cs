// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerSocket.cs --
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
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisServerSocket
        : IDisposable
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
        public IrbisServerSocket
            (
                [NotNull] TcpClient client,
                CancellationToken token
            )
        {
            Code.NotNull(client, "client");

            Client = client;
            _token = token;
        }

        #endregion

        #region Private members

        private CancellationToken _token;

        #endregion

        #region Public methods

        /// <summary>
        /// Get remote address.
        /// </summary>
        [NotNull]
        public virtual string GetRemoteAddress()
        {
            EndPoint endPoint = Client.Client.RemoteEndPoint;
            IPEndPoint ip = endPoint as IPEndPoint;
            if (!ReferenceEquals(ip, null))
            {
                return ip.Address.ToString();
            }

            return endPoint.ToString();
        }

        #endregion

        #region IDisposable

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
#if UAP

            Client.Dispose();

#else

            Client.Close();

#endif
        }

        #endregion

        #region Object members

        #endregion
    }
}
