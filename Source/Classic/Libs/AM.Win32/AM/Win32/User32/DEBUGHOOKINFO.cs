// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DEBUGHOOKINFO.cs -- 
   Ars Magna project, http://arsmagna.ru */

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
	/// contains debugging information passed to a WH_DEBUG 
	/// hook procedure, DebugProc.
	/// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public struct DEBUGHOOKINFO
	{
		/// <summary>
		/// Handle to the thread containing the filter function.
		/// </summary>
		public int idThread;

		/// <summary>
		/// Handle to the thread that installed the debugging 
		/// filter function. 
		/// </summary>
		public int idThreadInstaller;

		/// <summary>
		/// Specifies the value to be passed to the hook in the 
		/// lParam parameter of the DebugProc callback function.
		/// </summary>
		public int lParam;

		/// <summary>
		/// Specifies the value to be passed to the hook in the 
		/// wParam parameter of the DebugProc callback function. 
		/// </summary>
		public int wParam;
		
		/// <summary>
		/// Specifies the value to be passed to the hook in the 
		/// nCode parameter of the DebugProc callback function.
		/// </summary>
		public int code;
	}
}
