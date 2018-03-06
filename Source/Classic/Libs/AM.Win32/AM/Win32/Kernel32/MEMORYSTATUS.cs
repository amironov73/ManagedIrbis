// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MEMORYSTATUS.cs -- information about the current state of both physical and virtual memory. 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The MEMORYSTATUS structure contains information about 
	/// the current state of both physical and virtual memory. 
	/// The GlobalMemoryStatus function stores information in 
	/// a MEMORYSTATUS structure.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Pack = 4,
		Size = MEMORYSTATUS.StructureSize )]
	public struct MEMORYSTATUS
	{
		/// <summary>
		/// Structure size.
		/// </summary>
		public const int StructureSize = 32;

		/// <summary>
		/// Size of the MEMORYSTATUS data structure, in bytes. 
		/// You do not need to set this member before calling the 
		/// GlobalMemoryStatus function; the function sets it. 
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwLength;

		/// <summary>
		/// <para>Approximate percentage of total physical memory 
		/// that is in use.</para>
		/// <para>Windows NT:  Percentage of approximately the last 
		/// 1000 pages of physical memory that is in use.</para>
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwMemoryLoad;

		/// <summary>
		/// Total size of physical memory, in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwTotalPhys;

		/// <summary>
		/// Size of physical memory available, in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwAvailPhys;

		/// <summary>
		/// Size of the committed memory limit, in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwTotalPageFile;

		/// <summary>
		/// Size of available memory to commit, in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwAvailPageFile;

		/// <summary>
		/// Total size of the user mode portion of the virtual address 
		/// space of the calling process, in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwTotalVirtual;

		/// <summary>
		/// Size of unreserved and uncommitted memory in the user mode 
		/// portion of the virtual address space of the calling process, 
		/// in bytes.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwAvailVirtual;
	}
}