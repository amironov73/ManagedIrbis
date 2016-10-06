/* LowLevelMouseHookFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

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
