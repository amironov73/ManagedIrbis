/* Unifor3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class Unifor3
    {
        #region Private members

        private static string[] monthNames1 =
        {
            "январь", "февраль", "март", "апрель", "май", "июнь",
            "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"
        };

        private static string[] monthNames2 =
        {
            "января", "февраля", "марта", "апреля", "мая", "июня",
            "июля", "августа", "сентября", "октября", "ноября", "декабря"
        };

        private static string[] monthNames3 =
        {
            "january", "february", "march", "april", "may", "june",
            "july", "august", "september", "october", "november", "december"
        };

        private static string _GetMonthName
            (
                string expression,
                string[] table
            )
        {
            int index;

            if (expression.Length != 3)
            {
                return null;
            }
            if (!Int32.TryParse(expression.Substring(1), out index))
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

        private static string _AddDate
            (
                string expression,
                bool changeSign
            )
        {
            if (expression.Length < 11)
            {
                return null;
            }

            string dateString = expression.Substring(1, 8);
            DateTime date;
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
            string deltaString = expression.Substring(10);
            int delta;
            if (!Int32.TryParse(deltaString, out delta))
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

        private static string _ToJulianDate
            (
                string expression
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
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (ReferenceEquals(expression, null))
            {
                return;
            }

            char secondChar = expression.Length == 0
                ? '\0'
                : expression[0];
            string format = null;
            DateTime now = DateTime.Now;
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

            if (!String.IsNullOrEmpty(format))
            {
                string output = String.Format(format, now);
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
