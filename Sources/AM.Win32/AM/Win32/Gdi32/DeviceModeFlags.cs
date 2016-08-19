/* DeviceModeFlags.cs -- 
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
	public enum DeviceModeFlags
	{
        /// <summary>
        /// 
        /// </summary>
		DM_ORIENTATION = 0x00000001,

        /// <summary>
        /// 
        /// </summary>
		DM_PAPERSIZE = 0x00000002,

        /// <summary>
        /// 
        /// </summary>
		DM_PAPERLENGTH = 0x00000004,

        /// <summary>
        /// 
        /// </summary>
		DM_PAPERWIDTH = 0x00000008,

        /// <summary>
        /// 
        /// </summary>
		DM_SCALE = 0x00000010,

        /// <summary>
        /// 
        /// </summary>
		DM_POSITION = 0x00000020,

        /// <summary>
        /// 
        /// </summary>
		DM_NUP = 0x00000040,

        /// <summary>
        /// 
        /// </summary>
		DM_DISPLAYORIENTATION = 0x00000080,

        /// <summary>
        /// 
        /// </summary>
		DM_COPIES = 0x00000100,

        /// <summary>
        /// 
        /// </summary>
		DM_DEFAULTSOURCE = 0x00000200,

        /// <summary>
        /// 
        /// </summary>
		DM_PRINTQUALITY = 0x00000400,

        /// <summary>
        /// 
        /// </summary>
		DM_COLOR = 0x00000800,

        /// <summary>
        /// 
        /// </summary>
		DM_DUPLEX = 0x00001000,

        /// <summary>
        /// 
        /// </summary>
		DM_YRESOLUTION = 0x00002000,

        /// <summary>
        /// 
        /// </summary>
		DM_TTOPTION = 0x00004000,

        /// <summary>
        /// 
        /// </summary>
		DM_COLLATE = 0x00008000,

        /// <summary>
        /// 
        /// </summary>
		DM_FORMNAME = 0x00010000,

        /// <summary>
        /// 
        /// </summary>
		DM_LOGPIXELS = 0x00020000,

        /// <summary>
        /// 
        /// </summary>
		DM_BITSPERPEL = 0x00040000,

        /// <summary>
        /// 
        /// </summary>
		DM_PELSWIDTH = 0x00080000,

        /// <summary>
        /// 
        /// </summary>
		DM_PELSHEIGHT = 0x00100000,

        /// <summary>
        /// 
        /// </summary>
		DM_DISPLAYFLAGS = 0x00200000,

        /// <summary>
        /// 
        /// </summary>
		DM_DISPLAYFREQUENCY = 0x00400000,

        /// <summary>
        /// 
        /// </summary>
		DM_ICMMETHOD = 0x00800000,

        /// <summary>
        /// 
        /// </summary>
		DM_ICMINTENT = 0x01000000,

        /// <summary>
        /// 
        /// </summary>
		DM_MEDIATYPE = 0x02000000,

        /// <summary>
        /// 
        /// </summary>
		DM_DITHERTYPE = 0x04000000,

        /// <summary>
        /// 
        /// </summary>
		DM_PANNINGWIDTH = 0x08000000,

        /// <summary>
        /// 
        /// </summary>
		DM_PANNINGHEIGHT = 0x10000000,

        /// <summary>
        /// 
        /// </summary>
		DM_DISPLAYFIXEDOUTPUT = 0x20000000,
	}
}
