// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstControlRecord64.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Первая запись в файле документов – управляющая 
    /// запись, которая формируется (в момент определения 
    /// базы данных или при ее инициализации) и поддерживается 
    /// автоматически.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MstControlRecord64
    {
        #region Constants

        /// <summary>
        /// Размер управляющей записи.
        /// </summary>
        public const int RecordSize = 32;

        /// <summary>
        /// Позиция индикатора блокировки базы данных
        /// в управляющей записи.
        /// </summary>
        public const long LockFlagPosition = 28;

        #endregion

        #region Properties

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv1 { get; set; }

        /// <summary>
        /// Номер записи файла документов, назначаемый 
        /// для следующей записи, создаваемой в базе данных.
        /// </summary>
        public int NextMfn { get; set; }

        /// <summary>
        /// Смещение свободного места в файле; (всегда указывает
        /// на конец файла MST).
        /// </summary>
        public long NextPosition { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv2 { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv3 { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv4 { get; set; }

        /// <summary>
        /// Индикатор блокировки базы данных.
        /// </summary>
        public int Blocked { get; set; }

        #endregion
    }
}
