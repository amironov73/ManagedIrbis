// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextAlign.cs -- 
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
	public enum TextAlign
	{
        /// <summary>
        /// 
        /// </summary>
		TA_NOUPDATECP = 0,

        /// <summary>
        /// 
        /// </summary>
		TA_UPDATECP = 1,

        /// <summary>
        /// 
        /// </summary>
		TA_LEFT = 0,

        /// <summary>
        /// 
        /// </summary>
		TA_RIGHT = 2,

        /// <summary>
        /// 
        /// </summary>
		TA_CENTER = 6,

        /// <summary>
        /// 
        /// </summary>
		TA_TOP = 0,

        /// <summary>
        /// 
        /// </summary>
		TA_BOTTOM = 8,

        /// <summary>
        /// 
        /// </summary>
		TA_BASELINE = 24,

        /// <summary>
        /// 
        /// </summary>
		TA_RTLREADING = 256,

        /// <summary>
        /// 
        /// </summary>
		TA_MASK = ( TA_BASELINE + TA_CENTER + TA_UPDATECP + TA_RTLREADING ),

        /// <summary>
        /// 
        /// </summary>
		VTA_BASELINE = TA_BASELINE,

        /// <summary>
        /// 
        /// </summary>
		VTA_LEFT = TA_BOTTOM,

        /// <summary>
        /// 
        /// </summary>
		VTA_RIGHT = TA_TOP,

        /// <summary>
        /// 
        /// </summary>
		VTA_CENTER = TA_CENTER,

        /// <summary>
        /// 
        /// </summary>
		VTA_BOTTOM = TA_RIGHT,

        /// <summary>
        /// 
        /// </summary>
		VTA_TOP = TA_LEFT
	}
}
