// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InSendMessageCode.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Return codes of InSendMessageEx function.
	/// </summary>
	public enum InSendMessageCode
	{
		/// <summary>
		/// No message send.
		/// </summary>
		ISMEX_NOSEND = 0x00000000,

		/// <summary>
		/// The message was sent using the SendMessage or SendMessageTimeout 
		/// function. If ISMEX_REPLIED is not set, the thread that sent the 
		/// message is blocked.
		/// </summary>
		ISMEX_SEND = 0x00000001,

		/// <summary>
		/// The message was sent using the SendNotifyMessage function. 
		/// The thread that sent the message is not blocked.
		/// </summary>
		ISMEX_NOTIFY = 0x00000002,

		/// <summary>
		/// The message was sent using the SendMessageCallback function. 
		/// The thread that sent the message is not blocked.
		/// </summary>
		ISMEX_CALLBACK = 0x00000004,

		/// <summary>
		/// The window procedure has processed the message. The thread that 
		/// sent the message is no longer blocked.
		/// </summary>
		ISMEX_REPLIED = 0x00000008
	}
}
