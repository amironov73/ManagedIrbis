// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermInfo.cs -- term info
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
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
    /// Search term info
    /// </summary>
    [PublicAPI]
    [XmlRoot("term")]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Count}] {Text}")]
    public class TermInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        [XmlAttribute("count")]
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="TermInfo"/>.
        /// </summary>
        [NotNull]
        public TermInfo Clone()
        {
            return (TermInfo) MemberwiseClone();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static TermInfo[] Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<TermInfo> result = new List<TermInfo>();

            Regex regex = new Regex(@"^(\d+)\#(.+)$");
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
                    TermInfo item = new TermInfo
                        {
                            Count = int.Parse(match.Groups[1].Value),
                            Text = match.Groups[2].Value
                        };
                    result.Add(item);
                }

            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="Text"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeText()
        {
            return !string.IsNullOrEmpty(Text);
        }

        /// <summary>
        /// Should serialize the <see cref="Count"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeCount()
        {
            return Count != 0;
        }

        /// <summary>
        /// Trim prefix from terms.
        /// </summary>
        [NotNull]
        public static TermInfo[] TrimPrefix
            (
                [NotNull][ItemNotNull] TermInfo[] terms,
                [NotNull] string prefix
            )
        {
            Code.NotNull(terms, "terms");
            Code.NotNull(prefix, "prefix");

            int prefixLength = prefix.Length;
            List<TermInfo> result = new List<TermInfo>(terms.Length);
            if (prefixLength == 0)
            {
                foreach (TermInfo term in terms)
                {
                    result.Add(term.Clone());
                }
            }
            else
            {
                foreach (TermInfo term in terms)
                {
                    string item = term.Text;
                    if (!string.IsNullOrEmpty(item)
                        && item.StartsWith(prefix))
                    {
                        item = item.Substring(prefixLength);
                    }
                    TermInfo clone = term.Clone();
                    clone.Text = item;
                    result.Add(clone);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public virtual void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Count = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public virtual void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Count)
                .WriteNullable(Text);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<TermInfo> verifier
                = new Verifier<TermInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Text, "text")
                .Assert(Count >= 0, "Count");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}#{1}",
                    Count,
                    Text.ToVisibleString()
                );
        }

        #endregion
    }
}
