// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextAlign.cs -- text alignment by using a mask of the values
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The text alignment by using a mask of the values in the following list.
    /// Only one flag can be chosen from those that affect horizontal
    /// and vertical alignment. In addition, only one of the two flags
    /// that alter the current position can be chosen.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum TextAlign
    {
        /// <summary>
        /// The current position is not updated after each text output call.
        /// The reference point is passed to the text output function.
        /// </summary>
        TA_NOUPDATECP = 0,

        /// <summary>
        /// The current position is updated after each text output call.
        /// The current position is used as the reference point.
        /// </summary>
        TA_UPDATECP = 1,

        /// <summary>
        /// The reference point will be on the left edge
        /// of the bounding rectangle.
        /// </summary>
        TA_LEFT = 0,

        /// <summary>
        /// The reference point will be on the right edge
        /// of the bounding rectangle.
        /// </summary>
        TA_RIGHT = 2,

        /// <summary>
        /// The reference point will be aligned horizontally
        /// with the center of the bounding rectangle.
        /// </summary>
        TA_CENTER = 6,

        /// <summary>
        /// The reference point will be on the top edge
        /// of the bounding rectangle.
        /// </summary>
        TA_TOP = 0,

        /// <summary>
        /// The reference point will be on the bottom edge
        /// of the bounding rectangle.
        /// </summary>
        TA_BOTTOM = 8,

        /// <summary>
        /// The reference point will be on the base line of the text.
        /// </summary>
        TA_BASELINE = 24,

        /// <summary>
        /// Middle East language edition of Windows: The text is laid
        /// out in right to left reading order, as opposed
        /// to the default left to right order.
        /// This applies only when the font selected into the device context
        /// is either Hebrew or Arabic.
        /// </summary>
        TA_RTLREADING = 256,

        /// <summary>
        /// ???
        /// </summary>
        TA_MASK = TA_BASELINE + TA_CENTER + TA_UPDATECP + TA_RTLREADING,

        /// <summary>
        /// The reference point will be on the base line of the text.
        /// </summary>
        VTA_BASELINE = TA_BASELINE,

        /// <summary>
        /// ???
        /// </summary>
        VTA_LEFT = TA_BOTTOM,

        /// <summary>
        /// ???
        /// </summary>
        VTA_RIGHT = TA_TOP,

        /// <summary>
        /// 
        /// </summary>
        VTA_CENTER = TA_CENTER,

        /// <summary>
        /// ???
        /// </summary>
        VTA_BOTTOM = TA_RIGHT,

        /// <summary>
        /// ???
        /// </summary>
        VTA_TOP = TA_LEFT
    }
}
