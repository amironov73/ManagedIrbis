// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DrawStateProc.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The DrawStateProc function is an application-defined callback 
	/// function that renders a complex image for the DrawState function. 
	/// The DRAWSTATEPROC type defines a pointer to this callback function. 
	/// DrawStateProc is a placeholder for the application-defined function name. 
	/// </summary>
	/// <param name="hdc">Handle to the device context to draw in. 
	/// The device context is a memory device context with a bitmap 
	/// selected, the dimensions of which are at least as great as 
	/// those specified by the cx and cy parameters.</param>
	/// <param name="lData">Specifies information about the image, 
	/// which the application passed to DrawState.</param>
	/// <param name="wData">Specifies information about the image, 
	/// which the application passed to DrawState.</param>
	/// <param name="cx">Specifies the image width, in device units, 
	/// as specified by the call to DrawState.</param>
	/// <param name="cy">Specifies the image height, in device units, 
	/// as specified by the call to DrawState.</param>
	/// <returns>If the function succeeds, the return value is TRUE.
	/// </returns>
	public delegate bool DrawStateProc
		(
			IntPtr hdc,
			int lData,
			int wData,
			int cx,
			int cy
		);
}
