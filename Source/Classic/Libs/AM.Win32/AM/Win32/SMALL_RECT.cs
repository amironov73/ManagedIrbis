/* SMALL_RECT.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Defines the coordinates of the upper left and lower 
	/// right corners of a rectangle.
	/// </summary>
	/// <remarks>This structure is used by console functions 
	/// to specify rectangular areas of console screen buffers, 
	/// where the coordinates specify the rows and columns of 
	/// screen-buffer character cells.</remarks>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Size = 8 )]
	public struct SMALL_RECT
	{
		/// <summary>
		/// X-coordinate of the upper left corner of the rectangle.
		/// </summary>
		public short Left;  

		/// <summary>
		/// Y-coordinate of the upper left corner of the rectangle.
		/// </summary>
		public short Top;  

		/// <summary>
		/// X-coordinate of the lower right corner of the rectangle.
		/// </summary>
		public short Right;  

		/// <summary>
		/// Y-coordinate of the lower right corner of the rectangle.
		/// </summary>
		public short Bottom;
	}
}
