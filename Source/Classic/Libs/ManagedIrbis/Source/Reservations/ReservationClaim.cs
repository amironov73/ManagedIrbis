// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationClaim.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    [MoonSharpUserData]
    public sealed class ReservationClaim
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abcdz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const string Tag = "20";

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
        public string Date { get; set; }

        /// <summary>
        /// Time.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("time")]
        [JsonProperty("time")]
        [Description("Время")]
        [DisplayName("Время")]
        public string Time { get; set; }

        /// <summary>
        /// Duration.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("duration")]
        [JsonProperty("duration")]
        [Description("Продолжительность")]
        [DisplayName("Продолжительность")]
        public string Duration { get; set; }

        /// <summary>
        /// Whether the claim is active?
        /// </summary>
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
        [Description("Поле с подполями")]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; set; }

        /// <summary>
        /// Associated reader.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Description("Читатель")]
        [DisplayName("Читатель")]
        public ReaderInfo Reader { get; set; }

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
                .ApplySubField('b', Date)
                .ApplySubField('c', Time)
                .ApplySubField('d', Duration)
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
                Date = field.GetFirstSubFieldValue('b'),
                Time = field.GetFirstSubFieldValue('c'),
                Duration = field.GetFirstSubFieldValue('d'),
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
                if (field.Tag.SameString(Tag))
                {
                    ReservationClaim claim = ParseField(field);
                    result.Add(claim);
                }
            }

            return result;
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
                .AddNonEmptySubField('b', Date)
                .AddNonEmptySubField('c', Time)
                .AddNonEmptySubField('d', Duration)
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
            Date = reader.ReadNullableString();
            Time = reader.ReadNullableString();
            Duration = reader.ReadNullableString();
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
                .WriteNullable(Date)
                .WriteNullable(Time)
                .WriteNullable(Duration)
                .WriteNullable(Status);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Ticket.ToVisibleString();
        }

        #endregion
    }
}
