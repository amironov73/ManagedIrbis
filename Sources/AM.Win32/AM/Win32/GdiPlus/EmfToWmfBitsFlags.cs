/* EmfToWmfBitsFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the flags/options for the unmanaged call 
	/// to the GDI+ method Metafile.EmfToWmfBits().
	/// </summary>
	[Flags]
	public enum EmfToWmfBitsFlags
	{
		/// <summary>
		/// Use the default conversion.
		/// </summary>
		Default = 0x00000000,

		/// <summary>
		/// Embedded the source of the EMF metafiel within the resulting 
		/// WMF metafile.
		/// </summary>
		EmbedEmf = 0x00000001,

		/// <summary>
		/// Place a 22-byte header in the resulting WMF file.  
		/// The header is required for the metafile to be considered 
		/// placeable.
		/// </summary>
		IncludePlaceable = 0x00000002,

		/// <summary>
		/// Don't simulate clipping by using the XOR operator.
		/// </summary>
		NoXORClip = 0x00000004
	}
}