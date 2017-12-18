// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Semester.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// Enum that represents semesters (half-years).
    /// </summary>
    [Flags]
    public enum Semester
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// First semester.
        /// </summary>
        Semester1 = 1,

        /// <summary>
        /// Second semester.
        /// </summary>
        Semester2 = 2,

        /// <summary>
        /// Third semester.
        /// </summary>
        Semester3 = 4,

        /// <summary>
        /// 
        /// </summary>
        Semester4 = 8,

        /// <summary>
        /// 
        /// </summary>
        Semester5 = 16,

        /// <summary>
        /// 
        /// </summary>
        Semester6 = 32,

        /// <summary>
        /// 
        /// </summary>
        Semester7 = 64,

        /// <summary>
        /// 
        /// </summary>
        Semester8 = 128,

        /// <summary>
        /// 
        /// </summary>
        Semester9 = 256,

        /// <summary>
        /// 
        /// </summary>
        Semester10 = 512,

        /// <summary>
        /// 
        /// </summary>
        Semester11 = 1024,

        /// <summary>
        /// 
        /// </summary>
        Semester12 = 2048,

        /// <summary>
        /// 
        /// </summary>
        NumSemesters = 12,

        /// <summary>
        /// 
        /// </summary>
        OddSemesters = Semester1 | Semester3 | Semester5
                       | Semester7 | Semester9 | Semester11,

        /// <summary>
        /// 
        /// </summary>
        EvenSemesters = Semester2 | Semester4 | Semester6
                        | Semester8 | Semester10 | Semester12,

        /// <summary>
        /// Все семестры
        /// </summary>
        All = 0xFFF
    }
}
