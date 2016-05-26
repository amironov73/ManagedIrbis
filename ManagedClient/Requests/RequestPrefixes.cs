/* RequestPrefixes.cs
  * Ars Magna project, http://arsmagna.ru
*/

namespace ManagedClient.Requests
{
    public class RequestPrefixes
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
        /// Отказы.
        /// </summary>
        public const string Refused = "I=2";

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
