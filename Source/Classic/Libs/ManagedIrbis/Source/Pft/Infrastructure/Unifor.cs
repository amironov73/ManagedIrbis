// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Identifiers;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
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
        public static CaseInsensitiveDictionary<Action<PftContext, PftNode, string>> Registry
        {
            get; private set;
        }

        /// <summary>
        /// Throw exception on unknown key.
        /// </summary>
        public static bool ThrowOnUnknown { get; set; }

        #endregion

        #region Construction

        static Unifor()
        {
            ThrowOnUnknown = false;

            Registry = new CaseInsensitiveDictionary<Action<PftContext, PftNode, string>>();

            RegisterActions();
        }

        #endregion

        #region Private members

        private static void RegisterActions()
        {
            Registry.Add("0", FormatAll);
            Registry.Add("1", Unifor1.GetElement);
            Registry.Add("3", Unifor3.PrintDate);
            Registry.Add("4", Unifor4.FormatPreviousVersion);
            Registry.Add("6", Unifor6.ExecuteNestedFormat);
            Registry.Add("9", RemoveDoubleQuotes);
            Registry.Add("A", GetFieldRepeat);
            Registry.Add("B", Convolution);
            Registry.Add("C", CheckIsbn);
            Registry.Add("D", UniforD.FormatDocumentDB);
            Registry.Add("E", UniforE.GetFirstWords);
            Registry.Add("F", UniforE.GetLastWords);
            Registry.Add("G", GetPart);
            Registry.Add("I", GetIniFileEntry);
            Registry.Add("J", UniforJ.GetTermRecordCountDB);
            Registry.Add("K", GetMenuEntry);
            Registry.Add("M", UniforM.Sort);
            Registry.Add("O", UniforO.AllExemplars);
            Registry.Add("P", UniqueField);
            Registry.Add("Q", ToLower);
            Registry.Add("R", RandomNumber);
            Registry.Add("S", UniforS.Add);
            Registry.Add("S0", UniforS.Clear);
            Registry.Add("SA", UniforS.Arabic);
            Registry.Add("SX", UniforS.Roman);
            Registry.Add("T", Transliterate);
            Registry.Add("U", UniforU.Cumulate);
            Registry.Add("V", UniforU.Decumulate);
            Registry.Add("W", UniforU.Check);
            Registry.Add("X", RemoveAngleBrackets);
            Registry.Add("Y", UniforO.FreeExemplars);
            Registry.Add("+2", UniforPlus2.System);
            Registry.Add("+3D", UniforPlus3.UrlDecode);
            Registry.Add("+3E", UniforPlus3.UrlEncode);
            Registry.Add("+3U", UniforPlus3.ConvertToUtf);
            Registry.Add("+3W", UniforPlus3.ConvertToAnsi);
            Registry.Add("+3+", UniforPlus3.ReplacePlus);
            Registry.Add("+6", GetRecordStatus);
            Registry.Add("+7", UniforPlus7.ClearGlobals);
            Registry.Add("+7A", UniforPlus7.UnionGlobals);
            Registry.Add("+7G", UniforPlus7.DistinctGlobal);
            Registry.Add("+7M", UniforPlus7.MultiplyGlobals);
            Registry.Add("+7R", UniforPlus7.ReadGlobal);
            Registry.Add("+7S", UniforPlus7.SubstractGlobals);
            Registry.Add("+7T", UniforPlus7.SortGlobal);
            Registry.Add("+7U", UniforPlus7.AppendGlobal);
            Registry.Add("+7W", UniforPlus7.WriteGlobal);
            Registry.Add("+8", UniforPlus8.ExecuteNativeMethod);
            Registry.Add("+90", UniforPlus9.GetIndex);
            Registry.Add("+91", UniforPlus9.GetFileName);
            Registry.Add("+92", UniforPlus9.GetDirectoryName);
            Registry.Add("+93", UniforPlus9.GetExtension);
            Registry.Add("+94", UniforPlus9.GetDrive);
            Registry.Add("+95", UniforPlus9.Length);
            Registry.Add("+96", UniforPlus9.Substring);
            Registry.Add("+97", UniforPlus9.ToUpper);
            Registry.Add("+98", UniforPlus9.ReplaceCharacter);
            Registry.Add("+9F", UniforPlus9.GetCharacter);
            Registry.Add("+9G", UniforPlus9.SplitWords);
            Registry.Add("+9I", UniforPlus9.ReplaceString);
            Registry.Add("+9S", FindSubstring);
            Registry.Add("+9V", UniforPlus9.GetVersion);
            Registry.Add("+D", GetDatabaseName);
            Registry.Add("+E", GetFieldIndex);
            Registry.Add("+F", CleanRtf);
            Registry.Add("+N", GetFieldCount);
            Registry.Add("+R", TrimAtLastDot);
            Registry.Add("+S", DecodeTitle);
            Registry.Add("+@", UniforPlusAt.FormatJson);
            Registry.Add("!", CleanDoubleText);
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

        // ================================================================

        /// <summary>
        /// Контроль ISSN/ISBN.
        /// Возвращаемое значение: 0 – при положительном
        /// результате, 1 – при отрицательном.
        /// </summary>
        public static void CheckIsbn
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expresion
            )
        {
            string output = "1";

            if (!string.IsNullOrEmpty(expresion))
            {
                List<char> digits = new List<char>(expresion.Length);
                foreach (char c in expresion)
                {
                    if (PftUtility.DigitsX.Contains(c))
                    {
                        digits.Add(c);
                    }
                }
                if (digits.Count == 8)
                {
                    if (Issn.CheckControlDigit(expresion))
                    {
                        output = "0";
                    }
                }
                else if (digits.Count == 10)
                {
                    if (Isbn.Validate(expresion, false))
                    {
                        output = "0";
                    }
                }
            }

            context.Write(node, output);
            context.OutputFlag = true;
        }

        // ================================================================

        private static char[] _GoodCharacters =
        {
            '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035',
            '\u0036', '\u0037', '\u0038', '\u0039', '\u0041', '\u0042',
            '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048',
            '\u0049', '\u004A', '\u004B', '\u004C', '\u004D', '\u004E',
            '\u004F', '\u0050', '\u0051', '\u0052', '\u0053', '\u0054',
            '\u0055', '\u0056', '\u0057', '\u0058', '\u0059', '\u005A',
            '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066',
            '\u0067', '\u0068', '\u0069', '\u006A', '\u006B', '\u006C',
            '\u006D', '\u006E', '\u006F', '\u0070', '\u0071', '\u0072',
            '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078',
            '\u0079', '\u007A', '\u0402', '\u0403', '\u201A', '\u0453',
            '\u201E', '\u2026', '\u2020', '\u2021', '\u20AC', '\u2030',
            '\u0409', '\u2039', '\u040A', '\u040C', '\u040B', '\u040F',
            '\u0452', '\u2018', '\u2019', '\u201C', '\u201D', '\u2022',
            '\u2013', '\u2014', '\u0098', '\u2122', '\u0459', '\u203A',
            '\u045A', '\u045C', '\u045B', '\u045F', '\u00A0', '\u040E',
            '\u045E', '\u0408', '\u00A4', '\u0490', '\u00A6', '\u00A7',
            '\u0401', '\u00A9', '\u0404', '\u00AB', '\u00AC', '\u00AD',
            '\u00AE', '\u0407', '\u00B0', '\u00B1', '\u0406', '\u0456',
            '\u0491', '\u00B5', '\u00B6', '\u00B7', '\u0451', '\u2116',
            '\u0454', '\u00BB', '\u0458', '\u0405', '\u0455', '\u0457',
            '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415',
            '\u0416', '\u0417', '\u0418', '\u0419', '\u041A', '\u041B',
            '\u041C', '\u041D', '\u041E', '\u041F', '\u0420', '\u0421',
            '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427',
            '\u0428', '\u0429', '\u042A', '\u042B', '\u042C', '\u042D',
            '\u042E', '\u042F', '\u0430', '\u0431', '\u0432', '\u0433',
            '\u0434', '\u0435', '\u0436', '\u0437', '\u0438', '\u0439',
            '\u043A', '\u043B', '\u043C', '\u043D', '\u043E', '\u043F',
            '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445',
            '\u0446', '\u0447', '\u0448', '\u0449', '\u044A', '\u044B',
            '\u044C', '\u044D', '\u044E', '\u044F'
        };

        /// <summary>
        /// Convolution of the text.
        /// </summary>
        public static void Convolution
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            const int MaxLength = 64;

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            string[] words = navigator.SplitByGoodCharacters(_GoodCharacters);
            if (words.Length == 0)
            {
                return;
            }

            StringBuilder output = new StringBuilder(MaxLength);

            for (int charOffset = 0; ; charOffset++)
            {
                if (output.Length == MaxLength)
                {
                    break;
                }

                bool flag = false;
                for (
                        int wordIndex = words.Length - 1;
                        wordIndex >= 0;
                        wordIndex--
                    )
                {
                    if (charOffset < words[wordIndex].Length)
                    {
                        flag = true;
                        char c = CharUtility.ToUpperInvariant
                            (
                                words[wordIndex][charOffset]
                            );
                        output.Append(c);
                        if (output.Length == MaxLength)
                        {
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    break;
                }
            }

            if (output.Length != 0)
            {
                context.Write(node, output.ToString());
                context.OutputFlag = true;
            }
        }

        // ================================================================

        /// <summary>
        /// Post processing: cleanup double text.
        /// </summary>
        public static void CleanDoubleText
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.GetRootContext().PostProcessing |= PftCleanup.DoubleText;
        }

        // ================================================================

        /// <summary>
        /// Post processing: cleanup RTF markup.
        /// </summary>
        public static void CleanRtf
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            context.GetRootContext().PostProcessing |= PftCleanup.Rtf;
        }

        // ================================================================

        private static string _FirstEvaluator
            (
                Match match
            )
        {
            return match.Groups["first"].Value;
        }

        private static string _SecondEvaluator
            (
                Match match
            )
        {
            return match.Groups["second"].Value;
        }

        /// <summary>
        /// Decode title.
        /// </summary>
        public static void DecodeTitle
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char index = navigator.ReadChar();
                string input = navigator.GetRemainingText();
                if (!string.IsNullOrEmpty(input))
                {
                    MatchEvaluator evaluator = _FirstEvaluator;
                    if (index != '0')
                    {
                        evaluator = _SecondEvaluator;
                    }
                    string output = Regex.Replace
                        (
                            input,
                            "[<](?<first>.+?)[=](?<second>.+?)[>]",
                            evaluator
                        );
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Возвращает позицию первого символа найденного вхождения подстроки
        /// в исходную строку. Считается, что символы в строке нумеруются с 1.
        /// Если подстрока не найдена, то возвращает 0.
        /// </summary>
        /// <remarks>
        /// Формат:
        /// +9S!подстрока!исходная_строка
        /// </remarks>
        public static void FindSubstring
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                goto NOTFOUND;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char delimiter = navigator.ReadChar();
            if (delimiter == '\0')
            {
                goto NOTFOUND;
            }
            string substring = navigator.ReadUntil(delimiter);
            if (string.IsNullOrEmpty(substring)
                || navigator.ReadChar() != delimiter)
            {
                goto NOTFOUND;
            }
            string text = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(text))
            {
                goto NOTFOUND;
            }
            int position = text.IndexOf
                (
                    substring,
                    StringComparison.CurrentCultureIgnoreCase
                );
            if (position < 0)
            {
                goto NOTFOUND;
            }
            string output = (position + 1).ToInvariantString();
            context.Write(node, output);
            context.OutputFlag = true;

            return;

            NOTFOUND:
            context.Write(node, "0");
            context.OutputFlag = true;
        }

        // ================================================================

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

        // ================================================================

        /// <summary>
        /// Get current database name.
        /// </summary>
        public static void GetDatabaseName
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            string output = context.Provider.Database;
            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        /// <summary>
        /// Get field count.
        /// </summary>
        public static void GetFieldCount
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null)
                && !string.IsNullOrEmpty(expression))
            {
                int count = record.Fields
                    .GetField(expression)
                    .Length;
                string output = count.ToInvariantString();
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        /// <summary>
        /// Get field index.
        /// </summary>
        public static void GetFieldIndex
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null)
                && !string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split('#');
                string tag = parts[0];
                string occText = parts.Length > 1
                    ? parts[1]
                    : "1";
                int occ;
                if (occText == "*")
                {
                    occ = context.Index;
                }
                else if (occText == string.Empty)
                {
                    occ = 0;
                }
                else
                {
                    if (!NumericUtility.TryParseInt32(occText, out occ))
                    {
                        return;
                    }
                    occ--;
                }
                RecordField field = record.Fields
                    .GetField(tag, occ);
                if (!ReferenceEquals(field, null))
                {
                    int index = record.Fields.IndexOf(field) + 1;
                    string output = index.ToInvariantString();
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetFieldRepeat
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            try
            {
                MarcRecord record = context.Record;
                if (!ReferenceEquals(record, null))
                {
                    FieldSpecification specification = new FieldSpecification();
                    if (specification.ParseUnifor(expression))
                    {
                        FieldReference reference = new FieldReference();
                        reference.Apply(specification);

                        string result = reference.Format(record);
                        if (!string.IsNullOrEmpty(result))
                        {
                            context.Write(node, result);
                            context.OutputFlag = true;

                            if (!ReferenceEquals(context._vMonitor, null))
                            {
                                context._vMonitor.Output = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "Unifor::GetFieldRepeat",
                        exception
                    );
            }
        }

        // ================================================================

        /// <summary>
        /// Get INI-file entry.
        /// </summary>
        public static void GetIniFileEntry
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
#if PocketPC || WINMOBILE || SILVERLIGHT

                string[] parts = expression.Split(new[] { ',' });

#else

                string[] parts = expression.Split(new[] { ',' }, 3);

#endif

                if (parts.Length >= 2)
                {
                    string section = parts[0];
                    string parameter = parts[1];
                    string defaultValue = parts.Length > 2
                        ? parts[2]
                        : null;

                    if (!string.IsNullOrEmpty(section)
                        && !string.IsNullOrEmpty(parameter))
                    {
                        string result;
                        using (IniFile iniFile
                            = context.Provider.GetUserIniFile())
                        {
                            result = iniFile.GetValue
                                (
                                    section,
                                    parameter,
                                    defaultValue
                                );
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
                            context.Write(node, result);
                            context.OutputFlag = true;
                        }
                    }
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Get MNU-file entry.
        /// </summary>
        public static void GetMenuEntry
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string menuName = navigator.ReadUntil('\\', '!');
                if (string.IsNullOrEmpty(menuName))
                {
                    return;
                }
                char separator = navigator.ReadChar();
                if (separator != '\\' && separator != '!')
                {
                    return;
                }
                string key = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }
                FileSpecification specification = new FileSpecification
                        (
                            IrbisPath.MasterFile,
                            context.Provider.Database,
                            menuName
                        );
                MenuFile menu = context.Provider.ReadMenuFile
                    (
                        specification
                    );
                if (!ReferenceEquals(menu, null))
                {
                    string output = null;

                    switch (separator)
                    {
                        case '\\':
                            output = menu.GetStringSensitive(key);
                            break;

                        case '!':
                            output = menu.GetString(key);
                            break;
                    }

                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Вернуть часть строки до или начиная с заданного символа.
        /// </summary>
        /// <remarks>
        /// Формат (передаваемая строка):
        /// GNAстрока
        /// где:
        /// N может принимать значения:
        /// 0 (или A) – до заданного символа не включая его;
        /// 1 (или B) – начиная с заданного символа;
        /// 2 (или C) – после заданного символа;
        /// 3 (или D) – после последнего вхождения заданного символа;
        /// 4 (или E) – до последнего вхождения заданного символа(включая его);
        /// 5 – до последнего вхождения заданного символа(НЕ ВКЛЮЧАЯ его).
        /// А – заданный символ.Символ обозначает самого себя, кроме 
        /// # (обозначает любую цифру) и $ (обозначает любую букву).
        /// </remarks>
        public static void GetPart
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char code = navigator.ReadChar();
                char symbol = navigator.ReadChar();
                string text = navigator.GetRemainingText();
                if (!string.IsNullOrEmpty(text))
                {
                    int firstOffset = -1, lastOffset = -1;
                    string output = null;

                    switch (symbol)
                    {
                        case '#':
                            firstOffset = text.IndexOfAny(PftUtility.Digits);
                            lastOffset = text.LastIndexOfAny(PftUtility.Digits);
                            break;

                        case '$':
                            firstOffset = text.IndexOfAny(PftUtility.Letters);
                            lastOffset = text.LastIndexOfAny(PftUtility.Letters);
                            break;

                        default:
                            firstOffset = text.IndexOf(symbol);
                            lastOffset = text.LastIndexOf(symbol);
                            break;
                    }

                    switch (code)
                    {
                        case '0':
                        case 'A':
                        case 'a':
                            output = firstOffset < 0
                                ? text
                                : text.Substring(0, firstOffset);
                            break;

                        case '1':
                        case 'B':
                        case 'b':
                            output = firstOffset < 0
                                ? text
                                : text.Substring(firstOffset);
                            break;

                        case '2':
                        case 'C':
                        case 'c':
                            output = firstOffset < 0
                                ? text
                                : text.Substring(firstOffset + 1);
                            break;

                        case '3':
                        case 'D':
                        case 'd':
                            output = lastOffset < 0
                                ? text
                                : text.Substring(lastOffset + 1);
                            break;

                        case '4':
                        case 'E':
                        case 'e':
                            output = lastOffset < 0
                                ? text
                                : text.Substring(0, lastOffset + 1);
                            break;

                        case '5':
                        case 'F':
                        case 'f':
                            output = lastOffset < 0
                                ? text
                                : text.Substring(0, lastOffset);
                            break;

                        default:
                            Log.Error
                                (
                                    "Unifor::GetPart: "
                                    + "unexpected code="
                                    + code
                                );
                            break;
                    }

                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Get record status: whether the record is deleted?
        /// </summary>
        public static void GetRecordStatus
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            string result = "1";

            if (!ReferenceEquals(context.Record, null)
                && context.Record.Deleted)
            {
                result = "0";
            }

            context.Write(node, result);
            context.OutputFlag = true;
        }

        // ================================================================

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
                NumericUtility.TryParseInt32(expression, out length);
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

        // ================================================================

        /// <summary>
        /// Remove text surrounded with angle brackets.
        /// </summary>
        public static void RemoveAngleBrackets
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = Regex.Replace(expression, "<.*?>", string.Empty);
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        // ================================================================

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

        // ================================================================

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
#if PocketPC || WINMOBILE

                string output = expression.ToLower();

#else

                string output = expression.ToLowerInvariant();

#endif

                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

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
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            //
            // Attention: in original IRBIS &uf('T0ё') breaks the script
            //

            if (!string.IsNullOrEmpty(expression))
            {
                StringBuilder result = new StringBuilder();

                for (int index = 1; index < expression.Length; index++)
                {
                    char c = expression[index];
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

        // ================================================================

        /// <summary>
        /// Trim text at last dot.
        /// </summary>
        public static void TrimAtLastDot
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                int position = expression.LastIndexOf('.');
                if (position >= 0)
                {
                    string output = expression.Substring(0, position);
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        // ================================================================

        /// <summary>
        /// Get unique field value.
        /// </summary>
        public static void UniqueField
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                try
                {
                    MarcRecord record = context.Record;
                    if (!ReferenceEquals(record, null))
                    {
                        FieldSpecification specification = new FieldSpecification();
                        if (specification.ParseUnifor(expression))
                        {
                            FieldReference reference = new FieldReference();
                            reference.Apply(specification);

                            string[] array = reference.GetUniqueValues(record);
                            string result = null;
                            switch (reference.FieldRepeat.Kind)
                            {
                                case IndexKind.None:
                                    result = StringUtility.Join(",", array);
                                    break;

                                case IndexKind.Literal:
                                    result = array.GetOccurrence
                                        (
                                            reference.FieldRepeat.Literal - 1
                                        );
                                    break;

                                case IndexKind.LastRepeat:
                                    if (array.Length != 0)
                                    {
                                        result = array[array.Length - 1];
                                    }
                                    break;

                                default:
                                    throw new IrbisException
                                        (
                                            "Unexpected repeat: "
                                            + reference.FieldRepeat.Kind
                                        );
                            }

                            if (!string.IsNullOrEmpty(result))
                            {
                                context.Write(node, result);
                                context.OutputFlag = true;

                                if (!ReferenceEquals(context._vMonitor, null))
                                {
                                    context._vMonitor.Output = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                    (
                        "Unifor::UniqueField",
                        exception
                    );
                }

            }
        }


        // ================================================================

        #endregion

        #region IFormatExit members

        /// <inheritdoc cref="IFormatExit.Name" />
        public string Name { get { return "unifor"; } }

        /// <inheritdoc cref="IFormatExit.Execute" />
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
                Log.Trace
                    (
                        "Unifor::Execute: "
                        + "empty expression"
                    );

                return;
            }

            Action<PftContext, PftNode, string> action
                = FindAction(ref expression);

            if (ReferenceEquals(action, null))
            {
                Log.Error
                    (
                        "Unifor::Execute: "
                        + "unknown action="
                        + expression.ToVisibleString()
                    );

                if (ThrowOnUnknown)
                {
                    throw new PftException
                        (
                            "Unknown unifor: "
                            + expression.ToVisibleString()
                        );
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
