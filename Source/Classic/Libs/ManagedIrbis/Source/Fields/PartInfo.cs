// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PartInfo.cs -- 
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
    /// Выпуск, часть. Поле 923.
    /// </summary>
    [PublicAPI]
    [XmlRoot("part")]
    [MoonSharpUserData]
    public sealed class PartInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "hiklu";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 923;

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и № 2-ой единицы деления (Выпуск). Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlElement("secondLevelNumber")]
        [JsonProperty("secondLevelNumber")]
        [Description("Обозначение и № 2-ой единицы деления (Выпуск)")]
        [DisplayName("Обозначение и № 2-ой единицы деления (Выпуск)")]
        public string SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-ой единицы деления (Выпуск). Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlElement("secondLevelTitle")]
        [JsonProperty("secondLevelTitle")]
        [Description("Заглавие 2-ой единицы деления (Выпуск)")]
        [DisplayName("Заглавие 2-ой единицы деления (Выпуск)")]
        public string SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-ей единицы деления (Часть). Подполе k.
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlElement("thirdLevelNumber")]
        [JsonProperty("thirdLevelNumber")]
        [Description("Обозначение и № 3-ей единицы деления (Часть)")]
        [DisplayName("Обозначение и № 3-ей единицы деления (Часть)")]
        public string ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-ей единицы деления (Часть). Подполе l.
        /// </summary>
        [CanBeNull]
        [SubField('l')]
        [XmlElement("thirdLevelTitle")]
        [JsonProperty("thirdLevelTitle")]
        [Description("Заглавие 3-ей единицы деления (Часть)")]
        [DisplayName("Заглавие 3-ей единицы деления (Часть)")]
        public string ThirdLevelTitle { get; set; }

        /// <summary>
        /// Роль (как выводить в словарь?). Подполе u.
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlElement("role")]
        [JsonProperty("role")]
        [Description("Роль (как выводить в словарь?)")]
        [DisplayName("Роль (как выводить в словарь?)")]
        public string Role { get; set; }

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
        [DisplayName("Поле с подполями")]
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
        /// Apply the <see cref="PartInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('h', SecondLevelNumber)
                .ApplySubField('i', SecondLevelTitle)
                .ApplySubField('k', ThirdLevelNumber)
                .ApplySubField('l', ThirdLevelTitle)
                .ApplySubField('u', Role);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static PartInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<PartInfo> result = new List<PartInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    PartInfo part = ParseField(field);
                    result.Add(part);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static PartInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            PartInfo result = new PartInfo
            {
                SecondLevelNumber = field.GetFirstSubFieldValue('h'),
                SecondLevelTitle = field.GetFirstSubFieldValue('i'),
                ThirdLevelNumber = field.GetFirstSubFieldValue('k'),
                ThirdLevelTitle = field.GetFirstSubFieldValue('l'),
                Role = field.GetFirstSubFieldValue('u'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
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
        /// Transform back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('h', SecondLevelNumber)
                .AddNonEmptySubField('i', SecondLevelTitle)
                .AddNonEmptySubField('k', ThirdLevelNumber)
                .AddNonEmptySubField('l', ThirdLevelTitle)
                .AddNonEmptySubField('u', Role)
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

            SecondLevelNumber = reader.ReadNullableString();
            SecondLevelTitle = reader.ReadNullableString();
            ThirdLevelNumber = reader.ReadNullableString();
            ThirdLevelTitle = reader.ReadNullableString();
            Role = reader.ReadNullableString();
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
                .WriteNullable(SecondLevelNumber)
                .WriteNullable(SecondLevelTitle)
                .WriteNullable(ThirdLevelNumber)
                .WriteNullable(ThirdLevelTitle)
                .WriteNullable(Role)
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
            Verifier<PartInfo> verifier
                = new Verifier<PartInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(SecondLevelNumber)
                    || !string.IsNullOrEmpty(SecondLevelTitle)
                    || !string.IsNullOrEmpty(ThirdLevelNumber)
                    || !string.IsNullOrEmpty(ThirdLevelTitle)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return StringUtility.Join
                (
                    " -- ",
                    new[]
                    {
                        SecondLevelNumber, SecondLevelTitle,
                        ThirdLevelNumber, ThirdLevelTitle
                    }
                ).ToVisibleString();
        }

        #endregion
    }
}
