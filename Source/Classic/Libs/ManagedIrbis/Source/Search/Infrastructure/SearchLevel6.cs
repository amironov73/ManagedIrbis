// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel6.cs --
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
            TermLink[] result = first.Union
                (
                    second,
                    comparer
                )
                .Distinct(comparer)
                .ToArray();

            return result;
        }

        #endregion
    }
}
