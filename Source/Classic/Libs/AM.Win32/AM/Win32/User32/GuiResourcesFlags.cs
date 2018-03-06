// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GuiResourcesFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// GUI object type.
	/// </summary>
	public enum GuiResourcesFlags
	{
		/// <summary>
		/// Count of GDI objects.
		/// </summary>
		GR_GDIOBJECTS = 0,

		/// <summary>
		/// Count of USER objects.
		/// </summary>
		GR_USEROBJECTS = 1
	}
}
