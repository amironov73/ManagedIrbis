/* PftDebugger.cs -- 
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

namespace ManagedIrbis.Pft.Infrastructure.Diagnostics
{
    /// <summary>
    /// Debugger for PFT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PftDebugger
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftDebugger
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Activate the debugger.
        /// </summary>
        public virtual void Activate
            (
                PftDebugEventArgs eventArgs
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Cancel script execution.
        /// </summary>
        public virtual void CancelExecution
            (
                PftDebugEventArgs eventArgs
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Deactivate the debugger.
        /// </summary>
        public virtual void Deactivate
            (
                PftDebugEventArgs eventArgs
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Goto next step.
        /// </summary>
        public virtual void NextStep
            (
                PftDebugEventArgs eventArgs
            )
        {
            // Nothing to do here
        }

        #endregion
    }
}
