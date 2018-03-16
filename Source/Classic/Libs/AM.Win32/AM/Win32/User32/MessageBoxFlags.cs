// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MessageBoxFlags.cs -- specifies the contents and behavior of the dialog box
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the contents and behavior of the dialog box.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum MessageBoxFlags
    {
        #region Buttons

        /// <summary>
        /// The message box contains one push button: OK. 
        /// This is the default.
        /// </summary>
        MB_OK = 0x00000000,

        /// <summary>
        /// The message box contains two push buttons: OK and Cancel.
        /// </summary>
        MB_OKCANCEL = 0x00000001,

        /// <summary>
        /// The message box contains three push buttons: Abort, Retry, 
        /// and Ignore.
        /// </summary>
        MB_ABORTRETRYIGNORE = 0x00000002,

        /// <summary>
        /// The message box contains three push buttons: Yes, No, and Cancel.
        /// </summary>
        MB_YESNOCANCEL = 0x00000003,

        /// <summary>
        /// The message box contains two push buttons: Yes and No.
        /// </summary>
        MB_YESNO = 0x00000004,

        /// <summary>
        /// The message box contains two push buttons: Retry and Cancel.
        /// </summary>
        MB_RETRYCANCEL = 0x00000005,

        /// <summary>
        /// Microsoft® Windows® 2000/XP: The message box contains 
        /// three push buttons: Cancel, Try Again, Continue. 
        /// Use this message box type instead of MB_ABORTRETRYIGNORE.
        /// </summary>
        MB_CANCELTRYCONTINUE = 0x00000006,

        #endregion

        #region Icons

        /// <summary>
        /// A stop-sign icon appears in the message box.
        /// </summary>
        MB_ICONHAND = 0x00000010,

        /// <summary>
        /// A question-mark icon appears in the message box.
        /// </summary>
        MB_ICONQUESTION = 0x00000020,

        /// <summary>
        /// An exclamation-point icon appears in the message box.
        /// </summary>
        MB_ICONEXCLAMATION = 0x00000030,

        /// <summary>
        /// An icon consisting of a lowercase letter i in a circle appears in the message box.
        /// </summary>
        MB_ICONASTERISK = 0x00000040,

        /// <summary>
        /// User icon.
        /// </summary>
        MB_USERICON = 0x00000080,

        /// <summary>
        /// An exclamation-point icon appears in the message box.
        /// </summary>
        MB_ICONWARNING = 0x00000030,

        /// <summary>
        /// A stop-sign icon appears in the message box.
        /// </summary>
        MB_ICONERROR = 0x00000010,

        /// <summary>
        /// An icon consisting of a lowercase letter i in a circle appears in the message box.
        /// </summary>
        MB_ICONINFORMATION = 0x00000040,

        /// <summary>
        /// A stop-sign icon appears in the message box.
        /// </summary>
        MB_ICONSTOP = 0x00000010,

        #endregion

        #region Default buttons

        /// <summary>
        /// <para>The first button is the default button.</para>
        /// <para>MB_DEFBUTTON1 is the default unless MB_DEFBUTTON2, 
        /// MB_DEFBUTTON3, or MB_DEFBUTTON4 is specified.</para>
        /// </summary>
        MB_DEFBUTTON1 = 0x00000000,

        /// <summary>
        /// The second button is the default button.
        /// </summary>
        MB_DEFBUTTON2 = 0x00000100,

        /// <summary>
        /// The third button is the default button.
        /// </summary>
        MB_DEFBUTTON3 = 0x00000200,

        /// <summary>
        /// The fourth button is the default button.
        /// </summary>
        MB_DEFBUTTON4 = 0x00000300,

        #endregion

        #region Modality

        /// <summary>
        /// <para>The user must respond to the message box before 
        /// continuing work in the window identified by the hWnd 
        /// parameter. However, the user can move to the windows 
        /// of other threads and work in those windows.</para> 
        /// <para>Depending on the hierarchy of windows in the 
        /// application, the user may be able to move to other 
        /// windows within the thread. All child windows of the 
        /// parent of the message box are automatically disabled, 
        /// but popup windows are not.</para>
        /// <para>MB_APPLMODAL is the default if neither 
        /// MB_SYSTEMMODAL nor MB_TASKMODAL is specified.</para>
        /// </summary>
        MB_APPLMODAL = 0x00000000,

        /// <summary>
        /// Same as MB_APPLMODAL except that the message box has 
        /// the WS_EX_TOPMOST style. Use system-modal message boxes 
        /// to notify the user of serious, potentially damaging errors 
        /// that require immediate attention (for example, running out 
        /// of memory). This flag has no effect on the user's ability 
        /// to interact with windows other than those associated with 
        /// hWnd.
        /// </summary>
        MB_SYSTEMMODAL = 0x00001000,

        /// <summary>
        /// Same as MB_APPLMODAL except that all the top-level windows 
        /// belonging to the current thread are disabled if the hWnd 
        /// parameter is NULL. Use this flag when the calling application 
        /// or library does not have a window handle available but still 
        /// needs to prevent input to other windows in the calling thread 
        /// without suspending other threads.
        /// </summary>
        MB_TASKMODAL = 0x00002000,

        #endregion

        #region Other options

        /// <summary>
        /// Windows 95/98/Me, Windows NT® 4.0 and later: Adds a Help 
        /// button to the message box. When the user clicks the Help 
        /// button or presses F1, the system sends a WM_HELP message 
        /// to the owner.
        /// </summary>
        MB_HELP = 0x00004000,

        /// <summary>
        /// ???
        /// </summary>
        MB_NOFOCUS = 0x00008000,

        /// <summary>
        /// The message box becomes the foreground window. 
        /// Internally, the system calls the SetForegroundWindow 
        /// function for the message box.
        /// </summary>
        MB_SETFOREGROUND = 0x00010000,

        /// <summary>
        /// <para>Windows NT/2000/XP: Same as MB_SERVICE_NOTIFICATION 
        /// except that the system will display the message box only 
        /// on the default desktop of the interactive window station. 
        /// </para>
        /// <para>Windows NT 4.0 and earlier: If the current input 
        /// desktop is not the default desktop, MessageBox fails.</para> 
        /// <para>Windows 2000/XP: If the current input desktop is not 
        /// the default desktop, MessageBox does not return until the 
        /// user switches to the default desktop.</para>
        /// <para>Windows 95/98/Me: This flag has no effect.</para>
        /// </summary>
        MB_DEFAULT_DESKTOP_ONLY = 0x00020000,

        /// <summary>
        /// The message box is created with the WS_EX_TOPMOST window style.
        /// </summary>
        MB_TOPMOST = 0x00040000,

        /// <summary>
        /// The text is right-justified.
        /// </summary>
        MB_RIGHT = 0x00080000,

        /// <summary>
        /// Displays message and caption text using right-to-left reading 
        /// order on Hebrew and Arabic systems.
        /// </summary>
        MB_RTLREADING = 0x00100000,

        /// <summary>
        /// <para>Windows NT/2000/XP: The caller is a service notifying 
        /// the user of an event. The function displays a message box on 
        /// the current active desktop, even if there is no user logged 
        /// on to the computer.</para>
        /// <para>Terminal Services: If the calling thread has an 
        /// impersonation token, the function directs the message box to 
        /// the session specified in the impersonation token.</para>
        /// <para>If this flag is set, the hWnd parameter must be NULL. 
        /// This is so the message box can appear on a desktop other 
        /// than the desktop corresponding to the hWnd.</para>
        /// </summary>
        MB_SERVICE_NOTIFICATION = 0x00200000,

        /// <summary>
        /// Windows NT/2000/XP: This value corresponds to the value 
        /// defined for MB_SERVICE_NOTIFICATION for Windows NT version 
        /// 3.51. 
        /// </summary>
        MB_SERVICE_NOTIFICATION_NT3X = 0x00040000,

        #endregion

        #region Masks

        /// <summary>
        /// Buttons mask.
        /// </summary>
        MB_TYPEMASK = 0x0000000F,

        /// <summary>
        /// Icon mask.
        /// </summary>
        MB_ICONMASK = 0x000000F0,

        /// <summary>
        /// Default button mask.
        /// </summary>
        MB_DEFMASK = 0x00000F00,

        /// <summary>
        /// Modality mask.
        /// </summary>
        MB_MODEMASK = 0x00003000,

        /// <summary>
        /// Miscellaneous mask.
        /// </summary>
        MB_MISCMASK = 0x0000C000,

        #endregion
    }
}
