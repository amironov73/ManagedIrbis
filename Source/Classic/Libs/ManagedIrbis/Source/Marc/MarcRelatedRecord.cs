// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
