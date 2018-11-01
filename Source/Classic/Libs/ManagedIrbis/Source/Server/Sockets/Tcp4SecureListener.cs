// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp4SecureListener.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

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
    /// Слушатель для TCP/IP v4 с поддержкой SSL/TLS.
    /// Выдает подключение клиента <see cref="Tcp4SecureSocket"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp4SecureListener
        : Tcp4Listener
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tcp4SecureListener
            (
                [NotNull] IPEndPoint endPoint,
                CancellationToken token
            )
            : base(endPoint, token)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create listener for the given port.
        /// </summary>
        [NotNull]
        public new static Tcp4SecureListener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portNumber);
            Tcp4SecureListener result = new Tcp4SecureListener(endPoint, token);

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
                        IrbisServerSocket socket = new Tcp4SecureSocket(client, Token);
                        result.SetResult(socket);
                    },
                    Token
                );

            return result.Task;

#endif
        }

        #endregion
    }
}

#endif
