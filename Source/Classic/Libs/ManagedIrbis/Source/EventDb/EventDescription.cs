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
    public class EventDescription
    {
        #region Properties

        /// <summary>
        /// Название мероприятия. Поле 972.
        /// </summary>
        [CanBeNull]
        [XmlElement("title")]
        [JsonProperty("title")]
        public EventTitle Title { get; set; }

        /// <summary>
        /// Даты. Поле 30.
        /// </summary>
        [CanBeNull]
        [XmlElement("dates")]
        [JsonProperty("dates")]
        public EventDates Dates { get; set; }

        /// <summary>
        /// Время. Поле 31.
        /// </summary>
        [CanBeNull]
        [XmlElement("times")]
        [JsonProperty("times")]
        public EventTimes Times { get; set; }

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
                Dates = EventDates.Parse(record.Fields.GetFirstField(30)),
                Times = EventTimes.Parse(record.Fields.GetFirstField(31))
            };

            return result;
        }

        #endregion
    }
}