/* SearchLogicType.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Какие логические операторы могут использоваться
    /// для данного вида поиска.
    /// </summary>
    public enum SearchLogicType
    {
        /// <summary>
        /// только логика ИЛИ
        /// </summary>
        Or = 0,

        /// <summary>
        /// логика ИЛИ и И
        /// </summary>
        OrAnd = 1,

        /// <summary>
        /// логика ИЛИ, И, НЕТ (по умолчанию)
        /// </summary>
        OrAndNot = 2,

        /// <summary>
        /// логика ИЛИ, И, НЕТ, И (в поле)
        /// </summary>
        OrAndNotField = 3,

        /// <summary>
        /// логика ИЛИ, И, НЕТ, И (в поле), И (фраза)
        /// </summary>
        OrAndNotPhrase = 4
    }
}
