/* UserInfo.cs -- информация о зарегистрированном пользователе
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

using ManagedIrbis.Network;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о зарегистрированном пользователе системы
    /// (по данным client_m.mnu).
    /// </summary>
    [PublicAPI]
    [XmlRoot("user")]
    [MoonSharpUserData]
    [DebuggerDisplay("{Name}")]
    public sealed class UserInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Номер по порядку.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [XmlAttribute("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        [XmlAttribute("cataloguer")]
        [JsonProperty("cataloguer")]
        public string Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        [XmlAttribute("reader")]
        [JsonProperty("reader")]
        public string Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        [XmlAttribute("circulation")]
        [JsonProperty("circulation")]
        public string Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        [XmlAttribute("acquisitions")]
        [JsonProperty("acquisitions")]
        public string Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        public string Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        [XmlAttribute("administrator")]
        [JsonProperty("administrator")]
        public string Administrator { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private string _FormatPair
            (
                string prefix,
                string value,
                string defaultValue
            )
        {
            if (value.SameString(defaultValue))
            {
                return string.Empty;
            }
            return string.Format
                (
                    "{0}={1};",
                    prefix,
                    value
                );

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        public static UserInfo[] Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<UserInfo> result = new List<UserInfo>();

            response.GetAnsiStrings(2);

            while (true)
            {
                string[] lines = response.GetAnsiStrings(9);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }

                UserInfo user = new UserInfo
                {
                    Number = lines[0],
                    Name = lines[1],
                    Password = lines[2],
                    Cataloger = lines[3],
                    Reader = lines[4],
                    Circulation = lines[5],
                    Acquisitions = lines[6],
                    Provision = lines[7],
                    Administrator = lines[8]
                };
                result.Add(user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Encode.
        /// </summary>
        [NotNull]
        public string Encode()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    _FormatPair("C", Cataloger, "irbisc.ini"),
                    _FormatPair("R", Reader, "irbisr.ini"),
                    _FormatPair("B", Circulation, "irbisb.ini"),
                    _FormatPair("M", Acquisitions, "irbisp.ini"),
                    _FormatPair("K", Provision, "irbisk.ini"),
                    _FormatPair("A", Administrator, "irbisa.ini")
                );
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Number = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Cataloger = reader.ReadNullableString();
            Circulation = reader.ReadNullableString();
            Acquisitions = reader.ReadNullableString();
            Provision = reader.ReadNullableString();
            Administrator = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Number)
                .WriteNullable(Name)
                .WriteNullable(Password)
                .WriteNullable(Cataloger)
                .WriteNullable(Reader)
                .WriteNullable(Circulation)
                .WriteNullable(Acquisitions)
                .WriteNullable(Provision)
                .WriteNullable(Administrator);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<UserInfo> verifier = new Verifier<UserInfo>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(Name, "Name");

            return verifier.Result;
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
                    "Number: {0}, Name: {1}, Password: {2}, "
                    + "Cataloger: {3}, Reader: {4}, Circulation: {5}, "
                    + "Acquisitions: {6}, Provision: {7}, " 
                    + "Administrator: {8}",
                    Number,
                    Name,
                    Password,
                    Cataloger,
                    Reader,
                    Circulation,
                    Acquisitions,
                    Provision,
                    Administrator
                );
        }

        #endregion
    }
}
