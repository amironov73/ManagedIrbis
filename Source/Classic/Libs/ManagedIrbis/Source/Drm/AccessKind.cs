// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AccessKind.cs --
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
    /// Значение доступа.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AccessKind
    {
        #region Constants

        /// <summary>
        /// Запрет доступа.
        /// </summary>
        public const string Denied = "0";

        /// <summary>
        /// Постраничный просмотр.
        /// </summary>
        public const string PageView = "1";

        /// <summary>
        /// Скачивание.
        /// </summary>
        public const string Download = "2";

        #endregion
    }
}