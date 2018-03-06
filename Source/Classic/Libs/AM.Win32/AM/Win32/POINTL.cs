// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* POINTL.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// ???
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct POINTL
	{
		/// <summary>
		/// ???
		/// </summary>
		public int x;

		/// <summary>
		/// ???
		/// </summary>
		public int y;
	}
}
