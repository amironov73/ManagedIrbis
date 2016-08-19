/* SIZEL.cs --  
   Ars Magna project, http://library.istu.edu/am */

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
	public struct SIZEL
	{
		/// <summary>
		/// ???
		/// </summary>
		public int cx;

		/// <summary>
		/// ???
		/// </summary>
		public int cy;
	}
}
