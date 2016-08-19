/* LowLevelKeyboardHookFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the extended-key flag, event-injected flag, 
	/// context code, and transition-state flag for low-level
	/// keyboard hook procedure.
	/// </summary>
	[Flags]
	public enum LowLevelKeyboardHookFlags
	{
		/// <summary>
		/// ???
		/// </summary>
		KF_EXTENDED = 0x0100,

		/// <summary>
		/// ???
		/// </summary>
		KF_DLGMODE = 0x0800,

		/// <summary>
		/// ???
		/// </summary>
		KF_MENUMODE = 0x1000,

		/// <summary>
		/// ???
		/// </summary>
		KF_ALTDOWN = 0x2000,

		/// <summary>
		/// ???
		/// </summary>
		KF_REPEAT = 0x4000,

		/// <summary>
		/// ???
		/// </summary>
		KF_UP = 0x8000,

		/// <summary>
		/// Test the extended-key flag.
		/// </summary>
		LLKHF_EXTENDED = KF_EXTENDED >> 8,

		/// <summary>
		/// Test the event-injected flag.
		/// </summary>
		LLKHF_INJECTED = 0x00000010,

		/// <summary>
		/// Test the context code.
		/// </summary>
		LLKHF_ALTDOWN = KF_ALTDOWN >> 8,

		/// <summary>
		/// Test the transition-state flag.
		/// </summary>
		LLKHF_UP = KF_UP >> 8
	}
}
