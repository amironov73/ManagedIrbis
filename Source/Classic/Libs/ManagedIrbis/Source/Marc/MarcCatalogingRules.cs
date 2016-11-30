// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
    {
        /// <summary>
        /// Не соответствуте ISBD, AACR2
        /// </summary>
        NotConforming = (int)' ',

        /// <summary>
        /// Соответствует AACR2.
        /// </summary>
        AACR2 = (int)'a',

        /// <summary>
        /// Соответствует ISBD.
        /// </summary>
        ISBD = (int)'i'
    }
}
