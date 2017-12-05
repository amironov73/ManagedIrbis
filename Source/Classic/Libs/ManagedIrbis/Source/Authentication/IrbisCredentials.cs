// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Credentials.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

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
    public sealed class IrbisCredentials
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Hostname.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("hostname")]
        [JsonProperty("hostname", NullValueHandling = NullValueHandling.Ignore)]
        public string Hostname { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("username")]
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("password")]
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// Resource.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("resource")]
        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public string Resource { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("role")]
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Hostname = reader.ReadNullableString();
            Username = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Resource = reader.ReadNullableString();
            Role = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Hostname)
                .WriteNullable(Username)
                .WriteNullable(Password)
                .WriteNullable(Resource)
                .WriteNullable(Role);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<IrbisCredentials> verifier
                = new Verifier<IrbisCredentials>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Username, "Username");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Username.ToVisibleString();
        }

        #endregion
    }
}
