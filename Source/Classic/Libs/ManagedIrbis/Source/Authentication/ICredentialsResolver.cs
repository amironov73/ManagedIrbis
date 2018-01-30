// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ICredentialsResolver.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Authentication
{
    /// <summary>
    /// Resolve missing connection elements.
    /// </summary>
    public interface ICredentialsResolver
    {
        /// <summary>
        /// Resolve the credentials.
        /// </summary>
        bool ResolveCredentials
            (
                [NotNull] IrbisCredentials credentials,
                ConnectionElement elements
            );
    }
}
