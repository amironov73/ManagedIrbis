/* FontCharset.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum FontCharset
	{
        /// <summary>
        /// 
        /// </summary>
		ANSI_CHARSET = 0,

        /// <summary>
        /// 
        /// </summary>
		DEFAULT_CHARSET = 1,

        /// <summary>
        /// 
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
