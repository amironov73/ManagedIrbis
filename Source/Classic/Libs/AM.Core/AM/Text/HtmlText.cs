// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
        /// Encode html entities.
        /// </summary>
        [CanBeNull]
        public static string Encode
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                switch (c)
                {
                    case '"':
                        result.Append("&quot;");
                        break;

                    case '#':
                        result.Append("&num;");
                        break;

                    case '&':
                        result.Append("&amp;");
                        break;

                    case '\'':
                        result.Append("&apos;");
                        break;

                    case '<':
                        result.Append("&lt;");
                        break;

                    case '>':
                        result.Append("&gt;");
                        break;

                    case '\x00A0':
                        // non-breaking space
                        result.Append("&nbsp;");
                        break;

                    case '\x00A2':
                        // cent sign
                        result.Append("&cent;");
                        break;

                    case '\x00A3':
                        // pound sign
                        result.Append("&pound;");
                        break;

                    case '\x00A5':
                        // yen sign
                        result.Append("&yen;");
                        break;

                    case '\x00A7':
                        // section sign
                        result.Append("&sect;");
                        break;

                    case '\x00A9':
                        // copyright sign
                        result.Append("&copy;");
                        break;

                    case '\x00AD':
                        // soft hyphen
                        result.Append("&shy;");
                        break;

                    case '\x00AE':
                        // registered sign
                        result.Append("&reg;");
                        break;

                    case '\x20AC':
                        // euro sign
                        result.Append("&euro;");
                        break;

                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

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
