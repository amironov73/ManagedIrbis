// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchTerm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    //
    // В общем виде операнд поискового выражения можно
    // представить следующим образом:
    //
    // “<префикс><термин>$”/(tag1,tag2,…tagN)
    //
    // где:
    //
    // <префикс> - префикс, определяющий вид
    // термина(вид словаря);
    // <термин> - собственно термин словаря;
    // $ - признак правого усечения термина;
    // определяет совокупность терминов, имеющих
    // начальную последовательность символов,
    // совпадающую с указанным термином;
    //может отсутствовать – в этом случае поиск
    // идет по точному значению указанного термина.
    // “ – символ-ограничитель термина (двойные кавычки);
    // должен использоваться обязательно, если термин
    // включает в себя символы пробел, круглые скобки,
    // решетка (#), а также символы, совпадающие
    // с обозначениями логических операторов;
    // / (tag1, tag2,…tagN) – конструкция квалификации
    // термина; определяет метки поля, в которых должен
    // находиться указанный термин, или точнее – вторую
    // часть ссылки термина
    // (Приложение  5. ТАБЛИЦЫ ВЫБОРА ПОЛЕЙ (ТВП));
    // может отсутствовать – что означает отсутствие
    // дополнительных требований в части меток полей.
    //

    /// <summary>
    /// Leaf node of AST.
    /// </summary>
    public sealed class SearchTerm
        : ISearchTree
    {
        #region Properties

        /// <summary>
        /// K=keyword
        /// </summary>
        [CanBeNull]
        public string Term { get; set; }

        /// <summary>
        /// $ or @
        /// </summary>
        [CanBeNull]
        public string Tail { get; set; }

        /// <summary>
        /// /(tag,tag,tag)
        /// </summary>
        [CanBeNull]
        public string[] Context { get; set; }

        #endregion

        #region ISearchTree members

        public ISearchTree[] Children
        {
            get { return new ISearchTree[0]; }
        }

        public string Value { get { return Term; } }

        public TermLink[] Find
            (
                SearchContext context
            )
        {
            return new TermLink[0];
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append('"');
            result.Append(Term);
            if (!string.IsNullOrEmpty(Tail))
            {
                result.Append(Tail);
            }
            result.Append('"');
            if (!ReferenceEquals(Context, null))
            {
                result.Append("/(");
                result.Append
                    (
                        string.Join
                        (
                            ",",
                            Context
                        )
                    );
                result.Append(')');
            }

            return result.ToString();
        }

        #endregion
    }
}
