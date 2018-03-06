// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SafeLibraryHandle.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Security.Permissions;

using Microsoft.Win32.SafeHandles;


#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	[SecurityPermission ( SecurityAction.LinkDemand, UnmanagedCode = true )]
	public class SafeLibraryHandle
		: SafeHandleZeroOrMinusOneIsInvalid
	{
		#region Construction

		/// <summary>
		/// Safes the file handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		public SafeLibraryHandle ( IntPtr handle )
			: base ( true )
		{
			SetHandle ( handle );
		}

		/// <summary>
		/// When overridden in a derived class, executes the code 
		/// required to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, 
		/// in the event of a catastrophic failure, false. In this case, 
		/// it generates a ReleaseHandleFailed Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle ()
		{
			return Kernel32.FreeLibrary ( handle );
		}

		#endregion
	}
}