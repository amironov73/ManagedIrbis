// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchContext
    {
        #region Properties

        /// <summary>
        /// Search manager.
        /// </summary>
        [NotNull]
        public SearchManager Manager { get; private set; }

        /// <summary>
        /// Providr.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchContext
            (
                [NotNull] SearchManager manager,
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(manager, "manager");
            Code.NotNull(provider, "provider");

            Manager = manager;
            Provider = provider;
        }

        #endregion
    }
}
