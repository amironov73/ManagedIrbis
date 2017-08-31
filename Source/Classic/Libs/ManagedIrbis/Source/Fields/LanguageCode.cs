// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LanguageCode.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коды языков в соответствии с jz.mnu.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class LanguageCode
    {
        #region Constants

        /// <summary>
        /// Английский.
        /// </summary>
        public const string English = "eng";

        /// <summary>
        /// Белорусский.
        /// </summary>
        public const string Belorussian = "bel";

        /// <summary>
        /// Бурятский.
        /// </summary>
        public const string Buryat = "bua";

        /// <summary>
        /// Испанский.
        /// </summary>
        public const string Spanish = "spa";

        /// <summary>
        /// Итальянский.
        /// </summary>
        public const string Italian = "ita";

        /// <summary>
        /// Казахский.
        /// </summary>
        public const string Kazakh = "kaz";

        /// <summary>
        /// Китайский.
        /// </summary>
        public const string Chinese = "chi";

        /// <summary>
        /// Корейский.
        /// </summary>
        public const string Korean = "kor";

        /// <summary>
        /// Латинский.
        /// </summary>
        public const string Latin = "lat";

        /// <summary>
        /// Многоязычное издание.
        /// </summary>
        public const string Multilanguage = "mul";

        /// <summary>
        /// Не определено.
        /// </summary>
        public const string Undefined = "und";

        /// <summary>
        /// Немецкий.
        /// </summary>
        public const string German = "ger";

        /// <summary>
        /// Польский.
        /// </summary>
        public const string Polish = "pol";

        /// <summary>
        /// Португальский.
        /// </summary>
        public const string Portuguese = "por";

        /// <summary>
        /// Румынский.
        /// </summary>
        public const string Rumanian = "rum";

        /// <summary>
        /// Русский.
        /// </summary>
        public const string Russian = "rus";

        /// <summary>
        /// Татарский.
        /// </summary>
        public const string Tartarian = "tar";

        /// <summary>
        /// Украинский.
        /// </summary>
        public const string Ukrainian = "ukr";

        /// <summary>
        /// Французский.
        /// </summary>
        public const string French = "fre";

        /// <summary>
        /// Хинди.
        /// </summary>
        public const string Hindi = "hin";

        /// <summary>
        /// Чешский.
        /// </summary>
        public const string Czech = "cze";

        /// <summary>
        /// Японский.
        /// </summary>
        public const string Japanese = "jpn";

        #endregion
    }
}
