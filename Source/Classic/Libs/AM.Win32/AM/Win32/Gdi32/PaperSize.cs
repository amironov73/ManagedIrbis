// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PaperSize.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum PaperSize
    {
        /// <summary>
        /// 
        /// </summary>
        DMPAPER_FIRST = DMPAPER_LETTER,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER = 1,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTERSMALL = 2,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_TABLOID = 3,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LEDGER = 4,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LEGAL = 5,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_STATEMENT = 6,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_EXECUTIVE = 7,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A3 = 8,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4 = 9,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4SMALL = 10,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A5 = 11,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B4 = 12,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B5 = 13,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_FOLIO = 14,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_QUARTO = 15,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_10X14 = 16,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_11X17 = 17,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_NOTE = 18,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_9 = 19,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_10 = 20,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_11 = 21,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_12 = 22,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_14 = 23,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_CSHEET = 24,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_DSHEET = 25,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ESHEET = 26,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_DL = 27,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_C5 = 28,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_C3 = 29,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_C4 = 30,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_C6 = 31,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_C65 = 32,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_B4 = 33,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_B5 = 34,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_B6 = 35,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_ITALY = 36,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_MONARCH = 37,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_PERSONAL = 38,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_FANFOLD_US = 39,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_FANFOLD_STD_GERMAN = 40,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_FANFOLD_LGL_GERMAN = 41,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ISO_B4 = 42,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JAPANESE_POSTCARD = 43,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_9X11 = 44,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_10X11 = 45,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_15X11 = 46,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_ENV_INVITE = 47,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_RESERVED_48 = 48,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_RESERVED_49 = 49,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER_EXTRA = 50,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LEGAL_EXTRA = 51,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_TABLOID_EXTRA = 52,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4_EXTRA = 53,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER_TRANSVERSE = 54,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4_TRANSVERSE = 55,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER_EXTRA_TRANSVERSE = 56,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A_PLUS = 57,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B_PLUS = 58,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER_PLUS = 59,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4_PLUS = 60,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A5_TRANSVERSE = 61,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B5_TRANSVERSE = 62,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A3_EXTRA = 63,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A5_EXTRA = 64,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B5_EXTRA = 65,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A2 = 66,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A3_TRANSVERSE = 67,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A3_EXTRA_TRANSVERSE = 68,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_DBL_JAPANESE_POSTCARD = 69,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A6 = 70,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_KAKU2 = 71,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_KAKU3 = 72,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_CHOU3 = 73,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_CHOU4 = 74,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LETTER_ROTATED = 75,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A3_ROTATED = 76,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A4_ROTATED = 77,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A5_ROTATED = 78,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B4_JIS_ROTATED = 79,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B5_JIS_ROTATED = 80,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JAPANESE_POSTCARD_ROTATED = 81,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_A6_ROTATED = 83,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_KAKU2_ROTATED = 84,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_KAKU3_ROTATED = 85,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_CHOU3_ROTATED = 86,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_CHOU4_ROTATED = 87,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B6_JIS = 88,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_B6_JIS_ROTATED = 89,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_12X11 = 90,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_YOU4 = 91,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_JENV_YOU4_ROTATED = 92,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P16K = 93,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P32K = 94,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P32KBIG = 95,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_1 = 96,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_2 = 97,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_3 = 98,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_4 = 99,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_5 = 100,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_6 = 101,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_7 = 102,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_8 = 103,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_9 = 104,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_10 = 105,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P16K_ROTATED = 106,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P32K_ROTATED = 107,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_P32KBIG_ROTATED = 108,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_1_ROTATED = 109,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_2_ROTATED = 110,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_3_ROTATED = 111,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_4_ROTATED = 112,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_5_ROTATED = 113,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_6_ROTATED = 114,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_7_ROTATED = 115,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_8_ROTATED = 116,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_9_ROTATED = 117,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_PENV_10_ROTATED = 118,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_LAST = DMPAPER_PENV_10_ROTATED,

        /// <summary>
        /// 
        /// </summary>
        DMPAPER_USER = 256
    }
}
