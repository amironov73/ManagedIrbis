/* PftProgram.cs --
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
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// AST root
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftProgram
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedures.
        /// </summary>
        [NotNull]
        public PftProcedureManager Procedures { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProgram()
        {
            Procedures = new PftProcedureManager();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc/>
        public override void Execute
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            try
            {
                base.Execute(context);
            }
            catch (PftBreakException)
            {
                if (!ReferenceEquals(context.Parent, null))
                {
                    throw;
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
