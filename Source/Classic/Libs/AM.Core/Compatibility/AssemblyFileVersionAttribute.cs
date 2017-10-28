// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AssemblyFileVersionAttribute.cs -- for WinMobile compatibility
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE

namespace System.Reflection
{
    /// <summary>
    /// For WinMobile compatibility.
    /// </summary>
    public sealed class AssemblyFileVersionAttribute
        : Attribute
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AssemblyFileVersionAttribute
            (
                string version
            )
        {
        }

        #endregion
    }
}

#endif
