// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyProvider.cs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedIrbis.Morphology
{
#if NOTDEF

    public class MorphologyProvider
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string[] Flatten
            (
                string word,
                MorphologyEntry[] entries
            )
        {
            List<string> result = new List<string>
            {
                word.ToUpper()
            };

            foreach (MorphologyEntry entry in entries)
            {
                result.Add(entry.MainTerm.ToUpper());
                result.AddRange(entry.Forms.Select(w => w.ToUpper()));
            }

            return result
                .Distinct()
                .ToArray();
        }

        public virtual MorphologyEntry[] FindWord
            (
                string word
            )
        {
            return new MorphologyEntry[0];
        }

        public virtual string RewriteQuery
            (
                string queryExpression
            )
        {
            return queryExpression;
        }

        #endregion
    }

#endif
}
