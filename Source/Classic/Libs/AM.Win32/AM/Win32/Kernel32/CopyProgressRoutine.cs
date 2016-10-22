/* CopyProgressRoutine.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The CopyProgressRoutine function is an application-defined 
	/// callback function used with the CopyFileEx and 
	/// MoveFileWithProgress functions. It is called when a portion 
	/// of a copy or move operation is completed. The 
	/// LPPROGRESS_ROUTINE type defines a pointer to this callback 
	/// function. CopyProgressRoutine is a placeholder for the 
	/// application-defined function name.
	/// </summary>
	/// <param name="TotalFileSize">Total size of the file, in bytes.</param>
	/// <param name="TotalBytesTransferred">Total number of bytes 
	/// transferred from the source file to the destination file since 
	/// the copy operation began.</param>
	/// <param name="StreamSize">Total size of the current file stream, 
	/// in bytes.</param>
	/// <param name="StreamBytesTransferred">Total number of bytes 
	/// in the current stream that have been transferred from the 
	/// source file to the destination file since the copy operation 
	/// began.</param>
	/// <param name="dwStreamNumber">Handle to the current stream. 
	/// The first time CopyProgressRoutine is called, the stream 
	/// number is 1.</param>
	/// <param name="dwCallbackReason">Reason that CopyProgressRoutine 
	/// was called.</param>
	/// <param name="hSourceFile">Handle to the source file.</param>
	/// <param name="hDestinationFile">Handle to the destination file.</param>
	/// <param name="lpData">Argument passed to CopyProgressRoutine 
	/// by the CopyFileEx or MoveFileWithProgress function.</param>
	/// <returns></returns>
	public delegate CopyProgressCode CopyProgressRoutine
		(
			long TotalFileSize,
			long TotalBytesTransferred,
			long StreamSize,
			long StreamBytesTransferred,
			int dwStreamNumber,
			CopyCallbackReason dwCallbackReason,
			IntPtr hSourceFile,
			IntPtr hDestinationFile,
			IntPtr lpData
	);
}
