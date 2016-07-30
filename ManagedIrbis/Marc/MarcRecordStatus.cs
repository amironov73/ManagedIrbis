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
        : byte
    {
        /// <summary>
        /// При изменении записи увеличивается ее уровень.
        /// </summary>
        LevelUp = (byte)'a',

        /// <summary>
        /// При изменении записи уровень остался прежним.
        /// </summary>
        Changed = (byte)'c',

        /// <summary>
        /// Запись удалена, но информация о ней сохраняется.
        /// </summary>
        Deleted = (byte)'d',

        /// <summary>
        /// Новая запись.
        /// </summary>
        New = (byte)'n',

        /// <summary>
        /// Запись изменилась после того, как была введена с плана издательства.
        /// </summary>
        Publisher = (byte)'p'
    }
}
