/* ACPowerState.cs -- AC power status.
   Ars Magna project, http://library.istu.edu/am */

namespace AM.Win32
{
	/// <summary>
	/// AC power status.
	/// </summary>
	public enum ACPowerStatus : byte
	{
		/// <summary>
		/// Offline.
		/// </summary>
		Offline = 0,

		/// <summary>
		/// Online.
		/// </summary>
		Online = 1,

		/// <summary>
		/// Unknown.
		/// </summary>
		Unknown = 255
	}
}