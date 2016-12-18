// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SynonymEntry.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SynonymEntry
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Record MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Main word. Field 10.
        /// </summary>
        [CanBeNull]
        [Field("10")]
        [XmlAttribute("main")]
        [JsonProperty("main")]
        public string MainWord { get; set; }

        /// <summary>
        /// Synonyms. Field 11.
        /// </summary>
        [CanBeNull]
        [Field("11")]
        [XmlAttribute("synonyms")]
        [JsonProperty("synonyms")]
        public string[] Synonyms { get; set; }

        /// <summary>
        /// Language code. Field 12.
        /// </summary>
        [CanBeNull]
        [Field("12")]
        [XmlAttribute("language")]
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Worksheet code. Field 920.
        /// </summary>
        [CanBeNull]
        [Field("920")]
        [XmlAttribute("worksheet")]
        [JsonProperty("worksheet")]
        public string Worksheet { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static void _AddField
            (
                MarcRecord record,
                string tag,
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
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static SynonymEntry Parse
            (
                [NotNull] MarcRecord record
            )
        {

            Code.NotNull(record, "record");

            SynonymEntry result = new SynonymEntry
            {
                Mfn = record.Mfn,
                MainWord = record.FM("10"),
                Synonyms = record.FMA("11"),
                Language = record.FM("12"),
                Worksheet = record.FM("920")
            };

            return result;
        }

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

            _AddField(result, "10", MainWord);
            if (!ReferenceEquals(Synonyms, null))
            {
                foreach (string synonym in Synonyms)
                {
                    _AddField(result, "11", synonym);
                }
            }
            _AddField(result, "12", Language);
            _AddField(result, "902", Worksheet);

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Mfn = reader.ReadPackedInt32();
            MainWord = reader.ReadNullableString();
            Synonyms = reader.ReadNullableStringArray();
            Language = reader.ReadNullableString();
            Worksheet = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(MainWord)
                .WriteNullableArray(Synonyms)
                .WriteNullable(Language)
                .WriteNullable(Worksheet);
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}: ", MainWord);
            if (!ReferenceEquals(Synonyms, null))
            {
                result.Append(string.Join(", ", Synonyms));
            }

            return result.ToString();
        }

        #endregion
    }
}
