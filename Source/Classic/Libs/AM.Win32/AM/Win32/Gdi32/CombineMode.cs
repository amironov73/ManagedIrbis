// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CombineMode.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies a mode indicating how the two regions will be combined.
	/// </summary>
	[CLSCompliant ( false )]
    public enum CombineMode : uint
	{
		/// <summary>
		/// Creates the intersection of the two combined regions.
		/// </summary>
		RGN_AND = 1,

		/// <summary>
		/// Creates the union of two combined regions.
		/// </summary>
		RGN_OR = 2,

		/// <summary>
		/// Creates the union of two combined regions except for any 
		/// overlapping areas.
		/// </summary>
		RGN_XOR = 3,

		/// <summary>
		/// Combines the parts of hrgnSrc1 that are not part of hrgnSrc2.
		/// </summary>
		RGN_DIFF = 4,

		/// <summary>
		/// Creates a copy of the region identified by hrgnSrc1.
		/// </summary>
		RGN_COPY = 5
	}
}
