/* OutputPrecision.cs -- 
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
	public enum OutputPrecision
	{
        /// <summary>
        /// 
        /// </summary>
		OUT_DEFAULT_PRECIS = 0,

        /// <summary>
        /// 
        /// </summary>
		OUT_STRING_PRECIS = 1,

        /// <summary>
        /// 
        /// </summary>
		OUT_CHARACTER_PRECIS = 2,

        /// <summary>
        /// 
        /// </summary>
		OUT_STROKE_PRECIS = 3,

        /// <summary>
        /// 
        /// </summary>
		OUT_TT_PRECIS = 4,

        /// <summary>
        /// 
        /// </summary>
		OUT_DEVICE_PRECIS = 5,

        /// <summary>
        /// 
        /// </summary>
		OUT_RASTER_PRECIS = 6,

        /// <summary>
        /// 
        /// </summary>
		OUT_TT_ONLY_PRECIS = 7,

        /// <summary>
        /// 
        /// </summary>
		OUT_OUTLINE_PRECIS = 8,

        /// <summary>
        /// 
        /// </summary>
		OUT_SCREEN_OUTLINE_PRECIS = 9,

        /// <summary>
        /// 
        /// </summary>
		OUT_PS_ONLY_PRECIS = 10
	}
}
