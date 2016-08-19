/* OVERLAPPED.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information used in asynchronous input and output (I/O).
	/// </summary>
	[StructLayout ( LayoutKind.Sequential, Size = 20 )]
	public struct OVERLAPPED
	{
		/// <summary>
		/// Reserved for operating system use. This member, which 
		/// specifies a system-dependent status, is valid when the 
		/// GetOverlappedResult function returns without setting the 
		/// extended error information to ERROR_IO_PENDING. 
		/// </summary>
		public int Internal; 

		/// <summary>
		/// Reserved for operating system use. This member, which 
		/// specifies the length of the data transferred, is valid 
		/// when the GetOverlappedResult function returns TRUE. 
		/// </summary>
		public int InternalHigh; 

		/// <summary>
		/// File position at which to start the transfer. The file 
		/// position is a byte offset from the start of the file. 
		/// The calling process sets this member before calling the 
		/// ReadFile or WriteFile function. This member is ignored 
		/// when reading from or writing to named pipes and 
		/// communications devices and should be zero. 
		/// </summary>
        [CLSCompliant ( false )]
        public uint Offset; 

		/// <summary>
		/// High-order word of the byte offset at which to start the 
		/// transfer. This member is ignored when reading from or 
		/// writing to named pipes and communications devices and 
		/// should be zero. 
		/// </summary>
        [CLSCompliant ( false )]
        public uint OffsetHigh; 

		/// <summary>
		/// Handle to an event set to the signaled state when the 
		/// operation has been completed. The calling process must set 
		/// this member either to zero or a valid event handle before 
		/// calling any overlapped functions. To create an event object, 
		/// use the CreateEvent function. 
		/// </summary>
		public IntPtr hEvent;
	}
}
