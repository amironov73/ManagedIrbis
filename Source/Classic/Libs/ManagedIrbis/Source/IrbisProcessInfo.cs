// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisProcessInfo.cs -- информация о процессе на сервере
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
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
    /// Информация о запущенном на сервере процессе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("process")]
    [DebuggerDisplay("[{Number}] {Name} ({Workstation})")]
    public sealed class IrbisProcessInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Просто порядковый номер процесса.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("number")]
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// С каким клиентом взаимодействует.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("ip-address")]
        [JsonProperty("ip-address", NullValueHandling = NullValueHandling.Ignore)]
        public string IPAddress { get; set; }

        /// <summary>
        /// Логин оператора.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("client-id")]
        [JsonProperty("client-id", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("workstation")]
        [JsonProperty("workstation", NullValueHandling = NullValueHandling.Ignore)]
        public string Workstation { get; set; }

        /// <summary>
        /// Время запуска.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("started")]
        [JsonProperty("started", NullValueHandling = NullValueHandling.Ignore)]
        public string Started { get; set; }

        /// <summary>
        /// Последняя выполненная (или выполняемая) команда.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("last-command")]
        [JsonProperty("last-command", NullValueHandling = NullValueHandling.Ignore)]
        public string LastCommand { get; set; }

        /// <summary>
        /// Порядковый номер последней команды.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("command-number")]
        [JsonProperty("command-number", NullValueHandling = NullValueHandling.Ignore)]
        public string CommandNumber { get; set; }

        /// <summary>
        /// Идентификатор процесса.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("process-id")]
        [JsonProperty("process-id", NullValueHandling = NullValueHandling.Ignore)]
        public string ProcessID { get; set; }

        /// <summary>
        /// Состояние.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("state")]
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        public static IrbisProcessInfo[] Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<IrbisProcessInfo> result = new List<IrbisProcessInfo>();
            int processCount = response.RequireInt32();
            int linesPerProcess = response.RequireInt32();
            for (int i=0; i<processCount; i++)
            {
                string[] lines = response.GetAnsiStrings(linesPerProcess + 1);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }

                IrbisProcessInfo info = new IrbisProcessInfo();
                if (lines.Length != 0)
                {
                    info.Number = lines[0].EmptyToNull();
                }
                if (lines.Length > 1)
                {
                    info.IPAddress = lines[1].EmptyToNull();
                }
                if (lines.Length > 2)
                {
                    info.Name = lines[2].EmptyToNull();
                }
                if (lines.Length > 3)
                {
                    info.ClientID = lines[3].EmptyToNull();
                }
                if (lines.Length > 4)
                {
                    info.Workstation = lines[4].EmptyToNull();
                }
                if (lines.Length > 5)
                {
                    info.Started = lines[5].EmptyToNull();
                }
                if (lines.Length > 6)
                {
                    info.LastCommand = lines[6].EmptyToNull();
                }
                if (lines.Length > 7)
                {
                    info.CommandNumber = lines[7].EmptyToNull();
                }
                if (lines.Length > 8)
                {
                    info.ProcessID = lines[8].EmptyToNull();
                }
                if (lines.Length > 9)
                {
                    info.State = lines[9].EmptyToNull();
                }
                result.Add(info);

            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Number = reader.ReadNullableString();
            IPAddress = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            ClientID = reader.ReadNullableString();
            Workstation = reader.ReadNullableString();
            Started = reader.ReadNullableString();
            LastCommand = reader.ReadNullableString();
            CommandNumber = reader.ReadNullableString();
            ProcessID = reader.ReadNullableString();
            State = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(Number)
                .WriteNullable(IPAddress)
                .WriteNullable(Name)
                .WriteNullable(ClientID)
                .WriteNullable(Workstation)
                .WriteNullable(Started)
                .WriteNullable(LastCommand)
                .WriteNullable(CommandNumber)
                .WriteNullable(ProcessID)
                .WriteNullable(State);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, IPAddress: {1}, "
                  + "Name: {2}, ID: {3}, Workstation: {4}, "
                  + "Started: {5}, LastCommand: {6}, "
                  + "CommandNumber: {7}, ProcessID: {8}, "
                  + "State: {9}",
                    Number.ToVisibleString(),
                    IPAddress.ToVisibleString(),
                    Name.ToVisibleString(),
                    ClientID.ToVisibleString(),
                    Workstation.ToVisibleString(),
                    Started.ToVisibleString(),
                    LastCommand.ToVisibleString(),
                    CommandNumber.ToVisibleString(),
                    ProcessID.ToVisibleString(),
                    State.ToVisibleString()
                );
        }

        #endregion
    }
}
