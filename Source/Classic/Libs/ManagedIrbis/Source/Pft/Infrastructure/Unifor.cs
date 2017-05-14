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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

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
            Registry.Add("D", UniforD.FormatDocumentDB);
            Registry.Add("E", UniforE.GetFirstWords);
            Registry.Add("F", UniforE.GetLastWords);
            Registry.Add("I", GetIniFileEntry);
            Registry.Add("J", UniforJ.GetTermRecordCountDB);
            Registry.Add("K", GetMenuEntry);
            Registry.Add("M", UniforM.Sort);
            Registry.Add("O", UniforO.AllExemplars);
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
        /// Post processing: cleanup double text.
        /// </summary>
        public static void CleanDoubleText
            (
                PftContext context,
                PftNode node,
                string expression
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

        private static string _FirstEvaluator(Match match)
        {
            return match.Groups["first"].Value;
        }

        private static string _SecondEvaluator(Match match)
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
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Eat the exception
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

                for (int index = 1; index < expression.Length; index++ )
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
