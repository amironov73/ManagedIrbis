// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClippingPrecision.cs -- 
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
	public enum ClippingPrecision
	{
        /// <summary>
        /// 
        /// </summary>
		CLIP_DEFAULT_PRECIS = 0,

        /// <summary>
        /// 
        /// </summary>
		CLIP_CHARACTER_PRECIS = 1,

        /// <summary>
        /// 
        /// </summary>
		CLIP_STROKE_PRECIS = 2,

        /// <summary>
        /// 
        /// </summary>
		CLIP_MASK = 0xf,

        /// <summary>
        /// 
        /// </summary>
		CLIP_LH_ANGLES = ( 1 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		CLIP_TT_ALWAYS = ( 2 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		CLIP_EMBEDDED = ( 8 << 4 )
	}
}
