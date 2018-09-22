// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerListener.cs --
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
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Server.Commands;

using MoonSharp.Interpreter;

#endregion


namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisServerListener
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Listener.
        /// </summary>
        [NotNull]
        public TcpListener Listener { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisServerListener
            (
                [NotNull] IPEndPoint endPoint,
                CancellationToken token
            )
        {
            Code.NotNull(endPoint, "endPoint");

            Listener = new TcpListener(endPoint);
            _token = token;
        }

        #endregion

        #region Private members

        private CancellationToken _token;

        #endregion

        #region Public methods

        /// <summary>
        /// Accept the client.
        /// </summary>
        public Task<IrbisServerSocket> AcceptClientAsync()
        {
#if FW35 || FW40

            throw new NotImplementedException();

#else

            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();
            Task<TcpClient> task = Listener.AcceptTcpClientAsync();
            task.ContinueWith(s1 =>
                {
                    TcpClient client = s1.Result;
                    IrbisServerSocket socket
                        = new IrbisServerSocket(client, _token);
                    result.SetResult(socket);
                },
                _token);

            return result.Task;

#endif
        }

        /// <summary>
        /// Start to listen.
        /// </summary>
        public void Start()
        {
            Listener.Start();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Listener.Stop();
        }

        #endregion
    }
}
