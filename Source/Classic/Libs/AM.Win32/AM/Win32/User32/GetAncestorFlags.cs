// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GetAncestorFlags.cs -- specifies the ancestor to be retrieved
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the ancestor to be retrieved.
    /// </summary>
    [PublicAPI]
    public enum GetAncestorFlags
    {
        /// <summary>
        /// Retrieves the parent window. This does not include the
        /// owner, as it does with the GetParent function.
        /// </summary>
        GA_PARENT = 1,

        /// <summary>
        /// Retrieves the root window by walking the chain of parent
        /// windows.
        /// </summary>
        GA_ROOT = 2,

        /// <summary>
        /// Retrieves the owned root window by walking the chain
        /// of parent and owner windows returned by GetParent.
        /// </summary>
        GA_ROOTOWNER = 3
    }
}
