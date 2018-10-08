// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RetryClientSocket.cs -- retry on network error.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Source.Infrastructure.Sockets
{
    /// <summary>
    /// Multiprotocol socket.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MutliprotocolSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Socket list.
        /// </summary>
        [NotNull]
        public List<AbstractClientSocket> InnerSockets { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MutliprotocolSocket
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            InnerSockets = new List<AbstractClientSocket>();
        }

        #endregion

        #region Private members

        private AbstractClientSocket _choosenSocket;

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            if (!ReferenceEquals(_choosenSocket, null))
            {
                return _choosenSocket.ExecuteRequest(request);
            }

            byte[] result = null;
            foreach (AbstractClientSocket socket in InnerSockets)
            {
                try
                {
                    result = socket.ExecuteRequest(request);
                    _choosenSocket = socket;
                    break;
                }
                catch (Exception exception)
                {
                    Log.TraceException("MultiprotocolSocket::ExecuteRequest", exception);
                }
            }

            if (ReferenceEquals(_choosenSocket, null)
                || ReferenceEquals(result, null))
            {
                throw new IrbisException();
            }

            return result;
        }

        #endregion

    }
}