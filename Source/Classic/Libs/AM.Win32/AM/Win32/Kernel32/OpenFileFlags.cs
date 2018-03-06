// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OpenFileFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// </summary>
	[Flags]
	[CLSCompliant ( false )]
	public enum OpenFileFlags : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		OF_READ = 0x0000,

		/// <summary>
		/// 
		/// </summary>
		OF_WRITE = 0x0001,

		/// <summary>
		/// 
		/// </summary>
		OF_READWRITE = 0x0002,

		/// <summary>
		/// 
		/// </summary>
		OF_SHARE_COMPAT = 0x0000,

		/// <summary>
		/// 
		/// </summary>
		OF_SHARE_EXCLUSIVE = 0x0010,

		/// <summary>
		/// 
		/// </summary>
		OF_SHARE_DENY_WRITE = 0x0020,

		/// <summary>
		/// 
		/// </summary>
		OF_SHARE_DENY_READ = 0x0030,

		/// <summary>
		/// 
		/// </summary>
		OF_SHARE_DENY_NONE = 0x0040,

		/// <summary>
		/// 
		/// </summary>
		OF_PARSE = 0x0100,

		/// <summary>
		/// 
		/// </summary>
		OF_DELETE = 0x0200,

		/// <summary>
		/// 
		/// </summary>
		OF_VERIFY = 0x0400,

		/// <summary>
		/// 
		/// </summary>
		OF_CANCEL = 0x0800,

		/// <summary>
		/// 
		/// </summary>
		OF_CREATE = 0x1000,

		/// <summary>
		/// 
		/// </summary>
		OF_PROMPT = 0x2000,

		/// <summary>
		/// 
		/// </summary>
		OF_EXIST = 0x4000,

		/// <summary>
		/// 
		/// </summary>
		OF_REOPEN = 0x8000
	}
}