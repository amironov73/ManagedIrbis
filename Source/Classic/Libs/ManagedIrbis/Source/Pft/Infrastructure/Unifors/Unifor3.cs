// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдача данных, связанных с датой и временем – &uf('3
    // Вид функции: 3.
    // Назначение: Выдача данных, связанных с датой и временем.
    // Формат (передаваемая строка):
    // Имеются следующие подфункции:
    // 3 – выдать текущую дату в виде ГГГГММДД.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 30 – выдать текущий год в виде ГГГГ.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 31 – выдать текущий месяц в виде ММ (с лидирующим нулем).
    // Присутствует в версиях ИРБИС с 2004.1.
    // 32 – выдать текущий день в виде ДД (с лидирующим нулем).
    // Присутствует в версиях ИРБИС с 2004.1.
    // 33 – выдать текущий год в виде ГГ.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 34 – выдать текущий месяц в виде М (без лидирующего нуля).
    // Присутствует в версиях ИРБИС с 2004.1.
    // 35 – выдать текущий день в виде Д (без лидирующего нуля).
    // Присутствует в версиях ИРБИС с 2004.1.
    // 36MM – выдать по заданному номеру месяца его название
    // на русском языке в именительном падеже.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 37MM – выдать по заданному номеру месяца его название
    // на русском языке в родительном падеже.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 38MM – выдать по заданному номеру месяца его название
    // на английском языке. Присутствует в версиях ИРБИС с 2004.1.
    // 39 – выдать текущее время.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 3А – выдать номер текущего дня от начала года.
    // Присутствует в версиях ИРБИС с 2004.1.
    // 3BГГГГММДД/ддд – прибавить/вычесть из заданной даты
    // в виде ГГГГММДД заданное количество дней
    // (ддд – может быть отрицательным) и вернуть полученную дату
    // в виде ГГГГММДД. Присутствует в версиях ИРБИС с 2007.2.
    // 3СГГГГММДД/ГГГГММДД – вычесть из одной даты в виде
    // ГГГГММДД другую дату в виде ГГГГММДД и вернуть разницу
    // в виде количества дней. Присутствует в версиях ИРБИС с 2007.2.
    // 3JГГГГММДД – переводит заданную юлианскую дату ГГГГММДД
    // в грегорианскую. Присутствует в версиях ИРБИС с 2009.1.
    //
    // Примеры:
    //
    // &unifor('36',&unifor('34'))
    // Вычесть из текущей даты сто дней:
    // &uf('3B',&uf('3'),'/-100')
    // Количество дней с 1 января 1900 года до сегодняшнего дня:
    // &uf('3С',&uf('3'),'/19000101')
    //

    static class Unifor3
    {
        #region Private members

        private static readonly string[] monthNames1 =
        {
            "январь", "февраль", "март", "апрель", "май", "июнь",
            "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"
        };

        private static readonly string[] monthNames2 =
        {
            "января", "февраля", "марта", "апреля", "мая", "июня",
            "июля", "августа", "сентября", "октября", "ноября", "декабря"
        };

        private static readonly string[] monthNames3 =
        {
            "january", "february", "march", "april", "may", "june",
            "july", "august", "september", "october", "november", "december"
        };

        [CanBeNull]
        private static string _GetMonthName
            (
                [NotNull] string expression,
                [NotNull][ItemNotNull] string[] table
            )
        {
            int index;

            if (expression.Length != 3)
            {
                return null;
            }

            if (!NumericUtility.TryParseInt32(expression.Substring(1), out index))
            {
                return null;
            }

            index--;
            if (index < 0 || index >= table.Length)
            {
                return null;
            }

            return table[index];
        }

        [CanBeNull]
        private static string _AddDate
            (
                [NotNull] string expression,
                bool changeSign
            )
        {
            if (expression.Length < 11)
            {
                return null;
            }

            string dateString = expression.Substring(1, 8);

            DateTime date;

#if PocketPC

            try
            {
                date = DateTime.ParseExact
                    (
                        dateString,
                        "yyyyMMdd",
                        null,
                        DateTimeStyles.None
                    );
            }
            catch
            {
                return null;
            }

#else

            if (!DateTime.TryParseExact
                (
                    dateString,
                    "yyyyMMdd",
                    null,
                    DateTimeStyles.None,
                    out date
                ))
            {
                return null;
            }

#endif

            string deltaString = expression.Substring(10);
            int delta;
            if (!NumericUtility.TryParseInt32(deltaString, out delta))
            {
                return null;
            }
            if (changeSign)
            {
                delta = -delta;
            }
            date = date.AddDays(delta);

            return date.ToString("yyyyMMdd");
        }

        [CanBeNull]
        private static string _ToJulianDate
            (
                [NotNull] string expression
            )
        {
#if CLASSIC

            if (expression.Length < 9)
            {
                return null;
            }

            DateTime date;
            if (!DateTime.TryParseExact
                (
                    expression.Substring(1),
                    "yyyyMMdd",
                    null,
                    DateTimeStyles.None,
                    out date
                ))
            {
                return null;
            }

            return DateTimeUtility.FromJulianDate(date);

#else

            return null;

#endif
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Print current date.
        /// </summary>
        public static void PrintDate
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            expression = expression ?? string.Empty;

            char secondChar = expression.FirstChar();
            string format;
            DateTime now = context.Provider.PlatformAbstraction.Now();
            switch (secondChar)
            {
                case '\0':
                    format = "{0:yyyyMMdd}";
                    break;

                case '0':
                    format = "{0:yyyy}";
                    break;

                case '1':
                    format = "{0:MM}";
                    break;

                case '2':
                    format = "{0:dd}";
                    break;

                case '3':
                    format = "{0:yy}";
                    break;

                case '4':
                    format = now.Month.ToInvariantString();
                    break;

                case '5':
                    format = now.Day.ToInvariantString();
                    break;

                case '6':
                    format = _GetMonthName(expression, monthNames1);
                    break;

                case '7':
                    format = _GetMonthName(expression, monthNames2);
                    break;

                case '8':
                    format = _GetMonthName(expression, monthNames3);
                    break;

                case '9':
                    format = "{0:hh:mm:ss}";
                    break;

                case 'a':
                case 'A':
                    format = now.DayOfYear.ToInvariantString();
                    break;

                case 'b':
                case 'B':
                    format = _AddDate(expression, false);
                    break;

                case 'c':
                case 'C':
                    format = _AddDate(expression, true);
                    break;

                case 'j':
                case 'J':
                    format = _ToJulianDate(expression);
                    break;

                default:
                    return;
            }

            if (!string.IsNullOrEmpty(format))
            {
                string output = string.Format(format, now);
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
