// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GetDCFlags.cs -- specifies how the DC is created
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies how the DC is created.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum GetDCFlags
    {
        /// <summary>
        /// Returns a DC that corresponds to the window rectangle rather
        /// than the client rectangle.
        /// </summary>
        DCX_WINDOW = 0x00000001,

        /// <summary>
        /// Returns a DC from the cache, rather than the OWNDC or CLASSDC
        /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.
        /// </summary>
        DCX_CACHE = 0x00000002,

        /// <summary>
        /// Does not reset the attributes of this DC to the default attributes
        /// when this DC is released.
        /// </summary>
        DCX_NORESETATTRS = 0x00000004,

        /// <summary>
        /// Excludes the visible regions of all child windows below the
        /// window identified by hWnd.
        /// </summary>
        DCX_CLIPCHILDREN = 0x00000008,

        /// <summary>
        /// Excludes the visible regions of all sibling windows above the
        /// window identified by hWnd.
        /// </summary>
        DCX_CLIPSIBLINGS = 0x00000010,

        /// <summary>
        /// Uses the visible region of the parent window. The parent's
        /// WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The
        /// origin is set to the upper-left corner of the window identified
        /// by hWnd.
        /// </summary>
        DCX_PARENTCLIP = 0x00000020,

        /// <summary>
        /// The clipping region identified by hrgnClip is excluded from the
        /// visible region of the returned DC.
        /// </summary>
        DCX_EXCLUDERGN = 0x00000040,

        /// <summary>
        /// The clipping region identified by hrgnClip is intersected with
        /// the visible region of the returned DC.
        /// </summary>
        DCX_INTERSECTRGN = 0x00000080,

        /// <summary>
        /// ???
        /// </summary>
        DCX_EXCLUDEUPDATE = 0x00000100,

        /// <summary>
        /// ???
        /// </summary>
        DCX_INTERSECTUPDATE = 0x00000200,

        /// <summary>
        /// Allows drawing even if there is a LockWindowUpdate call in effect
        /// that would otherwise exclude this window. Used for drawing during
        /// tracking.
        /// </summary>
        DCX_LOCKWINDOWUPDATE = 0x00000400,

        /// <summary>
        /// When specified with DCX_INTERSECTUPDATE, causes the DC to be
        /// completely validated. Using this function with both
        /// DCX_INTERSECTUPDATE and DCX_VALIDATE is identical to using the
        /// BeginPaint function.
        /// </summary>
        DCX_VALIDATE = 0x00200000
    }
}
