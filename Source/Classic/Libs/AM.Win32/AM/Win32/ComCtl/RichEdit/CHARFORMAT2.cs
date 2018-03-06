// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CHARFORMAT2.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [StructLayout ( LayoutKind.Sequential )]
    public struct CHARFORMAT2cs
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

		/// <summary>
		/// 
		/// </summary>
        public short wWeight;

		/// <summary>
		/// 
		/// </summary>
        public short sSpacing;

		/// <summary>
		/// 
		/// </summary>
        public int crBackColor;

		/// <summary>
		/// 
		/// </summary>
        public int lcid;

		/// <summary>
		/// 
		/// </summary>
        public int dwReserved;

		/// <summary>
		/// 
		/// </summary>
        public short sStyle;

		/// <summary>
		/// 
		/// </summary>
        public short wKerning;

		/// <summary>
		/// 
		/// </summary>
        public byte bUnderlineType;

		/// <summary>
		/// 
		/// </summary>
        public byte bAnimation;

		/// <summary>
		/// 
		/// </summary>
        public byte bRevAuthor;

		/// <summary>
		/// 
		/// </summary>
        public byte bReserved1;
    }
}
