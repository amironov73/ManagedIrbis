// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontWeight.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum FontWeight
	{
        /// <summary>
        /// 
        /// </summary>
		FW_DONTCARE = 0,

        /// <summary>
        /// 
        /// </summary>
		FW_THIN = 100,

        /// <summary>
        /// 
        /// </summary>
		FW_EXTRALIGHT = 200,

        /// <summary>
        /// 
        /// </summary>
		FW_LIGHT = 300,

        /// <summary>
        /// 
        /// </summary>
		FW_NORMAL = 400,

        /// <summary>
        /// 
        /// </summary>
		FW_MEDIUM = 500,

        /// <summary>
        /// 
        /// </summary>
		FW_SEMIBOLD = 600,

        /// <summary>
        /// 
        /// </summary>
		FW_BOLD = 700,

        /// <summary>
        /// 
        /// </summary>
		FW_EXTRABOLD = 800,

        /// <summary>
        /// 
        /// </summary>
		FW_HEAVY = 900,

        /// <summary>
        /// 
        /// </summary>
		FW_ULTRALIGHT = FW_EXTRALIGHT,

        /// <summary>
        /// 
        /// </summary>
		FW_REGULAR = FW_NORMAL,

        /// <summary>
        /// 
        /// </summary>
		FW_DEMIBOLD = FW_SEMIBOLD,

        /// <summary>
        /// 
        /// </summary>
		FW_ULTRABOLD = FW_EXTRABOLD,

        /// <summary>
        /// 
        /// </summary>
		FW_BLACK = FW_HEAVY
	}
}
