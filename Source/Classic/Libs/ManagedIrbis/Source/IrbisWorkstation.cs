// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisWorkstation.cs -- коды АРМов ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
