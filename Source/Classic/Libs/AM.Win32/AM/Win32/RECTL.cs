/* RECTL.cs --  
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
