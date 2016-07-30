/* MarcCatalogingRules.cs -- 
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
    /// Код правил описания записи.
    /// </summary>
    [PublicAPI]
    public enum MarcCatalogingRules
        : byte
    {
        /// <summary>
        /// Не соответствуте ISBD, AACR2
        /// </summary>
        NotConforming = (byte)' ',

        /// <summary>
        /// Соответствует AACR2.
        /// </summary>
        AACR2 = (byte)'a',

        /// <summary>
        /// Соответствует ISBD.
        /// </summary>
        ISBD = (byte)'i'
    }
}
