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

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Walking
{
    /// <summary>
    /// Abstract AST visitor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PftVisitor
    {
        #region Public methods

        /// <summary>
        /// Visit the node.
        /// </summary>
        /// <returns>
        /// <c>true</c> means "continue".
        /// </returns>
        public abstract bool VisitNode
            (
                [NotNull] PftNode node
            );

        #endregion
    }
}
