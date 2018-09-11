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
                [NotNull] ServerContext context
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
