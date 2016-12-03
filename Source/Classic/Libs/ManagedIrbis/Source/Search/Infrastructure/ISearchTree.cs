// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ISearchTree.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    internal interface ISearchTree
    {
        [NotNull]
        [ItemNotNull]
        ISearchTree[] Children { get; }

        [CanBeNull]
        string Value { get; }
    }
}
