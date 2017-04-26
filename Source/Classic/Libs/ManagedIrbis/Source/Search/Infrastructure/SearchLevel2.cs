// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel2.cs --
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
    // второй оператор контекстного И; соединение двух
    // терминов таким оператором контекстного И 
    // обозначает требование поиска записей, в которых
    // оба термина присутствуют в одном и том же
    // повторении поля (или точнее – когда у терминов
    // совпадают вторые и третьи части ссылок).
    //

    /// <summary>
    /// level1 (F) level1
    /// </summary>
    sealed class SearchLevel2
        : ComplexLevel<SearchLevel1>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel2()
            : base(" (F) ")
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

            TermLink[] first = Items[0].Find(context);
            TermLink[] second = Items[1].Find(context);

            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByOccurrence();
            TermLink[] result = first.Intersect
                (
                    second,
                    comparer
                )
                .ToArray();

            return result;
        }

        #endregion
    }
}
