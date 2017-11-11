// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Delimiters.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Common delimiters.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Delimiters
    {
        #region Public members

        /// <summary>
        /// Colon.
        /// </summary>
        [NotNull]
        public static readonly char[] Colon = { ':' };

        /// <summary>
        /// Comma.
        /// </summary>
        [NotNull]
        public static readonly char[] Comma = { ',' };

        /// <summary>
        /// Dot.
        /// </summary>
        [NotNull]
        public static readonly char[] Dot = { '.' };

        /// <summary>
        /// Semicolon.
        /// </summary>
        [NotNull]
        public static readonly char[] Semicolon = { ';' };

        /// <summary>
        /// Space.
        /// </summary>
        [NotNull]
        public static readonly char[] Space = { ' ' };

        /// <summary>
        /// Tab.
        /// </summary>
        [NotNull]
        public static readonly char[] Tab = { '\t' };

        /// <summary>
        /// Pipe sign.
        /// </summary>
        [NotNull]
        public static readonly char[] Pipe = { '|' };

        #endregion
    }
}
