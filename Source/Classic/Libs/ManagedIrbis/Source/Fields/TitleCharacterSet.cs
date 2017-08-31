// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TitleCharacterSet.cs -- 
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
    /// Графика заглавия согласно grz.mnu.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TitleCharacterSet
    {
        #region Constants

        /// <summary>
        /// Латинская.
        /// </summary>
        public const string Latin = "ba";

        /// <summary>
        /// Кириллическая.
        /// </summary>
        public const string Cyrillic = "ca";

        /// <summary>
        /// Японская неопределенная.
        /// </summary>
        public const string JapaneseUndefined = "da";

        /// <summary>
        /// Японская - канджи.
        /// </summary>
        public const string JapaneseKanji = "db";

        /// <summary>
        /// Японская - кана.
        /// </summary>
        public const string JapaneseKana = "dc";

        /// <summary>
        /// Китайская.
        /// </summary>
        public const string Chinese = "ea";

        /// <summary>
        /// Арабская.
        /// </summary>
        public const string Arab = "fa";

        /// <summary>
        /// Греческая.
        /// </summary>
        public const string Greek = "ga";

        /// <summary>
        /// Иврит.
        /// </summary>
        public const string Hebrew = "ha";

        /// <summary>
        /// Тайская.
        /// </summary>
        public const string Thai = "ia";

        /// <summary>
        /// Девангари.
        /// </summary>
        public const string Devanagari = "ja";

        /// <summary>
        /// Корейская.
        /// </summary>
        public const string Korean = "ka";

        /// <summary>
        /// Тамильская.
        /// </summary>
        public const string Tamil = "la";

        /// <summary>
        /// Грузинская.
        /// </summary>
        public const string Georgian = "ma";

        /// <summary>
        /// Армянская.
        /// </summary>
        public const string Armenian = "mb";

        /// <summary>
        /// Другая.
        /// </summary>
        public const string Other = "zz";

        #endregion
    }
}
