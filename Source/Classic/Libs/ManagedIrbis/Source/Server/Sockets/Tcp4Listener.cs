// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp4Listener.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простой слушатель для TCP/IP v4.
    /// Выдает подключение клиента <see cref="Tcp4Socket"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp4Listener
        : IrbisServerListener
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tcp4Listener
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

        #region IrbisServerListener methods

        /// <inheritdoc cref="IrbisServerListener.AcceptClientAsync"/>
        public override Task<IrbisServerSocket> AcceptClientAsync()
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
                        IrbisServerSocket socket = new Tcp4Socket(client, _token);
                        result.SetResult(socket);
                    },
                    _token
                );

            return result.Task;

#endif
        }

        /// <inheritdoc cref="IrbisServerListener.Start" />
        public override void Start()
        {
            _listener.Start();
        }

        /// <inheritdoc cref="IrbisServerListener.Stop" />
        public override void Stop()
        {
            _listener.Stop();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IrbisServerListener.Dispose" />
        public override void Dispose()
        {
            _listener.Stop();
        }

        #endregion

    }
}
