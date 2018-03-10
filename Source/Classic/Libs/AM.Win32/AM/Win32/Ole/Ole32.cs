// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Ole32.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public static class Ole32
    {
        #region Constants

        /// <summary>
        /// Name of the dynamic linking library.
        /// </summary>
        public const string DllName = "Ole32.dll";

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the bind CTX.
        /// </summary>
        [DllImport(DllName)]
        public static extern int CreateBindCtx
            (
                int reserved,
                out IBindCtx ppbc
            );

        /// <summary>
        /// Gets the running object table.
        /// </summary>
        [DllImport(DllName)]
        public static extern int GetRunningObjectTable
            (
                int reserved,
                out IRunningObjectTable prot
            );

        #endregion
    }
}
