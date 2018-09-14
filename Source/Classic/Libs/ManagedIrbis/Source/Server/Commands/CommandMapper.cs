// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommandMapper.cs --
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Maps codes to command
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CommandMapper
    {
        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        [NotNull]
        public IrbisServerEngine Engine { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandMapper
            (
                [NotNull] IrbisServerEngine engine
            )
        {
            Code.NotNull(engine, "engine");

            Engine = engine;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Map the command.
        /// </summary>
        [NotNull]
        public virtual ServerCommand MapCommand
            (
                [NotNull] ClientRequest request,
                [NotNull] ServerContext context,
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(request, "request");
            Code.NotNull(context, "context");
            Code.NotNull(response, "response");

            ServerCommand result;

            if (ReferenceEquals(request.CommandCode1, null)
                || request.CommandCode1 != request.CommandCode2)
            {
                throw new IrbisException();
            }

            string commandCode = request.CommandCode1.ToUpperInvariant();

            switch (commandCode)
            {
                case "A":
                    result = new ConnectCommand(request, context, response);
                    break;

                case "B":
                    result = new DisconnectCommand(request, context, response);
                    break;

                case "C":
                    result = new ReadRecordCommand(request, context, response);
                    break;

                case "D":
                    result = new WriteRecordCommand(request, context, response);
                    break;

                case "K":
                    result = new SearchCommand(request, context, response);
                    break;

                case "L":
                    result = new ReadFileCommand(request, context, response);
                    break;

                default:
                    throw new IrbisException();
            }

            return result;
        }

        #endregion
    }
}
