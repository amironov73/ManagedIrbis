// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SignatureInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Данные для подписи заказа на книги, поле 13 в БД CMPL.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Position} {Signature}")]
    [XmlRoot("signatureInfo")]
    public sealed class SignatureInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 13;

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "ab";

        #endregion

        #region Properties

        /// <summary>
        /// Должность, подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("position")]
        [JsonProperty("position")]
        [Description("Должность")]
        [DisplayName("Должность")]
        public string Position { get; set; }

        /// <summary>
        /// Подпись, подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("signature")]
        [JsonProperty("signature")]
        [Description("Подпись")]
        [DisplayName("Подпись")]
        public string Signature { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown")]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
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
                .ApplySubField('a', Position)
                .ApplySubField('b', Signature);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static SignatureInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            SignatureInfo result = new SignatureInfo
            {
                Position = field.GetFirstSubFieldValue('a'),
                Signature = field.GetFirstSubFieldValue('b'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes)
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SignatureInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<SignatureInfo> result = new List<SignatureInfo>();
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
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', Position)
                .AddNonEmptySubField('b', Signature)
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

            Position = reader.ReadNullableString();
            Signature = reader.ReadNullableString();
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
                .WriteNullable(Position)
                .WriteNullable(Signature)
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
            Verifier<SignatureInfo> verifier
                = new Verifier<SignatureInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Position, "Position")
                .NotNullNorEmpty(Signature, "Signature");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1}",
                    Position.ToVisibleString(),
                    Signature.ToVisibleString()
                );
        }

        #endregion
    }
}
