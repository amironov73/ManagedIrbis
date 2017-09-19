// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Данные о резервировании компьютера.
    /// База данных RESERV.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("reservation")]
    public sealed class ReservationInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Читальный зал.
        /// </summary>
        [CanBeNull]
        [Field("10")]
        [XmlAttribute("room")]
        [JsonProperty("room")]
        [Description("Читальный зал")]
        [DisplayName("Читальный зал")]
        public string Room { get; set; }

        /// <summary>
        /// Номер компьютера.
        /// </summary>
        [CanBeNull]
        [Field("11")]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Номер")]
        [DisplayName("Номер")]
        public string Number { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        /// <remarks>
        /// See <see cref="ReservationStatus"/>.
        /// </remarks>
        [CanBeNull]
        [Field("12")]
        [XmlAttribute("status")]
        [JsonProperty("status")]
        [Description("Статус")]
        [DisplayName("Статус")]
        public string Status { get; set; }

        /// <summary>
        /// Описание, например: "AutoCAD, MathCAD".
        /// </summary>
        [CanBeNull]
        [Field("13")]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        [Description("Описание")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Заявки на резервирование.
        /// </summary>
        [NotNull]
        [Field("20")]
        [XmlElement("claim")]
        [JsonProperty("claims")]
        [Description("Заявки")]
        [DisplayName("Заявки")]
        public NonNullCollection<ReservationClaim> Claims { get; private set; }

        /// <summary>
        /// История выдач.
        /// </summary>
        [NotNull]
        [Field("30")]
        [XmlElement("history")]
        [JsonProperty("history")]
        [Description("История")]
        [DisplayName("История")]
        public NonNullCollection<HistoryInfo> History { get; private set; }

        /// <summary>
        /// Associated record.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public MarcRecord Record { get; set; }

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservationInfo()
        {
            Claims = new NonNullCollection<ReservationClaim>();
            History = new NonNullCollection<HistoryInfo>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the record.
        /// </summary>
        public void ApplyToRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            record.Fields
                .ApplyFieldValue(10, Room)
                .ApplyFieldValue(11, Number)
                .ApplyFieldValue(12, Status)
                .ApplyFieldValue(13, Description);
            RecordField[] claims = Claims
                .Select(item => item.ToField())
                .ToArray();
            record.ReplaceField(ReservationClaim.Tag, claims);
            RecordField[] history = History
                .Select(item => item.ToField())
                .ToArray();
            record.ReplaceField(HistoryInfo.Tag, history);
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static ReservationInfo ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            ReservationInfo result = new ReservationInfo
            {
                Room = record.FM(10),
                Number = record.FM(11),
                Status = record.FM(12),
                Description = record.FM(13),
                Record = record
            };
            result.Claims.AddRange
                (
                    ReservationClaim.ParseRecord(record)
                );
            result.History.AddRange
                (
                    HistoryInfo.ParseRecord(record)
                );

            return result;
        }

        /// <summary>
        /// Should serialize <see cref="Claims"/> field?
        /// </summary>
        public bool ShouldSerializeClaims()
        {
            return Claims.Count != 0;
        }

        /// <summary>
        /// Should serialize <see cref="History"/> field?
        /// </summary>
        public bool ShouldSerializeHistory()
        {
            return History.Count != 0;
        }

        /// <summary>
        /// Convert back to record.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            MarcRecord result = new MarcRecord();
            ApplyToRecord(result);

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

            Room = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Status = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Claims = reader.ReadNonNullCollection<ReservationClaim>();
            History = reader.ReadNonNullCollection<HistoryInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Room)
                .WriteNullable(Number)
                .WriteNullable(Status)
                .WriteNullable(Description)
                .WriteCollection(Claims)
                .WriteCollection(History);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReservationInfo> verifier
                = new Verifier<ReservationInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Room, "Room")
                .NotNullNorEmpty(Number, "Number")
                .NotNullNorEmpty(Status, "Status");

            foreach (ReservationClaim claim in Claims)
            {
                verifier.VerifySubObject(claim, "Claims");
            }
            foreach (HistoryInfo history in History)
            {
                verifier.VerifySubObject(history, "History");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Room))
            {
                return string.Format
                    (
                        "[{0}] {1}: {2}",
                        Room,
                        Number.ToVisibleString(),
                        Status.ToVisibleString()
                    );
            }

            return string.Format
                (
                    "{0}: {1}",
                    Number.ToVisibleString(),
                    Status.ToVisibleString()
                );
        }

        #endregion
    }
}
