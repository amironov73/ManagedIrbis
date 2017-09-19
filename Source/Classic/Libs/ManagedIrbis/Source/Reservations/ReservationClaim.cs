// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationClaim.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Заявка на резревирование компьютера.
    /// </summary>
    [PublicAPI]
    [XmlRoot("claim")]
    [MoonSharpUserData]
    public sealed class ReservationClaim
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abcdz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Ticket number.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        [Description("Читательский билет")]
        [DisplayName("Читательский билет")]
        public string Ticket { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        [Description("Дата")]
        [DisplayName("Дата")]
        public string DateString { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public DateTime Date
        {
            get { return IrbisDate.ConvertStringToDate(DateString); }
            set { DateString = IrbisDate.ConvertDateToString(value); }
        }

        /// <summary>
        /// Time.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("time")]
        [JsonProperty("time")]
        [Description("Время")]
        [DisplayName("Время")]
        public string TimeString { get; set; }

        /// <summary>
        /// Time.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan Time
        {
            get { return IrbisDate.ConvertStringToTime(TimeString); }
            set { TimeString = IrbisDate.ConvertTimeToString(value); }
        }

        /// <summary>
        /// Begin date and time.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public DateTime BeginDateTime
        {
            get { return Date + Time; }
            set
            {
                DateTime date = value.Date;
                Date = date;
                Time = value - date;
            }
        }

        /// <summary>
        /// Duration.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("duration")]
        [JsonProperty("duration")]
        [Description("Продолжительность")]
        [DisplayName("Продолжительность")]
        public string DurationString { get; set; }

        /// <summary>
        /// Duration.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan Duration
        {
            get { return IrbisDate.ConvertStringToTime(DurationString); }
            set { DurationString = IrbisDate.ConvertTimeToString(value); }
        }

        /// <summary>
        /// End date and time.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public DateTime EndDateTime
        {
            get { return Date + Time + Duration; }
            set
            {
                DateTime date = value.Date;
                Date = date;
                Duration = value - BeginDateTime;
            }
        }

        /// <summary>
        /// Whether the claim is active?
        /// </summary>
        /// <remarks>
        /// Non-empty string means 'not active'.
        /// </remarks>
        [CanBeNull]
        [SubField('z')]
        [XmlAttribute("status")]
        [JsonProperty("status")]
        [Description("Статус")]
        [DisplayName("Статус")]
        public string Status { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Поле с подполями")]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; set; }

        /// <summary>
        /// Associated reader.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Читатель")]
        [DisplayName("Читатель")]
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Ticket)
                .ApplySubField('b', DateString)
                .ApplySubField('c', TimeString)
                .ApplySubField('d', DurationString)
                .ApplySubField('z', Status);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static ReservationClaim ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ReservationClaim result = new ReservationClaim
            {
                Ticket = field.GetFirstSubFieldValue('a'),
                DateString = field.GetFirstSubFieldValue('b'),
                TimeString = field.GetFirstSubFieldValue('c'),
                DurationString = field.GetFirstSubFieldValue('d'),
                Status = field.GetFirstSubFieldValue('z'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static NonNullCollection<ReservationClaim> ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            NonNullCollection<ReservationClaim> result
                = new NonNullCollection<ReservationClaim>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    ReservationClaim claim = ParseField(field);
                    result.Add(claim);
                }
            }

            return result;
        }

        /// <summary>
        /// Should serialize <see cref="Status"/> field?
        /// </summary>
        public bool ShouldSerializeStatus()
        {
            return !string.IsNullOrEmpty(Status);
        }

        /// <summary>
        /// Convert back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField('a', Ticket)
                .AddNonEmptySubField('b', DateString)
                .AddNonEmptySubField('c', TimeString)
                .AddNonEmptySubField('d', DurationString)
                .AddNonEmptySubField('z', Status);

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

            Ticket = reader.ReadNullableString();
            DateString = reader.ReadNullableString();
            TimeString = reader.ReadNullableString();
            DurationString = reader.ReadNullableString();
            Status = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Ticket)
                .WriteNullable(DateString)
                .WriteNullable(TimeString)
                .WriteNullable(DurationString)
                .WriteNullable(Status);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReservationClaim> verifier
                = new Verifier<ReservationClaim>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Ticket, "Ticket")
                .NotNullNorEmpty(DateString, "DateString")
                .NotNullNorEmpty(TimeString, "TimeString")
                .NotNullNorEmpty(DurationString, "DurationString");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1} {2} [{3}]",
                    DateString.ToVisibleString(),
                    TimeString.ToVisibleString(),
                    DurationString.ToVisibleString(),
                    Ticket.ToVisibleString()
                );
        }

        #endregion
    }
}
