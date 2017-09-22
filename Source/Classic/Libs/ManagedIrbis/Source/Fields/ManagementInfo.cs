// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ManagementInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о руководстве библиотеки, поле 14 в БД CMPL.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("management")]
    public sealed class ManagementInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 14;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefghij";

        #endregion

        #region Properties

        /// <summary>
        /// Директор библиотеки (текст - полное название). Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("fullTitle")]
        [JsonProperty("fullTitle")]
        public string FullTitle { get; set; }

        /// <summary>
        /// Директор библиотеки (текст - аббревиатура). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("shortTitle")]
        [JsonProperty("shortTitle")]
        public string ShortTitle { get; set; }

        /// <summary>
        /// ФИО директора. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("director")]
        [JsonProperty("director")]
        public string DirectorName { get; set; }

        /// <summary>
        /// ФИО главного бухгалтера. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("accountant")]
        [JsonProperty("accountant")]
        public string Accountant { get; set; }

        /// <summary>
        /// Контактное лицо - ФИО. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("contactPerson")]
        [JsonProperty("contactPerson")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// Контактное лицо - телефон. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlElement("contactPhone")]
        [JsonProperty("contactPhone")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Руководитель структурного подразделения. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("departmentHead")]
        [JsonProperty("departmentHead")]
        public string DepartmentHead { get; set; }

        /// <summary>
        /// Текст для Реестра (1-ая строка). Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlElement("registry1")]
        [JsonProperty("registry1")]
        public string Registry1 { get; set; }

        /// <summary>
        /// Текст для Реестра (2-я строка). Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlElement("registry2")]
        [JsonProperty("registry2")]
        public string Registry2 { get; set; }

        /// <summary>
        /// Текст для Реестра (3-я строка). Подполе j.
        /// </summary>
        [CanBeNull]
        [SubField('j')]
        [XmlElement("registry3")]
        [JsonProperty("registry3")]
        public string Registry3 { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown")]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', FullTitle)
                .ApplySubField('d', ShortTitle)
                .ApplySubField('b', DirectorName)
                .ApplySubField('c', Accountant)
                .ApplySubField('f', ContactPerson)
                .ApplySubField('e', ContactPhone)
                .ApplySubField('g', DepartmentHead)
                .ApplySubField('h', Registry1)
                .ApplySubField('i', Registry2)
                .ApplySubField('j', Registry3);
        }

        /// <summary>
        /// Parse the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static ManagementInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ManagementInfo result = new ManagementInfo
            {
                FullTitle = field.GetFirstSubFieldValue('a'),
                ShortTitle = field.GetFirstSubFieldValue('d'),
                DirectorName = field.GetFirstSubFieldValue('b'),
                Accountant = field.GetFirstSubFieldValue('c'),
                ContactPerson = field.GetFirstSubFieldValue('f'),
                ContactPhone = field.GetFirstSubFieldValue('e'),
                DepartmentHead = field.GetFirstSubFieldValue('g'),
                Registry1 = field.GetFirstSubFieldValue('h'),
                Registry2 = field.GetFirstSubFieldValue('i'),
                Registry3 = field.GetFirstSubFieldValue('j'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ManagementInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<ManagementInfo> result = new List<ManagementInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    ManagementInfo info = ParseField(field);
                    result.Add(info);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', FullTitle)
                .AddNonEmptySubField('d', ShortTitle)
                .AddNonEmptySubField('b', DirectorName)
                .AddNonEmptySubField('c', Accountant)
                .AddNonEmptySubField('f', ContactPerson)
                .AddNonEmptySubField('e', ContactPhone)
                .AddNonEmptySubField('g', DepartmentHead)
                .AddNonEmptySubField('h', Registry1)
                .AddNonEmptySubField('i', Registry2)
                .AddNonEmptySubField('j', Registry3)
                .AddSubFields(UnknownSubFields);

            return result;
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

            FullTitle = reader.ReadNullableString();
            ShortTitle = reader.ReadNullableString();
            DirectorName = reader.ReadNullableString();
            Accountant = reader.ReadNullableString();
            ContactPerson = reader.ReadNullableString();
            ContactPhone = reader.ReadNullableString();
            DepartmentHead = reader.ReadNullableString();
            Registry1 = reader.ReadNullableString();
            Registry2 = reader.ReadNullableString();
            Registry3 = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(FullTitle)
                .WriteNullable(ShortTitle)
                .WriteNullable(DirectorName)
                .WriteNullable(Accountant)
                .WriteNullable(ContactPerson)
                .WriteNullable(ContactPhone)
                .WriteNullable(DepartmentHead)
                .WriteNullable(Registry1)
                .WriteNullable(Registry2)
                .WriteNullable(Registry3)
                .WriteNullableArray(UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ManagementInfo> verifier
                = new Verifier<ManagementInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(FullTitle, "FullTitle")
                .NotNullNorEmpty(DirectorName, "DirectorName");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Title: {0}{2}Name: {1}",
                    FullTitle.ToVisibleString(),
                    DirectorName.ToVisibleString(),
                    Environment.NewLine
                );
        }

        #endregion
    }
}
