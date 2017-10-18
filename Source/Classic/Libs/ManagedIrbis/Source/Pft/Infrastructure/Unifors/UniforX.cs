// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforX.cs -- 
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Удаление из заданной строки фрагментов,
    // выделенных угловыми скобками<> – &uf('X…
    // Вид функции: X.
    // Назначение: Удаление из заданной строки фрагментов,
    // выделенных угловыми скобками<>.
    // Формат (передаваемая строка):
    // X<строка>
    // Примеры:
    // &unifor("X"v200)
    //

    static class UniforX
    {
        #region Public methods

        /// <summary>
        /// Remove text surrounded with angle brackets.
        /// </summary>
        public static void RemoveAngleBrackets
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string result = expression;

                if (expression.Contains("<"))
                {
                    StringBuilder builder = new StringBuilder(expression.Length);
                    TextNavigator navigator = new TextNavigator(expression);
                    while (!navigator.IsEOF)
                    {
                        string text = navigator.ReadUntil('<');
                        builder.Append(text);
                        char c = navigator.ReadChar();
                        if (c != '<')
                        {
                            break;
                        }
                        text = navigator.ReadUntil('>');
                        c = navigator.ReadChar();
                        if (c != '>')
                        {
                            builder.Append('<');
                            builder.Append(text);
                        }
                    }
                    builder.Append(navigator.GetRemainingText());
                    result = builder.ToString();
                }

                //string clear = Regex.Replace(expression, "<.*?>", string.Empty);

                context.Write(node, result);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
