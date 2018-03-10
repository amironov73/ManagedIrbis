// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListViewAlignment.cs -- ListView items alignment
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// ListView items alignment.
    /// </summary>
    [PublicAPI]
    public enum ListViewAlignment
    {
        /// <summary>
        /// Aligns items according to the list-view control's current 
        /// alignment styles (the default value).
        /// </summary>
        LVA_DEFAULT = 0x0000,

        /// <summary>
        /// Aligns items along the left edge of the window.
        /// </summary>
        LVA_ALIGNLEFT = 0x0001,

        /// <summary>
        /// Aligns items along the top edge of the window.
        /// </summary>
        LVA_ALIGNTOP = 0x0002,

        /// <summary>
        /// Snaps all icons to the nearest grid position.
        /// </summary>
        LVA_SNAPTOGRID = 0x0005
    }
}
