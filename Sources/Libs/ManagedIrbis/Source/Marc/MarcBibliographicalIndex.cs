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
        : byte
    {
        /// <summary>
        /// Часть монографии.
        /// </summary>
        PartOfMonograph = (byte)'a',

        /// <summary>
        /// Часть сериального издания.
        /// </summary>
        PartOfSerialPrinting = (byte)'b',

        /// <summary>
        /// Коллекция.
        /// </summary>
        Collection = (byte)'c',

        /// <summary>
        /// Часть коллекции.
        /// </summary>
        PartOfCollection = (byte)'d',

        /// <summary>
        /// Монография (книга, рукопись, картина и т.д.).
        /// </summary>
        Monograph = (byte)'m',

        /// <summary>
        /// Сериальное издание, периодика.
        /// </summary>
        SerialPrinting = (byte)'s'
    }
}
