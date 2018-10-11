// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventDescription.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.EventDb
{
    /// <summary>
    /// Информация о мероприятии.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EventDescription
    {
        #region Properties

        /// <summary>
        /// Название мероприятия. Поле 972.
        /// </summary>
        [CanBeNull]
        [XmlElement("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public EventTitle Title { get; set; }

        /// <summary>
        /// Коды. Поле 900.
        /// </summary>
        [CanBeNull]
        [XmlElement("codes")]
        [JsonProperty("codes", NullValueHandling = NullValueHandling.Ignore)]
        public EventCodes Codes { get; set; }

        /// <summary>
        /// Даты. Поле 30.
        /// </summary>
        [CanBeNull]
        [XmlElement("dates")]
        [JsonProperty("dates", NullValueHandling = NullValueHandling.Ignore)]
        public EventDates Dates { get; set; }

        /// <summary>
        /// Время. Поле 31.
        /// </summary>
        [CanBeNull]
        [XmlElement("times")]
        [JsonProperty("times", NullValueHandling = NullValueHandling.Ignore)]
        public EventTimes Times { get; set; }

        /// <summary>
        /// Место проведения мероприятия. Поле 210.
        /// </summary>
        [CanBeNull]
        [XmlElement("location")]
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public EventLocation Location { get; set; }

        /// <summary>
        /// Статус мероприятия. Поле 997.
        /// </summary>
        [CanBeNull]
        [XmlElement("status")]
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public EventStatus Status { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static EventDescription Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            EventDescription result = new EventDescription
            {
                Title = EventTitle.Parse(record.Fields.GetFirstField(972)),
                Codes = EventCodes.Parse(record.Fields.GetFirstField(900)),
                Dates = EventDates.Parse(record.Fields.GetFirstField(30)),
                Times = EventTimes.Parse(record.Fields.GetFirstField(31)),
                Location = EventLocation.Parse(record.Fields.GetFirstField(210)),
                Status = EventStatus.Parse(record.Fields.GetFirstField(997)),
                Record = record
            };

            return result;
        }

        #endregion
    }
}