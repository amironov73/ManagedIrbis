// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerCommand.cs --
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

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class ServerCommand
    {
        #region Properties

        /// <summary>
        /// Код команды.
        /// </summary>
        [NotNull]
        public abstract string CommandCode { get; }

        /// <summary>
        /// Client request.
        /// </summary>
        [NotNull]
        public ClientRequest Request { get; set; }

        /// <summary>
        /// Context.
        /// </summary>
        public ServerContext Context { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ServerCommand
            (
                [NotNull] ClientRequest request,
                [NotNull] ServerContext context
            )
        {
            Code.NotNull(request, "request");
            Code.NotNull(context, "context");

            Request = request;
            Context = context;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the command.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] ClientQuery query
            )
        {
            // Nothing to do here?
        }

        #endregion

        #region Object members

        #endregion
    }
}
