// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* User32.cs -- wrapper for USER32.DLL API. 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Wrapper for USER32.DLL API.
	/// </summary>
	public static class User32
	{
		#region Constants

		/// <summary>
		/// Default window position for CreateWindow().
		/// </summary>
		const int CW_USEDEFAULT = unchecked ( (int) 0x80000000 );

		/// <summary>
		/// Default value for TrackMouseEvent().
		/// </summary>
		const uint HOVER_DEFAULT = 0xFFFFFFFF;

		/// <summary>
		/// Allow set foreground window for all processes 
		/// (see AllowSetForegroundWindow function).
		/// </summary>
		const uint ASFW_ANY = 0xFFFFFFFF;

		/// <summary>
		/// Disables calls to SetForegroundWindow 
		/// (see LockSetForegroundWindow).
		/// </summary>
		const int LSFW_LOCK = 1;

		/// <summary>
		/// Enables calls to SetForegroundWindow
		/// (see LockSetForegroundWindow).
		/// </summary>
		const int LSFW_UNLOCK = 2;

		/// <summary>
		/// Broadcast window.
		/// </summary>
		public static readonly IntPtr HWND_BROADCAST = new IntPtr ( 0xffff );

		/// <summary>
		/// Message-only window.
		/// </summary>
		public static readonly IntPtr HWND_MESSAGE = new IntPtr ( -3 );

		/// <summary>
		/// Places the window at the top of the Z order
		/// (see DeferWindowPos function).
		/// </summary>
		public static readonly IntPtr HWND_TOP = new IntPtr ( 0 );

		/// <summary>
		/// Places the window at the bottom of the Z order. 
		/// If the hWnd parameter identifies a topmost window, 
		/// the window loses its topmost status and is placed at the 
		/// bottom of all other windows (see DeferWindowPos function).
		/// </summary>
		public static readonly IntPtr HWND_BOTTOM = new IntPtr ( 1 );

		/// <summary>
		/// Places the window above all non-topmost windows. The window 
		/// maintains its topmost position even when it is deactivated
		/// (see DeferWindowPos function).
		/// </summary>
		public static readonly IntPtr HWND_TOPMOST = new IntPtr ( -1 );

		/// <summary>
		/// Places the window above all non-topmost windows (that is, 
		/// behind all topmost windows). This flag has no effect if 
		/// the window is already a non-topmost window (see
		/// DeferWindowPos function).
		/// </summary>
		public static readonly IntPtr HWND_NOTOPMOST = new IntPtr ( -2 );

		/// <summary>
		/// Infinite wait interval.
		/// </summary>
        [CLSCompliant ( false )]
        public const uint INFINITE = 0xFFFFFFFF;

		#endregion

		/// <summary>
		/// Dll Name.
		/// </summary>
		public const string DllName = "User32.dll";

		/// <summary>
		///	Stops a system shutdown started by using the 
		/// InitiateSystemShutdown function.
		/// </summary>
		/// <param name="lpMachineName">Pointer to the null-terminated 
		/// string that specifies the network name of the computer 
		/// where the shutdown is to be stopped. If lpMachineName is 
		/// NULL or an empty string, the function stops the shutdown 
		/// on the local computer.</param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		/// <remarks><para>Included in: Windows NT/XP/2000/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool AbortSystemShutdown
			(
				string lpMachineName
			);

		/// <summary>
		/// <para>The AdjustWindowRect function calculates the required 
		/// size of the window rectangle, based on the desired 
		/// client-rectangle size. The window rectangle can then be 
		/// passed to the CreateWindow function to create a window 
		/// whose client area is the desired size.</para>
		/// <para>To specify an extended window style, use the 
		/// AdjustWindowRectEx function.</para>
		/// </summary>
		/// <param name="lpRect">Pointer to a Rectangle structure that 
		/// contains the coordinates of the top-left and bottom-right 
		/// corners of the desired client area. When the function 
		/// returns, the structure contains the coordinates of the 
		/// top-left and bottom-right corners of the window to 
		/// accommodate the desired client area.</param>
		/// <param name="dwStyle">Specifies the window style of the 
		/// window whose required size is to be calculated. Note that 
		/// you cannot specify the WS_OVERLAPPED style.</param>
		/// <param name="bMenu">Specifies whether the window has a menu.
		/// </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>
		/// <para>A client rectangle is the smallest rectangle that 
		/// completely encloses a client area. A window rectangle is 
		/// the smallest rectangle that completely encloses the window, 
		/// which includes the client area and the nonclient area.
		/// </para>
		/// <para>The AdjustWindowRect function does not add extra 
		/// space when a menu bar wraps to two or more rows.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool AdjustWindowRect
			(
				ref Rectangle lpRect,
				WindowStyle dwStyle,
				bool bMenu
			);

		/// <summary>
		/// The AdjustWindowRectEx function calculates the required 
		/// size of the window rectangle, based on the desired size 
		/// of the client rectangle. The window rectangle can then be 
		/// passed to the CreateWindowEx function to create a window 
		/// whose client area is the desired size.
		/// </summary>
		/// <param name="lpRect">Pointer to a Rectangle structure that 
		/// contains the coordinates of the top-left and bottom-right 
		/// corners of the desired client area. When the function 
		/// returns, the structure contains the coordinates of the 
		/// top-left and bottom-right corners of the window to 
		/// accommodate the desired client area.</param>
		/// <param name="dwStyle">Specifies the window style of the 
		/// window whose required size is to be calculated. Note that 
		/// you cannot specify the WS_OVERLAPPED style.</param>
		/// <param name="bMenu">Specifies whether the window has a menu.
		/// </param>
		/// <param name="dwExStyle">Specifies the extended window style 
		/// of the window whose required size is to be calculated. 
		/// For more information, see CreateWindowEx.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>
		/// <para>A client rectangle is the smallest rectangle that 
		/// completely encloses a client area. A window rectangle is 
		/// the smallest rectangle that completely encloses the window, 
		/// which includes the client area and the nonclient area.</para>
		/// <para>The AdjustWindowRectEx function does not add extra 
		/// space when a menu bar wraps to two or more rows.</para>
		/// <para>The AdjustWindowRectEx function does not take the 
		/// WS_VSCROLL or WS_HSCROLL styles into account. To account 
		/// for the scroll bars, call the GetSystemMetrics function 
		/// with SM_CXVSCROLL or SM_CYHSCROLL.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool AdjustWindowRectEx
			(
				ref Rectangle lpRect,
				WindowStyle dwStyle,
				bool bMenu,
				ExtendedWindowStyle dwExStyle
			);

		/// <summary>
		/// Enables the specified process to set the foreground 
		/// window using the SetForegroundWindow function. The 
		/// calling process must already be able to set the 
		/// foreground window.
		/// </summary>
		/// <param name="dwProcessId">Specifies the identifier 
		/// of the process that will be enabled to set the 
		/// foreground window. If this parameter is ASFW_ANY, 
		/// all processes will be enabled to set the foreground 
		/// window.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>
		/// <para>Starting with Microsoft® Windows® 98 and Windows 2000, 
		/// the system restricts which processes can set the foreground 
		/// window. A process can set the foreground window only if one 
		/// of the following conditions is true:</para>
		/// <list type="bullet">
		/// <item>The process is the foreground process.</item>
		/// <item>The process was started by the foreground process.
		/// </item>
		/// <item>The process received the last input event.</item>
		/// <item>There is no foreground process.</item>
		/// <item>The foreground process is being debugged.</item>
		/// <item>The foreground is not locked (see 
		/// LockSetForegroundWindow).</item>
		/// <item>The foreground lock time-out has expired 
		/// (see SPI_GETFOREGROUNDLOCKTIMEOUT in SystemParametersInfo).
		/// </item>
		/// <item>Windows 2000/XP: No menus are active.</item>
		/// </list>
		/// <para>A process that can set the foreground window can 
		/// enable another process to set the foreground window by 
		/// calling AllowSetForegroundWindow. The process specified 
		/// by dwProcessId loses the ability to set the foreground 
		/// window the next time the user generates input, unless the 
		/// input is directed at that process, or the next time a 
		/// process calls AllowSetForegroundWindow, unless that 
		/// process is specified.</para>
		/// <para>Windows 95/98/Me: This function is not implemented. 
		/// Therefore, processes must cooperate to manage the 
		/// foreground window. For example, an application may wish 
		/// to support only one instance. When the second instance 
		/// starts up, it should detect the previous instance and 
		/// call SetForegroundWindow on the window of the previous 
		/// instance. It should not post a message to the window 
		/// of the previous instance asking it to call 
		/// SetForegroundWindow on itself, because the previous 
		/// instance will not necessarily have permission to call 
		/// SetForegroundWindow.</para>
		/// <para>Included in: Windows ME/XP/2000/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool AllowSetForegroundWindow
			(
				uint dwProcessId
			);

		/// <summary>
		/// The AnimateWindow function enables you to produce special 
		/// effects when showing or hiding windows. There are four 
		/// types of animation: roll, slide, collapse or expand, and 
		/// alpha-blended fade.
		/// </summary>
		/// <param name="hwnd">Handle to the window to animate. The 
		/// calling thread must own this window.</param>
		/// <param name="dwTime">Specifies how long it takes to play 
		/// the animation, in milliseconds. Typically, an animation 
		/// takes 200 milliseconds to play.</param>
		/// <param name="dwFlags">Specifies the type of animation. 
		/// This parameter can be one or more of the following values. 
		/// Note that, by default, these flags take effect when showing 
		/// a window. To take effect when hiding a window, use AW_HIDE 
		/// and a logical OR operator with the appropriate flags.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool AnimateWindow
			(
				IntPtr hwnd,
				uint dwTime,
				AnimateWindowFlags dwFlags
			);

		/// <summary>
		/// Arranges all the minimized (iconic) child windows of the 
		/// specified parent window.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// height of one row of icons. If the function fails, the return 
		/// value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern uint ArrangeIconicWindows
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Allocates memory for a multiple-window- position structure 
		/// and returns the handle to the structure.
		/// </summary>
		/// <param name="nNumWindows">Specifies the initial number 
		/// of windows for which to store position information. 
		/// The DeferWindowPos function increases the size of 
		/// the structure, if necessary.</param>
		/// <returns>If the function succeeds, the return value 
		/// identifies the multiple-window-position structure. 
		/// If insufficient system resources are available to allocate 
		/// the structure, the return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr BeginDeferWindowPos
			(
				int nNumWindows
			);

		/// <summary>
		/// Prepares the specified window for painting and fills a PAINTSTRUCT 
		/// structure with information about the painting.
		/// </summary>
		/// <param name="hwnd">Handle to the window to be repainted.</param>
		/// <param name="lpPaint">Pointer to the PAINTSTRUCT structure that 
		/// will receive painting information.</param>
		/// <returns>If the function succeeds, the return value is the handle 
		/// to a display device context for the specified window.
		/// If the function fails, the return value is NULL, indicating that 
		/// no display device context is available.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr BeginPaint 
			(
				IntPtr hwnd,
				out PAINTSTRUCT lpPaint
			);

		/// <summary>
		/// The BlockInput function blocks keyboard and mouse input events 
		/// from reaching applications. 
		/// </summary>
		/// <param name="fBlockIt">Specifies the function's purpose. 
		/// If this parameter is TRUE, keyboard and mouse input events 
		/// are blocked. If this parameter is FALSE, keyboard and mouse 
		/// events are unblocked. Note that only the thread that blocked 
		/// input can successfully unblock input. </param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool BlockInput 
			( 
				bool fBlockIt
			);

		/// <summary>
		/// The BringWindowToTop function brings the specified window to 
		/// the top of the Z order. If the window is a top-level window, 
		/// it is activated. If the window is a child window, the top-level 
		/// parent window associated with the child window is activated. 
		/// </summary>
		/// <param name="hWnd">Handle to the window to bring to the top of 
		/// the Z order.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool BringWindowToTop 
			( 
				IntPtr hWnd
			);

		/// <summary>
		/// Sends a message to the specified recipients. The recipients 
		/// can be applications, installable drivers, network drivers, 
		/// system-level device drivers, or any combination of these 
		/// system components. 
		/// </summary>
		/// <param name="dwFlags">Specifies the broadcast option.</param>
		/// <param name="lpdwRecipients"><para>Pointer to a variable that 
		/// contains and receives information about the recipients of 
		/// the message.</para>
		/// <para>When the function returns, this variable receives a 
		/// combination of these values identifying which recipients 
		/// actually received the message.</para>
		/// <para>If this parameter is NULL, the function broadcasts to 
		/// all components.</para></param>
		/// <param name="uiMessage">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is a 
		/// positive value.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int BroadcastSystemMessage
			(
				BroadcastSystemMessageFlags dwFlags,
				ref BroadcastSystemMessageRecipient lpdwRecipients,
				int uiMessage,
				int wParam,
				int lParam
			);

		/// <summary>
		/// The CallMsgFilter function passes the specified message 
		/// and hook code to the hook procedures associated with 
		/// the WH_SYSMSGFILTER and WH_MSGFILTER hooks. A WH_SYSMSGFILTER 
		/// or WH_MSGFILTER hook procedure is an application-defined 
		/// callback function that examines and, optionally, modifies 
		/// messages for a dialog box, message box, menu, or scroll bar.
		/// </summary>
		/// <param name="lpMsg">Pointer to an MSG structure that contains 
		/// the message to be passed to the hook procedures.</param>
		/// <param name="nCode">Specifies an application-defined code used 
		/// by the hook procedure to determine how to process the message. 
		/// The code must not have the same value as system-defined hook 
		/// codes (MSGF_ and HC_) associated with the WH_SYSMSGFILTER and 
		/// WH_MSGFILTER hooks.</param>
		/// <returns>If the application should process the message further, 
		/// the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool CallMsgFilter 
			( 
				ref MSG lpMsg,
				int nCode
			);

		/// <summary>
		/// The CallNextHookEx function passes the hook information to 
		/// the next hook procedure in the current hook chain. A hook 
		/// procedure can call this function either before or after 
		/// processing the hook information.
		/// </summary>
		/// <param name="hhk">Handle to the current hook. An application 
		/// receives this handle as a result of a previous call to the 
		/// SetWindowsHookEx function.</param>
		/// <param name="nCode">Specifies the hook code passed to the 
		/// current hook procedure. The next hook procedure uses this 
		/// code to determine how to process the hook information.</param>
		/// <param name="wParam">Specifies the wParam value passed to the 
		/// current hook procedure. The meaning of this parameter depends 
		/// on the type of hook associated with the current hook chain.
		/// </param>
		/// <param name="lParam">Specifies the lParam value passed to the 
		/// current hook procedure. The meaning of this parameter depends 
		/// on the type of hook associated with the current hook chain.
		/// </param>
		/// <returns>This value is returned by the next hook procedure 
		/// in the chain. The current hook procedure must also return 
		/// this value. The meaning of the return value depends on the 
		/// hook type. For more information, see the descriptions of the 
		/// individual hook procedures.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int CallNextHookEx 
			( 
				IntPtr hhk,
				int nCode,
				int wParam,
				int lParam
			);

		/// <summary>
		/// Passes message information to the specified window procedure.
		/// </summary>
		/// <param name="lpPrevWndFunc">Pointer to the previous window 
		/// procedure. If this value is obtained by calling the GetWindowLong 
		/// function with the nIndex parameter set to GWL_WNDPROC or 
		/// DWL_DLGPROC, it is actually either the address of a window or 
		/// dialog box procedure, or a special internal value meaningful only 
		/// to CallWindowProc.</param>
		/// <param name="hWnd">Handle to the window procedure to receive 
		/// the message.</param>
		/// <param name="Msg">Specifies the message.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>The return value specifies the result of the message 
		/// processing and depends on the message sent.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int CallWindowProc 
			( 
				WindowProc lpPrevWndFunc,
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// Cascades the specified child windows of the specified parent window.
		/// </summary>
		/// <param name="hwndParent">Handle to the parent window. If this 
		/// parameter is NULL, the desktop window is assumed.</param>
		/// <param name="wHow">Specifies a cascade flag.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that specifies 
		/// the rectangular area, in client coordinates, within which the 
		/// windows are arranged. This parameter can be NULL, in which case 
		/// the client area of the parent window is used.</param>
		/// <param name="cKids">Specifies the number of elements in the array 
		/// specified by the lpKids parameter. This parameter is ignored if 
		/// lpKids is NULL.</param>
		/// <param name="lpKids"> Pointer to an array of handles to the child 
		/// windows to arrange. If this parameter is NULL, all child windows 
		/// of the specified parent window (or of the desktop window) are 
		/// arranged.</param>
		/// <returns>If the function succeeds, the return value is the number 
		/// of windows arranged.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern short CascadeWindows
			(          
				IntPtr hwndParent,
				int wHow,
				ref Rectangle lpRect,
				int cKids,
				IntPtr [] lpKids
			);

		/// <summary>
		/// The ChangeDisplaySettings function changes the settings of the 
		/// default display device to the specified graphics mode.
		/// To change the settings of a specified display device, 
		/// use the ChangeDisplaySettingsEx function.
		/// </summary>
		/// <param name="lpDevMode"><para>Pointer to a DEVMODE structure 
		/// that describes the new graphics mode. If lpDevMode is NULL, 
		/// all the values currently in the registry will be used for 
		/// the display setting. Passing NULL for the lpDevMode parameter 
		/// and 0 for the dwFlags parameter is the easiest way to return 
		/// to the default mode after a dynamic mode change.</para>
		/// <para>The dmSize member of DEVMODE must be initialized to the 
		/// size, in bytes, of the DEVMODE structure. The dmDriverExtra 
		/// member of DEVMODE must be initialized to indicate the number 
		/// of bytes of private driver data following the DEVMODE structure. 
		/// In addition, you can use any or all of members 
		/// of the DEVMODE structure.</para></param>
		/// <param name="dwflags"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true,
			EntryPoint = "ChangeDisplaySettingsW" )]
		public static extern int ChangeDisplaySettings 
			(
				ref DEVMODEW_DISPLAY lpDevMode,
				ChangeDisplaySettingsFlags dwflags
			);

		/// <summary>
		/// The ChildWindowFromPoint function determines which, if any, 
		/// of the child windows belonging to a parent window contains 
		/// the specified point. The search is restricted to immediate 
		/// child windows, grandchildren, and deeper descendant windows 
		/// are not searched.
		/// </summary>
		/// <param name="hWndParent">Handle to the parent window.</param>
		/// <param name="Point">Specifies a POINT structure that defines 
		/// the client coordinates (relative to hWndParent of the point 
		/// to be checked.</param>
		/// <returns>The return value is a handle to the child window that 
		/// contains the point, even if the child window is hidden or disabled. 
		/// If the point lies outside the parent window, the return value is 
		/// NULL. If the point is within the parent window but not within 
		/// any child window, the return value is a handle to the parent 
		/// window.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern IntPtr ChildWindowFromPoint 
			( 
				IntPtr hWndParent,
				Point Point
			);

		/// <summary>
		/// Minimizes (but does not destroy) the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be minimized.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool CloseWindow 
			( 
				IntPtr hWnd
			);

		/// <summary>
		/// Creates an overlapped, pop-up, or child window with an 
		/// extended window style.
		/// </summary>
		/// <param name="dwExStyle">Specifies the extended window 
		/// style of the window being created.</param>
		/// <param name="lpClassName">Pointer to a null-terminated 
		/// string or a class atom created by a previous call to the 
		/// RegisterClass or RegisterClassEx function. The atom must 
		/// be in the low-order word of lpClassName; the high-order 
		/// word must be zero. If lpClassName is a string, 
		/// it specifies the window class name. The class name can 
		/// be any name registered with RegisterClass or 
		/// RegisterClassEx, provided that the module that registers 
		/// the class is also the module that creates the window. 
		/// The class name can also be any of the predefined system 
		/// class names.</param>
		/// <param name="lpWindowName">Pointer to a null-terminated string 
		/// that specifies the window name. If the window style specifies 
		/// a title bar, the window title pointed to by lpWindowName 
		/// is displayed in the title bar. When using CreateWindow to 
		/// create controls, such as buttons, check boxes, and static 
		/// controls, use lpWindowName to specify the text of the control. 
		/// When creating a static control with the SS_ICON style, use 
		/// lpWindowName to specify the icon name or identifier. 
		/// To specify an identifier, use the syntax "#num".</param>
		/// <param name="dwStyle">Specifies the style of the window 
		/// being created. This parameter can be a combination of 
		/// window styles, plus the control styles indicated in the Remarks section.</param>
		/// <param name="x">Specifies the initial horizontal position of 
		/// the window. For an overlapped or pop-up window, the x 
		/// parameter is the initial x-coordinate of the window's 
		/// upper-left corner, in screen coordinates. For a child 
		/// window, x is the x-coordinate of the upper-left corner 
		/// of the window relative to the upper-left corner of the 
		/// parent window's client area. If x is set to CW_USEDEFAULT, 
		/// the system selects the default position for the window's 
		/// upper-left corner and ignores the y parameter. CW_USEDEFAULT 
		/// is valid only for overlapped windows; if it is specified 
		/// for a pop-up or child window, the x and y parameters are 
		/// set to zero.</param>
		/// <param name="y">Specifies the initial vertical position of 
		/// the window. For an overlapped or pop-up window, the y 
		/// parameter is the initial y-coordinate of the window's 
		/// upper-left corner, in screen coordinates. For a child 
		/// window, y is the initial y-coordinate of the upper-left 
		/// corner of the child window relative to the upper-left 
		/// corner of the parent window's client area. For a list box 
		/// y is the initial y-coordinate of the upper-left corner of 
		/// the list box's client area relative to the upper-left corner 
		/// of the parent window's client area. If an overlapped window 
		/// is created with the WS_VISIBLE style bit set and the x 
		/// parameter is set to CW_USEDEFAULT, the system ignores the 
		/// y parameter.</param>
		/// <param name="nWidth">Specifies the width, in device units, 
		/// of the window. For overlapped windows, nWidth is the window's 
		/// width, in screen coordinates, or CW_USEDEFAULT. If nWidth is 
		/// CW_USEDEFAULT, the system selects a default width and height 
		/// for the window; the default width extends from the initial 
		/// x-coordinates to the right edge of the screen; the default 
		/// height extends from the initial y-coordinate to the top of 
		/// the icon area. CW_USEDEFAULT is valid only for overlapped 
		/// windows; if CW_USEDEFAULT is specified for a pop-up or child 
		/// window, the nWidth and nHeight parameter are set to zero.
		/// </param>
		/// <param name="nHeight">Specifies the height, in device units, 
		/// of the window. For overlapped windows, nHeight is the window's 
		/// height, in screen coordinates. If the nWidth parameter is 
		/// set to CW_USEDEFAULT, the system ignores nHeight.</param>
		/// <param name="hWndParent"><para>Handle to the parent or owner 
		/// window of the window being created. To create a child window 
		/// or an owned window, supply a valid window handle. This 
		/// parameter is optional for pop-up windows.</para>
		/// <para>Windows 2000/XP: To create a message-only window, 
		/// supply HWND_MESSAGE or a handle to an existing message-only 
		/// window.</para></param>
		/// <param name="hMenu">Handle to a menu, or specifies a 
		/// child-window identifier, depending on the window style. 
		/// For an overlapped or pop-up window, hMenu identifies the 
		/// menu to be used with the window; it can be NULL if the 
		/// class menu is to be used. For a child window, hMenu 
		/// specifies the child-window identifier, an integer value 
		/// used by a dialog box control to notify its parent about 
		/// events. The application determines the child-window 
		/// identifier; it must be unique for all child windows with 
		/// the same parent window.</param>
		/// <param name="hInstance"><para>Windows 95/98/Me: Handle to 
		/// the instance of the module to be associated with the window.
		/// </para>
		/// <para>Windows NT/2000/XP: This value is ignored.</para>
		/// </param>
		/// <param name="lpParam">Pointer to a value to be passed to 
		/// the window through the CREATESTRUCT structure passed in the 
		/// lpParam parameter the WM_CREATE message. If an application 
		/// calls CreateWindow to create a MDI client window, lpParam 
		/// must point to a CLIENTCREATESTRUCT structure.</param>
		/// <returns><para>If the function succeeds, the return value 
		/// is a handle to the new window.</para>
		/// <para>If the function fails, the return value is NULL.</para>
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr CreateWindowEx
			(
				ExtendedWindowStyle dwExStyle,
				string lpClassName,
				string lpWindowName,
				WindowStyle dwStyle,
				int x,
				int y,
				int nWidth,
				int nHeight,
				IntPtr hWndParent,
				IntPtr hMenu,
				IntPtr hInstance,
				int lpParam
			);

		/// <summary>
		/// The DeferWindowPos function updates the specified 
		/// multiple-window – position structure for the specified 
		/// window. The function then returns a handle to the updated 
		/// structure. The EndDeferWindowPos function uses the 
		/// information in this structure to change the position and 
		/// size of a number of windows simultaneously. The 
		/// BeginDeferWindowPos function creates the structure.
		/// </summary>
		/// <param name="hWinPosInfo">Handle to a multiple-window – 
		/// position structure that contains size and position 
		/// information for one or more windows. This structure is 
		/// returned by BeginDeferWindowPos or by the most recent call 
		/// to DeferWindowPos.</param>
		/// <param name="hWnd">Handle to the window for which update 
		/// information is stored in the structure. All windows in a 
		/// multiple-window – position structure must have the same 
		/// parent.</param>
		/// <param name="hWndInsertAfter"><para>Handle to the window that 
		/// precedes the positioned window in the Z order. This parameter 
		/// must be a window handle or one of the following values:</para>
		/// <list type="bullet">
		/// <item>HWND_BOTTOM</item>
		/// <item>HWND_NOTOPMOST</item>
		/// <item>HWND_TOP</item>
		/// <item>HWND_TOPMOST</item>
		/// </list>
		/// <para>This parameter is ignored if the SWP_NOZORDER flag is 
		/// set in the uFlags parameter.</para>
		/// </param>
		/// <param name="x">Specifies the x-coordinate of the window's 
		/// upper-left corner.</param>
		/// <param name="y">Specifies the y-coordinate of the window's 
		/// upper-left corner.</param>
		/// <param name="cx">Specifies the window's new width, 
		/// in pixels.</param>
		/// <param name="cy">Specifies the window's new height, 
		/// in pixels.</param>
		/// <param name="uFlags">Specifies a combination of the 
		/// values that affect the size and position of the window.
		/// </param>
		/// <returns>The return value identifies the updated 
		/// multiple-window – position structure. The handle 
		/// returned by this function may differ from the handle 
		/// passed to the function. The new handle that this function 
		/// returns should be passed during the next call to the 
		/// DeferWindowPos or EndDeferWindowPos function. If insufficient 
		/// system resources are available for the function to succeed, 
		/// the return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr DeferWindowPos
			(
				IntPtr hWinPosInfo,
				IntPtr hWnd,
				IntPtr hWndInsertAfter,
				int x,
				int y,
				int cx,
				int cy,
				SetWindowPosFlags uFlags
			);

		/// <summary>
		/// The DefWindowProc function calls the default window procedure 
		/// to provide default processing for any window messages that an 
		/// application does not process. This function ensures that every 
		/// message is processed. DefWindowProc is called with the same 
		/// parameters received by the window procedure.
		/// </summary>
		/// <param name="hWnd">Handle to the window procedure that received 
		/// the message.</param>
		/// <param name="Msg">Specifies the message.</param>
		/// <param name="wParam">Specifies additional message information.</param>
		/// <param name="lParam">Specifies additional message information.</param>
		/// <returns>The return value is the result of the message processing 
		/// and depends on the message.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int DefWindowProc 
			( 
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// <para>The DestroyWindow function destroys the specified 
		/// window. The function sends WM_DESTROY and WM_NCDESTROY 
		/// messages to the window to deactivate it and remove the 
		/// keyboard focus from it. The function also destroys the 
		/// window's menu, flushes the thread message queue, destroys 
		/// timers, removes clipboard ownership, and breaks the 
		/// clipboard viewer chain (if the window is at the top of 
		/// the viewer chain).</para>
		/// <para>If the specified window is a parent or owner window, 
		/// DestroyWindow automatically destroys the associated child 
		/// or owned windows when it destroys the parent or owner window.
		/// The function first destroys child or owned windows, and then 
		/// it destroys the parent or owner window.</para>
		/// <para>DestroyWindow also destroys modeless dialog boxes 
		/// created by the CreateDialog function.</para>
		/// </summary>
		/// <param name="hWnd">Handle to the window to be destroyed.
		/// </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>A thread cannot use DestroyWindow to destroy a window 
		/// created by a different thread.</remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DestroyWindow
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The DispatchMessage function dispatches a message to a window 
		/// procedure. It is typically used to dispatch a message retrieved 
		/// by the GetMessage function.
		/// </summary>
		/// <param name="lpmsg">Pointer to an MSG structure that contains 
		/// the message.</param>
		/// <returns>The return value specifies the value returned by the 
		/// window procedure. Although its meaning depends on the message 
		/// being dispatched, the return value generally is ignored.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int DispatchMessage
			(
				ref MSG lpmsg
			);

		/// <summary>
		/// Draws a wire-frame rectangle and animates it to indicate 
		/// the opening of an icon or the minimizing or maximizing of a window.
		/// </summary>
		/// <param name="hwnd">Handle to the window to which the rectangle 
		/// is clipped. If this parameter is NULL, the working area of the 
		/// screen is used.</param>
		/// <param name="idAni">Specifies the type of animation. 
		/// If you specify IDANI_CAPTION, the window caption will animate 
		/// from the position specified by lprcFrom to the position specified 
		/// by lprcTo. The effect is similar to minimizing or maximizing 
		/// a window.</param>
		/// <param name="lprcFrom">Pointer to a Rectangle structure specifying 
		/// the location and size of the icon or minimized window. 
		/// Coordinates are relative to the clipping window hwnd.</param>
		/// <param name="lprcTo">Pointer to a Rectangle structure specifying 
		/// the location and size of the restored window. Coordinates are 
		/// relative to the clipping window hwnd.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawAnimatedRects
			(
				IntPtr hwnd,
				AnimatedRectangleFlags idAni,
				ref Rectangle lprcFrom,
				ref Rectangle lprcTo
			);

		/// <summary>
		/// Draws a window caption.
		/// </summary>
		/// <param name="hwnd">Handle to a window that supplies text 
		/// and an icon for the window caption.</param>
		/// <param name="hdc"> Handle to a device context. 
		/// The function draws the window caption into this device context.
		/// </param>
		/// <param name="lprc">Pointer to a Rectangle structure that specifies 
		/// the bounding rectangle for the window caption in logical 
		/// coordinates.</param>
		/// <param name="uFlags">Specifies drawing options.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawCaption 
			(
				IntPtr hwnd,
				IntPtr hdc,
				ref Rectangle lprc,
				DrawCaptionFlags uFlags
			);

		/// <summary>
		/// Draws one or more edges of rectangle.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="qrc">Pointer to a Rectangle structure that contains 
		/// the logical coordinates of the rectangle.</param>
		/// <param name="edge">Specifies the type of inner and outer 
		/// edges to draw.</param>
		/// <param name="grfFlags">Specifies the type of border.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawEdge
			(
				IntPtr hdc,
				ref Rectangle qrc,
				BorderFlags3D edge,
				BorderFlags grfFlags
			);

		/// <summary>
		/// Draws a rectangle in the style used to indicate that 
		/// the rectangle has the focus.
		/// </summary>
		/// <param name="hDC">Handle to the device context.</param>
		/// <param name="lprc">Pointer to a Rectangle structure that 
		/// specifies the logical coordinates of the rectangle.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawFocusRect
			(
				IntPtr hDC,
				ref Rectangle lprc
			);

		/// <summary>
		/// Draws a frame control of the specified type and style.
		/// </summary>
		/// <param name="hdc">Handle to the device context of the 
		/// window in which to draw the control.</param>
		/// <param name="lprc">Pointer to a Rectangle structure that contains 
		/// the logical coordinates of the bounding rectangle for frame 
		/// control.</param>
		/// <param name="uType">Specifies the type of frame control to draw.
		/// </param>
		/// <param name="uState">Specifies the initial state of the frame 
		/// control.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawFrameControl 
			(
				IntPtr hdc,
				ref Rectangle lprc,
				FrameKind uType,
				FrameState uState
			);

		/// <summary>
		/// Displays an image and applies a visual effect to indicate 
		/// a state, such as a disabled or default state.
		/// </summary>
		/// <param name="hdc">Handle to the device context to draw in.</param>
		/// <param name="hbr">Handle to the brush used to draw the image, 
		/// if the state specified by the fuFlags parameter is DSS_MONO. 
		/// This parameter is ignored for other states.</param>
		/// <param name="lpOutputFunc">Pointer to an application-defined 
		/// callback function used to render the image. This parameter is 
		/// required if the image type in fuFlags is DST_COMPLEX. It is 
		/// optional and can be NULL if the image type is DST_TEXT. 
		/// For all other image types, this parameter is ignored.</param>
		/// <param name="lData">Specifies information about the image. 
		/// The meaning of this parameter depends on the image type.</param>
		/// <param name="wData">Specifies information about the image. 
		/// The meaning of this parameter depends on the image type. 
		/// It is, however, zero extended for use with the DrawStateProc 
		/// function. </param>
		/// <param name="x">Specifies the horizontal location, 
		/// in device units, at which to draw the image.</param>
		/// <param name="y">Specifies the vertical location, 
		/// in device units, at which to draw the image.</param>
		/// <param name="cx">Specifies the width of the image, in device units. 
		/// This parameter is required if the image type is DST_COMPLEX. 
		/// Otherwise, it can be zero to calculate the width of the image.
		/// </param>
		/// <param name="cy">Specifies the height of the image, in device units. 
		/// This parameter is required if the image type is DST_COMPLEX. 
		/// Otherwise, it can be zero to calculate the height of the image.
		/// </param>
		/// <param name="fuFlags">Specifies the image type and state.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool DrawState 
			(
				IntPtr hdc,
				IntPtr hbr,
				DrawStateProc lpOutputFunc,
				int lData,
				int wData,
				int x,
				int y,
				int cx,
				int cy,
				DrawStateFlags fuFlags
			);

		/// <summary>
		/// Enables or disables one or both scroll bar arrows.
		/// </summary>
		/// <param name="hWnd">Handle to a window or a scroll bar control, 
		/// depending on the value of the wSBflags parameter.</param>
		/// <param name="wSBflags">Specifies the scroll bar type.</param>
		/// <param name="wArrows">Specifies whether the scroll bar arrows 
		/// are enabled or disabled and indicates which arrows are enabled 
		/// or disabled.</param>
		/// <returns>If the arrows are enabled or disabled as specified, 
		/// the return value is nonzero. If the arrows are already in the 
		/// requested state or an error occurs, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnableScrollBar 
			( 
				IntPtr hWnd,
				ScrollBarFlags wSBflags,
				EnableScrollBarFlags wArrows
			);

		/// <summary>
		/// The EnableWindow function enables or disables mouse and keyboard 
		/// input to the specified window or control. When input is disabled, 
		/// the window does not receive input such as mouse clicks and key 
		/// presses. When input is enabled, the window receives all input. 
		/// </summary>
		/// <param name="hWnd">Handle to the window to be enabled or disabled.
		/// </param>
		/// <param name="bEnable">Specifies whether to enable or disable 
		/// the window. If this parameter is TRUE, the window is enabled. 
		/// If the parameter is FALSE, the window is disabled.</param>
		/// <returns>If the window was previously disabled, the return value 
		/// is nonzero. If the window was not previously disabled, the return 
		/// value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnableWindow 
			( 
				IntPtr hWnd,
				bool bEnable
			);

		/// <summary>
		/// Simultaneously updates the position and size of one 
		/// or more windows in a single screen-refreshing cycle.
		/// </summary>
		/// <param name="hWinPosInfo">Handle to a multiple-window – 
		/// position structure that contains size and position 
		/// information for one or more windows. This internal 
		/// structure is returned by the BeginDeferWindowPos function 
		/// or by the most recent call to the DeferWindowPos function.
		/// </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EndDeferWindowPos
			(
				IntPtr hWinPosInfo
			);

		/// <summary>
		/// The EndPaint function marks the end of painting in the specified 
		/// window. This function is required for each call to the BeginPaint 
		/// function, but only after painting is complete.
		/// </summary>
		/// <param name="hWnd">Handle to the window that has been repainted.
		/// </param>
		/// <param name="lpPaint">Pointer to a PAINTSTRUCT structure that contains 
		/// the painting information retrieved by BeginPaint.</param>
		/// <returns>The return value is always nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EndPaint
			(
				IntPtr hWnd,
				ref PAINTSTRUCT lpPaint
			);

		/// <summary>
		/// The EndTask function is called to forcibly close a 
		/// specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be closed.</param>
		/// <param name="fShutDown">Ignored. Must be FALSE.</param>
		/// <param name="fForce">A TRUE for this parameter will force 
		/// the destruction of the window if an initial attempt fails 
		/// to gently close the window using WM_CLOSE. With a FALSE for 
		/// this parameter, only the close with WM_CLOSE is attempted.
		/// </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks><para>Although you can access this function by 
		/// using LoadLibrary and GetProcAddress combined in 
		/// Microsoft® Windows® versions prior to Windows XP, 
		/// the function is not accessible using the standard 
		/// Include file and library linkage. The header files 
		/// included in Windows XP Service Pack 1 (SP1) and 
		/// Windows Server 2003 document this function and make 
		/// it accessible using the appropriate Include file and 
		/// library linkage. However, this function is not intended 
		/// for general use. It is recommended that you do not use 
		/// it in new programs because it might be altered or 
		/// unavailable in subsequent versions of Windows.</para>
		/// <para>Included in: Windows XP/2000/2003</para></remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EndTask
			(
				IntPtr hWnd,
				bool fShutDown,
				bool fForce
			);

		/// <summary>
		/// The EnumChildWindows function enumerates the child windows 
		/// that belong to the specified parent window by passing the 
		/// handle to each child window, in turn, to an application-defined 
		/// callback function. EnumChildWindows continues until the last 
		/// child window is enumerated or the callback function returns FALSE. 
		/// </summary>
		/// <param name="hWndParent">Handle to the parent window whose 
		/// child windows are to be enumerated. If this parameter is NULL, 
		/// this function is equivalent to EnumWindows.
		/// Windows 95/98/Me: hWndParent cannot be NULL.</param>
		/// <param name="lpEnumFunc">Pointer to an application-defined 
		/// callback function.</param>
		/// <param name="lParam">Specifies an application-defined value 
		/// to be passed to the callback function.</param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumChildWindows 
			( 
				IntPtr hWndParent,
				EnumChildProc lpEnumFunc,
				IntPtr lParam
			);

		/// <summary>
		/// The EnumDisplayDevices function lets you obtain 
		/// information about the display devices in a system.
		/// </summary>
		/// <param name="lpDevice">Pointer to the device name. 
		/// If NULL, function returns information for the display 
		/// adapter(s) on the machine, based on iDevNum.</param>
		/// <param name="iDevNum">Index value that specifies the 
		/// display device of interest. The operating system 
		/// identifies each display device with an index value. 
		/// The index values are consecutive integers, starting 
		/// at 0. If a system has three display devices, for 
		/// example, they are specified by the index values 0, 1, and 2.
		/// </param>
		/// <param name="lpDisplayDevice">Pointer to a DISPLAY_DEVICE 
		/// structure that receives information about the display 
		/// device specified by iDevNum. Before calling EnumDisplayDevices, 
		/// you must initialize the cb member of DISPLAY_DEVICE to 
		/// the size, in bytes, of DISPLAY_DEVICE.</param>
		/// <param name="dwFlags">This parameter is currently not 
		/// used and should be set to zero. </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumDisplayDevices
			(
				string lpDevice,
				int iDevNum,
				ref DISPLAY_DEVICEA lpDisplayDevice,
				int dwFlags
			);

		/// <summary>
		/// The EnumDisplayDevices function lets you obtain 
		/// information about the display devices in a system.
		/// </summary>
		/// <param name="lpDevice">Pointer to the device name. 
		/// If NULL, function returns information for the display 
		/// adapter(s) on the machine, based on iDevNum.</param>
		/// <param name="iDevNum">Index value that specifies the 
		/// display device of interest. The operating system 
		/// identifies each display device with an index value. 
		/// The index values are consecutive integers, starting 
		/// at 0. If a system has three display devices, for 
		/// example, they are specified by the index values 0, 1, and 2.
		/// </param>
		/// <param name="lpDisplayDevice">Pointer to a DISPLAY_DEVICE 
		/// structure that receives information about the display 
		/// device specified by iDevNum. Before calling EnumDisplayDevices, 
		/// you must initialize the cb member of DISPLAY_DEVICE to 
		/// the size, in bytes, of DISPLAY_DEVICE.</param>
		/// <param name="dwFlags">This parameter is currently not 
		/// used and should be set to zero. </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumDisplayDevices 
			(
				string lpDevice,
				int iDevNum,
				ref DISPLAY_DEVICEW lpDisplayDevice,
				int dwFlags
			);

		/// <summary>
		/// The EnumDisplaySettings function retrieves information 
		/// about one of the graphics modes for a display device. 
		/// To retrieve information for all the graphics modes 
		/// of a display device, make a series of calls to this function.
		/// </summary>
		/// <param name="lpszDeviceName">Pointer to a null-terminated 
		/// string that specifies the display device about whose 
		/// graphics mode the function will obtain information.
		/// This parameter is either NULL or a 
		/// DISPLAY_DEVICE.DeviceName returned from EnumDisplayDevices. 
		/// A NULL value specifies the current display device on the 
		/// computer on which the calling thread is running.
		/// Windows 95: lpszDeviceName must be NULL.</param>
		/// <param name="iModeNum">Specifies the type of information 
		/// to retrieve.</param>
		/// <param name="lpDevMode">Pointer to a DEVMODE structure 
		/// into which the function stores information about the 
		/// specified graphics mode. Before calling EnumDisplaySettings, 
		/// set the dmSize member to sizeof(DEVMODE), and set the 
		/// dmDriverExtra member to indicate the size, in bytes, 
		/// of the additional space available to receive private 
		/// driver data.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumDisplaySettings 
			(
				string lpszDeviceName,
				int iModeNum,
				ref DEVMODEW_DISPLAY lpDevMode
			);

		/// <summary>
		/// The EnumDisplaySettings function retrieves information 
		/// about one of the graphics modes for a display device. 
		/// To retrieve information for all the graphics modes 
		/// of a display device, make a series of calls to this function.
		/// </summary>
		/// <param name="lpszDeviceName">Pointer to a null-terminated 
		/// string that specifies the display device about whose 
		/// graphics mode the function will obtain information.
		/// This parameter is either NULL or a 
		/// DISPLAY_DEVICE.DeviceName returned from EnumDisplayDevices. 
		/// A NULL value specifies the current display device on the 
		/// computer on which the calling thread is running.
		/// Windows 95: lpszDeviceName must be NULL.</param>
		/// <param name="iModeNum">Specifies the type of information 
		/// to retrieve.</param>
		/// <param name="lpDevMode">Pointer to a DEVMODE structure 
		/// into which the function stores information about the 
		/// specified graphics mode. Before calling EnumDisplaySettings, 
		/// set the dmSize member to sizeof(DEVMODE), and set the 
		/// dmDriverExtra member to indicate the size, in bytes, 
		/// of the additional space available to receive private 
		/// driver data.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumDisplaySettings
			(
				string lpszDeviceName,
				int iModeNum,
				ref DEVMODEA_DISPLAY lpDevMode
			);

		/// <summary>
		/// The EnumThreadWindows function enumerates all nonchild windows 
		/// associated with a thread by passing the handle to each window, 
		/// in turn, to an application-defined callback function. 
		/// EnumThreadWindows continues until the last window is enumerated 
		/// or the callback function returns FALSE.
		/// </summary>
		/// <param name="dwThreadId">Identifies the thread whose windows are 
		/// to be enumerated.</param>
		/// <param name="lpfn">Pointer to an application-defined callback 
		/// function.</param>
		/// <param name="lParam">Specifies an application-defined value to 
		/// be passed to the callback function.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumThreadWindows 
			( 
				IntPtr dwThreadId,
				EnumThreadWndProc lpfn,
				IntPtr lParam
			);

		/// <summary>
		/// The EnumWindows function enumerates all top-level windows 
		/// on the screen by passing the handle to each window, in turn, 
		/// to an application-defined callback function. EnumWindows 
		/// continues until the last top-level window is enumerated 
		/// or the callback function returns FALSE.
		/// </summary>
		/// <param name="lpEnumFunc">Pointer to an application-defined 
		/// callback function.</param>
		/// <param name="lParam">Specifies an application-defined value 
		/// to be passed to the callback function.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool EnumWindows 
			( 
				EnumWindowsProc lpEnumFunc,
				IntPtr lParam
			);

		/// <summary>
		/// Prevents drawing within invalid areas of a window by excluding 
		/// an updated region in the window from a clipping region.
		/// </summary>
		/// <param name="hDC">Handle to the device context associated with 
		/// the clipping region.</param>
		/// <param name="hWnd">Handle to the window to update.</param>
		/// <returns>The return value specifies the complexity of the 
		/// excluded region.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern RegionComplexity ExcludeUpdateRgn 
			(
				IntPtr hDC,
				IntPtr hWnd
			);

		/// <summary>
		/// Logs off the current user. Sends the WM_QUERYENDSESSION 
		/// message to determine if they can be terminated.
		/// </summary>
		/// <param name="dwReserved">Reserved, must be zero.</param>
		/// <param name="uReserved">Reserved, must be zero.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>Included in: Windows 95/98/ME/NT/XP/2000/2003.
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool ExitWindows
			(
				uint dwReserved,
				uint uReserved
			);

		/// <summary>
		/// The ExitWindowsEx function either logs off the current user, 
		/// shuts down the system, or shuts down and restarts the system. 
		/// It sends the WM_QUERYENDSESSION message to all applications 
		/// to determine if they can be terminated.
		/// </summary>
		/// <param name="uFlags">Shutdown type.</param>
		/// <param name="dwReason"><para>Reason for initiating the 
		/// shutdown.</para><para>Windows 2000, Windows NT, and Windows 
		/// Me/98/95:  This parameter is ignored.</para>
		/// </param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero. Because the function executes asynchronously, 
		/// a nonzero return value indicates that the shutdown has 
		/// been initiated. It does not indicate whether the shutdown 
		/// will succeed. It is possible that the system, the user, 
		/// or another application will abort the shutdown.</returns>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool ExitWindowsEx
			(
				ExitWindwowsFlags uFlags,
				uint dwReason
			);

		/// <summary>
		/// The FlashWindow function flashes the specified window one 
		/// time. It does not change the active state of the window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be flashed. 
		/// The window can be either open or minimized.</param>
		/// <param name="bInvert"><para>If this parameter is TRUE, 
		/// the window is flashed from one state to the other. 
		/// If it is FALSE, the window is returned to its original 
		/// state (either active or inactive).</para>
		/// <para>When an application is minimized and this parameter 
		/// is TRUE, the taskbar window button flashes active/inactive. 
		/// If it is FALSE, the taskbar window button flashes inactive, 
		/// meaning that it does not change colors. It flashes, as if 
		/// it were being redrawn, but it does not provide the visual 
		/// invert clue to the user.</para></param>
		/// <returns>The return value specifies the window's state 
		/// before the call to the FlashWindow function. If the window 
		/// caption was drawn as active before the call, the return 
		/// value is nonzero. Otherwise, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool FlashWindow
			(
				IntPtr hWnd,
				bool bInvert
			);

		/// <summary>
		/// The FlashWindowEx function flashes the specified window. 
		/// It does not change the active state of the window.
		/// </summary>
		/// <param name="pfwi">Pointer to the FLASHWINFO structure.
		/// </param>
		/// <returns>The return value specifies the window's state 
		/// before the call to the FlashWindowEx function. If the window 
		/// caption was drawn as active before the call, the return 
		/// value is nonzero. Otherwise, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool FlashWindowEx
			(
				ref FLASHWINFO pfwi
			);

		/// <summary>
		/// <para>The FindWindow function retrieves a handle to the 
		/// top-level window whose class name and window name match 
		/// the specified strings. This function does not search child 
		/// windows. This function does not perform a case-sensitive 
		/// search.</para>
		/// <para>To search child windows, beginning with a specified 
		/// child window, use the FindWindowEx function.</para>
		/// </summary>
		/// <param name="lpClassName"><para>Pointer to a null-terminated 
		/// string that specifies the class name or a class atom created 
		/// by a previous call to the RegisterClass or RegisterClassEx 
		/// function. The atom must be in the low-order word of 
		/// lpClassName; the high-order word must be zero.</para>
		/// <para>If lpClassName points to a string, it specifies the 
		/// window class name. The class name can be any name registered 
		/// with RegisterClass or RegisterClassEx, or any of the 
		/// predefined control-class names.</para>
		/// <para>If lpClassName is NULL, it finds any window whose 
		/// title matches the lpWindowName parameter.</para></param>
		/// <param name="lpWindowName">Pointer to a null-terminated 
		/// string that specifies the window name (the window's title). 
		/// If this parameter is NULL, all window names match.</param>
		/// <returns>If the function succeeds, the return value is a 
		/// handle to the window that has the specified class name and 
		/// window name. If the function fails, the return value is NULL.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr FindWindow
			(
				string lpClassName,
				string lpWindowName
			);

		/// <summary>
		/// The FindWindowEx function retrieves a handle to a window 
		/// whose class name and window name match the specified 
		/// strings. The function searches child windows, beginning 
		/// with the one following the specified child window. 
		/// This function does not perform a case-sensitive search.
		/// </summary>
		/// <param name="hwndParent"><para>Handle to the parent window 
		/// whose child windows are to be searched.</para>
		/// <para>If hwndParent is NULL, the function uses the desktop 
		/// window as the parent window. The function searches among 
		/// windows that are child windows of the desktop.</para>
		/// <para>Microsoft® Windows® 2000 and Windows XP: If hwndParent 
		/// is HWND_MESSAGE, the function searches all message-only 
		/// windows.</para></param>
		/// <param name="hwndChildAfter"><para>Handle to a child window. 
		/// The search begins with the next child window in the Z order. 
		/// The child window must be a direct child window of hwndParent, 
		/// not just a descendant window.</para>
		/// <para>If hwndChildAfter is NULL, the search begins with the 
		/// first child window of hwndParent.</para>
		/// <para>Note that if both hwndParent and hwndChildAfter are 
		/// NULL, the function searches all top-level and message-only 
		/// windows.</para></param>
		/// <param name="lpszClass"><para>Pointer to a null-terminated 
		/// string that specifies the class name or a class atom created 
		/// by a previous call to the RegisterClass or RegisterClassEx 
		/// function. The atom must be placed in the low-order word of 
		/// lpszClass; the high-order word must be zero.</para>
		/// <para>If lpszClass is a string, it specifies the window 
		/// class name. The class name can be any name registered with 
		/// RegisterClass or RegisterClassEx, or any of the predefined 
		/// control-class names, or it can be MAKEINTATOM(0x800). In 
		/// this latter case, 0x8000 is the atom for a menu class. For 
		/// more information, see the Remarks section of this topic.
		/// </para></param>
		/// <param name="lpszWindow">Pointer to a null-terminated string 
		/// that specifies the window name (the window's title). If this 
		/// parameter is NULL, all window names match.</param>
		/// <returns>If the function succeeds, the return value is 
		/// a handle to the window that has the specified class and 
		/// window names. If the function fails, the return value 
		/// is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr FindWindowEx
			(
				IntPtr hwndParent,
				IntPtr hwndChildAfter,
				string lpszClass,
				string lpszWindow
			);

		/// <summary>
		/// Retrieves the window handle to the active window attached 
		/// to the calling thread's message queue.
		/// </summary>
		/// <returns>The return value is the handle to the active 
		/// window attached to the calling thread's message queue. 
		/// Otherwise, the return value is NULL.</returns>
		/// <remarks>
		/// <para>To get the handle to the foreground window, you can 
		/// use GetForegroundWindow.</para>
		/// <para>Windows 98/Me and Windows NT 4.0 SP3 and later: 
		/// To get the window handle to the active window in the 
		/// message queue for another thread, use GetGUIThreadInfo.
		/// </para></remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern IntPtr GetActiveWindow ();

		/// <summary>
		/// Retrieves status information for the specified window if 
		/// it is the application-switching (ALT+TAB) window.
		/// </summary>
		/// <param name="hwnd">Handle to the window for which status 
		/// information will be retrieved. This window must be the 
		/// application-switching window.</param>
		/// <param name="iItem">Specifies the index of the icon in the 
		/// application-switching window. If the pszItemText parameter 
		/// is not NULL, the name of the item is copied to the pszItemText 
		/// string. If this parameter is –1, the name of the item is not 
		/// copied.</param>
		/// <param name="pati">Pointer to an ALTTABINFO structure to receive 
		/// the status information. Note that you must set ALTTABINFO.csSize 
		/// to sizeof(ALTTABINFO) before calling this function.</param>
		/// <param name="pszItemText">Pointer to a string that receives the 
		/// name of the item. If this parameter is NULL, the name of the item 
		/// is not copied.</param>
		/// <param name="cchItemText">Specifies the size, in TCHARs, of the 
		/// pszItemText buffer.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetAltTabInfo 
			( 
				IntPtr hwnd,
				int iItem,
				ref ALTTABINFO pati,
				StringBuilder pszItemText,
				int cchItemText
			);

		/// <summary>
		/// Retrieves the handle to the ancestor of the specified 
		/// window.
		/// </summary>
		/// <param name="hwnd">Handle to the window whose ancestor is 
		/// to be retrieved. If this parameter is the desktop window, 
		/// the function returns NULL.</param>
		/// <param name="gaFlags">Specifies the ancestor to be 
		/// retrieved.</param>
		/// <returns>The return value is the handle to the 
		/// ancestor window.</returns>
		/// <remarks>Included in: Windows 98/NT 4.0 SP4/ME/XP/2000/2003.
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetAncestor
			(
				IntPtr hwnd,
				GetAncestorFlags gaFlags
			);

		/// <summary>
		/// The GetAsyncKeyState function determines whether a key is up 
		/// or down at the time the function is called, and whether the 
		/// key was pressed after a previous call to GetAsyncKeyState. 
		/// </summary>
		/// <param name="vKey">Specifies one of 256 possible virtual-key 
		/// codes.</param>
		/// <returns>If the function succeeds, the return value specifies 
		/// whether the key was pressed since the last call to 
		/// GetAsyncKeyState, and whether the key is currently up or down. 
		/// If the most significant bit is set, the key is down, and if the 
		/// least significant bit is set, the key was pressed after the 
		/// previous call to GetAsyncKeyState.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern short GetAsyncKeyState 
			( 
				Keys vKey
			);

		/// <summary>
		/// Retrieves the name of the class to which the specified window 
		/// belongs.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, 
		/// the class to which the window belongs.</param>
		/// <param name="lpClassName">Pointer to the buffer that 
		/// is to receive the class name string.</param>
		/// <param name="nMaxCount">Specifies the length, in TCHAR, 
		/// of the buffer pointed to by the lpClassName parameter. 
		/// The class name string is truncated if it is longer than 
		/// the buffer and is always null-terminated.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// number of TCHAR copied to the specified buffer.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetClassName 
			( 
				IntPtr hWnd,
				StringBuilder lpClassName,
				int nMaxCount
			);

		/// <summary>
		/// Retrieves the coordinates of a window's client area. 
		/// The client coordinates specify the upper-left and 
		/// lower-right corners of the client area. Because client 
		/// coordinates are relative to the upper-left corner of 
		/// a window's client area, the coordinates of the upper-left 
		/// corner are (0,0). 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client 
		/// coordinates are to be retrieved.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that 
		/// receives the client coordinates. The left and top members 
		/// are zero. The right and bottom members contain the width 
		/// and height of the window.</param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetClientRect
			(
				IntPtr hWnd,
				out Rectangle lpRect
			);

		/// <summary>
		/// The GetDC function retrieves a handle to a display device context 
		/// (DC) for the client area of a specified window or for the entire 
		/// screen. You can use the returned handle in subsequent GDI functions 
		/// to draw in the DC.
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose DC is to be 
		/// retrieved. If this value is NULL, GetDC retrieves the DC for the 
		/// entire screen.</para>
		/// <para>Windows 98/Me, Windows 2000/XP: To get the DC for a specific 
		/// display monitor, use the EnumDisplayMonitors and CreateDC functions.
		/// </para></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetDC 
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The GetDCEx function retrieves a handle to a display device context 
		/// (DC) for the client area of a specified window or for the entire 
		/// screen. You can use the returned handle in subsequent GDI functions 
		/// to draw in the DC.
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose DC is to be 
		/// retrieved. If this value is NULL, GetDCEx retrieves the DC for the 
		/// entire screen.</para>
		/// <para>Windows 98/Me, Windows 2000/XP: To get the DC for a specific 
		/// display monitor, use the EnumDisplayMonitors and CreateDC functions.
		/// </para></param>
		/// <param name="hrgnClip">Specifies a clipping region that may be 
		/// combined with the visible region of the DC. If the value of flags 
		/// is DCX_INTERSECTRGN or DCX_EXCLUDERGN, then the operating system 
		/// assumes ownership of the region and will automatically delete it 
		/// when it is no longer needed. In this case, applications should not 
		/// use the region—not even delete it—after a successful call to GetDCEx.
		/// </param>
		/// <param name="flags">Specifies how the DC is created.</param>
		/// <returns>If the function succeeds, the return value is the handle 
		/// to the DC for the specified window. If the function fails, the return 
		/// value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetDCEx 
			(
				IntPtr hWnd,
				IntPtr hrgnClip,
				int flags
			);

		/// <summary>
		/// The GetDesktopWindow function returns a handle to the 
		/// desktop window. The desktop window covers the entire screen. 
		/// The desktop window is the area on top of which all icons 
		/// and other windows are painted. 
		/// </summary>
		/// <returns>The return value is a handle to the desktop window.
		/// </returns>
		[DllImport ( DllName )]
		public static extern IntPtr GetDesktopWindow ();

		/// <summary>
		/// The GetFocus function retrieves the handle to the window that 
		/// has the keyboard focus, if the window is attached to the 
		/// calling thread's message queue. 
		/// </summary>
		/// <returns>The return value is the handle to the window with the 
		/// keyboard focus. If the calling thread's message queue does not 
		/// have an associated window with the keyboard focus, the return 
		/// value is NULL.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern IntPtr GetFocus ();

		/// <summary>
		/// The GetForegroundWindow function returns a handle to the 
		/// foreground window (the window with which the user is 
		/// currently working). The system assigns a slightly higher 
		/// priority to the thread that creates the foreground window 
		/// than it does to other threads.
		/// </summary>
		/// <returns>The return value is a handle to the foreground 
		/// window. The foreground window can be NULL in certain 
		/// circumstances, such as when a window is losing activation.
		/// </returns>
		[DllImport ( DllName )]
		public static extern IntPtr GetForegroundWindow ();

		/// <summary>
		/// Retrieves the count of handles to graphical user 
		/// interface (GUI) objects in use by the specified process.
		/// </summary>
		/// <param name="hProcess">Handle to the process. The handle 
		/// must have the PROCESS_QUERY_INFORMATION access right.</param>
		/// <param name="uiFlags">GUI object type.</param>
		/// <returns>If the function succeeds, the return value is 
		/// the count of handles to GUI objects in use by the process. 
		/// If no GUI objects are in use, the return value is zero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern uint GetGuiResources
			(
				IntPtr hProcess,
				GuiResourcesFlags uiFlags
			);

		/// <summary>
		/// Determines whether there are mouse-button or keyboard messages 
		/// in the calling thread's message queue.
		/// </summary>
		/// <returns>If the queue contains one or more new mouse-button 
		/// or keyboard messages, the return value is nonzero.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool GetInputState ();

		/// <summary>
		/// The GetKBCodePage function returns the current code page.
		/// </summary>
		/// <returns>The return value is an OEM code-page identifier, 
		/// or it is the default identifier if the registry value is 
		/// not readable. For a list of OEM code-page identifiers, 
		/// see GetOEMCP.</returns>
		/// <remarks>This function is provided only for compatibility 
		/// with 16-bit versions of Windows. Applications should use 
		/// the GetOEMCP function to retrieve the OEM code-page 
		/// identifier for the system.</remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int GetKBCodePage ();

		/// <summary>
		/// The GetMessage function retrieves a message from the calling 
		/// thread's message queue. The function dispatches incoming sent 
		/// messages until a posted message is available for retrieval.
		/// </summary>
		/// <param name="lpMsg">Pointer to an MSG structure that receives 
		/// message information from the thread's message queue.</param>
		/// <param name="hWnd"><para>Handle to the window whose messages 
		/// are to be retrieved. The window must belong to the calling thread. 
		/// The NULL value has a special meaning: GetMessage retrieves messages 
		/// for any window that belongs to the calling thread and thread messages 
		/// posted to the calling thread using the PostThreadMessage function.
		/// </para></param>
		/// <param name="wMsgFilterMin"><para>Specifies the integer value of 
		/// the lowest message value to be retrieved. Use WM_KEYFIRST to specify 
		/// the first keyboard message or WM_MOUSEFIRST to specify the first 
		/// mouse message.</para>
		/// <para>Windows XP: Use WM_INPUT here and in wMsgFilterMax to specify 
		/// only the WM_INPUT messages.</para>
		/// <para>If wMsgFilterMin and wMsgFilterMax are both zero, GetMessage 
		/// returns all available messages (that is, no range filtering is 
		/// performed).</para></param>
		/// <param name="wMsgFilterMax"><para>Specifies the integer value of 
		/// the highest message value to be retrieved. Use WM_KEYLAST to specify 
		/// the first keyboard message or WM_MOUSELAST to specify the last mouse 
		/// message.</para>
		/// <para>Windows XP: Use WM_INPUT here and in wMsgFilterMin to specify 
		/// only the WM_INPUT messages.</para>
		/// <para>If wMsgFilterMin and wMsgFilterMax are both zero, GetMessage 
		/// returns all available messages (that is, no range filtering is 
		/// performed).</para></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetMessage
			(
				out MSG lpMsg,
				IntPtr hWnd,
				int wMsgFilterMin,
				int wMsgFilterMax
			);

		/// <summary>
		/// The GetMessageExtraInfo function retrieves the extra message 
		/// information for the current thread. Extra message information 
		/// is an application- or driver-defined value associated with the 
		/// current thread's message queue.
		/// </summary>
		/// <returns>The return value specifies the extra information. 
		/// The meaning of the extra information is device specific.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int GetMessageExtraInfo ();

		/// <summary>
		/// Retrieves the cursor position for the last message retrieved 
		/// by the GetMessage function.
		/// </summary>
		/// <returns>The return value specifies the x- and y-coordinates 
		/// of the cursor position. The x-coordinate is the low order int 
		/// and the y-coordinate is the high-order int.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int GetMessagePos ();

		/// <summary>
		/// The GetMessageTime function retrieves the message time for 
		/// the last message retrieved by the GetMessage function. 
		/// The time is a long integer that specifies the elapsed time, 
		/// in milliseconds, from the time the system was started to the 
		/// time the message was created (that is, placed in the thread's 
		/// message queue). 
		/// </summary>
		/// <returns>The return value specifies the message time.</returns>
		/// <remarks><para>The return value from the GetMessageTime function 
		/// does not necessarily increase between subsequent messages, 
		/// because the value wraps to zero if the timer count exceeds 
		/// the maximum value for a long integer.</para>
		/// <para>To calculate time delays between messages, verify that 
		/// the time of the second message is greater than the time of 
		/// the first message; then, subtract the time of the first message 
		/// from the time of the second message.</para></remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int GetMessageTime ();

		/// <summary>
		/// Retrieves a handle to the next or previous window in 
		/// the Z-Order. The next window is below the specified 
		/// window; the previous window is above. If the specified 
		/// window is a topmost window, the function retrieves a 
		/// handle to the next (or previous) topmost window. If the 
		/// specified window is a top-level window, the function 
		/// retrieves a handle to the next (or previous) top-level 
		/// window. If the specified window is a child window, the 
		/// function searches for a handle to the next (or previous) 
		/// child window. 
		/// </summary>
		/// <param name="hWnd">Handle to a window. The window handle 
		/// retrieved is relative to this window, based on the value 
		/// of the wCmd parameter.</param>
		/// <param name="uCmd"><para>Specifies whether the function 
		/// returns a handle to the next window or of the previous 
		/// window. This parameter can be either of the following 
		/// values:</para><list type="bullet">
		/// <item>GW_HWNDNEXT</item>
		/// <item>GW_HWNDPREV</item>
		/// </list></param>
		/// <returns>If the function succeeds, the return value is a 
		/// handle to the next (or previous) window. If there is no 
		/// next (or previous) window, the return value is NULL.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetNextWindow
			(
				IntPtr hWnd,
				GetWindowFlags uCmd
			);

		/// <summary>
		/// The GetParent function retrieves a handle to the 
		/// specified window's parent or owner.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose parent 
		/// window handle is to be retrieved.</param>
		/// <returns>If the window is a child window, the return value 
		/// is a handle to the parent window. If the window is a 
		/// top-level window, the return value is a handle to the 
		/// owner window. If the window is a top-level unowned window 
		/// or if the function fails, the return value is NULL. To get 
		/// extended error information, call GetLastError. For example, 
		/// this would determine, when the function returns NULL, if 
		/// the function failed or the window was a top-level window.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetParent
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Indicates the type of messages found in the calling thread's 
		/// message queue.
		/// </summary>
		/// <param name="flags">Specifies the types of messages for 
		/// which to check.</param>
		/// <returns>The high-order word of the return value indicates 
		/// the types of messages currently in the queue. The low-order 
		/// word indicates the types of messages that have been added 
		/// to the queue and that are still in the queue since the last 
		/// call to the GetQueueStatus, GetMessage, or PeekMessage function.
		/// </returns>
		/// <remarks><para>The presence of a QS_ flag in the return value 
		/// does not guarantee that a subsequent call to the GetMessage or 
		/// PeekMessage function will return a message. GetMessage and 
		/// PeekMessage perform some internal filtering that may cause 
		/// the message to be processed internally. For this reason, 
		/// the return value from GetQueueStatus should be considered 
		/// only a hint as to whether GetMessage or PeekMessage should 
		/// be called.</para>
		/// <para>The QS_ALLPOSTMESSAGE and QS_POSTMESSAGE flags differ 
		/// in when they are cleared. QS_POSTMESSAGE is cleared when you 
		/// call GetMessage or PeekMessage, whether or not you are filtering 
		/// messages. QS_ALLPOSTMESSAGE is cleared when you call GetMessage 
		/// or PeekMessage without filtering messages (wMsgFilterMin and 
		/// wMsgFilterMax are 0). This can be useful when you call PeekMessage 
		/// multiple times to get messages in different ranges.</para></remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int GetQueueStatus
			(
				QueueStatusFlags flags
			);

		/// <summary>
		/// Retrieves the parameters of a scroll bar, including the minimum 
		/// and maximum scrolling positions, the page size, and the position 
		/// of the scroll box (thumb).
		/// </summary>
		/// <param name="hwnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the fnBar parameter.
		/// </param>
		/// <param name="fnBar">Specifies the type of scroll bar for which to 
		/// retrieve parameters.</param>
		/// <param name="lpsi">Pointer to a SCROLLINFO structure.</param>
		/// <returns>If the function retrieved any values, the return value is 
		/// nonzero. If the function does not retrieve any values, the return 
		/// value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetScrollInfo 
			( 
				IntPtr hwnd,
				ScrollBarFlags fnBar,
				ref SCROLLINFO lpsi
			);

		/// <summary>
		/// The GetScrollPos function retrieves the current position of the 
		/// scroll box (thumb) in the specified scroll bar. The current position 
		/// is a relative value that depends on the current scrolling range. 
		/// For example, if the scrolling range is 0 through 100 and the scroll 
		/// box is in the middle of the bar, the current position is 50.
		/// </summary>
		/// <param name="hwnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the nBar parameter.
		/// </param>
		/// <param name="nBar">Specifies the scroll bar to be examined.</param>
		/// <returns>If the function succeeds, the return value is the current 
		/// position of the scroll box. If the function fails, the return value 
		/// is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetScrollPos
			(
				IntPtr hwnd,
				ScrollBarFlags nBar
			);

		/// <summary>
		/// Retrieves the current minimum and maximum scroll box (thumb) 
		/// positions for the specified scroll bar. 
		/// </summary>
		/// <param name="hWnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the nBar parameter.
		/// </param>
		/// <param name="nBar">Specifies the scroll bar from which the positions 
		/// are retrieved.</param>
		/// <param name="lpMinPos">Pointer to the integer variable that receives 
		/// the minimum position.</param>
		/// <param name="lpMaxPos">Pointer to the integer variable that receives 
		/// the maximum position.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetScrollRange 
			( 
				IntPtr hWnd,
				ScrollBarFlags nBar,
				out int lpMinPos,
				out int lpMaxPos
			);

		/// <summary>
		/// Returns a handle to the Shell's desktop window.
		/// </summary>
		/// <returns>The return value is the handle of the Shell's 
		/// desktop window. If no Shell process is present, the return 
		/// value is NULL.</returns>
		/// <remarks>Included in: Windows 2000/XP/2003.</remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern IntPtr GetShellWindow ();

		/// <summary>
		/// The GetSystemMetrics function retrieves various system 
		/// metrics (widths and heights of display elements) and 
		/// system configuration settings. All dimensions retrieved 
		/// by GetSystemMetrics are in pixels.
		/// </summary>
		/// <param name="nIndex">System metric or configuration setting 
		/// to retrieve.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// requested system metric or configuration setting.
		/// If the function fails, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetSystemMetrics 
			(
				int nIndex
			);

		/// <summary>
		/// The GetSystemMetrics function retrieves various system 
		/// metrics (widths and heights of display elements) and 
		/// system configuration settings. All dimensions retrieved 
		/// by GetSystemMetrics are in pixels.
		/// </summary>
		/// <param name="nIndex">System metric or configuration setting 
		/// to retrieve.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// requested system metric or configuration setting.
		/// If the function fails, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetSystemMetrics
			(
				SystemMetricsFlags nIndex
			);

		/// <summary>
		/// Examines the Z order of the child windows associated 
		/// with the specified parent window and retrieves a handle 
		/// to the child window at the top of the Z order.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window whose child 
		/// windows are to be examined. If this parameter is NULL, the 
		/// function returns a handle to the window at the top of the 
		/// Z order.</param>
		/// <returns>If the function succeeds, the return value is 
		/// a handle to the child window at the top of the Z order. 
		/// If the specified window has no child windows, the return 
		/// value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetTopWindow
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The GetUpdateRect function retrieves the coordinates of 
		/// the smallest rectangle that completely encloses the update 
		/// region of the specified window. GetUpdateRect retrieves 
		/// the rectangle in logical coordinates. If there is no 
		/// update region, GetUpdateRect retrieves an empty rectangle 
		/// (sets all coordinates to zero). 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region 
		/// is to be retrieved.</param>
		/// <param name="lpRect">Pointer to the Rectangle structure that 
		/// receives the coordinates, in device units, of the enclosing 
		/// rectangle. An application can set this parameter to NULL to 
		/// determine whether an update region exists for the window. 
		/// If this parameter is NULL, GetUpdateRect returns nonzero 
		/// if an update region exists, and zero if one does not. 
		/// This provides a simple and efficient means of determining 
		/// whether a WM_PAINT message resulted from an invalid area.</param>
		/// <param name="bErase">Specifies whether the background in 
		/// the update region is to be erased. If this parameter is TRUE 
		/// and the update region is not empty, GetUpdateRect sends a 
		/// WM_ERASEBKGND message to the specified window to erase the 
		/// background.</param>
		/// <returns>If the update region is not empty, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetUpdateRect
			(
				IntPtr hWnd,
				out Rectangle lpRect,
				bool bErase
			);

		/// <summary>
		/// The GetUpdateRect function retrieves the coordinates of 
		/// the smallest rectangle that completely encloses the update 
		/// region of the specified window. GetUpdateRect retrieves 
		/// the rectangle in logical coordinates. If there is no 
		/// update region, GetUpdateRect retrieves an empty rectangle 
		/// (sets all coordinates to zero). 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region 
		/// is to be retrieved.</param>
		/// <param name="lpRect">Pointer to the Rectangle structure that 
		/// receives the coordinates, in device units, of the enclosing 
		/// rectangle. An application can set this parameter to NULL to 
		/// determine whether an update region exists for the window. 
		/// If this parameter is NULL, GetUpdateRect returns nonzero 
		/// if an update region exists, and zero if one does not. 
		/// This provides a simple and efficient means of determining 
		/// whether a WM_PAINT message resulted from an invalid area.</param>
		/// <param name="bErase">Specifies whether the background in 
		/// the update region is to be erased. If this parameter is TRUE 
		/// and the update region is not empty, GetUpdateRect sends a 
		/// WM_ERASEBKGND message to the specified window to erase the 
		/// background.</param>
		/// <returns>If the update region is not empty, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetUpdateRect
			(
				IntPtr hWnd,
				IntPtr lpRect,
				bool bErase
			);

		/// <summary>
		/// The GetUpdateRgn function retrieves the update region of 
		/// a window by copying it into the specified region. 
		/// The coordinates of the update region are relative to the 
		/// upper-left corner of the window (that is, they are client 
		/// coordinates). 
		/// </summary>
		/// <param name="hWnd">Handle to the window with an update region 
		/// that is to be retrieved.</param>
		/// <param name="hRgn">Handle to the region to receive the update 
		/// region.</param>
		/// <param name="bErase">Specifies whether the window background 
		/// should be erased and whether nonclient areas of child windows 
		/// should be drawn. If this parameter is FALSE, no drawing is done.
		/// </param>
		/// <returns>The return value indicates the complexity of the 
		/// resulting region.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern RegionComplexity GetUpdateRgn
			(
				IntPtr hWnd,
				IntPtr hRgn,
				bool bErase
			);

		/// <summary>
		/// Retrieves a handle to a window that has the specified 
		/// relationship (Z-Order or owner) to the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to a window. The window handle 
		/// retrieved is relative to this window, based on the value 
		/// of the uCmd parameter.</param>
		/// <param name="uCmd">Specifies the relationship between the 
		/// specified window and the window whose handle is to be 
		/// retrieved.</param>
		/// <returns>If the function succeeds, the return value 
		/// is a window handle. If no window exists with the 
		/// specified relationship to the specified window, the 
		/// return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetWindow
			(
				IntPtr hWnd,
				GetWindowFlags uCmd
			);

		/// <summary>
		/// The GetWindowDC function retrieves the device context (DC) 
		/// for the entire window, including title bar, menus, and scroll 
		/// bars. A window device context permits painting anywhere in 
		/// a window, because the origin of the device context is the 
		/// upper-left corner of the window instead of the client area.
		/// GetWindowDC assigns default attributes to the window device 
		/// context each time it retrieves the device context. 
		/// Previous attributes are lost.</summary>
		/// <param name="hWnd">Handle to the window with a device context 
		/// that is to be retrieved. If this value is NULL, GetWindowDC 
		/// retrieves the device context for the entire screen.
		/// Windows 98/Me, Windows 2000/XP: If this parameter is NULL, 
		/// GetWindowDC retrieves the device context for the primary 
		/// display monitor. To get the device context for other display 
		/// monitors, use the EnumDisplayMonitors and CreateDC functions.
		/// </param>
		/// <returns>If the function succeeds, the return value is a handle 
		/// to a device context for the specified window.
		/// If the function fails, the return value is NULL, indicating 
		/// an error or an invalid hWnd parameter.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr GetWindowDC 
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The GetWindowLong function retrieves information about the 
		/// specified window. The function also retrieves the 32-bit (long) 
		/// value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the 
		/// class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value 
		/// to be retrieved.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// requested 32-bit value. If the function fails, the return value 
		/// is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetWindowLong 
			( 
				IntPtr hWnd,
				int nIndex
			);

		/// <summary>
		/// The GetWindowLong function retrieves information about the 
		/// specified window. The function also retrieves the 32-bit (long) 
		/// value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the 
		/// class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value 
		/// to be retrieved.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// requested 32-bit value. If the function fails, the return value 
		/// is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetWindowLong
			(
				IntPtr hWnd,
				WindowLongFlags nIndex
			);
		
		/// <summary>
		/// Retrieves the dimensions of the bounding rectangle of the 
		/// specified window. The dimensions are given in screen 
		/// coordinates that are relative to the upper-left corner of 
		/// the screen.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="rect">Pointer to a structure that receives the 
		/// screen coordinates of the upper-left and lower-right corners 
		/// of the window.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GetWindowRect
			(
				IntPtr hWnd,
				out Rectangle rect
			);

		/// <summary>
		/// The GetWindowRgn function obtains a copy of the window region 
		/// of a window. The window region of a window is set by calling 
		/// the SetWindowRgn function. The window region determines the 
		/// area within the window where the system permits drawing. 
		/// The system does not display any portion of a window that 
		/// lies outside of the window region.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window region 
		/// is to be obtained.</param>
		/// <param name="hRgn">Handle to the region which will be modified 
		/// to represent the window region.</param>
		/// <returns>The return value specifies the type of the region that 
		/// the function obtains.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern RegionComplexity GetWindowRgn 
			(
				IntPtr hWnd,
				IntPtr hRgn
			);

		/// <summary>
		/// Retrieves the dimensions of the tightest bounding rectangle 
		/// for the window region of a window.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="lprc">Pointer to a Rectangle structure that receives 
		/// the rectangle dimensions, in device units relative to the 
		/// upper-left corner of the window.</param>
		/// <returns>The return value specifies the type of the region that 
		/// the function obtains.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern RegionComplexity GetWindowRgnBox 
			(
				IntPtr hWnd,
				out Rectangle lprc
			);

		/// <summary>
		/// The GetWindowText function copies the text of the specified 
		/// window's title bar (if it has one) into a buffer. If the specified 
		/// window is a control, the text of the control is copied. However, 
		/// GetWindowText cannot retrieve the text of a control in another 
		/// application.
		/// </summary>
		/// <param name="hWnd">Handle to the window or control containing 
		/// the text.</param>
		/// <param name="lpString">Pointer to the buffer that will receive 
		/// the text. If the string is as long or longer than the buffer, 
		/// the string is truncated and terminated with a NULL character.
		/// </param>
		/// <param name="nMaxCount">Specifies the maximum number of characters 
		/// to copy to the buffer, including the NULL character. If the text 
		/// exceeds this limit, it is truncated.</param>
		/// <returns>If the function succeeds, the return value is the length, 
		/// in characters, of the copied string, not including the terminating 
		/// NULL character. If the window has no title bar or text, if the title 
		/// bar is empty, or if the window or control handle is invalid, the 
		/// return value is zero. To get extended error information, call 
		/// GetLastError.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetWindowText 
			( 
				IntPtr hWnd,
				StringBuilder lpString,
				int nMaxCount
			);

		/// <summary>
		/// The GetWindowTextLength function retrieves the length, in characters, 
		/// of the specified window's title bar text (if the window has a title 
		/// bar). If the specified window is a control, the function retrieves 
		/// the length of the text within the control. However, 
		/// GetWindowTextLength cannot retrieve the length of the text of an 
		/// edit control in another application.
		/// </summary>
		/// <param name="hWnd">Handle to the window or control.</param>
		/// <returns>If the function succeeds, the return value is the length, 
		/// in characters, of the text. Under certain conditions, this value 
		/// may actually be greater than the length of the text.
		/// If the window has no text, the return value is zero. 
		/// To get extended error information, call GetLastError.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetWindowTextLength 
			( 
				IntPtr hWnd
			);

		/// <summary>
		/// Adds a character string to the global atom table and 
		/// returns a unique value (an atom) identifying the string. 
		/// </summary>
		/// <param name="lpString">Pointer to the null-terminated string 
		/// to be added. The string can have a maximum size of 255 bytes. 
		/// Strings that differ only in case are considered identical. 
		/// The case of the first string of this name added to the table 
		/// is preserved and returned by the GlobalGetAtomName function.
		///	</param>
		/// <returns>If the function succeeds, the return value is the 
		/// newly created atom. If the function fails, the return value 
		/// is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern short GlobalAddAtom 
			( 
				string lpString
			);

		/// <summary>
		/// The GlobalDeleteAtom function decrements the reference count 
		/// of a global string atom. If the atom's reference count reaches 
		/// zero, GlobalDeleteAtom removes the string associated with the 
		/// atom from the global atom table. 
		/// </summary>
		/// <param name="nAtom">Identifies the atom and character string to 
		/// be deleted.</param>
		/// <returns>The function always returns (ATOM) 0. To determine 
		/// whether the function has failed, call SetLastError 
		/// (ERROR_SUCCESS) before calling GlobalDeleteAtom, then call 
		/// GetLastError. If the last error code is still ERROR_SUCCESS, 
		/// GlobalDeleteAtom has succeeded.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern short GlobalDeleteAtom 
			( 
				short nAtom
			);

		/// <summary>
		/// The GrayString function draws gray text at the specified 
		/// location. The function draws the text by copying it into 
		/// a memory bitmap, graying the bitmap, and then copying the 
		/// bitmap to the screen. The function grays the text regardless 
		/// of the selected brush and background. GrayString uses the 
		/// font currently selected for the specified device context.
		/// If the lpOutputFunc parameter is NULL, GDI uses the TextOut 
		/// function, and the lpData parameter is assumed to be a pointer 
		/// to the character string to be output. If the characters to be 
		/// output cannot be handled by TextOut (for example, the string 
		/// is stored as a bitmap), the application must supply its own 
		/// output function.</summary>
		/// <param name="hDC">Handle to the device context.</param>
		/// <param name="hBrush">Handle to the brush to be used for graying. 
		/// If this parameter is NULL, the text is grayed with the same brush 
		/// that was used to draw window text.</param>
		/// <param name="lpOutputFunc">Pointer to the application-defined 
		/// function that will draw the string, or, if TextOut is to be used 
		/// to draw the string, it is a NULL pointer.</param>
		/// <param name="lpData">Specifies a pointer to data to be passed 
		/// to the output function. If the lpOutputFunc parameter is NULL, 
		/// lpData must be a pointer to the string to be output.</param>
		/// <param name="nCount">Specifies the number of characters to be 
		/// output. If the nCount parameter is zero, GrayString calculates 
		/// the length of the string (assuming lpData is a pointer to the 
		/// string). If nCount is –1 and the function pointed to by 
		/// lpOutputFunc returns FALSE, the image is shown but 
		/// not grayed.</param>
		/// <param name="X">Specifies the device x-coordinate of the 
		/// starting position of the rectangle that encloses the string.</param>
		/// <param name="Y">Specifies the device y-coordinate of the 
		/// starting position of the rectangle that encloses the string.</param>
		/// <param name="nWidth">Specifies the width, in device units, 
		/// of the rectangle that encloses the string. If this parameter 
		/// is zero, GrayString calculates the width of the area, assuming 
		/// lpData is a pointer to the string. </param>
		/// <param name="nHeight">Specifies the height, in device units, 
		/// of the rectangle that encloses the string. If this parameter is 
		/// zero, GrayString calculates the height of the area, assuming 
		/// lpData is a pointer to the string.</param>
		/// <returns>If the string is drawn, the return value is nonzero.
		/// If either the TextOut function or the application-defined 
		/// output function returned zero, or there was insufficient 
		/// memory to create a memory bitmap for graying, the return 
		/// value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GrayString 
			(
				IntPtr hDC,
				IntPtr hBrush,
				OutputProc lpOutputFunc,
				IntPtr lpData,
				int nCount,
				int X,
				int Y,
				int nWidth,
				int nHeight
			);

		/// <summary>
		/// The GrayString function draws gray text at the specified 
		/// location. The function draws the text by copying it into 
		/// a memory bitmap, graying the bitmap, and then copying the 
		/// bitmap to the screen. The function grays the text regardless 
		/// of the selected brush and background. GrayString uses the 
		/// font currently selected for the specified device context.
		/// If the lpOutputFunc parameter is NULL, GDI uses the TextOut 
		/// function, and the lpData parameter is assumed to be a pointer 
		/// to the character string to be output. If the characters to be 
		/// output cannot be handled by TextOut (for example, the string 
		/// is stored as a bitmap), the application must supply its own 
		/// output function.</summary>
		/// <param name="hDC">Handle to the device context.</param>
		/// <param name="hBrush">Handle to the brush to be used for graying. 
		/// If this parameter is NULL, the text is grayed with the same brush 
		/// that was used to draw window text.</param>
		/// <param name="lpOutputFunc">Pointer to the application-defined 
		/// function that will draw the string, or, if TextOut is to be used 
		/// to draw the string, it is a NULL pointer.</param>
		/// <param name="lpData">Specifies a pointer to data to be passed 
		/// to the output function. If the lpOutputFunc parameter is NULL, 
		/// lpData must be a pointer to the string to be output.</param>
		/// <param name="nCount">Specifies the number of characters to be 
		/// output. If the nCount parameter is zero, GrayString calculates 
		/// the length of the string (assuming lpData is a pointer to the 
		/// string). If nCount is –1 and the function pointed to by 
		/// lpOutputFunc returns FALSE, the image is shown but 
		/// not grayed.</param>
		/// <param name="X">Specifies the device x-coordinate of the 
		/// starting position of the rectangle that encloses the string.</param>
		/// <param name="Y">Specifies the device y-coordinate of the 
		/// starting position of the rectangle that encloses the string.</param>
		/// <param name="nWidth">Specifies the width, in device units, 
		/// of the rectangle that encloses the string. If this parameter 
		/// is zero, GrayString calculates the width of the area, assuming 
		/// lpData is a pointer to the string. </param>
		/// <param name="nHeight">Specifies the height, in device units, 
		/// of the rectangle that encloses the string. If this parameter is 
		/// zero, GrayString calculates the height of the area, assuming 
		/// lpData is a pointer to the string.</param>
		/// <returns>If the string is drawn, the return value is nonzero.
		/// If either the TextOut function or the application-defined 
		/// output function returned zero, or there was insufficient 
		/// memory to create a memory bitmap for graying, the return 
		/// value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool GrayString
			(
				IntPtr hDC,
				IntPtr hBrush,
				IntPtr lpOutputFunc,
				string lpData,
				int nCount,
				int X,
				int Y,
				int nWidth,
				int nHeight
			);

		/// <summary>
		/// Initiates a shutdown and optional restart of the 
		/// specified computer.
		/// </summary>
		/// <param name="lpMachineName">Pointer to the null-terminated 
		/// string that specifies the network name of the computer to 
		/// shut down. If lpMachineName is NULL or an empty string, 
		/// the function shuts down the local computer.</param>
		/// <param name="lpMessage">Pointer to a null-terminated string 
		/// that specifies a message to display in the shutdown dialog 
		/// box. This parameter can be NULL if no message is required.
		/// </param>
		/// <param name="dwTimeout"><para>Time that the shutdown dialog box 
		/// should be displayed, in seconds. While this dialog box is 
		/// displayed, the shutdown can be stopped by the 
		/// AbortSystemShutdown function.</para>
		/// <para>If dwTimeout is not zero, InitiateSystemShutdown 
		/// displays a dialog box on the specified computer. The dialog 
		/// box displays the name of the user who called the function, 
		/// displays the message specified by the lpMessage parameter, 
		/// and prompts the user to log off. The dialog box beeps when 
		/// it is created and remains on top of other windows in the 
		/// system. The dialog box can be moved but not closed. 
		/// A timer counts down the remaining time before a forced 
		/// shutdown.</para></param>
		/// <param name="bForceAppsClosed"><para>If this parameter is 
		/// TRUE, applications with unsaved changes are to be forcibly 
		/// closed. Note that this can result in data loss.</para>
		/// <para>If this parameter is FALSE, the system displays 
		/// a dialog box instructing the user to close the applications.
		/// </para></param>
		/// <param name="bRebootAfterShutdown"><para>If this parameter 
		/// is TRUE, the computer is to restart immediately after 
		/// shutting down.</para>
		/// <para>If this parameter is FALSE, the system flushes all 
		/// caches to disk, clears the screen, and displays a message 
		/// indicating that it is safe to power down.</para></param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		/// <remarks><para>To shut down the local computer, the calling 
		/// thread must have the SE_SHUTDOWN_NAME privilege. To shut 
		/// down a remote computer, the calling thread must have the 
		/// SE_REMOTE_SHUTDOWN_NAME privilege on the remote computer. 
		/// By default, users can enable the SE_SHUTDOWN_NAME privilege 
		/// on the computer they are logged onto, and administrators can 
		/// enable the SE_REMOTE_SHUTDOWN_NAME privilege on remote 
		/// computers.</para>
		/// <para>Included in: Windows NT/XP/2000/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool InitiateSystemShutdown
			(
				string lpMachineName,
				string lpMessage,
				uint dwTimeout,
				bool bForceAppsClosed,
				bool bRebootAfterShutdown
			);

		/// <summary>
		/// The InSendMessage function determines whether the current 
		/// window procedure is processing a message that was sent from 
		/// another thread (in the same process or a different process) 
		/// by a call to the SendMessage function.
		/// </summary>
		/// <returns>If the window procedure is processing a message sent 
		/// to it from another thread using the SendMessage function, 
		/// the return value is nonzero.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool InSendMessage ();

		/// <summary>
		/// Determines whether the current window procedure is processing 
		/// a message that was sent from another thread (in the same process 
		/// or a different process).
		/// </summary>
		/// <param name="lpReserved">Reserved; must be NULL.</param>
		/// <returns>If the message was not sent, the return value is 
		/// ISMEX_NOSEND.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern InSendMessageCode InSendMessageEx
			(
				ref int lpReserved
			);

		/// <summary>
		/// The InvalidateRect function adds a rectangle to the specified 
		/// window's update region. The update region represents the portion 
		/// of the window's client area that must be redrawn.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region has 
		/// changed. If this parameter is NULL, the system invalidates and 
		/// redraws all windows, and sends the WM_ERASEBKGND and WM_NCPAINT 
		/// messages to the window procedure before the function returns.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that contains the 
		/// client coordinates of the rectangle to be added to the update region. 
		/// If this parameter is NULL, the entire client area is added to the 
		/// update region.</param>
		/// <param name="bErase">Specifies whether the background within the 
		/// update region is to be erased when the update region is processed. 
		/// If this parameter is TRUE, the background is erased when the 
		/// BeginPaint function is called. If this parameter is FALSE, the 
		/// background remains unchanged.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool InvalidateRect
			(
				IntPtr hWnd,
				ref Rectangle lpRect,
				bool bErase
			);

		/// <summary>
		/// The InvalidateRect function adds a rectangle to the specified 
		/// window's update region. The update region represents the portion 
		/// of the window's client area that must be redrawn.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region has 
		/// changed. If this parameter is NULL, the system invalidates and 
		/// redraws all windows, and sends the WM_ERASEBKGND and WM_NCPAINT 
		/// messages to the window procedure before the function returns.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that contains the 
		/// client coordinates of the rectangle to be added to the update region. 
		/// If this parameter is NULL, the entire client area is added to the 
		/// update region.</param>
		/// <param name="bErase">Specifies whether the background within the 
		/// update region is to be erased when the update region is processed. 
		/// If this parameter is TRUE, the background is erased when the 
		/// BeginPaint function is called. If this parameter is FALSE, the 
		/// background remains unchanged.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool InvalidateRect
			(
				IntPtr hWnd,
				IntPtr lpRect,
				bool bErase
			);

		/// <summary>
		/// The InvalidateRgn function invalidates the client area within 
		/// the specified region by adding it to the current update region 
		/// of a window. The invalidated region, along with all other areas 
		/// in the update region, is marked for painting when the next 
		/// WM_PAINT message occurs. 
		/// </summary>
		/// <param name="hWnd">Handle to the window with an update region 
		/// that is to be modified.</param>
		/// <param name="hRgn">Handle to the region to be added to the update 
		/// region. The region is assumed to have client coordinates. 
		/// If this parameter is NULL, the entire client area is added to 
		/// the update region.</param>
		/// <param name="bErase">Specifies whether the background within 
		/// the update region should be erased when the update region is 
		/// processed. If this parameter is TRUE, the background is erased 
		/// when the BeginPaint function is called. If the parameter is 
		/// FALSE, the background remains unchanged.</param>
		/// <returns>The return value is always nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool InvalidateRgn 
			(
				IntPtr hWnd,
				IntPtr hRgn,
				bool bErase
			);

		/// <summary>
		/// The IsChild function tests whether a window is a child 
		/// window or descendant window of a specified parent window. 
		/// A child window is the direct descendant of a specified 
		/// parent window if that parent window is in the chain of 
		/// parent windows; the chain of parent windows leads from the 
		/// original overlapped or pop-up window to the child window. 
		/// </summary>
		/// <param name="hWndParent">Handle to the parent window.</param>
		/// <param name="hWnd">Handle to the window to be tested.</param>
		/// <returns>If the window is a child or descendant window of 
		/// the specified parent window, the return value is nonzero.
		/// If the window is not a child or descendant window of the 
		/// specified parent window, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsChild
			(
				IntPtr hWndParent,
				IntPtr hWnd
			);

		/// <summary>
		/// Determines whether a message is intended for the specified 
		/// dialog box and, if it is, processes the message.
		/// </summary>
		/// <param name="hDlg">Handle to the dialog box.</param>
		/// <param name="lpMsg">Pointer to an MSG structure that contains 
		/// the message to be checked.</param>
		/// <returns>If the message has been processed, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool IsDialogMessage
			(
				IntPtr hDlg,
				ref MSG lpMsg
			);

		/// <summary>
		/// The IsGUIThread function tests whether the calling 
		/// thread is already a graphical user interface (GUI) 
		/// thread. It can also optionally convert the thread to 
		/// a GUI thread.
		/// </summary>
		/// <param name="bConvert">If TRUE and the thread is not 
		/// a GUI thread, convert thread to a GUI thread.</param>
		/// <returns><para>The function returns a nonzero value 
		/// in the following situations:</para>
		/// <list type="bullet">
		/// <item>If the calling thread is already a GUI thread.
		/// </item>
		/// <item>If bConvert is TRUE and the function successfully 
		/// converts the thread to a GUI thread.</item>
		/// </list></returns>
		/// <remarks>Included in: Windows XP/2003.</remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsGUIThread
			(
				bool bConvert
			);

		/// <summary>
		/// You call the IsHungAppWindow function to determine if 
		/// Microsoft® Windows® considers that a specified application 
		/// is not responding, or "hung". An application is considered 
		/// to be not responding if it is not waiting for input, is not 
		/// in startup processing, and has not called PeekMessage within 
		/// the internal timeout period of 5 seconds. 
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <returns>Returns TRUE if the window stops responding, 
		/// otherwise returns FALSE. Ghost windows always return TRUE.
		/// </returns>
		/// <remarks>
		/// <para>The Windows timeout criteria of 5 seconds is subject 
		/// to change.</para>
		/// <para>Although you can access this function by using 
		/// LoadLibrary and GetProcAddress combined in Windows versions 
		/// prior to Windows XP, the function is not accessible using 
		/// the standard Include file and library linkage. The header 
		/// files included in Windows XP Service Pack 1 (SP1) and 
		/// Windows Server 2003 document this function and make it 
		/// accessible using the appropriate Include file and library 
		/// linkage. However, this function is not intended for general 
		/// use. It is recommended that you do not use it in new 
		/// programs because it might be altered or unavailable in 
		/// subsequent versions of Windows.</para>
		/// <para>Included in: Windows 2000/XP/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsHungAppWindow
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Determines whether the specified window is minimized 
		/// (iconic).
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns>If the window is iconic, the return value is 
		/// nonzero. If the window is not iconic, the return value 
		/// is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsIconic
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Determines whether the specified window handle identifies 
		/// an existing window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns>If the window handle identifies an existing window, 
		/// the return value is nonzero. If the window handle does not 
		/// identify an existing window, the return value is zero.
		/// </returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool IsWindow
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Determines whether the specified window is a native Unicode window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns><para>If the window is a native Unicode window, 
		/// the return value is nonzero.</para>
		/// <para>If the window is not a native Unicode window, 
		/// the return value is zero. The window is a native ANSI 
		/// window.</para>
		/// <para>Windows 95/98/Me: Under the , the return value is 
		/// nonzero (TRUE) if hWnd belongs to a window created by a 
		/// function, for example CreateWindowExW", that is supported 
		/// by the Microsoft® Layer for Unicode (MSLU). However, if 
		/// CreateWindowExA creates the window, it returns zero (TRUE).
		/// </para></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsWindowUnicode
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Retrieves the visibility state of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns><para>If the specified window, its parent window, 
		/// its parent's parent window, and so forth, have the 
		/// WS_VISIBLE style, the return value is nonzero. Otherwise, 
		/// the return value is zero.</para>
		/// <para>Because the return value specifies whether the window 
		/// has the WS_VISIBLE style, it may be nonzero even if the 
		/// window is totally obscured by other windows.</para>
		/// </returns>
		/// <remarks>
		/// <para>The visibility state of a window is indicated by the 
		/// WS_VISIBLE style bit. When WS_VISIBLE is set, the window is 
		/// displayed and subsequent drawing into it is displayed as 
		/// long as the window has the WS_VISIBLE style.</para>
		/// <para>Any drawing to a window with the WS_VISIBLE style 
		/// will not be displayed if the window is obscured by other 
		/// windows or is clipped by its parent window.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsWindowVisible
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Determines whether a window is maximized.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns>If the window is zoomed, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool IsZoomed
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The keybd_event function synthesizes a keystroke. The system 
		/// can use such a synthesized keystroke to generate a WM_KEYUP 
		/// or WM_KEYDOWN message. The keyboard driver's interrupt handler 
		/// calls the keybd_event function.
		/// </summary>
		/// <param name="bVk"> Specifies a virtual-key code. The code must 
		/// be a value in the range 1 to 254.</param>
		/// <param name="bScan">This parameter is not used.</param>
		/// <param name="dwFlags"> Specifies various aspects of function 
		/// operation.</param>
		/// <param name="dwExtraInfo">Specifies an additional value associated 
		/// with the key stroke.</param>
		[DllImport ( DllName, SetLastError = false )]
		public static extern void keybd_event 
			( 
				byte bVk,
				byte bScan,
				int dwFlags,
				IntPtr dwExtraInfo
			);

		/// <summary>
		/// <para>The LoadIcon function loads the specified icon 
		/// resource from the executable (.exe) file associated with 
		/// an application instance.</para>
		/// <para>Note: This function hase been superseded by the 
		/// LoadImage function.</para></summary>
		/// <param name="hInstance">Handle to an instance of the module 
		/// whose executable file contains the icon to be loaded. 
		/// This parameter must be NULL when a standard icon is being 
		/// loaded.</param>
		/// <param name="lpIconName">Pointer to a null-terminated string 
		/// that contains the name of the icon resource to be loaded. 
		/// Alternatively, this parameter can contain the resource 
		/// identifier in the low-order word and zero in the high-order 
		/// word. Use the MAKEINTRESOURCE macro to create this value.
		/// </param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr LoadIcon
			(
				IntPtr hInstance,
				string lpIconName
			);

		/// <summary>
		/// Loads an icon, cursor, animated cursor, or bitmap.
		/// </summary>
		/// <param name="hinst">Handle to an instance of the module that 
		/// contains the image to be loaded. To load an OEM image, set 
		/// this parameter to zero.</param>
		/// <param name="lpszName"><para>Specifies the image to load. 
		/// If the hinst parameter is non-NULL and the fuLoad parameter 
		/// omits LR_LOADFROMFILE, lpszName specifies the image resource 
		/// in the hinst module. If the image resource is to be loaded 
		/// by name, the lpszName parameter is a pointer to a 
		/// null-terminated string that contains the name of the image 
		/// resource. If the image resource is to be loaded by ordinal, 
		/// use the MAKEINTRESOURCE macro to convert the image ordinal 
		/// into a form that can be passed to the LoadImage function.
		/// </para>
		/// <para>If the hinst parameter is NULL and the fuLoad 
		/// parameter omits the LR_LOADFROMFILE value, the lpszName 
		/// specifies the OEM image to load.</para></param>
		/// <param name="uType">Specifies the type of image to be loaded.
		/// </param>
		/// <param name="cxDesired">Specifies the width, in pixels, 
		/// of the icon or cursor. If this parameter is zero and the 
		/// fuLoad parameter is LR_DEFAULTSIZE, the function uses the 
		/// SM_CXICON or SM_CXCURSOR system metric value to set the width. 
		/// If this parameter is zero and LR_DEFAULTSIZE is not used, 
		/// the function uses the actual resource width.</param>
		/// <param name="cyDesired">Specifies the height, in pixels, 
		/// of the icon or cursor. If this parameter is zero and the 
		/// fuLoad parameter is LR_DEFAULTSIZE, the function uses the 
		/// SM_CYICON or SM_CYCURSOR system metric value to set the 
		/// height. If this parameter is zero and LR_DEFAULTSIZE is not 
		/// used, the function uses the actual resource height.</param>
		/// <param name="fuLoad">Options for image loading.</param>
		/// <returns>If the function succeeds, the return value is 
		/// the handle of the newly loaded image. If the function fails, 
		/// the return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr LoadImage
			(
				IntPtr hinst,
				string lpszName,
				LoadImageFlags uType,
				int cxDesired,
				int cyDesired,
				LoadResourceFlags fuLoad
			);

		/// <summary>
		/// The foreground process can call the LockSetForegroundWindow 
		/// function to disable calls to the SetForegroundWindow function.
		/// </summary>
		/// <param name="uLockCode">Specifies whether to enable or 
		/// disable calls to SetForegroundWindow. This parameter can 
		/// be one of the following values:
		/// <list type="bullet">
		/// <item>LSFW_LOCK</item>
		/// <item>LSFW_UNLOCK</item>
		/// </list>
		/// </param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>
		/// <para>The system automatically enables calls to 
		/// SetForegroundWindow if the user presses the ALT key or 
		/// takes some action that causes the system itself to change 
		/// the foreground window (for example, clicking a background 
		/// window).</para>
		/// <para>This function is provided so applications can prevent 
		/// other applications from making a foreground change that 
		/// can interrupt its interaction with the user.</para>
		/// <para>Included in: Windows ME/XP/2000/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern bool LockSetForegroundWindow
			(
				uint uLockCode
			);

		/// <summary>
		/// Submits a request to lock the workstation's display. 
		/// Locking a workstation protects it from unauthorized use.
		/// </summary>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		/// <remarks>Included in: Windows XP/2000/2003.</remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool LockWorkstation ();

		/// <summary>
		/// Disables or enables drawing in the specified window. 
		/// Only one window can be locked at a time.
		/// </summary>
		/// <param name="hWndLock">Specifies the window in which drawing 
		/// will be disabled. If this parameter is NULL, drawing in the 
		/// locked window is enabled.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool LockWindowUpdate 
			(
				IntPtr hWndLock
			);

		/// <summary>
		/// The MAKEINTRESOURCE macro converts an integer value to a 
		/// resource type compatible with the resource-management 
		/// functions. This macro is used in place of a string 
		/// containing the name of the resource. 
		/// </summary>
		/// <param name="resourceID"></param>
		/// <returns></returns>
		public static string MAKEINTRESOURCE
			(
				int resourceID
			)
		{
			return "#" + resourceID;
		}

		/// <summary>
		/// The MessageBeep function plays a waveform sound. The 
		/// waveform sound for each sound type is identified by an 
		/// entry in the registry.
		/// </summary>
		/// <param name="uType">Sound type, as identified by an entry 
		/// in the registry.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool MessageBeep
			(
				MessageBeepFlags uType
			);

		/// <summary>
		/// The MessageBox function creates, displays, and operates 
		/// a message box. The message box contains an application-defined 
		/// message and title, plus any combination of predefined icons 
		/// and push buttons.
		/// </summary>
		/// <param name="hWnd">Handle to the owner window of the message 
		/// box to be created. If this parameter is NULL, the message 
		/// box has no owner window.</param>
		/// <param name="lpText">Pointer to a null-terminated string that 
		/// contains the message to be displayed.</param>
		/// <param name="lpCaption">Pointer to a null-terminated string 
		/// that contains the dialog box title. If this parameter is NULL, 
		/// the default title Error is used.</param>
		/// <param name="uType">Specifies the contents and behavior of the 
		/// dialog box.</param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern MessageBoxResult MessageBox
			(
				IntPtr hWnd,
				string lpText,
				string lpCaption,
				MessageBoxFlags uType
			);

		/// <summary>
		/// Retrieves a handle to the display monitor that contains 
		/// a specified point.
		/// </summary>
		/// <param name="pt">A POINT structure that specifies the 
		/// point of interest in virtual-screen coordinates.</param>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr MonitorFromPoint
			(
				Point pt,
				MonitorFlags dwFlags
			);

		/// <summary>
		/// Retrieves a handle to the display monitor that has the 
		/// largest area of intersection with a specified rectangle.
		/// </summary>
		/// <param name="lprc"></param>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr MonitorFromRect
			(
				ref Rectangle lprc,
				MonitorFlags dwFlags
			);

		/// <summary>
		/// Retrieves a handle to the display monitor that has the 
		/// largest area of intersection with the bounding rectangle 
		/// of a specified window. 
		/// </summary>
		/// <param name="hwnd">Handle to the window of interest.</param>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr MonitorFromWindow
			(
				IntPtr hwnd,
				MonitorFlags dwFlags
			);

		/// <summary>
		/// The MoveWindow function changes the position and dimensions 
		/// of the specified window. For a top-level window, the 
		/// position and dimensions are relative to the upper-left 
		/// corner of the screen. For a child window, they are relative 
		/// to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="X">Specifies the new position of the left side 
		/// of the window.</param>
		/// <param name="Y">Specifies the new position of the top of 
		/// the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.
		/// </param>
		/// <param name="nHeight">Specifies the new height of the window.
		/// </param>
		/// <param name="bRepaint">Specifies whether the window is to be 
		/// repainted. If this parameter is TRUE, the window receives a 
		/// message. If the parameter is FALSE, no repainting of any 
		/// kind occurs. This applies to the client area, the nonclient 
		/// area (including the title bar and scroll bars), and any part 
		/// of the parent window uncovered as a result of moving a child 
		/// window.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks><para>If the bRepaint parameter is TRUE, the 
		/// system sends the WM_PAINT message to the window procedure 
		/// immediately after moving the window (that is, the MoveWindow 
		/// function calls the UpdateWindow function). If bRepaint is 
		/// FALSE, the application must explicitly invalidate or redraw 
		/// any parts of the window and parent window that need 
		/// redrawing.</para>
		/// <para>MoveWindow sends the WM_WINDOWPOSCHANGING, 
		/// WM_WINDOWPOSCHANGED, WM_MOVE, WM_SIZE, and WM_NCCALCSIZE 
		/// messages to the window.</para></remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool MoveWindow
			(
				IntPtr hWnd,
				int X,
				int Y,
				int nWidth,
				int nHeight,
				bool bRepaint
			);

		/// <summary>
		/// The PaintDesktop function fills the clipping region in the 
		/// specified device context with the desktop pattern or wallpaper. 
		/// The function is provided primarily for shell desktops.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool PaintDesktop 
			(
				IntPtr hdc
			);

		/// <summary>
		/// Dispatches incoming sent messages, checks the thread message queue 
		/// for a posted message, and retrieves the message (if any exist).
		/// </summary>
		/// <param name="lpMsg">Pointer to an MSG structure that receives 
		/// message information.</param>
		/// <param name="hWnd"><para>Handle to the window whose messages are 
		/// to be examined. The window must belong to the current thread.
		/// </para>
		/// <para>If hWnd is NULL, PeekMessage retrieves messages for any 
		/// window that belongs to the current thread. If hWnd is 
		/// INVALID_HANDLE_VALUE, PeekMessage retrieves messages whose hWnd 
		/// value is NULL, as posted by the PostThreadMessage function.
		/// </para></param>
		/// <param name="wMsgFilterMin"><para>Specifies the value of the first 
		/// message in the range of messages to be examined. Use WM_KEYFIRST 
		/// to specify the first keyboard message or WM_MOUSEFIRST to specify 
		/// the first mouse message.</para>
		/// <para>If wMsgFilterMin and wMsgFilterMax are both zero, PeekMessage 
		/// returns all available messages (that is, no range filtering is 
		/// performed).</para></param>
		/// <param name="wMsgFilterMax"><para>Specifies the value of the last 
		/// message in the range of messages to be examined. Use WM_KEYLAST to 
		/// specify the first keyboard message or WM_MOUSELAST to specify the 
		/// last mouse message.</para>
		/// <para>If wMsgFilterMin and wMsgFilterMax are both zero, PeekMessage 
		/// returns all available messages (that is, no range filtering is 
		/// performed).</para></param>
		/// <param name="wRemoveMsg">Specifies how messages are handled.</param>
		/// <returns>If a message is available, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool PeekMessage 
			( 
				out MSG lpMsg,
				IntPtr hWnd,
				int wMsgFilterMin,
				int wMsgFilterMax,
				PeekMessageFlags wRemoveMsg
			);

		/// <summary>
		/// The PostMessage function places (posts) a message in the 
		/// message queue associated with the thread that created the 
		/// specified window and returns without waiting for the thread 
		/// to process the message. 
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose window 
		/// procedure is to receive the message.</para>
		/// <list type="bullet">
		/// <item>HWND_BROADCAST - The message is posted to all top-level 
		/// windows in the system, including disabled or invisible unowned 
		/// windows, overlapped windows, and pop-up windows.</item>
		/// <item>NULL - The function behaves like a call to PostThreadMessage 
		/// with the dwThreadId parameter set to the identifier of the current 
		/// thread.</item>
		/// </list></param>
		/// <param name="Msg">Specifies the message to be posted.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool PostMessage
			(
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// The PostMessage function places (posts) a message in the 
		/// message queue associated with the thread that created the 
		/// specified window and returns without waiting for the thread 
		/// to process the message. 
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose window 
		/// procedure is to receive the message.</para>
		/// <list type="bullet">
		/// <item>HWND_BROADCAST - The message is posted to all top-level 
		/// windows in the system, including disabled or invisible unowned 
		/// windows, overlapped windows, and pop-up windows.</item>
		/// <item>NULL - The function behaves like a call to PostThreadMessage 
		/// with the dwThreadId parameter set to the identifier of the current 
		/// thread.</item>
		/// </list></param>
		/// <param name="Msg">Specifies the message to be posted.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool PostMessage
			(
				IntPtr hWnd,
				WindowMessage Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// The PostMessage function places (posts) a message in the 
		/// message queue associated with the thread that created the 
		/// specified window and returns without waiting for the thread 
		/// to process the message. 
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose window 
		/// procedure is to receive the message.</para>
		/// <list type="bullet">
		/// <item>HWND_BROADCAST - The message is posted to all top-level 
		/// windows in the system, including disabled or invisible unowned 
		/// windows, overlapped windows, and pop-up windows.</item>
		/// <item>NULL - The function behaves like a call to PostThreadMessage 
		/// with the dwThreadId parameter set to the identifier of the current 
		/// thread.</item>
		/// </list></param>
		/// <param name="Msg">Specifies the message to be posted.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool PostMessage
			(
				IntPtr hWnd,
				WindowMessage Msg,
				SystemMenuCommand wParam,
				int lParam
			);

		/// <summary>
		/// The PostQuitMessage function indicates to the system that 
		/// a thread has made a request to terminate (quit). It is typically 
		/// used in response to a WM_DESTROY message
		/// </summary>
		/// <param name="nExitCode">Specifies an application exit code. 
		/// This value is used as the wParam parameter of the WM_QUIT message.
		/// </param>
		/// <remarks><para>The PostQuitMessage function posts a WM_QUIT 
		/// message to the thread's message queue and returns immediately; 
		/// the function simply indicates to the system that the thread is 
		/// requesting to quit at some time in the future.</para>
		/// <para>When the thread retrieves the WM_QUIT message from its 
		/// message queue, it should exit its message loop and return control 
		/// to the system. The exit value returned to the system must be the 
		/// wParam parameter of the WM_QUIT message.</para></remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern void PostQuitMessage
			(
				int nExitCode
			);

		/// <summary>
		/// The PostThreadMessage function posts a message to the message 
		/// queue of the specified thread. It returns without waiting for 
		/// the thread to process the message.
		/// </summary>
		/// <param name="idThread"><para>Identifier of the thread to which 
		/// the message is to be posted.</para>
		/// <para>The function fails if the specified thread does not have 
		/// a message queue. The system creates a thread's message queue 
		/// when the thread makes its first call to one of the User or GDI 
		/// functions. For more information, see the Remarks section.</para>
		/// <para>Windows 2000/XP: This thread must either belong to the 
		/// same desktop as the calling thread or to a process with the 
		/// same locally unique identifier (LUID). Otherwise, the function 
		/// fails and returns ERROR_INVALID_THREAD_ID.</para></param>
		/// <param name="Msg">Specifies the type of message to be posted.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool PostThreadMessage
			(
				IntPtr idThread,
				int Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// Updates the specified rectangle or region in a window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be redrawn. If this 
		/// parameter is NULL, the desktop window is updated.</param>
		/// <param name="lprcUpdate">Pointer to a Rectangle structure containing 
		/// the coordinates, in device units, of the update rectangle. 
		/// This parameter is ignored if the hrgnUpdate parameter identifies 
		/// a region.</param>
		/// <param name="hrgnUpdate">Handle to the update region. If both the 
		/// hrgnUpdate and lprcUpdate parameters are NULL, the entire client 
		/// area is added to the update region.</param>
		/// <param name="flags">Specifies one or more redraw flags.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool RedrawWindow
			(
				IntPtr hWnd,
				ref Rectangle lprcUpdate,
				IntPtr hrgnUpdate,
				RedrawWindowFlags flags
			);

		/// <summary>
		/// Updates the specified rectangle or region in a window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be redrawn. If this 
		/// parameter is NULL, the desktop window is updated.</param>
		/// <param name="lprcUpdate">Pointer to a Rectangle structure containing 
		/// the coordinates, in device units, of the update rectangle. 
		/// This parameter is ignored if the hrgnUpdate parameter identifies 
		/// a region.</param>
		/// <param name="hrgnUpdate">Handle to the update region. If both the 
		/// hrgnUpdate and lprcUpdate parameters are NULL, the entire client 
		/// area is added to the update region.</param>
		/// <param name="flags">Specifies one or more redraw flags.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool RedrawWindow
			(
				IntPtr hWnd,
				IntPtr lprcUpdate,
				IntPtr hrgnUpdate,
				RedrawWindowFlags flags
			);

		/// <summary>
		/// Defines a system-wide hot key.
		/// </summary>
		/// <param name="hWnd">Handle to the window that will receive 
		/// WM_HOTKEY messages generated by the hot key. If this parameter 
		/// is NULL, WM_HOTKEY messages are posted to the message queue 
		/// of the calling thread and must be processed in the message 
		/// loop.</param>
		/// <param name="id">Specifies the identifier of the hot key. 
		/// No other hot key in the calling thread should have the same 
		/// identifier. An application must specify a value in the range 
		/// 0x0000 through 0xBFFF. A shared dynamic-link library (DLL) 
		/// must specify a value in the range 0xC000 through 0xFFFF 
		/// (the range returned by the GlobalAddAtom function). To avoid 
		/// conflicts with hot-key identifiers defined by other shared DLLs, 
		/// a DLL should use the GlobalAddAtom function to obtain the 
		/// hot-key identifier.</param>
		/// <param name="fsModifiers">Specifies keys that must be pressed 
		/// in combination with the key specified by the uVirtKey parameter 
		/// in order to generate the WM_HOTKEY message.</param>
		/// <param name="vk">Specifies the virtual-key code of the hot key.
		/// </param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		/// <remarks><para>When a key is pressed, the system looks for 
		/// a match against all hot keys. Upon finding a match, the system 
		/// posts the WM_HOTKEY message to the message queue of the thread 
		/// that registered the hot key. This message is posted to the 
		/// beginning of the queue so it is removed by the next iteration 
		/// of the message loop.</para>
		/// <para>This function cannot associate a hot key with a window 
		/// created by another thread.</para>
		/// <para>RegisterHotKey fails if the keystrokes specified for the 
		/// hot key have already been registered by another hot key.</para>
		/// <para>If the window identified by the hWnd parameter already 
		/// registered a hot key with the same identifier as that specified 
		/// by the id parameter, the new values for the fsModifiers and vk 
		/// parameters replace the previously specified values for these 
		/// parameters.</para>
		/// <para>Windows NT4 and Windows 2000/XP: The F12 key is reserved 
		/// for use by the debugger at all times, so it should not 
		/// be registered as a hot key. Even when you are not debugging an 
		/// application, F12 is reserved in case a kernel-mode debugger or 
		/// a just-in-time debugger is resident.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool RegisterHotKey 
			( 
				IntPtr hWnd,
				int id,
				HotkeyModifiers fsModifiers,
				Keys vk
			);

		/// <summary>
		/// The RegisterWindowMessage function defines a new window message 
		/// that is guaranteed to be unique throughout the system. The message 
		/// value can be used when sending or posting messages.
		/// </summary>
		/// <param name="lpString">Pointer to a null-terminated string that 
		/// specifies the message to be registered.</param>
		/// <returns>If the message is successfully registered, the return 
		/// value is a message identifier in the range 0xC000 through 0xFFFF.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int RegisterWindowMessage
			(
				string lpString
			);

		/// <summary>
		/// The ReleaseDC function releases a device context (DC), 
		/// freeing it for use by other applications. The effect of the ReleaseDC 
		/// function depends on the type of DC. It frees only common and 
		/// window DCs. It has no effect on class or private DCs.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose DC is 
		/// to be released.</param>
		/// <param name="hDC">Handle to the DC to be released.
		/// </param>
		/// <returns>The return value indicates whether the DC 
		/// was released. If the DC was released, the return value is 1. 
		/// If the DC was not released, the return value is zero.
		/// </returns>
		/// <remarks>
		/// <para>The application must call the ReleaseDC function for 
		/// each call to the GetWindowDC function and for each call to 
		/// the GetDC function that retrieves a common DC.</para>
		/// <para>An application cannot use the ReleaseDC function 
		/// to release a DC that was created by calling the CreateDC function; 
		/// instead, it must use the DeleteDC function. ReleaseDC must be 
		/// called from the same thread that called GetDC.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError=true )]
		public static extern int ReleaseDC 
			(
				IntPtr hWnd,
				IntPtr hDC
			);

		/// <summary>
		/// The ReplyMessage function is used to reply to a message sent 
		/// through the SendMessage function without returning control 
		/// to the function that called SendMessage.
		/// </summary>
		/// <param name="lResult">Specifies the result of the message processing. 
		/// The possible values are based on the message sent.</param>
		/// <returns>If the calling thread was processing a message sent from 
		/// another thread or process, the return value is nonzero.</returns>
		/// <remarks><para>By calling this function, the window procedure that 
		/// receives the message allows the thread that called SendMessage 
		/// to continue to run as though the thread receiving the message had 
		/// returned control. The thread that calls the ReplyMessage function 
		/// also continues to run.</para>
		/// <para>If the message was not sent through SendMessage or if the 
		/// message was sent by the same thread, ReplyMessage has no effect.
		/// </para></remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool ReplyMessage
			(
				int lResult
			);

		/// <summary>
		/// Scrolls a rectangle of bits horizontally and vertically.
		/// </summary>
		/// <param name="hDC">Handle to the device context that contains the 
		/// bits to be scrolled.</param>
		/// <param name="dx">Specifies the amount, in device units, of horizontal 
		/// scrolling. This parameter must be a negative value to scroll to the 
		/// left.</param>
		/// <param name="dy">Specifies the amount, in device units, of vertical 
		/// scrolling. This parameter must be a negative value to scroll up.
		/// </param>
		/// <param name="lprcScroll">Pointer to a Rectangle structure containing the 
		/// coordinates of the bits to be scrolled. The only bits affected by the 
		/// scroll operation are bits in the intersection of this rectangle and 
		/// the rectangle specified by lprcClip. If lprcScroll is NULL, the entire
		/// client area is used.</param>
		/// <param name="lprcClip">Pointer to a Rectangle structure containing the 
		/// coordinates of the clipping rectangle. The only bits that will be 
		/// painted are the bits that remain inside this rectangle after the 
		/// scroll operation has been completed. If lprcClip is NULL, the entire 
		/// client area is used.</param>
		/// <param name="hrgnUpdate">Handle to the region uncovered by the 
		/// scrolling process. ScrollDC defines this region; it is not 
		/// necessarily a rectangle.</param>
		/// <param name="lprcUpdate">Pointer to a Rectangle structure that receives 
		/// the coordinates of the rectangle bounding the scrolling update region. 
		/// This is the largest rectangular area that requires repainting. 
		/// When the function returns, the values in the structure are in client 
		/// coordinates, regardless of the mapping mode for the specified device 
		/// context. This allows applications to use the update region in a call 
		/// to the InvalidateRgn function, if required.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError=true )]
		public static extern bool ScrollDC
			(
				IntPtr hDC,
				int dx,
				int dy,
				ref Rectangle lprcScroll,
				ref Rectangle lprcClip,
				IntPtr hrgnUpdate,
				out Rectangle lprcUpdate
			);

		/// <summary>
		/// Scrolls the contents of the specified window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window where the client area 
		/// is to be scrolled.</param>
		/// <param name="XAmount"> Specifies the amount, in device units, 
		/// of horizontal scrolling. If the window being scrolled has the 
		/// CS_OWNDC or CS_CLASSDC style, then this parameter uses logical 
		/// units rather than device units. This parameter must be a negative 
		/// value to scroll the content of the window to the left.</param>
		/// <param name="YAmount">Specifies the amount, in device units, 
		/// of vertical scrolling. If the window being scrolled has the 
		/// CS_OWNDC or CS_CLASSDC style, then this parameter uses logical 
		/// units rather than device units. This parameter must be a negative 
		/// value to scroll the content of the window up.</param>
		/// <param name="lpRect">Pointer to the Rectangle structure specifying 
		/// the portion of the client area to be scrolled. If this parameter 
		/// is NULL, the entire client area is scrolled.</param>
		/// <param name="lpClipRect">Pointer to the Rectangle structure containing 
		/// the coordinates of the clipping rectangle. Only device bits within 
		/// the clipping rectangle are affected. Bits scrolled from the outside 
		/// of the rectangle to the inside are painted; bits scrolled from the 
		/// inside of the rectangle to the outside are not painted.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError=true )]
		public static extern bool ScrollWindow
			(
				IntPtr hWnd,
				int XAmount,
				int YAmount,
				ref Rectangle lpRect,
				ref Rectangle lpClipRect
			);

		/// <summary>
		/// Scrolls the contents of the specified window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window where the client area 
		/// is to be scrolled.</param>
		/// <param name="dx">Specifies the amount, in device units, of 
		/// horizontal scrolling. This parameter must be a negative value 
		/// to scroll to the left.</param>
		/// <param name="dy">Specifies the amount, in device units, of 
		/// vertical scrolling. This parameter must be a negative value 
		/// to scroll up.</param>
		/// <param name="prcScroll">Pointer to a Rectangle structure that specifies 
		/// the portion of the client area to be scrolled. If this parameter 
		/// is NULL, the entire client area is scrolled.</param>
		/// <param name="prcClip">Pointer to a Rectangle structure that contains 
		/// the coordinates of the clipping rectangle. Only device bits within 
		/// the clipping rectangle are affected. Bits scrolled from the outside 
		/// of the rectangle to the inside are painted; bits scrolled from the 
		/// inside of the rectangle to the outside are not painted. This 
		/// parameter may be NULL.</param>
		/// <param name="hrgnUpdate">Handle to the region that is modified to 
		/// hold the region invalidated by scrolling. This parameter may be NULL.
		/// </param>
		/// <param name="prcUpdate">Pointer to a Rectangle structure that receives 
		/// the boundaries of the rectangle invalidated by scrolling. This 
		/// parameter may be NULL.</param>
		/// <param name="flags">Specifies flags that control scrolling.</param>
		/// <returns>If the function succeeds, the return value is 
		/// SIMPLEREGION (rectangular invalidated region), COMPLEXREGION 
		/// (nonrectangular invalidated region; overlapping rectangles), 
		/// or NULLREGION (no invalidated region). If the function fails, 
		/// the return value is ERROR.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int ScrollWindowEx
			(
				IntPtr hWnd,
				int dx,
				int dy,
				ref Rectangle prcScroll,
				ref Rectangle prcClip,
				IntPtr hrgnUpdate,
				ref Rectangle prcUpdate,
				ScrollWindowFlags flags
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure 
		/// will receive the message. If this parameter is HWND_BROADCAST, 
		/// the message is sent to all top-level windows in the system, 
		/// including disabled or invisible unowned windows, overlapped 
		/// windows, and pop-up windows; but the message is not sent to 
		/// child windows.</param>
		/// <param name="msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>The return value specifies the result of the message 
		/// processing; it depends on the message sent.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage 
			( 
				IntPtr hWnd, 
				int msg,
				int wParam, 
				int lParam 
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure 
		/// will receive the message. If this parameter is HWND_BROADCAST, 
		/// the message is sent to all top-level windows in the system, 
		/// including disabled or invisible unowned windows, overlapped 
		/// windows, and pop-up windows; but the message is not sent to 
		/// child windows.</param>
		/// <param name="msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>The return value specifies the result of the message 
		/// processing; it depends on the message sent.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage 
			( 
				IntPtr hWnd, 
				WindowMessage msg,
				int wParam, 
				out IntPtr lParam 
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="msg"></param>
		/// <param name="dc"></param>
		/// <param name="opts"></param>
		/// <returns></returns>
		/// <remarks>This overload is intended for WM_PRINT message.</remarks>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage 
			( 
				IntPtr hWnd,
				WindowMessage msg, 
				IntPtr dc, 
				DrawingOptions opts 
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage
			(
				IntPtr hWnd,
				WindowMessage msg,
				IntPtr wParam,
				string lParam
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage
			(
				IntPtr hWnd,
				WindowMessage msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls 
		/// the window procedure for the specified window and does not 
		/// return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SendMessage
			(
				IntPtr hWnd,
				WindowMessage msg,
				SystemMenuCommand wParam,
				int lParam
			);

		/// <summary>
		/// The SendMessageCallback function sends the specified message 
		/// to a window or windows. It calls the window procedure for the 
		/// specified window and returns immediately. After the window 
		/// procedure processes the message, the system calls the specified 
		/// callback function, passing the result of the message processing 
		/// and an application-defined value to the callback function.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure 
		/// will receive the message. If this parameter is HWND_BROADCAST, 
		/// the message is sent to all top-level windows in the system, 
		/// including disabled or invisible unowned windows, overlapped 
		/// windows, and pop-up windows; but the message is not sent to 
		/// child windows.</param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lpCallBack"><para>Pointer to a callback function that 
		/// the system calls after the window procedure processes the message. 
		/// For more information, see SendAsyncProc.</para>
		/// <para>If hWnd is HWND_BROADCAST, the system calls the SendAsyncProc 
		/// callback function once for each top-level window.</para></param>
		/// <param name="dwData">Specifies an application-defined value to 
		/// be sent to the callback function pointed to by the lpCallBack 
		/// parameter</param>
		/// <returns>.
		/// If the function succeeds, the return value is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SendMessageCallback 
			( 
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam,
				SendAsyncProc lpCallBack,
				IntPtr dwData
			);

		/// <summary>
		/// Sends the specified message to one of more windows.
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window whose window 
		/// procedure will receive the message.</para>
		/// <para>If this parameter is HWND_BROADCAST, the message 
		/// is sent to all top-level windows in the system, including 
		/// disabled or invisible unowned windows. The function does 
		/// not return until each window has timed out. Therefore, 
		/// the total wait time can be up to the value of uTimeout 
		/// multiplied by the number of top-level windows.</para></param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="fuFlags">Specifies how to send the message.</param>
		/// <param name="uTimeout">Specifies the duration, in milliseconds, 
		/// of the time-out period. If the message is a broadcast message, 
		/// each window can use the full time-out period. For example, if 
		/// you specify a five second time-out period and there are three 
		/// top-level windows that fail to process the message, you could 
		/// have up to a 15 second delay.</param>
		/// <param name="lpdwResult">Receives the result of the message 
		/// processing. This value depends on the message sent.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int SendMessageTimeout
			(
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam,
				SendMessageTimeoutFlags fuFlags,
				int uTimeout,
				out int lpdwResult
			);

		/// <summary>
		/// The SendNotifyMessage function sends the specified message to 
		/// a window or windows. If the window was created by the calling 
		/// thread, SendNotifyMessage calls the window procedure for the 
		/// window and does not return until the window procedure has 
		/// processed the message. If the window was created by a different 
		/// thread, SendNotifyMessage passes the message to the window 
		/// procedure and returns immediately; it does not wait for the 
		/// window procedure to finish processing the message.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure 
		/// will receive the message. If this parameter is HWND_BROADCAST, 
		/// the message is sent to all top-level windows in the system, 
		/// including disabled or invisible unowned windows, overlapped 
		/// windows, and pop-up windows; but the message is not sent to 
		/// child windows.</param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific 
		/// information.</param>
		/// <param name="lParam">Specifies additional message-specific 
		/// information.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SendNotifyMessage
			(
				IntPtr hWnd,
				int Msg,
				int wParam,
				int lParam
			);

		/// <summary>
		/// The SetFocus function sets the keyboard focus to the specified 
		/// window. The window must be attached to the calling thread's 
		/// message queue.
		/// </summary>
		/// <param name="hWnd">Handle to the window that will receive the 
		/// keyboard input. If this parameter is NULL, keystrokes are 
		/// ignored.</param>
		/// <returns>If the function succeeds, the return value is the handle 
		/// to the window that previously had the keyboard focus. If the hWnd 
		/// parameter is invalid or the window is not attached to the calling 
		/// thread's message queue, the return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr SetFocus 
			( 
				IntPtr hWnd
			);

		/// <summary>
		/// The SetForegroundWindow function puts the thread that 
		/// created the specified window into the foreground and 
		/// activates the window. Keyboard input is directed to the 
		/// window, and various visual cues are changed for the user. 
		/// The system assigns a slightly higher priority to the thread 
		/// that created the foreground window than it does to other 
		/// threads. 
		/// </summary>
		/// <param name="hWnd">Handle to the window that should be 
		/// activated and brought to the foreground.</param>
		/// <returns>If the window was brought to the foreground, the 
		/// return value is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SetForegroundWindow
			(
				IntPtr hWnd
			);

		/// <summary>
		/// The SetMessageExtraInfo function sets the extra message information 
		/// for the current thread. Extra message information is an application- 
		/// or driver-defined value associated with the current thread's message 
		/// queue. An application can use the GetMessageExtraInfo function to 
		/// retrieve a thread's extra message information.
		/// </summary>
		/// <param name="lParam">Specifies the value to associate with the current 
		/// thread.</param>
		/// <returns>The return value is the previous value associated with the 
		/// current thread.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int SetMessageExtraInfo 
			( 
				int lParam
			);

		/// <summary>
		/// Changes the parent window of the specified child window.
		/// </summary>
		/// <param name="hWndChild">Handle to the child window.</param>
		/// <param name="hWndNewParent"><para>Handle to the new parent 
		/// window. If this parameter is NULL, the desktop window 
		/// becomes the new parent window.</para>
		/// <para>Windows 2000/XP: If this parameter is HWND_MESSAGE, 
		/// the child window becomes a message-only window.</para>
		/// </param>
		/// <returns>If the function succeeds, the return value is a 
		/// handle to the previous parent window. If the function fails, 
		/// the return value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr SetParent
			(
				IntPtr hWndChild,
				IntPtr hWndNewParent
			);

		/// <summary>
		/// The SetScrollInfo function sets the parameters of a scroll bar, 
		/// including the minimum and maximum scrolling positions, the page 
		/// size, and the position of the scroll box (thumb). The function 
		/// also redraws the scroll bar, if requested.
		/// </summary>
		/// <param name="hwnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the fnBar parameter.
		/// </param>
		/// <param name="fnBar">Specifies the type of scroll bar for which to 
		/// set parameters.</param>
		/// <param name="lpsi">Pointer to a SCROLLINFO structure.</param>
		/// <param name="fRedraw">Specifies whether the scroll bar is redrawn 
		/// to reflect the changes to the scroll bar. If this parameter is TRUE, 
		/// the scroll bar is redrawn, otherwise, it is not redrawn.</param>
		/// <returns>The return value is the current position of the scroll box.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int SetScrollInfo 
			( 
				IntPtr hwnd,
				ScrollBarFlags fnBar,
				ref SCROLLINFO lpsi,
				bool fRedraw
			);

		/// <summary>
		/// Function sets the position of the scroll box (thumb) in the 
		/// specified scroll bar and, if requested, redraws the scroll bar 
		/// to reflect the new position of the scroll box.
		/// </summary>
		/// <param name="hWnd">Handle to a scroll bar control or a window 
		/// with a standard scroll bar, depending on the value of the nBar 
		/// parameter.</param>
		/// <param name="nBar">Specifies the scroll bar to be set.</param>
		/// <param name="nPos">Specifies the new position of the scroll box. 
		/// The position must be within the scrolling range.</param>
		/// <param name="bRedraw">Specifies whether the scroll bar is redrawn 
		/// to reflect the new scroll box position. If this parameter is TRUE, 
		/// the scroll bar is redrawn. If it is FALSE, the scroll bar is not 
		/// redrawn.</param>
		/// <returns><para>If the function succeeds, the return value is the 
		/// previous position of the scroll box.</para>
		/// <para>Windows XP: If the desktop is themed and the parent window 
		/// is a message-only window, the function returns an incorrect value.
		/// </para>
		/// <para>If the function fails, the return value is zero.</para>
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int SetScrollPos 
			( 
				IntPtr hWnd,
				ScrollBarFlags nBar,
				int nPos,
				bool bRedraw
			);

		/// <summary>
		/// Sets the minimum and maximum scroll box positions for the specified 
		/// scroll bar.
		/// </summary>
		/// <param name="hWnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the nBar parameter.
		/// </param>
		/// <param name="nBar">Specifies the scroll bar to be set.</param>
		/// <param name="nMinPos">Specifies the minimum scrolling position.
		/// </param>
		/// <param name="nMaxPos">Specifies the maximum scrolling position.
		/// </param>
		/// <param name="bRedraw">Specifies whether the scroll bar should be 
		/// redrawn to reflect the change. If this parameter is TRUE, the scroll 
		/// bar is redrawn. If it is FALSE, the scroll bar is not redrawn.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SetScrollRange 
			( 
				IntPtr hWnd,
				ScrollBarFlags nBar,
				int nMinPos,
				int nMaxPos,
				bool bRedraw
			);

		/// <summary>
		/// The SetWindowLong function changes an attribute of the specified 
		/// window. The function also sets the 32-bit (long) value at the 
		/// specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window and, indirectly, 
		/// the class to which the window belongs.</para>
		/// <para>Windows 95/98/Me: The SetWindowLong function may fail if 
		/// the window specified by the hWnd parameter does not belong to the 
		/// same process as the calling thread.</para></param>
		/// <param name="nIndex"></param>
		/// <param name="dwNewLong"></param>
		/// <returns>If the function succeeds, the return value is the previous 
		/// value of the specified 32-bit integer. If the function fails, 
		/// the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int SetWindowLong 
			( 
				IntPtr hWnd,
				int nIndex,
				int dwNewLong
			);

		/// <summary>
		/// The SetWindowLong function changes an attribute of the specified 
		/// window. The function also sets the 32-bit (long) value at the 
		/// specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd"><para>Handle to the window and, indirectly, 
		/// the class to which the window belongs.</para>
		/// <para>Windows 95/98/Me: The SetWindowLong function may fail if 
		/// the window specified by the hWnd parameter does not belong to the 
		/// same process as the calling thread.</para></param>
		/// <param name="nIndex"></param>
		/// <param name="dwNewLong"></param>
		/// <returns>If the function succeeds, the return value is the previous 
		/// value of the specified 32-bit integer. If the function fails, 
		/// the return value is zero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int SetWindowLong
			(
				IntPtr hWnd,
				WindowLongFlags nIndex,
				int dwNewLong
			);

		/// <summary>
		/// The SetWindowPos function changes the size, position, 
		/// and Z order of a child, pop-up, or top-level window. 
		/// Child, pop-up, and top-level windows are ordered according 
		/// to their appearance on the screen. The topmost window 
		/// receives the highest rank and is the first window in the 
		/// Z order.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="hWndInsertAfter"><para>Handle to the window that 
		/// precedes the positioned window in the Z order. This parameter 
		/// must be a window handle or one of the following values:</para>
		/// <list type="bullet">
		/// <item>HWND_BOTTOM</item>
		/// <item>HWND_NOTOPMOST</item>
		/// <item>HWND_TOP</item>
		/// <item>HWND_TOPMOST</item>
		/// </list>
		/// <para>This parameter is ignored if the SWP_NOZORDER flag is 
		/// set in the uFlags parameter.</para>
		/// </param>
		/// <param name="X">Specifies the new position of the left side 
		/// of the window, in client coordinates.</param>
		/// <param name="Y">Specifies the new position of the top of the 
		/// window, in client coordinates.</param>
		/// <param name="cx">Specifies the new width of the window, 
		/// in pixels.</param>
		/// <param name="cy">Specifies the new height of the window, 
		/// in pixels.</param>
		/// <param name="uFlags"></param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SetWindowPos
			(
				IntPtr hWnd,
				IntPtr hWndInsertAfter,
				int X,
				int Y,
				int cx,
				int cy,
				SetWindowPosFlags uFlags
			);

		/// <summary>
		/// The SetWindowRgn function sets the window region of a window. 
		/// The window region determines the area within the window where 
		/// the system permits drawing. The system does not display any 
		/// portion of a window that lies outside of the window region.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window region 
		/// is to be set.</param>
		/// <param name="hRgn"> Handle to a region. The function sets the 
		/// window region of the window to this region.
		/// If hRgn is NULL, the function sets the window region to NULL.
		/// </param>
		/// <param name="bRedraw">Specifies whether the system redraws 
		/// the window after setting the window region. If bRedraw is TRUE, 
		/// the system does so; otherwise, it does not.
		/// Typically, you set bRedraw to TRUE if the window is visible.
		/// </param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern RegionComplexity SetWindowRgn 
			(
				IntPtr hWnd,
				IntPtr hRgn,
				bool bRedraw
			);

		/// <summary>
		/// The SetWindowText function changes the text of the 
		/// specified window's title bar (if it has one). If the 
		/// specified window is a control, the text of the control is 
		/// changed. However, SetWindowText cannot change the text of a 
		/// control in another application.
		/// </summary>
		/// <param name="hWnd">Handle to the window or control whose text 
		/// is to be changed.</param>
		/// <param name="lpString">Pointer to a null-terminated string to 
		/// be used as the new title or control text.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		/// <remarks>
		/// <para>If the target window is owned by the current process, 
		/// SetWindowText causes a WM_SETTEXT message to be sent to the 
		/// specified window or control. If the control is a list box 
		/// control created with the WS_CAPTION style, however, 
		/// SetWindowText sets the text for the control, not for the 
		/// list box entries.</para>
		/// <para>To set the text of a control in another process, 
		/// send the WM_SETTEXT message directly instead of calling 
		/// SetWindowText.</para>
		/// <para>The SetWindowText function does not expand tab 
		/// characters (ASCII code 0x09). Tab characters are displayed 
		/// as vertical bar (|) characters.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SetWindowText
			(
				IntPtr hWnd,
				string lpString
			);

		/// <summary>
		/// The SetWindowsHookEx function installs an application-defined 
		/// hook procedure into a hook chain. You would install a hook 
		/// procedure to monitor the system for certain types of events. 
		/// These events are associated either with a specific thread 
		/// or with all threads in the same desktop as the calling thread. 
		/// </summary>
		/// <param name="idHook">Specifies the type of hook procedure to 
		/// be installed.</param>
		/// <param name="lpfn"> Pointer to the hook procedure. If the 
		/// dwThreadId parameter is zero or specifies the identifier of 
		/// a thread created by a different process, the lpfn parameter 
		/// must point to a hook procedure in a dynamic-link library (DLL). 
		/// Otherwise, lpfn can point to a hook procedure in the code 
		/// associated with the current process.</param>
		/// <param name="hMod">Handle to the DLL containing the hook 
		/// procedure pointed to by the lpfn parameter. The hMod parameter 
		/// must be set to NULL if the dwThreadId parameter specifies a 
		/// thread created by the current process and if the hook procedure 
		/// is within the code associated with the current process.</param>
		/// <param name="dwThreadId">Specifies the identifier of the thread 
		/// with which the hook procedure is to be associated. If this 
		/// parameter is zero, the hook procedure is associated with all 
		/// existing threads running in the same desktop as the calling 
		/// thread.</param>
		/// <returns>If the function succeeds, the return value is the 
		/// handle to the hook procedure. If the function fails, the return 
		/// value is NULL.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern IntPtr SetWindowsHookEx 
			( 
				HookKind idHook,
				HookProcedure lpfn,
				IntPtr hMod,
				int dwThreadId
			);

		/// <summary>
		/// Shows or hides the specified scroll bar.
		/// </summary>
		/// <param name="hWnd">Handle to a scroll bar control or a window with 
		/// a standard scroll bar, depending on the value of the wBar parameter.
		/// </param>
		/// <param name="wBar">Specifies the scroll bar(s) to be shown or hidden.
		/// </param>
		/// <param name="bShow">Specifies whether the scroll bar is shown or 
		/// hidden. If this parameter is TRUE, the scroll bar is shown; 
		/// otherwise, it is hidden.</param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ShowScrollBar 
			( 
				IntPtr hWnd,
				ScrollBarFlags wBar,
				bool bShow
			);

		/// <summary>
		/// Sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="nCmdShow">Specifies how the window is to be 
		/// shown. This parameter is ignored the first time an 
		/// application calls ShowWindow, if the program that launched 
		/// the application provides a STARTUPINFO structure. Otherwise, 
		/// the first time ShowWindow is called, the value should be the 
		/// value obtained by the WinMain function in its nCmdShow 
		/// parameter.</param>
		/// <returns>If the window was previously visible, the return 
		/// value is nonzero. If the window was previously hidden, the 
		/// return value is zero.</returns>
		/// <remarks>To perform certain special effects when showing or 
		/// hiding a window, use AnimateWindow.</remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ShowWindow
			(
				IntPtr hWnd,
				ShowWindowFlags nCmdShow
			);

		/// <summary>
		/// Sets the show state of a window created by a different 
		/// thread.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="nCmdShow">Specifies how the window is to be 
		/// shown. This parameter is ignored the first time an 
		/// application calls ShowWindow, if the program that launched 
		/// the application provides a STARTUPINFO structure. Otherwise, 
		/// the first time ShowWindow is called, the value should be the 
		/// value obtained by the WinMain function in its nCmdShow 
		/// parameter.</param>
		/// <returns>If the window was previously visible, the return 
		/// value is nonzero. If the window was previously hidden, the 
		/// return value is zero.</returns>
		/// <remarks><para>To perform certain special effects when 
		/// showing or hiding a window, use AnimateWindow.</para>
		/// <para>This function posts a show-window event to the 
		/// message queue of the given window. An application can 
		/// use this function to avoid becoming hung while waiting 
		/// for a hung application to finish processing a show-window 
		/// event.</para></remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ShowWindowAsync
			(
				IntPtr hWnd,
				ShowWindowFlags nCmdShow
			);

		/// <summary>
		/// The SwitchToThisWindow function is called to switch focus 
		/// to a specified window and bring it to the foreground.
		/// </summary>
		/// <param name="hWnd">Handle to the window being switched to.
		/// </param>
		/// <param name="fAltTab">A TRUE for this parameter indicates 
		/// that the window is being switched to using the Alt/Ctl+Tab 
		/// key sequence. This parameter should be FALSE otherwise.
		/// </param>
		/// <remarks><para>This function is typically called to 
		/// maintain window z-ordering.</para>
		/// <para>Although you can access this function by using 
		/// LoadLibrary and GetProcAddress combined in Microsoft® 
		/// Windows® versions prior to Windows XP, the function is not 
		/// accessible using the standard Include file and library 
		/// linkage. The header files included in Windows XP Service 
		/// Pack 1 (SP1) and Windows Server 2003 document this function 
		/// and make it accessible using the appropriate Include file 
		/// and library linkage. However, this function is deprecated 
		/// and not intended for general use. It is recommended that 
		/// you do not use it in new programs because it might be 
		/// altered or unavailable in subsequent versions of Windows.
		/// </para>
		/// <para>Included in: Windows 2000/XP/2003.</para>
		/// </remarks>
		[DllImport ( DllName, SetLastError = true )]
		public static extern void SwitchToThisWindow
			(
				IntPtr hWnd,
				bool fAltTab
			);

		/// <summary>
		/// The SystemParametersInfo function retrieves or sets the value 
		/// of one of the system-wide parameters. This function can also 
		/// update the user profile while setting a parameter.
		/// </summary>
		/// <param name="uiAction">System-wide parameter to be retrieved 
		/// or set.</param>
		/// <param name="uiParam">Depends on the system parameter being 
		/// queried or set.</param>
		/// <param name="pvParam">Depends on the system parameter being 
		/// queried or set.</param>
		/// <param name="fWinIni">If a system parameter is being set, 
		/// specifies whether the user profile is to be updated, and if so, 
		/// whether the WM_SETTINGCHANGE message is to be broadcast to all 
		/// top-level windows to notify them of the change.</param>
		/// <returns>If the function succeeds, the return value is a nonzero 
		/// value.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool SystemParametersInfo 
			(
				int uiAction,
				int uiParam,
				IntPtr pvParam,
				int fWinIni
			);

		/// <summary>
		/// The ToAscii function translates the specified virtual-key code 
		/// and keyboard state to the corresponding character or characters. 
		/// The function translates the code using the input language and 
		/// physical keyboard layout identified by the keyboard layout 
		/// handle.
		/// </summary>
		/// <param name="uVirtKey">Specifies the virtual-key code to be 
		/// translated.</param>
		/// <param name="uScanCode">Specifies the hardware scan code of the 
		/// key to be translated. The high-order bit of this value is set if 
		/// the key is up (not pressed).</param>
		/// <param name="lpKeyState">Pointer to a 256-byte array that contains 
		/// the current keyboard state. Each element (byte) in the array 
		/// contains the state of one key. If the high-order bit of a byte 
		/// is set, the key is down (pressed). The low bit, if set, indicates 
		/// that the key is toggled on. In this function, only the toggle bit 
		/// of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK 
		/// and SCROLL LOCK keys is ignored.</param>
		/// <param name="lpChar">Pointer to the buffer that receives the 
		/// translated character or characters.</param>
		/// <param name="uFlags">Specifies whether a menu is active. 
		/// This parameter must be 1 if a menu is active, or 0 otherwise.
		/// </param>
		/// <returns>If the specified key is a dead key, the return value 
		/// is negative.</returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int ToAscii 
			( 
				int uVirtKey,
				int uScanCode,
				byte [] lpKeyState,
				ref short lpChar,
				int uFlags
			);

		/// <summary>
		/// The ToUnicode function translates the specified virtual-key code 
		/// and keyboard state to the correspondingUnicodecharacter or 
		/// characters.
		/// </summary>
		/// <param name="wVirtKey">Specifies the virtual-key code to be 
		/// translated.</param>
		/// <param name="wScanCode">Specifies the hardware scan code of the 
		/// key to be translated. The high-order bit of this value is set 
		/// if the key is up.</param>
		/// <param name="lpKeyState">Pointer to a 256-byte array that contains 
		/// the current keyboard state. Each element (byte) in the array 
		/// contains the state of one key. If the high-order bit of a byte 
		/// is set, the key is down.</param>
		/// <param name="pwszBuff">Pointer to the buffer that receives the 
		/// translatedUnicodecharacter or characters. However, this buffer 
		/// may be returned without being NULL-terminated even though the 
		/// variable name suggests that it is NULL-terminated.</param>
		/// <param name="cchBuff">Specifies the size, in wide characters, 
		/// of the buffer pointed to by the pwszBuff parameter.</param>
		/// <param name="wFlags">Specifies the behavior of the function. 
		/// If bit 0 is set, a menu is active. Bits 1 through 31 are 
		/// reserved.</param>
		/// <returns></returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern int ToUnicode
			(          
				int wVirtKey,
				int wScanCode,
				byte [] lpKeyState,
				short [] pwszBuff,
				int cchBuff,
				int wFlags
			);

		/// <summary>
		/// The TrackMouseEvent function posts messages when the mouse 
		/// pointer leaves a window or hovers over a window for a 
		/// specified amount of time.
		/// </summary>
		/// <param name="lpEventTrack">Pointer to a TRACKMOUSEEVENT 
		/// structure that contains tracking information.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool TrackMouseEvent
			(
				ref TRACKMOUSEEVENT lpEventTrack
			);

		/// <summary>
		/// The TranslateMessage function translates virtual-key messages 
		/// into character messages. The character messages are posted to the 
		/// calling thread's message queue, to be read the next time the thread 
		/// calls the GetMessage or PeekMessage function.
		/// </summary>
		/// <param name="lpMsg">Pointer to an MSG structure that contains message 
		/// information retrieved from the calling thread's message queue by using 
		/// the GetMessage or PeekMessage function.</param>
		/// <returns><para>If the message is translated (that is, a character 
		/// message is posted to the thread's message queue), the return value 
		/// is nonzero.</para>
		/// <para>If the message is WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or 
		/// WM_SYSKEYUP, the return value is nonzero, regardless of the 
		/// translation.</para>
		/// <para>If the message is not translated (that is, a character message 
		/// is not posted to the thread's message queue), the return value is 
		/// zero.</para></returns>
		[DllImport ( DllName, SetLastError = false )]
		public static extern bool TranslateMessage
			(          
				ref MSG lpMsg
			);

		/// <summary>
		/// Removes a hook procedure installed in a hook chain by the 
		/// SetWindowsHookEx function.
		/// </summary>
		/// <param name="hhk">Handle to the hook to be removed. 
		/// This parameter is a hook handle obtained by a previous 
		/// call to SetWindowsHookEx.</param>
		/// <returns>If the function succeeds, the return value 
		/// is nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool UnhookWindowsHookEx 
			( 
				IntPtr hhk
			);

		/// <summary>
		/// Frees a hot key previously registered by the calling thread.
		/// </summary>
		/// <param name="hWnd">Handle to the window associated with the 
		/// hot key to be freed. This parameter should be NULL if the 
		/// hot key is not associated with a window.</param>
		/// <param name="id">Specifies the identifier of the hot key 
		/// to be freed.</param>
		/// <returns>If the function succeeds, the return value is 
		/// nonzero.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool UnregisterHotKey 
			( 
				IntPtr hWnd,
				int id
			);

		/// <summary>
		/// The UpdateWindow function updates the client area of the 
		/// specified window by sending a WM_PAINT message to the window 
		/// if the window's update region is not empty. The function 
		/// sends a WM_PAINT message directly to the window procedure 
		/// of the specified window, bypassing the application queue. 
		/// If the update region is empty, no message is sent. 
		/// </summary>
		/// <param name="hWnd">Handle to the window to be updated.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool UpdateWindow 
			(
				IntPtr hWnd
			);

		/// <summary>
		/// Validates the client area within a rectangle by removing the 
		/// rectangle from the update region of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region 
		/// is to be modified. If this parameter is NULL, the system 
		/// invalidates and redraws all windows and sends the WM_ERASEBKGND 
		/// and WM_NCPAINT messages to the window procedure before the 
		/// function returns.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that contains 
		/// the client coordinates of the rectangle to be removed from the 
		/// update region. If this parameter is NULL, the entire client area 
		/// is removed.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ValidateRect
			(
				IntPtr hWnd,
				ref Rectangle lpRect
			);

		/// <summary>
		/// Validates the client area within a rectangle by removing the 
		/// rectangle from the update region of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region 
		/// is to be modified. If this parameter is NULL, the system 
		/// invalidates and redraws all windows and sends the WM_ERASEBKGND 
		/// and WM_NCPAINT messages to the window procedure before the 
		/// function returns.</param>
		/// <param name="lpRect">Pointer to a Rectangle structure that contains 
		/// the client coordinates of the rectangle to be removed from the 
		/// update region. If this parameter is NULL, the entire client area 
		/// is removed.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ValidateRect
			(
				IntPtr hWnd,
				IntPtr lpRect
			);

		/// <summary>
		/// Validates the client area within a region by removing the region 
		/// from the current update region of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose update region 
		/// is to be modified.</param>
		/// <param name="hRgn">Handle to a region that defines the area 
		/// to be removed from the update region. If this parameter is NULL, 
		/// the entire client area is removed.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool ValidateRgn 
			(
				IntPtr hWnd,
				IntPtr hRgn
			);

		/// <summary>
		/// Waits until the specified process is waiting for user 
		/// input with no input pending, or until the time-out interval 
		/// has elapsed.
		/// </summary>
		/// <param name="hProcess">Handle to the process. If this process 
		/// is a console application or does not have a message queue, 
		/// WaitForInputIdle returns immediately.</param>
		/// <param name="milliSeconds">Time-out interval, in milliseconds. 
		/// If dwMilliseconds is INFINITE, the function does not return 
		/// until the process is idle.</param>
		/// <returns>The following table shows the possible return values:
		/// 0, WAIT_TIMEOUT, WAIT_FAILED.</returns>
		[DllImport ( DllName, SetLastError = true )]
        [CLSCompliant ( false )]
		public static extern uint WaitForInputIdle
			(
				IntPtr hProcess,
				uint milliSeconds
			);

		/// <summary>
		/// The WaitMessage function yields control to other threads when 
		/// a thread has no other messages in its message queue. The WaitMessage 
		/// function suspends the thread and does not return until a new message 
		/// is placed in the thread's message queue.
		/// </summary>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern bool WaitMessage ();

		/// <summary>
		/// Returns a handle to the window associated with the specified 
		/// display device context (DC).
		/// </summary>
		/// <param name="hDC">Handle to the device context from which a 
		/// handle for the associated window is to be retrieved.</param>
		/// <returns>The return value is a handle to the window associated 
		/// with the specified DC. If no window is associated with the 
		/// specified DC, the return value is NULL.</returns>
		[DllImport ( DllName )]
		public static extern IntPtr WindowFromDC
			(
				IntPtr hDC
			);

		/// <summary>
		/// Retrieves a handle to the window that contains the specified 
		/// point.
		/// </summary>
		/// <param name="Point">Specifies a <see cref="Point">POINT 
		/// structure</see> that defines the point to be checked.</param>
		/// <returns>The return value is a handle to the window that 
		/// contains the point. If no window exists at the given point, 
		/// the return value is NULL. If the point is over a static text 
		/// control, the return value is a handle to the window under the 
		/// static text control.</returns>
		[DllImport ( DllName )]
		public static extern IntPtr WindowFromPoint
			(
				Point Point
			);
	}
}
