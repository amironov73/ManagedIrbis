/* AbortProc.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The AbortProc function is an application-defined callback function 
	/// used with the SetAbortProc function. It is called when a print job 
	/// is to be canceled during spooling. The ABORTPROC type defines a pointer 
	/// to this callback function. AbortProc is a placeholder for the 
	/// application-defined function name.
	/// </summary>
	public delegate bool AbortProc
		(
			IntPtr hdc,
			int iError
		);
}
