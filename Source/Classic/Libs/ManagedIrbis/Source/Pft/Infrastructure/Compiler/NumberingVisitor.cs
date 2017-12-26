// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberingVisitor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Walking;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class NumberingVisitor
        : PftVisitor
    {
        #region Properties

        [NotNull]
        public NodeDictionary Dictionary { get; private set; }

        public int LastId { get; private set; }

        #endregion

        #region Construction

        public NumberingVisitor
            (
                [NotNull] NodeDictionary dictionary,
                int start
            )
        {
            Dictionary = dictionary;
            LastId = start;
        }

        #endregion

        #region PftVisitor members

        /// <inheritdoc cref="PftVisitor.VisitNode" />
        public override bool VisitNode
            (
                PftNode node
            )
        {
            int id = ++LastId;
            NodeInfo info = new NodeInfo(id, node);
            Dictionary.Add(info);

            return true;
        }

        #endregion
    }
}
