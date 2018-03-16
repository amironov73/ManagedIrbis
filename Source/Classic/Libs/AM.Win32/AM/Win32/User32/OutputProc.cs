// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OutputProc.cs -- application-defined callback function used with the GrayString function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The OutputProc function is an application-defined callback
    /// function used with the GrayString function. It is used to
    /// draw a string. The GRAYSTRINGPROC type defines a pointer
    /// to this callback function. OutputProc is a placeholder for
    /// the application-defined or library-defined function name.
    /// </summary>
    /// <param name="hdc">Handle to a device context with a bitmap
    /// of at least the width and height specified by the nWidth and
    /// nHeight parameters passed to GrayString.</param>
    /// <param name="lpData">Pointer to the string to be drawn.</param>
    /// <param name="cchData">Specifies the length, in characters,
    /// of the string.</param>
    /// <returns>If it succeeds, the callback function should return TRUE.
    /// </returns>
    [PublicAPI]
    public delegate bool OutputProc
        (
            IntPtr hdc,
            IntPtr lpData,
            int cchData
        );
}
