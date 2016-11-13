/* PftUtility.cs --
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Utility routines for PFT scripting.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static int CompareStrings
            (
                [CanBeNull] string first,
                [CanBeNull] string second
            )
        {
            int result = string.Compare
                (
                    first,
                    second,
                    StringComparison.CurrentCultureIgnoreCase
                );

            return result;
        }

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubString
            (
                [CanBeNull] string outer,
                [CanBeNull] string inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                return true;
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            outer = outer.ToLower();
            inner = inner.ToLower();

            bool result = outer.Contains(inner);

            return result;
        }

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubStringSensitive
            (
                [CanBeNull] string outer,
                [CanBeNull] string inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                return true;
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            bool result = outer.Contains(inner);

            return result;
        }

        /// <summary>
        /// Extract numeric value from the input text.
        /// </summary>
        public static double ExtractNumericValue
            (
                [CanBeNull] string input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0.0;
            }

            Match match = Regex.Match
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            if (!match.Success)
            {
                return 0.0;
            }

            string value = match.Value;
            double result;
            double.TryParse
                (
                    value,
                    NumberStyles.AllowDecimalPoint
                    | NumberStyles.AllowLeadingSign
                    | NumberStyles.AllowExponent
                    | NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out result
                );

            return result;
        }

        /// <summary>
        /// Extract numeric values from the input text.
        /// </summary>
        public static double[] ExtractNumericValues
            (
                [CanBeNull] string input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return new double[0];
            }

            List<double> result = new List<double>();
            MatchCollection matches = Regex.Matches
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            foreach (Match match in matches)
            {
                double value;
                if (double.TryParse
                    (
                        match.Value,
                        NumberStyles.AllowDecimalPoint
                        | NumberStyles.AllowLeadingSign
                        | NumberStyles.AllowExponent
                        | NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out value
                    ))
                {
                    result.Add(value);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Format for data mode.
        /// </summary>
        [NotNull]
        public static string FormatDataMode
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            string result = FormatHeaderMode(field);

            if (!result.EndsWith(".")
                & !result.EndsWith(". ")
                & !result.EndsWith(".  "))
            {
                result = result + ".";
            }

            if (result.EndsWith("  "))
            {
                // nothing to do
            }
            else if (result.EndsWith(" "))
            {
                result = result + " ";
            }
            else
            {
                result = result + "  ";
            }

            return result;
        }

        /// <summary>
        /// Format for header mode.
        /// </summary>
        [NotNull]
        public static string FormatHeaderMode
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            StringBuilder result = new StringBuilder();

            result.Append(field.Value);
            foreach (SubField subField in field.SubFields)
            {
                string delimiter = ". ";
                char code = char.ToLower(subField.Code);
                switch (code)
                {
                    case 'a':
                        delimiter = "; ";
                        break;

                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                        delimiter = ", ";
                        break;
                }
                if (result.Length != 0)
                {
                    result.Append(delimiter);
                }

                string value = subField.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    value = value
                        .Replace("><", "; ")
                        .Replace("<", string.Empty)
                        .Replace(">", string.Empty);
                }

                result.Append(value);
            }

            return result.ToString();
        }

        /// <summary>
        /// Format field according to specified output mode.
        /// </summary>
        [NotNull]
        public static string FormatField
            (
                [NotNull] this RecordField field,
                PftFieldOutputMode mode,
                bool uppercase
            )
        {
            Code.NotNull(field, "field");

            string result;

            switch (mode)
            {
                case PftFieldOutputMode.DataMode:
                    result = FormatDataMode(field);
                    break;

                case PftFieldOutputMode.HeaderMode:
                    result = FormatHeaderMode(field);
                    break;

                case PftFieldOutputMode.PreviewMode:
                    result = field.ToText();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (uppercase)
            {
                result = result.ToUpper();
            }

            return result;
        }

        /// <summary>
        /// Format value like function f does.
        /// </summary>
        public static string FormatLikeF
            (
                double value,
                int arg2,
                int arg3
            )
        {
            int minLength = 1;
            if (arg2 < 0)
            {
                if (arg3 < 0)
                {
                    minLength = 16;
                }
            }
            else
            {
                minLength = arg2;
            }


            bool useE = true;
            int decimalPoints = 0;
            if (arg3 >= 0)
            {
                useE = false;
                decimalPoints = arg3;
            }

            string format = useE
                ? string.Format("E{0}", minLength)
                : string.Format("F{0}", decimalPoints);

            string result = value.ToString
                (
                    format,
                    CultureInfo.InvariantCulture
                )
                .PadLeft
                (
                    minLength,
                    ' '
                );

            return result;
        }

        /// <summary>
        /// Get array of reserved words.
        /// </summary>
        public static string[] GetReservedWords()
        {
            return new[]
            {
                "a",
                "abs",
                "and",
                "break",
                "ceil",
                "div",
                "do",
                "else",
                "end",
                "f",
                "f2",
                "fi",
                "floor",
                "for",
                "frac",
                "if",
                "l",
                "mdl",
                "mdu",
                "mfn",
                "mhl",
                "mhu",
                "mpl",
                "mpu",
                "not",
                "or",
                "p",
                "pow",
                "proc",
                "ravr",
                "ref",
                "rmax",
                "rmin",
                "round",
                "rsum",
                "s",
                "sign",
                "then",
                "trunc",
                "uf",
                "unifor",
                "val",
                "while"
            };
        }

        /// <summary>
        /// Prepare text for <see cref="PftUnconditionalLiteral"/>,
        /// <see cref="PftConditionalLiteral"/>,
        /// <see cref="PftRepeatableLiteral"/>.
        /// </summary>
        [CanBeNull]
        public static string PrepareText
            (
                [CanBeNull] string text
            )
        {
            string result = text;

            if (!string.IsNullOrEmpty(text))
            {
                result = text
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            return result;
        }

        #endregion
    }
}
