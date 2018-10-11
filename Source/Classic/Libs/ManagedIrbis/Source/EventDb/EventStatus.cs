// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventStatus.cs --
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
    /// Статус мероприятия. Поле 997.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EventStatus
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 997;

        #endregion

        #region Properties

        /// <summary>
        /// Статус мероприятия. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("status")]
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Текст. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("text")]
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// Первоначальная дата (для перенесенных мероприятий). Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("initial-date")]
        [JsonProperty("initialDate")]
        public IrbisDate InitialDate { get; set; }

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
        public static EventStatus Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            EventStatus result = new EventStatus
            {
                Status = field.GetFirstSubFieldValue('a'),
                Text = field.GetFirstSubFieldValue('b'),
                InitialDate = IrbisDate.SafeParse(field.GetFirstSubFieldValue('c')),
                Field = field
            };

            return result;
        }

        #endregion
    }
}