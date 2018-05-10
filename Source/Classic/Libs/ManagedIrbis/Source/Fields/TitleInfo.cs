// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TitleInfo.cs -- заглавие, поле 200
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сведения о заглавии, поле 200.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{VolumeNumber} {Title}")]
    public sealed class TitleInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abefguv";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 200;

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и номер тома. Подполе v.
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        [Description("Обозначение и номер тома")]
        [DisplayName("Обозначение и номер тома")]
        public string VolumeNumber { get; set; }

        /// <summary>
        /// Заглавие.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        /// <summary>
        /// Нехарактерное заглавие. Подполе u.
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("specific")]
        [JsonProperty("specific")]
        [Description("Нехарактерное заглавие")]
        [DisplayName("Нехарактерное заглавие")]
        public string Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("general")]
        [JsonProperty("general")]
        [Description("Общее обозначение материала")]
        [DisplayName("Общее обозначение материала")]
        public string General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("subtitle")]
        [JsonProperty("subtitle")]
        [Description("Сведения, относящиеся к заглавию")]
        [DisplayName("Сведения, относящиеся к заглавию")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Первые сведения об ответственности. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("first")]
        [JsonProperty("first")]
        [Description("Первые сведения об ответственности")]
        [DisplayName("Первые сведения об ответственности")]
        public string FirstResponsibility { get; set; }

        /// <summary>
        /// Последующие сведения об ответственности. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("other")]
        [JsonProperty("other")]
        [Description("Последующие сведения об ответственности")]
        [DisplayName("Последующие сведения об ответственности")]
        public string OtherResponsibility { get; set; }

        /// <summary>
        /// Full title.
        /// </summary>
        [NotNull]
        public string FullTitle
        {
            get
            {
                StringBuilder result = new StringBuilder();
                if (!string.IsNullOrEmpty(VolumeNumber))
                {
                    result.Append(VolumeNumber);
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    if (result.Length != 0)
                    {
                        result.Append(". ");
                    }

                    result.Append(Title);
                }

                if (!string.IsNullOrEmpty(General))
                {
                    if (result.Length != 0)
                    {
                        result.Append(" ");
                    }

                    result.Append('[');
                    result.Append(General);
                    result.Append(']');
                }

                if (!string.IsNullOrEmpty(Subtitle))
                {
                    if (result.Length != 0)
                    {
                        result.Append(": ");
                        result.Append(Subtitle);
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Description("Поле")]
        [DisplayName("Поле")]
        public RecordField Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Description("Произвольные данные")]
        [DisplayName("Произвольные данные")]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo
            (
                string title
            )
        {
            Title = title;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo
            (
                string volumeNumber,
                string title
            )
        {
            VolumeNumber = volumeNumber;
            Title = title;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse field 200.
        /// </summary>
        [NotNull]
        public static TitleInfo ParseField200
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO: support for unknown subfields

            TitleInfo result = new TitleInfo
            {
                VolumeNumber = field.GetFirstSubFieldValue('v'),
                Title = field.GetFirstSubFieldValue('a'),
                Specific = field.GetFirstSubFieldValue('u'),
                General = field.GetFirstSubFieldValue('b'),
                Subtitle = field.GetFirstSubFieldValue('e'),
                FirstResponsibility = field.GetFirstSubFieldValue('f'),
                OtherResponsibility = field.GetFirstSubFieldValue('g'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse field 330 or 922.
        /// </summary>
        [NotNull]
        public static TitleInfo ParseField330
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO: support for unknown subfields

            TitleInfo result = new TitleInfo
            {
                Title = field.GetFirstSubFieldValue('c'),
                Subtitle = field.GetFirstSubFieldValue('e'),
                FirstResponsibility = field.GetFirstSubFieldValue('g'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static TitleInfo[] Parse
            (
                [NotNull] MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(tag)
                .Select(field => ParseField200(field))
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static TitleInfo[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Should serialize <see cref="FirstResponsibility"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFirstResponsibility()
        {
            return !ReferenceEquals(FirstResponsibility, null);
        }

        /// <summary>
        /// Should serialize <see cref="General"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGeneral()
        {
            return !ReferenceEquals(General, null);
        }

        /// <summary>
        /// Should serialize <see cref="OtherResponsibility"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOtherResponsibility()
        {
            return !ReferenceEquals(OtherResponsibility, null);
        }

        /// <summary>
        /// Should serialize <see cref="Title"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTitle()
        {
            return !ReferenceEquals(Title, null);
        }

        /// <summary>
        /// Should serialize <see cref="Specific"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpecific()
        {
            return !ReferenceEquals(Specific, null);
        }

        /// <summary>
        /// Should serialize <see cref="Subtitle"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSubtitle()
        {
            return !ReferenceEquals(Subtitle, null);
        }

        /// <summary>
        /// Should serialize <see cref="VolumeNumber"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeVolumeNumber()
        {
            return !ReferenceEquals(VolumeNumber, null);
        }

        /// <summary>
        /// Превращение обратно в поле 200.
        /// </summary>
        [NotNull]
        public RecordField ToField200()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('v', VolumeNumber)
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('u', Specific)
                .AddNonEmptySubField('b', General)
                .AddNonEmptySubField('e', Subtitle)
                .AddNonEmptySubField('f', FirstResponsibility)
                .AddNonEmptySubField('g', OtherResponsibility);

            return result;
        }

        /// <summary>
        /// Convert back to field 330/922.
        /// </summary>
        [NotNull]
        public RecordField ToField330
            (
                int tag
            )
        {
            RecordField result = new RecordField(tag)
                .AddNonEmptySubField('c', Title)
                .AddNonEmptySubField('e', Subtitle)
                .AddNonEmptySubField('g', FirstResponsibility);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            VolumeNumber = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Specific = reader.ReadNullableString();
            General = reader.ReadNullableString();
            Subtitle = reader.ReadNullableString();
            FirstResponsibility = reader.ReadNullableString();
            OtherResponsibility = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(VolumeNumber)
                .WriteNullable(Title)
                .WriteNullable(Specific)
                .WriteNullable(General)
                .WriteNullable(Subtitle)
                .WriteNullable(FirstResponsibility)
                .WriteNullable(OtherResponsibility);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<TitleInfo> verifier
                = new Verifier<TitleInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(VolumeNumber))
            {
                return string.Format
                (
                    "Title: {0}, Subtitle: {1}",
                    Title.ToVisibleString(),
                    Subtitle.ToVisibleString()
                );
            }

            return string.Format
                (
                    "Volume: {0}, Title: {1}, Subtitle: {2}",
                    VolumeNumber.ToVisibleString(),
                    Title.ToVisibleString(),
                    Subtitle.ToVisibleString()
                );
        }

        #endregion
    }
}
