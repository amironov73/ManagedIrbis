// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UserInfo.cs -- информация о зарегистрированном пользователе
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
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public string Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [CanBeNull]
        [XmlElement("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [CanBeNull]
        [XmlElement("password")]
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        [CanBeNull]
        [XmlElement("cataloguer")]
        [JsonProperty("cataloguer", NullValueHandling = NullValueHandling.Ignore)]
        public string Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        [CanBeNull]
        [XmlElement("reader")]
        [JsonProperty("reader", NullValueHandling = NullValueHandling.Ignore)]
        public string Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        [CanBeNull]
        [XmlElement("circulation")]
        [JsonProperty("circulation", NullValueHandling = NullValueHandling.Ignore)]
        public string Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        [CanBeNull]
        [XmlElement("acquisitions")]
        [JsonProperty("acquisitions", NullValueHandling = NullValueHandling.Ignore)]
        public string Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        [CanBeNull]
        [XmlElement("provision")]
        [JsonProperty("provision", NullValueHandling = NullValueHandling.Ignore)]
        public string Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        [XmlElement("administrator")]
        [JsonProperty("administrator", NullValueHandling = NullValueHandling.Ignore)]
        public string Administrator { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Private members

        [NotNull]
        private string _FormatPair
            (
                [NotNull] string prefix,
                [CanBeNull] string value,
                [NotNull] string defaultValue
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
                string[] lines = response.GetAnsiStringsPlus(9);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }

                UserInfo user = new UserInfo
                {
                    Number = lines[0].EmptyToNull(),
                    Name = lines[1].EmptyToNull(),
                    Password = lines[2].EmptyToNull(),
                    Cataloger = lines[3].EmptyToNull(),
                    Reader = lines[4].EmptyToNull(),
                    Circulation = lines[5].EmptyToNull(),
                    Acquisitions = lines[6].EmptyToNull(),
                    Provision = lines[7].EmptyToNull(),
                    Administrator = lines[8].EmptyToNull()
                };
                result.Add(user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="Cataloger"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCataloger()
        {
            return !string.IsNullOrEmpty(Cataloger);
        }

        /// <summary>
        /// Should serialize the <see cref="Reader"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReader()
        {
            return !string.IsNullOrEmpty(Reader);
        }

        /// <summary>
        /// Should serialize the <see cref="Circulation"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCirculation()
        {
            return !string.IsNullOrEmpty(Circulation);
        }

        /// <summary>
        /// Should serialize the <see cref="Acquisitions"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAcquisitions()
        {
            return !string.IsNullOrEmpty(Acquisitions);
        }

        /// <summary>
        /// Should serialize the <see cref="Provision"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProvision()
        {
            return !string.IsNullOrEmpty(Provision);
        }

        /// <summary>
        /// Should serialize the <see cref="Administrator"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAdministrator()
        {
            return !string.IsNullOrEmpty(Administrator);
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

            Number = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Cataloger = reader.ReadNullableString();
            Circulation = reader.ReadNullableString();
            Acquisitions = reader.ReadNullableString();
            Provision = reader.ReadNullableString();
            Administrator = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
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

        /// <inheritdoc cref="IVerifiable.Verify" />
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

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Name: {1}, Password: {2}, "
                    + "Cataloger: {3}, Reader: {4}, Circulation: {5}, "
                    + "Acquisitions: {6}, Provision: {7}, " 
                    + "Administrator: {8}",
                    Number.ToVisibleString(),
                    Name.ToVisibleString(),
                    Password.ToVisibleString(),
                    Cataloger.ToVisibleString(),
                    Reader.ToVisibleString(),
                    Circulation.ToVisibleString(),
                    Acquisitions.ToVisibleString(),
                    Provision.ToVisibleString(),
                    Administrator.ToVisibleString()
                );
        }

        #endregion
    }
}
