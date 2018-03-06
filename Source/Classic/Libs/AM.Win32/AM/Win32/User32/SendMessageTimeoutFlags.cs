// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SendMessageTimeoutFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies how to send the message.
	/// </summary>
	[Flags]
	public enum SendMessageTimeoutFlags
	{
		/// <summary>
		/// The calling thread is not prevented from processing other 
		/// requests while waiting for the function to return.
		/// </summary>
		SMTO_NORMAL = 0x0000,

		/// <summary>
		/// Prevents the calling thread from processing any other 
		/// requests until the function returns.
		/// </summary>
		SMTO_BLOCK = 0x0001,

		/// <summary>
		/// Returns without waiting for the time-out period to elapse 
		/// if the receiving thread appears to not respond or "hangs."
		/// </summary>
		SMTO_ABORTIFHUNG = 0x0002,

		/// <summary>
		/// Microsoft® Windows® 2000/Windows XP: Does not return when 
		/// the time-out period elapses if the receiving thread stops 
		/// responding.
		/// </summary>
		SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
	}
}
