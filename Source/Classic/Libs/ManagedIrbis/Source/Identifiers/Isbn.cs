﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isbn.cs -- ISBN
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // Международный стандартный книжный номер
    // (англ. International Standard Book Number,
    // сокращённо — англ. ISBN) — уникальный номер книжного издания,
    // необходимый для распространения книги в торговых сетях
    // и автоматизации работы с изданием. Наряду с индексами
    // библиотечно-библиографической классификации (ББК),
    // универсальной десятичной классификации (УДК) и авторским знаком,
    // международный стандартный книжный номер является частью
    // так называемого издательского пакета.
    //
    // Стандарт был разработан в Великобритании в 1966-м году
    // на базе 9-значного стандартного книжного номера
    // (англ. Standard Book Numbering code, сокращённо — англ.
    // SBN code) Гордона Фостера (англ. Gordon Foster).
    // В 1970-м году с небольшим изменением был принят как
    // международный стандарт ISO 2108. С 1 января 2007 года введен
    // новый стандарт международного стандартного книжного
    // номера — 13-значный, совпадающий со штрихкодом.
    //
    // Существует также подобный стандарт ISSN (International
    // Standard Serial Number) для периодических изданий.
    //
    // В России международный стандартный книжный номер используется
    // с 1987 года, в Белоруссии — с 1993 года.
    //
    // Пример номера
    // 978-3-16-148410-0, где:
    // 978 — префикс EAN.UCC;
    // 3 — номер регистрационной группы;
    // 16 — номер регистранта;
    // 148410 — номер издания;
    // 0 — контрольная цифра.
    //
    // Состав номера
    // Идентификаторы изданиям присваивают национальные агентства
    // в области международной стандартной нумерации книг.
    // В России до 9 декабря 2013 года этим занималась Российская
    // книжная палата, в Беларуси — Национальная книжная палата,
    // на Украине — Книжная палата Украины, в Казахстане — Национальная
    // государственная книжная палата Республики Казахстан.
    // Международные стандартные книжные номера, присвоенные книгам
    // до 2006-го года издания включительно, состоят из аббревиатуры
    // международного стандартного книжного номера (независимо от
    // языка издания) и десяти символов, разделённых дефисом или
    // пробелом на четыре поля переменной длины:
    //
    // страна происхождения или группа стран, объединенная языком
    // издания; присваивается Международным агентством ISBN.
    // Число цифр в идентификаторе группы зависит от объёмов
    // выпуска книжной продукции (может быть больше одной),
    // например: 0 и 1 — группа англоязычных стран, 2 — франкоязычных,
    // 3 — немецкий, 4 — японский, 5 — русскоязычные страны
    // (некоторые страны бывшего СССР, Россия), 7 — китайский язык,
    // 80 — Чехия и Словакия, 600 — Иран, 953 — Хорватия, 966 — Украина,
    // 985 — Республика Беларусь, 9956 — Камерун, 99948 — Эритрея.
    // В целом, группам присвоены номера 0—5, 7, 600—649, 80—94,
    // 950—993, 9940—9989, и 99900—99999;
    // код издательства; присваивается Национальным агентством
    // ISBN, при этом учитывается количество изданий, которое
    // издатель намерен выпустить в свет. Более крупным издателям
    // присваивается более короткий номер, чтобы сделать доступным
    // больше знаков для нумерации изданий (суммарная длина номеров
    // издателя и издания для ISBN, присваиваемого российским
    // агентством, составляет восемь цифр).
    // уникальный номер издания (в России — от 6 до 1 знака);
    // контрольная цифра (арабская от 0 до 9 или римская X);
    // служит для проверки правильности числовой части ISBN.
    // Расчет производит национальное агентство ISBN.
    //
    // Пример расчета контрольной цифры
    // Для 9-значного номера ISBN 2-266-11156:
    // Код ISBN     2  2  6  6  1 1 1 5  6
    // Коэффициент  10 9  8  7  6 5 4 3  2
    // Произведение 20 18 48 42 6 5 4 15 12
    //
    // Сумма произведений (цифры кода на коэффициент) составляет 170,
    // тогда остаток от деления этого числа на 11 равен 5.
    // Тогда контрольная цифра рассчитывается следующим образом: 11 − 5 = 6.
    // Полный номер ISBN: 2-266-11156-6.
    // Подтверждение правильности контрольной цифры: произведение 170
    // складывается с контрольной цифрой 170 + 6 = 176,
    // это число является кратным 11.
    //
    // Для использования в качестве штрихкодов формата EAN-13 к ISBN
    // добавляется префикс GS1 978, предоставленный Ассоциацией GS1,
    // и вместо контрольной цифры ISBN используется контрольная цифра,
    // рассчитанная по стандарту EAN-13. Выделен ещё один префикс — 979.
    //
    // С 1 января 2007 года введен новый стандарт ISBN — 13-значный,
    // совпадающий со штрихкодом. Все ранее присвоенные ISBN однозначно
    // конвертируются в новые (978 + первые 9 цифр старого ISBN
    // + контрольная цифра, рассчитанная по EAN-13).
    // Для изданий, выходящих малым тиражом (в издательской практике
    // 2003 года — до 1 000 экземпляров), либо для «личного»
    // использования присваивать номер ISBN необязательно.
    //
    // ISBN является обязательным элементом выходных данных.
    // В России по ГОСТ Р 7.0.53-2007 его помещают в нижнем левом
    // углу оборота титульного листа или в нижней левой части
    // совмещенного титульного листа. Каждая новая книга, каждое её
    // переиздание, перевод на иной язык или выпуск в новом
    // оформлении должны иметь свой международный стандартный номер.
    //
    // На издании могут стоять два и более международных стандартных
    // книжных номера, если это:
    // многотомное издание (номер тома и номер издания);
    // совместное издание (номера каждого издателя с указанием
    // в круглых скобках их наименования после соответствующего
    // международного стандартного книжного номера);
    // издание, впервые выходящее в переводе (номер перевода
    // и номер оригинала с указанием в круглых скобках сведений
    // о языке после соответствующего ISBN);
    // комплектное издание, то есть собранное в папку, футляр
    // или заключенное в общую обложку (собственный и международный
    // стандартный книжный номер, общий для всего комплекта).
    //
    // ISBN позволяет вести оперативный поиск информации о конкретном
    // издании в различных информационных ресурсах, совершенствовать
    // заказ книг, вести контроль за их продажами.
    //
    // Сведения об издателе (названия, идентификаторы ISBN,
    // адресные данные, специализация) передаются в Международное
    // агентство ISBN для выпуска Международного указателя
    // издательств и издающих организаций (англ. Publishers’
    // International ISBN Directory).
    //
    // В Российской Федерации выдачей ISBN заведует Федеральное
    // государственное учреждение науки «Российская книжная палата».
    //
    // См. https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D0%B6%D0%B4%D1%83%D0%BD%D0%B0%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D0%B4%D0%B0%D1%80%D1%82%D0%BD%D1%8B%D0%B9_%D0%BA%D0%BD%D0%B8%D0%B6%D0%BD%D1%8B%D0%B9_%D0%BD%D0%BE%D0%BC%D0%B5%D1%80
    // https://en.wikipedia.org/wiki/International_Standard_Book_Number
    // https://www.isbn-international.org/


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

        #endregion

        #region Public methods

        //
        // Appendix 1 of the International ISBN Agency's official user manual
        // describes how the 13-digit ISBN check digit is calculated.
        // The ISBN-13 check digit, which is the last digit of the ISBN,
        // must range from 0 to 9 and must be such that the sum of all the
        // thirteen digits, each multiplied by its (integer) weight,
        // alternating between 1 and 3, is a multiple of 10.
        //
        // Formally, using modular arithmetic, this is rendered:
        //
        // {\displaystyle (x_{1}+3x_{2}+x_{3}+3x_{4}+x_{5}+3x_{6}+x_{7}+3x_{8}
        // +x_{9}+3x_{10}+x_{11}+3x_{12}+x_{13})\equiv 0{\pmod {10}}.}.
        //
        // The calculation of an ISBN-13 check digit begins with the first
        // twelve digits of the 13-digit ISBN (thus excluding the check digit itself).
        // Each digit, from left to right, is alternately multiplied by 1 or 3,
        // then those products are summed modulo 10 to give a value ranging
        // from 0 to 9. Subtracted from 10, that leaves a result from 1 to 10.
        // A zero (0) replaces a ten (10), so, in all cases, a single check digit results.
        //
        //    For example, the ISBN-13 check digit of 978-0-306-40615-? is calculated as follows:
        //
        // s = 9×1 + 7×3 + 8×1 + 0×3 + 3×1 + 0×3 + 6×1 + 4×3 + 0×1 + 6×3 + 1×1 + 5×3
        // =   9 +  21 +   8 +   0 +   3 +   0 +   6 +  12 +   0 +  18 +   1 +  15
        // = 93
        // 93 / 10 = 9 remainder 3
        // 10 –  3 = 7
        //

        /// <summary>
        /// 
        /// </summary>
        public static bool Check978
            (
                string isbn
            )
        {
            int result = 0;
            int[] multipliers = { 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1 };
            int index = 0;
            for (int i = 0; i < isbn.Length; i++)
            {
                if (isbn[i].IsArabicDigit())
                {
                    if (index >= multipliers.Length)
                    {
                        return false;
                    }
                    result += ((int)(isbn[i] - '0')) * multipliers[index++];
                }
            }

            return (index == 13) && (result % 10) == 0;
        }

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
            if (!string.IsNullOrEmpty(isbn)
                && isbn.StartsWith("978-"))
            {
                return Check978(isbn);
            }

            int[] digits = new int[10];
            int i, j, sum;

            if (ReferenceEquals(isbn, null)
                || isbn.Length != 13)
            {
                return false;
            }

            isbn = isbn.ToUpper();
            hyphen = char.ToUpper(hyphen);
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
                    if (chr >= '0' && chr <= '9')
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

            return sum == 0;
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
                || isbn[0] == hyphen
                || isbn[isbn.Length - 1] == hyphen
                || isbn[isbn.Length - 2] != hyphen
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

            return count == 3;
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
        /// Исправляет самые распространенные ошибки.
        /// </summary>
        [NotNull]
        public static string FixIsbn
            (
                [NotNull] string isbn
            )
        {
            CodeJam.Code.NotNullNorEmpty(isbn, "isbn");

            string result = isbn.Trim().Trim('(', ')', '.').Trim();
            if (result.StartsWith("ISBN"))
            {
                result = result.Substring(4).TrimStart();
            }

            result = result
                .Replace('\x004F', '0')
                .Replace('\x006F', '0')
                .Replace('\x041E', '0')
                .Replace('\x043E', '0');

            result = result
                .Replace('\x0425', 'X')
                .Replace('\x0445', 'X')
                .Replace('x', 'X');

            result = result
                .Replace('\x2013',StandardHyphen)
                .Replace('\x2014', StandardHyphen);

            result = result
                .Replace(' ', StandardHyphen);

            return result;
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

            if (!result)
            {
                Log.Error
                    (
                        "Isbn::Validate: "
                        + "isbn="
                        + isbn.ToVisibleString()
                    );

                if (throwException)
                {
                    throw new ArgumentOutOfRangeException("isbn");
                }
            }

            return result;
        }

        /// <summary>
        /// Конвертирует ISBN в штрих-код EAN13.
        /// </summary>
        /// <param name="isbn">ISBN.</param>
        /// <returns>Штрих-код.</returns>
        public static string ToEan13
            (
                string isbn
            )
        {
            if (string.IsNullOrEmpty(isbn)
                || isbn.Length != 13)
            {
                return null;
            }

            char[] digits =
            {
                '9', '7', '8', ' ', ' ', ' ', ' ', ' ',
                ' ', ' ', ' ', ' ', ' '
            };
            for (int i = 0, j = 2; i < isbn.Length; i++)
            {
                char chr = isbn[i];
                if (chr >= '0' && chr <= '9')
                {
                    digits[++j] = chr;
                }
            }
            digits[12] = Ean13.ComputeCheckDigit(digits);

            return new string(digits);
        }

        /// <summary>
        /// Получает (суррогатный) ISBN из штрих-кода EAN13.
        /// </summary>
        /// <param name="ean">штрих-код.</param>
        /// <returns>Суррогатный ISBN.</returns>
        [CanBeNull]
        public static string FromEan13
            (
                [CanBeNull] string ean
            )
        {
            if (string.IsNullOrEmpty(ean)
                || ean.Length != 13)
            {
                return null;
            }

            char[] digits = { ' ', '-', ' ', ' ', ' ', '-', ' ', ' ', ' ', ' ', ' ', '-', ' ' };
            char[] possible = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X' };
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
                {
                    break;
                }
            }
            return result;
        }

        #endregion
    }
}
