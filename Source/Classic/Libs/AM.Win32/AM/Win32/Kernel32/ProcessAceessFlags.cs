/* ProcessAccessFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Process access flags.
	/// </summary>
	[Flags]
	public enum ProcessAccessFlags
	{
		/// <summary>
		/// Required to terminate a process using TerminateProcess.
		/// </summary>
		PROCESS_TERMINATE = 0x0001,

		/// <summary>
		/// Required to create a thread.
		/// </summary>
		PROCESS_CREATE_THREAD = 0x0002,

		/// <summary>
		/// ???
		/// </summary>
		PROCESS_SET_SESSIONID = 0x0004,

		/// <summary>
		/// Required to perform an operation on the address space of 
		/// a process (see VirtualProtectEx and WriteProcessMemory).
		/// </summary>
		PROCESS_VM_OPERATION = 0x0008,

		/// <summary>
		/// Required to read memory in a process using ReadProcessMemory.
		/// </summary>
		PROCESS_VM_READ = 0x0010,

		/// <summary>
		/// Required to write to memory in a process using WriteProcessMemory.
		/// </summary>
		PROCESS_VM_WRITE = 0x0020,

		/// <summary>
		/// Required to duplicate a handle using DuplicateHandle.
		/// </summary>
		PROCESS_DUP_HANDLE = 0x0040,

		/// <summary>
		/// Required to create a process.
		/// </summary>
		PROCESS_CREATE_PROCESS = 0x0080,

		/// <summary>
		/// Required to set memory limits using SetProcessWorkingSetSize.
		/// </summary>
		PROCESS_SET_QUOTA = 0x0100,

		/// <summary>
		/// Required to set certain information about a process, such as 
		/// its priority class (see SetPriorityClass).
		/// </summary>
		PROCESS_SET_INFORMATION = 0x0200,

		/// <summary>
		/// Required to retrieve certain information about a process, 
		/// such as its exit code and priority class (see 
		/// GetExitCodeProcess and GetPriorityClass).
		/// </summary>
		PROCESS_QUERY_INFORMATION = 0x0400,

		/// <summary>
		/// ???
		/// </summary>
		PROCESS_SUSPEND_RESUME = 0x0800,

		/// <summary>
		/// ???
		/// </summary>
		DELETE = 0x00010000,

		/// <summary>
		/// ???
		/// </summary>
		READ_CONTROL = 0x00020000,

		/// <summary>
		/// ???
		/// </summary>
		WRITE_DAC = 0x00040000,

		/// <summary>
		/// ???
		/// </summary>
		WRITE_OWNER = 0x00080000,

		/// <summary>
		/// Required to wait for the process to terminate using the 
		/// wait functions.
		/// </summary>
		SYNCHRONIZE = 0x00100000,

		/// <summary>
		/// ???
		/// </summary>
		STANDARD_RIGHTS_REQUIRED = 0x000F0000,

		/// <summary>
		/// All possible access rights for a process object.
		/// </summary>
		PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF
	}
}
