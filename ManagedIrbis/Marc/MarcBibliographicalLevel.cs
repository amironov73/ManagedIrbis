/* MarcBibliographicalLevel.cs -- 
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
    /// Уровень кодирования библиографического описания.
    /// </summary>
    [PublicAPI]
    public enum MarcBibliographicalLevel
        : byte
    {
        /// <summary>
        /// Полный уровень описания.
        /// </summary>
        Full = (byte)' ',

        /// <summary>
        /// Полный уровень, но запись не была проверена.
        /// </summary>
        FullNotChecked = (byte)'1',

        /// <summary>
        /// Не окончательно созданная запись.
        /// </summary>
        NotComplete = (byte)'5',

        /// <summary>
        /// Минимальный уровень.
        /// </summary>
        Minimal = (byte)'7',

        /// <summary>
        /// Запись сделана по тематическому плану издательства.
        /// </summary>
        Publisher = (byte)'8',

        /// <summary>
        /// Уровень неизвестен.
        /// </summary>
        Unknown = (byte)'u',

        /// <summary>
        /// Невозможно установить уровень записи.
        /// </summary>
        NotAvailable = (byte)'z'
    }
}
