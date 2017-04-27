// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    //
    // первый оператор контекстного И; соединение двух
    // терминов таким оператором контекстного И обозначает
    // требование поиска записей, в которых оба термина
    // присутствуют в одном и том же поле
    // (или точнее – когда у терминов совпадают вторые
    // части ссылок).
    //

    /// <summary>
    /// level2 (G) level2
    /// </summary>
    sealed class SearchLevel3
        : ComplexLevel<SearchLevel2>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel3()
            : base(" (G) ")
        {
        }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ISearchTree.Find"/>
        public override TermLink[] Find
            (
                SearchContext context
            )
        {
            Code.NotNull(context, "context");

            TermLink[] result = Items[0].Find(context);
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByTag();
            for (int i = 1; i < Items.Count; i++)
            {
                if (result.Length == 0)
                {
                    return result;
                }
                result = result.Intersect
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
