// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontFamily.cs -- 
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
	public enum FontPitchAndFamily
	{
        /// <summary>
        /// 
        /// </summary>
		DEFAULT_PITCH = 0,

        /// <summary>
        /// 
        /// </summary>
		FIXED_PITCH = 1,

        /// <summary>
        /// 
        /// </summary>
		VARIABLE_PITCH = 2,

        /// <summary>
        /// 
        /// </summary>
		MONO_FONT = 8,

        /// <summary>
        /// 
        /// </summary>
		FF_DONTCARE = ( 0 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		FF_ROMAN = ( 1 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		FF_SWISS = ( 2 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		FF_MODERN = ( 3 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		FF_SCRIPT = ( 4 << 4 ),

        /// <summary>
        /// 
        /// </summary>
		FF_DECORATIVE = ( 5 << 4 )
	}
}
