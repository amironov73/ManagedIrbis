// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationStatus.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Статус компьютера для резервирования.
    /// </summary>
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public static class ReservationStatus
    {
        #region Constants

        /// <summary>
        /// Свободен, доступен для резервирования.
        /// </summary>
        public const string Available = "0";

        /// <summary>
        /// Свободен, доступен для резервирования.
        /// </summary>
        public const string Free = "0";

        /// <summary>
        /// Занят.
        /// </summary>
        public const string Busy = "1";

        /// <summary>
        /// Зарезервирован (заказан), но пока не занят.
        /// </summary>
        public const string Reserved = "9";

        /// <summary>
        /// Недоступен для резервирования.
        /// </summary>
        public const string NotAvailable = "5";

        #endregion
    }
}
