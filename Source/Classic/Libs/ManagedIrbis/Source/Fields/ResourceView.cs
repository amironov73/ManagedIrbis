// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ResourceView.cs -- 
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
    /// Характер просмотра двоичного ресурса. См. xp.mnu
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ResourceView
    {
        #region Constants

        /// <summary>
        /// Контекстный (есть указание в полях данных).
        /// </summary>
        public const string Context = "0";

        /// <summary>
        /// Неконтекстный.
        /// </summary>
        public const string NonContext = "1";

        /// <summary>
        /// Обложка.
        /// </summary>
        public const string Cover = "2";

        /// <summary>
        /// Неизвестно
        /// </summary>
        public const string Unknown = "";

        #endregion
    }
}
