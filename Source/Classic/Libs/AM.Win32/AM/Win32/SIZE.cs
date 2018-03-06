// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SIZE.cs -- contains size
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains size.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct SIZE
	{
		/// <summary>
		/// Width.
		/// </summary>
		public int cx;

		/// <summary>
		/// Height.
		/// </summary>
		public int cy;
	}
}
