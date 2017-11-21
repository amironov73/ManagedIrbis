// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEntry.cs -- entry of the morphology database.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
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

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// Entry of the morphology database.
    /// </summary>
    [PublicAPI]
    [XmlRoot("word")]
    [MoonSharpUserData]
    public sealed class MorphologyEntry
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Record MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mfn { get; set; }

        /// <summary>
        /// Main term. Field 10.
        /// </summary>
        [CanBeNull]
        [Field("10")]
        [XmlAttribute("main")]
        [JsonProperty("main", NullValueHandling = NullValueHandling.Ignore)]
        public string MainTerm { get; set; }

        /// <summary>
        /// Dictionary term. Field 11.
        /// </summary>
        [CanBeNull]
        [Field("11")]
        [XmlAttribute("dictionary")]
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public string Dictionary { get; set; }

        /// <summary>
        /// Language name. Field 12.
        /// </summary>
        [CanBeNull]
        [Field("12")]
        [XmlAttribute("language")]
        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        /// <summary>
        /// Forms of the word. Repeatable field 20.
        /// </summary>
        [CanBeNull]
        [Field("20")]
        [XmlElement("form")]
        [JsonProperty("forms", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Forms { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                [NotNull] MarcRecord record,
                int tag,
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                record.Fields.Add(new RecordField(tag, text));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode the record.
        /// </summary>
        [NotNull]
        public MarcRecord Encode()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = Mfn
            };

            _AddField(result, 10, MainTerm);
            _AddField(result, 11, Dictionary);
            if (!ReferenceEquals(Forms, null))
            {
                foreach (string synonym in Forms)
                {
                    _AddField(result, 20, synonym);
                }
            }
            _AddField(result, 12, Language);

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static MorphologyEntry Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MorphologyEntry result = new MorphologyEntry
            {
                Mfn = record.Mfn,
                MainTerm = record.FM(10),
                Dictionary = record.FM(11),
                Language = record.FM(12),
                Forms = record.FMA(20)
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Mfn = reader.ReadPackedInt32();
            MainTerm = reader.ReadNullableString();
            Dictionary = reader.ReadNullableString();
            Language = reader.ReadNullableString();
            Forms = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(MainTerm)
                .WriteNullable(Dictionary)
                .WriteNullable(Language)
                .WriteNullableArray(Forms);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<MorphologyEntry> verifier
                = new Verifier<MorphologyEntry>(this, throwOnError);

            verifier
                .NotNullNorEmpty(MainTerm, "MainTerm")
                .NotNullNorEmpty(Dictionary, "Dictionary")
                .NotNullNorEmpty(Language, "Language")
                .NotNull(Forms, "Forms");

            if (!ReferenceEquals(Forms, null))
            {
                foreach (string form in Forms)
                {
                    verifier.NotNullNorEmpty(form, "form");
                }
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}: ", MainTerm.ToVisibleString());
            if (!ReferenceEquals(Forms, null)
                && Forms.Length != 0)
            {
                result.Append(string.Join(", ", Forms));
            }
            else
            {
                result.Append("(none)");
            }

            return result.ToString();
        }

        #endregion
    }
}
