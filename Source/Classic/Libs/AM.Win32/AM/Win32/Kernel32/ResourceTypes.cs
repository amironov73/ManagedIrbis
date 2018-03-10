// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ResourceTypes.cs -- WIN32 resource types
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// WIN32 resource types.
    /// </summary>
    [PublicAPI]
    public enum ResourceTypes
    {
        /// <summary>
        /// Hardware-dependent cursor resource.
        /// </summary>
        RT_CURSOR = 1,

        /// <summary>
        /// Bitmap resource.
        /// </summary>
        RT_BITMAP = 2,

        /// <summary>
        /// Hardware-dependent icon resource.
        /// </summary>
        RT_ICON = 3,

        /// <summary>
        /// Menu resource.
        /// </summary>
        RT_MENU = 4,

        /// <summary>
        /// Dialog box.
        /// </summary>
        RT_DIALOG = 5,

        /// <summary>
        /// String-table entry.
        /// </summary>
        RT_STRING = 6,

        /// <summary>
        /// Font directory resource.
        /// </summary>
        RT_FONTDIR = 7,

        /// <summary>
        /// Font resource.
        /// </summary>
        RT_FONT = 8,

        /// <summary>
        /// Accelerator table.
        /// </summary>
        RT_ACCELERATOR = 9,

        /// <summary>
        /// Application-defined resource (raw data).
        /// </summary>
        RT_RCDATA = 10,

        /// <summary>
        /// Message-table entry.
        /// </summary>
        RT_MESSAGETABLE = 11,

        /// <summary>
        /// Hardware-independent cursor resource.
        /// </summary>
        RT_GROUP_CURSOR = 12,

        /// <summary>
        /// Hardware-independent icon resource.
        /// </summary>
        RT_GROUP_ICON = 14,

        /// <summary>
        /// Version resource.
        /// </summary>
        RT_VERSION = 16,

        /// <summary>
        /// <para>Allows a resource editing tool to associate a string 
        /// with an .rc file. Typically, the string is the name of the 
        /// header file that provides symbolic names. The resource 
        /// compiler parses the string but otherwise ignores the value. 
        /// For example,</para>
        /// <code>1 DLGINCLUDE "foo.h"</code>
        /// </summary>
        RT_DLGINCLUDE = 17,

        /// <summary>
        /// Plug and Play resource.
        /// </summary>
        RT_PLUGPLAY = 19,

        /// <summary>
        /// VXD.
        /// </summary>
        RT_VXD = 20,

        /// <summary>
        /// Animated cursor.
        /// </summary>
        RT_ANICURSOR = 21,

        /// <summary>
        /// Animated icon.
        /// </summary>
        RT_ANIICON = 22,

        /// <summary>
        /// HTML.
        /// </summary>
        RT_HTML = 23,

        /// <summary>
        /// Microsoft® Windows® XP: Side-by-Side Assembly XML Manifest.
        /// </summary>
        RT_MANIFEST = 24
    }
}
