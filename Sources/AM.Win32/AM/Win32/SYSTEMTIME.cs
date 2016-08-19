/* SYSTEMTIME.cs -- represents a date and time 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The SYSTEMTIME structure represents a date and time using 
    /// individual members for the month, day, year, weekday, hour, 
    /// minute, second, and millisecond.
    /// </summary>
    [Serializable]
    [StructLayout ( LayoutKind.Sequential, Pack = 2 )]
    public struct SYSTEMTIME
    {
        /// <summary>
        /// <para>Current year. The year must be greater than 1601.
        /// </para>
        /// <para>Windows Server 2003, Windows XP: The year cannot 
        /// be greater than 30827.</para>
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wYear;

        /// <summary>
        /// Current month; January = 1, February = 2, and so on. 
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wMonth;

        /// <summary>
        /// Current day of the week; Sunday = 0, Monday = 1, and so on.
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wDayOfWeek;

        /// <summary>
        /// Current day of the month. 
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wDay;

        /// <summary>
        /// Current hour. 
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wHour;

        /// <summary>
        /// Current minute.
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wMinute;

        /// <summary>
        /// Current second.
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wSecond;

        /// <summary>
        /// Current millisecond.
        /// </summary>
        [CLSCompliant ( false )]
        public ushort wMilliseconds;
    }
}
