// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RECTL.cs --  
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
	public struct RECTL
	{
		/// <summary>
		/// ???
		/// </summary>
		public int left;

		/// <summary>
		/// ???
		/// </summary>
		public int top;

		/// <summary>
		/// ???
		/// </summary>
		public int right;

		/// <summary>
		/// ???
		/// </summary>
		public int bottom;
	}
}
