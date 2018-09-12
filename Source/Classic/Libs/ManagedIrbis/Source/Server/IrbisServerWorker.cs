// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerWorker.cs --
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
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisServerWorker
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        public ServerContext Context { get; set; }

        /// <summary>
        /// Server.
        /// </summary>
        [NotNull]
        public IrbisServerEngine Engine { get; private set; }

        /// <summary>
        /// Socket.
        /// </summary>
        [NotNull]
        public IrbisServerSocket Socket { get; private set; }

        /// <summary>
        /// Client request.
        /// </summary>
        public ClientRequest Request { get; private set; }

        /// <summary>
        /// Task.
        /// </summary>
        [NotNull]
        public Task Task { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisServerWorker
            (
                [NotNull] IrbisServerEngine engine,
                [NotNull] IrbisServerSocket socket
            )
        {
            Code.NotNull(engine, "engine");
            Code.NotNull(socket, "socket");

            Engine = engine;
            Socket = socket;

            Task = new Task(DoWork);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Do the work.
        /// </summary>
        public void DoWork()
        {
            Request = new ClientRequest(Socket.Client);

            // TODO dispose the Socket
        }

        #endregion

        #region Object members

        #endregion
    }
}
