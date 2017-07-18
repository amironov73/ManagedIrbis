// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniversalVisitor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

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
    public sealed class UniversalVisitor
        : PftVisitor
    {
        #region Events

        /// <summary>
        /// Visit the node.
        /// </summary>
        public event Action<VisitorContext> Visitor;

        #endregion

        #region PftVisitor members

        /// <inheritdoc cref="PftVisitor.VisitNode" />
        public override bool VisitNode
            (
                PftNode node
            )
        {
            Code.NotNull(node, "node");

            Action<VisitorContext> visitor = Visitor;
            if (!ReferenceEquals(visitor, null))
            {
                VisitorContext context = new VisitorContext(node);
                visitor(context);

                return context.Result;
            }

            return false;
        }

        #endregion
    }
}
