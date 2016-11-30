// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
    {
        /// <summary>
        /// Полный уровень описания.
        /// </summary>
        Full = (int)' ',

        /// <summary>
        /// Полный уровень, но запись не была проверена.
        /// </summary>
        FullNotChecked = (int)'1',

        /// <summary>
        /// Не окончательно созданная запись.
        /// </summary>
        NotComplete = (int)'5',

        /// <summary>
        /// Минимальный уровень.
        /// </summary>
        Minimal = (int)'7',

        /// <summary>
        /// Запись сделана по тематическому плану издательства.
        /// </summary>
        Publisher = (int)'8',

        /// <summary>
        /// Уровень неизвестен.
        /// </summary>
        Unknown = (int)'u',

        /// <summary>
        /// Невозможно установить уровень записи.
        /// </summary>
        NotAvailable = (int)'z'
    }
}
