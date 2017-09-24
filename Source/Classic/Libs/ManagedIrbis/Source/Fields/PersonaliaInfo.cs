// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PersonaliaInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("personalia")]
    public sealed class PersonaliaInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 600;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdfgrp19)";

        #endregion

        #region Properties

        /// <summary>
        /// Вид данных. Подполе ).
        /// </summary>
        [CanBeNull]
        [SubField(')')]
        [XmlElement("dataKind")]
        [JsonProperty("dataKind")]
        [Description("Вид данных")]
        [DisplayName("Вид данных")]
        public string DataKind { get; set; }

        /// <summary>
        /// Текст. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("text")]
        [JsonProperty("text")]
        [Description("Текст")]
        [DisplayName("Текст")]
        public string Text { get; set; }

        /// <summary>
        /// Фамилия, инициалы. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("name")]
        [JsonProperty("name")]
        [Description("Фамилия, инициалы")]
        [DisplayName("Фамилия, инициалы")]
        public string Name { get; set; }

        /// <summary>
        /// Расширение инициалов. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("extension")]
        [JsonProperty("extension")]
        [Description("Расширение инициалов")]
        [DisplayName("Расширение инициалов")]
        public string Extension { get; set; }

        /// <summary>
        /// Инвертирование имени недопустимо? Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlElement("cantBeInverted")]
        [JsonProperty("cantBeInverted")]
        [Description("Инвертирование имени недопустимо")]
        [DisplayName("Инвертирование имени недопустимо")]
        public bool CantBeInverted { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (отец, сын, младший, старший
        /// и т. п.). Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("postfix")]
        [JsonProperty("postfix")]
        [Description("Неотъемлемая часть имени. Подполе 1.")]
        [DisplayName("Неотъемлемая часть имени")]
        public string Postfix { get; set; }

        /// <summary>
        /// Дополнения к имени кроме дат (род деятельности, звание,
        /// титул и т. д.). Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("appendix")]
        [JsonProperty("appendix")]
        [Description("Дополнения к имени кроме дат. Подполе c.")]
        [DisplayName("Дополнения к имени кроме дат")]
        public string Appendix { get; set; }

        /// <summary>
        /// Династический номер (римские цифры). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Династический номер (римские цифры). Подполе d.")]
        [DisplayName("Династический номер (римские цифры)")]
        public string Number { get; set; }

        /// <summary>
        /// Даты жизни. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dates")]
        [JsonProperty("dates")]
        [Description("Даты жизни. Подполе f.")]
        [DisplayName("Даты жизни")]
        public string Dates { get; set; }

        /// <summary>
        /// Разночтение фамилии. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlAttribute("variant")]
        [JsonProperty("variant")]
        [Description("Разночтение фамилии. Подполе r.")]
        [DisplayName("Разночтение фамилии")]
        public string Variant { get; set; }

        /// <summary>
        /// Место работы автора. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("workplace")]
        [JsonProperty("workplace")]
        [Description("Место работы автора. Подполе p.")]
        [DisplayName("Место работы автора")]
        public string WorkPlace { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
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
        /// Apply the <see cref="PersonaliaInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField(')', DataKind)
                .ApplySubField('b', Text)
                .ApplySubField('a', Name)
                .ApplySubField('g', Extension)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static PersonaliaInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<PersonaliaInfo> result = new List<PersonaliaInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    PersonaliaInfo personalia = ParseField(field);
                    result.Add(personalia);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static PersonaliaInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            PersonaliaInfo result = new PersonaliaInfo
            {
                DataKind = field.GetFirstSubFieldValue(')'),
                Text = field.GetFirstSubFieldValue('b'),
                Name = field.GetFirstSubFieldValue('a'),
                Extension = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="CantBeInverted"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCantBeInverted()
        {
            return CantBeInverted;
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
        /// Convert <see cref="PersonaliaInfo"/>
        /// to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField(')', DataKind)
                .AddNonEmptySubField('b', Text)
                .AddNonEmptySubField('a', Name)
                .AddNonEmptySubField('g', Extension)
                .AddNonEmptySubField('9', CantBeInverted ? "1" : null)
                .AddNonEmptySubField('1', Postfix)
                .AddNonEmptySubField('c', Appendix)
                .AddNonEmptySubField('d', Number)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('r', Variant)
                .AddNonEmptySubField('p', WorkPlace)
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

            DataKind = reader.ReadNullableString();
            Text = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Extension = reader.ReadNullableString();
            CantBeInverted = reader.ReadBoolean();
            Postfix = reader.ReadNullableString();
            Appendix = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Dates = reader.ReadNullableString();
            Variant = reader.ReadNullableString();
            WorkPlace = reader.ReadNullableString();
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
                .WriteNullable(DataKind)
                .WriteNullable(Text)
                .WriteNullable(Name)
                .WriteNullable(Extension)
                .Write(CantBeInverted);
            writer
                .WriteNullable(Postfix)
                .WriteNullable(Appendix)
                .WriteNullable(Number)
                .WriteNullable(Dates)
                .WriteNullable(Variant)
                .WriteNullable(WorkPlace)
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
            Verifier<PersonaliaInfo> verifier
                = new Verifier<PersonaliaInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
