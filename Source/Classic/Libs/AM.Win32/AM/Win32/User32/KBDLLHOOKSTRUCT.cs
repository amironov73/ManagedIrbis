// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* KBDLLHOOKSTRUCT.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information about a low-level keyboard input event.
	/// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public struct KBDLLHOOKSTRUCT
	{
		/// <summary>
		/// Specifies a virtual-key code. 
		/// The code must be a value in the range 1 to 254.
		/// </summary>
		public int vkCode;

		/// <summary>
		/// Specifies a hardware scan code for the key.
		/// </summary>
		public int scanCode;

		/// <summary>
		/// Specifies the extended-key flag, event-injected flag, 
		/// context code, and transition-state flag.
		/// </summary>
		public LowLevelKeyboardHookFlags flags;

		/// <summary>
		/// Specifies the time stamp for this message.
		/// </summary>
		public int time;

		/// <summary>
		/// Specifies extra information associated with the message.
		/// </summary>
		public IntPtr dwExtraInfo;
	}
}
