// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PictureViewMode.cs -- picture view mode.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Image view mode for <see cref="PictureViewForm"/>.
    /// </summary>
    [PublicAPI]
    public enum PictureViewMode
    {
        /// <summary>
        /// Automatic mode.
        /// </summary>
        Auto,

        /// <summary>
        /// Fit form to picture size.
        /// </summary>
        Fit,

        /// <summary>
        /// Scroll picture.
        /// </summary>
        Scroll
    }
}
