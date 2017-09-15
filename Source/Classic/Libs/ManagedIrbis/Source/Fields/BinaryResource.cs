// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BinaryResource.cs -- field 953.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.Collections;
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
    /// Binary resource in field 953.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Resource}")]
    public sealed class BinaryResource
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Default tag for binary resources.
        /// </summary>
        public const int Tag = 953;

        #endregion

        #region Properties

        /// <summary>
        /// Kind of resource. Subfield a.
        /// </summary>
        /// <remarks>For example, "jpg".</remarks>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("kind")]
        [JsonProperty("kind")]
        [Description("Тип двоичного ресурса")]
        [DisplayName("Тип двоичного ресурса")]
        public string Kind { get; set; }

        /// <summary>
        /// Percent-encoded resource. Subfield b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("resource")]
        [JsonProperty("resource")]
        [Description("Двоичный ресурс (закодированный)")]
        [DisplayName("Двоичный ресурс (закодированный)")]
        public string Resource { get; set; }

        /// <summary>
        /// Title of resource. Subfield t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlElement("title")]
        [JsonProperty("title")]
        [Description("Название двоичного ресурса")]
        [DisplayName("Название двоичного ресурса")]
        public string Title { get; set; }

        /// <summary>
        /// View method. Subfield p.
        /// </summary>
        /// <remarks>
        /// См. <see cref="ResourceView"/>.
        /// </remarks>
        [CanBeNull]
        [SubField('p')]
        [XmlElement("view")]
        [JsonProperty("view")]
        [Description("Характер просмотра")]
        [DisplayName("Характер просмотр")]
        public string View { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource
            (
                [CanBeNull] string kind,
                [CanBeNull] string resource
            )
        {
            Kind = kind;
            Resource = resource;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource
            (
                [CanBeNull] string kind,
                [CanBeNull] string resource,
                [CanBeNull] string title
            )
        {
            Kind = kind;
            Resource = resource;
            Title = title;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Kind)
                .ApplySubField('b', Resource)
                .ApplySubField('t', Title)
                .ApplySubField('p', View);
        }

        /// <summary>
        /// Decode the resource.
        /// </summary>
        [NotNull]
        public byte[] Decode()
        {
            if (string.IsNullOrEmpty(Resource))
            {
                return new byte[0];
            }

            byte[] result = IrbisUtility.DecodePercentString(Resource);

            return result;
        }

        /// <summary>
        /// Encode the resource.
        /// </summary>
        [CanBeNull]
        public string Encode
            (
                [CanBeNull] byte[] array
            )
        {
            Resource = array.IsNullOrEmpty()
                ? null
                : IrbisUtility.EncodePercentString(array);

            return Resource;
        }

        /// <summary>
        /// Parse field 953.
        /// </summary>
        [NotNull]
        public static BinaryResource Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            BinaryResource result = new BinaryResource
            {
                Kind = field.GetFirstSubFieldValue('a'),
                Resource = field.GetFirstSubFieldValue('b'),
                Title = field.GetFirstSubFieldValue('t'),
                View = field.GetFirstSubFieldValue('p')
            };

            return result;
        }

        /// <summary>
        /// Parse fields 953 of the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static BinaryResource[] Parse
            (
                [NotNull] MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            RecordField[] fields = record
                .Fields
                .GetField(tag);

            BinaryResource[] result = fields
                .Select(field => Parse(field))
                .ToArray();

            return result;
        }

        /// <summary>
        /// Parse fields 953 of the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static BinaryResource[] Parse
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
        /// Convert back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', Kind)
                .AddNonEmptySubField('b', Resource)
                .AddNonEmptySubField('t', Title)
                .AddNonEmptySubField('p', View);

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

            Kind = reader.ReadNullableString();
            Resource = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            View = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Kind)
                .WriteNullable(Resource)
                .WriteNullable(Title)
                .WriteNullable(View);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BinaryResource> verifier
                = new Verifier<BinaryResource>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Resource, "Resource");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Kind: {0}, Resource: {1}, Title: {2}",
                    Kind.ToVisibleString(),
                    Resource.ToVisibleString(),
                    Title.ToVisibleString()
                );
        }

        #endregion
    }
}
