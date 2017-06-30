// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Ismn.cs -- ISMN
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Globalization;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // Международный стандартный музыкальный номер
    // или ISMN (англ. International Standard Music Number)
    // — стандарт десятизначной буквенно-цифровой идентификации
    // нотных изданий. Стандарт разработан в ISO (ISO 10957).
    //
    // Международная стандартная нумерация музыкальных изданий
    // осуществляется под руководством Международного агентства
    // ISMN(англ. International ISMN Agency). Международное
    // агентство ISMN централизованно предоставляет номера ISMN
    // национальным агентствам ISMN стран-участ­ни­ков системы ISMN.
    // В Российской Федерации функции национального агентства
    // ISMN в 2007 году возложены на Российскую книжную палату.
    //
    // ISMN составляется из четырёх блоков, разделённых дефисами:
    //
    // префикс M (необходим в целях отличия от ISBN),
    // идентификатор издателя,
    // идентификатор конкретного издания,
    // одна контрольная цифра.
    //
    // Нотному изданию могут быть присвоены оба кода, как ISMN,
    // так и ISBN. В отличие от ISBN, в стандарт ISMN не заложено
    // разделение издателей по странам.
    //
    // Расчёт контрольной цифры производится по следующему алгоритму.
    // Каждый знак в коде ISMN умножается на его вес.
    // Веса знака — чередующиеся 3 и 1. Префикс M всегда имеет вес 3,
    // следующий знак — вес 1, следующий — вес 3, и так далее.
    // Префиксу М присвоено цифровое значение 3. Цифровые значения
    // умножаются на соответствующие веса знаков, затем полученные
    // значения складываются. Полученная сумма делится на 10 и от
    // получившегося частного берётся остаток. Он и будет
    // являться контрольной цифрой.
    //
    // Например, для ISMN-кода M-060-11561 расчёт контрольной цифры
    // будет таким:
    // 3xM + 1x0 + 3x6 + 1x0 + 3x1 + 1x1 + 3x5 + 1x6 + 3x1 =
    //  9  +  0  +  18 +  0  +  3  +  1  +  15 +  6  +  3  =  55
    //
    // Поскольку 55 mod 10=5 , контрольная цифра — 5, а полный
    // ISMN-код для данного издания: M-060-11561-5.
    //
    // См. https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D0%B6%D0%B4%D1%83%D0%BD%D0%B0%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D0%B4%D0%B0%D1%80%D1%82%D0%BD%D1%8B%D0%B9_%D0%BC%D1%83%D0%B7%D1%8B%D0%BA%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9_%D0%BD%D0%BE%D0%BC%D0%B5%D1%80
    //
    // https://en.wikipedia.org/wiki/International_Standard_Music_Number
    //
    // To calculate the check digit, each digit of the ISMN is multiplied
    // by a weight, alternating 1 and 3 left to right. These weighted
    // digits are added together. The check digit is the integer between
    // 0 and 9 that makes the sum a multiple of 10.
    //
    // Examples
    //
    // For instance, for the item with ISMN beginning 979-0-060-11561:
    //
    // 1x9 + 3x7 + 1x9 + 3x0 + 1x0 + 3x6 + 1x0 + 3x1 + 1x1 + 3x5 + 1x6 + 3x1
    // = 9 + 21 + 9 + 0 + 0 + 18 + 0 + 3 + 1 + 15 + 6 + 3
    // = 85
    //
    // As 85 mod 10 = 5, the check digit is 10 - 5 = 5 and the full
    // number is 979-0-060-11561-5.
    //
    // For another example, Robert Fripp's collection of Guitar Craft
    // scores has the ISMN 979-0-9016791-7-7.
    // Given first 12 digits 979-0-9016791-7, the ISMN algorithm evaluates
    //
    // 1x9 + 3x7 + 1x9 + 3x0 + 1x9 + 3x0 + 1x1 + 3x1 + 1x6 + 3x7 + 1x9 + 3x1
    // = 9 + 21 + 9 + 0 + 9 + 0 + 1 + 3 + 6 + 21 + 9 + 3
    // = 123
    //
    // which implies that the check digit is indeed 7 (because 123+7=130=13x10).

    /// <summary>
    /// Всё, связанное с ISMN.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Ismn
    {
        #region Constants

        /// <summary>
        /// Стандартный знак дефис.
        /// </summary>
        public const char StandardHyphen = '-';

        #endregion

        #region Properties

        /// <summary>
        /// Allowed digits.
        /// </summary>
        public static CodeDigit[] IsoIsmn =
        {
            new CodeDigit('M', 3),
            new CodeDigit('m', 3),
            new CodeDigit('0', 0),
            new CodeDigit('1', 1),
            new CodeDigit('2', 2),
            new CodeDigit('3', 3),
            new CodeDigit('4', 4),
            new CodeDigit('5', 5),
            new CodeDigit('6', 6),
            new CodeDigit('7', 7),
            new CodeDigit('8', 8),
            new CodeDigit('9', 9)
        };

        /// <summary>
        /// Allowed digits.
        /// </summary>
        public static CodeDigit[] EanIsmn =
        {
            new CodeDigit('0', 0),
            new CodeDigit('1', 1),
            new CodeDigit('2', 2),
            new CodeDigit('3', 3),
            new CodeDigit('4', 4),
            new CodeDigit('5', 5),
            new CodeDigit('6', 6),
            new CodeDigit('7', 7),
            new CodeDigit('8', 8),
            new CodeDigit('9', 9)
        };

        #endregion

        #region Private members

        private static CultureInfo Invariant
        {
            get { return CultureInfo.InvariantCulture; }
        }

        private static int[] _IsoWeight =
        {
            3, 1, 3, 1, 3, 1, 3, 1, 3, 1
        };

        private static int[] _EanWeight =
        {
            1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Проверяет контрольную цифру ISMN.
        /// </summary>
        public static bool CheckControlDigit
            (
                [CanBeNull] string ismn
            )
        {
            if (string.IsNullOrEmpty(ismn))
            {
                return false;
            }

            CodeDigit[] digits;
            int[] weight;

            if (ismn.Length == 13)
            {
                // ISO, i. e. M-060-11561-5

                digits = CodeDigit.ExtractDigits(ismn, IsoIsmn);
                if (digits.Length != 10)
                {
                    return false;
                }
                char firstDigit = char.ToUpper(digits[0].Digit);
                if (firstDigit != 'M')
                {
                    return false;
                }
                weight = _IsoWeight;
            }
            else if (ismn.Length == 17)
            {
                // EAN, i. e. 979-0-060-11561-5

                digits = CodeDigit.ExtractDigits(ismn, EanIsmn);
                if (digits.Length != 13
                    || digits[0].Digit != '9'
                    || digits[1].Digit != '7'
                    || digits[2].Digit != '9')
                {
                    return false;
                }
                weight = _EanWeight;
            }
            else
            {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                sum += digits[i].Value * weight[i];
            }

            return sum % 10 == 0;
        }

        /// <summary>
        /// Проверяем дефисы.
        /// </summary>
        /// <param name="ismn">Проверяемая строка.</param>
        /// <param name="hyphen">Символ дефиса.</param>
        /// <returns><c>true</c> если дефисы расставлены правильно.
        /// </returns>
        public static bool CheckHyphens
            (
                [CanBeNull] string ismn,
                char hyphen
            )
        {
            int count = 0;

            if (string.IsNullOrEmpty(ismn)
                || ismn.Length != 13 && ismn.Length != 17
                || ismn[0] == hyphen
                || ismn[ismn.Length - 1] == hyphen
                || ismn[ismn.Length - 2] != hyphen
            )
            {
                return false;
            }

            for (int i = 0; i < ismn.Length - 1; i++)
            {
                if (ismn[i] == hyphen)
                {
                    if (ismn[i + 1] == hyphen)
                    {
                        return false;
                    }
                    count++;
                }
            }

            if (ismn.Length == 13)
            {
                // ISO

                if (ismn[1] != hyphen)
                {
                    return false;
                }

                return count == 3;
            }

            // EAN

            if (ismn[3] != hyphen)
            {
                return false;
            }

            return count == 4;
        }

        /// <summary>
        /// Проверяем дефисы.
        /// </summary>
        public static bool CheckHyphens
            (
                [CanBeNull] string ismn
            )
        {
            return CheckHyphens(ismn, StandardHyphen);
        }

        /// <summary>
        /// Verify the ISMN.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string ismn,
                bool throwOnError
            )
        {
            bool result = CheckControlDigit(ismn)
                          && CheckHyphens(ismn);

            if (!result)
            {
                Log.Error
                    (
                        "Ismn::Verify: "
                        + "failed for "
                        + ismn.ToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "ISMN validation failed: "
                            + ismn.ToVisibleString()
                        );
                }
            }

            return result;
        }

        #endregion
    }
}
