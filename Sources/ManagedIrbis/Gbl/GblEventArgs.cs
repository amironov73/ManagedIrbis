/* GblEventArgs.cs -- event arguments for GlobalCorrecor
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Event arguments for <see cref="GlobalCorrector"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// <see cref="GlobalCorrector"/>
        /// </summary>
        [NotNull]
        public GlobalCorrector Corrector { get; set; }

        /// <summary>
        /// Whether the execution canceled.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblEventArgs
            (
                [NotNull] GlobalCorrector corrector
            )
        {
            Code.NotNull(corrector, "corrector");

            Corrector = corrector;
        }

        #endregion
    }
}
