// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DeviceStateFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Display device state flags
	/// </summary>
	[Flags]
	public enum DeviceStateFlags
	{
		/// <summary>
		/// The device is part of the desktop.
		/// </summary>
		DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001,

		/// <summary>
		/// 
		/// </summary>
		DISPLAY_DEVICE_MULTI_DRIVER = 0x00000002,

		/// <summary>
		/// The primary desktop is on the device. 
		/// For a system with a single display card, 
		/// this is always set. For a system with multiple 
		/// display cards, only one device can have this set.
		/// </summary>
		DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004,

		/// <summary>
		/// Represents a pseudo device used to mirror application 
		/// drawing for remoting or other purposes. An invisible pseudo 
		/// monitor is associated with this device. For example, 
		/// NetMeeting uses it. Note that GetSystemMetrics(SM_MONITORS) 
		/// only accounts for visible display monitors.
		/// </summary>
		DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008,

		/// <summary>
		/// The device is VGA compatible.
		/// </summary>
		DISPLAY_DEVICE_VGA_COMPATIBLE = 0x00000010,

		/// <summary>
		/// The device is removable; it cannot be the primary display.
		/// </summary>
		DISPLAY_DEVICE_REMOVABLE = 0x00000020,

		/// <summary>
		/// The device has more display modes than its output 
		/// devices support.
		/// </summary>
		DISPLAY_DEVICE_MODESPRUNED = 0x08000000,

		/// <summary>
		/// ???
		/// </summary>
		DISPLAY_DEVICE_REMOTE = 0x04000000,

		/// <summary>
		/// ???
		/// </summary>
		DISPLAY_DEVICE_DISCONNECT = 0x02000000
	}
}
