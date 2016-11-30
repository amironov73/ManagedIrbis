// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MarcRecordStatus.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Статус записи.
    /// </summary>
    [PublicAPI]
    public enum MarcRecordStatus
    {
        /// <summary>
        /// При изменении записи увеличивается ее уровень.
        /// </summary>
        LevelUp = (int)'a',

        /// <summary>
        /// При изменении записи уровень остался прежним.
        /// </summary>
        Changed = (int)'c',

        /// <summary>
        /// Запись удалена, но информация о ней сохраняется.
        /// </summary>
        Deleted = (int)'d',

        /// <summary>
        /// Новая запись.
        /// </summary>
        New = (int)'n',

        /// <summary>
        /// Запись изменилась после того, как была введена с плана издательства.
        /// </summary>
        Publisher = (int)'p'
    }
}
