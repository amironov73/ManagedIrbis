// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DigestAuthenticator.cs -- костыль, позволяющий аутентифицироваться методом Digest
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System.Net;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using RestSharp;
using RestSharp.Authenticators;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// Authenticator for Digest method.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DigestAuthenticator 
        : IAuthenticator
    {
        #region Properties

        /// <summary>
        /// User name.
        /// </summary>
        [NotNull]
        public string UserName { get; }

        /// <summary>
        /// Password.
        /// </summary>
        [NotNull]
        public string Password { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DigestAuthenticator
            (
                [NotNull] string username, 
                [NotNull] string password
            )
        {
            Code.NotNull(username, nameof(username));
            Code.NotNull(password, nameof(password));

            UserName = username;
            Password = password;
        }

        #endregion

        #region IAuthenticator members

        /// <inheritdoc/>
        public void Authenticate
            (
                IRestClient client,
                IRestRequest request
            )
        {
            request.Credentials = new NetworkCredential
                (
                    UserName, 
                    Password
                );
        }

        #endregion
    }
}

#endif
