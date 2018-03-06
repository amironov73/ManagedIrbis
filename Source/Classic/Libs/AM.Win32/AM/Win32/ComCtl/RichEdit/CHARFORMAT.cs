// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CHARFORMAT.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [StructLayout ( LayoutKind.Sequential )]
    public struct CHARFORMAT
    {
		/// <summary>
		/// 
		/// </summary>
        public int cbSize;

		/// <summary>
		/// 
		/// </summary>
        public int dwMask;

		/// <summary>
		/// 
		/// </summary>
        public int dwEffects;

		/// <summary>
		/// 
		/// </summary>
        public int yHeight;

		/// <summary>
		/// 
		/// </summary>
        public int yOffset;

		/// <summary>
		/// 
		/// </summary>
        public int crTextColor;

		/// <summary>
		/// 
		/// </summary>
        public byte bCharSet;

		/// <summary>
		/// 
		/// </summary>
        public byte bPitchAndFamily;

		/// <summary>
		/// 
		/// </summary>
        [MarshalAs ( UnmanagedType.ByValTStr, SizeConst = 32 )]
        public string szFaceName;
    }
}
