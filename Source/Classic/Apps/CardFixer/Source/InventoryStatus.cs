/* InventoryStatus.cs -- статус инвентарного номера
 */

namespace CardFixer
{
    /// <summary>
    /// Статус инвентарного номера
    /// по результатам проверки.
    /// </summary>
    public enum InventoryStatus
    {
        /// <summary>
        /// Неизвестен
        /// </summary>
        Unknown,

        /// <summary>
        /// Не найден
        /// </summary>
        NotFound,

        /// <summary>
        /// Найден
        /// </summary>
        Found,

        /// <summary>
        /// Списан
        /// </summary>
        WrittenOff,

        /// <summary>
        /// Проблемный
        /// </summary>
        Problem
    }
}
