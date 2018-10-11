// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LimitKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Единица ограничения.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class LimitKind
    {
        #region Constants

        /// <summary>
        /// Страница.
        /// </summary>
        public const string Page = "";

        /// <summary>
        /// Процент.
        /// </summary>
        public const string Percent = "%";

        #endregion
    }
}