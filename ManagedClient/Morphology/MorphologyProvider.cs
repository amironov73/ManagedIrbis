/* MorphologyProvider.cs
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Morphology
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
