// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblNode.cs -- 
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

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class GblNode
    {
        #region Properties

        /// <summary>
        /// Parent node (if any).
        /// </summary>
        [CanBeNull]
        public GblNode Parent { get; internal set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Called after node execution.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                [NotNull] GblContext context
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Called before node execution.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                [NotNull] GblContext context
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the node.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
