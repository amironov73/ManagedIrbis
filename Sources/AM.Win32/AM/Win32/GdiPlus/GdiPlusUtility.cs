/* GdiPlusUtility.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	public static class GdiPlusUtility
	{
		#region Private members
		#endregion

		#region Public methods

		/// <summary>
		/// Gets the bitmap from GDI plus.
		/// </summary>
		/// <param name="gdiPlusBitmap">The GDI plus bitmap.</param>
		/// <returns></returns>
		public static Bitmap GetBitmapFromGdiPlus ( IntPtr gdiPlusBitmap )
		{
			MethodInfo method = typeof ( Bitmap ).GetMethod
				(
					"FromGDIplus",
					BindingFlags.Static|BindingFlags.NonPublic,
					null,
					new Type[] { typeof(IntPtr)}, 
					null
				);
			Bitmap result =  (Bitmap) method.Invoke 
				( 
				null, 
				new object[] {gdiPlusBitmap} 
				);
			return result;
		}

		#endregion
	}
}
