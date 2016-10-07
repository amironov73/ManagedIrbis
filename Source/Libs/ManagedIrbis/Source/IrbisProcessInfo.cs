/* IrbisProcessInfo.cs -- информация о процессе на сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Number}] {Name} ({Workstation})")]
#endif
    public sealed class IrbisProcessInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Просто порядковый номер процесса.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// С каким клиентом взаимодействует.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("ip-address")]
        [JsonProperty("ip-address")]
        public string IPAddress { get; set; }

        /// <summary>
        /// Логин оператора.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("client-id")]
        [JsonProperty("client-id")]
        public string ClientID { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("workstation")]
        [JsonProperty("workstation")]
        public string Workstation { get; set; }

        /// <summary>
        /// Время запуска.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("started")]
        [JsonProperty("started")]
        public string Started { get; set; }

        /// <summary>
        /// Последняя выполненная (или выполняемая) команда.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("last-command")]
        [JsonProperty("last-command")]
        public string LastCommand { get; set; }

        /// <summary>
        /// Порядковый номер последней команды.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("command-number")]
        [JsonProperty("command-number")]
        public string CommandNumber { get; set; }

        /// <summary>
        /// Идентификатор процесса.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("process-id")]
        [JsonProperty("process-id")]
        public string ProcessID { get; set; }

        /// <summary>
        /// Состояние.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("state")]
        [JsonProperty("state")]
        public string State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static IrbisProcessInfo[] Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<IrbisProcessInfo> result = new List<IrbisProcessInfo>();
            while (true)
            {
                string[] lines = response.GetAnsiStrings(10);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }
                IrbisProcessInfo info = new IrbisProcessInfo
                {
                    ProcessID = lines[0],
                    State = lines[1],
                    Number = lines[2],
                    IPAddress = lines[3],
                    Name = lines[4],
                    ClientID = lines[5],
                    Workstation = lines[6],
                    Started  = lines[7],
                    LastCommand = lines[8],
                    CommandNumber = lines[9]
                };
                result.Add(info);

            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
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

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
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
                    "Number: {0}, IPAddress: {1}, "
                  + "Name: {2}, ID: {3}, Workstation: {4}, "
                  + "Started: {5}, LastCommand: {6}, "
                  + "CommandNumber: {7}, ProcessID: {8}, "
                  + "State: {9}",
                    Number,
                    IPAddress,
                    Name,
                    ClientID,
                    Workstation,
                    Started,
                    LastCommand,
                    CommandNumber,
                    ProcessID,
                    State
                );
        }

        #endregion
    }
}
