// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RetryClientSocket.cs -- retry on network error.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Retry on network error.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RetryClientSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Delay between sequent attempts.
        /// </summary>
        public int DelayInterval
        {
            get { return RetryManager.DelayInterval; }
            set { RetryManager.DelayInterval = value; }
        }

        /// <summary>
        /// Working socket.
        /// </summary>
        [NotNull]
        public AbstractClientSocket InnerSocket
        {
            get; private set;
        }

        /// <summary>
        /// Retry manager.
        /// </summary>
        [NotNull]
        public RetryManager RetryManager
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetryClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket,
                [NotNull] RetryManager retryManager
            )
            : base(connection)
        {
            Code.NotNull(innerSocket, "innerSocket");
            Code.NotNull(retryManager, "retryManager");

            InnerSocket = innerSocket;
            RetryManager = retryManager;
        }

        #endregion

        #region Private members

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
