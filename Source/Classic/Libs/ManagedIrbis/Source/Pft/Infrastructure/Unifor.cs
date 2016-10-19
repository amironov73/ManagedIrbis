/* Unifor.cs --
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
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Unifor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Unifor
        : IFormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, Action<PftContext, PftNode, string>> Registry { get; private set; }

        /// <summary>
        /// Throw exception on unknown key.
        /// </summary>
        public static bool ThrowOnUnknown { get; set; }

        #endregion

        #region Construction

        static Unifor()
        {
            ThrowOnUnknown = false;

            Registry = new Dictionary<string, Action<PftContext, PftNode, string>>
                (
#if NETCORE || UAP || WIN81

                    StringComparer.OrdinalIgnoreCase

#else

                StringComparer.InvariantCultureIgnoreCase

#endif
                );

            RegisterActions();
        }

        #endregion

        #region Private members

        private static void RegisterActions()
        {
            Registry.Add("0", FormatAll);
            Registry.Add("3", PrintDate);
            Registry.Add("9", RemoveDoubleQuotes);
            Registry.Add("A", GetFieldRepeat);
            Registry.Add("R", RandomNumber);
            Registry.Add("+90", GetIndex);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find action for specified expression.
        /// </summary>
        public static Action<PftContext, PftNode, string> FindAction
            (
            [NotNull] ref string expression
            )
        {
            var keys = Registry.Keys;
            int bestMatch = 0;
            Action<PftContext, PftNode, string> result = null;

            foreach (string key in keys)
            {
                if (key.Length > bestMatch
                    && expression.StartsWith(key))
                {
                    bestMatch = key.Length;
                    result = Registry[key];
                }
            }

            if (bestMatch != 0)
            {
                expression = expression.Substring(bestMatch);
            }

            return result;
        }

        /// <summary>
        /// ALL format for records
        /// </summary>
        public static void FormatAll
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                string text = record.ToPlainText();
                context.Write(node, text);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetFieldRepeat
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            try
            {
                MarcRecord record = context.Record;
                if (!ReferenceEquals(record, null))
                {
                    FieldSpecification specification = new FieldSpecification();
                    if (specification.Parse(expression))
                    {
                        FieldReference reference = new FieldReference();
                        reference.Apply(specification);

                        string result = reference.Format(record);
                        if (!string.IsNullOrEmpty(result))
                        {
                            context.Write(node, result);
                            context.OutputFlag = true;
                        }
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Eat the exception
            }
        }

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetIndex
            (
            PftContext context,
            PftNode node,
            string expression
            )
        {
            int index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }
            string text = index.ToInvariantString();
            context.Write(node, text);
            context.OutputFlag = true;
        }

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
            if (!int.TryParse(expression.Substring(1), out index))
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
            if (!int.TryParse(deltaString, out delta))
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

            if (!string.IsNullOrEmpty(format))
            {
                string output = string.Format(format, now);
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Generate random number.
        /// </summary>
        public static void RandomNumber
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            int length = 6;
            if (!string.IsNullOrEmpty(expression))
            {
                int.TryParse(expression, out length);
            }
            if (length <= 0 || length > 9)
            {
                return;
            }

            int maxValue = 1;
            for (int i = 0; i < length; i++)
            {
                maxValue = maxValue*10;
            }

            int result = new Random().Next(maxValue);
            string format = new string('0', length);
            string output = result.ToString(format);
            context.Write(node, output);
        }


        /// <summary>
        /// Remove double quotes from the string.
        /// </summary>
        public static void RemoveDoubleQuotes
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("\"", string.Empty);
                context.Write(node, clear);
            }
        }

        #endregion

        #region IFormatExit members

        /// <inheritdoc/>
        public string Name { get { return "unifor"; } }

        /// <inheritdoc/>
        public void Execute
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            Action<PftContext, PftNode, string> action
                = FindAction(ref expression);

            if (ReferenceEquals(action, null))
            {
                if (ThrowOnUnknown)
                {
                    throw new PftException("Unknown unifor: " + expression);
                }
            }
            else
            {
                action
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        #endregion
    }
}
