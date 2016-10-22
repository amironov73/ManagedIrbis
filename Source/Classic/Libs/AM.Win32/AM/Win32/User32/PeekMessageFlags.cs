/* PeekMessageFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies how messages are handled by PeekMessage function.
	/// </summary>
	[Flags]
	public enum PeekMessageFlags
	{
		/// <summary>
		///Messages are not removed from the queue after processing 
		/// by PeekMessage. 
		/// </summary>
		PM_NOREMOVE = 0x0000,

		/// <summary>
		/// Messages are removed from the queue after processing 
		/// by PeekMessage.
		/// </summary>
		PM_REMOVE = 0x0001,

		/// <summary>
		/// ???
		/// </summary>
		PM_NOYIELD = 0x0002,

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: Process mouse and keyboard messages.
		/// </summary>
		PM_QS_INPUT = ( QueueStatusFlags.QS_INPUT << 16 ),

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: Process all posted messages, including 
		/// timers and hotkeys.
		/// </summary>
		PM_QS_POSTMESSAGE = ( ( QueueStatusFlags.QS_POSTMESSAGE
			| QueueStatusFlags.QS_HOTKEY
			| QueueStatusFlags.QS_TIMER ) << 16 ),

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: Process paint messages.
		/// </summary>
		PM_QS_PAINT = ( QueueStatusFlags.QS_PAINT << 16 ),

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: Process all sent messages.
		/// </summary>
		PM_QS_SENDMESSAGE = ( QueueStatusFlags.QS_SENDMESSAGE << 16 ),
	}
}
