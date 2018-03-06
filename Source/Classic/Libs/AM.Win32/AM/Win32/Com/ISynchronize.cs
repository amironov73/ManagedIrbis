// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ISynchronize.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The ISynchronize interface provides asynchronous communication 
	/// between objects about the occurrence of an event. Objects that 
	/// implement ISynchronize can receive indications that an event 
	/// has occurred, and they can respond to queries about the event. 
	/// In this way, clients can make sure that one request has been 
	/// processed before they submit a subsequent request that depends on 
	/// completion of the first one.
	/// </summary>
	[ComImport]
	[Guid ( "00000030-0000-0000-C000-000000000046" )]
	[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface ISynchronize : IUnknown
	{
		/// <summary>
		/// Waits for the synchronization object to be signaled or for 
		/// a specified timeout period to elapse, whichever comes first.
		/// </summary>
		/// <param name="dwFlags">Wait options. Values are taken from the 
		/// COWAIT_FLAGS enumeration.</param>
		/// <param name="dwMilliseconds">Time, in milliseconds, this call will 
		/// wait before returning. If INFINITE, the caller will wait until the 
		/// synchronization object is signaled, no matter how long it takes. 
		/// If 0, Wait returns immediately.</param>
		/// <returns>If the caller is waiting in a single-thread apartment, 
		/// Wait enters the COM modal loop. If the caller is waiting in a 
		/// multithread apartment, the caller is blocked until Wait returns.
		/// </returns>
		[PreserveSig]
		int Wait
			(
			ComWaitFlags dwFlags,
			int dwMilliseconds
			);

		/// <summary>
		/// Sets the synchronization object to the signaled state and causes 
		/// pending calls to ISynchronize::Wait to return S_OK.
		/// </summary>
		/// <returns>The synchronization object was signaled.</returns>
		[PreserveSig]
		int Signal ();

		/// <summary>
		/// Resets the synchronization object to the not signaled state.
		/// </summary>
		/// <returns>The synchronization object was reset.</returns>
		[PreserveSig]
		int Reset ();
	}
}