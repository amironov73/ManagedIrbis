// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WinApiUtility.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	public static class WinApiUtility
	{
		#region Private members
		#endregion

		#region Public methods

		/// <summary>
		/// Checks the specified result code.
		/// </summary>
		/// <param name="resultCode">The result code.</param>
		public static void Check ( int resultCode )
		{
			if ( resultCode < 0 )
			{
				throw new Win32Exception ();
			}
		}

		#endregion
	}
}
