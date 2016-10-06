/* TermInfoEx.cs -- extended term info
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Extended search term info.
    /// </summary>
    [PublicAPI]
    [XmlRoot("term-info")]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Count}] {Text} {Formatted}")]
#endif
    public sealed class TermInfoEx
        : TermInfo
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
        /// Смещение от начала поля.
        /// </summary>
        [JsonProperty("index")]
        [XmlAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// Расформатированная запись
        /// </summary>
        [JsonProperty("formatted")]
        [XmlAttribute("formatted")]
        [CanBeNull]
        public string Formatted { get; set; }

        #endregion

        #region Private members

        private static char[] _separators = { '\x1E' };

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static TermInfoEx[] ParseEx
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<TermInfoEx> result = new List<TermInfoEx>();

            Regex regex = new Regex(@"^(\d+)\#(\d+)#(\d+)#(\d+)#(\d+)$");
            while (true)
            {
                string line = response.GetUtfString();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

#if !WINMOBILE && !PocketPC

                string[] parts = line.Split(_separators, 3);

#else
                // TODO Implement it properly

                string[] parts = line.Split(_separators);

#endif

                if (parts.Length != 3)
                {
                    throw new IrbisNetworkException();
                }

                Match match = regex.Match(parts[0]);
                if (!match.Success)
                {
                    throw new IrbisNetworkException();
                }
                TermInfoEx item = new TermInfoEx
                    {
                        Count = int.Parse(match.Groups[1].Value),
                        Mfn = int.Parse(match.Groups[2].Value),
                        Tag = int.Parse(match.Groups[3].Value),
                        Occurrence = int.Parse(match.Groups[4].Value),
                        Index = int.Parse(match.Groups[5].Value),
                        Text = parts[1],
                        Formatted = parts[2]
                    };
                result.Add(item);
            }


            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the specified stream.
        /// </summary>
        public override void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            base.RestoreFromStream(reader);
            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
            Formatted = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public override void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            base.SaveToStream(writer);

            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Index)
                .WriteNullable(Formatted);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<TermInfo> verifier
                = new Verifier<TermInfo>(this, throwOnError);

            verifier.Assert
                (
                    base.Verify(throwOnError),
                    "base.Verify"
                )
                .Assert(Mfn > 0, "MFN > 0")
                .Assert(Tag != 0, "Tag != 0")
                .Assert(Occurrence > 0, "Occurrence > 0");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
