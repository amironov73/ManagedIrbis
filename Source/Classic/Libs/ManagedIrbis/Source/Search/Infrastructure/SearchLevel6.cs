// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel6.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using CodeJam;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    //
    // оператор логического ИЛИ; соединение двух операндов
    // (терминов) логическим оператором ИЛИ обозначает 
    // требование поиска записей, в которых присутствует
    // хотя бы один из терминов.
    //

    /// <summary>
    /// level5 + level5
    /// </summary>
    sealed class SearchLevel6
        : ComplexLevel<SearchLevel5>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel6()
            : base(" + ")
        {
        }

        #endregion

        #region ISearchTree member

        /// <inheritdoc cref="ComplexLevel{T}.Find"/>
        public override TermLink[] Find
            (
                SearchContext context
            )
        {
            Code.NotNull(context, "context");

            TermLink[] result = Items[0].Find(context);
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByMfn();
            for (int i = 1; i < Items.Count; i++)
            {
                result = result.Union
                    (
                        Items[i].Find(context),
                        comparer
                    )
                    .ToArray();
            }
            result = result.Distinct(comparer).ToArray();

            return result;
        }

        #endregion
    }
}
