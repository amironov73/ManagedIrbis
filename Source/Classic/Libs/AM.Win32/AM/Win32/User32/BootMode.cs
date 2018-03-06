// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BootMode.cs -- operating system boot mode.
   Ars Magna project, http://arsmagna.ru */

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
