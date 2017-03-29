// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RichText.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Rich text support.
    /// </summary>
    public static class RichText
    {
        #region Constants

        

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Encode text.
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

            StringBuilder result = new StringBuilder
                (
                    text.Length
                );

            foreach (char c in text)
            {
                switch (c)
                {
                    case '{':
                        result.Append("\\{");
                        break;

                    case '}':
                        result.Append("\\}");
                        break;

                    case '\\':
                        result.Append("\\\\");
                        break;

                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
