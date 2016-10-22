/* ISuggester.cs -- interface of suggester class
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
    /// Interface of suggester class.
    /// </summary>
    public interface ISuggester
    {
        /// <summary>
        /// Collection of suggested values.
        /// </summary>
        ICollection SuggestedValues();
    }
}
