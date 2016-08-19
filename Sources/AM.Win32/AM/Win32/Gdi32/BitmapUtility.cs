/* BitmapUtility.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	public static class BitmapUtility
	{
		#region Private members

		#endregion

		#region Public methods

		/// <summary>
		/// Gets the pixel data.
		/// </summary>
		/// <param name="dibptr">The dibptr.</param>
		/// <returns></returns>
		public static IntPtr GetPixelData ( IntPtr dibptr )
		{
			// BITMAPINFOHEADER bmi = new BITMAPINFOHEADER ();
			// BITMAPINFO bi = new BITMAPINFO ();
			BITMAPINFOHEADER bmi = (BITMAPINFOHEADER)
				Marshal.PtrToStructure ( dibptr, typeof(BITMAPINFOHEADER) );
			// BITMAPINFOHEADER bmi = bi.bmiHeader;
			unchecked
			{
				if ( bmi.biSizeImage == 0 )
				{
					bmi.biSizeImage =
						(uint) ( ( ( ( bmi.biWidth * bmi.biBitCount + 31 ) & ~31 ) >> 3 )
						         * bmi.biHeight );
				}
				int result = (int) bmi.biClrUsed;
				if ( ( result == 0 )
				     && ( bmi.biBitCount <= 8 ) )
				{
					result = 1 << bmi.biBitCount;
				}
				result = (int) ( ( result * 4 ) + bmi.biSize + dibptr.ToInt32 () );
				return new IntPtr ( result );
			}
		}

		#endregion
	}
}