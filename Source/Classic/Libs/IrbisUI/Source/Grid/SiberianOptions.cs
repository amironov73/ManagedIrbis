// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianOptions.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// Options for <see cref="SiberianColumn"/>.
    /// </summary>
    [Flags]
    public enum SiberianOptions
    {
        /// <summary>
        /// Editable.
        /// </summary>
        Editable = 0x01,

        /// <summary>
        /// Selectable.
        /// </summary>
        Selectable = 0x02,

        /// <summary>
        /// Resizeable.
        /// </summary>
        Resizeable = 0x04
    }
}
