/* IrbisWorkstation.cs -- коды АРМов ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Коды АРМов ИРБИС.
    /// </summary>
    public enum IrbisWorkstation
        : byte
    {
        /// <summary>
        /// Администратор
        /// </summary>
        Administrator = (byte)'A',

        /// <summary>
        /// Каталогизатор
        /// </summary>
        Cataloger = (byte)'C',

        /// <summary>
        /// Комплектатор
        /// </summary>
        Acquisitions = (byte)'M',

        /// <summary>
        /// Читатель
        /// </summary>
        Reader = (byte)'R',

        /// <summary>
        /// Книговыдача
        /// </summary>
        Circulation = (byte)'B',

        /// <summary>
        /// Тоже книговыдача
        /// </summary>
        Bookland = (byte)'B',

        /// <summary>
        /// Книгообеспеченность
        /// </summary>
        Provision = (byte)'K',

        /// <summary>
        /// Не задан
        /// </summary>
        None = 0
    }
}
