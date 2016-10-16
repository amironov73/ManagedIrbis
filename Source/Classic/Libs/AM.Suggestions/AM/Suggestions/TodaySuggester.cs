/* TodaySuggester.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// 
    /// </summary>
    public class TodaySuggester
        : ISuggester
    {
        #region ISuggestor members

        /// <inheritdoc />
        public ICollection SuggestedValues()
        {
            return new ArrayList
                (
                    new[]
                    {
                        DateTime.Now.ToString("dd.MM.yyyy")
                    }
                );
        }

        #endregion
    }
}
