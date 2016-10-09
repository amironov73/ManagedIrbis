/* MarcRelatedRecord.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Необходимость связанной записи.
    /// </summary>
    [PublicAPI]
    public enum MarcRelatedRecord
    {
        /// <summary>
        /// Не требуется.
        /// </summary>
        NotRequired = (int)' ',

        /// <summary>
        /// Требуется.
        /// </summary>
        Required = (int)'r'
    }
}
