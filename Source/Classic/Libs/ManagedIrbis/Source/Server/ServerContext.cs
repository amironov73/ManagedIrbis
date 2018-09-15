// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
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

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("context")]
    public class ServerContext
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Address.
        /// </summary>
        [CanBeNull]
        [XmlElement("address")]
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Command count.
        /// </summary>
        [XmlElement("commandCount")]
        [JsonProperty("commandCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CommandCount { get; set; }

        /// <summary>
        /// Connection established time.
        /// </summary>
        [XmlElement("connected")]
        [JsonProperty("connected", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Connected { get; set; }

        /// <summary>
        /// Context identifier.
        /// </summary>
        [XmlElement("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Last activity time.
        /// </summary>
        [XmlElement("lastActivity")]
        [JsonProperty("lastActivity", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Last command code.
        /// </summary>
        [XmlElement("lastCommand")]
        [JsonProperty("lastCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string LastCommand { get;set; }

        /// <summary>
        /// Password.
        /// </summary>
        [CanBeNull]
        [XmlElement("password")]
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [CanBeNull]
        [XmlElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Workstation.
        /// </summary>
        [XmlElement("workstation")]
        [JsonProperty("workstation")]
        public string Workstation { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="CommandCount"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeCommandCount()
        {
            return CommandCount != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Connected"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeConnected()
        {
            return Connected != DateTime.MinValue;
        }

        /// <summary>
        /// Should serialize the <see cref="LastActivity"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeLastActivity()
        {
            return LastActivity != DateTime.MinValue;
        }

        /// <summary>
        /// Should serialize the <see cref="Workstation"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeWorkstation()
        {
            return !string.IsNullOrEmpty(Workstation);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Address = reader.ReadNullableString();
            CommandCount = reader.ReadPackedInt32();
            Connected = reader.ReadDateTime();
            Id = reader.ReadNullableString();
            LastActivity = reader.ReadDateTime();
            LastCommand = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Username = reader.ReadNullableString();
            Workstation = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Address)
                .WritePackedInt32(CommandCount)
                .Write(Connected)
                .WriteNullable(Id)
                .Write(LastActivity)
                .WriteNullable(LastCommand)
                .WriteNullable(Password)
                .WriteNullable(Username)
                .WriteNullable(Workstation);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ServerContext> verifier
                = new Verifier<ServerContext>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Id, "Id");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Id.ToVisibleString();
        }

        #endregion
    }
}
