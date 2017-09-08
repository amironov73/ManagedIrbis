// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PersonaliaInfo.cs -- 
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PersonaliaInfo
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 600;

        #endregion

        #region Properties

        /// <summary>
        /// Вид данных. Подполе ).
        /// </summary>
        [CanBeNull]
        [SubField(')')]
        [XmlElement("dataKind")]
        [JsonProperty("dataKind")]
        [Description("Вид данных")]
        [DisplayName("Вид данных")]
        public string DataKind { get; set; }

        /// <summary>
        /// Текст. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("text")]
        [JsonProperty("text")]
        [Description("Текст")]
        [DisplayName("Текст")]
        public string Text { get; set; }

        /// <summary>
        /// Фамилия, инициалы. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("name")]
        [JsonProperty("name")]
        [Description("Фамилия, инициалы")]
        [DisplayName("Фамилия, инициалы")]
        public string Name { get; set; }

        /// <summary>
        /// Расширение инициалов. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("extension")]
        [JsonProperty("extension")]
        [Description("Расширение инициалов")]
        [DisplayName("Расширение инициалов")]
        public string Extension { get; set; }

        /// <summary>
        /// Инвертирование имени недопустимо? Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlElement("cantBeInverted")]
        [JsonProperty("cantBeInverted")]
        [Description("Инвертирование имени недопустимо")]
        [DisplayName("Инвертирование имени недопустимо")]
        public bool CantBeInverted { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (отец, сын, младший, старший
        /// и т. п.). Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("postfix")]
        [JsonProperty("postfix")]
        [Description("Неотъемлемая часть имени. Подполе 1.")]
        [DisplayName("Неотъемлемая часть имени")]
        public string Postfix { get; set; }

        /// <summary>
        /// Дополнения к имени кроме дат (род деятельности, звание,
        /// титул и т. д.). Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("appendix")]
        [JsonProperty("appendix")]
        [Description("Дополнения к имени кроме дат. Подполе c.")]
        [DisplayName("Дополнения к имени кроме дат")]
        public string Appendix { get; set; }

        /// <summary>
        /// Династический номер (римские цифры). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Династический номер (римские цифры). Подполе d.")]
        [DisplayName("Династический номер (римские цифры)")]
        public string Number { get; set; }

        /// <summary>
        /// Даты жизни. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dates")]
        [JsonProperty("dates")]
        [Description("Даты жизни. Подполе f.")]
        [DisplayName("Даты жизни")]
        public string Dates { get; set; }

        /// <summary>
        /// Разночтение фамилии. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlAttribute("variant")]
        [JsonProperty("variant")]
        [Description("Разночтение фамилии. Подполе r.")]
        [DisplayName("Разночтение фамилии")]
        public string Variant { get; set; }

        /// <summary>
        /// Место работы автора. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("workplace")]
        [JsonProperty("workplace")]
        [Description("Место работы автора. Подполе p.")]
        [DisplayName("Место работы автора")]
        public string WorkPlace { get; set; }

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

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="PersonaliaInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField(')', DataKind)
                .ApplySubField('b', Text)
                .ApplySubField('a', Name)
                .ApplySubField('g', Extension)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static PersonaliaInfo[] ParseRecord
            (
                [NotNull] MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            List<PersonaliaInfo> result = new List<PersonaliaInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    PersonaliaInfo personalia = ParseField(field);
                    result.Add(personalia);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [CanBeNull]
        public static PersonaliaInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            PersonaliaInfo result = new PersonaliaInfo
            {
                DataKind = field.GetFirstSubFieldValue(')'),
                Text = field.GetFirstSubFieldValue('b'),
                Name = field.GetFirstSubFieldValue('a'),
                Extension = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Convert <see cref="PersonaliaInfo"/>
        /// to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField(')', DataKind)
                .AddNonEmptySubField('b', Text)
                .AddNonEmptySubField('a', Name)
                .AddNonEmptySubField('g', Extension)
                .AddNonEmptySubField('9', CantBeInverted ? "1" : null)
                .AddNonEmptySubField('1', Postfix)
                .AddNonEmptySubField('c', Appendix)
                .AddNonEmptySubField('d', Number)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('r', Variant)
                .AddNonEmptySubField('p', WorkPlace);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
