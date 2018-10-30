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

            Listener = new TcpListener(endPoint);
            Token = token;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Cancellation token.
        /// </summary>
        protected CancellationToken Token;

        /// <summary>
        /// TCP listener.
        /// </summary>
        protected readonly TcpListener Listener;

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
#if WINMOBILE || POCKETPC

            return new Task<IrbisServerSocket> (AM.ActionUtility.NoActionFunction<IrbisServerSocket>);

#else

            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();

#if FW35 || FW40

            Task<TcpClient> task = Task<TcpClient>.Factory.FromAsync
                (
                    Listener.BeginAcceptTcpClient,
                    Listener.EndAcceptTcpClient,
                    Listener
                );

#else

            Task<TcpClient> task = Listener.AcceptTcpClientAsync();

#endif

            task.ContinueWith
                (
                    s1 =>
                    {
                        TcpClient client = s1.Result;
                        IrbisServerSocket socket = new Tcp4Socket(client, Token);
                        result.SetResult(socket);
                    },
                    Token
                );

            return result.Task;

#endif
        }

        /// <inheritdoc cref="IrbisServerListener.GetLocalAddress" />
        public override string GetLocalAddress()
        {
            return Listener.LocalEndpoint.ToString();
        }

        /// <inheritdoc cref="IrbisServerListener.Start" />
        public override void Start()
        {
            Listener.Start();
        }

        /// <inheritdoc cref="IrbisServerListener.Stop" />
        public override void Stop()
        {
            Listener.Stop();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IrbisServerListener.Dispose" />
        public override void Dispose()
        {
            Listener.Stop();
        }

        #endregion
    }
}
