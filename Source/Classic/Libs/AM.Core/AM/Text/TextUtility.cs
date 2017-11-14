// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public static class TextUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Determine kind of the text.
        /// </summary>
        public static TextKind DetermineTextKind
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return TextKind.PlainText;
            }

            if (text.StartsWith("{")
                || text.EndsWith("}"))
            {
                return TextKind.RichText;
            }

            if (text.StartsWith("<")
                || text.EndsWith(">"))
            {
                return TextKind.Html;
            }

            bool curly = text.Contains("{")
                && text.Contains("}");
            bool angle = text.Contains("<")
                && text.Contains(">");

            if (curly && !angle)
            {
                return TextKind.RichText;
            }
            if (angle && !curly)
            {
                return TextKind.Html;
            }

            return TextKind.PlainText;
        }

        #endregion
    }
}
