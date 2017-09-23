// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HeadingInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Предметная рубрика, поле 606.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("heading")]
    public sealed class HeadingInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 606;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdegho9";

        #endregion

        #region Properties

        /// <summary>
        /// Предметный заголовок. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("title")]
        [JsonProperty("title")]
        [Description("Предметный заголовок")]
        [DisplayName("Предметный заголовок")]
        public string Title { get; set; }

        /// <summary>
        /// Первый подзаголовок. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("subtitle1")]
        [JsonProperty("subtitle1")]
        [Description("Первый подзаголовок")]
        [DisplayName("Первый подзаголовок")]
        public string Subtitle1 { get; set; }

        /// <summary>
        /// Второй подзаголовок. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("subtitle2")]
        [JsonProperty("subtitle2")]
        [Description("Второй подзаголовок")]
        [DisplayName("Второй подзаголовок")]
        public string Subtitle2 { get; set; }

        /// <summary>
        /// Третий подзаголовок. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("subtitle3")]
        [JsonProperty("subtitle3")]
        [Description("Третий подзаголовок")]
        [DisplayName("Третий подзаголовок")]
        public string Subtitle3 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("geoSubtitle1")]
        [JsonProperty("geoSubtitle1")]
        [Description("Первый географический подзаголовок")]
        [DisplayName("Первый географический подзаголовок")]
        public string GeographicalSubtitle1 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlElement("geoSubtitle2")]
        [JsonProperty("geoSubtitle2")]
        [Description("Второй географический подзаголовок")]
        [DisplayName("Второй географический подзаголовок")]
        public string GeographicalSubtitle2 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе o.
        /// </summary>
        [CanBeNull]
        [SubField('o')]
        [XmlElement("geoSubtitle3")]
        [JsonProperty("geoSubtitle3")]
        [Description("Третий географический подзаголовок")]
        [DisplayName("Третий географический подзаголовок")]
        public string GeographicalSubtitle3 { get; set; }

        /// <summary>
        /// Хронологический подзаголовок. Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlElement("chronoSubtitle")]
        [JsonProperty("chronoSubtitle")]
        [Description("Хронологический подзаголовок")]
        [DisplayName("Хронологический подзаголовок")]
        public string ChronologicalSubtitle { get; set; }

        /// <summary>
        /// Формальный подзаголовок (аспект). Подполе 9.
        /// </summary>
        [CanBeNull]
        [SubField('9')]
        [XmlElement("aspect")]
        [JsonProperty("aspect")]
        [Description("Формальный подзаголовок (аспект)")]
        [DisplayName("Формальный подзаголовок (аспект)")]
        public string Aspect { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private void _AppendSubTitle
            (
                [NotNull] StringBuilder builder,
                [CanBeNull] string subtitle
            )
        {
            if (!string.IsNullOrEmpty(subtitle))
            {
                builder.AppendFormat(" -- {0}", subtitle);
            }
        }

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
                .ApplySubField('a', Title)
                .ApplySubField('b', Subtitle1)
                .ApplySubField('c', Subtitle2)
                .ApplySubField('d', Subtitle3)
                .ApplySubField('g', GeographicalSubtitle1)
                .ApplySubField('e', GeographicalSubtitle2)
                .ApplySubField('o', GeographicalSubtitle3)
                .ApplySubField('h', ChronologicalSubtitle)
                .ApplySubField('9', Aspect);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static HeadingInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            HeadingInfo result = new HeadingInfo
            {
                Title = field.GetFirstSubFieldValue('a'),
                Subtitle1 = field.GetFirstSubFieldValue('b'),
                Subtitle2 = field.GetFirstSubFieldValue('c'),
                Subtitle3 = field.GetFirstSubFieldValue('d'),
                GeographicalSubtitle1 = field.GetFirstSubFieldValue('g'),
                GeographicalSubtitle2 = field.GetFirstSubFieldValue('e'),
                GeographicalSubtitle3 = field.GetFirstSubFieldValue('o'),
                ChronologicalSubtitle = field.GetFirstSubFieldValue('h'),
                Aspect = field.GetFirstSubFieldValue('9'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static HeadingInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<HeadingInfo> result = new List<HeadingInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    HeadingInfo heading = ParseField(field);
                    result.Add(heading);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Convert <see cref="HeadingInfo"/>
        /// to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag);
            result
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('b', Subtitle1)
                .AddNonEmptySubField('c', Subtitle2)
                .AddNonEmptySubField('d', Subtitle3)
                .AddNonEmptySubField('g', GeographicalSubtitle1)
                .AddNonEmptySubField('e', GeographicalSubtitle2)
                .AddNonEmptySubField('o', GeographicalSubtitle3)
                .AddNonEmptySubField('h', ChronologicalSubtitle)
                .AddNonEmptySubField('9', Aspect)
                .AddSubFields(UnknownSubFields);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Title = reader.ReadNullableString();
            Subtitle1 = reader.ReadNullableString();
            Subtitle2 = reader.ReadNullableString();
            Subtitle3 = reader.ReadNullableString();
            GeographicalSubtitle1 = reader.ReadNullableString();
            GeographicalSubtitle2 = reader.ReadNullableString();
            GeographicalSubtitle3 = reader.ReadNullableString();
            ChronologicalSubtitle = reader.ReadNullableString();
            Aspect = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Title)
                .WriteNullable(Subtitle1)
                .WriteNullable(Subtitle2)
                .WriteNullable(Subtitle3)
                .WriteNullable(GeographicalSubtitle1)
                .WriteNullable(GeographicalSubtitle2)
                .WriteNullable(GeographicalSubtitle3)
                .WriteNullable(ChronologicalSubtitle)
                .WriteNullable(Aspect)
                .WriteNullableArray(UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<HeadingInfo> verifier
                = new Verifier<HeadingInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return Title.ToVisibleString();
            }

            StringBuilder result = new StringBuilder();
            result.Append(Title);
            _AppendSubTitle(result, Subtitle1);
            _AppendSubTitle(result, Subtitle2);
            _AppendSubTitle(result, Subtitle3);
            _AppendSubTitle(result, GeographicalSubtitle1);
            _AppendSubTitle(result, GeographicalSubtitle2);
            _AppendSubTitle(result, GeographicalSubtitle3);
            _AppendSubTitle(result, ChronologicalSubtitle);
            _AppendSubTitle(result, Aspect);

            return result.ToString();
        }

        #endregion
    }
}

