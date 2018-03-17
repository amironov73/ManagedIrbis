// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WinApiUtility.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.ComponentModel;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public static class WinApiUtility
    {
        #region Public methods

        /// <summary>
        /// Checks the specified result code.
        /// </summary>
        public static void Check
            (
                int resultCode
            )
        {
            if (resultCode < 0)
            {
                throw new Win32Exception();
            }
        }

        #endregion
    }
}
