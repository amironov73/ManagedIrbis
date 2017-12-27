// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVisitor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
        /// Visitor.
        /// </summary>
        [NotNull]
        public PftVisitor Visitor { get; private set; }

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
                [NotNull] PftVisitor visitor,
                [NotNull] PftNode node
            )
        {
            Code.NotNull(visitor, "visitor");
            Code.NotNull(node, "node");

            Visitor = visitor;
            Node = node;
            Result = true;
        }

        #endregion
    }
}
