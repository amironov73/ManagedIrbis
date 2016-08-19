/* WindowStyle.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The following styles can be specified wherever a window style 
	/// is required. After the control has been created, these styles 
	/// cannot be modified, except as noted.
	/// </summary>
	[Flags]
	public enum WindowStyle
	{
		/// <summary>
		/// Creates an overlapped window. An overlapped window has 
		/// a title bar and a border. Same as the WS_TILED style.
		/// </summary>
		WS_OVERLAPPED       = 0x00000000,

		/// <summary>
		/// Creates a pop-up window. This style cannot be used with 
		/// the WS_CHILD style.
		/// </summary>
		WS_POPUP            = unchecked((int)0x80000000),

		/// <summary>
		/// Creates a child window. A window with this style cannot 
		/// have a menu bar. This style cannot be used with the 
		/// WS_POPUP style.
		/// </summary>
		WS_CHILD            = 0x40000000,
		
		/// <summary>
		/// Creates a window that is initially minimized. 
		/// Same as the WS_ICONIC style.
		/// </summary>
		WS_MINIMIZE         = 0x20000000,
		
		/// <summary>
		/// <para>Creates a window that is initially visible.</para>
		/// <para>This style can be turned on and off by using 
		/// ShowWindow or SetWindowPos</para>
		/// </summary>
		WS_VISIBLE          = 0x10000000,

		/// <summary>
		/// Creates a window that is initially disabled. A disabled 
		/// window cannot receive input from the user. To change this 
		/// after a window has been created, use EnableWindow.
		/// </summary>
		WS_DISABLED         = 0x08000000,

		/// <summary>
		/// Clips child windows relative to each other; that is, 
		/// when a particular child window receives a WM_PAINT message, 
		/// the WS_CLIPSIBLINGS style clips all other overlapping child 
		/// windows out of the region of the child window to be updated. 
		/// If WS_CLIPSIBLINGS is not specified and child windows 
		/// overlap, it is possible, when drawing within the client area 
		/// of a child window, to draw within the client area of a 
		/// neighboring child window.
		/// </summary>
		WS_CLIPSIBLINGS     = 0x04000000,

		/// <summary>
		/// Excludes the area occupied by child windows when drawing 
		/// occurs within the parent window. This style is used when 
		/// creating the parent window.
		/// </summary>
		WS_CLIPCHILDREN     = 0x02000000,
		
		/// <summary>
		/// Creates a window that is initially maximized.
		/// </summary>
		WS_MAXIMIZE         = 0x01000000,

		/// <summary>
		/// Creates a window that has a title bar 
		/// (includes the WS_BORDER style).
		/// </summary>
		WS_CAPTION          = 0x00C00000,     /* WS_BORDER | WS_DLGFRAME  */
		
		/// <summary>
		/// Creates a window that has a thin-line border.
		/// </summary>
		WS_BORDER           = 0x00800000,

		/// <summary>
		/// Creates a window that has a border of a style typically 
		/// used with dialog boxes. A window with this style cannot 
		/// have a title bar.
		/// </summary>
		WS_DLGFRAME         = 0x00400000,
		
		/// <summary>
		/// Creates a window that has a vertical scroll bar.
		/// </summary>
		WS_VSCROLL          = 0x00200000,

		/// <summary>
		/// Creates a window that has a horizontal scroll bar.
		/// </summary>
		WS_HSCROLL          = 0x00100000,
		
		/// <summary>
		/// Creates a window that has a window menu on its title bar. 
		/// The WS_CAPTION style must also be specified.
		/// </summary>
		WS_SYSMENU          = 0x00080000,
		
		/// <summary>
		/// Creates a window that has a sizing border. 
		/// Same as the WS_SIZEBOX style.
		/// </summary>
		WS_THICKFRAME       = 0x00040000,

		/// <summary>
		/// <para>Specifies the first control of a group of controls. 
		/// The group consists of this first control and all controls 
		/// defined after it, up to the next control with the WS_GROUP 
		/// style. The first control in each group usually has the 
		/// WS_TABSTOP style so that the user can move from group to 
		/// group. The user can subsequently change the keyboard focus 
		/// from one control in the group to the next control in the 
		/// group by using the direction keys.</para>
		/// <para>You can turn this style on and off to change dialog 
		/// box navigation. To change this style after a window has 
		/// been created, use SetWindowLong.</para>
		/// </summary>
		WS_GROUP            = 0x00020000,

		/// <summary>
		/// <para>Specifies a control that can receive the keyboard 
		/// focus when the user presses the TAB key. Pressing the TAB 
		/// key changes the keyboard focus to the next control with 
		/// the WS_TABSTOP style.</para>
		/// <para>You can turn this style on and off to change dialog 
		/// box navigation. To change this style after a window has 
		/// been created, use SetWindowLong.</para>
		/// </summary>
		WS_TABSTOP          = 0x00010000,
		
		/// <summary>
		/// Creates a window that has a minimize button. 
		/// Cannot be combined with the WS_EX_CONTEXTHELP style. 
		/// The WS_SYSMENU style must also be specified.
		/// </summary>
		WS_MINIMIZEBOX      = 0x00020000,
		
		/// <summary>
		/// Creates a window that has a maximize button. 
		/// Cannot be combined with the WS_EX_CONTEXTHELP style. 
		/// The WS_SYSMENU style must also be specified.
		/// </summary>
		WS_MAXIMIZEBOX      = 0x00010000,
		
		/// <summary>
		/// Creates an overlapped window. An overlapped window has a 
		/// title bar and a border. Same as the WS_OVERLAPPED style.
		/// </summary>
		WS_TILED            = WS_OVERLAPPED,
		
		/// <summary>
		/// Creates a window that is initially minimized. 
		/// Same as the WS_MINIMIZE style.
		/// </summary>
		WS_ICONIC           = WS_MINIMIZE,
		
		/// <summary>
		/// Creates a window that has a sizing border. 
		/// Same as the WS_THICKFRAME style.
		/// </summary>
		WS_SIZEBOX          = WS_THICKFRAME,

		/// <summary>
		/// Creates an overlapped window with the WS_OVERLAPPED, 
		/// WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, 
		/// and WS_MAXIMIZEBOX styles. Same as the WS_OVERLAPPEDWINDOW 
		/// style.
		/// </summary>
		WS_TILEDWINDOW      = WS_OVERLAPPEDWINDOW,
		
		/// <summary>
		/// Creates an overlapped window with the WS_OVERLAPPED, 
		/// WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, 
		/// and WS_MAXIMIZEBOX styles. Same as the WS_TILEDWINDOW style.
		/// </summary>
		WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED     | 
                             WS_CAPTION        | 
                             WS_SYSMENU        | 
                             WS_THICKFRAME     | 
                             WS_MINIMIZEBOX    | 
                             WS_MAXIMIZEBOX),
		
		/// <summary>
		/// Creates a pop-up window with WS_BORDER, WS_POPUP, 
		/// and WS_SYSMENU styles. The WS_CAPTION and WS_POPUPWINDOW 
		/// styles must be combined to make the window menu visible.
		/// </summary>
		WS_POPUPWINDOW      = (WS_POPUP          | 
                             WS_BORDER         | 
                             WS_SYSMENU),

		/// <summary>
		/// Same as the WS_CHILD style.
		/// </summary>
		WS_CHILDWINDOW      = (WS_CHILD)
	}
}
