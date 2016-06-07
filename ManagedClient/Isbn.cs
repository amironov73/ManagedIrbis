/* Isbn.cs -- ISBN
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Всё, связанное с ISBN.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Isbn
    {
        #region Constants

        /// <summary>
        /// Стандартный знак дефис.
        /// </summary>
        public const char StandardHyphen = '-';

        #endregion

        #region Private members

        private static CultureInfo Invariant
        {
            get { return CultureInfo.InvariantCulture; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверяет контрольную цифру ISBN.
        /// </summary>
        /// <param name="isbn">Проверяемая строка.</param>
        /// <param name="hyphen">Символ дефиса.</param>
        /// <returns><c>true</c> если вычисленная и фактическая 
        /// контрольные цифры совпадают.</returns>
        public static bool CheckControlDigit
            (
                [CanBeNull] string isbn,
                char hyphen
            )
        {
            int[] digits = new int[10];
            int i, j, sum;

            if ((isbn == null) || (isbn.Length != 13))
            {
                return false;
            }

            isbn = isbn.ToUpper(Invariant);
            hyphen = char.ToUpper(hyphen, Invariant);
            for (i = j = 0; i < isbn.Length; i++)
            {
                char chr = isbn[i];
                if (chr == hyphen)
                {
                    continue;
                }
                if (chr == 'X')
                {
                    if (j == 9)
                    {
                        digits[j] = 10;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if ((chr >= '0') && (chr <= '9'))
                    {
                        digits[j++] = chr - '0';
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            for (i = sum = 0; i < 10; i++)
            {
                sum += digits[i] * (10 - i);
            }
            sum %= 11;

            return (sum == 0);
        }

        /// <summary>
        /// Проверяем контрольную цифру.
        /// </summary>
        public static bool CheckControlDigit
            (
                [CanBeNull] string isbn
            )
        {
            return CheckControlDigit
                (
                    isbn,
                    StandardHyphen
                );
        }

        /// <summary>
        /// Проверяем дефисы.
        /// </summary>
        /// <param name="isbn">Проверяемая строка.</param>
        /// <param name="hyphen">Символ дефиса.</param>
        /// <returns><c>true</c> если дефисы расставлены правильно.
        /// </returns>
        public static bool CheckHyphens
            (
                [CanBeNull] string isbn,
                char hyphen
            )
        {
            int count = 0;

            if (string.IsNullOrEmpty(isbn)
                || (isbn[0] == hyphen)
                || (isbn[isbn.Length - 1] == hyphen)
                || (isbn[isbn.Length - 2] != hyphen)
                )
            {
                return false;
            }

            for (int i = 0; i < isbn.Length - 1; i++)
            {
                if (isbn[i] == hyphen)
                {
                    if (isbn[i + 1] == hyphen)
                    {
                        return false;
                    }
                    count++;
                }
            }

            return (count == 3);
        }

        /// <summary>
        /// Проверяет дефисы.
        /// </summary>
        /// <param name="isbn">Проверяемая строка.</param>
        /// <returns><c>true</c> если дефисы расставлены правильно.
        /// </returns>
        public static bool CheckHyphens
            (
                [CanBeNull] string isbn
            )
        {
            return CheckHyphens
                (
                    isbn,
                    StandardHyphen
                );
        }

        /// <summary>
        /// Проверяет ISBN.
        /// </summary>
        /// <returns><c>true</c> если ISBN написан правильно.
        /// </returns>
        public static bool Validate
            (
                [CanBeNull] string isbn,
                bool throwException
            )
        {
            bool result = CheckHyphens(isbn)
                && CheckControlDigit(isbn);

            if (!result && throwException)
            {
                throw new ArgumentOutOfRangeException("isbn");
            }

            return result;
        }

#if NOTDEF

        /// <summary>
        /// Конвертирует ISBN в штрих-код EAN13.
        /// </summary>
        /// <param name="isbn">ISBN.</param>
        /// <returns>Штрих-код.</returns>
        public static string ToEan13(string isbn)
        {
            if ((isbn == null) || (isbn.Length != 13))
                return null;

            char[] digits = new char[13] { '9', '7', '8', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            for (int i = 0, j = 2; i < isbn.Length; i++)
            {
                char chr = isbn[i];
                if ((chr >= '0') && (chr <= '9'))
                    digits[++j] = chr;
            }
            digits[12] = ComputeCheckDigit(new string(digits));
            return new string(digits);
        }

        /// <summary>
        /// Получает (суррогатный) ISBN из штрих-кода EAN13.
        /// </summary>
        /// <param name="ean">штрих-код.</param>
        /// <returns>Суррогатный ISBN.</returns>
        public static string FromEan13(string ean)
        {
            if ((ean == null) || (ean.Length != 13))
                return null;

            char[] digits = new char[] { ' ', '-', ' ', ' ', ' ', '-', ' ', ' ', ' ', ' ', ' ', '-', ' ' };
            char[] possible = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X' };
            // Пропускаем начальные 978
            // страна
            digits[0] = ean[3];
            // издательство
            digits[2] = ean[4];
            digits[3] = ean[5];
            digits[4] = ean[6];
            // номер в темплане
            digits[6] = ean[7];
            digits[7] = ean[8];
            digits[8] = ean[9];
            digits[9] = ean[10];
            digits[10] = ean[11];
            // контрольная цифра
            string result = null;
            foreach (char chr in possible)
            {
                digits[12] = chr;
                result = new string(digits);
                if (CheckControlDigit(result))
                    break;
            }
            return result;
        }

#endif

        #endregion
    }
}
