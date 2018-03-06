// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SystemMenuCommand.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// System Menu Command values.
	/// </summary>
	public enum SystemMenuCommand
	{
        /// <summary>
        /// 
        /// </summary>
		SC_SIZE = 0xF000,

        /// <summary>
        /// 
        /// </summary>
		SC_MOVE = 0xF010,

        /// <summary>
        /// 
        /// </summary>
		SC_MINIMIZE = 0xF020,

        /// <summary>
        /// 
        /// </summary>
		SC_MAXIMIZE = 0xF030,

        /// <summary>
        /// 
        /// </summary>
		SC_NEXTWINDOW = 0xF040,

        /// <summary>
        /// 
        /// </summary>
		SC_PREVWINDOW = 0xF050,

        /// <summary>
        /// 
        /// </summary>
		SC_CLOSE = 0xF060,

        /// <summary>
        /// 
        /// </summary>
		SC_VSCROLL = 0xF070,

        /// <summary>
        /// 
        /// </summary>
		SC_HSCROLL = 0xF080,

        /// <summary>
        /// 
        /// </summary>
		SC_MOUSEMENU = 0xF090,

        /// <summary>
        /// 
        /// </summary>
		SC_KEYMENU = 0xF100,

        /// <summary>
        /// 
        /// </summary>
		SC_ARRANGE = 0xF110,

        /// <summary>
        /// 
        /// </summary>
		SC_RESTORE = 0xF120,

        /// <summary>
        /// 
        /// </summary>
		SC_TASKLIST = 0xF130,

        /// <summary>
        /// 
        /// </summary>
		SC_SCREENSAVE = 0xF140,

        /// <summary>
        /// 
        /// </summary>
		SC_HOTKEY = 0xF150,

        /// <summary>
        /// 
        /// </summary>
		SC_DEFAULT = 0xF160,

        /// <summary>
        /// 
        /// </summary>
		SC_MONITORPOWER = 0xF170,

        /// <summary>
        /// 
        /// </summary>
		SC_CONTEXTHELP = 0xF180,

        /// <summary>
        /// 
        /// </summary>
		SC_SEPARATOR = 0xF00F
	}
}
