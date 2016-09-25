/* AbstractAuthenticator.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Authentication
{
    /// <summary>
    /// Credentials.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractAuthenticator
    {
        #region Public methods

        /// <summary>
        /// Authenticate given user according provided role.
        /// </summary>
        [NotNull]
        public abstract AuthenticationResult Authenticate
            (
                [NotNull] Credentials credentials
            );

        /// <summary>
        /// Get or create context for authenticated user.
        /// </summary>
        [NotNull]
        public abstract UserContext GetContext
            (
                [NotNull] AuthenticationResult authentication
            );

        #endregion
    }
}
