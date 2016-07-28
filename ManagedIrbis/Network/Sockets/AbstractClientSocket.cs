/* AbstractClientSocket.cs -- 
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
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Threading;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Busy state flag.
        /// </summary>
        [NotNull]
        public BusyState Busy
        {
            get { return _busy; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractClientSocket
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            _busy = new BusyState(false);
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        private readonly BusyState _busy;

        #endregion

        #region Public methods

        /// <summary>
        /// Abort the request.
        /// </summary>
        public abstract void AbortRequest();

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        /// <exception cref="IrbisNetworkException"></exception>
        [NotNull]
        public abstract byte[] ExecuteRequest
            (
                [NotNull] byte[] request
            );

        #endregion
    }
}
