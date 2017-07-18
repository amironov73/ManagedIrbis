// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVisitor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Walking
{
    /// <summary>
    /// Context for AST visitor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class VisitorContext
    {
        #region Properties

        /// <summary>
        /// Node.
        /// </summary>
        [NotNull]
        public PftNode Node { get; private set; }

        /// <summary>
        /// Result. <c>true</c> means "continue" (default value).
        /// </summary>
        public bool Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public VisitorContext
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            Node = node;
            Result = true;
        }

        #endregion
    }
}
