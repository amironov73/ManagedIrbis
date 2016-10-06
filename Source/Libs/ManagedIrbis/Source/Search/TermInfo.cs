/* TermInfo.cs -- term info
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
    [XmlRoot("term-info")]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Count}] {Text}")]
#endif
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

        /// <summary>
        /// Restore object state from the specified stream.
        /// </summary>
        public virtual void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Count = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
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

        /// <summary>
        /// Verify object state.
        /// </summary>
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
                    "{0}#{1}",
                    Count,
                    Text
                );
        }

        #endregion
    }
}
