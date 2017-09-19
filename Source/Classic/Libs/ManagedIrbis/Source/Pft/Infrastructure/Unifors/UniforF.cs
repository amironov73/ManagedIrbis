// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforF.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть часть строки, начиная со следующего слова
    // после указанного и до конца строки – &uf('F…
    // Вид функции: F.
    // Назначение: Вернуть часть строки, начиная со следующего
    // слова после указанного и до конца строки.
    // Формат (передаваемая строка):
    // FN<строка>
    // где N – количество слов (одна цифра).
    // Примеры:
    // &unifor("F3"v200^a)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class UniforF
    {
        #region Public methods

        /// <summary>
        /// Часть строки после N первых слов.
        /// </summary>
        public static void GetLastWords
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!String.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string countText = navigator.ReadInteger();
                if (!String.IsNullOrEmpty(countText))
                {
                    int wordCount;
                    if (NumericUtility.TryParseInt32(countText, out wordCount))
                    {
                        string text = navigator.GetRemainingText();
                        string output = UniforE.GetLastWords(text, wordCount);
                        if (!String.IsNullOrEmpty(output))
                        {
                            context.Write(node, output);
                            context.OutputFlag = true;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
