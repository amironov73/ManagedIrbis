// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FoundItem.cs --
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

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Found item.
    /// </summary>
    [PublicAPI]
    [XmlRoot("item")]
    [MoonSharpUserData]
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Delimiter.
        /// </summary>
        public const char Delimiter = '#';

        #endregion

        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("text")]
        [JsonProperty("text")]
        [Description("Библиографическое описание")]
        [DisplayName("Библиографическое описание")]
        public string Text { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        [Description("MFN")]
        [DisplayName("MFN")]
        public int Mfn { get; set; }

        /// <summary>
        /// Associated record.
        /// </summary>
        [CanBeNull]
        [XmlElement("record")]
        [JsonProperty("record")]
        [Browsable(false)]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Is selected?
        /// </summary>
        [XmlAttribute("selected")]
        [JsonProperty("selected")]
        [Description("Пометка")]
        [DisplayName("Пометка")]
        public bool Selected { get; set; }

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

        private static readonly char[] _delimiters = { Delimiter };

        #endregion

        #region Public methods

        /// <summary>
        /// Convert to MFN array.
        /// </summary>
        [NotNull]
        public static int[] ConvertToMfn
            (
                [NotNull][ItemNotNull] List<FoundItem> found
            )
        {
            Code.NotNull(found, "found");

            int[] result = new int[found.Count];
            for (int i = 0; i < found.Count; i++)
            {
                result[i] = found[i].Mfn;
            }

            return result;
        }

        /// <summary>
        /// Convert to string array.
        /// </summary>
        [NotNull]
        [ItemCanBeNull]
        public static string[] ConvertToText
            (
                [NotNull][ItemNotNull] List<FoundItem> found
            )
        {
            Code.NotNull(found, "found");

            string[] result = new string[found.Count];
            for (int i = 0; i < found.Count; i++)
            {
                result[i] = found[i].Text.EmptyToNull();
            }

            return result;
        }

        /// <summary>
        /// Parse text line.
        /// </summary>
        [NotNull]
        public static FoundItem ParseLine
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            string[] parts = StringUtility.SplitString(line, _delimiters, 2);
            FoundItem result = new FoundItem
            {
                Mfn = int.Parse(parts[0])
            };
            if (parts.Length > 1)
            {
                string text = parts[1].EmptyToNull();
                text = IrbisText.IrbisToWindows(text);
                result.Text = text;
            }

            return result;
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static List<FoundItem> ParseServerResponse
            (
                [NotNull] ServerResponse response,
                int sizeHint
            )
        {
            Code.NotNull(response, "response");

            List<FoundItem> result = sizeHint > 0
                ? new List<FoundItem>(sizeHint)
                : new List<FoundItem>();

            string line;
            while ((line = response.GetUtfString()) != null)
            {
                FoundItem item = ParseLine(line);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Text"/> field?
        /// </summary>
        public bool ShouldSerializeText()
        {
            return !string.IsNullOrEmpty(Text);
        }

        /// <summary>
        /// Should serialize the <see cref="Selected"/> field?
        /// </summary>
        public bool ShouldSerializeSelected()
        {
            return Selected;
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

            Mfn = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
            Record = reader.RestoreNullable<MarcRecord>();
            Selected = reader.ReadBoolean();
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
                .WriteNullable(Text)
                .WriteNullable(Record)
                .Write(Selected);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FoundItem> verifier = new Verifier<FoundItem>
                (
                    this,
                    throwOnError
                );

            verifier
                .Assert(Mfn > 0, "Mfn > 0")
                .NotNullNorEmpty(Text, "Text");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "[{0}] {1}",
                    Mfn,
                    Text.NullableToVisibleString()
                );
        }

        #endregion
    }
}
