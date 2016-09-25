/* Credentials.cs -- 
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
    [XmlRoot("credentials")]
    public sealed class Credentials
    {
        #region Properties

        /// <summary>
        /// Hostname.
        /// </summary>
        [CanBeNull]
        [JsonProperty("hostname")]
        [XmlAttribute("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        [CanBeNull]
        [JsonProperty("username")]
        [XmlAttribute("username")]
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [CanBeNull]
        [JsonProperty("password")]
        [XmlAttribute("password")]
        public string Password { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        [CanBeNull]
        [JsonProperty("role")]
        [XmlAttribute("role")]
        public string Role { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
