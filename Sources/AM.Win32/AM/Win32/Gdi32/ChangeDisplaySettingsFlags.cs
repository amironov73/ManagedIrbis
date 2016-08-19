/* ChangeDisplaySettingsFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum ChangeDisplaySettingsFlags
	{
		/// <summary>
		/// The graphics mode for the current screen will be changed 
		/// dynamically.
		/// </summary>
		ZERO = 0,

		/// <summary>
		/// The graphics mode for the current screen will be changed 
		/// dynamically and the graphics mode will be updated in the 
		/// registry. The mode information is stored in the USER profile.
		/// </summary>
		CDS_UPDATEREGISTRY = 0x00000001,

		/// <summary>
		/// The system tests if the requested graphics mode could be set.
		/// </summary>
		CDS_TEST = 0x00000002,

		/// <summary>
		/// <para>The mode is temporary in nature.</para>
		/// <para>Windows NT/2000/XP: If you change to and from another 
		/// desktop, this mode will not be reset.</para>
		/// </summary>
		CDS_FULLSCREEN = 0x00000004,

		/// <summary>
		/// The settings will be saved in the global settings area so that 
		/// they will affect all users on the machine. Otherwise, only the 
		/// settings for the user are modified. This flag is only valid when 
		/// specified with the CDS_UPDATEREGISTRY flag.
		/// </summary>
		CDS_GLOBAL = 0x00000008,

		/// <summary>
		/// This device will become the primary device. 
		/// </summary>
		CDS_SET_PRIMARY = 0x00000010,

		/// <summary>
		/// ???
		/// </summary>
		CDS_VIDEOPARAMETERS = 0x00000020,

		/// <summary>
		/// The settings should be changed, even if the requested settings 
		///		are the same as the current settings. 
		/// </summary>
		CDS_RESET = 0x40000000,

		/// <summary>
		/// The settings will be saved in the registry, but will not 
		/// take affect. This flag is only valid when specified with 
		/// the CDS_UPDATEREGISTRY flag. 
		/// </summary>
		CDS_NORESET = 0x10000000
	}
}
