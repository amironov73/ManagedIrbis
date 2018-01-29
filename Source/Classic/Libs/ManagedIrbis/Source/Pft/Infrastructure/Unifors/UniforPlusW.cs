// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusW.cs -- 
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
    // ibatrak
    //
    // Неописанная функция &unifor('+W')
    // Параметры: А#Б
    // Первый параметр отбрасывает, обрабатывает второй.
    // Похожа на unifor('+C), только ищет группу цифр в любом месте строки.
    // Если группа цифр не одна или есть минус в начале, то не делает ничего.
    // Очень странная функция.
    // Ищет группу цифр в любом месте строки.
    // Если группа цифр одна, парсит число как int32 прибавляет 1.
    // Если цифр больше, чем влазит в int32, не обрабатывает.
    // Если число переполняется, выводится с минусом.
    //

    static class UniforPlusW
    {
        #region Public methods

        /// <summary>
        /// Инкремент строки.
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

            // ibatrak
            // если в строке больше, чем 1 символ #, лишние части отбрасываются
            string[] parts = StringUtility.SplitString
                (
                    expression,
                    CommonSeparators.NumberSign,
                    StringSplitOptions.None
                );
            if (parts.Length < 2)
            {
                return;
            }

            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            string output = Increment(parts[1]);
            context.WriteAndSetFlag(node, output);
        }

        [NotNull]
        public static string Increment
            (
                [NotNull] string input
            )
        {
            Code.NotNull(input, "input");

            input = input.ToUpperInvariant();
            string result = input;
            Regex regex = new Regex("(-)?([\\d]+)");
            Match match = regex.Match(input);
            if (match.Success)
            {
                int number;

                if (!match.Groups[1].Success
                    && NumericUtility.TryParseInt32(match.Groups[2].Value, out number))
                {
                    result = input.Substring(0, match.Index)
                             + unchecked (number + 1).ToInvariantString()
                             + input.Substring(match.Index + match.Length);
                }
            }

            return result;
        }

        #endregion
    }
}
