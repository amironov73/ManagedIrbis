// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MEMORYSTATUSEX.cs -- information about the current state of both physical and virtual memory. 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The MEMORYSTATUSEX structure contains information about 
	/// the current state of both physical and virtual memory, 
	/// including extended memory. The GlobalMemoryStatusEx function 
	/// stores information in this structure.
	/// </summary>
	/// <remarks>MEMORYSTATUSEX reflects the state of memory at the 
	/// time of the call. It reflects the size of the paging file at 
	/// that time. The operating system can enlarge the paging file 
	/// up to the maximum size set by the administrator.</remarks>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Pack = 4, Size = MEMORYSTATUSEX.StructureSize )]
	public struct MEMORYSTATUSEX
	{
		/// <summary>
		/// Structure size.
		/// </summary>
		public const int StructureSize = 64;

//		/// <summary>
//		/// Get memory status.
//		/// </summary>
//		/// <returns>Memory status.</returns>
//		public static MEMORYSTATUSEX GetMemoryStatusEx ()
//		{
//			MEMORYSTATUSEX result = new MEMORYSTATUSEX ();
//			result.dwLength = StructureSize;
//			Kernel32.GlobalMemoryStatusEx ( ref result );
//			return result;
//		}

		/// <summary>
		/// Size of the structure, in bytes. You must set this member 
		/// before calling GlobalMemoryStatusEx.
		/// </summary>
        [CLSCompliant ( false )]
        public uint dwLength; 

		/// <summary>
		/// Number between 0 and 100 that gives a general idea of 
		/// current memory utilization, in which 0 indicates no memory 
		/// use and 100 indicates full memory use. 
		/// </summary>
        [CLSCompliant ( false )]
        public uint dwMemoryLoad; 

		/// <summary>
		/// Total size of physical memory, in bytes. 
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullTotalPhys; 

		/// <summary>
		/// Size of physical memory available, in bytes. 
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullAvailPhys; 

		/// <summary>
		/// Size of the committed memory limit, in bytes. This is 
		/// physical memory plus the size of the page file, minus 
		/// a small overhead.
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullTotalPageFile; 

		/// <summary>
		/// Size of available memory to commit, in bytes. The limit 
		/// is ullTotalPageFile.
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullAvailPageFile; 

		/// <summary>
		/// Total size of the user mode portion of the virtual address 
		/// space of the calling process, in bytes. 
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullTotalVirtual; 

		/// <summary>
		/// Size of unreserved and uncommitted memory in the user mode 
		/// portion of the virtual address space of the calling process, 
		/// in bytes.
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullAvailVirtual; 

		/// <summary>
		/// Size of unreserved and uncommitted memory in the extended 
		/// portion of the virtual address space of the calling process, 
		/// in bytes. 
		/// </summary>
        [CLSCompliant ( false )]
        public ulong ullAvailExtendedVirtual;
	}
}
