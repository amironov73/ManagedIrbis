// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventCodes.cs --
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
    /// Коды мероприятия. Поле 900.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EventCodes
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcfpz";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 900;

        #endregion

        #region Properties

        /// <summary>
        /// Категория мероприятия. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("category")]
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        /// <summary>
        /// Вид мероприятия. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("kind")]
        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public string Kind { get; set; }

        /// <summary>
        /// Характер мероприятия. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("character")]
        [JsonProperty("character", NullValueHandling = NullValueHandling.Ignore)]
        public string Character { get; set; }

        /// <summary>
        /// Степень доступности. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("accessibility")]
        [JsonProperty("accessibility", NullValueHandling = NullValueHandling.Ignore)]
        public string Accessibility { get; set; }

        /// <summary>
        /// Финансирование мероприятия. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("financing")]
        [JsonProperty("financing", NullValueHandling = NullValueHandling.Ignore)]
        public string Financing { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlAttribute("age")]
        [JsonProperty("age", NullValueHandling = NullValueHandling.Ignore)]
        public string AgeRestrictions { get; set; }

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
        public static EventCodes Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            EventCodes result = new EventCodes
            {
                Category = field.GetFirstSubFieldValue('a'),
                Kind = field.GetFirstSubFieldValue('b'),
                Character = field.GetFirstSubFieldValue('c'),
                Accessibility = field.GetFirstSubFieldValue('p'),
                Financing = field.GetFirstSubFieldValue('f'),
                AgeRestrictions = field.GetFirstSubFieldValue('z'),
                Field = field
            };

            return result;
        }

        #endregion
    }
}