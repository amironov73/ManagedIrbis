/* EVENTMSG.cs -- 
   Ars Magna project, http://library.istu.edu/am */

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
	/// The EVENTMSG structure contains information about a hardware 
	/// message sent to the system message queue. This structure is 
	/// used to store message information for the JournalPlaybackProc 
	/// callback function.
	/// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public struct EVENTMSG
	{
		/// <summary>
		/// Specifies the message.
		/// </summary>
		public static WindowMessage message;

		/// <summary>
		/// Specifies additional information about the message. 
		/// The exact meaning depends on the message value. 
		/// </summary>
		public static int paramL;

		/// <summary>
		/// Specifies additional information about the message. 
		/// The exact meaning depends on the message value.
		/// </summary>
		public static int paramH;

		/// <summary>
		/// Specifies the time at which the message was posted.
		/// </summary>
		public static int time;

		/// <summary>
		/// Handle to the window to which the message was posted.
		/// </summary>
		public static IntPtr hwnd;
	}
}
