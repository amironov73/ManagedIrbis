// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoadImageFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags for LoadImage function.
	/// </summary>
	public enum LoadImageFlags
	{
		/// <summary>
		/// Loads a bitmap.
		/// </summary>
		IMAGE_BITMAP = 0,

		/// <summary>
		/// Loads an icon.
		/// </summary>
		IMAGE_ICON = 1,

		/// <summary>
		/// Loads a cursor.
		/// </summary>
		IMAGE_CURSOR = 2,

		/// <summary>
		/// Loads an enhanced metafile.
		/// </summary>
		IMAGE_ENHMETAFILE = 3
	}
}
