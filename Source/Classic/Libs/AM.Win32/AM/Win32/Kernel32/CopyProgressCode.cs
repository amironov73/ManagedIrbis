/* CopyProgressCode.cs -- 
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
	/// </summary>
	public enum CopyProgressCode
	{
		/// <summary>
		/// Continue the copy operation.
		/// </summary>
		PROGRESS_CONTINUE = 0,

		/// <summary>
		/// Cancel the copy operation and delete the destination file.
		/// </summary>
		PROGRESS_CANCEL = 1,

		/// <summary>
		/// Stop the copy operation. It can be restarted at a later time.
		/// </summary>
		PROGRESS_STOP = 2,

		/// <summary>
		/// Continue the copy operation, but stop invoking 
		/// CopyProgressRoutine to report progress.
		/// </summary>
		PROGRESS_QUIET = 3
	}
}
