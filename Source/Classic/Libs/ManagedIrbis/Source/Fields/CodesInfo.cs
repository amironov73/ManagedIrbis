// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CodesInfo.cs -- коды (поле 900)
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коды (поле 900).
    /// </summary>
    public sealed class CodesInfo
    {
        #region Properties

        /// <summary>
        /// Тип документа. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("type")]
        [JsonProperty("type")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Вид документа. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("kind")]
        [JsonProperty("kind")]
        public string DocumentKind { get; set; }

        /// <summary>
        /// Характер документа. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("character1")]
        [JsonProperty("character1")]
        public string DocumentCharacter1 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("character2")]
        [JsonProperty("character2")]
        public string DocumentCharacter2 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("character3")]
        [JsonProperty("character3")]
        public string DocumentCharacter3 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 4.
        /// </summary>
        [CanBeNull]
        [SubField('4')]
        [XmlAttribute("character4")]
        [JsonProperty("character4")]
        public string DocumentCharacter4 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 5.
        /// </summary>
        [CanBeNull]
        [SubField('5')]
        [XmlAttribute("character5")]
        [JsonProperty("character5")]
        public string DocumentCharacter5 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 6.
        /// </summary>
        [CanBeNull]
        [SubField('6')]
        [XmlAttribute("character6")]
        [JsonProperty("character6")]
        public string DocumentCharacter6 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlAttribute("purpose1")]
        [JsonProperty("purpose1")]
        public string PurposeCode1 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlAttribute("purpose2")]
        [JsonProperty("purpose2")]
        public string PurposeCode2 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе 9.
        /// </summary>
        [CanBeNull]
        [SubField('9')]
        [XmlAttribute("purpose3")]
        [JsonProperty("purpose4")]
        public string PurposeCode3 { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlAttribute("age")]
        [JsonProperty("age")]
        public string AgeRestrictions { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static CodesInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            CodesInfo result = new CodesInfo
                {
                    DocumentType = field.GetFirstSubFieldValue('t'),
                    DocumentKind = field.GetFirstSubFieldValue('b'),
                    DocumentCharacter1 = field.GetFirstSubFieldValue('c'),
                    DocumentCharacter2 = field.GetFirstSubFieldValue('2'),
                    DocumentCharacter3 = field.GetFirstSubFieldValue('3'),
                    DocumentCharacter4 = field.GetFirstSubFieldValue('4'),
                    DocumentCharacter5 = field.GetFirstSubFieldValue('5'),
                    DocumentCharacter6 = field.GetFirstSubFieldValue('6'),
                    PurposeCode1 = field.GetFirstSubFieldValue('x'),
                    PurposeCode2 = field.GetFirstSubFieldValue('y'),
                    PurposeCode3 = field.GetFirstSubFieldValue('9'),
                    AgeRestrictions = field.GetFirstSubFieldValue('z')
                };

            return result;
        }

        /// <summary>
        /// Transform back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("900")
                .AddNonEmptySubField('t', DocumentType)
                .AddNonEmptySubField('b', DocumentKind)
                .AddNonEmptySubField('c', DocumentCharacter1)
                .AddNonEmptySubField('2', DocumentCharacter2)
                .AddNonEmptySubField('3', DocumentCharacter3)
                .AddNonEmptySubField('4', DocumentCharacter4)
                .AddNonEmptySubField('5', DocumentCharacter5)
                .AddNonEmptySubField('6', DocumentCharacter6)
                .AddNonEmptySubField('x', PurposeCode1)
                .AddNonEmptySubField('y', PurposeCode2)
                .AddNonEmptySubField('9', PurposeCode3)
                .AddNonEmptySubField('z', AgeRestrictions);

            return result;
        }

        #endregion
    }
}
