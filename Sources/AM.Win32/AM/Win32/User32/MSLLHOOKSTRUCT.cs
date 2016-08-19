/* MSLLHOOKSTRUCT.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
	public struct MSLLHOOKSTRUCT
	{
		/// <summary>
		/// Specifies a POINT structure that contains the x- and 
		/// y-coordinates of the cursor, in screen coordinates.
		/// </summary>
		public Point pt;

		/// <summary>
		/// Data.
		/// </summary>
		public int mouseData;

		/// <summary>
		/// Specifies the event-injected flag.
		/// </summary>
		public LowLevelMouseHookFlags flags;

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
