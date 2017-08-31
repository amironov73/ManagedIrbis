// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogingRules.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Правила каталогизации в соответствии с 919g.mnu.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class CatalogingRules
    {
        #region Constants

        /// <summary>
        /// Anglo-American cataloguing rules.
        /// </summary>
        public const string AACR2 = "AACR2";

        /// <summary>
        /// Library of Congress (USA).
        /// </summary>
        public const string BDRB = "BDRB";

        /// <summary>
        /// Российские "Правила составления библиографического описания".
        /// </summary>
        public const string PSBO = "PSBO";

        /// <summary>
        /// Российские "Правила каталогизации" (РПК).
        /// (Москва, 2005).
        /// </summary>
        public const string RCR = "RCR";

        #endregion
    }
}
