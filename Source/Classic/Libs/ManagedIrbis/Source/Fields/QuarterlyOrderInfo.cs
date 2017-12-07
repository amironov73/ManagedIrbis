// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* QuarterlyOrderInfo.cs -- 
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
using AM.Collections;
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
    /// Сведения о заказах (поквартальные), поле 938.
    /// </summary>
    [PublicAPI]
    [XmlRoot("order")]
    [MoonSharpUserData]
    public sealed class QuarterlyOrderInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 938;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abdenqvxy";

        #endregion

        #region Properties

        /// <summary>
        /// Период заказа. Подполе q.
        /// </summary>
        [CanBeNull]
        [SubField('q')]
        [XmlAttribute("period")]
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public string Period { get; set; }

        /// <summary>
        /// Число номеров. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlAttribute("issues")]
        [JsonProperty("issues", NullValueHandling = NullValueHandling.Ignore)]
        public string NumberOfIssues { get; set; }

        /// <summary>
        /// Первый номер. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("first")]
        [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstIssue { get; set; }

        /// <summary>
        /// Последний номер. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("last")]
        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public string LastIssue { get; set; }

        /// <summary>
        /// Цена заказа. Подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlAttribute("totalPrice")]
        [JsonProperty("totalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string TotalPrice { get; set; }

        /// <summary>
        /// Цена номера по комплектам. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("issuePrice")]
        [JsonProperty("issuePrice", NullValueHandling = NullValueHandling.Ignore)]
        public string IssuePrice { get; set; }

        /// <summary>
        /// Валюта. Подполе v.
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("currency")]
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        /// <summary>
        /// Периодичность (код). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("code")]
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string PeriodicityCode { get; set; }

        /// <summary>
        /// Периодичность (код). Подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("periodicity")]
        [JsonProperty("periodicity", NullValueHandling = NullValueHandling.Ignore)]
        public string PeriodicityNumber { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [CanBeNull]
        [Browsable(false)]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
        public SubField[] UnknownSubfields { get; set; }

        /// <summary>
        /// Связанное поле.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение к уже имеющемуся полю.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('q', Period)
                .ApplySubField('n', NumberOfIssues)
                .ApplySubField('a', FirstIssue)
                .ApplySubField('b', LastIssue)
                .ApplySubField('y', TotalPrice)
                .ApplySubField('e', IssuePrice)
                .ApplySubField('v', Currency)
                .ApplySubField('d', PeriodicityCode)
                .ApplySubField('x', PeriodicityNumber);
        }

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static QuarterlyOrderInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            QuarterlyOrderInfo result = new QuarterlyOrderInfo
            {
                Period = field.GetFirstSubFieldValue('q'),
                NumberOfIssues = field.GetFirstSubFieldValue('n'),
                FirstIssue = field.GetFirstSubFieldValue('a'),
                LastIssue = field.GetFirstSubFieldValue('b'),
                TotalPrice = field.GetFirstSubFieldValue('y'),
                IssuePrice = field.GetFirstSubFieldValue('e'),
                Currency = field.GetFirstSubFieldValue('v'),
                PeriodicityCode = field.GetFirstSubFieldValue('d'),
                PeriodicityNumber = field.GetFirstSubFieldValue('x'),
                UnknownSubfields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static QuarterlyOrderInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<QuarterlyOrderInfo> result = new List<QuarterlyOrderInfo>();
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
        /// Should serialize <see cref="UnknownSubfields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeUnknownSubfields()
        {
            return !UnknownSubfields.IsNullOrEmpty();
        }

        /// <summary>
        /// Преобразование обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('q', Period)
                .AddNonEmptySubField('n', NumberOfIssues)
                .AddNonEmptySubField('a', FirstIssue)
                .AddNonEmptySubField('b', LastIssue)
                .AddNonEmptySubField('y', TotalPrice)
                .AddNonEmptySubField('e', IssuePrice)
                .AddNonEmptySubField('v', Currency)
                .AddNonEmptySubField('d', PeriodicityCode)
                .AddNonEmptySubField('x', PeriodicityNumber)
                .AddSubFields(UnknownSubfields);

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

            Period = reader.ReadNullableString();
            NumberOfIssues = reader.ReadNullableString();
            FirstIssue = reader.ReadNullableString();
            LastIssue = reader.ReadNullableString();
            TotalPrice = reader.ReadNullableString();
            IssuePrice = reader.ReadNullableString();
            Currency = reader.ReadNullableString();
            PeriodicityCode = reader.ReadNullableString();
            PeriodicityNumber = reader.ReadNullableString();
            UnknownSubfields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Period)
                .WriteNullable(NumberOfIssues)
                .WriteNullable(FirstIssue)
                .WriteNullable(LastIssue)
                .WriteNullable(TotalPrice)
                .WriteNullable(IssuePrice)
                .WriteNullable(Currency)
                .WriteNullable(PeriodicityCode)
                .WriteNullable(PeriodicityNumber)
                .WriteNullableArray(UnknownSubfields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<QuarterlyOrderInfo> verifier
                = new Verifier<QuarterlyOrderInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Period, "Period")
                .NotNullNorEmpty(FirstIssue, "FirstIssue")
                .NotNullNorEmpty(LastIssue, "LastIssue");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Period.ToVisibleString();
        }

        #endregion
    }
}
