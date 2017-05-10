// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Ean13.cs --
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

namespace ManagedIrbis.Identifiers
{
    //
    // Information from Wikipedia
    // https://ru.wikipedia.org/wiki/European_Article_Number
    //
    // European Article Number, EAN (европейский номер
    // товара) — европейский стандарт штрихкода,
    // предназначенный для кодирования идентификатора
    // товара и производителя. Является надмножеством
    // американского стандарта UPC.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Ean13
    {
        #region Public data

        /// <summary>
        /// Coefficients for control digit calculations.
        /// </summary>
        public static int[] Coefficients
            = { 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1 };


        #endregion

        #region Public methods

        /// <summary>
        /// Compute check digit.
        /// </summary>
        public static char ComputeCheckDigit
            (
                [NotNull] char[] digits
            )
        {
            Code.NotNull(digits, "digits");

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum = sum + (digits[i] - '0') * Coefficients[12 - i];
            }
            char result = (char)(10 - sum % 10 + '0');

            return result;
        }

        /// <summary>
        /// Check control digit.
        /// </summary>
        public static bool CheckControlDigit
            (
                [NotNull] char[] digits
            )
        {
            Code.NotNull(digits, "digits");

            int sum = 0;
            for (int i = 0; i < 13; i++)
            {
                sum = sum + (digits[i] - '0') * Coefficients[12 - i];
            }
            bool result = sum % 10 == 0;

            return result;
        }

        #endregion
    }
}
