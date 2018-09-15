// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WorkData.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Threading.Tasks;
using JetBrains.Annotations;

using ManagedIrbis.Server.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// All the data for client request handling cycle.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WorkData
    {
        #region Properties

        /// <summary>
        /// Command.
        /// </summary>
        public ServerCommand Command { get; set; }

        /// <summary>
        /// Context.
        /// </summary>
        public ServerContext Context { get; set; }

        /// <summary>
        /// Engine
        /// </summary>
        public IrbisServerEngine Engine { get; set; }

        /// <summary>
        /// Response.
        /// </summary>
        public ServerResponse Response { get; set; }

        /// <summary>
        /// Request.
        /// </summary>
        public ClientRequest Request { get; set; }

        /// <summary>
        /// Socket.
        /// </summary>
        public IrbisServerSocket Socket { get; set; }

        /// <summary>
        /// Task.
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Worker.
        /// </summary>
        public ServerWorker Worker { get; set; }

        #endregion
    }
}
