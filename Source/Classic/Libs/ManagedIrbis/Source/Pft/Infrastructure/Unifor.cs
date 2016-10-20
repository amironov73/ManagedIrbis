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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Pft.Infrastructure.Unifors;
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
            Registry.Add("3", Unifor3.PrintDate);
            Registry.Add("9", RemoveDoubleQuotes);
            Registry.Add("A", GetFieldRepeat);
            Registry.Add("Q", ToLower);
            Registry.Add("R", RandomNumber);
            Registry.Add("T", Transliterate);
            Registry.Add("X", RemoveAngleBrackets);
            Registry.Add("+90", UniforPlus9.GetIndex);
            Registry.Add("+91", UniforPlus9.GetFileName);
            Registry.Add("+92", UniforPlus9.GetDirectoryName);
            Registry.Add("+93", UniforPlus9.GetExtension);
            Registry.Add("+94", UniforPlus9.GetDrive);
            Registry.Add("+95", UniforPlus9.Length);
            Registry.Add("+97", UniforPlus9.ToUpper);
            Registry.Add("+98", UniforPlus9.ReplaceCharacter);
            Registry.Add("+9F", UniforPlus9.GetCharacter);
            Registry.Add("+9G", UniforPlus9.SplitWords);
            Registry.Add("+9I", UniforPlus9.ReplaceString);
            Registry.Add("+9V", UniforPlus9.GetVersion);
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
                maxValue = maxValue * 10;
            }

            int result = new Random().Next(maxValue);
            string format = new string('0', length);
            string output = result.ToString(format);
            context.Write(node, output);
            context.OutputFlag = true;
        }

        /// <summary>
        /// Remove text surrounded with angle brackets.
        /// </summary>
        public static void RemoveAngleBrackets
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = Regex.Replace(expression, "<.*?>", string.Empty);
                context.Write(node, clear);
                context.OutputFlag = true;
            }
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
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Convert the string to lower case.
        /// </summary>
        public static void ToLower
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = expression.ToLowerInvariant();
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        private static Dictionary<char, string> _transliterator
            = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
                {'е', "e"}, {'ё', "io"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
                {'й', "i"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
                {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
                {'ш', "sh"}, {'щ', "shch"}, {'ь', "'"}, {'ы', "y"}, {'ъ', "\""},
                {'э', "e"}, {'ю', "iu"}, {'я', "ia"},
                {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"},
                {'Е', "E"}, {'Ё', "IO"}, {'Ж', "ZH"}, {'З', "Z"}, {'И', "I"},
                {'Й', "I"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
                {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"},
                {'У', "U"}, {'Ф', "F"}, {'Х', "kh"}, {'Ц', "ts"}, {'Ч', "ch"},
                {'Ш', "sh"}, {'Щ', "shch"}, {'Ь', "'"}, {'Ы', "Y"}, {'Ъ', "\""},
                {'Э', "E"}, {'Ю', "IU"}, {'Я', "IA"}
            };

        /// <summary>
        /// Transliterate cyrillic to latin.
        /// </summary>
        public static void Transliterate
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // Attention: in original IRBIS &uf('T0ё') breaks the script
            //

            if (!string.IsNullOrEmpty(expression))
            {
                StringBuilder result = new StringBuilder();

                foreach (char c in expression.Skip(1))
                {
                    string s;
                    if (_transliterator.TryGetValue(c, out s))
                    {
                        result.Append(s);
                    }
                    else
                    {
                        result.Append(c);
                    }
                }

                context.Write(node, result.ToString());
                context.OutputFlag = true;
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
