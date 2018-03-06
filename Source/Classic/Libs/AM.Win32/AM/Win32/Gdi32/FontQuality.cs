// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontQuality.cs -- 
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
	public enum FontQuality
	{
        /// <summary>
        /// 
        /// </summary>
		DEFAULT_QUALITY = 0,

        /// <summary>
        /// 
        /// </summary>
		DRAFT_QUALITY = 1,

        /// <summary>
        /// 
        /// </summary>
		PROOF_QUALITY = 2,

        /// <summary>
        /// 
        /// </summary>
		NONANTIALIASED_QUALITY = 3,

        /// <summary>
        /// 
        /// </summary>
		ANTIALIASED_QUALITY = 4,

        /// <summary>
        /// 
        /// </summary>
		CLEARTYPE_QUALITY = 5,

        /// <summary>
        /// 
        /// </summary>
		CLEARTYPE_NATURAL_QUALITY = 6
	}
}
