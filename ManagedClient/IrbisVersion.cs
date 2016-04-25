/* IrbisVersion.cs -- информация о версии ИРБИС-сервера.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о версии ИРБИС-сервера.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Version={Version}")]
    public sealed class IrbisVersion
    {
        #region Properties

        /// <summary>
        /// На кого приобретен.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("organization")]
        [JsonProperty("organization")]
        public string Organization { get; set; }

        /// <summary>
        /// Собственно версия.
        /// </summary>
        /// <remarks>Например, 64.2008.1</remarks>
        [CanBeNull]
        [XmlAttribute("version")]
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Максимальное количество подключений.
        /// </summary>
        [XmlAttribute("max-clients")]
        [JsonProperty("max-clients")]
        public int MaxClients { get; set; }

        /// <summary>
        /// Текущее количество подключений.
        /// </summary>
        [XmlAttribute("connected-clients")]
        [JsonProperty("connected-clients")]
        public int ConnectedClients { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        public static IrbisVersion ParseServerAnswer
            (
                [NotNull] List<string> lines
            )
        {
            Code.NotNull(lines, "lines");

            IrbisVersion result = new IrbisVersion
               {
                   Organization = lines.GetItem(1),
                   Version = lines.GetItem(2),
                   ConnectedClients = lines.GetItem(3).SafeToInt32(),
                   MaxClients = lines.GetItem(4).SafeToInt32()
               };

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Version: {0}, MaxClients: {1}, "
                    + "ConnectedClients: {2}, Organization: {3}",
                    Version,
                    MaxClients,
                    ConnectedClients,
                    Organization
                );
        }

        #endregion
    }
}
