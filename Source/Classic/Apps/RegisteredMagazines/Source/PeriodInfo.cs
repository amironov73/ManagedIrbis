// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PeriodInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace RegisteredMagazines
{
    class PeriodInfo
    {
        #region Properties

        /// <summary>
        /// Заглавие журнала.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Зарегистрированные номера.
        /// </summary>
        public string Registered { get; set; }

        #endregion
    }
}
