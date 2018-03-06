// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WindowProc.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The WindowProc function is an application-defined function that 
	/// processes messages sent to a window. The WNDPROC type defines a 
	/// pointer to this callback function. WindowProc is a placeholder for 
	/// the application-defined function name.
	/// </summary>
	public delegate int WindowProc
	(          
		IntPtr hwnd,
		int uMsg,
		int wParam,
		int lParam
	);
}
