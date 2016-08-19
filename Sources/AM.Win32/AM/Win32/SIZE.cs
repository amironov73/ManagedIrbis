/* SIZE.cs -- contains size
   Ars Magna project, http://library.istu.edu/am */

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
