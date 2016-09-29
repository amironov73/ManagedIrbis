/* SearchQueryUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SearchQueryUtility
    {
        #region Public methods

        /// <summary>
        /// Require syntax element.
        /// </summary>
        public static string RequireSyntax
            (
                [CanBeNull] this string element,
                [NotNull] string message
            )
        {
            if (ReferenceEquals(element, null))
            {
                throw new SearchSyntaxException(message);
            }

            return element;
        }

        #endregion
    }
}
