/* InventoryResult.cs -- результат проверки номера по базе.
 */

namespace CardFixer
{
    /// <summary>
    /// Результат проверки номера по базе.
    /// </summary>
    public class InventoryResult
    {
        #region Properties

        /// <summary>
        /// Номер.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public InventoryStatus Status { get; set; }

        /// <summary>
        /// Текст, описывающий статус.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}", 
                    Number, 
                    Status
                );
        }

        #endregion
    }
}
