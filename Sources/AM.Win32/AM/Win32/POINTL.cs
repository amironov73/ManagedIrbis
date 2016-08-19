/* POINTL.cs --  
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
