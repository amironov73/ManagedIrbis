// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TitleInfo.cs -- заглавие, поле 200
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abefguv";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "200";

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и номер тома. Подполе v.
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string VolumeNumber { get; set; }

        /// <summary>
        /// Заглавие.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Нехарактерное заглавие. Подполе u.
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("specific")]
        [JsonProperty("specific")]
        public string Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("general")]
        [JsonProperty("general")]
        public string General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("subtitle")]
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Первые сведения об ответственности. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("first")]
        [JsonProperty("first")]
        public string FirstResponsibility { get; set; }

        /// <summary>
        /// Последующие сведения об ответственности. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("other")]
        [JsonProperty("other")]
        public string OtherResponsibility { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
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
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static TitleInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull (field, "field");

            // TODO: support for unknown subfields

            TitleInfo result = new TitleInfo
            {
                VolumeNumber = field.GetFirstSubFieldValue('v'),
                Title = field.GetFirstSubFieldValue('a'),
                Specific = field.GetFirstSubFieldValue('u'),
                General = field.GetFirstSubFieldValue('b'),
                Subtitle = field.GetFirstSubFieldValue('e'),
                FirstResponsibility = field.GetFirstSubFieldValue('f'),
                OtherResponsibility = field.GetFirstSubFieldValue('g')
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
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
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
        /// Превращение обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
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

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
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
