// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReplacementInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
    /// Сведения о замене утерянных книг, поле 80 в БД CMPL.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("replacement")]
    public sealed class ReplacementInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 80;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abd";

        #endregion

        #region Properties

        /// <summary>
        /// Инвентарный номер утерянного экземпляра. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("lost")]
        [JsonProperty("lost", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Утерянный экземпляр")]
        [DisplayName("Утерянный экземпляр")]
        public string LostNumber { get; set; }

        /// <summary>
        /// Место хранения экземпляра. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("storage")]
        [JsonProperty("storage", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Место хранения")]
        [DisplayName("Место хранения")]
        public string StorageCode { get; set; }

        /// <summary>
        /// Инвентарный номер заменяющего экземпляра.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("replacing")]
        [JsonProperty("replacing", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Заменяющий экземпляр")]
        [DisplayName("Заменяющий экземпляр")]
        public string ReplacingNumber { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReplacementInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReplacementInfo
            (
                [CanBeNull] string lostNumber,
                [CanBeNull] string storageCode,
                [CanBeNull] string replacingNumber
            )
        {
            LostNumber = lostNumber;
            StorageCode = storageCode;
            ReplacingNumber = replacingNumber;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ReplacementInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', LostNumber)
                .ApplySubField('d', StorageCode)
                .ApplySubField('b', ReplacingNumber);
        }

        /// <summary>
        /// Parse the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static ReplacementInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ReplacementInfo result = new ReplacementInfo
            {
                LostNumber = field.GetFirstSubFieldValue('a'),
                StorageCode = field.GetFirstSubFieldValue('d'),
                ReplacingNumber = field.GetFirstSubFieldValue('b'),
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
        public static ReplacementInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<ReplacementInfo> result = new List<ReplacementInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    result.Add(ParseField(field));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert the <see cref="ReplacementInfo"/>
        /// back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', LostNumber)
                .AddNonEmptySubField('d', StorageCode)
                .AddNonEmptySubField('b', ReplacingNumber)
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

            LostNumber = reader.ReadNullableString();
            StorageCode = reader.ReadNullableString();
            ReplacingNumber = reader.ReadNullableString();
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
                .WriteNullable(LostNumber)
                .WriteNullable(StorageCode)
                .WriteNullable(ReplacingNumber)
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
            Verifier<ReplacementInfo> verifier
                = new Verifier<ReplacementInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(LostNumber)
                    && !string.IsNullOrEmpty(ReplacingNumber)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Lost: {0}, Storage: {1}, Replacing: {2}",
                    LostNumber.ToVisibleString(),
                    StorageCode.ToVisibleString(),
                    ReplacingNumber.ToVisibleString()
                );
        }

        #endregion
    }
}
