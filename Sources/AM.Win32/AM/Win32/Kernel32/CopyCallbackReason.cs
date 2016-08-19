/* CopyCallbackReason.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Reason that CopyProgressRoutine was called.
	/// </summary>
	public enum CopyCallbackReason
	{
		/// <summary>
		/// Another part of the data file was copied.
		/// </summary>
		CALLBACK_CHUNK_FINISHED = 0x00000000,

		/// <summary>
		/// Another stream was created and is about to be copied. 
		/// This is the callback reason given when the callback 
		/// routine is first invoked.
		/// </summary>
		CALLBACK_STREAM_SWITCH = 0x00000001
	}
}
