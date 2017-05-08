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

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree Parent { get; set; }

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

        /// <summary>
        /// Children nodes.
        /// </summary>
        public ISearchTree[] Children
        {
            get { return new ISearchTree[0]; }
        }

        /// <summary>
        /// Value of the term.
        /// </summary>
        public string Value { get { return Term; } }

        /// <summary>
        /// Find the term.
        /// </summary>
        public TermLink[] Find
            (
                SearchContext context
            )
        {
            Code.NotNull(context, "context");

            IrbisProvider provider = context.Provider;
            TermLink[] result;

            switch (Tail)
            {
                case null:
                case "":
                    result = provider.ExactSearchLinks(Term);
                    break;

                case "$":
                    result = provider.ExactSearchTrimLinks(Term, 1000);
                    break;

                case "@":
                    throw new NotImplementedException();

                default:
                    throw new IrbisException("Unexpected tail");
            }

            // TODO implement context filtering

            return result;
        }

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree toChild
            )
        {
            throw new NotImplementedException();
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
