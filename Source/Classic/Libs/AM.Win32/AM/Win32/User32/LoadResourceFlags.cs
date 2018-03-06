// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoadResourceFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Flags for LoadImage and similar functions
    /// </summary>
    [Flags]
    public enum LoadResourceFlags
    {
        /// <summary>
        /// The default flag; it does nothing. All it means is 
        /// "not LR_MONOCHROME".
        /// </summary>
        LR_DEFAULTCOLOR = 0x0000,

        /// <summary>
        /// Loads the image in black and white.
        /// </summary>
        LR_MONOCHROME = 0x0001,

        /// <summary>
        /// ???
        /// </summary>
        LR_COLOR = 0x0002,

        /// <summary>
        /// ???
        /// </summary>
        LR_COPYRETURNORG = 0x0004,

        /// <summary>
        /// ???
        /// </summary>
        LR_COPYDELETEORG = 0x0008,

        /// <summary>
        /// Loads the image from the file specified by the lpszName 
        /// parameter. If this flag is not specified, lpszName is the 
        /// name of the resource.
        /// </summary>
        LR_LOADFROMFILE = 0x0010,

        /// <summary>
        /// <para>Retrieves the color value of the first pixel in the 
        /// image and replaces the corresponding entry in the color 
        /// table with the default window color (COLOR_WINDOW). All 
        /// pixels in the image that use that entry become the default 
        /// window color. This value applies only to images that have 
        /// corresponding color tables.</para>
        /// <para>Do not use this option if you are loading a bitmap 
        /// with a color depth greater than 8bpp.</para>
        /// </summary>
        LR_LOADTRANSPARENT = 0x0020,

        /// <summary>
        /// Uses the width or height specified by the system metric 
        /// values for cursors or icons, if the cxDesired or cyDesired 
        /// values are set to zero. If this flag is not specified and 
        /// cxDesired and cyDesired are set to zero, the function uses 
        /// the actual resource size. If the resource contains multiple 
        /// images, the function uses the size of the first image. 
        /// </summary>
        LR_DEFAULTSIZE = 0x0040,

        /// <summary>
        /// Uses true VGA colors.
        /// </summary>
        LR_VGACOLOR = 0x0080,

        /// <summary>
        /// Searches the color table for the image and replaces the 
        /// shades of gray with the corresponding 3-D color.
        /// </summary>
        LR_LOADMAP3DCOLORS = 0x1000,

        /// <summary>
        /// When the uType parameter specifies IMAGE_BITMAP, causes 
        /// the function to return a DIB section bitmap rather than a 
        /// compatible bitmap. This flag is useful for loading a bitmap 
        /// without mapping it to the colors of the display device.
        /// </summary>
        LR_CREATEDIBSECTION = 0x2000,

        /// <summary>
        /// ???
        /// </summary>
        LR_COPYFROMRESOURCE = 0x4000,

        /// <summary>
        /// Shares the image handle if the image is loaded multiple 
        /// times. If LR_SHARED is not set, a second call to LoadImage 
        /// for the same resource will load the image again and return 
        /// a different handle. When you use this flag, the system will 
        /// destroy the resource when it is no longer needed. 
        /// </summary>
        LR_SHARED = 0x8000
    }
}
