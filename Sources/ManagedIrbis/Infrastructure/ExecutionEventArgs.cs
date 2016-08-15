/* ExecutionEvenArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Event arguments.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExecutionEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public ExecutionContext Context { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutionEventArgs
            (
                [NotNull] ExecutionContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
        }

        #endregion
    }
}
