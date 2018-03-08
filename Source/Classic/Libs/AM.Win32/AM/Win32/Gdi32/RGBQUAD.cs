// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RGBQUAD.cs -- describes a color consisting of relative intensities of red, green, and blue
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The RGBQUAD structure describes a color consisting
    /// of relative intensities of red, green, and blue.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        /// <summary>
        /// Specifies the intensity of blue in the color.
        /// </summary>
        public byte rgbBlue;

        /// <summary>
        /// Specifies the intensity of green in the color.
        /// </summary>
        public byte rgbGreen;

        /// <summary>
        /// Specifies the intensity of red in the color.
        /// </summary>
        public byte rgbRed;

        /// <summary>
        /// Reserved; must be zero.
        /// </summary>
        public byte rgbReserved;
    }
}
