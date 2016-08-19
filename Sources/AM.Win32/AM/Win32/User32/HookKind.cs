/* HookKind.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies type of the hook.
	/// </summary>
	public enum HookKind
	{
		/// <summary>
		/// ???
		/// </summary>
		WH_MIN = -1,

		/// <summary>
		/// Hook procedure that monitors messages generated 
		/// as a result of an input event in a dialog box, 
		/// message box, menu, or scroll bar.
		/// </summary>
		WH_MSGFILTER = -1,

		/// <summary>
		/// Hook procedure that records input messages 
		/// posted to the system message queue.
		/// </summary>
		WH_JOURNALRECORD = 0,

		/// <summary>
		/// Hook procedure that posts messages previously 
		/// recorded by a WH_JOURNALRECORD hook procedure.
		/// </summary>
		WH_JOURNALPLAYBACK = 1,

		/// <summary>
		/// Hook procedure that monitors keystroke messages.
		/// </summary>
		WH_KEYBOARD = 2,

		/// <summary>
		/// Hook procedure that monitors messages posted 
		/// to a message queue.
		/// </summary>
		WH_GETMESSAGE = 3,

		/// <summary>
		/// Hook procedure that monitors messages before 
		/// the system sends them to the destination window 
		/// procedure.
		/// </summary>
		WH_CALLWNDPROC = 4,

		/// <summary>
		/// Hook procedure that receives notifications 
		/// useful to a computer-based training (CBT) application.
		/// </summary>
		WH_CBT = 5,

		/// <summary>
		/// Hook procedure that monitors messages generated as 
		/// a result of an input event in a dialog box, message 
		/// box, menu, or scroll bar. The hook procedure monitors 
		/// these messages for all applications in the same desktop 
		/// as the calling thread.
		/// </summary>
		WH_SYSMSGFILTER = 6,

		/// <summary>
		/// Hook procedure that monitors mouse messages.
		/// </summary>
		WH_MOUSE = 7,

		/// <summary>
		/// ???
		/// </summary>
		WH_HARDWARE = 8,

		/// <summary>
		/// Hook procedure useful for debugging other hook 
		/// procedures.
		/// </summary>
		WH_DEBUG = 9,

		/// <summary>
		/// Hook procedure that receives notifications useful 
		/// to shell applications.
		/// </summary>
		WH_SHELL = 10,

		/// <summary>
		/// Hook procedure that will be called when the 
		/// application's foreground thread is about to 
		/// become idle. This hook is useful for performing 
		/// low priority tasks during idle time. 
		/// </summary>
		WH_FOREGROUNDIDLE = 11,

		/// <summary>
		/// Hook procedure that monitors messages after 
		/// they have been processed by the destination 
		/// window procedure. 
		/// </summary>
		WH_CALLWNDPROCRET = 12,

		/// <summary>
		/// Windows NT/2000/XP: Hook procedure that monitors 
		/// low-level keyboard input events.
		/// </summary>
		WH_KEYBOARD_LL = 13,

		/// <summary>
		/// Windows NT/2000/XP: Hook procedure that monitors 
		/// low-level mouse input events.
		/// </summary>
		WH_MOUSE_LL = 14,

		/// <summary>
		/// ???
		/// </summary>
		WH_MAX = 14,

		/// <summary>
		/// ???
		/// </summary>
		WH_MINHOOK = WH_MIN,

		/// <summary>
		/// ???
		/// </summary>
		WH_MAXHOOK = WH_MAX,
	}
}
