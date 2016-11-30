// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MarcBibliographicalIndex.cs -- 
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
    /// Библиографический указатель.
    /// </summary>
    [PublicAPI]
    public enum MarcBibliographicalIndex
    {
        /// <summary>
        /// Часть монографии.
        /// </summary>
        PartOfMonograph = (int)'a',

        /// <summary>
        /// Часть сериального издания.
        /// </summary>
        PartOfSerialPrinting = (int)'b',

        /// <summary>
        /// Коллекция.
        /// </summary>
        Collection = (int)'c',

        /// <summary>
        /// Часть коллекции.
        /// </summary>
        PartOfCollection = (int)'d',

        /// <summary>
        /// Монография (книга, рукопись, картина и т.д.).
        /// </summary>
        Monograph = (int)'m',

        /// <summary>
        /// Сериальное издание, периодика.
        /// </summary>
        SerialPrinting = (int)'s'
    }
}
