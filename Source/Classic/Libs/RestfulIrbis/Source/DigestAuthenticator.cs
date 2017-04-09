// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DigestAuthenticator.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !PORTABLE

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
        public string UserName { get { return _username; } }

        /// <summary>
        /// Password.
        /// </summary>
        [NotNull]
        public string Password { get { return _password; } }

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
            Code.NotNull(username, "username");
            Code.NotNull(password, "password");

            _username = username;
            _password = password;
        }

        #endregion

        #region Private members

        private readonly string _username;
        private readonly string _password;

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
