// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GetObjectOptions.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum GetObjectOptions
    {
		/// <summary>
		/// 
		/// </summary>
        REO_GETOBJ_NO_INTERFACES = 0x00000000,

		/// <summary>
		/// 
		/// </summary>
        REO_GETOBJ_POLEOBJ = 0x00000001,

		/// <summary>
		/// 
		/// </summary>
        REO_GETOBJ_PSTG = 0x00000002,

		/// <summary>
		/// 
		/// </summary>
        REO_GETOBJ_POLESITE = 0x00000004,

		/// <summary>
		/// 
		/// </summary>
        REO_GETOBJ_ALL_INTERFACES = 0x00000007
    }
}
