// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldState.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Состояние поля записи: не изменилось, добавлено,
    /// удалено, редактировано.
    /// </summary>
    public enum FieldState
    {
        /// <summary>
        /// Unchanged.
        /// </summary>
        Unchanged,

        /// <summary>
        /// Edited.
        /// </summary>
        Edited,

        /// <summary>
        /// Added.
        /// </summary>
        Added,

        /// <summary>
        /// Removed.
        /// </summary>
        Removed
    }
}
