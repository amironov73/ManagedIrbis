/* RGBQUAD.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The RGBQUAD structure describes a color consisting 
	/// of relative intensities of red, green, and blue. 
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Pack = 1 )]
	public struct RGBQUAD
	{
		/// <summary>
		/// Specifies the intensity of blue in the color.
		/// </summary>
		public byte rgbBlue;

		/// <summary>
		/// Specifies the intensity of green in the color.
		/// </summary>
		public byte rgbGreen;

		/// <summary>
		/// Specifies the intensity of red in the color.
		/// </summary>
		public byte rgbRed;

		/// <summary>
		/// Reserved; must be zero.
		/// </summary>
		public byte rgbReserved;
	}
}
