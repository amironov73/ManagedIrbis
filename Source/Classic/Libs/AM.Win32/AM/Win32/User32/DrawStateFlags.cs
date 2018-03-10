// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DrawStateFlags.cs -- specifies the image type and state for DrawState
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the image type and state for DrawState().
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DrawStateFlags
    {
        /// <summary>
        /// The image is application defined. To render the image,
        /// DrawState calls the callback function specified by the
        /// lpOutputFunc parameter.
        /// </summary>
        DST_COMPLEX = 0x0000,

        /// <summary>
        /// The image is text. The lData parameter is a pointer to the
        /// string, and the wData parameter specifies the length.
        /// If wData is zero, the string must be null-terminated.
        /// </summary>
        DST_TEXT = 0x0001,

        /// <summary>
        /// The image is text that may contain an accelerator mnemonic.
        /// DrawState interprets the ampersand (&amp;) prefix character as
        /// a directive to underscore the character that follows. The
        /// lData parameter is a pointer to the string, and the wData
        /// parameter specifies the length. If wData is zero, the string
        /// must be null-terminated.
        /// </summary>
        DST_PREFIXTEXT = 0x0002,

        /// <summary>
        /// The image is an icon. The lData parameter is the icon handle.
        /// </summary>
        DST_ICON = 0x0003,

        /// <summary>
        /// The image is a bitmap. The lData parameter is the
        /// bitmap handle. Note that the bitmap cannot already
        /// be selected into an existing device context.
        /// </summary>
        DST_BITMAP = 0x0004,

        /// <summary>
        /// Draws the image without any modification.
        /// </summary>
        DSS_NORMAL = 0x0000,

        /// <summary>
        /// Dithers the image.
        /// </summary>
        DSS_UNION = 0x0010,

        /// <summary>
        /// Embosses the image.
        /// </summary>
        DSS_DISABLED = 0x0020,

        /// <summary>
        /// Draws the image using the brush specified by the hbr parameter.
        /// </summary>
        DSS_MONO = 0x0080,

        /// <summary>
        /// Windows 2000/XP: Ignores the ampersand (&amp;) prefix character
        /// in the text, thus the letter that follows will not be
        /// underlined. This must be used with DST_PREFIXTEXT.
        /// </summary>
        DSS_HIDEPREFIX = 0x0200,

        /// <summary>
        /// Windows 2000/XP: Draws only the underline at the position
        /// of the letter after the ampersand (&amp;) prefix character.
        /// No text in the string is drawn. This must be used with
        /// DST_PREFIXTEXT.
        /// </summary>
        DSS_PREFIXONLY = 0x0400,

        /// <summary>
        /// Aligns the text to the right.
        /// </summary>
        DSS_RIGHT = 0x8000
    }
}
