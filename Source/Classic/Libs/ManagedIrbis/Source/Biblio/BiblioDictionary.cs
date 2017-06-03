﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioDictionary.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioDictionary
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("expression")]
        public string Expression
        {
            get { return _expression; }
            set { SetExpression(value); }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private PftFormatter _formatter;

        private string _expression;

        #endregion

        #region Public methods

        /// <summary>
        /// Gather terms from the <see cref="BiblioItem"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public virtual BiblioTerm[] GatherTerms
            (
                [NotNull] BiblioContext context,
                [NotNull] BiblioItem item
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(item, "item");

            List<BiblioTerm> result = new List<BiblioTerm>();

            if (ReferenceEquals(_formatter, null))
            {
                string expression = Expression;
                if (!string.IsNullOrEmpty(expression))
                {
                    PftContext pftContext = new PftContext(null);
                    pftContext.SetProvider(context.Provider);
                    _formatter = new PftFormatter(pftContext);
                    _formatter.ParseProgram(_expression);
                }
            }

            if (!ReferenceEquals(_formatter, null))
            {
                MarcRecord record = item.Record;
                if (!ReferenceEquals(record, null))
                {
                    string text = _formatter.Format(record);
                    if (!string.IsNullOrEmpty(text))
                    {
                        string[] lines = text
                            .SplitLines()
                            .TrimLines()
                            .NonEmptyLines()
                            .ToArray();

                        foreach (string line in lines)
                        {
                            BiblioTerm term = new BiblioTerm
                            {
                                Dictionary = this,
                                Data = line
                            };
                            item.Terms.Add(term);
                            result.Add(term);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Set expression.
        /// </summary>
        public virtual void SetExpression
            (
                [CanBeNull] string expression
            )
        {
            _formatter = null;
            _expression = expression;
        }

        /// <summary>
        /// Sort terms.
        /// </summary>
        public virtual IEnumerable<BiblioTerm> SortTerms
            (
                [NotNull] IEnumerable<BiblioTerm> terms
            )
        {
            Code.NotNull(terms, "terms");

            BiblioTerm[] array = terms.ToArray();
            Array.Sort(array, new TermComparer.Trivial());

            return array;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioDictionary> verifier
                = new Verifier<BiblioDictionary>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Expression, "Expression");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
