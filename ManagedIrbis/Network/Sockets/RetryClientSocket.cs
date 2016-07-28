/* RetryClientSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Sockets
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RetryClientSocket
        : AbstractClientSocket
    {
        #region Properties

        public int DelayInterval
        {
            get { return RetryManager.DelayInterval; }
            set { RetryManager.DelayInterval = value; }
        }

        [NotNull]
        public AbstractClientSocket InnerSocket
        {
            get { return _innerSocket; }
        }

        [NotNull]
        public RetryManager RetryManager
        {
            get { return _retryManager; }
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

            _innerSocket = innerSocket;
            _retryManager = retryManager;
        }

        #endregion

        #region Private members

        private readonly RetryManager _retryManager;

        private readonly AbstractClientSocket _innerSocket;

        #endregion

        #region AbstractClientSocket members

        public override void AbortRequest()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ExecuteRequest(byte[] request)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
