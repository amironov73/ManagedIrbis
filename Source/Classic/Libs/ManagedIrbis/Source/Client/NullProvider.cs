// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullProvider.cs -- null provider used for testing
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Null provider used for testing.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NullProvider
        : IrbisProvider
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NullProvider()
        {
            Database = "IBIS";
        }

        #endregion
    }
}
