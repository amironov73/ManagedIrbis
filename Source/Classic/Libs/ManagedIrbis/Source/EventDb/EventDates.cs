// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventDates.cs --
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
    /// Даты мероприятия. Поле 30.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class EventDates
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Начальная дата. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("from")]
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate From { get; set; }

        /// <summary>
        /// Конечная дата. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("till")]
        [JsonProperty("till", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate Till { get; set; }

        /// <summary>
        /// Исключения дат. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("exclude")]
        [JsonProperty("exclude", NullValueHandling = NullValueHandling.Ignore)]
        public string Exclude { get; set; }

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
        public static EventDates Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            EventDates result = new EventDates
            {
                From = IrbisDate.SafeParse(field.GetFirstSubFieldValue('a')),
                Till = IrbisDate.SafeParse(field.GetFirstSubFieldValue('b')),
                Exclude = field.GetFirstSubFieldValue('c'),
                Field = field
            };

            return result;
        }

        #endregion

    }
}