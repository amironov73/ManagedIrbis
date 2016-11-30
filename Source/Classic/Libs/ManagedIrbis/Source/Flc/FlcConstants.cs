// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FlcConstants.cs -- formal control related constants
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// Formal control related constants.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FlcConstants
    {
        #region Constants

        /// <summary>
        /// Check record update or creation.
        /// </summary>
        public const string DbnFlc = "@dbnflc";

        /// <summary>
        /// Check record deletion.
        /// </summary>
        public const string DelFlc = "@delflc";

        #endregion
    }
}
