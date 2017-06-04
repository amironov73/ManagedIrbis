// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

        #endregion

        #region Construction

        #endregion

        #region Private members

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

            Log.Warn
                (
                    "BiblioDictionary::GatherTerms: "
                    + "not overridden"
                );

            return new BiblioTerm[0];
        }

        /// <summary>
        /// Sort terms.
        /// </summary>
        [NotNull]
        public virtual IEnumerable<BiblioTerm> SortTerms
            (
                [NotNull] IEnumerable<BiblioTerm> terms
            )
        {
            Code.NotNull(terms, "terms");

            Log.Warn
                (
                    "BiblioDictionary::SortTerms: "
                    + "not overridden"
                );

            return terms;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioDictionary> verifier
                = new Verifier<BiblioDictionary>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
