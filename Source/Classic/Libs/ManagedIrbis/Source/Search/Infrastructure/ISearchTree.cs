// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ISearchTree.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using ManagedIrbis.Client;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    internal interface ISearchTree
    {
        /// <summary>
        /// Children of the node.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        ISearchTree[] Children { get; }

        /// <summary>
        /// Value of the node.
        /// </summary>
        [CanBeNull]
        string Value { get; }

        /// <summary>
        /// Find records for the node.
        /// </summary>
        [NotNull]
        int[] Find([NotNull] IrbisProvider provider);
    }
}
