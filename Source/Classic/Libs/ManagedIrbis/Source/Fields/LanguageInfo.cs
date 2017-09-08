// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LanguageInfo.cs -- 
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
    /// Язык документа (дополнительные данные). Поле 919.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LanguageInfo
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abefgklnoz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 919;

        #endregion

        #region Properties

        /// <summary>
        /// Язык каталогизации. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("catalogingLanguage")]
        [JsonProperty("catalogingLanguage")]
        [Description("Язык каталогизации")]
        [DisplayName("Язык каталогизации")]
        public string CatalogingLanguage { get; set; }

        /// <summary>
        /// Правила каталогизации. Подполе k.
        /// </summary>
        /// <remarks>See
        /// <see cref="ManagedIrbis.Fields.CatalogingRules"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('k')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Правила каталогизации")]
        [DisplayName("Правила каталогизации")]
        public string CatalogingRules { get; set; }

        /// <summary>
        /// Набор символов. Подполе n.
        /// </summary>
        /// <remarks>See <see cref="CharacterSetCode"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('n')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Набор символов")]
        [DisplayName("Набор символов")]
        public string CharacterSet { get; set; }

        /// <summary>
        /// Графика заглавия. Подполе g.
        /// </summary>
        /// <remarks>See <see cref="Fields.TitleCharacterSet"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Графика заглавия")]
        [DisplayName("Графика заглавия")]
        public string TitleCharacterSet { get; set; }

        /// <summary>
        /// Язык промежуточного перевода. Подполе b.
        /// </summary>
        /// <remarks>See <see cref="LanguageCode"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык промежуточного перевода")]
        [DisplayName("Язык промежуточного перевода")]
        public string IntermediateTranslationLanguage { get; set; }

        /// <summary>
        /// Язык оригинала. Подполе o.
        /// </summary>
        [CanBeNull]
        [SubField('o')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык оригинала")]
        [DisplayName("Язык оригинала")]
        public string OriginalLanguage { get; set; }

        /// <summary>
        /// Язык оглавления. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык оглавления")]
        [DisplayName("Язык оглавления")]
        public string TocLanguage { get; set; }

        /// <summary>
        /// Язык титульного листа. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык титульного листа")]
        [DisplayName("Язык титульного листа")]
        public string TitlePageLanguage { get; set; }

        /// <summary>
        /// Язык основного заглавия. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык основного заглавия")]
        [DisplayName("Язык основного заглавия")]
        public string MainTitleLanguage { get; set; }

        /// <summary>
        /// Язык сопроводительного материала. Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlElement("")]
        [JsonProperty("")]
        [Description("Язык сопроводительного материала")]
        [DisplayName("Язык сопроводительного материала")]
        public string AccompanyingMaterialLanguage { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="HeadingInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', CatalogingLanguage)
                .ApplySubField('k', CatalogingRules)
                .ApplySubField('n', CharacterSet)
                .ApplySubField('g', TitleCharacterSet)
                .ApplySubField('b', IntermediateTranslationLanguage)
                .ApplySubField('e', OriginalLanguage)
                .ApplySubField('o', TocLanguage)
                .ApplySubField('f', TitlePageLanguage)
                .ApplySubField('z', MainTitleLanguage)
                .ApplySubField('i', AccompanyingMaterialLanguage);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static LanguageInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            LanguageInfo result = new LanguageInfo
            {
                CatalogingLanguage = field.GetFirstSubFieldValue('a'),
                CatalogingRules = field.GetFirstSubFieldValue('k'),
                CharacterSet = field.GetFirstSubFieldValue('n'),
                TitleCharacterSet = field.GetFirstSubFieldValue('g'),
                IntermediateTranslationLanguage = field.GetFirstSubFieldValue('b'),
                OriginalLanguage = field.GetFirstSubFieldValue('e'),
                TocLanguage = field.GetFirstSubFieldValue('o'),
                TitlePageLanguage = field.GetFirstSubFieldValue('f'),
                MainTitleLanguage = field.GetFirstSubFieldValue('z'),
                AccompanyingMaterialLanguage = field.GetFirstSubFieldValue('i'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static LanguageInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<LanguageInfo> result = new List<LanguageInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    LanguageInfo heading = ParseField(field);
                    result.Add(heading);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Convert <see cref="LanguageInfo"/>
        /// to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField('a', CatalogingLanguage)
                .AddNonEmptySubField('k', CatalogingRules)
                .AddNonEmptySubField('n', CharacterSet)
                .AddNonEmptySubField('g', TitleCharacterSet)
                .AddNonEmptySubField('b', IntermediateTranslationLanguage)
                .AddNonEmptySubField('e', OriginalLanguage)
                .AddNonEmptySubField('o', TocLanguage)
                .AddNonEmptySubField('f', TitlePageLanguage)
                .AddNonEmptySubField('z', MainTitleLanguage)
                .AddNonEmptySubField('i', AccompanyingMaterialLanguage);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return CatalogingLanguage.ToVisibleString();
        }

        #endregion
    }
}
