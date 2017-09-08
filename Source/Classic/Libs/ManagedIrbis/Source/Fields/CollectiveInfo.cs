// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CollectiveInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коллективный (в т. ч. временный) автор.
    /// Раскладка полей 710, 711, 962, 972.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CollectiveInfo
    {
        #region Properties


        /// <summary>
        /// Empty array of the <see cref="CollectiveInfo"/>.
        /// </summary>
        public static readonly CollectiveInfo[] EmptyArray
            = new CollectiveInfo[0];

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static int[] KnownTags { get { return _knownTags; } }

        /// <summary>
        /// Наименование. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("title")]
        [JsonProperty("title")]
        [Description("Наименование")]
        [DisplayName("Наименование")]
        public string Title { get; set; }

        /// <summary>
        /// Страна. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlElement("country")]
        [JsonProperty("country")]
        [Description("Страна")]
        [DisplayName("Страна")]
        public string Country { get; set; }

        /// <summary>
        /// Аббревиатура. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlElement("abbreviation")]
        [JsonProperty("abbreviation")]
        [Description("Аббревиатура")]
        [DisplayName("Аббревиатура")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Номер. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlElement("number")]
        [JsonProperty("number")]
        [Description("Номер")]
        [DisplayName("Номер")]
        public string Number { get; set; }

        /// <summary>
        /// Дата проведения мероприятия. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("date")]
        [JsonProperty("date")]
        [Description("Дата проведения мероприятия")]
        [DisplayName("Дата проведения мероприятия")]
        public string Date { get; set; }

        /// <summary>
        /// Город. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("city")]
        [JsonProperty("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string City1 { get; set; }

        /// <summary>
        /// Подразделение. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("department")]
        [JsonProperty("department")]
        [Description("Подразделение")]
        [DisplayName("Подразделение")]
        public string Department { get; set; }

        /// <summary>
        /// Характерное название подразделения. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlElement("characteristic")]
        [JsonProperty("characteristic")]
        [Description("Характерное название подразделения")]
        [DisplayName("Характерное название подразделения")]
        public bool Characteristic { get; set; }

        /// <summary>
        /// Сокращение по ГОСТ. Подполе 7.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlElement("gost")]
        [JsonProperty("gost")]
        [Description("Сокращение по ГОСТ")]
        [DisplayName("Сокращение по ГОСТ")]
        public string Gost { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static readonly int[] _knownTags =
        {
            710, 711, 962, 972
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="CollectiveInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Title)
                .ApplySubField('s', Country)
                .ApplySubField('r', Abbreviation)
                .ApplySubField('n', Number)
                .ApplySubField('f', Date)
                .ApplySubField('c', City1)
                .ApplySubField('b', Department)
                .ApplySubField('x', Characteristic ? "1" : null)
                .ApplySubField('7', Gost);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static CollectiveInfo[] ParseRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] int[] tags
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(tags, "tags");

            List<CollectiveInfo> result = new List<CollectiveInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag.OneOf(tags))
                {
                    CollectiveInfo collective = ParseField(field);
                    result.Add(collective);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static CollectiveInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            CollectiveInfo result = new CollectiveInfo
            {
                Title = field.GetFirstSubFieldValue('a'),
                Country = field.GetFirstSubFieldValue('s'),
                Abbreviation = field.GetFirstSubFieldValue('r'),
                Number = field.GetFirstSubFieldValue('n'),
                Date = field.GetFirstSubFieldValue('f'),
                City1 = field.GetFirstSubFieldValue('c'),
                Department = field.GetFirstSubFieldValue('b'),
                Characteristic = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('x')
                    ),
                Gost = field.GetFirstSubFieldValue('7'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Convert <see cref="CollectiveInfo"/>
        /// to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField
            (
                [NotNull] string tag
            )
        {
            Code.NotNullNorEmpty(tag, "tag");

            RecordField result = new RecordField(tag);
            result
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('s', Country)
                .AddNonEmptySubField('r', Abbreviation)
                .AddNonEmptySubField('n', Number)
                .AddNonEmptySubField('f', Date)
                .AddNonEmptySubField('c', City1)
                .AddNonEmptySubField('b', Department)
                .AddNonEmptySubField('x', Characteristic ? "1" : null)
                .AddNonEmptySubField('7', Gost);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
