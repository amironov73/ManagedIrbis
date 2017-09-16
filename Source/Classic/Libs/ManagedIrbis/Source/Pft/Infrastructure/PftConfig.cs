// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConfig.cs -- configuration for PFT scripting
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Configuration for PFT scripting
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftConfig
    {
        #region Constants

        /// <summary>
        /// Максимальное число повторений группы по умолчанию.
        /// </summary>
        public const int DefaultMaxRepeat = 500;

        #endregion

        #region Properties

        /// <summary>
        /// Немедленный выход из группы по break.
        /// </summary>
        public static bool BreakImmediate { get; set; }

        /// <summary>
        /// Максимальное число повторений группы.
        /// </summary>
        public static int MaxRepeat { get; set; }

        /// <summary>
        /// Выводить ли предупреждения?
        /// </summary>
        public static bool EnableWarnings { get; set; }

        #endregion

        #region Construction

        static PftConfig()
        {
            MaxRepeat = DefaultMaxRepeat;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
