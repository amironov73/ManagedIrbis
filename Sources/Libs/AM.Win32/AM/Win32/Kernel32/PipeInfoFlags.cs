/* PipeInfoFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Named pipe info.
	/// </summary>
	[Flags]
	public enum PipeInfoFlags
	{
		/// <summary>
		/// The handle refers to the client end of a named pipe instance. 
		/// This is the default.
		/// </summary>
		PIPE_CLIENT_END = 0x00000000,

		/// <summary>
		/// The handle refers to the server end of a named pipe instance. 
		/// If this value is not specified, the handle refers to the 
		/// client end of a named pipe instance.
		/// </summary>
		PIPE_SERVER_END = 0x00000001,

		/// <summary>
		/// Data is written to the pipe as a stream of bytes. This mode 
		/// cannot be used with PIPE_READMODE_MESSAGE.
		/// </summary>
		PIPE_TYPE_BYTE = 0x00000000,

		/// <summary>
		/// Data is written to the pipe as a stream of messages. 
		/// This mode can be used with either PIPE_READMODE_MESSAGE 
		/// or PIPE_READMODE_BYTE.
		/// </summary>
		PIPE_TYPE_MESSAGE = 0x00000004,

		/// <summary>
		/// Blocking mode is enabled. When the pipe handle is specified 
		/// in the ReadFile, WriteFile, or ConnectNamedPipe function, 
		/// the operations are not completed until there is data to read, 
		/// all data is written, or a client is connected. Use of this 
		/// mode can mean waiting indefinitely in some situations for a 
		/// client process to perform an action.
		/// </summary>
		PIPE_WAIT = 0x00000000,

		/// <summary>
		/// <para>Nonblocking mode is enabled. In this mode, ReadFile, 
		/// WriteFile, and ConnectNamedPipe always return immediately.
		/// </para>
		/// <para>Note that nonblocking mode is supported for 
		/// compatibility with Microsoft LAN Manager version 2.0 and 
		/// should not be used to achieve asynchronous I/O with named 
		/// pipes.</para>
		/// </summary>
		PIPE_NOWAIT = 0x00000001,
	}
}
