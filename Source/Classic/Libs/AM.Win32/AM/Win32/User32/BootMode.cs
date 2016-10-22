/* BootMode.cs -- operating system boot mode.
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Operating system boot mode.
    /// </summary>
	public enum BootMode
	{
        /// <summary>
        /// Unknown boot mode.
        /// </summary>
		UnknownBootMode = -1,

        /// <summary>
        /// Normal.
        /// </summary>
		NormalBoot = 0,

        /// <summary>
        /// Failsafe mode.
        /// </summary>
		FailSafeBoot = 1,

        /// <summary>
        /// Failsafe mode with network support.
        /// </summary>
		FailSafeWithNetworkSupportBoot = 2
	}
}
