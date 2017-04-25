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
    /// <summary>
    /// Leaf node of AST.
    /// </summary>
    sealed class SearchTerm
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
