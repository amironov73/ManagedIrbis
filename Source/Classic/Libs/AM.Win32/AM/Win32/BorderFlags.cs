/* BorderFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the type of border to draw.
	/// </summary>
	[Flags]
	public enum BorderFlags
	{
		/// <summary>
		/// Left side of border rectangle.
		/// </summary>
		BF_LEFT = 0x0001,

		/// <summary>
		/// Top of border rectangle.
		/// </summary>
		BF_TOP = 0x0002,

		/// <summary>
		/// Right side of border rectangle.
		/// </summary>
		BF_RIGHT = 0x0004,

		/// <summary>
		/// Bottom of border rectangle.
		/// </summary>
		BF_BOTTOM = 0x0008,

		/// <summary>
		/// Top and left side of border rectangle.
		/// </summary>
		BF_TOPLEFT = ( BF_TOP | BF_LEFT ),

		/// <summary>
		/// Top and right side of border rectangle.
		/// </summary>
		BF_TOPRIGHT = ( BF_TOP | BF_RIGHT ),

		/// <summary>
		/// Bottom and left side of border rectangle.
		/// </summary>
		BF_BOTTOMLEFT = ( BF_BOTTOM | BF_LEFT ),

		/// <summary>
		/// Bottom and right side of border rectangle.
		/// </summary>
		BF_BOTTOMRIGHT = ( BF_BOTTOM | BF_RIGHT ),

		/// <summary>
		/// Entire border rectangle.
		/// </summary>
		BF_RECT = ( BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM ),

		/// <summary>
		/// Diagonal border.
		/// </summary>
		BF_DIAGONAL = 0x0010,

		/// <summary>
		/// Diagonal border. The end point is the upper-right corner of the 
		/// rectangle; the origin is the lower-left corner.
		/// </summary>
		BF_DIAGONAL_ENDTOPRIGHT = ( BF_DIAGONAL | BF_TOP | BF_RIGHT ),

		/// <summary>
		/// Diagonal border. The end point is the upper-left corner of the 
		/// rectangle; the origin is the lower-right corner.
		/// </summary>
		BF_DIAGONAL_ENDTOPLEFT = ( BF_DIAGONAL | BF_TOP | BF_LEFT ),

		/// <summary>
		/// Diagonal border. The end point is the lower-left corner of the 
		/// rectangle; the origin is the upper-right corner.
		/// </summary>
		BF_DIAGONAL_ENDBOTTOMLEFT = ( BF_DIAGONAL | BF_BOTTOM | BF_LEFT ),

		/// <summary>
		/// Diagonal border. The end point is the lower-right corner of the 
		/// rectangle; the origin is the upper-left corner.
		/// </summary>
		BF_DIAGONAL_ENDBOTTOMRIGHT = ( BF_DIAGONAL | BF_BOTTOM | BF_RIGHT ),

		/// <summary>
		/// Interior of the rectangle is to be filled.
		/// </summary>
		BF_MIDDLE = 0x0800,

		/// <summary>
		/// Soft buttons instead of tiles.
		/// </summary>
		BF_SOFT = 0x1000,

		/// <summary>
		/// The rectangle pointed to by the pDestRect parameter is shrunk to 
		/// exclude the edges that were drawn; otherwise the rectangle does 
		/// not change.
		/// </summary>
		BF_ADJUST = 0x2000,

		/// <summary>
		/// Flat border.
		/// </summary>
		BF_FLAT = 0x4000,

		/// <summary>
		/// One-dimensional border.
		/// </summary>
		BF_MONO = 0x8000
	}
}
