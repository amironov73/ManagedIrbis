// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DEVMODEW_DISPLAY.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The DEVMODE data structure contains information about 
	/// the initialization and environment of a printer or a display device.
	/// </summary>
	/// <remarks>Unicode-version for display device only.</remarks>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Size=220 )]
	//[StructLayout ( LayoutKind.Explicit, 
	//    CharSet = CharSet.Unicode, Size = 220 )]
	public struct DEVMODEW_DISPLAY
	{
        /// <summary>
        /// 
        /// </summary>
		public const int CCHDEVICENAME = 32; 

        /// <summary>
        /// 
        /// </summary>
		public const int CCHFORMNAME = 32;

        /// <summary>
        /// 
        /// </summary>
		public const short DM_SPECVERSION = 0x0401;

        /// <summary>
        /// 
        /// </summary>
		public const short SIZE = 220;

		/// <summary>
		/// Specifies the "friendly" name of the printer or display; 
		/// for example, "PCL/HP LaserJet" in the case of PCL/HP LaserJet®. 
		/// This string is unique among device drivers. Note that this name 
		/// may be truncated to fit in the dmDeviceName array.
		/// </summary>
		//[FieldOffset ( 0 )]
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME )]
		public string dmDeviceName;

		/// <summary>
		/// Specifies the version number of the initialization data 
		/// specification on which the structure is based. To ensure the 
		/// correct version is used for any operating system, use DM_SPECVERSION. 
		/// </summary>
		//[FieldOffset ( 64 )]
		public short dmSpecVersion;
    
		/// <summary>
		/// Specifies the driver version number assigned by the driver developer.
		/// </summary>
		//[FieldOffset ( 66 )]
		public short dmDriverVersion;
    
		/// <summary>
		/// Specifies the size, in bytes, of the DEVMODE structure, 
		/// not including any private driver-specific data that might 
		/// follow the structure's public members. Set this member to 
		/// sizeof(DEVMODE) to indicate the version of the DEVMODE structure 
		/// being used.
		/// </summary>
		//[FieldOffset ( 68 )]
		public short dmSize;
    
		/// <summary>
		/// Contains the number of bytes of private driver-data that follow 
		/// this structure. If a device driver does not use device-specific 
		/// information, set this member to zero. 
		/// </summary>
		//[FieldOffset ( 70 )]
		public short dmDriverExtra;
    
		/// <summary>
		/// Specifies whether certain members of the DEVMODE structure 
		/// have been initialized. If a member is initialized, its 
		/// corresponding bit is set, otherwise the bit is clear. 
		/// A driver supports only those DEVMODE members that are 
		/// appropriate for the printer or display technology. 
		/// </summary>
		//[FieldOffset ( 72 )]
		public DeviceModeFlags dmFields;
    
		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: For display devices only, 
		/// a POINTL structure that indicates the positional coordinates 
		/// of the display device in reference to the desktop area. 
		/// The primary display device is always located at coordinates (0,0). 
		/// </summary>
		//[FieldOffset ( 76 )]
		public POINTL dmPosition;

		/// <summary>
		/// Windows XP: For display devices only, the orientation at which 
		/// images should be presented. If DM_DISPLAYORIENTATION is not set, 
		/// this member must be zero.
		/// </summary>
		//[FieldOffset ( 84 )]
		public DeviceOrientation dmDisplayOrientation;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 88 )]
        public int dmDisplayFixedOutput; 

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 92 )]
		public short dmColor;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 94 )]
		public short dmDuplex;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 96 )]
		public short dmYResolution;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 98 )]
		public short dmTTOption;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 100 )]
		public short  dmCollate;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 102 )]
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME )]
		public string dmFormName;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 166 )]
		public short dmLogPixels;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 168 )]
		public int dmBitsPerPel;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 172 )]
		public int dmPelsWidth;
    
        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 176 )]
		public int dmPelsHeight;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 180 )]
		public int dmDisplayFlags;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 184 )]
		public int dmDisplayFrequency;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 188 )]
		public int dmICMMethod;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 192 )]
		public int dmICMIntent;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 196 )]
		public int dmMediaType;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 200 )]
		public int dmDitherType;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 204 )]
		public int dmReserved1;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 208 )]
		public int dmReserved2;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 212 )]
		public int dmPanningWidth;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 216 )]
		public int dmPanningHeight;
	}

	/// <summary>
	/// The DEVMODE data structure contains information about 
	/// the initialization and environment of a printer or a display device.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Size = 156 )]
	public struct DEVMODEA_DISPLAY
	{
        /// <summary>
        /// 
        /// </summary>
		public const int CCHDEVICENAME = 32;

        /// <summary>
        /// 
        /// </summary>
		public const int CCHFORMNAME = 32;

        /// <summary>
        /// 
        /// </summary>
		public const short DM_SPECVERSION = 0x0401;

        /// <summary>
        /// 
        /// </summary>
		public const short SIZE = 156;

		/// <summary>
		/// Specifies the "friendly" name of the printer or display; 
		/// for example, "PCL/HP LaserJet" in the case of PCL/HP LaserJet®. 
		/// This string is unique among device drivers. Note that this name 
		/// may be truncated to fit in the dmDeviceName array.
		/// </summary>
		//[FieldOffset ( 0 )]
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME )]
		public string dmDeviceName;

		/// <summary>
		/// Specifies the version number of the initialization data 
		/// specification on which the structure is based. To ensure the 
		/// correct version is used for any operating system, use DM_SPECVERSION. 
		/// </summary>
		//[FieldOffset ( 64 )]
		public short dmSpecVersion;

		/// <summary>
		/// Specifies the driver version number assigned by the driver developer.
		/// </summary>
		//[FieldOffset ( 66 )]
		public short dmDriverVersion;

		/// <summary>
		/// Specifies the size, in bytes, of the DEVMODE structure, 
		/// not including any private driver-specific data that might 
		/// follow the structure's public members. Set this member to 
		/// sizeof(DEVMODE) to indicate the version of the DEVMODE structure 
		/// being used.
		/// </summary>
		//[FieldOffset ( 68 )]
		public short dmSize;

		/// <summary>
		/// Contains the number of bytes of private driver-data that follow 
		/// this structure. If a device driver does not use device-specific 
		/// information, set this member to zero. 
		/// </summary>
		//[FieldOffset ( 70 )]
		public short dmDriverExtra;

		/// <summary>
		/// Specifies whether certain members of the DEVMODE structure 
		/// have been initialized. If a member is initialized, its 
		/// corresponding bit is set, otherwise the bit is clear. 
		/// A driver supports only those DEVMODE members that are 
		/// appropriate for the printer or display technology. 
		/// </summary>
		//[FieldOffset ( 72 )]
		public DeviceModeFlags dmFields;

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: For display devices only, 
		/// a POINTL structure that indicates the positional coordinates 
		/// of the display device in reference to the desktop area. 
		/// The primary display device is always located at coordinates (0,0). 
		/// </summary>
		//[FieldOffset ( 76 )]
		public POINTL dmPosition;

		/// <summary>
		/// Windows XP: For display devices only, the orientation at which 
		/// images should be presented. If DM_DISPLAYORIENTATION is not set, 
		/// this member must be zero.
		/// </summary>
		//[FieldOffset ( 84 )]
		public DeviceOrientation dmDisplayOrientation;

        /// <summary>
        /// 
        /// </summary>
		//[FieldOffset ( 88 )]
		public int dmDisplayFixedOutput;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 92 )]
		public short dmColor;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 94 )]
		public short dmDuplex;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 96 )]
		public short dmYResolution;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 98 )]
		public short dmTTOption;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 100 )]
		public short dmCollate;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 102 )]
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME )]
		public string dmFormName;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 166 )]
		public short dmLogPixels;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 168 )]
		public int dmBitsPerPel;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 172 )]
		public int dmPelsWidth;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 176 )]
		public int dmPelsHeight;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 180 )]
		public int dmDisplayFlags;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 184 )]
		public int dmDisplayFrequency;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 188 )]
		public int dmICMMethod;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 192 )]
		public int dmICMIntent;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 196 )]
		public int dmMediaType;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 200 )]
		public int dmDitherType;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 204 )]
		public int dmReserved1;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 208 )]
		public int dmReserved2;

		//[FieldOffset ( 212 )]
        /// <summary>
        /// 
        /// </summary>
        public int dmPanningWidth;

        /// <summary>
        /// 
        /// </summary>
        //[FieldOffset ( 216 )]
		public int dmPanningHeight;
	}
}
