// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MOUSEHOOKSTRUCTEX.cs -- 
   Ars Magna project, http://arsmagna.ru */

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
	/// Contains information about a mouse event passed 
	/// to a WH_MOUSE hook procedure, MouseProc. 
	/// </summary>
	public struct MOUSEHOOKSTRUCTEX
	{
		/// <summary>
		/// Specifies a POINT structure that contains 
		/// the x- and y-coordinates of the cursor, 
		/// in screen coordinates.
		/// </summary>
		public Point pt;

		/// <summary>
		/// Handle to the window that will receive the 
		/// mouse message corresponding to the mouse event.
		/// </summary>
		public IntPtr hwnd;

		/// <summary>
		/// Specifies the hit-test value. For a list of hit-test 
		/// values, see the description of the WM_NCHITTEST message.
		/// </summary>
		public HitTestCode wHitTestCode;

		/// <summary>
		/// Specifies extra information associated with the message.
		/// </summary>
		public IntPtr dwExtraInfo;

		/// <summary>
		/// Data.
		/// </summary>
		public int mouseData;
	}
}
