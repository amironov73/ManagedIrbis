// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RequestRefusals.cs - причины отказов
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Requests
{
    /// <summary>
    /// Причины отказов.
    /// </summary>
    public static class RequestRefusals
    {
        #region Constants

        /// <summary>
        /// Занято.
        /// </summary>
        public const string Busy = "01";

        /// <summary>
        /// Нет на месте.
        /// </summary>
        public const string OutOfStock = "02";

        /// <summary>
        /// Нет на месте и по топокаталогу.
        /// </summary>
        public const string OutOfStockAndTopo = "03";

        /// <summary>
        /// В переплете.
        /// </summary>
        public const string Bound = "04";

        /// <summary>
        /// Списано.
        /// </summary>
        public const string WrittenOff = "05";
        
        /// <summary>
        /// Лакуна.
        /// </summary>
        public const string Lacuna = "06";

        /// <summary>
        /// Дефектный.
        /// </summary>
        public const string Defect = "07";

        /// <summary>
        /// Ветхий.
        /// </summary>
        public const string Decrepit = "08";

        /// <summary>
        /// Перенаправление.
        /// </summary>
        public const string Redirect = "09";

        /// <summary>
        /// Прочее.
        /// </summary>
        public const string Other = "*";

        #endregion
    }
}
