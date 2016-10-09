/* SystemMetricsFlags.cs -- 
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
	public enum SystemMetricsFlags
	{
        /// <summary>
        /// 
        /// </summary>
		SM_CXSCREEN = 0,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSCREEN = 1,

        /// <summary>
        /// 
        /// </summary>
		SM_CXVSCROLL = 2,

        /// <summary>
        /// 
        /// </summary>
		SM_CYHSCROLL = 3,

        /// <summary>
        /// 
        /// </summary>
		SM_CYCAPTION = 4,

        /// <summary>
        /// 
        /// </summary>
		SM_CXBORDER = 5,

        /// <summary>
        /// 
        /// </summary>
		SM_CYBORDER = 6,

        /// <summary>
        /// 
        /// </summary>
		SM_CXDLGFRAME = 7,

        /// <summary>
        /// 
        /// </summary>
		SM_CYDLGFRAME = 8,

        /// <summary>
        /// 
        /// </summary>
		SM_CYVTHUMB = 9,

        /// <summary>
        /// 
        /// </summary>
		SM_CXHTHUMB = 10,

        /// <summary>
        /// 
        /// </summary>
		SM_CXICON = 11,

        /// <summary>
        /// 
        /// </summary>
		SM_CYICON = 12,

        /// <summary>
        /// 
        /// </summary>
		SM_CXCURSOR = 13,

        /// <summary>
        /// 
        /// </summary>
		SM_CYCURSOR = 14,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMENU = 15,

        /// <summary>
        /// 
        /// </summary>
		SM_CXFULLSCREEN = 16,

        /// <summary>
        /// 
        /// </summary>
		SM_CYFULLSCREEN = 17,

        /// <summary>
        /// 
        /// </summary>
		SM_CYKANJIWINDOW = 18,

        /// <summary>
        /// 
        /// </summary>
		SM_MOUSEPRESENT = 19,

        /// <summary>
        /// 
        /// </summary>
		SM_CYVSCROLL = 20,

        /// <summary>
        /// 
        /// </summary>
		SM_CXHSCROLL = 21,

        /// <summary>
        /// 
        /// </summary>
		SM_DEBUG = 22,

        /// <summary>
        /// 
        /// </summary>
		SM_SWAPBUTTON = 23,

        /// <summary>
        /// 
        /// </summary>
		SM_RESERVED1 = 24,

        /// <summary>
        /// 
        /// </summary>
		SM_RESERVED2 = 25,

        /// <summary>
        /// 
        /// </summary>
		SM_RESERVED3 = 26,

        /// <summary>
        /// 
        /// </summary>
		SM_RESERVED4 = 27,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMIN = 28,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMIN = 29,

        /// <summary>
        /// 
        /// </summary>
		SM_CXSIZE = 30,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSIZE = 31,

        /// <summary>
        /// 
        /// </summary>
		SM_CXFRAME = 32,

        /// <summary>
        /// 
        /// </summary>
		SM_CYFRAME = 33,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMINTRACK = 34,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMINTRACK = 35,

        /// <summary>
        /// 
        /// </summary>
		SM_CXDOUBLECLK = 36,

        /// <summary>
        /// 
        /// </summary>
		SM_CYDOUBLECLK = 37,

        /// <summary>
        /// 
        /// </summary>
		SM_CXICONSPACING = 38,

        /// <summary>
        /// 
        /// </summary>
		SM_CYICONSPACING = 39,

        /// <summary>
        /// 
        /// </summary>
		SM_MENUDROPALIGNMENT = 40,

        /// <summary>
        /// 
        /// </summary>
		SM_PENWINDOWS = 41,

        /// <summary>
        /// 
        /// </summary>
		SM_DBCSENABLED = 42,

        /// <summary>
        /// 
        /// </summary>
		SM_CMOUSEBUTTONS = 43,

        /// <summary>
        /// 
        /// </summary>
		SM_CXFIXEDFRAME = SM_CXDLGFRAME,

        /// <summary>
        /// 
        /// </summary>
		SM_CYFIXEDFRAME = SM_CYDLGFRAME,

        /// <summary>
        /// 
        /// </summary>
		SM_CXSIZEFRAME = SM_CXFRAME,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSIZEFRAME = SM_CYFRAME,

        /// <summary>
        /// 
        /// </summary>
		SM_SECURE = 44,

        /// <summary>
        /// 
        /// </summary>
		SM_CXEDGE = 45,

        /// <summary>
        /// 
        /// </summary>
		SM_CYEDGE = 46,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMINSPACING = 47,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMINSPACING = 48,

        /// <summary>
        /// 
        /// </summary>
		SM_CXSMICON = 49,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSMICON = 50,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSMCAPTION = 51,

        /// <summary>
        /// 
        /// </summary>
		SM_CXSMSIZE = 52,

        /// <summary>
        /// 
        /// </summary>
		SM_CYSMSIZE = 53,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMENUSIZE = 54,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMENUSIZE = 55,

        /// <summary>
        /// 
        /// </summary>
		SM_ARRANGE = 56,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMINIMIZED = 57,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMINIMIZED = 58,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMAXTRACK = 59,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMAXTRACK = 60,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMAXIMIZED = 61,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMAXIMIZED = 62,

        /// <summary>
        /// 
        /// </summary>
		SM_NETWORK = 63,

        /// <summary>
        /// 
        /// </summary>
		SM_CLEANBOOT = 67,

        /// <summary>
        /// 
        /// </summary>
		SM_CXDRAG = 68,

        /// <summary>
        /// 
        /// </summary>
		SM_CYDRAG = 69,

        /// <summary>
        /// 
        /// </summary>
		SM_SHOWSOUNDS = 70,

        /// <summary>
        /// 
        /// </summary>
		SM_CXMENUCHECK = 71,

        /// <summary>
        /// 
        /// </summary>
		SM_CYMENUCHECK = 72,

        /// <summary>
        /// 
        /// </summary>
		SM_SLOWMACHINE = 73,

        /// <summary>
        /// 
        /// </summary>
		SM_MIDEASTENABLED = 74,

        /// <summary>
        /// 
        /// </summary>
		SM_MOUSEWHEELPRESENT = 75,

        /// <summary>
        /// 
        /// </summary>
		SM_XVIRTUALSCREEN = 76,

        /// <summary>
        /// 
        /// </summary>
		SM_YVIRTUALSCREEN = 77,

        /// <summary>
        /// 
        /// </summary>
		SM_CXVIRTUALSCREEN = 78,

        /// <summary>
        /// 
        /// </summary>
		SM_CYVIRTUALSCREEN = 79,

        /// <summary>
        /// 
        /// </summary>
		SM_CMONITORS = 80,

        /// <summary>
        /// 
        /// </summary>
		SM_SAMEDISPLAYFORMAT = 81,

        /// <summary>
        /// 
        /// </summary>
		SM_IMMENABLED = 82,

        /// <summary>
        /// 
        /// </summary>
		SM_CXFOCUSBORDER = 83,

        /// <summary>
        /// 
        /// </summary>
		SM_CYFOCUSBORDER = 84,

        /// <summary>
        /// 
        /// </summary>
		SM_TABLETPC = 86,

        /// <summary>
        /// 
        /// </summary>
		SM_MEDIACENTER = 87,

        /// <summary>
        /// 
        /// </summary>
		SM_CMETRICS = 88,

        /// <summary>
        /// 
        /// </summary>
		SM_REMOTESESSION = 0x1000,

        /// <summary>
        /// 
        /// </summary>
		SM_SHUTTINGDOWN = 0x2000,

        /// <summary>
        /// 
        /// </summary>
		SM_REMOTECONTROL = 0x2001
	}
}
