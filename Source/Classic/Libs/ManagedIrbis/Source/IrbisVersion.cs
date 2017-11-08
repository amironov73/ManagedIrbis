// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisVersion.cs -- информация о версии ИРБИС-сервера.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о версии ИРБИС-сервера.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("version")]
    [DebuggerDisplay("Version={Version}")]
    public sealed class IrbisVersion
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// На кого приобретен.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("organization")]
        [JsonProperty("organization", NullValueHandling = NullValueHandling.Ignore)]
        public string Organization { get; set; }

        /// <summary>
        /// Собственно версия.
        /// </summary>
        /// <remarks>Например, 64.2008.1</remarks>
        [CanBeNull]
        [XmlAttribute("version")]
        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        /// <summary>
        /// Максимальное количество подключений.
        /// </summary>
        [XmlAttribute("max-clients")]
        [JsonProperty("max-clients", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxClients { get; set; }

        /// <summary>
        /// Текущее количество подключений.
        /// </summary>
        [XmlAttribute("connected-clients")]
        [JsonProperty("connected-clients", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ConnectedClients { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static IrbisVersion ParseServerResponse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<string> lines = response.RemainingAnsiStrings();
            IrbisVersion result = ParseServerResponse(lines);

            return result;
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static IrbisVersion ParseServerResponse
            (
                [NotNull] List<string> lines
            )
        {
            Code.NotNull(lines, "lines");

            IrbisVersion result = lines.Count == 4
                ? new IrbisVersion
                   {
                       Organization = lines.GetItem(0),
                       Version = lines.GetItem(1),
                       ConnectedClients = lines.GetItem(2).SafeToInt32(),
                       MaxClients = lines.GetItem(3).SafeToInt32()
                   }
               : new IrbisVersion
                   {
                       Version = lines.GetItem(0),
                       ConnectedClients = lines.GetItem(1).SafeToInt32(),
                       MaxClients = lines.GetItem(2).SafeToInt32()
                   };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="ConnectedClients"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeConnectedClients()
        {
            return ConnectedClients != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MaxClients"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMaxClients()
        {
            return MaxClients != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Organization = reader.ReadNullableString();
            Version = reader.ReadNullableString();
            MaxClients = reader.ReadPackedInt32();
            ConnectedClients = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Organization)
                .WriteNullable(Version)
                .WritePackedInt32(MaxClients)
                .WritePackedInt32(ConnectedClients);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Version: {0}, MaxClients: {1}, "
                    + "ConnectedClients: {2}, Organization: {3}",
                    Version.ToVisibleString(),
                    MaxClients,
                    ConnectedClients,
                    Organization.ToVisibleString()
                );
        }

        #endregion
    }
}
