// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfNode.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Ibf.Infrastructure
{
    //
    // http://sntnarciss.ru/irbis/spravka/wa0101104100.htm
    // http://sntnarciss.ru/irbis/spravka/wa0203050100.htm
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IbfNode
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Called after node execution.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                [NotNull] IbfContext context
            )
        {
            // Nothing to do here yet
        }

        /// <summary>
        /// Called before node execution.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                [NotNull] IbfContext context
            )
        {
            // Nothing to do here yet
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the command.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] IbfContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here yet

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
