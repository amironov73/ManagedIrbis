// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WindowMessage.cs -- window message mnemonics
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable All

namespace AM.Win32
{
    /// <summary>
    /// Window message mnemonics.
    /// </summary>
    [PublicAPI]
    public enum WindowMessage
    {
        #region General messages

        /// <summary>
        /// The WM_NULL message performs no operation. An application
        /// sends the WM_NULL message if it wants to post a message
        /// that the recipient window will ignore.
        /// </summary>
        WM_NULL = 0x0000,

        /// <summary>
        /// The WM_CREATE message is sent when an application requests
        /// that a window be created by calling the CreateWindowEx or
        /// CreateWindow function. (The message is sent before the
        /// function returns.) The window procedure of the new window
        /// receives this message after the window is created, but
        /// before the window becomes visible.
        /// </summary>
        WM_CREATE = 0x0001,

        /// <summary>
        /// <para>The WM_DESTROY message is sent when a window is being
        /// destroyed. It is sent to the window procedure of the window
        /// being destroyed after the window is removed from the screen.
        /// </para>
        /// <para>This message is sent first to the window being
        /// destroyed and then to the child windows (if any) as they
        /// are destroyed. During the processing of the message, it
        /// can be assumed that all child windows still exist.</para>
        /// </summary>
        WM_DESTROY = 0x0002,

        /// <summary>
        /// The WM_MOVE message is sent after a window has been moved.
        /// </summary>
        WM_MOVE = 0x0003,

        /// <summary>
        /// The WM_SIZE message is sent to a window after its size
        /// has changed.
        /// </summary>
        WM_SIZE = 0x0005,

        /// <summary>
        /// The WM_ACTIVATE message is sent to both the window being
        /// activated and the window being deactivated. If the windows
        /// use the same input queue, the message is sent synchronously,
        /// first to the window procedure of the top-level window being
        /// deactivated, then to the window procedure of the top-level
        /// window being activated. If the windows use different input
        /// queues, the message is sent asynchronously, so the window is
        /// activated immediately.
        /// </summary>
        WM_ACTIVATE = 0x0006,

        /// <summary>
        /// The WM_SETFOCUS message is sent to a window after it has
        /// gained the keyboard focus.
        /// </summary>
        WM_SETFOCUS = 0x0007,

        /// <summary>
        /// The WM_KILLFOCUS message is sent to a window immediately
        /// before it loses the keyboard focus.
        /// </summary>
        WM_KILLFOCUS = 0x0008,

        /// <summary>
        /// The WM_ENABLE message is sent when an application changes
        /// the enabled state of a window. It is sent to the window
        /// whose enabled state is changing. This message is sent
        /// before the EnableWindow function returns, but after the
        /// enabled state (WS_DISABLED style bit) of the window has
        /// changed.
        /// </summary>
        WM_ENABLE = 0x000A,

        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window
        /// to allow changes in that window to be redrawn or to prevent
        /// changes in that window from being redrawn.
        /// </summary>
        WM_SETREDRAW = 0x000B,

        /// <summary>
        /// An application sends a WM_SETTEXT message to set the text
        /// of a window.
        /// </summary>
        WM_SETTEXT = 0x000C,

        /// <summary>
        /// An application sends a WM_GETTEXT message to copy the text
        /// that corresponds to a window into a buffer provided by the
        /// caller.
        /// </summary>
        WM_GETTEXT = 0x000D,

        /// <summary>
        /// An application sends a WM_GETTEXTLENGTH message to determine
        /// the length, in characters, of the text associated with a
        /// window.
        /// </summary>
        WM_GETTEXTLENGTH = 0x000E,

        /// <summary>
        /// The WM_PAINT message is sent when the system or another
        /// application makes a request to paint a portion of an
        /// application's window. The message is sent when the
        /// UpdateWindow or RedrawWindow function is called, or by
        /// the DispatchMessage function when the application obtains
        /// a WM_PAINT message by using the GetMessage or PeekMessage
        /// function.
        /// </summary>
        WM_PAINT = 0x000F,

        /// <summary>
        /// The WM_CLOSE message is sent as a signal that a window
        /// or an application should terminate.
        /// </summary>
        WM_CLOSE = 0x0010,

        /// <summary>
        /// <para>The WM_QUERYENDSESSION message is sent when the user
        /// chooses to end the session or when an application calls
        /// the ExitWindows function. If any application returns zero,
        /// the session is not ended. The system stops sending
        /// WM_QUERYENDSESSION messages as soon as one application
        /// returns zero.</para>
        /// <para>After processing this message, the system sends
        /// the WM_ENDSESSION message with the wParam parameter set
        /// to the results of the WM_QUERYENDSESSION message.</para>
        /// </summary>
        WM_QUERYENDSESSION = 0x0011,

        /// <summary>
        /// The WM_QUERYOPEN message is sent to an icon when the user
        /// requests that the window be restored to its previous size
        /// and position.
        /// </summary>
        WM_QUERYOPEN = 0x0013,

        /// <summary>
        /// The WM_ENDSESSION message is sent to an application after
        /// the system processes the results of the WM_QUERYENDSESSION
        /// message. The WM_ENDSESSION message informs the application
        /// whether the session is ending.
        /// </summary>
        WM_ENDSESSION = 0x0016,

        /// <summary>
        /// The WM_QUIT message indicates a request to terminate an
        /// application and is generated when the application calls the
        /// PostQuitMessage function. It causes the GetMessage function
        /// to return zero.
        /// </summary>
        WM_QUIT = 0x0012,

        /// <summary>
        /// The WM_ERASEBKGND message is sent when the window background
        /// must be erased (for example, when a window is resized).
        /// The message is sent to prepare an invalidated portion
        /// of a window for painting.
        /// </summary>
        WM_ERASEBKGND = 0x0014,

        /// <summary>
        /// The WM_SYSCOLORCHANGE message is sent to all top-level
        /// windows when a change is made to a system color setting.
        /// </summary>
        WM_SYSCOLORCHANGE = 0x0015,

        /// <summary>
        /// The WM_SHOWWINDOW message is sent to a window when the
        /// window is about to be hidden or shown.
        /// </summary>
        WM_SHOWWINDOW = 0x0018,

        /// <summary>
        /// <para>An application sends the WM_WININICHANGE message
        /// to all top-level windows after making a change to the
        /// WIN.INI file. The SystemParametersInfo function sends
        /// this message after an application uses the function to
        /// change a setting in WIN.INI.</para>
        /// <para>Note: The WM_WININICHANGE message is provided
        /// only for compatibility with earlier versions of the
        /// system. Applications should use the WM_SETTINGCHANGE
        /// message.</para>
        /// </summary>
        WM_WININICHANGE = 0x001A,

        /// <summary>
        /// <para>The system sends the WM_SETTINGCHANGE message to
        /// all top-level windows when the SystemParametersInfo function
        /// changes a system-wide setting or when policy settings have
        /// changed.</para>
        /// <para>Applications should send WM_SETTINGCHANGE to all
        /// top-level windows when they make changes to system
        /// parameters. (This message cannot be sent directly to a
        /// window.) To send the WM_SETTINGCHANGE message to all
        /// top-level windows, use the SendMessageTimeout function
        /// with the hwnd parameter set to HWND_BROADCAST.</para>
        /// </summary>
        WM_SETTINGCHANGE = 0x001A,

        /// <summary>
        /// The WM_DEVMODECHANGE message is sent to all top-level
        /// windows whenever the user changes device-mode settings.
        /// </summary>
        WM_DEVMODECHANGE = 0x001B,

        /// <summary>
        /// The WM_ACTIVATEAPP message is sent when a window belonging
        /// to a different application than the active window is about
        /// to be activated. The message is sent to the application
        /// whose window is being activated and to the application
        /// whose window is being deactivated.
        /// </summary>
        WM_ACTIVATEAPP = 0x001C,

        /// <summary>
        /// An application sends the WM_FONTCHANGE message to all
        /// top-level windows in the system after changing the pool
        /// of font resources.
        /// </summary>
        WM_FONTCHANGE = 0x001D,

        /// <summary>
        /// An application sends the WM_TIMECHANGE message whenever
        /// it updates the system time.
        /// </summary>
        WM_TIMECHANGE = 0x001E,

        /// <summary>
        /// The WM_CANCELMODE message is sent to cancel certain modes,
        /// such as mouse capture. For example, the system sends this
        /// message to the active window when a dialog box or message
        /// box is displayed. Certain functions also send this message
        /// explicitly to the specified window regardless of whether
        /// it is the active window. For example, the EnableWindow
        /// function sends this message when disabling the specified
        /// window.
        /// </summary>
        WM_CANCELMODE = 0x001F,

        /// <summary>
        /// The WM_SETCURSOR message is sent to a window if the mouse
        /// causes the cursor to move within a window and mouse input
        /// is not captured.
        /// </summary>
        WM_SETCURSOR = 0x0020,

        /// <summary>
        /// The WM_MOUSEACTIVATE message is sent when the cursor is in
        /// an inactive window and the user presses a mouse button.
        /// The parent window receives this message only if the child
        /// window passes it to the DefWindowProc function.
        /// </summary>
        WM_MOUSEACTIVATE = 0x0021,

        /// <summary>
        /// The WM_CHILDACTIVATE message is sent to a child window when
        /// the user clicks the window's title bar or when the window is
        /// activated, moved, or sized.
        /// </summary>
        WM_CHILDACTIVATE = 0x0022,

        /// <summary>
        /// The WM_QUEUESYNC message is sent by a computer-based training
        /// (CBT) application to separate user-input messages from other
        /// messages sent through the WH_JOURNALPLAYBACK Hook procedure.
        /// </summary>
        WM_QUEUESYNC = 0x0023,

        /// <summary>
        /// The WM_GETMINMAXINFO message is sent to a window when the
        /// size or position of the window is about to change. An
        /// application can use this message to override the window's
        /// default maximized size and position, or its default minimum
        /// or maximum tracking size.
        /// </summary>
        WM_GETMINMAXINFO = 0x0024,

        /// <summary>
        /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent
        /// to a minimized window when the icon is to be painted. This
        /// message is not sent by newer versions of Microsoft® Windows®,
        /// except in unusual circumstances.
        /// </summary>
        WM_PAINTICON = 0x0026,

        /// <summary>
        /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message
        /// is sent to a minimized window when the background of the
        /// icon must be filled before painting the icon. A window
        /// receives this message only if a class icon is defined for
        /// the window; otherwise, WM_ERASEBKGND is sent. This message
        /// is not sent by newer versions of Windows.
        /// </summary>
        WM_ICONERASEBKGND = 0x0027,

        /// <summary>
        /// The WM_NEXTDLGCTL message is sent to a dialog box procedure
        /// to set the keyboard focus to a different control in the
        /// dialog box.
        /// </summary>
        WM_NEXTDLGCTL = 0x0028,

        /// <summary>
        /// The WM_SPOOLERSTATUS message is sent from Print Manager
        /// whenever a job is added to or removed from the Print Manager
        /// queue.
        /// </summary>
        WM_SPOOLERSTATUS = 0x002A,

        /// <summary>
        /// The WM_DRAWITEM message is sent to the owner window of an
        /// owner-drawn button, combo box, list box, or menu when a
        /// visual aspect of the button, combo box, list box, or menu
        /// has changed.
        /// </summary>
        WM_DRAWITEM = 0x002B,

        /// <summary>
        /// The WM_MEASUREITEM message is sent to the owner window of
        /// a combo box, list box, list view control, or menu item when
        /// the control or menu is created.
        /// </summary>
        WM_MEASUREITEM = 0x002C,

        /// <summary>
        /// The WM_DELETEITEM message is sent to the owner of a list
        /// box or combo box when the list box or combo box is destroyed
        /// or when items are removed by the LB_DELETESTRING,
        /// LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message.
        /// The system sends a WM_DELETEITEM message for each deleted
        /// item. The system sends the WM_DELETEITEM message for any
        /// deleted list box or combo box item with nonzero item data.
        /// </summary>
        WM_DELETEITEM = 0x002D,

        /// <summary>
        /// The WM_VKEYTOITEM message is sent by a list box with the
        /// LBS_WANTKEYBOARDINPUT style to its owner in response to
        /// a WM_KEYDOWN message.
        /// </summary>
        WM_VKEYTOITEM = 0x002E,

        /// <summary>
        /// The WM_CHARTOITEM message is sent by a list box with the
        /// LBS_WANTKEYBOARDINPUT style to its owner in response to
        /// a WM_CHAR message.
        /// </summary>
        WM_CHARTOITEM = 0x002F,

        /// <summary>
        /// An application sends a WM_SETFONT message to specify the
        /// font that a control is to use when drawing text.
        /// </summary>
        WM_SETFONT = 0x0030,

        /// <summary>
        /// An application sends a WM_GETFONT message to a control
        /// to retrieve the font with which the control is currently
        /// drawing its text.
        /// </summary>
        WM_GETFONT = 0x0031,

        /// <summary>
        /// An application sends a WM_SETHOTKEY message to a window to
        /// associate a hot key with the window. When the user presses
        /// the hot key, the system activates the window.
        /// </summary>
        WM_SETHOTKEY = 0x0032,

        /// <summary>
        /// An application sends a WM_GETHOTKEY message to determine
        /// the hot key associated with a window.
        /// </summary>
        WM_GETHOTKEY = 0x0033,

        /// <summary>
        /// The WM_QUERYDRAGICON message is sent to a minimized (iconic)
        /// window. The window is about to be dragged by the user but
        /// does not have an icon defined for its class. An application
        /// can return a handle to an icon or cursor. The system displays
        /// this cursor or icon while the user drags the icon.
        /// </summary>
        WM_QUERYDRAGICON = 0x0037,

        /// <summary>
        /// The system sends the WM_COMPAREITEM message to determine the
        /// relative position of a new item in the sorted list of an
        /// owner-drawn combo box or list box. Whenever the application
        /// adds a new item, the system sends this message to the owner
        /// of a combo box or list box created with the CBS_SORT or
        /// LBS_SORT style.
        /// </summary>
        WM_COMPAREITEM = 0x0039,

        /// <summary>
        /// <para>Active Accessibility sends the WM_GETOBJECT message
        /// to obtain information about an accessible object contained
        /// in a server application.</para>
        /// <para>Applications never send this message directly. It is
        /// sent only by Active Accessibility in response to calls
        /// to AccessibleObjectFromPoint, AccessibleObjectFromEvent,
        /// or AccessibleObjectFromWindow. However, server applications
        /// handle this message.</para>
        /// </summary>
        WM_GETOBJECT = 0x003D,

        /// <summary>
        /// The WM_COMPACTING message is sent to all top-level windows
        /// when the system detects more than 12.5 percent of system
        /// time over a 30- to 60-second interval is being spent
        /// compacting memory. This indicates that system memory is low.
        /// </summary>
        WM_COMPACTING = 0x0041,

        /// <summary>
        /// ???
        /// </summary>
        WM_COMMNOTIFY = 0x0044,

        /// <summary>
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose
        /// size, position, or place in the Z order is about to change
        /// as a result of a call to the SetWindowPos function or
        /// another window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x0046,

        /// <summary>
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose
        /// size, position, or place in the Z order has changed as a
        /// result of a call to the SetWindowPos function or another
        /// window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x0047,

        /// <summary>
        /// <para>The WM_POWER message is broadcast when the system,
        /// typically a battery-powered personal computer, is about
        /// to enter suspended mode.</para>
        /// <para>Note  The WM_POWER message is obsolete. It is
        /// provided only for compatibility with 16-bit Windows-based
        /// applications. Applications should use the WM_POWERBROADCAST
        /// message.</para>
        /// </summary>
        WM_POWER = 0x0048,

        /// <summary>
        /// An application sends the WM_COPYDATA message to pass data
        /// to another application.
        /// </summary>
        WM_COPYDATA = 0x004A,

        /// <summary>
        /// The WM_CANCELJOURNAL message is posted to an application
        /// when a user cancels the application's journaling activities.
        /// The message is posted with a NULL window handle.
        /// </summary>
        WM_CANCELJOURNAL = 0x004B,

        /// <summary>
        /// The WM_NOTIFY message is sent by a common control to its
        /// parent window when an event has occurred or the control
        /// requires some information.
        /// </summary>
        WM_NOTIFY = 0x004E,

        /// <summary>
        /// The WM_INPUTLANGCHANGEREQUEST message is posted to the
        /// window with the focus when the user chooses a new input
        /// language, either with the hotkey (specified in the Keyboard
        /// control panel application) or from the indicator on the
        /// system taskbar. An application can accept the change by
        /// passing the message to the DefWindowProc function or
        /// reject the change (and prevent it from taking place) by
        /// returning immediately.
        /// </summary>
        WM_INPUTLANGCHANGEREQUEST = 0x0050,

        /// <summary>
        /// The WM_INPUTLANGCHANGE message is sent to the topmost
        /// affected window after an application's input language has
        /// been changed. You should make any application-specific
        /// settings and pass the message to the DefWindowProc function,
        /// which passes the message to all first-level child windows.
        /// These child windows can pass the message to DefWindowProc
        /// to have it pass the message to their child windows,
        /// and so on.
        /// </summary>
        WM_INPUTLANGCHANGE = 0x0051,

        /// <summary>
        /// Sent to an application that has initiated a training card
        /// with Microsoft® Windows® Help. The message informs the
        /// application when the user clicks an authorable button.
        /// An application initiates a training card by specifying
        /// the HELP_TCARD command in a call to the WinHelp function.
        /// </summary>
        WM_TCARD = 0x0052,

        /// <summary>
        /// Indicates that the user pressed the F1 key. If a menu is
        /// active when F1 is pressed, WM_HELP is sent to the window
        /// associated with the menu; otherwise, WM_HELP is sent to
        /// the window that has the keyboard focus. If no window has
        /// the keyboard focus, WM_HELP is sent to the currently active
        /// window.
        /// </summary>
        WM_HELP = 0x0053,

        /// <summary>
        /// The WM_USERCHANGED message is sent to all windows after
        /// the user has logged on or off. When the user logs on or off,
        /// the system updates the user-specific settings. The system
        /// sends this message immediately after updating the settings.
        /// </summary>
        WM_USERCHANGED = 0x0054,

        /// <summary>
        /// Used to determine if a window accepts ANSI or Unicode
        /// structures in the WM_NOTIFY notification message.
        /// WM_NOTIFYFORMAT messages are sent from a common control
        /// to its parent window and from the parent window to the
        /// common control.
        /// </summary>
        WM_NOTIFYFORMAT = 0x0055,

        /// <summary>
        /// The WM_CONTEXTMENU message notifies a window that the
        /// user clicked the right mouse button (right clicked)
        /// in the window.
        /// </summary>
        WM_CONTEXTMENU = 0x007B,

        /// <summary>
        /// The WM_STYLECHANGING message is sent to a window when
        /// the SetWindowLong function is about to change one or more
        /// of the window's styles.
        /// </summary>
        WM_STYLECHANGING = 0x007C,

        /// <summary>
        /// The WM_STYLECHANGED message is sent to a window after the
        /// SetWindowLong function has changed one or more of the
        /// window's styles.
        /// </summary>
        WM_STYLECHANGED = 0x007D,

        /// <summary>
        /// The WM_DISPLAYCHANGE message is sent to all windows when
        /// the display resolution has changed.
        /// </summary>
        WM_DISPLAYCHANGE = 0x007E,

        /// <summary>
        /// The WM_GETICON message is sent to a window to retrieve
        /// a handle to the large or small icon associated with
        /// a window. The system displays the large icon in the
        /// ALT+TAB dialog, and the small icon in the window caption.
        /// </summary>
        WM_GETICON = 0x007F,

        /// <summary>
        /// An application sends the WM_SETICON message to associate
        /// a new large or small icon with a window. The system displays
        /// the large icon in the ALT+TAB dialog box, and the small icon
        /// in the window caption.
        /// </summary>
        WM_SETICON = 0x0080,

        /// <summary>
        /// The WM_NCCREATE message is sent prior to the WM_CREATE
        /// message when a window is first created.
        /// </summary>
        WM_NCCREATE = 0x0081,

        /// <summary>
        /// <para>The WM_NCDESTROY message informs a window that its
        /// nonclient area is being destroyed. The DestroyWindow
        /// function sends the WM_NCDESTROY message to the window
        /// following the WM_DESTROY message. WM_DESTROY is used to
        /// free the allocated memory object associated with the window.
        /// </para>
        /// <para>The WM_NCDESTROY message is sent after the child
        /// windows have been destroyed. In contrast, WM_DESTROY is
        /// sent before the child windows are destroyed.</para>
        /// </summary>
        WM_NCDESTROY = 0x0082,

        /// <summary>
        /// The WM_NCCALCSIZE message is sent when the size and
        /// position of a window's client area must be calculated.
        /// By processing this message, an application can control
        /// the content of the window's client area when the size
        /// or position of the window changes.
        /// </summary>
        WM_NCCALCSIZE = 0x0083,

        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the
        /// cursor moves, or when a mouse button is pressed or released.
        /// If the mouse is not captured, the message is sent to the
        /// window beneath the cursor. Otherwise, the message is sent
        /// to the window that has captured the mouse.
        /// </summary>
        WM_NCHITTEST = 0x0084,

        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame
        /// must be painted.
        /// </summary>
        WM_NCPAINT = 0x0085,

        /// <summary>
        /// The WM_NCACTIVATE message is sent to a window when its
        /// nonclient area needs to be changed to indicate an active
        /// or inactive state.
        /// </summary>
        WM_NCACTIVATE = 0x0086,

        /// <summary>
        /// The WM_GETDLGCODE message is sent to the window procedure
        /// associated with a control. By default, the system handles
        /// all keyboard input to the control; the system interprets
        /// certain types of keyboard input as dialog box navigation
        /// keys. To override this default behavior, the control can
        /// respond to the WM_GETDLGCODE message to indicate the types
        /// of input it wants to process itself.
        /// </summary>
        WM_GETDLGCODE = 0x0087,

        /// <summary>
        /// The WM_SYNCPAINT message is used to synchronize painting while
        /// avoiding linking independent GUI threads.
        /// </summary>
        WM_SYNCPAINT = 0x0088,

        /// <summary>
        /// The WM_NCMOUSEMOVE message is posted to a window when the
        /// cursor is moved within the nonclient area of the window.
        /// This message is posted to the window that contains the
        /// cursor. If a window has captured the mouse, this message
        /// is not posted.
        /// </summary>
        WM_NCMOUSEMOVE = 0x00A0,

        /// <summary>
        /// The WM_NCLBUTTONDOWN message is posted when the user presses
        /// the left mouse button while the cursor is within the
        /// nonclient area of a window. This message is posted to the
        /// window that contains the cursor. If a window has captured
        /// the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONDOWN = 0x00A1,

        /// <summary>
        /// The WM_NCLBUTTONUP message is posted when the user releases
        /// the left mouse button while the cursor is within the
        /// nonclient area of a window. This message is posted to the
        /// window that contains the cursor. If a window has captured
        /// the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONUP = 0x00A2,

        /// <summary>
        /// The WM_NCLBUTTONDBLCLK message is posted when the user
        /// double-clicks the left mouse button while the cursor is
        /// within the nonclient area of a window. This message is
        /// posted to the window that contains the cursor. If a window
        /// has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0x00A3,

        /// <summary>
        /// The WM_NCRBUTTONDOWN message is posted when the user presses
        /// the right mouse button while the cursor is within the
        /// nonclient area of a window. This message is posted to the
        /// window that contains the cursor. If a window has captured
        /// the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONDOWN = 0x00A4,

        /// <summary>
        /// The WM_NCRBUTTONUP message is posted when the user releases
        /// the right mouse button while the cursor is within the
        /// nonclient area of a window. This message is posted to the
        /// window that contains the cursor. If a window has captured
        /// the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONUP = 0x00A5,

        /// <summary>
        /// The WM_NCRBUTTONDBLCLK message is posted when the user
        /// double-clicks the right mouse button while the cursor is
        /// within the nonclient area of a window. This message is
        /// posted to the window that contains the cursor. If a window
        /// has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0x00A6,

        /// <summary>
        /// The WM_NCMBUTTONDOWN message is posted when the user
        /// presses the middle mouse button while the cursor is within
        /// the nonclient area of a window. This message is posted
        /// to the window that contains the cursor. If a window has
        /// captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMBUTTONDOWN = 0x00A7,

        /// <summary>
        /// The WM_NCMBUTTONUP message is posted when the user releases
        /// the middle mouse button while the cursor is within the
        /// nonclient area of a window. This message is posted to the
        /// window that contains the cursor. If a window has captured
        /// the mouse, this message is not posted.
        /// </summary>
        WM_NCMBUTTONUP = 0x00A8,

        /// <summary>
        /// The WM_NCMBUTTONDBLCLK message is posted when the user
        /// double-clicks the middle mouse button while the cursor
        /// is within the nonclient area of a window. This message
        /// is posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message
        /// is not posted.
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0x00A9,

        /// <summary>
        /// The WM_NCXBUTTONDOWN message is posted when the user
        /// presses the first or second X button while the cursor
        /// is in the nonclient area of a window. This message
        /// is posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message
        /// is not posted.
        /// </summary>
        WM_NCXBUTTONDOWN = 0x00AB,

        /// <summary>
        /// The WM_NCXBUTTONUP message is posted when the user
        /// releases the first or second X button while the cursor
        /// is in the nonclient area of a window. This message is
        /// posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message
        /// is not posted.
        /// </summary>
        WM_NCXBUTTONUP = 0x00AC,

        /// <summary>
        /// The WM_NCXBUTTONDBLCLK message is posted when the user
        /// double-clicks the first or second X button while the
        /// cursor is in the nonclient area of a window. This message
        /// is posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message
        /// is not posted.
        /// </summary>
        WM_NCXBUTTONDBLCLK = 0x00AD,

        /// <summary>
        /// The WM_INPUT message is sent to the window that is
        /// getting raw input.
        /// </summary>
        WM_INPUT = 0x00FF,

        /// <summary>
        /// ???
        /// </summary>
        WM_KEYFIRST = 0x0100,

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with
        /// the keyboard focus when a nonsystem key is pressed.
        /// A nonsystem key is a key that is pressed when the
        /// ALT key is not pressed.
        /// </summary>
        WM_KEYDOWN = 0x0100,

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the
        /// keyboard focus when a nonsystem key is released.
        /// A nonsystem key is a key that is pressed when the
        /// ALT key is not pressed, or a keyboard key that is
        /// pressed when a window has the keyboard focus.
        /// </summary>
        WM_KEYUP = 0x0101,

        /// <summary>
        /// The WM_CHAR message is posted to the window with
        /// the keyboard focus when a WM_KEYDOWN message is
        /// translated by the TranslateMessage function.
        /// The WM_CHAR message contains the character code
        /// of the key that was pressed.
        /// </summary>
        WM_CHAR = 0x0102,

        /// <summary>
        /// The WM_DEADCHAR message is posted to the window with
        /// the keyboard focus when a WM_KEYUP message is translated
        /// by the TranslateMessage function. WM_DEADCHAR specifies
        /// a character code generated by a dead key. A dead key is
        /// a key that generates a character, such as the umlaut
        /// (double-dot), that is combined with another character to
        /// form a composite character. For example, the umlaut-O
        /// character is generated by typing the dead key for the
        /// umlaut character, and then typing the O key.
        /// </summary>
        WM_DEADCHAR = 0x0103,

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with
        /// the keyboard focus when the user presses the F10 key
        /// (which activates the menu bar) or holds down the ALT key
        /// and then presses another key. It also occurs when no
        /// window currently has the keyboard focus; in this case,
        /// the WM_SYSKEYDOWN message is sent to the active window.
        /// The window that receives the message can distinguish
        /// between these two contexts by checking the context code
        /// in the lParam parameter.
        /// </summary>
        WM_SYSKEYDOWN = 0x0104,

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with
        /// the keyboard focus when the user releases a key that
        /// was pressed while the ALT key was held down. It also
        /// occurs when no window currently has the keyboard focus;
        /// in this case, the WM_SYSKEYUP message is sent to the active
        /// window. The window that receives the message can distinguish
        /// between these two contexts by checking the context code
        /// in the lParam parameter.
        /// </summary>
        WM_SYSKEYUP = 0x0105,

        /// <summary>
        /// The WM_SYSCHAR message is posted to the window with the
        /// keyboard focus when a WM_SYSKEYDOWN message is translated
        /// by the TranslateMessage function. It specifies the character
        /// code of a system character key — that is, a character key
        /// that is pressed while the ALT key is down.
        /// </summary>
        WM_SYSCHAR = 0x0106,

        /// <summary>
        /// The WM_SYSDEADCHAR message is sent to the window with
        /// the keyboard focus when a WM_SYSKEYDOWN message is
        /// translated by the TranslateMessage function.
        /// WM_SYSDEADCHAR specifies the character code of a system
        /// dead key — that is, a dead key that is pressed while
        /// holding down the ALT key.
        /// </summary>
        WM_SYSDEADCHAR = 0x0107,

        /// <summary>
        /// <para>The WM_UNICHAR message is posted to the window
        /// with the keyboard focus when a WM_KEYDOWN message is
        /// translated by the TranslateMessage function. The
        /// WM_UNICHAR message contains the character code of the key
        /// that was pressed.</para>
        /// <para>The WM_UNICHAR message is equivalent to WM_CHAR,
        /// but it uses Unicode transformation format (UTF)-32,
        /// whereas WM_CHAR uses UTF-16. It is designed to send or
        /// post Unicode characters to ANSI windows and it can can
        /// handle Unicode Supplementary Plane characters</para>
        /// </summary>
        WM_UNICHAR = 0x0109,

        /// <summary>
        /// ???
        /// </summary>
        WM_KEYLAST_NT501 = 0x0109,

        /// <summary>
        /// ???
        /// </summary>
        UNICODE_NOCHAR = 0xFFFF,

        /// <summary>
        /// ???
        /// </summary>
        WM_KEYLAST_PRE501 = 0x0108,

        /// <summary>
        /// The WM_IME_STARTCOMPOSITION message is sent immediately
        /// before the IME generates the composition string as a result
        /// of a keystroke. The message is a notification to an IME
        /// window to open its composition window. An application should
        /// process this message if it displays composition characters
        /// itself.
        /// </summary>
        WM_IME_STARTCOMPOSITION = 0x010D,

        /// <summary>
        /// The WM_IME_ENDCOMPOSITION message is sent to an application
        /// when the IME ends composition. An application should process
        /// this message if it displays composition characters itself.
        /// </summary>
        WM_IME_ENDCOMPOSITION = 0x010E,

        /// <summary>
        /// The WM_IME_COMPOSITION message is sent to an application
        /// when the IME changes composition status as a result of
        /// a keystroke. An application should process this message
        /// if it displays composition characters itself. Otherwise,
        /// it should send the message to the IME window.
        /// </summary>
        WM_IME_COMPOSITION = 0x010F,

        /// <summary>
        /// ???
        /// </summary>
        WM_IME_KEYLAST = 0x010F,

        /// <summary>
        /// The WM_INITDIALOG message is sent to the dialog box
        /// procedure immediately before a dialog box is displayed.
        /// Dialog box procedures typically use this message to
        /// initialize controls and carry out any other initialization
        /// tasks that affect the appearance of the dialog box.
        /// </summary>
        WM_INITDIALOG = 0x0110,

        /// <summary>
        /// The WM_COMMAND message is sent when the user selects
        /// a command item from a menu, when a control sends a
        /// notification message to its parent window, or when an
        /// accelerator keystroke is translated.
        /// </summary>
        WM_COMMAND = 0x0111,

        /// <summary>
        /// A window receives this message when the user chooses
        /// a command from the Window menu (formerly known as the
        /// system or control menu) or when the user chooses the
        /// maximize button, minimize button, restore button, or
        /// close button.
        /// </summary>
        WM_SYSCOMMAND = 0x0112,

        /// <summary>
        /// The WM_TIMER message is posted to the installing thread's
        /// message queue when a timer expires. The message is posted
        /// by the GetMessage or PeekMessage function.
        /// </summary>
        WM_TIMER = 0x0113,

        /// <summary>
        /// The WM_HSCROLL message is sent to a window when a scroll
        /// event occurs in the window's standard horizontal scroll bar.
        /// This message is also sent to the owner of a horizontal
        /// scroll bar control when a scroll event occurs in the
        /// control.
        /// </summary>
        WM_HSCROLL = 0x0114,

        /// <summary>
        /// The WM_VSCROLL message is sent to a window when a scroll
        /// event occurs in the window's standard vertical scroll bar.
        /// This message is also sent to the owner of a vertical
        /// scroll bar control when a scroll event occurs in the
        /// control.
        /// </summary>
        WM_VSCROLL = 0x0115,

        /// <summary>
        /// The WM_INITMENU message is sent when a menu is about to
        /// become active. It occurs when the user clicks an item on
        /// the menu bar or presses a menu key. This allows the
        /// application to modify the menu before it is displayed.
        /// </summary>
        WM_INITMENU = 0x0116,

        /// <summary>
        /// The WM_INITMENUPOPUP message is sent when a drop-down menu
        /// or submenu is about to become active. This allows an
        /// application to modify the menu before it is displayed,
        /// without changing the entire menu.
        /// </summary>
        WM_INITMENUPOPUP = 0x0117,

        /// <summary>
        /// The WM_MENUSELECT message is sent to a menu's owner window
        /// when the user selects a menu item.
        /// </summary>
        WM_MENUSELECT = 0x011F,

        /// <summary>
        /// The WM_MENUCHAR message is sent when a menu is active and
        /// the user presses a key that does not correspond to any
        /// mnemonic or accelerator key. This message is sent to the
        /// window that owns the menu.
        /// </summary>
        WM_MENUCHAR = 0x0120,

        /// <summary>
        /// The WM_ENTERIDLE message is sent to the owner window of
        /// a modal dialog box or menu that is entering an idle state.
        /// A modal dialog box or menu enters an idle state when no
        /// messages are waiting in its queue after it has processed
        /// one or more previous messages.
        /// </summary>
        WM_ENTERIDLE = 0x0121,

        /// <summary>
        /// The WM_MENURBUTTONUP message is sent when the user releases
        /// the right mouse button while the cursor is on a menu item.
        /// </summary>
        WM_MENURBUTTONUP = 0x0122,

        /// <summary>
        /// The WM_MENUDRAG message is sent to the owner of a
        /// drag-and-drop menu when the user drags a menu item.
        /// </summary>
        WM_MENUDRAG = 0x0123,

        /// <summary>
        /// The WM_MENUGETOBJECT message is sent to the owner of a
        /// drag-and-drop menu when the mouse cursor enters a menu item
        /// or moves from the center of the item to the top or bottom
        /// of the item.
        /// </summary>
        WM_MENUGETOBJECT = 0x0124,

        /// <summary>
        /// The WM_UNINITMENUPOPUP message is sent when a drop-down
        /// menu or submenu has been destroyed.
        /// </summary>
        WM_UNINITMENUPOPUP = 0x0125,

        /// <summary>
        /// The WM_MENUCOMMAND message is sent when the user makes
        /// a selection from a menu.
        /// </summary>
        WM_MENUCOMMAND = 0x0126,

        /// <summary>
        /// An application sends the WM_CHANGEUISTATE message to
        /// indicate that the user interface (UI) state should be
        /// changed.
        /// </summary>
        WM_CHANGEUISTATE = 0x0127,

        /// <summary>
        /// An application sends the WM_UPDATEUISTATE message to change
        /// the user interface (UI) state for the specified window and
        /// all its child windows.
        /// </summary>
        WM_UPDATEUISTATE = 0x0128,

        /// <summary>
        /// An application sends the WM_QUERYUISTATE message to retrieve
        /// the user interface (UI) state for a window.
        /// </summary>
        WM_QUERYUISTATE = 0x0129,

        /// <summary>
        /// ???
        /// </summary>
        WM_CTLCOLORMSGBOX = 0x0132,

        /// <summary>
        /// An edit control that is not read-only or disabled sends
        /// the WM_CTLCOLOREDIT message to its parent window when the
        /// control is about to be drawn. By responding to this message,
        /// the parent window can use the specified device context
        /// handle to set the text and background colors of the edit
        /// control.
        /// </summary>
        WM_CTLCOLOREDIT = 0x0133,

        /// <summary>
        /// The WM_CTLCOLORLISTBOX message is sent to the parent window
        /// of a list box before the system draws the list box. By
        /// responding to this message, the parent window can set the
        /// text and background colors of the list box by using the
        /// specified display device context handle.
        /// </summary>
        WM_CTLCOLORLISTBOX = 0x0134,

        /// <summary>
        /// The WM_CTLCOLORBTN message is sent to the parent window of
        /// a button before drawing the button. The parent window can
        /// change the button's text and background colors. However,
        /// only owner-drawn buttons respond to the parent window
        /// processing this message.
        /// </summary>
        WM_CTLCOLORBTN = 0x0135,

        /// <summary>
        /// The WM_CTLCOLORDLG message is sent to a dialog box before
        /// the system draws the dialog box. By responding to this
        /// message, the dialog box can set its text and background
        /// colors using the specified display device context handle.
        /// </summary>
        WM_CTLCOLORDLG = 0x0136,

        /// <summary>
        /// The WM_CTLCOLORSCROLLBAR message is sent to the parent
        /// window of a scroll bar control when the control is about
        /// to be drawn. By responding to this message, the parent
        /// window can use the display context handle to set the
        /// background color of the scroll bar control.
        /// </summary>
        WM_CTLCOLORSCROLLBAR = 0x0137,

        /// <summary>
        /// A static control, or an edit control that is read-only or
        /// disabled, sends the WM_CTLCOLORSTATIC message to its parent
        /// window when the control is about to be drawn. By responding
        /// to this message, the parent window can use the specified
        /// device context handle to set the text and background colors
        /// of the static control.
        /// </summary>
        WM_CTLCOLORSTATIC = 0x0138,

        /// <summary>
        /// ???
        /// </summary>
        WM_MOUSEFIRST = 0x0200,

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the
        /// cursor moves. If the mouse is not captured, the message
        /// is posted to the window that contains the cursor. Otherwise,
        /// the message is posted to the window that has captured the
        /// mouse.
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses
        /// the left mouse button while the cursor is in the client area
        /// of a window. If the mouse is not captured, the message is
        /// posted to the window beneath the cursor. Otherwise, the
        /// message is posted to the window that has captured the mouse.
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases
        /// the left mouse button while the cursor is in the client area
        /// of a window. If the mouse is not captured, the message is
        /// posted to the window beneath the cursor. Otherwise, the
        /// message is posted to the window that has captured the mouse.
        /// </summary>
        WM_LBUTTONUP = 0x0202,

        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user
        /// double-clicks the left mouse button while the cursor is
        /// in the client area of a window. If the mouse is not captured,
        /// the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has
        /// captured the mouse.
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses
        /// the right mouse button while the cursor is in the client
        /// area of a window. If the mouse is not captured, the message
        /// is posted to the window beneath the cursor. Otherwise, the
        /// message is posted to the window that has captured the mouse.
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the
        /// right mouse button while the cursor is in the client area of
        /// a window. If the mouse is not captured, the message is posted
        /// to the window beneath the cursor. Otherwise, the message is
        /// posted to the window that has captured the mouse.
        /// </summary>
        WM_RBUTTONUP = 0x0205,

        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user
        /// double-clicks the right mouse button while the cursor is in
        /// the client area of a window. If the mouse is not captured,
        /// the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has
        /// captured the mouse.
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses
        /// the middle mouse button while the cursor is in the client
        /// area of a window. If the mouse is not captured, the message
        /// is posted to the window beneath the cursor. Otherwise,
        /// the message is posted to the window that has captured the
        /// mouse.
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases
        /// the middle mouse button while the cursor is in the client
        /// area of a window. If the mouse is not captured, the message
        /// is posted to the window beneath the cursor. Otherwise, the
        /// message is posted to the window that has captured the mouse.
        /// </summary>
        WM_MBUTTONUP = 0x0208,

        /// <summary>
        /// ???
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,

        /// <summary>
        /// The WM_MOUSEWHEEL message is sent to the focus window when
        /// the mouse wheel is rotated. The DefWindowProc function
        /// propagates the message to the window's parent. There should
        /// be no internal forwarding of the message, since DefWindowProc
        /// propagates it up the parent chain until it finds a window
        /// that processes it.
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,

        /// <summary>
        /// The WM_XBUTTONDOWN message is posted when the user presses
        /// the first or second X button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the
        /// message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has
        /// captured the mouse.
        /// </summary>
        WM_XBUTTONDOWN = 0x020B,

        /// <summary>
        /// The WM_XBUTTONUP message is posted when the user releases
        /// the first or second X button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the
        /// message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has
        /// captured the mouse.
        /// </summary>
        WM_XBUTTONUP = 0x020C,

        /// <summary>
        /// The WM_XBUTTONDBLCLK message is posted when the user
        /// double-clicks the first or second X button while the cursor
        /// is in the client area of a window. If the mouse is not
        /// captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that
        /// has captured the mouse.
        /// </summary>
        WM_XBUTTONDBLCLK = 0x020D,

        /// <summary>
        /// ???
        /// </summary>
        WM_MOUSELAST_5 = 0x020D,

        /// <summary>
        /// ???
        /// </summary>
        WM_MOUSELAST_4 = 0x020A,

        /// <summary>
        /// ???
        /// </summary>
        WM_MOUSELAST_PRE_4 = 0x0209,

        /// <summary>
        /// The WM_PARENTNOTIFY message is sent to the parent of a
        /// child window when the child window is created or destroyed,
        /// or when the user clicks a mouse button while the cursor is
        /// over the child window. When the child window is being
        /// created, the system sends WM_PARENTNOTIFY just before the
        /// CreateWindow or CreateWindowEx function that creates the
        /// window returns. When the child window is being destroyed,
        /// the system sends the message before any processing to
        /// destroy the window takes place.
        /// </summary>
        WM_PARENTNOTIFY = 0x0210,

        /// <summary>
        /// The WM_ENTERMENULOOP message informs an application's main
        /// window procedure that a menu modal loop has been entered.
        /// </summary>
        WM_ENTERMENULOOP = 0x0211,

        /// <summary>
        /// The WM_EXITMENULOOP message informs an application's main
        /// window procedure that a menu modal loop has been exited.
        /// </summary>
        WM_EXITMENULOOP = 0x0212,

        /// <summary>
        /// The WM_NEXTMENU message is sent to an application when the
        /// right or left arrow key is used to switch between the menu
        /// bar and the system menu.
        /// </summary>
        WM_NEXTMENU = 0x0213,

        /// <summary>
        /// The WM_SIZE message is sent to a window after its size
        /// has changed.
        /// </summary>
        WM_SIZING = 0x0214,

        /// <summary>
        /// The WM_CAPTURECHANGED message is sent to the window that is
        /// losing the mouse capture.
        /// </summary>
        WM_CAPTURECHANGED = 0x0215,

        /// <summary>
        /// The WM_MOVING message is sent to a window that the user is
        /// moving. By processing this message, an application can
        /// monitor the position of the drag rectangle and, if needed,
        /// change its position.
        /// </summary>
        WM_MOVING = 0x0216,

        /// <summary>
        /// The WM_POWERBROADCAST message is broadcast to an application
        /// to notify it of power-management events.
        /// </summary>
        WM_POWERBROADCAST = 0x0218,

        /// <summary>
        /// The WM_DEVICECHANGE device message notifies an application
        /// of a change to the hardware configuration of a device or the
        /// computer.
        /// </summary>
        WM_DEVICECHANGE = 0x0219,

        /// <summary>
        /// An application sends the WM_MDICREATE message to a
        /// multiple-document interface (MDI) client window to create
        /// an MDI child window.
        /// </summary>
        WM_MDICREATE = 0x0220,

        /// <summary>
        /// An application sends the WM_MDIDESTROY message to a
        /// multiple-document interface (MDI) client window to close
        /// an MDI child window.
        /// </summary>
        WM_MDIDESTROY = 0x0221,

        /// <summary>
        /// An application sends the WM_MDIACTIVATE message to a
        /// multiple-document interface (MDI) client window to instruct
        /// the client window to activate a different MDI child window.
        /// </summary>
        WM_MDIACTIVATE = 0x0222,

        /// <summary>
        /// An application sends the WM_MDIRESTORE message to a
        /// multiple-document interface (MDI) client window to restore
        /// an MDI child window from maximized or minimized size.
        /// </summary>
        WM_MDIRESTORE = 0x0223,

        /// <summary>
        /// An application sends the WM_MDINEXT message to a
        /// multiple-document interface (MDI) client window to activate
        /// the next or previous child window.
        /// </summary>
        WM_MDINEXT = 0x0224,

        /// <summary>
        /// An application sends the WM_MDIMAXIMIZE message to a
        /// multiple-document interface (MDI) client window to maximize
        /// an MDI child window. The system resizes the child window to
        /// make its client area fill the client window. The system
        /// places the child window's window menu icon in the rightmost
        /// position of the frame window's menu bar, and places the
        /// child window's restore icon in the leftmost position. The
        /// system also appends the title bar text of the child window
        /// to that of the frame window.
        /// </summary>
        WM_MDIMAXIMIZE = 0x0225,

        /// <summary>
        /// An application sends the WM_MDITILE message to a
        /// multiple-document interface (MDI) client window to arrange
        /// all of its MDI child windows in a tile format.
        /// </summary>
        WM_MDITILE = 0x0226,

        /// <summary>
        /// An application sends the WM_MDICASCADE message to a
        /// multiple-document interface (MDI) client window to arrange
        /// all its child windows in a cascade format.
        /// </summary>
        WM_MDICASCADE = 0x0227,

        /// <summary>
        /// An application sends the WM_MDIICONARRANGE message to a
        /// multiple-document interface (MDI) client window to arrange
        /// all minimized MDI child windows. It does not affect child
        /// windows that are not minimized.
        /// </summary>
        WM_MDIICONARRANGE = 0x0228,

        /// <summary>
        /// An application sends the WM_MDIGETACTIVE message to a
        /// multiple-document interface (MDI) client window to retrieve
        /// the handle to the active MDI child window.
        /// </summary>
        WM_MDIGETACTIVE = 0x0229,

        /// <summary>
        /// An application sends the WM_MDISETMENU message to a
        /// multiple-document interface (MDI) client window to replace
        /// the entire menu of an MDI frame window, to replace the
        /// window menu of the frame window, or both.
        /// </summary>
        WM_MDISETMENU = 0x0230,

        /// <summary>
        /// <para>The WM_ENTERSIZEMOVE message is sent one time to
        /// a window after it enters the moving or sizing modal loop.
        /// The window enters the moving or sizing modal loop when the
        /// user clicks the window's title bar or sizing border, or when
        /// the window passes the WM_SYSCOMMAND message to the
        /// DefWindowProc function and the wParam parameter of the
        /// message specifies the SC_MOVE or SC_SIZE value. The
        /// operation is complete when DefWindowProc returns.</para>
        /// <para>The system sends the WM_ENTERSIZEMOVE message
        /// regardless of whether the dragging of full windows is
        /// enabled.</para>
        /// </summary>
        WM_ENTERSIZEMOVE = 0x0231,

        /// <summary>
        /// The WM_EXITSIZEMOVE message is sent one time to a window,
        /// after it has exited the moving or sizing modal loop. The
        /// window enters the moving or sizing modal loop when the user
        /// clicks the window's title bar or sizing border, or when the
        /// window passes the WM_SYSCOMMAND message to the DefWindowProc
        /// function and the wParam parameter of the message specifies
        /// the SC_MOVE or SC_SIZE value. The operation is complete when
        /// DefWindowProc returns.
        /// </summary>
        WM_EXITSIZEMOVE = 0x0232,

        /// <summary>
        /// Sent when the user drops a file on the window of an
        /// application that has registered itself as a recipient of
        /// dropped files.
        /// </summary>
        WM_DROPFILES = 0x0233,

        /// <summary>
        /// An application sends the WM_MDIREFRESHMENU message to a
        /// multiple-document interface (MDI) client window to refresh
        /// the window menu of the MDI frame window.
        /// </summary>
        WM_MDIREFRESHMENU = 0x0234,

        /// <summary>
        /// The WM_IME_SETCONTEXT message is sent to an application
        /// when a window of the application is activated. If the
        /// application has created an IME window, it should call the
        /// ImmIsUIMessage function. Otherwise, it should pass this
        /// message to the DefWindowProc function.
        /// </summary>
        WM_IME_SETCONTEXT = 0x0281,

        /// <summary>
        /// The WM_IME_NOTIFY message is sent to an application to
        /// notify it of changes to the IME window. An application
        /// processes this message if it is responsible for managing
        /// the IME window.
        /// </summary>
        WM_IME_NOTIFY = 0x0282,

        /// <summary>
        /// The WM_IME_CONTROL message directs the IME window to carry
        /// out the requested command. An application uses this message
        /// to control the IME window created by the application.
        /// </summary>
        WM_IME_CONTROL = 0x0283,

        /// <summary>
        /// The WM_IME_COMPOSITIONFULL message is sent to an application
        /// when the IME window finds no space to extend the area for the
        /// composition window. The application should use the
        /// IMC_SETCOMPOSITIONWINDOW command to specify how the window
        /// should be displayed.
        /// </summary>
        WM_IME_COMPOSITIONFULL = 0x0284,

        /// <summary>
        /// The WM_IME_SELECT message is sent to an application when the
        /// system is about to change the current IME. An application
        /// that has created an IME window should pass this message to
        /// that window so that it can retrieve the keyboard layout
        /// handle for the newly selected IME.
        /// </summary>
        WM_IME_SELECT = 0x0285,

        /// <summary>
        /// The WM_IME_CHAR message is sent to an application when the
        /// IME gets a character of the conversion result. Unlike the
        /// WM_CHAR message for a non-Unicode window, this message can
        /// include double-byte as well as single-byte character values.
        /// For a Unicode window, this message is the same as WM_CHAR.
        /// </summary>
        WM_IME_CHAR = 0x0286,

        /// <summary>
        /// The WM_IME_REQUEST message provides a group of commands to
        /// request information from an application.
        /// </summary>
        WM_IME_REQUEST = 0x0288,

        /// <summary>
        /// The WM_IME_KEYDOWN message is sent to an application by the
        /// IME to notify the application of a key press. An application
        /// can process this message or pass it to the DefWindowProc
        /// function to generate a matching WM_KEYDOWN message. This
        /// message is usually generated by the IME to keep message order.
        /// </summary>
        WM_IME_KEYDOWN = 0x0290,

        /// <summary>
        /// The WM_IME_KEYUP message is sent to an application by the
        /// IME to notify the application of a key release. An
        /// application can process this message or pass it to the
        /// DefWindowProc function to generate a matching WM_KEYUP
        /// message. This message is usually generated by the IME to
        /// keep message order.
        /// </summary>
        WM_IME_KEYUP = 0x0291,

        /// <summary>
        /// The WM_MOUSEHOVER message is posted to a window when the
        /// cursor hovers over the client area of the window for the
        /// period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSEHOVER = 0x02A1,

        /// <summary>
        /// The WM_MOUSELEAVE message is posted to a window when the
        /// cursor leaves the client area of the window specified in
        /// a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSELEAVE = 0x02A3,

        /// <summary>
        /// The WM_NCMOUSEHOVER message is posted to a window when the
        /// cursor hovers over the nonclient area of the window for the
        /// period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_NCMOUSEHOVER = 0x02A0,

        /// <summary>
        /// The WM_NCMOUSELEAVE message is posted to a window when the
        /// cursor leaves the nonclient area of the window specified in
        /// a prior call to TrackMouseEvent.
        /// </summary>
        WM_NCMOUSELEAVE = 0x02A2,

        /// <summary>
        /// The WM_WTSSESSION_CHANGE message notifies applications of
        /// changes in session state.
        /// </summary>
        WM_WTSSESSION_CHANGE = 0x02B1,

        /// <summary>
        /// ???
        /// </summary>
        WM_TABLET_FIRST = 0x02c0,

        /// <summary>
        /// ???
        /// </summary>
        WM_TABLET_LAST = 0x02df,

        /// <summary>
        /// An application sends a WM_CUT message to an edit control
        /// or combo box to delete (cut) the current selection, if any,
        /// in the edit control and copy the deleted text to the
        /// clipboard in CF_TEXT format.
        /// </summary>
        WM_CUT = 0x0300,

        /// <summary>
        /// An application sends the WM_COPY message to an edit control
        /// or combo box to copy the current selection to the clipboard
        /// in CF_TEXT format.
        /// </summary>
        WM_COPY = 0x0301,

        /// <summary>
        /// An application sends a WM_PASTE message to an edit control
        /// or combo box to copy the current content of the clipboard
        /// to the edit control at the current caret position. Data is
        /// inserted only if the clipboard contains data in CF_TEXT
        /// format.
        /// </summary>
        WM_PASTE = 0x0302,

        /// <summary>
        /// An application sends a WM_CLEAR message to an edit control
        /// or combo box to delete (clear) the current selection,
        /// if any, from the edit control.
        /// </summary>
        WM_CLEAR = 0x0303,

        /// <summary>
        /// An application sends a WM_UNDO message to an edit control to
        /// undo the last operation. When this message is sent to an edit
        /// control, the previously deleted text is restored or the
        /// previously added text is deleted.
        /// </summary>
        WM_UNDO = 0x0304,

        /// <summary>
        /// The WM_RENDERFORMAT message is sent to the clipboard owner
        /// if it has delayed rendering a specific clipboard format
        /// and if an application has requested data in that format.
        /// The clipboard owner must render data in the specified format
        /// and place it on the clipboard by calling the SetClipboardData
        /// function.
        /// </summary>
        WM_RENDERFORMAT = 0x0305,

        /// <summary>
        /// The WM_RENDERALLFORMATS message is sent to the clipboard
        /// owner before it is destroyed, if the clipboard owner has
        /// delayed rendering one or more clipboard formats. For the
        /// content of the clipboard to remain available to other
        /// applications, the clipboard owner must render data in all
        /// the formats it is capable of generating, and place the data
        /// on the clipboard by calling the SetClipboardData function.
        /// </summary>
        WM_RENDERALLFORMATS = 0x0306,

        /// <summary>
        /// The WM_DESTROYCLIPBOARD message is sent to the clipboard
        /// owner when a call to the EmptyClipboard function empties
        /// the clipboard.
        /// </summary>
        WM_DESTROYCLIPBOARD = 0x0307,

        /// <summary>
        /// The WM_DRAWCLIPBOARD message is sent to the first window
        /// in the clipboard viewer chain when the content of the
        /// clipboard changes. This enables a clipboard viewer window
        /// to display the new content of the clipboard.
        /// </summary>
        WM_DRAWCLIPBOARD = 0x0308,

        /// <summary>
        /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner
        /// by a clipboard viewer window when the clipboard contains
        /// data in the CF_OWNERDISPLAY format and the clipboard
        /// viewer's client area needs repainting.
        /// </summary>
        WM_PAINTCLIPBOARD = 0x0309,

        /// <summary>
        /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard
        /// owner by a clipboard viewer window when the clipboard
        /// contains data in the CF_OWNERDISPLAY format and an event
        /// occurs in the clipboard viewer's vertical scroll bar.
        /// The owner should scroll the clipboard image and update
        /// the scroll bar values.
        /// </summary>
        WM_VSCROLLCLIPBOARD = 0x030A,

        /// <summary>
        /// The WM_SIZECLIPBOARD message is sent to the clipboard
        /// owner by a clipboard viewer window when the clipboard
        /// contains data in the CF_OWNERDISPLAY format and the
        /// clipboard viewer's client area has changed size.
        /// </summary>
        WM_SIZECLIPBOARD = 0x030B,

        /// <summary>
        /// The WM_ASKCBFORMATNAME message is sent to the clipboard
        /// owner by a clipboard viewer window to request the name
        /// of a CF_OWNERDISPLAY clipboard format.
        /// </summary>
        WM_ASKCBFORMATNAME = 0x030C,

        /// <summary>
        /// The WM_CHANGECBCHAIN message is sent to the first window
        /// in the clipboard viewer chain when a window is being
        /// removed from the chain.
        /// </summary>
        WM_CHANGECBCHAIN = 0x030D,

        /// <summary>
        /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard
        /// owner by a clipboard viewer window. This occurs when the
        /// clipboard contains data in the CF_OWNERDISPLAY format and
        /// an event occurs in the clipboard viewer's horizontal scroll
        /// bar. The owner should scroll the clipboard image and update
        /// the scroll bar values.
        /// </summary>
        WM_HSCROLLCLIPBOARD = 0x030E,

        /// <summary>
        /// The WM_QUERYNEWPALETTE message informs a window that it is
        /// about to receive the keyboard focus, giving the window the
        /// opportunity to realize its logical palette when it receives
        /// the focus.
        /// </summary>
        WM_QUERYNEWPALETTE = 0x030F,

        /// <summary>
        /// The WM_PALETTEISCHANGING message informs applications that
        /// an application is going to realize its logical palette.
        /// </summary>
        WM_PALETTEISCHANGING = 0x0310,

        /// <summary>
        /// The WM_PALETTECHANGED message is sent to all top-level and
        /// overlapped windows after the window with the keyboard focus
        /// has realized its logical palette, thereby changing the
        /// system palette. This message enables a window that uses
        /// a color palette but does not have the keyboard focus to
        /// realize its logical palette and update its client area.
        /// </summary>
        WM_PALETTECHANGED = 0x0311,

        /// <summary>
        /// The WM_HOTKEY message is posted when the user presses a
        /// hot key registered by the RegisterHotKey function. The
        /// message is placed at the top of the message queue associated
        /// with the thread that registered the hot key.
        /// </summary>
        WM_HOTKEY = 0x0312,

        /// <summary>
        /// The WM_PRINT message is sent to a window to request that
        /// it draw itself in the specified device context, most
        /// commonly in a printer device context.
        /// </summary>
        WM_PRINT = 0x0317,

        /// <summary>
        /// The WM_PRINTCLIENT message is sent to a window to request that
        /// it draw its client area in the specified device context, most
        /// commonly in a printer device context.
        /// </summary>
        WM_PRINTCLIENT = 0x0318,

        /// <summary>
        /// The WM_APPCOMMAND message notifies a window that the user
        /// generated an application command event, for example, by
        /// clicking an application command button using the mouse
        /// or typing an application command key on the keyboard.
        /// </summary>
        WM_APPCOMMAND = 0x0319,

        /// <summary>
        /// The WM_THEMECHANGED message is broadcast to every window
        /// following a theme change event. Examples of theme change
        /// events are the activation of a theme, the deactivation of
        /// a theme, or a transition from one theme to another.
        /// </summary>
        WM_THEMECHANGED = 0x031A,

        /// <summary>
        /// ???
        /// </summary>
        WM_HANDHELDFIRST = 0x0358,

        /// <summary>
        /// ???
        /// </summary>
        WM_HANDHELDLAST = 0x035F,

        /// <summary>
        /// ???
        /// </summary>
        WM_AFXFIRST = 0x0360,

        /// <summary>
        /// ???
        /// </summary>
        WM_AFXLAST = 0x037F,

        /// <summary>
        /// ???
        /// </summary>
        WM_PENWINFIRST = 0x0380,

        /// <summary>
        /// ???
        /// </summary>
        WM_PENWINLAST = 0x038F,

        /// <summary>
        /// he WM_APP constant is used by applications to help define
        /// private messages, usually of the form WM_APP+X, where X is
        /// an integer value.
        /// </summary>
        WM_APP = 0x8000,

        /// <summary>
        /// The WM_USER constant is used by applications to help define
        /// private messages for use by private window classes, usually
        /// of the form WM_USER+X, where X is an integer value.
        /// </summary>
        WM_USER = 0x0400,

        #endregion

        #region EDIT control messages

        /// <summary>
        /// The EM_GETSEL message retrieves the starting and ending
        /// character positions of the current selection in an edit
        /// control.
        /// </summary>
        EM_GETSEL = 0x00B0,

        /// <summary>
        /// The EM_SETSEL message selects a range of characters in an
        /// edit control.
        /// </summary>
        EM_SETSEL = 0x00B1,

        /// <summary>
        /// The EM_GETRECT message retrieves the formatting rectangle
        /// of an edit control. The formatting rectangle is the limiting
        /// rectangle into which the control draws the text. The limiting
        /// rectangle is independent of the size of the edit-control
        /// window.
        /// </summary>
        EM_GETRECT = 0x00B2,

        /// <summary>
        /// The EM_SETRECT message sets the formatting rectangle of a
        /// multiline edit control. The formatting rectangle is the
        /// limiting rectangle into which the control draws the text.
        /// The limiting rectangle is independent of the size of the
        /// edit control window.
        /// </summary>
        EM_SETRECT = 0x00B3,

        /// <summary>
        /// <para>The EM_SETRECTNP message sets the formatting rectangle
        /// of a multiline edit control. The EM_SETRECTNP message is
        /// identical to the EM_SETRECT message, except that
        /// EM_SETRECTNP does not redraw the edit control window.</para>
        /// <para>The formatting rectangle is the limiting rectangle
        /// into which the control draws the text. The limiting
        /// rectangle is independent of the size of the edit control
        /// window</para>
        /// </summary>
        EM_SETRECTNP = 0x00B4,

        /// <summary>
        /// The EM_SCROLL message scrolls the text vertically in a
        /// multiline edit control. This message is equivalent to
        /// sending a WM_VSCROLL message to the edit control.
        /// </summary>
        EM_SCROLL = 0x00B5,

        /// <summary>
        /// The EM_LINESCROLL message scrolls the text in a multiline
        /// edit control.
        /// </summary>
        EM_LINESCROLL = 0x00B6,

        /// <summary>
        /// The EM_SCROLLCARET message scrolls the caret into view in
        /// an edit control.
        /// </summary>
        EM_SCROLLCARET = 0x00B7,

        /// <summary>
        /// The EM_GETMODIFY message retrieves the state of an edit
        /// control's modification flag. The flag indicates whether
        /// the contents of the edit control have been modified.
        /// </summary>
        EM_GETMODIFY = 0x00B8,

        /// <summary>
        /// The EM_SETMODIFY message sets or clears the modification
        /// flag for an edit control. The modification flag indicates
        /// whether the text within the edit control has been modified.
        /// </summary>
        EM_SETMODIFY = 0x00B9,

        /// <summary>
        /// The EM_GETLINECOUNT message retrieves the number of lines
        /// in a multiline edit control.
        /// </summary>
        EM_GETLINECOUNT = 0x00BA,

        /// <summary>
        /// The EM_LINEINDEX message retrieves the character index of
        /// the first character of a specified line in a multiline edit
        /// control. A character index is the zero-based index of the
        /// character from the beginning of the edit control.
        /// </summary>
        EM_LINEINDEX = 0x00BB,

        /// <summary>
        /// The EM_SETHANDLE message sets the handle of the memory that
        /// will be used by a multiline edit control.
        /// </summary>
        EM_SETHANDLE = 0x00BC,

        /// <summary>
        /// The EM_GETHANDLE message retrieves a handle of the memory
        /// currently allocated for a multiline edit control's text.
        /// </summary>
        EM_GETHANDLE = 0x00BD,

        /// <summary>
        /// The EM_GETTHUMB message retrieves the position of the scroll
        /// box (thumb) in the vertical scroll bar of a multiline edit
        /// control.
        /// </summary>
        EM_GETTHUMB = 0x00BE,

        /// <summary>
        /// The EM_LINELENGTH message retrieves the length,
        /// in characters, of a line in an edit control.
        /// </summary>
        EM_LINELENGTH = 0x00C1,

        /// <summary>
        /// The EM_REPLACESEL message replaces the current selection
        /// in an edit control with the specified text.
        /// </summary>
        EM_REPLACESEL = 0x00C2,

        /// <summary>
        /// The EM_GETLINE message copies a line of text from an edit
        /// control and places it in a specified buffer.
        /// </summary>
        EM_GETLINE = 0x00C4,

        /// <summary>
        /// <para>The EM_LIMITTEXT message sets the text limit of an
        /// edit control. The text limit is the maximum amount of text,
        /// in TCHARs, that the user can type into the edit control.
        /// </para>
        /// <para>For edit controls and Microsoft® Rich Edit 1.0, bytes
        /// are used. For Rich Edit 2.0 and later, characters are used.
        /// </para>
        /// </summary>
        EM_LIMITTEXT = 0x00C5,

        /// <summary>
        /// The EM_CANUNDO message determines whether there are any
        /// actions in an edit control's undo queue.
        /// </summary>
        EM_CANUNDO = 0x00C6,

        /// <summary>
        /// The EM_UNDO message undoes the last edit control operation
        /// in the control's undo queue. You can send this message to
        /// either an edit control or a rich edit control.
        /// </summary>
        EM_UNDO = 0x00C7,

        /// <summary>
        /// The EM_FMTLINES message sets a flag that determines whether
        /// a multiline edit control includes soft line-break characters.
        /// A soft line break consists of two carriage returns and a line
        /// feed and is inserted at the end of a line that is broken
        /// because of wordwrapping.
        /// </summary>
        EM_FMTLINES = 0x00C8,

        /// <summary>
        /// The EM_LINEFROMCHAR message retrieves the index of the line
        /// that contains the specified character index in a multiline
        /// edit control. A character index is the zero-based index of
        /// the character from the beginning of the edit control.
        /// </summary>
        EM_LINEFROMCHAR = 0x00C9,

        /// <summary>
        /// <para>The EM_SETTABSTOPS message sets the tab stops in a
        /// multiline edit control. When text is copied to the control,
        /// any tab character in the text causes space to be generated
        /// up to the next tab stop.</para>
        /// <para>This message is processed only by multiline edit
        /// controls.</para>
        /// </summary>
        EM_SETTABSTOPS = 0x00CB,

        /// <summary>
        /// The EM_SETPASSWORDCHAR message sets or removes the password
        /// character for an edit control. When a password character is
        /// set, that character is displayed in place of the characters
        /// typed by the user.
        /// </summary>
        EM_SETPASSWORDCHAR = 0x00CC,

        /// <summary>
        /// The EM_EMPTYUNDOBUFFER message resets the undo flag of an
        /// edit control. The undo flag is set whenever an operation
        /// within the edit control can be undone.
        /// </summary>
        EM_EMPTYUNDOBUFFER = 0x00CD,

        /// <summary>
        /// The EM_GETFIRSTVISIBLELINE message retrieves the zero-based
        /// index of the uppermost visible line in a multiline edit
        /// control.
        /// </summary>
        EM_GETFIRSTVISIBLELINE = 0x00CE,

        /// <summary>
        /// The EM_SETREADONLY message sets or removes the read-only
        /// style (ES_READONLY) of an edit control.
        /// </summary>
        EM_SETREADONLY = 0x00CF,

        /// <summary>
        /// The EM_SETWORDBREAKPROC message replaces an edit control's
        /// default Wordwrap function with an application-defined
        /// Wordwrap function.
        /// </summary>
        EM_SETWORDBREAKPROC = 0x00D0,

        /// <summary>
        /// The EM_GETWORDBREAKPROC message retrieves the address
        /// of the current Wordwrap function.
        /// </summary>
        EM_GETWORDBREAKPROC = 0x00D1,

        /// <summary>
        /// The EM_GETPASSWORDCHAR message retrieves the password
        /// character that an edit control displays when the user
        /// enters text.
        /// </summary>
        EM_GETPASSWORDCHAR = 0x00D2,

        /// <summary>
        /// The EM_SETMARGINS message sets the widths of the left and
        /// right margins for an edit control. The message redraws the
        /// control to reflect the new margins.
        /// </summary>
        EM_SETMARGINS = 0x00D3,

        /// <summary>
        /// An application sends the EM_GETMARGINS message to retrieve
        /// the widths of the left and right margins for an edit control.
        /// </summary>
        EM_GETMARGINS = 0x00D4,

        /// <summary>
        /// <para>The EM_SETLIMITTEXT message sets the text limit of
        /// an edit control. The text limit is the maximum amount of
        /// text, in TCHARs, that the user can type into the edit
        /// control.</para>
        /// <para>For edit controls and Microsoft® Rich Edit 1.0, bytes
        /// are used. For Rich Edit 2.0 and later, characters are used.
        /// </para>
        /// <para>The EM_SETLIMITTEXT message is identical to the
        /// EM_LIMITTEXT message.</para>
        /// </summary>
        EM_SETLIMITTEXT = EM_LIMITTEXT,

        /// <summary>
        /// The EM_GETLIMITTEXT message retrieves the current text
        /// limit for an edit control.
        /// </summary>
        EM_GETLIMITTEXT = 0x00D5,

        /// <summary>
        /// The EM_POSFROMCHAR message retrieves the client area
        /// coordinates of a specified character in an edit control.
        /// </summary>
        EM_POSFROMCHAR = 0x00D6,

        /// <summary>
        /// The EM_CHARFROMPOS message retrieves information about
        /// the character closest to a specified point in the client
        /// area of an edit control.
        /// </summary>
        EM_CHARFROMPOS = 0x00D7,

        /// <summary>
        /// The EM_SETIMESTATUS message sets the status flags that
        /// determine how an edit control interacts with the Input
        /// Method Editor (IME).
        /// </summary>
        EM_SETIMESTATUS = 0x00D8,

        /// <summary>
        /// The EM_GETIMESTATUS message retrieves a set of status flags
        /// that indicate how the edit control interacts with the
        /// Input Method Editor (IME).
        /// </summary>
        EM_GETIMESTATUS = 0x00D9,

        /// <summary>
        ///
        /// </summary>
        EM_CANPASTE = WM_USER + 50,

        /// <summary>
        ///
        /// </summary>
        EM_DISPLAYBAND = WM_USER + 51,

        /// <summary>
        ///
        /// </summary>
        EM_EXGETSEL = WM_USER + 52,

        /// <summary>
        ///
        /// </summary>
        EM_EXLIMITTEXT = WM_USER + 53,

        /// <summary>
        ///
        /// </summary>
        EM_EXLINEFROMCHAR = WM_USER + 54,

        /// <summary>
        ///
        /// </summary>
        EM_EXSETSEL = WM_USER + 55,

        /// <summary>
        ///
        /// </summary>
        EM_FINDTEXT = WM_USER + 56,

        /// <summary>
        ///
        /// </summary>
        EM_FORMATRANGE = WM_USER + 57,

        /// <summary>
        ///
        /// </summary>
        EM_GETCHARFORMAT = WM_USER + 58,

        /// <summary>
        ///
        /// </summary>
        EM_GETEVENTMASK = WM_USER + 59,

        /// <summary>
        ///
        /// </summary>
        EM_GETOLEINTERFACE = WM_USER + 60,

        /// <summary>
        ///
        /// </summary>
        EM_GETPARAFORMAT = WM_USER + 61,

        /// <summary>
        ///
        /// </summary>
        EM_GETSELTEXT = WM_USER + 62,

        /// <summary>
        ///
        /// </summary>
        EM_HIDESELECTION = WM_USER + 63,

        /// <summary>
        ///
        /// </summary>
        EM_PASTESPECIAL = WM_USER + 64,

        /// <summary>
        ///
        /// </summary>
        EM_REQUESTRESIZE = WM_USER + 65,

        /// <summary>
        ///
        /// </summary>
        EM_SELECTIONTYPE = WM_USER + 66,

        /// <summary>
        ///
        /// </summary>
        EM_SETBKGNDCOLOR = WM_USER + 67,

        /// <summary>
        ///
        /// </summary>
        EM_SETCHARFORMAT = WM_USER + 68,

        /// <summary>
        ///
        /// </summary>
        EM_SETEVENTMASK = WM_USER + 69,

        /// <summary>
        ///
        /// </summary>
        EM_SETOLECALLBACK = WM_USER + 70,

        /// <summary>
        ///
        /// </summary>
        EM_SETPARAFORMAT = WM_USER + 71,

        /// <summary>
        ///
        /// </summary>
        EM_SETTARGETDEVICE = WM_USER + 72,

        /// <summary>
        ///
        /// </summary>
        EM_STREAMIN = WM_USER + 73,

        /// <summary>
        ///
        /// </summary>
        EM_STREAMOUT = WM_USER + 74,

        /// <summary>
        ///
        /// </summary>
        EM_GETTEXTRANGE = WM_USER + 75,

        /// <summary>
        ///
        /// </summary>
        EM_FINDWORDBREAK = WM_USER + 76,

        /// <summary>
        ///
        /// </summary>
        EM_SETOPTIONS = WM_USER + 77,

        /// <summary>
        ///
        /// </summary>
        EM_GETOPTIONS = WM_USER + 78,

        /// <summary>
        ///
        /// </summary>
        EM_FINDTEXTEX = WM_USER + 79,

        /// <summary>
        ///
        /// </summary>
        EM_GETWORDBREAKPROCEX = WM_USER + 80,

        /// <summary>
        ///
        /// </summary>
        EM_SETWORDBREAKPROCEX = WM_USER + 81,

        /// <summary>
        ///
        /// </summary>
        EM_SETUNDOLIMIT = WM_USER + 82,

        /// <summary>
        ///
        /// </summary>
        EM_REDO = WM_USER + 84,

        /// <summary>
        ///
        /// </summary>
        EM_CANREDO = WM_USER + 85,

        /// <summary>
        ///
        /// </summary>
        EM_GETUNDONAME = WM_USER + 86,

        /// <summary>
        ///
        /// </summary>
        EM_GETREDONAME = WM_USER + 87,

        /// <summary>
        ///
        /// </summary>
        EM_STOPGROUPTYPING = WM_USER + 88,

        /// <summary>
        ///
        /// </summary>
        EM_SETTEXTMODE = WM_USER + 89,

        /// <summary>
        ///
        /// </summary>
        EM_GETTEXTMODE = WM_USER + 90,

        /// <summary>
        ///
        /// </summary>
        EM_AUTOURLDETECT = WM_USER + 91,

        /// <summary>
        ///
        /// </summary>
        EM_GETAUTOURLDETECT = WM_USER + 92,

        /// <summary>
        ///
        /// </summary>
        EM_SETPALETTE = WM_USER + 93,

        /// <summary>
        ///
        /// </summary>
        EM_GETTEXTEX = WM_USER + 94,

        /// <summary>
        ///
        /// </summary>
        EM_GETTEXTLENGTHEX = WM_USER + 95,

        /// <summary>
        ///
        /// </summary>
        EM_SHOWSCROLLBAR = WM_USER + 96,

        /// <summary>
        ///
        /// </summary>
        EM_SETTEXTEX = WM_USER + 97,

        /// <summary>
        ///
        /// </summary>
        EM_SETPUNCTUATION = WM_USER + 100,

        /// <summary>
        ///
        /// </summary>
        EM_GETPUNCTUATION = WM_USER + 101,

        /// <summary>
        ///
        /// </summary>
        EM_SETWORDWRAPMODE = WM_USER + 102,

        /// <summary>
        ///
        /// </summary>
        EM_GETWORDWRAPMODE = WM_USER + 103,

        /// <summary>
        ///
        /// </summary>
        EM_SETIMECOLOR = WM_USER + 104,

        /// <summary>
        ///
        /// </summary>
        EM_GETIMECOLOR = WM_USER + 105,

        /// <summary>
        ///
        /// </summary>
        EM_SETIMEOPTIONS = WM_USER + 106,

        /// <summary>
        ///
        /// </summary>
        EM_GETIMEOPTIONS = WM_USER + 107,

        /// <summary>
        ///
        /// </summary>
        EM_CONVPOSITION = WM_USER + 108,

        /// <summary>
        ///
        /// </summary>
        EM_SETLANGOPTIONS = WM_USER + 120,

        /// <summary>
        ///
        /// </summary>
        EM_GETLANGOPTIONS = WM_USER + 121,

        /// <summary>
        ///
        /// </summary>
        EM_GETIMECOMPMODE = WM_USER + 122,

        /// <summary>
        ///
        /// </summary>
        EM_FINDTEXTW = WM_USER + 123,

        /// <summary>
        ///
        /// </summary>
        EM_FINDTEXTEXW = WM_USER + 124,

        /// <summary>
        ///
        /// </summary>
        EM_RECONVERSION = WM_USER + 125,

        /// <summary>
        ///
        /// </summary>
        EM_SETIMEMODEBIAS = WM_USER + 126,

        /// <summary>
        ///
        /// </summary>
        EM_GETIMEMODEBIAS = WM_USER + 127,

        /// <summary>
        ///
        /// </summary>
        EM_SETBIDIOPTIONS = WM_USER + 200,

        /// <summary>
        ///
        /// </summary>
        EM_GETBIDIOPTIONS = WM_USER + 201,

        /// <summary>
        ///
        /// </summary>
        EM_SETTYPOGRAPHYOPTIONS = WM_USER + 202,

        /// <summary>
        ///
        /// </summary>
        EM_GETTYPOGRAPHYOPTIONS = WM_USER + 203,

        /// <summary>
        ///
        /// </summary>
        EM_SETEDITSTYLE = WM_USER + 204,

        /// <summary>
        ///
        /// </summary>
        EM_GETEDITSTYLE = WM_USER + 205,

        #endregion

        #region BUTTON control messages

        /// <summary>
        /// An application sends a BM_GETCHECK message to retrieve
        /// the check state of a radio button or check box.
        /// </summary>
        BM_GETCHECK = 0x00F0,

        /// <summary>
        /// An application sends a BM_SETCHECK message to set the
        /// check state of a radio button or check box.
        /// </summary>
        BM_SETCHECK = 0x00F1,

        /// <summary>
        /// An application sends a BM_GETSTATE message to determine
        /// the state of a button or check box.
        /// </summary>
        BM_GETSTATE = 0x00F2,

        /// <summary>
        /// An application sends a BM_SETSTATE message to change the
        /// highlight state of a button. The highlight state indicates
        /// whether the button is highlighted as if the user had
        /// pushed it.
        /// </summary>
        BM_SETSTATE = 0x00F3,

        /// <summary>
        /// An application sends a BM_SETSTYLE message to change
        /// the style of a button.
        /// </summary>
        BM_SETSTYLE = 0x00F4,

        /// <summary>
        /// An application sends a BM_CLICK message to simulate the
        /// user clicking a button. This message causes the button to
        /// receive the WM_LBUTTONDOWN and WM_LBUTTONUP messages, and
        /// the button's parent window to receive a BN_CLICKED
        /// notification message.
        /// </summary>
        BM_CLICK = 0x00F5,

        /// <summary>
        /// An application sends a BM_GETIMAGE message to retrieve a
        /// handle to the image (icon or bitmap) associated with the
        /// button.
        /// </summary>
        BM_GETIMAGE = 0x00F6,

        /// <summary>
        /// An application sends a BM_SETIMAGE message to associate
        /// a new image (icon or bitmap) with the button.
        /// </summary>
        BM_SETIMAGE = 0x00F7,

        #endregion

        #region STATIC control messages

        /// <summary>
        /// An application sends the STM_SETICON message to associate
        /// an icon with an icon control.
        /// </summary>
        STM_SETICON = 0x0170,

        /// <summary>
        /// An application sends the STM_GETICON message to retrieve
        /// a handle to the icon associated with a static control that
        /// has the SS_ICON style.
        /// </summary>
        STM_GETICON = 0x0171,

        /// <summary>
        /// An application sends an STM_SETIMAGE message to associate
        /// a new image with a static control.
        /// </summary>
        STM_SETIMAGE = 0x0172,

        /// <summary>
        /// An application sends an STM_GETIMAGE message to retrieve a
        /// handle to the image (icon or bitmap) associated with a
        /// static control.
        /// </summary>
        STM_GETIMAGE = 0x0173,

        /// <summary>
        /// ???
        /// </summary>
        STM_MSGMAX = 0x0174,

        #endregion

        #region Dialog box messages

        /// <summary>
        /// An application sends a DM_GETDEFID message to retrieve the
        /// identifier of the default push button control for a dialog
        /// box.
        /// </summary>
        DM_GETDEFID = WM_USER + 0,

        /// <summary>
        /// An application sends a DM_SETDEFID message to change the
        /// identifier of the default push button for a dialog box.
        /// </summary>
        DM_SETDEFID = WM_USER + 1,

        /// <summary>
        /// The DM_REPOSITION message repositions a top-level dialog
        /// box so that it fits within the desktop area. An application
        /// can send this message to a dialog box after resizing it to
        /// ensure that the entire dialog box remains visible.
        /// </summary>
        DM_REPOSITION = WM_USER + 2,

        #endregion

        #region LISTBOX control messages

        /// <summary>
        /// An application sends an LB_ADDSTRING message to add a
        /// string to a list box. If the list box does not have the
        /// LBS_SORT style, the string is added to the end of the list.
        /// Otherwise, the string is inserted into the list and the list
        /// is sorted.
        /// </summary>
        LB_ADDSTRING = 0x0180,

        /// <summary>
        /// An application sends an LB_INSERTSTRING message to insert
        /// a string into a list box. Unlike the LB_ADDSTRING message,
        /// the LB_INSERTSTRING message does not cause a list with the
        /// LBS_SORT style to be sorted.
        /// </summary>
        LB_INSERTSTRING = 0x0181,

        /// <summary>
        /// An application sends an LB_DELETESTRING message to delete
        /// a string in a list box.
        /// </summary>
        LB_DELETESTRING = 0x0182,

        /// <summary>
        /// An application sends an LB_SELITEMRANGEEX message to select
        /// one or more consecutive items in a multiple-selection list
        /// box.
        /// </summary>
        LB_SELITEMRANGEEX = 0x0183,

        /// <summary>
        /// An application sends an LB_RESETCONTENT message to remove
        /// all items from a list box.
        /// </summary>
        LB_RESETCONTENT = 0x0184,

        /// <summary>
        /// An application sends an LB_SETSEL message to select a string
        /// in a multiple-selection list box.
        /// </summary>
        LB_SETSEL = 0x0185,

        /// <summary>
        /// An application sends an LB_SETCURSEL message to select a
        /// string and scroll it into view, if necessary. When the new
        /// string is selected, the list box removes the highlight from
        /// the previously selected string.
        /// </summary>
        LB_SETCURSEL = 0x0186,

        /// <summary>
        /// An application sends an LB_GETSEL message to retrieve the
        /// selection state of an item.
        /// </summary>
        LB_GETSEL = 0x0187,

        /// <summary>
        /// Send an LB_GETCURSEL message to retrieve the index of the
        /// currently selected item, if any, in a single-selection list
        /// box.
        /// </summary>
        LB_GETCURSEL = 0x0188,

        /// <summary>
        /// An application sends an LB_GETTEXT message to retrieve a
        /// string from a list box.
        /// </summary>
        LB_GETTEXT = 0x0189,

        /// <summary>
        /// An application sends an LB_GETTEXTLEN message to retrieve
        /// the length of a string in a list box.
        /// </summary>
        LB_GETTEXTLEN = 0x018A,

        /// <summary>
        /// An application sends an LB_GETCOUNT message to retrieve the
        /// number of items in a list box.
        /// </summary>
        LB_GETCOUNT = 0x018B,

        /// <summary>
        /// An application sends an LB_SELECTSTRING message to search
        /// a list box for an item that begins with the characters in a
        /// specified string. If a matching item is found, the item is
        /// selected.
        /// </summary>
        LB_SELECTSTRING = 0x018C,

        /// <summary>
        /// An application sends an LB_DIR message to a list box to add
        /// names to the list displayed by the list box. The message
        /// adds the names of directories and files that match a
        /// specified string and set of file attributes. LB_DIR can also
        /// add mapped drive letters to the list box.
        /// </summary>
        LB_DIR = 0x018D,

        /// <summary>
        /// An application sends an LB_GETTOPINDEX message to retrieve
        /// the index of the first visible item in a list box. Initially
        /// the item with index 0 is at the top of the list box, but if
        /// the list box contents have been scrolled another item may
        /// be at the top.
        /// </summary>
        LB_GETTOPINDEX = 0x018E,

        /// <summary>
        /// An application sends an LB_FINDSTRING message to find the
        /// first string in a list box that begins with the specified
        /// string.
        /// </summary>
        LB_FINDSTRING = 0x018F,

        /// <summary>
        /// An application sends an LB_GETSELCOUNT message to retrieve
        /// the total number of selected items in a multiple-selection
        /// list box.
        /// </summary>
        LB_GETSELCOUNT = 0x0190,

        /// <summary>
        /// An application sends an LB_GETSELITEMS message to fill a
        /// buffer with an array of integers that specify the item
        /// numbers of selected items in a multiple-selection list box.
        /// </summary>
        LB_GETSELITEMS = 0x0191,

        /// <summary>
        /// An application sends an LB_SETTABSTOPS message to set the
        /// tab-stop positions in a list box.
        /// </summary>
        LB_SETTABSTOPS = 0x0192,

        /// <summary>
        /// An application sends an LB_GETHORIZONTALEXTENT message to
        /// retrieve from a list box the width, in pixels, by which the
        /// list box can be scrolled horizontally (the scrollable width)
        /// if the list box has a horizontal scroll bar.
        /// </summary>
        LB_GETHORIZONTALEXTENT = 0x0193,

        /// <summary>
        /// An application sends an LB_SETHORIZONTALEXTENT message
        /// to set the width, in pixels, by which a list box can be
        /// scrolled horizontally (the scrollable width). If the width
        /// of the list box is smaller than this value, the horizontal
        /// scroll bar horizontally scrolls items in the list box. If
        /// the width of the list box is equal to or greater than this
        /// value, the horizontal scroll bar is hidden.
        /// </summary>
        LB_SETHORIZONTALEXTENT = 0x0194,

        /// <summary>
        /// An application sends an LB_SETCOLUMNWIDTH message to a
        /// multiple-column list box (created with the LBS_MULTICOLUMN
        /// style) to set the width, in pixels, of all columns in the
        /// list box.
        /// </summary>
        LB_SETCOLUMNWIDTH = 0x0195,

        /// <summary>
        ///  application sends an LB_ADDFILE message to add the
        /// specified filename to a list box that contains a directory
        /// listing.
        /// </summary>
        LB_ADDFILE = 0x0196,

        /// <summary>
        /// An application sends an LB_SETTOPINDEX message to ensure that
        /// a particular item in a list box is visible.
        /// </summary>
        LB_SETTOPINDEX = 0x0197,

        /// <summary>
        /// An application sends an LB_GETITEMRECT message to retrieve
        /// the dimensions of the rectangle that bounds a list box item
        /// as it is currently displayed in the list box.
        /// </summary>
        LB_GETITEMRECT = 0x0198,

        /// <summary>
        /// An application sends an LB_GETITEMDATA message to retrieve
        /// the application-defined value associated with the specified
        /// list box item.
        /// </summary>
        LB_GETITEMDATA = 0x0199,

        /// <summary>
        /// An application sends an LB_SETITEMDATA message to set a value
        /// associated with the specified item in a list box.
        /// </summary>
        LB_SETITEMDATA = 0x019A,

        /// <summary>
        /// An application sends an LB_SELITEMRANGE message to select
        /// one or more consecutive items in a multiple-selection list
        /// box.
        /// </summary>
        LB_SELITEMRANGE = 0x019B,

        /// <summary>
        /// An application sends an LB_SETANCHORINDEX message to set
        /// the anchor item—that is, the item from which a multiple
        /// selection starts. A multiple selection spans all items from
        /// the anchor item to the caret item.
        /// </summary>
        LB_SETANCHORINDEX = 0x019C,

        /// <summary>
        /// An application sends an LB_GETANCHORINDEX message to retrieve
        /// the index of the anchor item—that is, the item from which
        /// a multiple selection starts. A multiple selection spans all
        /// items from the anchor item to the caret item.
        /// </summary>
        LB_GETANCHORINDEX = 0x019D,

        /// <summary>
        /// An application sends an LB_SETCARETINDEX message to set the
        /// focus rectangle to the item at the specified index in a
        /// multiple-selection list box. If the item is not visible,
        /// it is scrolled into view.
        /// </summary>
        LB_SETCARETINDEX = 0x019E,

        /// <summary>
        /// An application sends an LB_GETCARETINDEX message to determine
        /// the index of the item that has the focus rectangle in a
        /// multiple-selection list box. The item may or may not be
        /// selected.
        /// </summary>
        LB_GETCARETINDEX = 0x019F,

        /// <summary>
        /// An application sends an LB_SETITEMHEIGHT message to set
        /// the height, in pixels, of items in a list box. If the list
        /// box has the LBS_OWNERDRAWVARIABLE style, this message sets
        /// the height of the item specified by the wParam parameter.
        /// Otherwise, this message sets the height of all items in the
        /// list box.
        /// </summary>
        LB_SETITEMHEIGHT = 0x01A0,

        /// <summary>
        /// An application sends an LB_GETITEMHEIGHT message to retrieve
        /// the height of items in a list box.
        /// </summary>
        LB_GETITEMHEIGHT = 0x01A1,

        /// <summary>
        /// An application sends a LB_FINDSTRINGEXACT message to find
        /// the first list box string that exactly matches the specified
        /// string, except that the search is not case sensitive.
        /// </summary>
        LB_FINDSTRINGEXACT = 0x01A2,

        /// <summary>
        /// An application sends an LB_SETLOCALE message to set the
        /// current locale of the list box. You can use the locale to
        /// determine the correct sorting order of displayed text (for
        /// list boxes with the LBS_SORT style) and of text added by
        /// the LB_ADDSTRING message.
        /// </summary>
        LB_SETLOCALE = 0x01A5,

        /// <summary>
        /// An application sends an LB_GETLOCALE message to retrieve
        /// the current locale of the list box. You can use the locale
        /// to determine the correct sorting order of displayed text
        /// (for list boxes with the LBS_SORT style) and of text added
        /// by the LB_ADDSTRING message.
        /// </summary>
        LB_GETLOCALE = 0x01A6,

        /// <summary>
        /// An application sends an LB_SETCOUNT message to set the
        /// count of items in a list box created with the LBS_NODATA
        /// style and not created with the LBS_HASSTRINGS style.
        /// </summary>
        LB_SETCOUNT = 0x01A7,

        /// <summary>
        /// An application sends the LB_INITSTORAGE message before
        /// adding a large number of items to a list box. This message
        /// allocates memory for storing list box items.
        /// </summary>
        LB_INITSTORAGE = 0x01A8,

        /// <summary>
        /// An application sends the LB_ITEMFROMPOINT message to
        /// retrieve the zero-based index of the item nearest the
        /// specified point in a list box.
        /// </summary>
        LB_ITEMFROMPOINT = 0x01A9,

        /// <summary>
        /// ???
        /// </summary>
        LB_MULTIPLEADDSTRING = 0x01B1,

        /// <summary>
        /// An application sends an LB_GETLISTBOXINFO message to
        /// retrieve the number of items per column in a specified
        /// list box.
        /// </summary>
        LB_GETLISTBOXINFO = 0x01B2,

        /// <summary>
        /// ???
        /// </summary>
        LB_MSGMAX_501 = 0x01B3,

        /// <summary>
        /// ???
        /// </summary>
        LB_MSGMAX_WCE4 = 0x01B1,

        /// <summary>
        /// ???
        /// </summary>
        LB_MSGMAX_4 = 0x01B0,

        /// <summary>
        /// ???
        /// </summary>
        LB_MSGMAX_PRE4 = 0x01A8,

        #endregion

        #region COMBOBOX control messages

        /// <summary>
        /// An application sends a CB_GETEDITSEL message to get the
        /// starting and ending character positions of the current
        /// selection in the edit control of a combo box.
        /// </summary>
        CB_GETEDITSEL = 0x0140,

        /// <summary>
        /// An application sends a CB_LIMITTEXT message to limit the
        /// length of the text the user may type into the edit control
        /// of a combo box.
        /// </summary>
        CB_LIMITTEXT = 0x0141,

        /// <summary>
        /// An application sends a CB_SETEDITSEL message to select
        /// characters in the edit control of a combo box.
        /// </summary>
        CB_SETEDITSEL = 0x0142,

        /// <summary>
        /// An application sends a CB_ADDSTRING message to add a string
        /// to the list box of a combo box. If the combo box does not
        /// have the CBS_SORT style, the string is added to the end of
        /// the list. Otherwise, the string is inserted into the list,
        /// and the list is sorted.
        /// </summary>
        CB_ADDSTRING = 0x0143,

        /// <summary>
        /// An application sends a CB_DELETESTRING message to delete
        /// a string in the list box of a combo box.
        /// </summary>
        CB_DELETESTRING = 0x0144,

        /// <summary>
        /// An application sends a CB_DIR message to a combo box to add
        /// names to the list displayed by the combo box. The message
        /// adds the names of directories and files that match a
        /// specified string and set of file attributes. CB_DIR can
        /// also add mapped drive letters to the list.
        /// </summary>
        CB_DIR = 0x0145,

        /// <summary>
        /// An application sends a CB_GETCOUNT message to retrieve the
        /// number of items in the list box of a combo box.
        /// </summary>
        CB_GETCOUNT = 0x0146,

        /// <summary>
        /// An application sends a CB_GETCURSEL message to retrieve the
        /// index of the currently selected item, if any, in the list
        /// box of a combo box.
        /// </summary>
        CB_GETCURSEL = 0x0147,

        /// <summary>
        /// An application sends a CB_GETLBTEXT message to retrieve
        /// a string from the list of a combo box.
        /// </summary>
        CB_GETLBTEXT = 0x0148,

        /// <summary>
        /// An application sends a CB_GETLBTEXTLEN message to retrieve
        /// the length, in characters, of a string in the list of a
        /// combo box.
        /// </summary>
        CB_GETLBTEXTLEN = 0x0149,

        /// <summary>
        /// An application sends a CB_INSERTSTRING message to insert
        /// a string into the list box of a combo box. Unlike the
        /// CB_ADDSTRING message, the CB_INSERTSTRING message does not
        /// cause a list with the CBS_SORT style to be sorted.
        /// </summary>
        CB_INSERTSTRING = 0x014A,

        /// <summary>
        /// An application sends a CB_RESETCONTENT message to remove
        /// all items from the list box and edit control of a combo box.
        /// </summary>
        CB_RESETCONTENT = 0x014B,

        /// <summary>
        /// An application sends a CB_FINDSTRING message to search the
        /// list box of a combo box for an item beginning with the
        /// characters in a specified string.
        /// </summary>
        CB_FINDSTRING = 0x014C,

        /// <summary>
        /// An application sends a CB_SELECTSTRING message to search
        /// the list of a combo box for an item that begins with the
        /// characters in a specified string. If a matching item is
        /// found, it is selected and copied to the edit control.
        /// </summary>
        CB_SELECTSTRING = 0x014D,

        /// <summary>
        /// An application sends a CB_SETCURSEL message to select
        /// a string in the list of a combo box. If necessary, the
        /// list scrolls the string into view. The text in the edit
        /// control of the combo box changes to reflect the new
        /// selection, and any previous selection in the list is
        /// removed.
        /// </summary>
        CB_SETCURSEL = 0x014E,

        /// <summary>
        /// An application sends a CB_SHOWDROPDOWN message to show or
        /// hide the list box of a combo box that has the CBS_DROPDOWN
        /// or CBS_DROPDOWNLIST style.
        /// </summary>
        CB_SHOWDROPDOWN = 0x014F,

        /// <summary>
        /// An application sends a CB_GETITEMDATA message to a combo
        /// box to retrieve the application-supplied value associated
        /// with the specified item in the combo box.
        /// </summary>
        CB_GETITEMDATA = 0x0150,

        /// <summary>
        /// An application sends a CB_SETITEMDATA message to set the
        /// value associated with the specified item in a combo box.
        /// </summary>
        CB_SETITEMDATA = 0x0151,

        /// <summary>
        /// An application sends a CB_GETDROPPEDCONTROLRECT message to
        /// retrieve the screen coordinates of a combo box in its
        /// dropped-down state.
        /// </summary>
        CB_GETDROPPEDCONTROLRECT = 0x0152,

        /// <summary>
        /// An application sends a CB_SETITEMHEIGHT message to set
        /// the height of list items or the selection field in a combo
        /// box.
        /// </summary>
        CB_SETITEMHEIGHT = 0x0153,

        /// <summary>
        /// An application sends a CB_GETITEMHEIGHT message to
        /// determine the height of list items or the selection field
        /// in a combo box.
        /// </summary>
        CB_GETITEMHEIGHT = 0x0154,

        /// <summary>
        /// An application sends a CB_SETEXTENDEDUI message to select
        /// either the default user interface or the extended user
        /// interface for a combo box that has the CBS_DROPDOWN or
        /// CBS_DROPDOWNLIST style.
        /// </summary>
        CB_SETEXTENDEDUI = 0x0155,

        /// <summary>
        /// An application sends a CB_GETEXTENDEDUI message to
        /// determine whether a combo box has the default user
        /// interface or the extended user interface.
        /// </summary>
        CB_GETEXTENDEDUI = 0x0156,

        /// <summary>
        /// An application sends a CB_GETDROPPEDSTATE message to
        /// determine whether the list box of a combo box is dropped
        /// down.
        /// </summary>
        CB_GETDROPPEDSTATE = 0x0157,

        /// <summary>
        /// An application sends a CB_FINDSTRINGEXACT message to find
        /// the first list box string in a combo box that matches the
        /// string specified in the lParam parameter.
        /// </summary>
        CB_FINDSTRINGEXACT = 0x0158,

        /// <summary>
        /// An application sends a CB_SETLOCALE message to set the
        /// current locale of the combo box. If the combo box has the
        /// CBS_SORT style and strings are added using CB_ADDSTRING,
        /// the locale of a combo box affects how list items are sorted.
        /// </summary>
        CB_SETLOCALE = 0x0159,

        /// <summary>
        /// An application sends a CB_GETLOCALE message to retrieve
        /// the current locale of the combo box. The locale is used
        /// to determine the correct sorting order of displayed text
        /// for combo boxes with the CBS_SORT style and text added
        /// by using the CB_ADDSTRING message.
        /// </summary>
        CB_GETLOCALE = 0x015A,

        /// <summary>
        /// An application sends the CB_GETTOPINDEX message to retrieve
        /// the zero-based index of the first visible item in the list
        /// box portion of a combo box. Initially, the item with index
        /// 0 is at the top of the list box, but if the list box
        /// contents have been scrolled, another item may be at the top.
        /// </summary>
        CB_GETTOPINDEX = 0x015B,

        /// <summary>
        /// An application sends the CB_SETTOPINDEX message to ensure
        /// that a particular item is visible in the list box of a combo
        /// box. The system scrolls the list box contents so that either
        /// the specified item appears at the top of the list box or the
        /// maximum scroll range has been reached.
        /// </summary>
        CB_SETTOPINDEX = 0x015C,

        /// <summary>
        /// An application sends the CB_GETHORIZONTALEXTENT message to
        /// retrieve from a combo box the width, in pixels, by which
        /// the list box can be scrolled horizontally (the scrollable
        /// width). This is applicable only if the list box has a
        /// horizontal scroll bar.
        /// </summary>
        CB_GETHORIZONTALEXTENT = 0x015d,

        /// <summary>
        /// An application sends the CB_SETHORIZONTALEXTENT message
        /// to set the width, in pixels, by which a list box can be
        /// scrolled horizontally (the scrollable width). If the width
        /// of the list box is smaller than this value, the horizontal
        /// scroll bar horizontally scrolls items in the list box.
        /// If the width of the list box is equal to or greater than
        /// this value, the horizontal scroll bar is hidden or, if the
        /// combo box has the CBS_DISABLENOSCROLL style, disabled.
        /// </summary>
        CB_SETHORIZONTALEXTENT = 0x015e,

        /// <summary>
        /// An application sends the CB_GETDROPPEDWIDTH message to
        /// retrieve the minimum allowable width, in pixels, of the
        /// list box of a combo box with the CBS_DROPDOWN or
        /// CBS_DROPDOWNLIST style.
        /// </summary>
        CB_GETDROPPEDWIDTH = 0x015f,

        /// <summary>
        /// An application sends the CB_SETDROPPEDWIDTH message to
        /// set the maximum allowable width, in pixels, of the list
        /// box of a combo box with the CBS_DROPDOWN or CBS_DROPDOWNLIST
        /// style.
        /// </summary>
        CB_SETDROPPEDWIDTH = 0x0160,

        /// <summary>
        /// An application sends the CB_INITSTORAGE message before
        /// adding a large number of items to the list box portion
        /// of a combo box. This message allocates memory for storing
        /// list box items.
        /// </summary>
        CB_INITSTORAGE = 0x0161,

        /// <summary>
        /// ???
        /// </summary>
        CB_MULTIPLEADDSTRING = 0x0163,

        /// <summary>
        /// An application sends the CB_GETCOMBOBOXINFO message to
        /// retrieve information about the specified combo box.
        /// </summary>
        CB_GETCOMBOBOXINFO = 0x0164,

        /// <summary>
        /// ???
        /// </summary>
        CB_MSGMAX_501 = 0x0165,

        /// <summary>
        /// ???
        /// </summary>
        CB_MSGMAX_WCE400 = 0x0163,

        /// <summary>
        /// ???
        /// </summary>
        CB_MSGMAX_400 = 0x0162,

        /// <summary>
        /// ???
        /// </summary>
        CB_MSGMAX_PRE400 = 0x015B,

        #endregion

        #region SCROLLBAR control messages

        /// <summary>
        /// <para>The SBM_SETPOS message is sent to set the position
        /// of the scroll box (thumb) and, if requested, redraw the
        /// scroll bar to reflect the new position of the scroll box.
        /// </para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the SetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the SetScrollPos function
        /// to work properly.</para>
        /// </summary>
        SBM_SETPOS = 0x00E0,

        /// <summary>
        /// <para>The SBM_GETPOS message is sent to retrieve the current
        /// position of the scroll box of a scroll bar control. The
        /// current position is a relative value that depends on the
        /// current scrolling range. For example, if the scrolling range
        /// is 0 through 100 and the scroll box is in the middle of the
        /// bar, the current position is 50.</para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the GetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the GetScrollPos function
        /// to function properly.</para>
        /// </summary>
        SBM_GETPOS = 0x00E1,

        /// <summary>
        /// <para>The SBM_SETRANGE message is sent to set the minimum
        /// and maximum position values for the scroll bar control.
        /// </para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the GetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the GetScrollPos function
        /// to function properly.</para>
        /// </summary>
        SBM_SETRANGE = 0x00E2,

        /// <summary>
        /// An application sends the SBM_SETRANGEREDRAW message to a
        /// scroll bar control to set the minimum and maximum position
        /// values and to redraw the control.
        /// </summary>
        SBM_SETRANGEREDRAW = 0x00E6,

        /// <summary>
        /// <para>The SBM_GETRANGE message is sent to retrieve the
        /// minimum and maximum position values for the scroll bar
        /// control.</para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the GetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the GetScrollPos function
        /// to function properly.</para>
        /// </summary>
        SBM_GETRANGE = 0x00E3,

        /// <summary>
        /// An application sends the SBM_ENABLE_ARROWS message to enable
        /// or disable one or both arrows of a scroll bar control.
        /// </summary>
        SBM_ENABLE_ARROWS = 0x00E4,

        /// <summary>
        /// <para>The SBM_SETSCROLLINFO message is sent to set the
        /// parameters of a scroll bar.</para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the GetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the GetScrollPos function
        /// to function properly.</para>
        /// </summary>
        SBM_SETSCROLLINFO = 0x00E9,

        /// <summary>
        /// <para>The SBM_GETSCROLLINFO message is sent to retrieve the
        /// parameters of a scroll bar.</para>
        /// <para>Applications should not send this message directly.
        /// Instead, they should use the GetScrollPos function. A window
        /// receives this message through its WindowProc function.
        /// Applications which implement a custom scroll bar control
        /// must respond to these messages for the GetScrollPos function
        /// to function properly.</para>
        /// </summary>
        SBM_GETSCROLLINFO = 0x00EA,

        /// <summary>
        /// An application sends the SBM_GETSCROLLBARINFO message to
        /// retrieve information about the specified scroll bar.
        /// </summary>
        SBM_GETSCROLLBARINFO = 0x00EB,

        #endregion

        /// <summary>
        /// ListView messages.
        /// </summary>
        LVM_FIRST = 0x1000,

        /// <summary>
        /// TreeView messages.
        /// </summary>
        TV_FIRST = 0x1100,

        /// <summary>
        ///
        /// </summary>
        HDM_FIRST = 0x1200,

        /// <summary>
        ///
        /// </summary>
        TCM_FIRST = 0x1300, // Tab control messages

        /// <summary>
        ///
        /// </summary>
        PGM_FIRST = 0x1400, // Pager control messages

        /// <summary>
        ///
        /// </summary>
        ECM_FIRST = 0x1500, // Edit control messages

        /// <summary>
        ///
        /// </summary>
        BCM_FIRST = 0x1600, // Button control messages

        /// <summary>
        ///
        /// </summary>
        CBM_FIRST = 0x1700, // Combobox control messages

        /// <summary>
        ///
        /// </summary>
        CCM_FIRST = 0x2000, // Common control shared messages

        /// <summary>
        ///
        /// </summary>
        CCM_LAST = CCM_FIRST + 0x200,

        /// <summary>
        ///
        /// </summary>
        CCM_SETBKCOLOR = CCM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        CCM_SETCOLORSCHEME = CCM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        CCM_GETCOLORSCHEME = CCM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        CCM_GETDROPTARGET = CCM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        CCM_SETUNICODEFORMAT = CCM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        CCM_GETUNICODEFORMAT = CCM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        CCM_SETVERSION = CCM_FIRST + 0x7,

        /// <summary>
        ///
        /// </summary>
        CCM_GETVERSION = CCM_FIRST + 0x8,

        /// <summary>
        ///
        /// </summary>
        CCM_SETNOTIFYWINDOW = CCM_FIRST + 0x9,

        /// <summary>
        ///
        /// </summary>
        CCM_SETWINDOWTHEME = CCM_FIRST + 0xb,

        /// <summary>
        ///
        /// </summary>
        CCM_DPISCALE = CCM_FIRST + 0xc,

        /// <summary>
        ///
        /// </summary>
        HDM_GETITEMCOUNT = HDM_FIRST + 0,

        /// <summary>
        ///
        /// </summary>
        HDM_INSERTITEMA = HDM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        HDM_INSERTITEMW = HDM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        HDM_DELETEITEM = HDM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        HDM_GETITEMA = HDM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        HDM_GETITEMW = HDM_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        HDM_SETITEMA = HDM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        HDM_SETITEMW = HDM_FIRST + 12,

        /// <summary>
        ///
        /// </summary>
        HDM_LAYOUT = HDM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        HDM_HITTEST = HDM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        HDM_GETITEMRECT = HDM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        HDM_SETIMAGELIST = HDM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        HDM_GETIMAGELIST = HDM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        HDM_ORDERTOINDEX = HDM_FIRST + 15,

        /// <summary>
        ///
        /// </summary>
        HDM_CREATEDRAGIMAGE = HDM_FIRST + 16,

        /// <summary>
        ///
        /// </summary>
        HDM_GETORDERARRAY = HDM_FIRST + 17,

        /// <summary>
        ///
        /// </summary>
        HDM_SETORDERARRAY = HDM_FIRST + 18,

        /// <summary>
        ///
        /// </summary>
        HDM_SETHOTDIVIDER = HDM_FIRST + 19,

        /// <summary>
        ///
        /// </summary>
        HDM_SETBITMAPMARGIN = HDM_FIRST + 20,

        /// <summary>
        ///
        /// </summary>
        HDM_GETBITMAPMARGIN = HDM_FIRST + 21,

        /// <summary>
        ///
        /// </summary>
        HDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        HDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        HDM_SETFILTERCHANGETIMEOUT = HDM_FIRST + 22,

        /// <summary>
        ///
        /// </summary>
        HDM_EDITFILTER = HDM_FIRST + 23,

        /// <summary>
        ///
        /// </summary>
        HDM_CLEARFILTER = HDM_FIRST + 24,

        /// <summary>
        ///
        /// </summary>
        TB_ENABLEBUTTON = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        TB_CHECKBUTTON = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        TB_PRESSBUTTON = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        TB_HIDEBUTTON = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        TB_INDETERMINATE = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        TB_MARKBUTTON = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONENABLED = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONCHECKED = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONPRESSED = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONHIDDEN = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONINDETERMINATE = WM_USER + 13,

        /// <summary>
        ///
        /// </summary>
        TB_ISBUTTONHIGHLIGHTED = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        TB_SETSTATE = WM_USER + 17,

        /// <summary>
        ///
        /// </summary>
        TB_GETSTATE = WM_USER + 18,

        /// <summary>
        ///
        /// </summary>
        TB_ADDBITMAP = WM_USER + 19,

        /// <summary>
        ///
        /// </summary>
        TB_ADDBUTTONSA = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        TB_INSERTBUTTONA = WM_USER + 21,

        /// <summary>
        ///
        /// </summary>
        TB_ADDBUTTONS = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        TB_INSERTBUTTON = WM_USER + 21,

        /// <summary>
        ///
        /// </summary>
        TB_DELETEBUTTON = WM_USER + 22,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTON = WM_USER + 23,

        /// <summary>
        ///
        /// </summary>
        TB_BUTTONCOUNT = WM_USER + 24,

        /// <summary>
        ///
        /// </summary>
        TB_COMMANDTOINDEX = WM_USER + 25,

        /// <summary>
        ///
        /// </summary>
        TB_SAVERESTOREA = WM_USER + 26,

        /// <summary>
        ///
        /// </summary>
        TB_SAVERESTOREW = WM_USER + 76,

        /// <summary>
        ///
        /// </summary>
        TB_CUSTOMIZE = WM_USER + 27,

        /// <summary>
        ///
        /// </summary>
        TB_ADDSTRINGA = WM_USER + 28,

        /// <summary>
        ///
        /// </summary>
        TB_ADDSTRINGW = WM_USER + 77,

        /// <summary>
        ///
        /// </summary>
        TB_GETITEMRECT = WM_USER + 29,

        /// <summary>
        ///
        /// </summary>
        TB_BUTTONSTRUCTSIZE = WM_USER + 30,

        /// <summary>
        ///
        /// </summary>
        TB_SETBUTTONSIZE = WM_USER + 31,

        /// <summary>
        ///
        /// </summary>
        TB_SETBITMAPSIZE = WM_USER + 32,

        /// <summary>
        ///
        /// </summary>
        TB_AUTOSIZE = WM_USER + 33,

        /// <summary>
        ///
        /// </summary>
        TB_GETTOOLTIPS = WM_USER + 35,

        /// <summary>
        ///
        /// </summary>
        TB_SETTOOLTIPS = WM_USER + 36,

        /// <summary>
        ///
        /// </summary>
        TB_SETPARENT = WM_USER + 37,

        /// <summary>
        ///
        /// </summary>
        TB_SETROWS = WM_USER + 39,

        /// <summary>
        ///
        /// </summary>
        TB_GETROWS = WM_USER + 40,

        /// <summary>
        ///
        /// </summary>
        TB_SETCMDID = WM_USER + 42,

        /// <summary>
        ///
        /// </summary>
        TB_CHANGEBITMAP = WM_USER + 43,

        /// <summary>
        ///
        /// </summary>
        TB_GETBITMAP = WM_USER + 44,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTONTEXTA = WM_USER + 45,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTONTEXTW = WM_USER + 75,

        /// <summary>
        ///
        /// </summary>
        TB_REPLACEBITMAP = WM_USER + 46,

        /// <summary>
        ///
        /// </summary>
        TB_SETINDENT = WM_USER + 47,

        /// <summary>
        ///
        /// </summary>
        TB_SETIMAGELIST = WM_USER + 48,

        /// <summary>
        ///
        /// </summary>
        TB_GETIMAGELIST = WM_USER + 49,

        /// <summary>
        ///
        /// </summary>
        TB_LOADIMAGES = WM_USER + 50,

        /// <summary>
        ///
        /// </summary>
        TB_GETRECT = WM_USER + 51,

        /// <summary>
        ///
        /// </summary>
        TB_SETHOTIMAGELIST = WM_USER + 52,

        /// <summary>
        ///
        /// </summary>
        TB_GETHOTIMAGELIST = WM_USER + 53,

        /// <summary>
        ///
        /// </summary>
        TB_SETDISABLEDIMAGELIST = WM_USER + 54,

        /// <summary>
        ///
        /// </summary>
        TB_GETDISABLEDIMAGELIST = WM_USER + 55,

        /// <summary>
        ///
        /// </summary>
        TB_SETSTYLE = WM_USER + 56,

        /// <summary>
        ///
        /// </summary>
        TB_GETSTYLE = WM_USER + 57,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTONSIZE = WM_USER + 58,

        /// <summary>
        ///
        /// </summary>
        TB_SETBUTTONWIDTH = WM_USER + 59,

        /// <summary>
        ///
        /// </summary>
        TB_SETMAXTEXTROWS = WM_USER + 60,

        /// <summary>
        ///
        /// </summary>
        TB_GETTEXTROWS = WM_USER + 61,

        /// <summary>
        ///
        /// </summary>
        TB_GETOBJECT = WM_USER + 62,

        /// <summary>
        ///
        /// </summary>
        TB_GETHOTITEM = WM_USER + 71,

        /// <summary>
        ///
        /// </summary>
        TB_SETHOTITEM = WM_USER + 72,

        /// <summary>
        ///
        /// </summary>
        TB_SETANCHORHIGHLIGHT = WM_USER + 73,

        /// <summary>
        ///
        /// </summary>
        TB_GETANCHORHIGHLIGHT = WM_USER + 74,

        /// <summary>
        ///
        /// </summary>
        TB_MAPACCELERATORA = WM_USER + 78,

        /// <summary>
        ///
        /// </summary>
        TB_GETINSERTMARK = WM_USER + 79,

        /// <summary>
        ///
        /// </summary>
        TB_SETINSERTMARK = WM_USER + 80,

        /// <summary>
        ///
        /// </summary>
        TB_INSERTMARKHITTEST = WM_USER + 81,

        /// <summary>
        ///
        /// </summary>
        TB_MOVEBUTTON = WM_USER + 82,

        /// <summary>
        ///
        /// </summary>
        TB_GETMAXSIZE = WM_USER + 83,

        /// <summary>
        ///
        /// </summary>
        TB_SETEXTENDEDSTYLE = WM_USER + 84,

        /// <summary>
        ///
        /// </summary>
        TB_GETEXTENDEDSTYLE = WM_USER + 85,

        /// <summary>
        ///
        /// </summary>
        TB_GETPADDING = WM_USER + 86,

        /// <summary>
        ///
        /// </summary>
        TB_SETPADDING = WM_USER + 87,

        /// <summary>
        ///
        /// </summary>
        TB_SETINSERTMARKCOLOR = WM_USER + 88,

        /// <summary>
        ///
        /// </summary>
        TB_GETINSERTMARKCOLOR = WM_USER + 89,

        /// <summary>
        ///
        /// </summary>
        TB_SETCOLORSCHEME = CCM_SETCOLORSCHEME,

        /// <summary>
        ///
        /// </summary>
        TB_GETCOLORSCHEME = CCM_GETCOLORSCHEME,

        /// <summary>
        ///
        /// </summary>
        TB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TB_MAPACCELERATORW = WM_USER + 90,

        /// <summary>
        ///
        /// </summary>
        TB_GETBITMAPFLAGS = WM_USER + 41,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTONINFOW = WM_USER + 63,

        /// <summary>
        ///
        /// </summary>
        TB_SETBUTTONINFOW = WM_USER + 64,

        /// <summary>
        ///
        /// </summary>
        TB_GETBUTTONINFOA = WM_USER + 65,

        /// <summary>
        ///
        /// </summary>
        TB_SETBUTTONINFOA = WM_USER + 66,

        /// <summary>
        ///
        /// </summary>
        TB_INSERTBUTTONW = WM_USER + 67,

        /// <summary>
        ///
        /// </summary>
        TB_ADDBUTTONSW = WM_USER + 68,

        /// <summary>
        ///
        /// </summary>
        TB_HITTEST = WM_USER + 69,

        /// <summary>
        ///
        /// </summary>
        TB_SETDRAWTEXTFLAGS = WM_USER + 70,

        /// <summary>
        ///
        /// </summary>
        TB_GETSTRINGW = WM_USER + 91,

        /// <summary>
        ///
        /// </summary>
        TB_GETSTRINGA = WM_USER + 92,

        /// <summary>
        ///
        /// </summary>
        TB_GETMETRICS = WM_USER + 101,

        /// <summary>
        ///
        /// </summary>
        TB_SETMETRICS = WM_USER + 102,

        /// <summary>
        ///
        /// </summary>
        TB_SETWINDOWTHEME = CCM_SETWINDOWTHEME,

        /// <summary>
        ///
        /// </summary>
        RB_INSERTBANDA = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        RB_DELETEBAND = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        RB_GETBARINFO = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        RB_SETBARINFO = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDINFO = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        RB_SETBANDINFOA = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        RB_SETPARENT = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        RB_HITTEST = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        RB_GETRECT = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        RB_INSERTBANDW = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        RB_SETBANDINFOW = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDCOUNT = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        RB_GETROWCOUNT = WM_USER + 13,

        /// <summary>
        ///
        /// </summary>
        RB_GETROWHEIGHT = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        RB_IDTOINDEX = WM_USER + 16,

        /// <summary>
        ///
        /// </summary>
        RB_GETTOOLTIPS = WM_USER + 17,

        /// <summary>
        ///
        /// </summary>
        RB_SETTOOLTIPS = WM_USER + 18,

        /// <summary>
        ///
        /// </summary>
        RB_SETBKCOLOR = WM_USER + 19,

        /// <summary>
        ///
        /// </summary>
        RB_GETBKCOLOR = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        RB_SETTEXTCOLOR = WM_USER + 21,

        /// <summary>
        ///
        /// </summary>
        RB_GETTEXTCOLOR = WM_USER + 22,

        /// <summary>
        ///
        /// </summary>
        RB_SIZETORECT = WM_USER + 23,

        /// <summary>
        ///
        /// </summary>
        RB_SETCOLORSCHEME = CCM_SETCOLORSCHEME,

        /// <summary>
        ///
        /// </summary>
        RB_GETCOLORSCHEME = CCM_GETCOLORSCHEME,

        /// <summary>
        ///
        /// </summary>
        RB_BEGINDRAG = WM_USER + 24,

        /// <summary>
        ///
        /// </summary>
        RB_ENDDRAG = WM_USER + 25,

        /// <summary>
        ///
        /// </summary>
        RB_DRAGMOVE = WM_USER + 26,

        /// <summary>
        ///
        /// </summary>
        RB_GETBARHEIGHT = WM_USER + 27,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDINFOW = WM_USER + 28,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDINFOA = WM_USER + 29,

        /// <summary>
        ///
        /// </summary>
        RB_MINIMIZEBAND = WM_USER + 30,

        /// <summary>
        ///
        /// </summary>
        RB_MAXIMIZEBAND = WM_USER + 31,

        /// <summary>
        ///
        /// </summary>
        RB_GETDROPTARGET = CCM_GETDROPTARGET,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDBORDERS = WM_USER + 34,

        /// <summary>
        ///
        /// </summary>
        RB_SHOWBAND = WM_USER + 35,

        /// <summary>
        ///
        /// </summary>
        RB_SETPALETTE = WM_USER + 37,

        /// <summary>
        ///
        /// </summary>
        RB_GETPALETTE = WM_USER + 38,

        /// <summary>
        ///
        /// </summary>
        RB_MOVEBAND = WM_USER + 39,

        /// <summary>
        ///
        /// </summary>
        RB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        RB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        RB_GETBANDMARGINS = WM_USER + 40,

        /// <summary>
        ///
        /// </summary>
        RB_SETWINDOWTHEME = CCM_SETWINDOWTHEME,

        /// <summary>
        ///
        /// </summary>
        RB_PUSHCHEVRON = WM_USER + 43,

        /// <summary>
        ///
        /// </summary>
        TTM_ACTIVATE = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        TTM_SETDELAYTIME = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        TTM_ADDTOOLA = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        TTM_ADDTOOLW = WM_USER + 50,

        /// <summary>
        ///
        /// </summary>
        TTM_DELTOOLA = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        TTM_DELTOOLW = WM_USER + 51,

        /// <summary>
        ///
        /// </summary>
        TTM_NEWTOOLRECTA = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        TTM_NEWTOOLRECTW = WM_USER + 52,

        /// <summary>
        ///
        /// </summary>
        TTM_RELAYEVENT = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTOOLINFOA = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTOOLINFOW = WM_USER + 53,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTOOLINFOA = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTOOLINFOW = WM_USER + 54,

        /// <summary>
        ///
        /// </summary>
        TTM_HITTESTA = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        TTM_HITTESTW = WM_USER + 55,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTEXTA = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTEXTW = WM_USER + 56,

        /// <summary>
        ///
        /// </summary>
        TTM_UPDATETIPTEXTA = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        TTM_UPDATETIPTEXTW = WM_USER + 57,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTOOLCOUNT = WM_USER + 13,

        /// <summary>
        ///
        /// </summary>
        TTM_ENUMTOOLSA = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        TTM_ENUMTOOLSW = WM_USER + 58,

        /// <summary>
        ///
        /// </summary>
        TTM_GETCURRENTTOOLA = WM_USER + 15,

        /// <summary>
        ///
        /// </summary>
        TTM_GETCURRENTTOOLW = WM_USER + 59,

        /// <summary>
        ///
        /// </summary>
        TTM_WINDOWFROMPOINT = WM_USER + 16,

        /// <summary>
        ///
        /// </summary>
        TTM_TRACKACTIVATE = WM_USER + 17,

        /// <summary>
        ///
        /// </summary>
        TTM_TRACKPOSITION = WM_USER + 18,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTIPBKCOLOR = WM_USER + 19,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTIPTEXTCOLOR = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        TTM_GETDELAYTIME = WM_USER + 21,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTIPBKCOLOR = WM_USER + 22,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTIPTEXTCOLOR = WM_USER + 23,

        /// <summary>
        ///
        /// </summary>
        TTM_SETMAXTIPWIDTH = WM_USER + 24,

        /// <summary>
        ///
        /// </summary>
        TTM_GETMAXTIPWIDTH = WM_USER + 25,

        /// <summary>
        ///
        /// </summary>
        TTM_SETMARGIN = WM_USER + 26,

        /// <summary>
        ///
        /// </summary>
        TTM_GETMARGIN = WM_USER + 27,

        /// <summary>
        ///
        /// </summary>
        TTM_POP = WM_USER + 28,

        /// <summary>
        ///
        /// </summary>
        TTM_UPDATE = WM_USER + 29,

        /// <summary>
        ///
        /// </summary>
        TTM_GETBUBBLESIZE = WM_USER + 30,

        /// <summary>
        ///
        /// </summary>
        TTM_ADJUSTRECT = WM_USER + 31,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTITLEA = WM_USER + 32,

        /// <summary>
        ///
        /// </summary>
        TTM_SETTITLEW = WM_USER + 33,

        /// <summary>
        ///
        /// </summary>
        TTM_POPUP = WM_USER + 34,

        /// <summary>
        ///
        /// </summary>
        TTM_GETTITLE = WM_USER + 35,

        /// <summary>
        ///
        /// </summary>
        TTM_SETWINDOWTHEME = CCM_SETWINDOWTHEME,

        /// <summary>
        ///
        /// </summary>
        SB_SETTEXTA = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        SB_SETTEXTW = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        SB_GETTEXTA = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        SB_GETTEXTW = WM_USER + 13,

        /// <summary>
        ///
        /// </summary>
        SB_GETTEXTLENGTHA = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        SB_GETTEXTLENGTHW = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        SB_SETPARTS = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        SB_GETPARTS = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        SB_GETBORDERS = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        SB_SETMINHEIGHT = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        SB_SIMPLE = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        SB_GETRECT = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        SB_ISSIMPLE = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        SB_SETICON = WM_USER + 15,

        /// <summary>
        ///
        /// </summary>
        SB_SETTIPTEXTA = WM_USER + 16,

        /// <summary>
        ///
        /// </summary>
        SB_SETTIPTEXTW = WM_USER + 17,

        /// <summary>
        ///
        /// </summary>
        SB_GETTIPTEXTA = WM_USER + 18,

        /// <summary>
        ///
        /// </summary>
        SB_GETTIPTEXTW = WM_USER + 19,

        /// <summary>
        ///
        /// </summary>
        SB_GETICON = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        SB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        SB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        SB_SETBKCOLOR = CCM_SETBKCOLOR,

        /// <summary>
        ///
        /// </summary>
        SB_SIMPLEID = 0x00ff,

        /// <summary>
        ///
        /// </summary>
        TBM_GETPOS = WM_USER,

        /// <summary>
        ///
        /// </summary>
        TBM_GETRANGEMIN = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        TBM_GETRANGEMAX = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        TBM_GETTIC = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        TBM_SETTIC = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        TBM_SETPOS = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        TBM_SETRANGE = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        TBM_SETRANGEMIN = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        TBM_SETRANGEMAX = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        TBM_CLEARTICS = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        TBM_SETSEL = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        TBM_SETSELSTART = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        TBM_SETSELEND = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        TBM_GETPTICS = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        TBM_GETTICPOS = WM_USER + 15,

        /// <summary>
        ///
        /// </summary>
        TBM_GETNUMTICS = WM_USER + 16,

        /// <summary>
        ///
        /// </summary>
        TBM_GETSELSTART = WM_USER + 17,

        /// <summary>
        ///
        /// </summary>
        TBM_GETSELEND = WM_USER + 18,

        /// <summary>
        ///
        /// </summary>
        TBM_CLEARSEL = WM_USER + 19,

        /// <summary>
        ///
        /// </summary>
        TBM_SETTICFREQ = WM_USER + 20,

        /// <summary>
        ///
        /// </summary>
        TBM_SETPAGESIZE = WM_USER + 21,

        /// <summary>
        ///
        /// </summary>
        TBM_GETPAGESIZE = WM_USER + 22,

        /// <summary>
        ///
        /// </summary>
        TBM_SETLINESIZE = WM_USER + 23,

        /// <summary>
        ///
        /// </summary>
        TBM_GETLINESIZE = WM_USER + 24,

        /// <summary>
        ///
        /// </summary>
        TBM_GETTHUMBRECT = WM_USER + 25,

        /// <summary>
        ///
        /// </summary>
        TBM_GETCHANNELRECT = WM_USER + 26,

        /// <summary>
        ///
        /// </summary>
        TBM_SETTHUMBLENGTH = WM_USER + 27,

        /// <summary>
        ///
        /// </summary>
        TBM_GETTHUMBLENGTH = WM_USER + 28,

        /// <summary>
        ///
        /// </summary>
        TBM_SETTOOLTIPS = WM_USER + 29,

        /// <summary>
        ///
        /// </summary>
        TBM_GETTOOLTIPS = WM_USER + 30,

        /// <summary>
        ///
        /// </summary>
        TBM_SETTIPSIDE = WM_USER + 31,

        /// <summary>
        ///
        /// </summary>
        TBM_SETBUDDY = WM_USER + 32,

        /// <summary>
        ///
        /// </summary>
        TBM_GETBUDDY = WM_USER + 33,

        /// <summary>
        ///
        /// </summary>
        TBM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TBM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        DL_BEGINDRAG = WM_USER + 133,

        /// <summary>
        ///
        /// </summary>
        DL_DRAGGING = WM_USER + 134,

        /// <summary>
        ///
        /// </summary>
        DL_DROPPED = WM_USER + 135,

        /// <summary>
        ///
        /// </summary>
        DL_CANCELDRAG = WM_USER + 136,

        /// <summary>
        ///
        /// </summary>
        UDM_SETRANGE = WM_USER + 101,

        /// <summary>
        ///
        /// </summary>
        UDM_GETRANGE = WM_USER + 102,

        /// <summary>
        ///
        /// </summary>
        UDM_SETPOS = WM_USER + 103,

        /// <summary>
        ///
        /// </summary>
        UDM_GETPOS = WM_USER + 104,

        /// <summary>
        ///
        /// </summary>
        UDM_SETBUDDY = WM_USER + 105,

        /// <summary>
        ///
        /// </summary>
        UDM_GETBUDDY = WM_USER + 106,

        /// <summary>
        ///
        /// </summary>
        UDM_SETACCEL = WM_USER + 107,

        /// <summary>
        ///
        /// </summary>
        UDM_GETACCEL = WM_USER + 108,

        /// <summary>
        ///
        /// </summary>
        UDM_SETBASE = WM_USER + 109,

        /// <summary>
        ///
        /// </summary>
        UDM_GETBASE = WM_USER + 110,

        /// <summary>
        ///
        /// </summary>
        UDM_SETRANGE32 = WM_USER + 111,

        /// <summary>
        ///
        /// </summary>
        UDM_GETRANGE32 = WM_USER + 112,

        /// <summary>
        ///
        /// </summary>
        UDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        UDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        UDM_SETPOS32 = WM_USER + 113,

        /// <summary>
        ///
        /// </summary>
        UDM_GETPOS32 = WM_USER + 114,

        /// <summary>
        ///
        /// </summary>
        PBM_SETRANGE = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        PBM_SETPOS = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        PBM_DELTAPOS = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        PBM_SETSTEP = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        PBM_STEPIT = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        PBM_SETRANGE32 = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        PBM_GETRANGE = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        PBM_GETPOS = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        PBM_SETBARCOLOR = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        PBM_SETBKCOLOR = CCM_SETBKCOLOR,

        /// <summary>
        ///
        /// </summary>
        HKM_SETHOTKEY = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        HKM_GETHOTKEY = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        HKM_SETRULES = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        LVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        LVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        LVM_GETBKCOLOR = LVM_FIRST + 0,

        /// <summary>
        ///
        /// </summary>
        LVM_SETBKCOLOR = LVM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        LVM_GETIMAGELIST = LVM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        LVM_SETIMAGELIST = LVM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMCOUNT = LVM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMA = LVM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMW = LVM_FIRST + 75,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMA = LVM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMW = LVM_FIRST + 76,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTITEMA = LVM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTITEMW = LVM_FIRST + 77,

        /// <summary>
        ///
        /// </summary>
        LVM_DELETEITEM = LVM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        LVM_DELETEALLITEMS = LVM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCALLBACKMASK = LVM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        LVM_SETCALLBACKMASK = LVM_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        LVM_FINDITEMA = LVM_FIRST + 13,

        /// <summary>
        ///
        /// </summary>
        LVM_FINDITEMW = LVM_FIRST + 83,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMRECT = LVM_FIRST + 14,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMPOSITION = LVM_FIRST + 15,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMPOSITION = LVM_FIRST + 16,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSTRINGWIDTHA = LVM_FIRST + 17,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSTRINGWIDTHW = LVM_FIRST + 87,

        /// <summary>
        ///
        /// </summary>
        LVM_HITTEST = LVM_FIRST + 18,

        /// <summary>
        ///
        /// </summary>
        LVM_ENSUREVISIBLE = LVM_FIRST + 19,

        /// <summary>
        ///
        /// </summary>
        LVM_SCROLL = LVM_FIRST + 20,

        /// <summary>
        ///
        /// </summary>
        LVM_REDRAWITEMS = LVM_FIRST + 21,

        /// <summary>
        ///
        /// </summary>
        LVM_ARRANGE = LVM_FIRST + 22,

        /// <summary>
        ///
        /// </summary>
        LVM_EDITLABELA = LVM_FIRST + 23,

        /// <summary>
        ///
        /// </summary>
        LVM_EDITLABELW = LVM_FIRST + 118,

        /// <summary>
        ///
        /// </summary>
        LVM_GETEDITCONTROL = LVM_FIRST + 24,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCOLUMNA = LVM_FIRST + 25,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCOLUMNW = LVM_FIRST + 95,

        /// <summary>
        ///
        /// </summary>
        LVM_SETCOLUMNA = LVM_FIRST + 26,

        /// <summary>
        ///
        /// </summary>
        LVM_SETCOLUMNW = LVM_FIRST + 96,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTCOLUMNA = LVM_FIRST + 27,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTCOLUMNW = LVM_FIRST + 97,

        /// <summary>
        ///
        /// </summary>
        LVM_DELETECOLUMN = LVM_FIRST + 28,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCOLUMNWIDTH = LVM_FIRST + 29,

        /// <summary>
        ///
        /// </summary>
        LVM_SETCOLUMNWIDTH = LVM_FIRST + 30,

        /// <summary>
        ///
        /// </summary>
        LVM_CREATEDRAGIMAGE = LVM_FIRST + 33,

        /// <summary>
        ///
        /// </summary>
        LVM_GETVIEWRECT = LVM_FIRST + 34,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTEXTCOLOR = LVM_FIRST + 35,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTEXTCOLOR = LVM_FIRST + 36,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTEXTBKCOLOR = LVM_FIRST + 37,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTEXTBKCOLOR = LVM_FIRST + 38,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTOPINDEX = LVM_FIRST + 39,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCOUNTPERPAGE = LVM_FIRST + 40,

        /// <summary>
        ///
        /// </summary>
        LVM_GETORIGIN = LVM_FIRST + 41,

        /// <summary>
        ///
        /// </summary>
        LVM_UPDATE = LVM_FIRST + 42,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMSTATE = LVM_FIRST + 43,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMSTATE = LVM_FIRST + 44,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMTEXTA = LVM_FIRST + 45,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMTEXTW = LVM_FIRST + 115,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMTEXTA = LVM_FIRST + 46,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMTEXTW = LVM_FIRST + 116,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMCOUNT = LVM_FIRST + 47,

        /// <summary>
        ///
        /// </summary>
        LVM_SORTITEMS = LVM_FIRST + 48,

        /// <summary>
        ///
        /// </summary>
        LVM_SETITEMPOSITION32 = LVM_FIRST + 49,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSELECTEDCOUNT = LVM_FIRST + 50,

        /// <summary>
        ///
        /// </summary>
        LVM_GETITEMSPACING = LVM_FIRST + 51,

        /// <summary>
        ///
        /// </summary>
        LVM_GETISEARCHSTRINGA = LVM_FIRST + 52,

        /// <summary>
        ///
        /// </summary>
        LVM_GETISEARCHSTRINGW = LVM_FIRST + 117,

        /// <summary>
        ///
        /// </summary>
        LVM_SETICONSPACING = LVM_FIRST + 53,

        /// <summary>
        ///
        /// </summary>
        LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54,

        /// <summary>
        ///
        /// </summary>
        LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSUBITEMRECT = LVM_FIRST + 56,

        /// <summary>
        ///
        /// </summary>
        LVM_SUBITEMHITTEST = LVM_FIRST + 57,

        /// <summary>
        ///
        /// </summary>
        LVM_SETCOLUMNORDERARRAY = LVM_FIRST + 58,

        /// <summary>
        ///
        /// </summary>
        LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59,

        /// <summary>
        ///
        /// </summary>
        LVM_SETHOTITEM = LVM_FIRST + 60,

        /// <summary>
        ///
        /// </summary>
        LVM_GETHOTITEM = LVM_FIRST + 61,

        /// <summary>
        ///
        /// </summary>
        LVM_SETHOTCURSOR = LVM_FIRST + 62,

        /// <summary>
        ///
        /// </summary>
        LVM_GETHOTCURSOR = LVM_FIRST + 63,

        /// <summary>
        ///
        /// </summary>
        LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64,

        /// <summary>
        ///
        /// </summary>
        LVM_SETWORKAREAS = LVM_FIRST + 65,

        /// <summary>
        ///
        /// </summary>
        LVM_GETWORKAREAS = LVM_FIRST + 70,

        /// <summary>
        ///
        /// </summary>
        LVM_GETNUMBEROFWORKAREAS = LVM_FIRST + 73,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSELECTIONMARK = LVM_FIRST + 66,

        /// <summary>
        ///
        /// </summary>
        LVM_SETSELECTIONMARK = LVM_FIRST + 67,

        /// <summary>
        ///
        /// </summary>
        LVM_SETHOVERTIME = LVM_FIRST + 71,

        /// <summary>
        ///
        /// </summary>
        LVM_GETHOVERTIME = LVM_FIRST + 72,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTOOLTIPS = LVM_FIRST + 74,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTOOLTIPS = LVM_FIRST + 78,

        /// <summary>
        ///
        /// </summary>
        LVM_SORTITEMSEX = LVM_FIRST + 81,

        /// <summary>
        ///
        /// </summary>
        LVM_SETBKIMAGEA = LVM_FIRST + 68,

        /// <summary>
        ///
        /// </summary>
        LVM_SETBKIMAGEW = LVM_FIRST + 138,

        /// <summary>
        ///
        /// </summary>
        LVM_GETBKIMAGEA = LVM_FIRST + 69,

        /// <summary>
        ///
        /// </summary>
        LVM_GETBKIMAGEW = LVM_FIRST + 139,

        /// <summary>
        ///
        /// </summary>
        LVM_SETSELECTEDCOLUMN = LVM_FIRST + 140,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTILEWIDTH = LVM_FIRST + 141,

        /// <summary>
        ///
        /// </summary>
        LVM_SETVIEW = LVM_FIRST + 142,

        /// <summary>
        ///
        /// </summary>
        LVM_GETVIEW = LVM_FIRST + 143,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTGROUP = LVM_FIRST + 145,

        /// <summary>
        ///
        /// </summary>
        LVM_SETGROUPINFO = LVM_FIRST + 147,

        /// <summary>
        ///
        /// </summary>
        LVM_GETGROUPINFO = LVM_FIRST + 149,

        /// <summary>
        ///
        /// </summary>
        LVM_REMOVEGROUP = LVM_FIRST + 150,

        /// <summary>
        ///
        /// </summary>
        LVM_MOVEGROUP = LVM_FIRST + 151,

        /// <summary>
        ///
        /// </summary>
        LVM_MOVEITEMTOGROUP = LVM_FIRST + 154,

        /// <summary>
        ///
        /// </summary>
        LVM_SETGROUPMETRICS = LVM_FIRST + 155,

        /// <summary>
        ///
        /// </summary>
        LVM_GETGROUPMETRICS = LVM_FIRST + 156,

        /// <summary>
        ///
        /// </summary>
        LVM_ENABLEGROUPVIEW = LVM_FIRST + 157,

        /// <summary>
        ///
        /// </summary>
        LVM_SORTGROUPS = LVM_FIRST + 158,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTGROUPSORTED = LVM_FIRST + 159,

        /// <summary>
        ///
        /// </summary>
        LVM_REMOVEALLGROUPS = LVM_FIRST + 160,

        /// <summary>
        ///
        /// </summary>
        LVM_HASGROUP = LVM_FIRST + 161,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTILEVIEWINFO = LVM_FIRST + 162,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTILEVIEWINFO = LVM_FIRST + 163,

        /// <summary>
        ///
        /// </summary>
        LVM_SETTILEINFO = LVM_FIRST + 164,

        /// <summary>
        ///
        /// </summary>
        LVM_GETTILEINFO = LVM_FIRST + 165,

        /// <summary>
        ///
        /// </summary>
        LVM_SETINSERTMARK = LVM_FIRST + 166,

        /// <summary>
        ///
        /// </summary>
        LVM_GETINSERTMARK = LVM_FIRST + 167,

        /// <summary>
        ///
        /// </summary>
        LVM_INSERTMARKHITTEST = LVM_FIRST + 168,

        /// <summary>
        ///
        /// </summary>
        LVM_GETINSERTMARKRECT = LVM_FIRST + 169,

        /// <summary>
        ///
        /// </summary>
        LVM_SETINSERTMARKCOLOR = LVM_FIRST + 170,

        /// <summary>
        ///
        /// </summary>
        LVM_GETINSERTMARKCOLOR = LVM_FIRST + 171,

        /// <summary>
        ///
        /// </summary>
        LVM_SETINFOTIP = LVM_FIRST + 173,

        /// <summary>
        ///
        /// </summary>
        LVM_GETSELECTEDCOLUMN = LVM_FIRST + 174,

        /// <summary>
        ///
        /// </summary>
        LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175,

        /// <summary>
        ///
        /// </summary>
        LVM_GETOUTLINECOLOR = LVM_FIRST + 176,

        /// <summary>
        ///
        /// </summary>
        LVM_SETOUTLINECOLOR = LVM_FIRST + 177,

        /// <summary>
        ///
        /// </summary>
        LVM_CANCELEDITLABEL = LVM_FIRST + 179,

        /// <summary>
        ///
        /// </summary>
        LVM_MAPINDEXTOID = LVM_FIRST + 180,

        /// <summary>
        ///
        /// </summary>
        LVM_MAPIDTOINDEX = LVM_FIRST + 181,

        /// <summary>
        ///
        /// </summary>
        TVM_INSERTITEMA = TV_FIRST + 0,

        /// <summary>
        ///
        /// </summary>
        TVM_INSERTITEMW = TV_FIRST + 50,

        /// <summary>
        ///
        /// </summary>
        TVM_DELETEITEM = TV_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        TVM_EXPAND = TV_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        TVM_GETITEMRECT = TV_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        TVM_GETCOUNT = TV_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        TVM_GETINDENT = TV_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        TVM_SETINDENT = TV_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        TVM_GETIMAGELIST = TV_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        TVM_SETIMAGELIST = TV_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        TVM_GETNEXTITEM = TV_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        TVM_SELECTITEM = TV_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        TVM_GETITEMA = TV_FIRST + 12,

        /// <summary>
        ///
        /// </summary>
        TVM_GETITEMW = TV_FIRST + 62,

        /// <summary>
        ///
        /// </summary>
        TVM_SETITEMA = TV_FIRST + 13,

        /// <summary>
        ///
        /// </summary>
        TVM_SETITEMW = TV_FIRST + 63,

        /// <summary>
        ///
        /// </summary>
        TVM_EDITLABELA = TV_FIRST + 14,

        /// <summary>
        ///
        /// </summary>
        TVM_EDITLABELW = TV_FIRST + 65,

        /// <summary>
        ///
        /// </summary>
        TVM_GETEDITCONTROL = TV_FIRST + 15,

        /// <summary>
        ///
        /// </summary>
        TVM_GETVISIBLECOUNT = TV_FIRST + 16,

        /// <summary>
        ///
        /// </summary>
        TVM_HITTEST = TV_FIRST + 17,

        /// <summary>
        ///
        /// </summary>
        TVM_CREATEDRAGIMAGE = TV_FIRST + 18,

        /// <summary>
        ///
        /// </summary>
        TVM_SORTCHILDREN = TV_FIRST + 19,

        /// <summary>
        ///
        /// </summary>
        TVM_ENSUREVISIBLE = TV_FIRST + 20,

        /// <summary>
        ///
        /// </summary>
        TVM_SORTCHILDRENCB = TV_FIRST + 21,

        /// <summary>
        ///
        /// </summary>
        TVM_ENDEDITLABELNOW = TV_FIRST + 22,

        /// <summary>
        ///
        /// </summary>
        TVM_GETISEARCHSTRINGA = TV_FIRST + 23,

        /// <summary>
        ///
        /// </summary>
        TVM_GETISEARCHSTRINGW = TV_FIRST + 64,

        /// <summary>
        ///
        /// </summary>
        TVM_SETTOOLTIPS = TV_FIRST + 24,

        /// <summary>
        ///
        /// </summary>
        TVM_GETTOOLTIPS = TV_FIRST + 25,

        /// <summary>
        ///
        /// </summary>
        TVM_SETINSERTMARK = TV_FIRST + 26,

        /// <summary>
        ///
        /// </summary>
        TVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TVM_SETITEMHEIGHT = TV_FIRST + 27,

        /// <summary>
        ///
        /// </summary>
        TVM_GETITEMHEIGHT = TV_FIRST + 28,

        /// <summary>
        ///
        /// </summary>
        TVM_SETBKCOLOR = TV_FIRST + 29,

        /// <summary>
        ///
        /// </summary>
        TVM_SETTEXTCOLOR = TV_FIRST + 30,

        /// <summary>
        ///
        /// </summary>
        TVM_GETBKCOLOR = TV_FIRST + 31,

        /// <summary>
        ///
        /// </summary>
        TVM_GETTEXTCOLOR = TV_FIRST + 32,

        /// <summary>
        ///
        /// </summary>
        TVM_SETSCROLLTIME = TV_FIRST + 33,

        /// <summary>
        ///
        /// </summary>
        TVM_GETSCROLLTIME = TV_FIRST + 34,

        /// <summary>
        ///
        /// </summary>
        TVM_SETINSERTMARKCOLOR = TV_FIRST + 37,

        /// <summary>
        ///
        /// </summary>
        TVM_GETINSERTMARKCOLOR = TV_FIRST + 38,

        /// <summary>
        ///
        /// </summary>
        TVM_GETITEMSTATE = TV_FIRST + 39,

        /// <summary>
        ///
        /// </summary>
        TVM_SETLINECOLOR = TV_FIRST + 40,

        /// <summary>
        ///
        /// </summary>
        TVM_GETLINECOLOR = TV_FIRST + 41,

        /// <summary>
        ///
        /// </summary>
        TVM_MAPACCIDTOHTREEITEM = TV_FIRST + 42,

        /// <summary>
        ///
        /// </summary>
        TVM_MAPHTREEITEMTOACCID = TV_FIRST + 43,

        /// <summary>
        ///
        /// </summary>
        CBEM_INSERTITEMA = WM_USER + 1,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETIMAGELIST = WM_USER + 2,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETIMAGELIST = WM_USER + 3,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETITEMA = WM_USER + 4,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETITEMA = WM_USER + 5,

        /// <summary>
        ///
        /// </summary>
        CBEM_DELETEITEM = CB_DELETESTRING,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETCOMBOCONTROL = WM_USER + 6,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETEDITCONTROL = WM_USER + 7,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETEXTENDEDSTYLE = WM_USER + 14,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETEXTENDEDSTYLE = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETEXSTYLE = WM_USER + 8,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETEXSTYLE = WM_USER + 9,

        /// <summary>
        ///
        /// </summary>
        CBEM_HASEDITCHANGED = WM_USER + 10,

        /// <summary>
        ///
        /// </summary>
        CBEM_INSERTITEMW = WM_USER + 11,

        /// <summary>
        ///
        /// </summary>
        CBEM_SETITEMW = WM_USER + 12,

        /// <summary>
        ///
        /// </summary>
        CBEM_GETITEMW = WM_USER + 13,

        /// <summary>
        ///
        /// </summary>
        TCM_GETIMAGELIST = TCM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        TCM_SETIMAGELIST = TCM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        TCM_GETITEMCOUNT = TCM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        TCM_GETITEMA = TCM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        TCM_GETITEMW = TCM_FIRST + 60,

        /// <summary>
        ///
        /// </summary>
        TCM_SETITEMA = TCM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        TCM_SETITEMW = TCM_FIRST + 61,

        /// <summary>
        ///
        /// </summary>
        TCM_INSERTITEMA = TCM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        TCM_INSERTITEMW = TCM_FIRST + 62,

        /// <summary>
        ///
        /// </summary>
        TCM_DELETEITEM = TCM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        TCM_DELETEALLITEMS = TCM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        TCM_GETITEMRECT = TCM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        TCM_GETCURSEL = TCM_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        TCM_SETCURSEL = TCM_FIRST + 12,

        /// <summary>
        ///
        /// </summary>
        TCM_HITTEST = TCM_FIRST + 13,

        /// <summary>
        ///
        /// </summary>
        TCM_SETITEMEXTRA = TCM_FIRST + 14,

        /// <summary>
        ///
        /// </summary>
        TCM_ADJUSTRECT = TCM_FIRST + 40,

        /// <summary>
        ///
        /// </summary>
        TCM_SETITEMSIZE = TCM_FIRST + 41,

        /// <summary>
        ///
        /// </summary>
        TCM_REMOVEIMAGE = TCM_FIRST + 42,

        /// <summary>
        ///
        /// </summary>
        TCM_SETPADDING = TCM_FIRST + 43,

        /// <summary>
        ///
        /// </summary>
        TCM_GETROWCOUNT = TCM_FIRST + 44,

        /// <summary>
        ///
        /// </summary>
        TCM_GETTOOLTIPS = TCM_FIRST + 45,

        /// <summary>
        ///
        /// </summary>
        TCM_SETTOOLTIPS = TCM_FIRST + 46,

        /// <summary>
        ///
        /// </summary>
        TCM_GETCURFOCUS = TCM_FIRST + 47,

        /// <summary>
        ///
        /// </summary>
        TCM_SETCURFOCUS = TCM_FIRST + 48,

        /// <summary>
        ///
        /// </summary>
        TCM_SETMINTABWIDTH = TCM_FIRST + 49,

        /// <summary>
        ///
        /// </summary>
        TCM_DESELECTALL = TCM_FIRST + 50,

        /// <summary>
        ///
        /// </summary>
        TCM_HIGHLIGHTITEM = TCM_FIRST + 51,

        /// <summary>
        ///
        /// </summary>
        TCM_SETEXTENDEDSTYLE = TCM_FIRST + 52,

        /// <summary>
        ///
        /// </summary>
        TCM_GETEXTENDEDSTYLE = TCM_FIRST + 53,

        /// <summary>
        ///
        /// </summary>
        TCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        TCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        ACM_OPENA = WM_USER + 100,

        /// <summary>
        ///
        /// </summary>
        ACM_OPENW = WM_USER + 103,

        /// <summary>
        ///
        /// </summary>
        ACM_PLAY = WM_USER + 101,

        /// <summary>
        ///
        /// </summary>
        ACM_STOP = WM_USER + 102,

        /// <summary>
        ///
        /// </summary>
        MCM_FIRST = 0x1000,

        /// <summary>
        ///
        /// </summary>
        MCM_GETCURSEL = MCM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        MCM_SETCURSEL = MCM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        MCM_GETMAXSELCOUNT = MCM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        MCM_SETMAXSELCOUNT = MCM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        MCM_GETSELRANGE = MCM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        MCM_SETSELRANGE = MCM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        MCM_GETMONTHRANGE = MCM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        MCM_SETDAYSTATE = MCM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        MCM_GETMINREQRECT = MCM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        MCM_SETCOLOR = MCM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        MCM_GETCOLOR = MCM_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        MCM_SETTODAY = MCM_FIRST + 12,

        /// <summary>
        ///
        /// </summary>
        MCM_GETTODAY = MCM_FIRST + 13,

        /// <summary>
        ///
        /// </summary>
        MCM_HITTEST = MCM_FIRST + 14,

        /// <summary>
        ///
        /// </summary>
        MCM_SETFIRSTDAYOFWEEK = MCM_FIRST + 15,

        /// <summary>
        ///
        /// </summary>
        MCM_GETFIRSTDAYOFWEEK = MCM_FIRST + 16,

        /// <summary>
        ///
        /// </summary>
        MCM_GETRANGE = MCM_FIRST + 17,

        /// <summary>
        ///
        /// </summary>
        MCM_SETRANGE = MCM_FIRST + 18,

        /// <summary>
        ///
        /// </summary>
        MCM_GETMONTHDELTA = MCM_FIRST + 19,

        /// <summary>
        ///
        /// </summary>
        MCM_SETMONTHDELTA = MCM_FIRST + 20,

        /// <summary>
        ///
        /// </summary>
        MCM_GETMAXTODAYWIDTH = MCM_FIRST + 21,

        /// <summary>
        ///
        /// </summary>
        MCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        MCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT,

        /// <summary>
        ///
        /// </summary>
        DTM_FIRST = 0x1000,

        /// <summary>
        ///
        /// </summary>
        DTM_GETSYSTEMTIME = DTM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        DTM_SETSYSTEMTIME = DTM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        DTM_GETRANGE = DTM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        DTM_SETRANGE = DTM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        DTM_SETFORMATA = DTM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        DTM_SETFORMATW = DTM_FIRST + 50,

        /// <summary>
        ///
        /// </summary>
        DTM_SETMCCOLOR = DTM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        DTM_GETMCCOLOR = DTM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        DTM_GETMONTHCAL = DTM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        DTM_SETMCFONT = DTM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        DTM_GETMCFONT = DTM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        PGM_SETCHILD = PGM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        PGM_RECALCSIZE = PGM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        PGM_FORWARDMOUSE = PGM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        PGM_SETBKCOLOR = PGM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        PGM_GETBKCOLOR = PGM_FIRST + 5,

        /// <summary>
        ///
        /// </summary>
        PGM_SETBORDER = PGM_FIRST + 6,

        /// <summary>
        ///
        /// </summary>
        PGM_GETBORDER = PGM_FIRST + 7,

        /// <summary>
        ///
        /// </summary>
        PGM_SETPOS = PGM_FIRST + 8,

        /// <summary>
        ///
        /// </summary>
        PGM_GETPOS = PGM_FIRST + 9,

        /// <summary>
        ///
        /// </summary>
        PGM_SETBUTTONSIZE = PGM_FIRST + 10,

        /// <summary>
        ///
        /// </summary>
        PGM_GETBUTTONSIZE = PGM_FIRST + 11,

        /// <summary>
        ///
        /// </summary>
        PGM_GETBUTTONSTATE = PGM_FIRST + 12,

        /// <summary>
        ///
        /// </summary>
        PGM_GETDROPTARGET = CCM_GETDROPTARGET,

        /// <summary>
        ///
        /// </summary>
        BCM_GETIDEALSIZE = BCM_FIRST + 0x0001,

        /// <summary>
        ///
        /// </summary>
        BCM_SETIMAGELIST = BCM_FIRST + 0x0002,

        /// <summary>
        ///
        /// </summary>
        BCM_GETIMAGELIST = BCM_FIRST + 0x0003,

        /// <summary>
        ///
        /// </summary>
        BCM_SETTEXTMARGIN = BCM_FIRST + 0x0004,

        /// <summary>
        ///
        /// </summary>
        BCM_GETTEXTMARGIN = BCM_FIRST + 0x0005,

        /// <summary>
        ///
        /// </summary>
        EM_SETCUEBANNER = ECM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        EM_GETCUEBANNER = ECM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        EM_SHOWBALLOONTIP = ECM_FIRST + 3,

        /// <summary>
        ///
        /// </summary>
        EM_HIDEBALLOONTIP = ECM_FIRST + 4,

        /// <summary>
        ///
        /// </summary>
        CB_SETMINVISIBLE = CBM_FIRST + 1,

        /// <summary>
        ///
        /// </summary>
        CB_GETMINVISIBLE = CBM_FIRST + 2,

        /// <summary>
        ///
        /// </summary>
        LM_HITTEST = WM_USER + 0x300,

        /// <summary>
        ///
        /// </summary>
        LM_GETIDEALHEIGHT = WM_USER + 0x301,

        /// <summary>
        ///
        /// </summary>
        LM_SETITEM = WM_USER + 0x302,

        /// <summary>
        ///
        /// </summary>
        LM_GETITEM = WM_USER + 0x303
    }
}
