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

        #region Public methods

        /// <summary>
        /// Create listener for the given port.
        /// </summary>
        [NotNull]
        public static Tcp4Listener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portNumber);
            Tcp4Listener result = new Tcp4Listener(endPoint, token);

            return result;
        }

        #endregion

        #region IrbisServerListener methods

        /// <inheritdoc cref="IrbisServerListener.AcceptClientAsync"/>
        public override Task<IrbisServerSocket> AcceptClientAsync()
        {
            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();

#if FW35 || FW40

            Task<TcpClient> task = Task<TcpClient>.Factory.FromAsync
                (
                    _listener.BeginAcceptTcpClient,
                    _listener.EndAcceptTcpClient,
                    _listener
                );

#else

            Task<TcpClient> task = _listener.AcceptTcpClientAsync();

#endif

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
        }

        /// <inheritdoc cref="IrbisServerListener.GetLocalAddress" />
        public override string GetLocalAddress()
        {
            return _listener.LocalEndpoint.ToString();
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
