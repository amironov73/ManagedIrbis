// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReplaceFileFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

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
	/// File replacement options.
	/// </summary>
	[Flags]
	public enum ReplaceFileFlags
	{
        /// <summary>
        /// 
        /// </summary>
		REPLACEFILE_WRITE_THROUGH       = 0x00000001,

        /// <summary>
        /// 
        /// </summary>
		REPLACEFILE_IGNORE_MERGE_ERRORS = 0x00000002
	}
}
