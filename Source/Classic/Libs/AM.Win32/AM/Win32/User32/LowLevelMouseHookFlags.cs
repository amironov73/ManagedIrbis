// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LowLevelMouseHookFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the event-injected flag.
	/// </summary>
	[Flags]
	public enum LowLevelMouseHookFlags
	{
		/// <summary>
		/// Test the event-injected flag. 
		/// </summary>
		LLMHF_INJECTED = 0x00000001
	}
}
