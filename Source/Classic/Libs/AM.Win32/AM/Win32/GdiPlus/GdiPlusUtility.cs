// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GdiPlusUtility.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    public static class GdiPlusUtility
    {
        #region Private members
        #endregion

        #region Public methods

        /// <summary>
        /// Gets the bitmap from GDI plus.
        /// </summary>
        /// <param name="gdiPlusBitmap">The GDI plus bitmap.</param>
        /// <returns></returns>
        public static Bitmap GetBitmapFromGdiPlus(IntPtr gdiPlusBitmap)
        {
            MethodInfo method = typeof(Bitmap).GetMethod
                (
                    "FromGDIplus",
                    BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    new Type[] { typeof(IntPtr) },
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
