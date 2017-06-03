// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithDictionary.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ChapterWithDictionary
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Dictionary.
        /// </summary>
        [NotNull]
        public BiblioDictionary Dictionary { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterWithDictionary()
        {
            Dictionary = new BiblioDictionary();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IVerifiable mebers

        /// <inheritdoc cref="IVerifiable.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ChapterWithDictionary> verifier
                = new Verifier<ChapterWithDictionary>(this, throwOnError);

            verifier
                .Assert(base.Verify(throwOnError))
                .VerifySubObject(Dictionary, "Dictionary");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
