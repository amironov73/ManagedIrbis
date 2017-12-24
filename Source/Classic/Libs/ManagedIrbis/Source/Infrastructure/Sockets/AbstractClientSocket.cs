// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

namespace ManagedIrbis.Infrastructure
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
        public IIrbisConnection Connection { get; internal set; }

        /// <summary>
        /// Busy state flag.
        /// </summary>
        [NotNull]
        public BusyState Busy { get; private set; }

        /// <summary>
        /// Inner socket.
        /// </summary>
        [CanBeNull]
        public AbstractClientSocket InnerSocket { get; internal set; }

        /// <summary>
        /// Requires connection?
        /// </summary>
        public virtual bool RequireConnection { get { return true; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractClientSocket
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            Busy = new BusyState(false);
        }

        #endregion

        #region Private members

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
