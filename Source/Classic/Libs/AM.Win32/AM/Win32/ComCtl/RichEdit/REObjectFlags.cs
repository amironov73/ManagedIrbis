// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* REObjectFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    [CLSCompliant ( false )]
    public enum REObjectFlags : uint
    {
        /// <summary>
        /// No flags.
        /// </summary>
        REO_NULL = 0x00000000,

        /// <summary>
        /// Mask out RO bits.
        /// </summary>
        REO_READWRITEMASK = 0x0000003F,

        /// <summary>
        /// Object doesn't need palette.
        /// </summary>
        REO_DONTNEEDPALETTE = 0x00000020,

        /// <summary>
        /// Object is blank.
        /// </summary>
        REO_BLANK = 0x00000010,

        /// <summary>
        /// Object defines size always.
        /// </summary>
        REO_DYNAMICSIZE = 0x00000008,

        /// <summary>
        /// Object drawn all inverted if sel.
        /// </summary>
        REO_INVERTEDSELECT = 0x00000004,

        /// <summary>
        /// Object sits below the baseline.
        /// </summary>
        REO_BELOWBASELINE = 0x00000002,

        /// <summary>
        /// Object may be resized.
        /// </summary>
        REO_RESIZABLE = 0x00000001,

        /// <summary>
        /// Object is a link (RO).
        /// </summary>
        REO_LINK = 0x80000000,

        /// <summary>
        /// Object is static (RO).
        /// </summary>
        REO_STATIC = 0x40000000,

        /// <summary>
        /// Object selected (RO).
        /// </summary>
        REO_SELECTED = 0x08000000,

        /// <summary>
        /// Object open in its server (RO).
        /// </summary>
        REO_OPEN = 0x04000000,

        /// <summary>
        /// Object in place active (RO).
        /// </summary>
        REO_INPLACEACTIVE = 0x02000000,

        /// <summary>
        /// Object is to be hilited (RO).
        /// </summary>
        REO_HILITED = 0x01000000,

        /// <summary>
        /// Link believed available (RO).
        /// </summary>
        REO_LINKAVAILABLE = 0x00800000,

        /// <summary>
        /// Object requires metafile (RO).
        /// </summary>
        REO_GETMETAFILE = 0x00400000
    }
}
