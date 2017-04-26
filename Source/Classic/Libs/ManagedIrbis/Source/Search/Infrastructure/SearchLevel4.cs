// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel4.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
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
    // оператор логического И; соединение двух терминов
    // логическим оператором И обозначает требование
    // поиска записей, в которых присутствуют оба термина.
    //

    /// <summary>
    /// level3 * level3
    /// </summary>
    sealed class SearchLevel4
        : ComplexLevel<SearchLevel3>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel4()
            : base(" * ")
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
                = new TermLinkComparer.ByMfn();
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
