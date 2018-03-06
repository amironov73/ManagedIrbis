// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SCROLLINFO.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The SCROLLINFO structure contains scroll bar parameters to be set by 
	/// the SetScrollInfo function (or SBM_SETSCROLLINFO message), or retrieved 
	/// by the GetScrollInfo function (or SBM_GETSCROLLINFO message).
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential, Size = SCROLLINFO.Size )]
	public struct SCROLLINFO
	{
		/// <summary>
		/// Size of structure, in bytes.
		/// </summary>
		public const int Size = 28;

		/// <summary>
		/// Specifies the size, in bytes, of this structure.
		/// </summary>
		int cbSize;

		/// <summary>
		/// Specifies the scroll bar parameters to set or retrieve.
		/// </summary>
		int fMask;

		/// <summary>
		/// Specifies the minimum scrolling position.
		/// </summary>
		int nMin;

		/// <summary>
		/// Specifies the maximum scrolling position.
		/// </summary>
		int nMax;

		/// <summary>
		/// Specifies the page size. A scroll bar uses this value to 
		/// determine the appropriate size of the proportional scroll box.
		/// </summary>
		int nPage;

		/// <summary>
		/// Specifies the position of the scroll box.
		/// </summary>
		int nPos;

		/// <summary>
		/// Specifies the immediate position of a scroll box that the user 
		/// is dragging. An application can retrieve this value while processing 
		/// the SB_THUMBTRACK request code. An application cannot set the 
		/// immediate scroll position; the SetScrollInfo function ignores 
		/// this member.
		/// </summary>
		int nTrackPos;
	}
}
