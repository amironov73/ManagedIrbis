/* HtmlText.cs -- 
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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class HtmlText
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert HTML to plain text by stripping tags.
        /// </summary>
        [CanBeNull]
        public static string HtmlToPlainText
            (
                [CanBeNull] string html
            )
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            string result = Regex.Replace
                (
                    html,
                    @"<.*?>",
                    string.Empty
                );

#if FW4

            result = WebUtility.HtmlDecode(result);

#endif

            return result;
        }

        #endregion
    }
}
