// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoadLibraryFlags.cs -- flags for LoadLibraryEx function 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags for LoadLibraryEx function.
	/// </summary>
	[Flags]
	public enum LoadLibraryFlags
	{
		/// <summary>
		/// <para>If this value is used, and the executable module is 
		/// a DLL, the system does not call DllMain for process and 
		/// thread initialization and termination. Also, the system 
		/// does not load additional executable modules that are 
		/// referenced by the specified module.</para>
		/// <para>If this value is not used, and the executable module 
		/// is a DLL, the system calls DllMain for process and thread 
		/// initialization and termination. The system loads additional 
		/// executable modules that are referenced by the specified 
		/// module.</para>
		/// <para>Windows Me/98/95: This value is not supported.
		/// </para></summary>
		DONT_RESOLVE_DLL_REFERENCES = 0x00000001,

		/// <summary>
		/// <para>If this value is used, the system maps the file into 
		/// the calling process's virtual address space as if it were 
		/// a data file. Nothing is done to execute or prepare to 
		/// execute the mapped file. Use this flag when you want to 
		/// load a DLL only to extract messages or resources from it.
		/// </para>
		/// <para>Windows Me/98/95: You can use the resulting module 
		/// handle only with resource management functions such as 
		/// EnumResourceLanguages, EnumResourceNames, EnumResourceTypes, 
		/// FindResource, FindResourceEx, LoadResource, and 
		/// SizeofResource. You cannot use this handle with specialized 
		/// resource management functions such as LoadBitmap, 
		/// LoadCursor, LoadIcon, LoadImage, and LoadMenu.</para>
		/// </summary>
		LOAD_LIBRARY_AS_DATAFILE = 0x00000002,

		/// <summary>
		/// <para>If this value is used, and lpFileName specifies a path, 
		/// the system uses the alternate file search strategy.</para>
		/// <para>If this value is not used, or if lpFileName does not 
		/// specify a path, the system uses the standard search strategy. 
		/// </para></summary>
		LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,

		/// <summary>
		/// <para>If this value is used, the system does not perform 
		/// automatic trust comparisons on the DLL or its dependents 
		/// when they are loaded.</para>
		/// <para>Windows 2000/NT, Windows Me/98/95:  This value is 
		/// not supported.</para></summary>
		LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010
	}
}