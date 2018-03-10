// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ImageListFlags.cs -- image list drawing style
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Image list drawing style.
    /// </summary>
    [PublicAPI]
    public enum ImageListFlags
    {
        /// <summary>
        /// Draws the image using the background color for the image list. 
        /// If the background color is the CLR_NONE value, the image is 
        /// drawn transparently using the mask.
        /// </summary>
        ILD_NORMAL = 0x00000000,

        /// <summary>
        /// Draws the image transparently using the mask, regardless of 
        /// the background color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_TRANSPARENT = 0x00000001,

        /// <summary>
        /// Draws the mask.
        /// </summary>
        ILD_MASK = 0x00000010,

        /// <summary>
        /// ???
        /// </summary>
        ILD_IMAGE = 0x00000020,

        /// <summary>
        /// ???
        /// </summary>
        ILD_ROP = 0x00000040,

        /// <summary>
        /// Draws the image, blending 25 percent with the system 
        /// highlight color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_BLEND25 = 0x00000002,

        /// <summary>
        /// Draws the image, blending 50 percent with the system 
        /// highlight color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_BLEND50 = 0x00000004,

        /// <summary>
        /// ???
        /// </summary>
        ILD_OVERLAYMASK = 0x00000F00,

        /// <summary>
        /// ???
        /// </summary>
        ILD_PRESERVEALPHA = 0x00001000,

        /// <summary>
        /// ???
        /// </summary>
        ILD_SCALE = 0x00002000,

        /// <summary>
        /// ???
        /// </summary>
        ILD_DPISCALE = 0x00004000,

        /// <summary>
        /// Draws the image, blending 50 percent with the system 
        /// highlight color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_SELECTED = ILD_BLEND50,

        /// <summary>
        /// Draws the image, blending 25 percent with the system 
        /// highlight color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_FOCUS = ILD_BLEND25,

        /// <summary>
        /// Draws the image, blending 50 percent with the system 
        /// highlight color. This value has no effect if the image 
        /// list does not contain a mask.
        /// </summary>
        ILD_BLEND = ILD_BLEND50
    }
}
