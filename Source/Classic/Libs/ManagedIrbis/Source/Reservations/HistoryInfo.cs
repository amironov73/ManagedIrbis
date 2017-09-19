// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HistoryInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Информация о посещении. Поле 30.
    /// </summary>
    [PublicAPI]
    [XmlRoot("history")]
    [MoonSharpUserData]
    public sealed class HistoryInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abcde";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Дата в виде строки.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("date")]
        [JsonProperty("date")]
        [Description("Дата")]
        [DisplayName("Дата")]
        public string DateString { get; set; }

        /// <summary>
        /// Дата.
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
        /// Дата начала (полностью).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public DateTime BeginDate
        {
            get { return Date.Add(BeginTime); }
            set
            {
                DateTime date = value.Date;
                TimeSpan time = value - date;
                Date = date;
                BeginTime = time;
            }
        }

        /// <summary>
        /// Время начала.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("beginTime")]
        [JsonProperty("beginTime")]
        [Description("Время начала")]
        [DisplayName("Время начала")]
        public string BeginTimeString { get; set; }

        /// <summary>
        /// Время начала.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan BeginTime
        {
            get { return IrbisDate.ConvertStringToTime(BeginTimeString); }
            set { BeginTimeString = IrbisDate.ConvertTimeToString(value); }
        }

        /// <summary>
        /// Дата окончания (полностью).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public DateTime EndDate
        {
            get { return Date.Add(EndTime); }
            set
            {
                DateTime date = value.Date;
                TimeSpan time = value - date;
                Date = date;
                EndTime = time;
            }
        }

        /// <summary>
        /// Время окончания.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("endTime")]
        [JsonProperty("endTime")]
        [Description("Время окончания")]
        [DisplayName("Время окончания")]
        public string EndTimeString { get; set; }

        /// <summary>
        /// Время окончания.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan EndTime
        {
            get { return IrbisDate.ConvertStringToTime(EndTimeString); }
            set { EndTimeString = IrbisDate.ConvertTimeToString(value); }
        }

        /// <summary>
        /// Продолжительность.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public TimeSpan Duration
        {
            get { return EndTime - BeginTime; }
            set { EndTime = BeginTime + value; }
        }

        /// <summary>
        /// Номер билета.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("ticket")]
        [JsonProperty("ticket")]
        [Description("Номер билета")]
        [DisplayName("Номер билета")]
        public string Ticket { get; set; }

        /// <summary>
        /// ФИО читателя.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlElement("name")]
        [JsonProperty("name")]
        [Description("ФИО читателя")]
        [DisplayName("ФИО читателя")]
        public string Name { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Description("Поле")]
        [DisplayName("Поле")]
        public RecordField Field { get; private set; }

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
        /// Apply to the field
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', DateString)
                .ApplySubField('b', BeginTimeString)
                .ApplySubField('c', EndTimeString)
                .ApplySubField('d', Ticket)
                .ApplySubField('e', Name);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static HistoryInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            HistoryInfo result = new HistoryInfo
            {
                DateString = field.GetFirstSubFieldValue('a'),
                BeginTimeString = field.GetFirstSubFieldValue('b'),
                EndTimeString = field.GetFirstSubFieldValue('c'),
                Ticket = field.GetFirstSubFieldValue('d'),
                Name = field.GetFirstSubFieldValue('e'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static HistoryInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(Tag)
                .Select(field => ParseField(field))
                .ToArray();
        }

        /// <summary>
        /// Convert to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField('a', DateString)
                .AddNonEmptySubField('b', BeginTimeString)
                .AddNonEmptySubField('c', EndTimeString)
                .AddNonEmptySubField('d', Ticket)
                .AddNonEmptySubField('e', Name);

            return result;
        }

        /// <summary>
        /// Should serialize <see cref="EndTimeString"/> field?
        /// </summary>
        public bool ShouldSerializeEndTimeString()
        {
            return !string.IsNullOrEmpty(EndTimeString);
        }

        /// <summary>
        /// Should serialize <see cref="Name"/> field?
        /// </summary>
        public bool ShouldSerializeName()
        {
            return !string.IsNullOrEmpty(Name);
        }

        /// <summary>
        /// /// Should serialize <see cref="Ticket"/> field?
        /// </summary>
        public bool ShouldSerializeTicket()
        {
            return !string.IsNullOrEmpty(Ticket);
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

            DateString = reader.ReadNullableString();
            BeginTimeString = reader.ReadNullableString();
            EndTimeString = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Name = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(DateString)
                .WriteNullable(BeginTimeString)
                .WriteNullable(EndTimeString)
                .WriteNullable(Ticket)
                .WriteNullable(Name);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<HistoryInfo> verifier
                = new Verifier<HistoryInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(DateString, "DateString")
                .NotNullNorEmpty(BeginTimeString, "BeginTimeString")
                .NotNullNorEmpty(Ticket, "Ticket");

            if (!string.IsNullOrEmpty(BeginTimeString)
                && !string.IsNullOrEmpty(EndTimeString))
            {
                verifier.Assert
                    (
                        string.CompareOrdinal
                            (
                                BeginTimeString,
                                EndTimeString
                            ) < 0,
                        "BeginTimeString < EndTimeString"
                    );
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}-{2} [{3}] ({4})",
                    DateString.ToVisibleString(),
                    BeginTimeString.ToVisibleString(),
                    EndTimeString.ToVisibleString(),
                    Ticket.ToVisibleString(),
                    Name.ToVisibleString()
                );
        }

        #endregion
    }
}
