/* IrbisProcessInfo.cs -- информация о процессе на сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о запущенном на сервере процессе.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Number}] {Name} ({Workstation})")]
    public sealed class IrbisProcessInfo
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
        public static IrbisProcessInfo[] ParseServerResponse
            (
                [NotNull] string[] text
            )
        {
            Code.NotNull(text, "text");

            List<IrbisProcessInfo> result = new List<IrbisProcessInfo>();

            for (int index = 3; index < (text.Length - 10); index += 10)
            {
                IrbisProcessInfo info = new IrbisProcessInfo
                    {
                        Number = text[index],
                        IPAddress = text[index + 1],
                        Name = text[index + 2],
                        ClientID = text[index + 3],
                        Workstation = text[index + 4],
                        Started = text[index + 5],
                        LastCommand = text[index + 6],
                        CommandNumber = text[index + 7],
                        ProcessID = text[index + 8],
                        State = text[index + 9]
                    };
                result.Add(info);
            }

            return result.ToArray();
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
