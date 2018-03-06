// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HookProcedure.cs -- 
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
	/// The HookProcedure hook procedure is an application-defined 
	/// or library-defined callback function used with the 
	/// SetWindowsHookEx function.
	/// </summary>
	/// <param name="code"></param>
	/// <param name="lParam"></param>
	/// <param name="wParam"></param>
	/// <returns></returns>
	public delegate int HookProcedure
	(
		int code,
	    int wParam,
		int lParam
	);
}
