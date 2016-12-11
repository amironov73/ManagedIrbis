// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WindowPlacement.cs -- form window placement enumeration
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="Form"/> window placement enumeration.
    /// </summary>
    public enum WindowPlacement
    {
        /// <summary>
        /// Center of the screen.
        /// </summary>
        [Description("Screen center")]
        ScreenCenter,

        /// <summary>
        /// Top left corner.
        /// </summary>
        [Description("Top left corner")]
        TopLeftCorner,

        /// <summary>
        /// Top right corner.
        /// </summary>
        [Description("Top right corner")]
        TopRightCorner,

        /// <summary>
        /// Center of the top side.
        /// </summary>
        [Description("Center of the top side")]
        TopSide,

        /// <summary>
        /// Center of the left side.
        /// </summary>
        [Description("Center of the left side")]
        LeftSide,

        /// <summary>
        /// Center of the right side.
        /// </summary>
        [Description("Center of the right side")]
        RightSide,

        /// <summary>
        /// Center of the bottom side.
        /// </summary>
        [Description("Center of the bottom side")]
        BottomSide,

        /// <summary>
        /// Bottom left corner.
        /// </summary>
        [Description("Bottom left corner")]
        BottomLeftCorner,

        /// <summary>
        /// Bottom right corner.
        /// </summary>
        [Description("Bottom right corner")]
        BottomRightCorner
    }
}