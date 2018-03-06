// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ErrorModeFlags.cs -- options for SetErrorMode function.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Options for SetErrorMode function.
	/// </summary>
	[Flags]
	public enum ErrorModeFlags
	{
		/// <summary>
		/// Use the system default, which is to display all 
		/// error dialog boxes.
		/// </summary>
		SystemDefault = 0,

		/// <summary>
		/// The system does not display the critical-error-handler 
		/// message box. Instead, the system sends the error to the 
		/// calling process.
		/// </summary>
		SEM_FAILCRITICALERRORS = 0x0001,

		/// <summary>
		/// The system does not display the general-protection-fault 
		/// message box. This flag should only be set by debugging 
		/// applications that handle general protection (GP) faults 
		/// themselves with an exception handler.
		/// </summary>
		SEM_NOGPFAULTERRORBOX = 0x0002,

		/// <summary>
		/// <para>64-bit Windows:  The system automatically fixes 
		/// memory alignment faults and makes them invisible to the 
		/// application. It does this for the calling process and any 
		/// descendant processes.</para>
		/// <para>After this value is set for a process, subsequent 
		/// attempts to clear the value are ignored.</para>
		/// </summary>
		SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,

		/// <summary>
		/// The system does not display a message box when it fails 
		/// to find a file. Instead, the error is returned to the 
		/// calling process.
		/// </summary>
		SEM_NOOPENFILEERRORBOX = unchecked( 0x8000 )
	}
}