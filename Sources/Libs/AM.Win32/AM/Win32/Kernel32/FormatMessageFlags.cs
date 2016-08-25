/* FormatMessageFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags for FormatMessage function.
	/// </summary>
	[Flags]
	public enum FormatMessageFlags
	{
		/// <summary>
		/// The lpBuffer parameter is a pointer to a PVOID pointer, 
		/// and that the nSize parameter specifies the minimum number 
		/// of TCHARs to allocate for an output message buffer. The 
		/// function allocates a buffer large enough to hold the 
		/// formatted message, and places a pointer to the allocated 
		/// buffer at the address specified by lpBuffer. The caller 
		/// should use the LocalFree function to free the buffer when 
		/// it is no longer needed.
		/// </summary>
		FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,

		/// <summary>
		/// Insert sequences in the message definition are to be ignored 
		/// and passed through to the output buffer unchanged. This flag 
		/// is useful for fetching a message for later formatting. If 
		/// this flag is set, the Arguments parameter is ignored.
		/// </summary>
		FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,

		/// <summary>
		/// The lpSource parameter is a pointer to a null-terminated 
		/// message definition. The message definition may contain insert 
		/// sequences, just as the message text in a message table 
		/// resource may. Cannot be used with FORMAT_MESSAGE_FROM_HMODULE 
		/// or FORMAT_MESSAGE_FROM_SYSTEM.
		/// </summary>
		FORMAT_MESSAGE_FROM_STRING = 0x00000400,

		/// <summary>
		/// The lpSource parameter is a module handle containing the 
		/// message-table resource(s) to search. If this lpSource handle 
		/// is NULL, the current process's application image file will 
		/// be searched. Cannot be used with FORMAT_MESSAGE_FROM_STRING.
		/// </summary>
		FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,

		/// <summary>
		/// <para>The function should search the system message-table 
		/// resource(s) for the requested message. If this flag is 
		/// specified with FORMAT_MESSAGE_FROM_HMODULE, the function 
		/// searches the system message table if the message is not 
		/// found in the module specified by lpSource. Cannot be used 
		/// with FORMAT_MESSAGE_FROM_STRING.</para>
		/// <para>If this flag is specified, an application can pass 
		/// the result of the GetLastError function to retrieve the 
		/// message text for a system-defined error.</para>
		/// </summary>
		FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,

		/// <summary>
		/// <para>The Arguments parameter is not a va_list structure, 
		/// but is a pointer to an array of values that represent the 
		/// arguments.</para>
		/// <para>This flag cannot be used with 64-bit argument values. 
		/// If you are using 64-bit values, you must use the va_list 
		/// structure.</para>
		/// </summary>
		FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,

		/// <summary>
		/// ???
		/// </summary>
		FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF
	}
}
