/* EnumChildProc.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The EnumChildProc function is an application-defined callback 
	/// function used with the EnumChildWindows function. It receives 
	/// the child window handles.
	/// </summary>
	/// <param name="hwnd">Handle to a child window of the parent window 
	/// specified in EnumChildWindows.</param>
	/// <param name="lParam">Specifies the application-defined value 
	/// given in EnumChildWindows.</param>
	/// <returns>To continue enumeration, the callback function must 
	/// return TRUE; to stop enumeration, it must return FALSE.</returns>
	public delegate bool EnumChildProc
	(
		IntPtr hwnd,
		IntPtr lParam
	);
}
