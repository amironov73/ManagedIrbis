/* FILEGROUPDESCRIPTORW.cs -- CF_FILEGROUPDESCRIPTOR clipboard format
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Defines the CF_FILEGROUPDESCRIPTOR clipboard format. 
	/// </summary>
	public struct FILEGROUPDESCRIPTORW
	{
		/// <summary>
		/// Number of elements in fgd.
		/// </summary>
		public int cItems;

		/// <summary>
		/// Array of <see cref="FILEDESCRIPTORW"/>
		///  structures that contain the file information.
		/// </summary>
		/// <remarks>
		/// SizeConst!!!
		/// </remarks>
		[MarshalAs ( UnmanagedType.ByValArray, SizeConst=1 )]
		public FILEDESCRIPTORW[] fgd;
	}
}