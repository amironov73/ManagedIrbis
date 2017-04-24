// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringBuilderCompatibility.cs -- compatibility with StringBuilder
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: poor
 */

#if FW35

#region Using directives

using System.Text;

using JetBrains.Annotations;

#endregion

namespace Compatibility
{

    /// <summary>
    /// Compatibility with <see cref="StringBuilder"/>.
    /// </summary>
    [PublicAPI]
    public static class StringBuilderCompatibility
    {
        #region Public methods

        /// <summary>
        /// Removes all characters from the given
        /// <see cref="StringBuilder"/> instance.
        /// </summary>
        [NotNull]
        public static StringBuilder Clear
            (
                [NotNull] this StringBuilder builder
            )
        {
            builder.Length = 0;

            return builder;
        }

        #endregion
    }

}

#endif
