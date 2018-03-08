// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontCharset.cs -- character set for CreateFont method
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Character set for CreateFont method.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum FontCharset
    {
        /// <summary>
        /// ANSI.
        /// </summary>
        ANSI_CHARSET = 0,

        /// <summary>
        /// Currently used charset.
        /// </summary>
        DEFAULT_CHARSET = 1,

        /// <summary>
        /// Symbol characters.
        /// </summary>
        SYMBOL_CHARSET = 2,

        /// <summary>
        /// 
        /// </summary>
        SHIFTJIS_CHARSET = 128,

        /// <summary>
        /// 
        /// </summary>
        HANGEUL_CHARSET = 129,

        /// <summary>
        /// 
        /// </summary>
        HANGUL_CHARSET = 129,

        /// <summary>
        /// 
        /// </summary>
        GB2312_CHARSET = 134,

        /// <summary>
        /// 
        /// </summary>
        CHINESEBIG5_CHARSET = 136,

        /// <summary>
        /// 
        /// </summary>
        OEM_CHARSET = 255,

        /// <summary>
        /// 
        /// </summary>
        JOHAB_CHARSET = 130,

        /// <summary>
        /// 
        /// </summary>
        HEBREW_CHARSET = 177,

        /// <summary>
        /// 
        /// </summary>
        ARABIC_CHARSET = 178,

        /// <summary>
        /// 
        /// </summary>
        GREEK_CHARSET = 161,

        /// <summary>
        /// 
        /// </summary>
        TURKISH_CHARSET = 162,

        /// <summary>
        /// 
        /// </summary>
        VIETNAMESE_CHARSET = 163,

        /// <summary>
        /// 
        /// </summary>
        THAI_CHARSET = 222,

        /// <summary>
        /// 
        /// </summary>
        EASTEUROPE_CHARSET = 238,

        /// <summary>
        /// 
        /// </summary>
        RUSSIAN_CHARSET = 204,

        /// <summary>
        /// 
        /// </summary>
        MAC_CHARSET = 77,

        /// <summary>
        /// 
        /// </summary>
        BALTIC_CHARSET = 186
    }
}
