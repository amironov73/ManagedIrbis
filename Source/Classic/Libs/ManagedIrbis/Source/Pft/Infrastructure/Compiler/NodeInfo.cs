// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class NodeInfo
    {
        #region Properties

        /// <summary>
        /// Node.
        /// </summary>
        [NotNull]
        public PftNode Node { get; private set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Ready?
        /// </summary>
        public bool Ready { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeInfo
            (
                int id,
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            Id = id;
            Node = node;
        }

        #endregion

        #region Object members

        private bool Equals
            (
                NodeInfo other
            )
        {
            return Id == other.Id;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is NodeInfo && Equals((NodeInfo) obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}
