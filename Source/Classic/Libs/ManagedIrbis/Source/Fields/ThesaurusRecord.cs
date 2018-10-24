// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ThesaurusRecord.cs -- запись в базе данных TEZ.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    /// Запись в базе данных TEZ.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ThesaurusRecord
    {
        #region Properties

        /// <summary>
        /// Основной термин.
        /// Поле 1.
        /// </summary>
        [CanBeNull]
        [Field(1)]
        [XmlElement("mainTerm")]
        [JsonProperty("mainTerm", NullValueHandling = NullValueHandling.Ignore)]
        public string MainTerm { get; set; }

        /// <summary>
        /// Вид - Дескриптор(1)/Недескриптор(0).
        /// Поле 2.
        /// Можно не вводить - формируется автоматически.
        /// </summary>
        [CanBeNull]
        [Field(2)]
        [XmlElement("termKind")]
        [JsonProperty("termKind", NullValueHandling = NullValueHandling.Ignore)]
        public string TermKind { get; set; }

        /// <summary>
        /// Русское примечание.
        /// Поле 3.
        /// </summary>
        [CanBeNull]
        [Field(3)]
        [XmlElement("russianNote")]
        [JsonProperty("russianNote", NullValueHandling = NullValueHandling.Ignore)]
        public string RussianNote { get; set; }

        /// <summary>
        /// Вышестоящий дескриптор.
        /// Поле 4.
        /// </summary>
        [CanBeNull]
        [Field(4)]
        [XmlElement("parentDescriptor")]
        [JsonProperty("parentDescriptor", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentDescriptor { get; set; }

        /// <summary>
        /// Ассоциативные дескрипторы.
        /// Поле 5.
        /// </summary>
        [CanBeNull]
        [Field(5)]
        [XmlArray("associativeDescriptors")]
        [XmlArrayItem("descriptor")]
        [JsonProperty("associativeDescriptors", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AssociativeDescriptors { get; set; }

        /// <summary>
        /// Для Недескрипторов-Синонимическая связь.
        /// Поле 6.
        /// </summary>
        [CanBeNull]
        [Field(6)]
        [XmlElement("synonymousLink")]
        [JsonProperty("synonymousLink", NullValueHandling = NullValueHandling.Ignore)]
        public string SynonymousLink { get; set; }

        /// <summary>
        /// Для Недескрипторов - Комбинация Дескрипторов.
        /// Поле 7.
        /// </summary>
        [CanBeNull]
        [Field(7)]
        [XmlArray("descriptorCombination")]
        [XmlArrayItem("descriptor")]
        [JsonProperty("descriptorCombination", NullValueHandling = NullValueHandling.Ignore)]
        public string[] DescriptorCombination { get; set; }

        /// <summary>
        /// Для Недескрипторов - Перечень Дескрипторов.
        /// Поле 8.
        /// </summary>
        [CanBeNull]
        [Field(8)]
        [XmlArray("descriptorList")]
        [XmlArrayItem("descriptor")]
        [JsonProperty("descriptorList", NullValueHandling = NullValueHandling.Ignore)]
        public string[] DescriptorList { get; set; }

        /// <summary>
        /// Английский термин.
        /// Поле 9.
        /// </summary>
        [CanBeNull]
        [Field(9)]
        [XmlElement("englishTerm")]
        [JsonProperty("englishTerm", NullValueHandling = NullValueHandling.Ignore)]
        public string EnglishTerm { get; set; }

        /// <summary>
        /// Аглийское примечание.
        /// Поле 10.
        /// </summary>
        [CanBeNull]
        [Field(10)]
        [XmlElement("englishNote")]
        [JsonProperty("englishNote", NullValueHandling = NullValueHandling.Ignore)]
        public string EnglishNote { get; set; }

        /// <summary>
        /// Французский термин.
        /// Поле 11.
        /// </summary>
        [CanBeNull]
        [Field(11)]
        [XmlElement("frenchTerm")]
        [JsonProperty("frenchTerm", NullValueHandling = NullValueHandling.Ignore)]
        public string FrenchTerm { get; set; }

        /// <summary>
        /// Французское примечание.
        /// Поле 12.
        /// </summary>
        [CanBeNull]
        [Field(12)]
        [XmlElement("frenchNote")]
        [JsonProperty("frenchNote", NullValueHandling = NullValueHandling.Ignore)]
        public string FrenchNote { get; set; }

        /// <summary>
        /// Другие примечания.
        /// Поле 13.
        /// </summary>
        [CanBeNull]
        [Field(13)]
        [XmlElement("otherNote")]
        [JsonProperty("otherNote", NullValueHandling = NullValueHandling.Ignore)]
        public string OtherNote { get; set; }

        /// <summary>
        /// Уровень дескриптора в иерархии.
        /// Поле 14.
        /// Можно не вводить - формируется автоматически.
        /// </summary>
        [CanBeNull]
        [Field(14)]
        [XmlElement("level")]
        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static ThesaurusRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            ThesaurusRecord result = new ThesaurusRecord
            {
                MainTerm = record.FM(1),
                TermKind = record.FM(2),
                RussianNote = record.FM(3),
                ParentDescriptor = record.FM(4),
                AssociativeDescriptors = record.FMA(5),
                SynonymousLink = record.FM(6),
                DescriptorCombination = record.FMA(7),
                DescriptorList = record.FMA(8),
                EnglishTerm = record.FM(9),
                EnglishNote = record.FM(10),
                FrenchTerm = record.FM(11),
                FrenchNote = record.FM(12),
                OtherNote = record.FM(13),
                Level = record.FM(14)
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MainTerm.ToVisibleString();
        }

        #endregion
    }
}
