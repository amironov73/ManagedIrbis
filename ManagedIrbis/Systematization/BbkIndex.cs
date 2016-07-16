/* BbkIndex.cs -- классификационный индекс ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM.Collections;

using CodeJam;

using Compatibility;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Systematization
{
    /// <summary>
    /// Классификационный индекс ББК,
    /// разложенный по элементам.
    /// </summary>
    [PublicAPI]
    [XmlRoot("bbk")]
    [MoonSharpUserData]
    [DebuggerDisplay("{MainIndex}")]
    public sealed class BbkIndex
    {
        #region Properties

        /// <summary>
        /// Основной индекс.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("main-index")]
        [JsonProperty("main-index")]
        public string MainIndex { get; set; }

        /// <summary>
        /// Территориальные типовые деления.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("territorial-index")]
        [JsonProperty("territorial-index")]
        public string TerritorialIndex { get; set; }

        /// <summary>
        /// Специальные типовые деления.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlAttribute("special-index")]
        [JsonProperty("special-index")]
        public NonNullCollection<string> SpecialIndex { get; set; }

        /// <summary>
        /// Код социальной системы.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("social-index")]
        [JsonProperty("social-index")]
        public string SocialIndex { get; set; }

        /// <summary>
        /// Комбинированный индекс.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("combined-index")]
        [JsonProperty("combined-index")]
        public string CombinedIndex { get; set; }

        /// <summary>
        /// Определители.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public NonNullCollection<string> Qualifiers { get; private set; }

        /// <summary>
        /// Некая хрень.
        /// </summary>
        [CanBeNull]
        public string Hren { get; set; }

        /// <summary>
        /// Запятая???
        /// </summary>
        [CanBeNull]
        public string Comma { get; set; }

        #endregion

        #region Construciton

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BbkIndex()
        {
            Qualifiers = new NonNullCollection<string>();
            SpecialIndex = new NonNullCollection<string>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BbkIndex
            (
                [CanBeNull] string mainIndex
            )
            : this()
        {
            MainIndex = mainIndex;
        }

        #endregion

        #region Private members

        private static readonly char[] _allowedSymbols =
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '.', '/'
        };

        private static readonly char[] _qualifierSymbols =
        {
            'в', 'г', 'д', 'е', 'ж', 'и', 'к', 'л', 
            'м', 'н', 'п', 'р', 'с', 'т', 'у', 'ф', 
            'ц', 'ю', 'я'
        };

        private static int _ParseSimple
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            for (; offset < length; offset++)
            {
                char c = text[offset];
                if (Array.IndexOf(_allowedSymbols, c) < 0)
                {
                    break;
                }
                result.Append(c);
            }

            return offset;
        }

        private static int _ParseCombined
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == ':')
                {
                    result.Append(':');
                    offset = _ParseSimple(text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        throw new BbkException
                            (
                                "Неверный комбинированный индекс"
                            );
                    }
                }
            }

            return offset;
        }

        private static int _ParseTerritorial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '(')
                {
                    result.Append('(');
                    offset++;
                    while (offset < length)
                    {
                        char c = text[offset];
                        offset++;
                        if (c == ')')
                        {
                            result.Append(')');
                            break;
                        }
                        result.Append(c);
                    }

                    if (result[result.Length - 1] != ')')
                    {
                        throw new BbkException
                            (
                                "Незакрытая скобка в территориальном делении"
                            );
                    }
                }
            }

            return offset;
        }

        static int _ParseQualifier
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                char c = text[offset];
                if (Array.IndexOf(_qualifierSymbols, c) >= 0)
                {
                    result.Append(c);
                    if (c == 'д')
                    {
                        offset++;
                        while (offset < length)
                        {
                            result.Append(text[offset]);
                            offset++;
                        }
                    }
                    else
                    {
                        offset = _ParseSimple(text, ++offset, length, result);
                    }
                }
            }

            return offset;
        }

        static int _ParseSpecial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '-')
                {
                    result.Append('-');
                    offset = _ParseSimple(text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        throw new BbkException
                            (
                            "Неверный код специального типового деления"
                            );
                    }
                }
            }

            return offset;
        }

        static int _ParseHren
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (char.IsDigit(text, offset))
                {
                    offset = _ParseSimple(text, offset, length, result);
                }
            }
            return offset;
        }

        static int _ParseComma
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == ',')
                {
                    result.Append(text[offset]);
                    offset = _ParseSimple(text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        throw new BbkException
                            (
                                "Неверно сформированный индекс"
                            );
                    }
                }
            }
            return offset;
        }

        static int _ParseSocial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '\'')
                {
                    result.Append('\'');
                    offset = _ParseSimple(text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        throw new BbkException
                            (
                            "Неверный код социальной системы"
                            );
                    }
                }
            }

            return offset;
        }

        private static string _NonEmpty
            (
                StringBuilder builder
            )
        {
            return (builder.Length == 0)
                ? null
                : builder.ToString();
        }

        private static string _Verify
            (
                string text,
                int skip
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string copy = text;
            char c = copy[0];
            if (c == '(')
            {
                return text;
            }
            if (skip != 0)
            {
                copy = copy.Substring(skip);
            }
            if (copy[copy.Length - 1] == '.')
            {
                throw new BbkException
                    (
                        "Индекс заканчивается точкой"
                    );
            }

            int length = copy.Length;
            if (length > 2)
            {
                if (copy[2] != '.')
                {
                    throw new BbkException
                        (
                            "Индекс должен начинаться с двузначной группы"
                        );
                }
            }

            int offset = 3;
            int count = 0;

            while (offset < length)
            {
                if ((copy[offset] == '.')
                    || (copy[offset] == '/'))
                {
                    if (count == 0)
                    {
                        throw new BbkException
                            (
                                "Два разделителя подряд"
                            );
                    }
                    if (count > 3)
                    {
                        throw new BbkException
                            (
                                "Слишком длинная группа"
                            );
                    }
                    count = 0;
                }
                else
                {
                    count++;
                    if (count > 3)
                    {
                        throw new BbkException
                            (
                                "Слишком длинная группа"
                            );
                    }
                }
                offset++;
            }

            return text;
        }

        private static void _Dump
            (
                TextWriter writer,
                string name,
                string value,
                string prefix
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteLine
                    (
                        "{0}{1}: {2}",
                        prefix,
                        name,
                        value
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстовой строки.
        /// </summary>
        [NotNull]
        public static BbkIndex Parse
        (
            [NotNull] string text
        )
        {
            Code.NotNull(text, "text");

            BbkIndex result = new BbkIndex();

            if (string.IsNullOrEmpty(text))
            {
                throw new BbkException("Пустой индекс ББК");
            }

            int length = text.Length;
            if (length < 2)
            {
                throw new BbkException
                    (
                        "ББК не может содержать меньше двух символов"
                    );
            }

            if (!char.IsDigit(text, 0)
                || !char.IsDigit(text, 1))
            {
                throw new BbkException
                    (
                        "Первые два символа ББК должны быть цифрами"
                    );
            }

            int offset = 2;
            StringBuilder accumulator = new StringBuilder();
            accumulator.Append(text, 0, 2);
            offset = _ParseSimple(text, offset, length, accumulator);
            result.MainIndex = _Verify(accumulator.ToString(), 0);

            while (offset < length)
            {
                int previousOffset = offset;

                // Ищем комбинированный индекс
                accumulator.Clear();
                offset = _ParseCombined(text, offset, length, accumulator);
                result.CombinedIndex = _Verify(_NonEmpty(accumulator), 1);

                // Ищем территориальный
                accumulator.Clear();
                offset = _ParseTerritorial(text, offset, length, accumulator);
                result.TerritorialIndex = _NonEmpty(accumulator);

                // Запятая
                accumulator.Clear();
                offset = _ParseComma(text, offset, length, accumulator);
                result.Comma = _NonEmpty(accumulator);

                // Неведомая хрень
                accumulator.Clear();
                offset = _ParseHren(text, offset, length, accumulator);
                result.Hren = _NonEmpty(accumulator);

                // Определитель
                accumulator.Clear();
                offset = _ParseQualifier(text, offset, length, accumulator);
                string qualifier = _NonEmpty(accumulator);
                if (!string.IsNullOrEmpty(qualifier))
                {
                    result.Qualifiers.Add(qualifier);
                }

                // Специальное типовое деление
                accumulator.Clear();
                offset = _ParseSpecial(text, offset, length, accumulator);
                string specialIndex = _NonEmpty(accumulator);
                if (!string.IsNullOrEmpty(specialIndex))
                {
                    result.SpecialIndex.Add(specialIndex);
                }

                // Код социальной системы
                accumulator.Clear();
                offset = _ParseSocial(text, offset, length, accumulator);
                result.SocialIndex = _NonEmpty(accumulator);

                if (offset == previousOffset)
                {
                    throw new BbkException
                        (
                            "Нераспознанные символы начиная с '"
                            + text.Substring(offset)
                            + "'"
                        );
                }
            }

            return result;
        }

        /// <summary>
        /// Дамп
        /// </summary>
        public void Dump
            (
                TextWriter writer,
                string prefix
            )
        {
            _Dump(writer, "Основной индекс", MainIndex, prefix);
            _Dump(writer, "Комбинированный индекс", CombinedIndex, prefix);
            _Dump(writer, "Территориальное деление", TerritorialIndex, prefix);
            _Dump(writer, "Некая хрень", Hren, prefix);
            _Dump(writer, "Запятая", Comma, prefix);
            foreach (string qualifier in Qualifiers)
            {
                _Dump(writer, "Определитель", qualifier, prefix);
            }
            foreach (string specialIndex in SpecialIndex)
            {
                _Dump(writer, "Специальное деление", specialIndex, prefix);
            }            
            _Dump(writer, "Социальная система ", SocialIndex, prefix);
        }

        #endregion

        #region Object members

        #endregion
    }
}
