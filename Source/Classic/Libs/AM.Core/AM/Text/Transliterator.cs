// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Transliterator.cs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Транслитерируем кириллицу в латиницу.
    /// </summary>
    [PublicAPI]
    public static class Transliterator
    {
        #region Construction

        // ReSharper disable once InconsistentNaming
        // ГОСТ 16876-71
        private static readonly Dictionary<string, string> _gost;

        #endregion

        #region Private members

        static Transliterator()
        {
            _gost = new Dictionary<string, string>
            {
                {"Є", "EH"},
                {"І", "I"},
                {"і", "i"},
                {"№", "#"},
                {"є", "eh"},
                {"А", "A"},
                {"Б", "B"},
                {"В", "V"},
                {"Г", "G"},
                {"Д", "D"},
                {"Е", "E"},
                {"Ё", "JO"},
                {"Ж", "ZH"},
                {"З", "Z"},
                {"И", "I"},
                {"Й", "JJ"},
                {"К", "K"},
                {"Л", "L"},
                {"М", "M"},
                {"Н", "N"},
                {"О", "O"},
                {"П", "P"},
                {"Р", "R"},
                {"С", "S"},
                {"Т", "T"},
                {"У", "U"},
                {"Ф", "F"},
                {"Х", "KH"},
                {"Ц", "C"},
                {"Ч", "CH"},
                {"Ш", "SH"},
                {"Щ", "SHH"},
                {"Ъ", "'"},
                {"Ы", "Y"},
                {"Ь", ""},
                {"Э", "EH"},
                {"Ю", "YU"},
                {"Я", "YA"},
                {"а", "a"},
                {"б", "b"},
                {"в", "v"},
                {"г", "g"},
                {"д", "d"},
                {"е", "e"},
                {"ё", "jo"},
                {"ж", "zh"},
                {"з", "z"},
                {"и", "i"},
                {"й", "jj"},
                {"к", "k"},
                {"л", "l"},
                {"м", "m"},
                {"н", "n"},
                {"о", "o"},
                {"п", "p"},
                {"р", "r"},
                {"с", "s"},
                {"т", "t"},
                {"у", "u"},
                {"ф", "f"},
                {"х", "kh"},
                {"ц", "c"},
                {"ч", "ch"},
                {"ш", "sh"},
                {"щ", "shh"},
                {"ъ", ""},
                {"ы", "y"},
                {"ь", ""},
                {"э", "eh"},
                {"ю", "yu"},
                {"я", "ya"},
                {"«", ""},
                {"»", ""},
                {"—", "-"},
                {" ", "-"}
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Transliterates the specified text.
        /// </summary>
        [CanBeNull]
        public static string Transliterate
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                string key = new string(c, 1);
                string value;
                if (!_gost.TryGetValue(key, out value))
                {
                    value = key;
                }
                result.Append(value);
            }

            return result.ToString();
        }

        #endregion
    }
}
