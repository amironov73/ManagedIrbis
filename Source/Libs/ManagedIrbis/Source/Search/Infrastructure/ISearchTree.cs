/* SearchLevel1.cs --
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
