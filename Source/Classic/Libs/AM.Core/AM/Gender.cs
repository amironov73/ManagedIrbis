// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Gender.cs -- обозначение пола
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: good
 */

namespace AM
{
    /// <summary>
    /// Обозначение пола.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Неизвестен/не установлен.
        /// </summary>
        NotSet,

        /// <summary>
        /// Мужской.
        /// </summary>
        Male,

        /// <summary>
        /// Женский.
        /// </summary>
        Female
    }
}
