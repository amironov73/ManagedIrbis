// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SYSTEM_POWER_STATUS.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information about the power status of the system.
	/// </summary>
	[StructLayout ( LayoutKind.Explicit, Size = 12 )]
	public struct SYSTEM_POWER_STATUS
	{
		/// <summary>
		/// AC power status.
		/// </summary>
		[FieldOffset ( 0 )]
		public ACPowerStatus ACLineStatus;

		/// <summary>
		/// Battery charge status.
		/// </summary>
		[FieldOffset ( 1 )]
		public BatteryFlags BatteryFlag;

		/// <summary>
		/// Percentage of full battery charge remaining. 
		/// This member can be a value in the range 0 to 100, 
		/// or 255 if status is unknown.
		/// </summary>
		[FieldOffset ( 2 )]
		public byte BatteryLifePercent;

		/// <summary>
		/// Reserved; must be zero.
		/// </summary>
		[FieldOffset ( 3 )]
		public byte Reserved1;

		/// <summary>
		/// Number of seconds of battery life remaining, 
		/// or –1 if remaining seconds are unknown.
		/// </summary>
		[FieldOffset ( 4 )]
		public int BatteryLifeTime;

		/// <summary>
		/// Number of seconds of battery life when at full charge, 
		/// or –1 if full battery lifetime is unknown.
		/// </summary>
		[FieldOffset ( 8 )]
		public int BatteryFullLifeTime;
	}
}