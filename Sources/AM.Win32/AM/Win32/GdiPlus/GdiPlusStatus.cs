/* GdiPlusStatus.cs -- indicates the result of GDI+ call
   Ars Magna project, http://library.istu.edu/am */

namespace AM.Win32
{
	/// <summary>
	/// The Status enumeration indicates the result 
	/// of a Microsoft Windows GDI+ method call.
	/// </summary>
	public enum GdiPlusStatus
	{
		/// <summary>
		/// Ok.
		/// </summary>
		Ok=0,

		/// <summary>
		/// Generic error.
		/// </summary>
		GenericError=1,

		/// <summary>
		/// Invalid parameter.
		/// </summary>
		InvalidParameter=2,

		/// <summary>
		/// Out of memory.
		/// </summary>
		OutOfMemory=3,

		/// <summary>
		/// Object busy.
		/// </summary>
		ObjectBusy=4,

		/// <summary>
		/// Insufficient Buffer.
		/// </summary>
		InsufficientBuffer=5,

		/// <summary>
		/// Not implemented.
		/// </summary>
		NotImplemented=6,

		/// <summary>
		/// Win32 error.
		/// </summary>
		Win32Error=7,

		/// <summary>
		/// Wrong state.
		/// </summary>
		WrongState=8,

		/// <summary>
		/// Aborted.
		/// </summary>
		Aborted=9,

		/// <summary>
		/// File not found.
		/// </summary>
		FileNotFound=10,

		/// <summary>
		/// Value overflow.
		/// </summary>
		ValueOverflow=11,

		/// <summary>
		/// Access denied.
		/// </summary>
		AccessDenied=12,

		/// <summary>
		/// Unknown image format.
		/// </summary>
		UnknownImageFormat=13,

		/// <summary>
		/// Font family not found.
		/// </summary>
		FontFamilyNotFound=14,

		/// <summary>
		/// Font style not found.
		/// </summary>
		FontStyleNotFound=15,

		/// <summary>
		/// Not TrueType font.
		/// </summary>
		NotTrueTypeFont=16,

		/// <summary>
		/// Unsupported GDI+ version.
		/// </summary>
		UnsupportedGdiplusVersion=17,

		/// <summary>
		/// GDI+ not initialized.
		/// </summary>
		GdiplusNotInitialized=18,

		/// <summary>
		/// Property not found.
		/// </summary>
		PropertyNotFound=19,

		/// <summary>
		/// Property not supported.
		/// </summary>
		PropertyNotSupported=20,

		/// <summary>
		/// Profile not found.
		/// </summary>
		ProfileNotFound=21
	}
}
