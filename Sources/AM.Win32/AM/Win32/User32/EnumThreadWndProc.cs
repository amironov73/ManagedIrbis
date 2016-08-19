/* EnumThreadWndProc.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The EnumThreadWndProc function is an application-defined 
	/// callback function used with the EnumThreadWindows function. 
	/// It receives the window handles associated with a thread.
	/// </summary>
	/// <param name="hwnd">Handle to a window associated with the 
	/// thread specified in the EnumThreadWindows function.</param>
	/// <param name="lParam">Specifies the application-defined 
	/// value given in the EnumThreadWindows function.</param>
	/// <returns>To continue enumeration, the callback function 
	/// must return TRUE; to stop enumeration, it must return FALSE.
	/// </returns>
	public delegate bool EnumThreadWndProc
		(
			IntPtr hwnd,
			IntPtr lParam
		);
}
