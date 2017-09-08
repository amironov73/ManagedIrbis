// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEntry.cs -- entry of the morphology database.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

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
        /// Main term. Field 10.
        /// </summary>
        [CanBeNull]
        [JsonProperty("main")]
        [XmlAttribute("main")]
        public string MainTerm { get; set; }

        /// <summary>
        /// Dictionary term. Field 11.
        /// </summary>
        [CanBeNull]
        [JsonProperty("dictionary")]
        [XmlAttribute("dictionary")]
        public string Dictionary { get; set; }

        /// <summary>
        /// Language name. Field 12.
        /// </summary>
        [CanBeNull]
        [JsonProperty("language")]
        [XmlAttribute("language")]
        public string Language { get; set; }

        /// <summary>
        /// Forms of the word. Repeatable field 20.
        /// </summary>
        [CanBeNull]
        [JsonProperty("forms")]
        [XmlElement("form")]
        public string[] Forms { get; set; }

        #endregion

        #region Public methods

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
                MainTerm = record.FM(10),
                Dictionary = record.FM(11),
                Language = record.FM(12),
                Forms = record.FMA(20)
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            MainTerm = reader.ReadNullableString();
            Dictionary = reader.ReadNullableString();
            Language = reader.ReadNullableString();
            Forms = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(MainTerm)
                .WriteNullable(Dictionary)
                .WriteNullable(Language)
                .WriteNullableArray(Forms);
        }

        #endregion


        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
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

            foreach (string form in Forms.ThrowIfNull())
            {
                verifier.NotNullNorEmpty(form, "form");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return MainTerm.ToVisibleString();
        }

        #endregion
    }
}
