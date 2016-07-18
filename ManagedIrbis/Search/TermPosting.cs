/* TermPosting.cs -- term posting
 * Ars Magna project, http://arsmagna.ru
 */


#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Network;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Ссылка на терм.
    /// </summary>
    [PublicAPI]
    [XmlRoot("term-link")]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Mfn}] {Tag} {Occurrence} {Count} {Text}")]
    public sealed class TermPosting
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        [JsonProperty("mfn")]
        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        [JsonProperty("tag")]
        [XmlAttribute("tag")]
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [JsonProperty("occurrence")]
        [XmlAttribute("occurrence")]
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        [JsonProperty("count")]
        [XmlAttribute("count")]
        public int Count { get; set; }

        /// <summary>
        /// Текст постинга.
        /// </summary>
        [XmlAttribute("text")]
        [JsonProperty("text")]
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// Если было запрошено форматирование.
        /// </summary>
        [XmlAttribute("formatted")]
        [JsonProperty("formatted")]
        [CanBeNull]
        public string Formatted { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static TermPosting[] Parse
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<TermPosting> result = new List<TermPosting>();
            Regex regex = new Regex(@"^(\d+)\#(\w+)\#(\d+)\#(\d+)\#(\d*)$");

            while (true)
            {
                string line = response.GetUtfString();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                Match match = regex.Match(line);
                if (match.Success)
                {
                    TermPosting item = new TermPosting
                    {
                        Mfn = int.Parse(match.Groups[1].Value),
                        Tag = int.Parse(match.Groups[2].Value),
                        Occurrence = int.Parse(match.Groups[3].Value),
                        Count = int.Parse(match.Groups[4].Value),
                    };
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Count = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
            Formatted = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Count)
                .WriteNullable(Text)
                .WriteNullable(Formatted);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<TermPosting> verifier
                = new Verifier<TermPosting>(this, throwOnError);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "[{0}] {1} {2} {3} \"{4}\" \"{5}\"",
                    Mfn,
                    Tag,
                    Occurrence,
                    Count,
                    Text,
                    Formatted
                );
        }

        #endregion
    }
}
