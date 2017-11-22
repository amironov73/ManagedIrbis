// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyProvider.cs -- base morphology provider
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// Base morphology provider.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MorphologyProvider
    {
        #region Public methods

        /// <summary>
        /// Flatten the query.
        /// </summary>
        [NotNull]
        public string[] Flatten
            (
                [NotNull] string word,
                [NotNull] MorphologyEntry[] entries
            )
        {
            Code.NotNullNorEmpty(word, "word");
            Code.NotNull(entries, "entries");

            List<string> result = new List<string>
            {
                word.ToUpper()
            };

            foreach (MorphologyEntry entry in entries)
            {
                string entryMainTerm = entry.MainTerm
                    .ThrowIfNull("entry.MainTerm");
                string[] entryForms = entry.Forms
                    .ThrowIfNull("entry.Forms");

                result.Add(entryMainTerm.ToUpper());
                result.AddRange(entryForms.Select(w => w.ToUpper()));
            }

            return result
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Find the word in the morphology database.
        /// </summary>
        [NotNull]
        public virtual MorphologyEntry[] FindWord
            (
                [NotNull] string word
            )
        {
            return new MorphologyEntry[0];
        }

        /// <summary>
        /// Rewrite the query using morphology.
        /// </summary>
        [NotNull]
        public virtual string RewriteQuery
            (
                [NotNull] string queryExpression
            )
        {
            return queryExpression;
        }

        #endregion
    }
}
