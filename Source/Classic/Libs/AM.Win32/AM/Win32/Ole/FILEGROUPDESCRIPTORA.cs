// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FILEGROUPDESCRIPTORA.cs -- CF_FILEGROUPDESCRIPTOR clipboard format
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Defines the CF_FILEGROUPDESCRIPTOR clipboard format. 
	/// </summary>
	public struct FILEGROUPDESCRIPTORA
	{
		/// <summary>
		/// Number of elements in fgd.
		/// </summary>
		public int cItems;

		/// <summary>
		/// Array of <see cref="FILEDESCRIPTORA"/>
		///  structures that contain the file information.
		/// </summary>
		/// <remarks>
		/// SizeConst!!!
		/// </remarks>
		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 1 )]
		public FILEDESCRIPTORA[] fgd;
	}
}