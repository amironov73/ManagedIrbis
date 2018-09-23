// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerListener.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
    public class IrbisServerListener
        : IDisposable
    {
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

            _listener = new TcpListener(endPoint);
            _token = token;
        }

        #endregion

        #region Private members

        private CancellationToken _token;

        private readonly TcpListener _listener;

        #endregion

        #region Public methods

        /// <summary>
        /// Accept the client.
        /// </summary>
        public virtual Task<IrbisServerSocket> AcceptClientAsync()
        {
#if FW35 || FW40

            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();
            Task<TcpClient> task = Task<TcpClient>.Factory.FromAsync
                (
                    Listener.BeginAcceptTcpClient,
                    Listener.EndAcceptTcpClient,
                    Listener
                );
            task.ContinueWith
                (
                    s1 =>
                    {
                        TcpClient client = s1.Result;
                        IrbisServerSocket socket
                            = new IrbisServerSocket(client, _token);
                        result.SetResult(socket);
                    },
                    _token
                );

            return result.Task;

#else

            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();
            Task<TcpClient> task = _listener.AcceptTcpClientAsync();
            task.ContinueWith
                (
                    s1 =>
                    {
                        TcpClient client = s1.Result;
                        IrbisServerSocket socket
                            = new IrbisServerSocket(client, _token);
                        result.SetResult(socket);
                    },
                    _token
                );

            return result.Task;

#endif
        }

        /// <summary>
        /// Start to listen.
        /// </summary>
        public virtual void Start()
        {
            _listener.Start();
        }

        /// <summary>
        /// Stop to listen.
        /// </summary>
        public virtual void Stop()
        {
            _listener.Stop();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            _listener.Stop();
        }

        #endregion
    }
}
