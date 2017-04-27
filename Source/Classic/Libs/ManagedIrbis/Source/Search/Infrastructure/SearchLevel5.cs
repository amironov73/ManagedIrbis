// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel5.cs --
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
    // оператор логического НЕ; соединение двух терминов
    // логическим оператором НЕ обозначает требование
    // поиска записей, в которых присутствует первый термин
    // и отсутствует второй; оператор НЕ не может быть
    // одноместным (т.е. данному оператору, как и всем
    // другим, должен ОБЯЗАТЕЛЬНО предшествовать термин).
    //

    /// <summary>
    /// level4 ^ level4
    /// </summary>
    sealed class SearchLevel5
        : ComplexLevel<SearchLevel4>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel5()
            : base(" ^ ")
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
                = new TermLinkComparer.ByMfn();
            for (int i = 1; i < Items.Count; i++)
            {
                if (result.Length == 0)
                {
                    return result;
                }
                result = result.Except
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
