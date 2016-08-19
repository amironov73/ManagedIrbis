/* DeviceOrientation.cs -- 
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
	public enum DeviceOrientation
	{
        /// <summary>
        /// 
        /// </summary>
		DMORIENT_PORTRAIT = 1,

        /// <summary>
        /// 
        /// </summary>
		DMORIENT_LANDSCAPE = 2,

        /// <summary>
        /// 
        /// </summary>
		DMDO_DEFAULT = 0,

        /// <summary>
        /// 
        /// </summary>
		DMDO_90 = 1,

        /// <summary>
        /// 
        /// </summary>
		DMDO_180 = 2,

        /// <summary>
        /// 
        /// </summary>
		DMDO_270 = 3
	}
}
