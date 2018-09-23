// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp6Listener.cs --
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
    /// Простой слушатель для TCP/IP v6.
    /// Выдает подключение клиента <see cref="Tcp6Socket"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp6Listener
        : IrbisServerListener
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tcp6Listener
            (
                [NotNull] IPEndPoint endPoint,
                CancellationToken token
            )
        {
            Code.NotNull(endPoint, "endPoint");

            if (!Socket.OSSupportsIPv6)
            {
                throw new IrbisException();
            }

            _listener = new TcpListener(endPoint);
            //_listener.Server.SetSocketOption
            //  (
            //      SocketOptionLevel.IPv6,
            //      SocketOptionName.IPv6Only,
            //      true
            //  );
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
        public static Tcp6Listener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.IPv6Any, portNumber);
            Tcp6Listener result = new Tcp6Listener(endPoint, token);

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
                        IrbisServerSocket socket = new Tcp6Socket(client, _token);
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
