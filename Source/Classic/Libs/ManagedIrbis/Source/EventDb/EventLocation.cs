// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventLocation.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.EventDb
{
    /// <summary>
    /// Место проведения мероприятия. Поле 210.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EventLocation
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "eghts";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 210;

        #endregion

        #region Properties

        /// <summary>
        /// Место проведения. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("location")]
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        /// <summary>
        /// Адрес. Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("address")]
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Телефон. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("phone")]
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        /// <summary>
        /// Страна. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlAttribute("country")]
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// Город. подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("city")]
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

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
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static EventLocation Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            EventLocation result = new EventLocation
            {
                Location = field.GetFirstSubFieldValue('e'),
                Address = field.GetFirstSubFieldValue('h'),
                Phone = field.GetFirstSubFieldValue('t'),
                Country = field.GetFirstSubFieldValue('s'),
                City = field.GetFirstSubFieldValue('g'),
                Field = field
            };

            return result;
        }

        #endregion
    }
}