/* ImageType.cs -- indicates whether an image is a bitmap or a metafile
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The ImageType enumeration indicates whether 
	/// an image is a bitmap or a metafile. The Image::GetType 
	/// method returns an element of this enumeration.
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// Unknown image type.
		/// </summary>
		ImageTypeUnknown=0,

		/// <summary>
		/// Bitmap.
		/// </summary>
		ImageTypeBitmap=1,

		/// <summary>
		/// Metafile.
		/// </summary>
		ImageTypeMetafile=2
	}
}
