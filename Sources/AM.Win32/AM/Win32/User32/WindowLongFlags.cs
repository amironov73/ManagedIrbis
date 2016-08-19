/* WindowLongFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the zero-based offset to the value to be retrieved by
	/// GetWindowLong function or set by SetWindowLong function.
	/// </summary>
	public enum WindowLongFlags : int
	{
		/// <summary>
		/// Retrieves the address of the window procedure, or a handle 
		/// representing the address of the window procedure.
		/// </summary>
		GWL_WNDPROC = -4,

		/// <summary>
		/// Retrieves a handle to the application instance.
		/// </summary>
		GWL_HINSTANCE = -6,

		/// <summary>
		/// Retrieves a handle to the parent window, if any.
		/// </summary>
		GWL_HWNDPARENT = -8,

		/// <summary>
		/// Retrieves the window styles.
		/// </summary>
		GWL_STYLE = -16,

		/// <summary>
		/// Retrieves the extended window styles.
		/// </summary>
		GWL_EXSTYLE = -20,

		/// <summary>
		/// Retrieves the user data associated with the window. 
		/// This data is intended for use by the application that 
		/// created the window. Its value is initially zero.
		/// </summary>
		GWL_USERDATA = -21,

		/// <summary>
		/// Retrieves the identifier of the window.
		/// </summary>
		GWL_ID = -12,

		/// <summary>
		/// Retrieves the return value of a message processed in the 
		/// dialog box procedure.
		/// </summary>
		DWL_MSGRESULT = 0,

		/// <summary>
		/// Retrieves the address of the dialog box procedure, or a 
		/// handle representing the address of the dialog box procedure. 
		/// You must use the CallWindowProc function to call the dialog box 
		/// procedure.
		/// </summary>
		DWL_DLGPROC = 4,

		/// <summary>
		/// Retrieves extra information private to the application, such 
		/// as handles or pointers.
		/// </summary>
		DWL_USER = 8
	}
}
