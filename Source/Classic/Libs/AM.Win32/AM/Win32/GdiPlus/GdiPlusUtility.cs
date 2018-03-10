// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GdiPlusUtility.cs -- some useful methods from GDI+
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Some useful methods from GDI+
    /// </summary>
    [PublicAPI]
    public static class GdiPlusUtility
    {
        #region Public methods

        /// <summary>
        /// Gets the bitmap from GDI plus.
        /// </summary>
        [NotNull]
        public static Bitmap GetBitmapFromGdiPlus
            (
                IntPtr gdiPlusBitmap
            )
        {
            MethodInfo method = typeof(Bitmap).GetMethod
                (
                    "FromGDIplus",
                    BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    new [] { typeof(IntPtr) },
                    null
                );
            Bitmap result = (Bitmap)method.Invoke
                (
                    null,
                    new object[] { gdiPlusBitmap }
                );

            return result;
        }

        #endregion
    }
}
