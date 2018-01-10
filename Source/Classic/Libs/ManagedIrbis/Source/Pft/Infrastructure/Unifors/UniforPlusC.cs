// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusC.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+C')
    // Очень странная функция.
    // Ищет группу цифр в начале или в конце строки.
    // Если группа цифр одна, парсит число как int32 прибавляет 1.
    // Если цифр больше, чем влазит в int32, лишние цифры отбрасываются
    // Если число переполняется, выводится с минусом.
    //

    static class UniforPlusC
    {
        #region Public methods

        /// <summary>
        /// Increment numeric value of the string.
        /// </summary>
        public static void Increment
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            string output = expression;
            Regex regex = new Regex("^([\\d]+)?([^\\d]*)([\\d]+)?$");
            Match match = regex.Match(expression);
            if (match.Success
                && match.Groups[1].Success ^ match.Groups[3].Success)
            {
                string s = match.Groups[1].Success
                    ? match.Groups[1].Value
                    : match.Groups[3].Value;
                for (int length = s.Length; length > 0; length--)
                {
                    unchecked
                    {
                        int value = 0;
                        if (NumericUtility.TryParseInt32(s.Substring(0, length), out value))
                        {
                            if (match.Groups[1].Success)
                            {
                                output = value + 1 + match.Groups[2].Value + match.Groups[3].Value;
                            }
                            else if (match.Groups[3].Success)
                            {
                                output = match.Groups[1].Value + match.Groups[2].Value + (value + 1);
                            }

                            break;
                        }
                    }
                }
            }

            context.Write(node, output);
            context.OutputFlag = true;
        }

        #endregion
    }
}
