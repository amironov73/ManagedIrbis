// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ArrivalsInfo.cs -- 
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
    /// Число наименований, поступивших впервые (на баланс,
    /// не на баланс, учебников), поле 17 в БД CMPL.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("arrivals")]
    public sealed class ArrivalsInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 17;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "ab123";

        #endregion

        #region Properties

        /// <summary>
        /// Поступило впервые на баланс (без периодики). Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("onBalanceWithoutPeriodicals")]
        [JsonProperty("onBalanceWithoutPeriodicals", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Поступило впервые на баланс (без периодики)")]
        [DisplayName("Поступило впервые на баланс (без периодики)")]
        public string OnBalanceWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые не на баланс (без периодики). Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("offBalanceWithoutPeriodicals")]
        [JsonProperty("offBalanceWithoutPeriodicals", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Поступило впервые не на баланс (без периодики)")]
        [DisplayName("Поступило впервые не на баланс (без периодики)")]
        public string OffBalanceWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые всего (без периодики). Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("totalWithoutPeriodicals")]
        [JsonProperty("totalWithoutPeriodicals", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Поступило впервые всего (без периодики)")]
        [DisplayName("Поступило впервые всего (без периодики)")]
        public string TotalWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые не на баланс (с периодикой). Подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("offBalanceWithPeriodicals")]
        [JsonProperty("offBalanceWithPeriodicals", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Поступило впервые не на баланс (с периодикой)")]
        [DisplayName("Поступило впервые не на баланс (с периодикой)")]
        public string OffBalanceWithPeriodicals { get; set; }

        /// <summary>
        /// Учебные издания. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("educational")]
        [JsonProperty("educational", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Учебные издания")]
        [DisplayName("Учебные издания")]
        public string Educational { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
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
        /// Apply the <see cref="ArrivalsInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('1', OnBalanceWithoutPeriodicals)
                .ApplySubField('b', OffBalanceWithoutPeriodicals)
                .ApplySubField('a', TotalWithoutPeriodicals)
                .ApplySubField('2', OffBalanceWithPeriodicals)
                .ApplySubField('3', Educational);
        }

        /// <summary>
        /// Parse the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static ArrivalsInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ArrivalsInfo result = new ArrivalsInfo
            {
                OnBalanceWithoutPeriodicals = field.GetFirstSubFieldValue('1'),
                OffBalanceWithoutPeriodicals = field.GetFirstSubFieldValue('b'),
                TotalWithoutPeriodicals = field.GetFirstSubFieldValue('a'),
                OffBalanceWithPeriodicals = field.GetFirstSubFieldValue('2'),
                Educational = field.GetFirstSubFieldValue('3'),
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
        public static ArrivalsInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<ArrivalsInfo> result = new List<ArrivalsInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    result.Add(ParseField(field));
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
        /// Convert the <see cref="ArrivalsInfo"/>
        /// back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('1', OnBalanceWithoutPeriodicals)
                .AddNonEmptySubField('b', OffBalanceWithoutPeriodicals)
                .AddNonEmptySubField('a', TotalWithoutPeriodicals)
                .AddNonEmptySubField('2', OffBalanceWithPeriodicals)
                .AddNonEmptySubField('3', Educational)
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

            OnBalanceWithoutPeriodicals = reader.ReadNullableString();
            OffBalanceWithoutPeriodicals = reader.ReadNullableString();
            TotalWithoutPeriodicals = reader.ReadNullableString();
            OffBalanceWithPeriodicals = reader.ReadNullableString();
            Educational = reader.ReadNullableString();
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
                .WriteNullable(OnBalanceWithoutPeriodicals)
                .WriteNullable(OffBalanceWithoutPeriodicals)
                .WriteNullable(TotalWithoutPeriodicals)
                .WriteNullable(OffBalanceWithPeriodicals)
                .WriteNullable(Educational)
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
            Verifier<ArrivalsInfo> verifier
                = new Verifier<ArrivalsInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(OnBalanceWithoutPeriodicals)
                    || !string.IsNullOrEmpty(OffBalanceWithoutPeriodicals)
                    || !string.IsNullOrEmpty(TotalWithoutPeriodicals)
                    || !string.IsNullOrEmpty(OffBalanceWithPeriodicals)
                    || !string.IsNullOrEmpty(Educational)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "OnBalanceWithoutPeriodicals: {0}, "
                    + "OffBalanceWithoutPeriodicals: {1}, "
                    + "TotalWithoutPeriodicals: {2}, "
                    + "OffBalanceWithPeriodicals: {3}, "
                    + "Educational: {4}",
                    OnBalanceWithoutPeriodicals.ToVisibleString(),
                    OffBalanceWithoutPeriodicals.ToVisibleString(),
                    TotalWithoutPeriodicals.ToVisibleString(),
                    OffBalanceWithPeriodicals.ToVisibleString(),
                    Educational.ToVisibleString()
                );
        }

        #endregion
    }
}
