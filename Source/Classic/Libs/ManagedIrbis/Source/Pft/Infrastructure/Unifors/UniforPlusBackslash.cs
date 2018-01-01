// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusBackslash.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Преобразование строки, удваивающее обратный слэш, или обратное – &uf('+\')
    // Вид функции: +\.
    // Назначение: Преобразование строки, удваивающее обратный слэш, или обратное.
    // Формат (передаваемая строка):
    // +\N<строка>
    // где:
    // N может принимать значения
    // 0 - удвоение знаков обратного слэш;
    // 1 - преобразование удвоенных знаков слэш в одинарные.
    //
    // Примеры:
    //
    // Результатом формата
    // &uf('+\0c:\example.txt')
    // будет строка
    // c:\\example.txt
    //
    // Результатом формата
    // &uf('+\1c:\\example.txt')
    // будет строка
    // c:\example.txt
    //

    static class UniforPlusBackslash
    {
        #region Public methods

        /// <summary>
        /// Convert backslashes.
        /// </summary>
        public static void ConvertBackslashes
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char command = navigator.ReadChar();
                string text = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
                if (!text.Contains("\\"))
                {
                    context.Write(node, text);
                    context.OutputFlag = true;

                    return;
                }

                bool ok = command == '1';
                navigator = new TextNavigator(text);
                while (!navigator.IsEOF)
                {
                    navigator.SkipTo('\\');
                    if (navigator.PeekChar() != '\\')
                    {
                        break;
                    }

                    string chunk = navigator.ReadWhile('\\').ThrowIfNull();
                    int length = chunk.Length;
                    if (command == '1')
                    {
                        // схлапывание удвоенных
                        if (length % 2 != 0)
                        {
                            ok = false;
                            break;
                        }
                    }
                    else
                    {
                        // удвоение
                        if (length % 2 != 0)
                        {
                            ok = true;
                            break;
                        }
                    }
                }

                if (!ok)
                {
                    context.Write(node, text);
                    context.OutputFlag = true;

                    return;
                }

                StringBuilder result = new StringBuilder(text.Length);
                navigator = new TextNavigator(text);
                while (!navigator.IsEOF)
                {
                    string chunk = navigator.ReadUntil('\\');
                    result.Append(chunk);
                    if (navigator.PeekChar() != '\\')
                    {
                        break;
                    }

                    chunk = navigator.ReadWhile('\\').ThrowIfNull();
                    int length = chunk.Length;
                    if (command == '1')
                    {
                        // схлапывание удвоенных
                        length = Math.Max(length / 2, 1);
                        for (int i = 0; i < length; i++)
                        {
                            result.Append('\\');
                        }
                    }
                    else
                    {
                        // удвоение
                        for (int i = 0; i < length; i++)
                        {
                            result.Append('\\');
                            result.Append('\\');
                        }
                    }
                }

                context.Write(node, result.ToString());
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
