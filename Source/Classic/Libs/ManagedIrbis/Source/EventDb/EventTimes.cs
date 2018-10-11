// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventTimes.cs --
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
    /// Время мероприятия. Поле 31.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EventTimes
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "ab";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 31;

        #endregion

        #region Properties

        /// <summary>
        /// Время проведения от... Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("from")]
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }

        /// <summary>
        /// Время проведения до ... Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("till")]
        [JsonProperty("till", NullValueHandling = NullValueHandling.Ignore)]
        public string Till { get; set; }

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
        public static EventTimes Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            EventTimes result = new EventTimes
            {
                From = field.GetFirstSubFieldValue('a'),
                Till = field.GetFirstSubFieldValue('b'),
                Field = field
            };

            return result;
        }

        #endregion
    }
}