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
        /// Data.
        /// </summary>
        [NotNull]
        public WorkData Data { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ServerCommand
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            Data = data;
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
