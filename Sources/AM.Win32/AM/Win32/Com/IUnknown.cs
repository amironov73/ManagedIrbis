/* IUnknown.cs -- most fundamental COM interface
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Interface to get pointers to other interfaces on a given object, using
	/// the QueryInterface method.
	/// DO NOT CHANGE ORDER OF THESE FUNCTIONS!!!!
	/// </summary>
	[ComImport]
	[Guid ( "00000000-0000-0000-c000-000000000046" )]
	[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IUnknown
	{
		/// <summary>
		/// Get a pointer to a specific interface on an object
		/// </summary>
		/// <param name="riid">GUID for the interface whose interface pointer 
		/// we are seeking.</param>
		/// <param name="pVoid">Pointer where the desired interface pointer 
		/// will be saved.</param>
		/// <returns>S_OK if interface is supported; E_NOINTERFACE if not.
		/// </returns>
		[PreserveSig]
		IntPtr QueryInterface
			(
				[MarshalAs ( UnmanagedType.LPStruct )]
				Guid riid,
				out IntPtr pVoid
			);

		/// <summary>
		/// Increments the reference counter to this object.
		/// </summary>
		/// <returns>New value of the reference counter.</returns>
		[PreserveSig]
		IntPtr AddRef ();

		/// <summary>
		/// Decrements the reference counter for an object.
		/// </summary>
		/// <returns>New value of the reference counter.</returns>
		[PreserveSig]
		IntPtr Release ();
	}
}
