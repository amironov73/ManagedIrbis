// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RequestPrefixes.cs -- префиксы запросов к базе RDR
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Requests
{
    /// <summary>
    /// Префиксы стандартных запросов к базе данных RDR.
    /// </summary>
    public static class RequestPrefixes
    {
        #region Constants

        /// <summary>
        /// Статус заказа.
        /// </summary>
        public const string Status = "I=";

        /// <summary>
        /// Невыполненные заказы.
        /// </summary>
        public const string Unfulfilled = "I=0";

        /// <summary>
        /// Выполненные заказы.
        /// </summary>
        public const string Fulfilled = "I=1";

        /// <summary>
        /// Забронированные.
        /// </summary>
        public const string Reserved = "I=2";

        /// <summary>
        /// Отказы.
        /// </summary>
        public const string Refused = "I=3";

        /// <summary>
        /// Идентификатор читателя.
        /// </summary>
        public const string ReaderID = "RS=";

        /// <summary>
        /// Идентификкатор издания.
        /// </summary>
        public const string DocumentID = "IS=";

        /// <summary>
        /// Инвентарный номер или штрих-код экземпляра.
        /// </summary>
        public const string Number = "H=";

        /// <summary>
        /// Дата заказа.
        /// </summary>
        public const string OrderDate = "DZ=";

        /// <summary>
        /// Шифр выданной литературы.
        /// </summary>
        public const string LoanID = "C=";

        /// <summary>
        /// Описание выданной литературы.
        /// </summary>
        public const string Description = "N=";

        /// <summary>
        /// Дата выдачи.
        /// </summary>
        public const string LoanDate = "DV=";

        /// <summary>
        /// Дата возврата.
        /// </summary>
        public const string ReturnDate = "DW=";

        /// <summary>
        /// Посещаемость.
        /// </summary>
        public const string Attendance = "VS=";

        /// <summary>
        /// Утерянные экземпляры.
        /// </summary>
        public const string Lost = "HU=";

        /// <summary>
        /// Читатели с отрицательным балансом.
        /// </summary>
        public const string Debtor = "S=";

        #endregion
    }
}
