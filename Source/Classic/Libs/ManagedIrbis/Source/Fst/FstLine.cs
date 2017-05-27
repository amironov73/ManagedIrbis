// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FstLine.cs -- FST file line
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
using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// FST file line.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Tag} {Method} {Format}")]
    public sealed class FstLine
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Line number.
        /// </summary>
        [JsonIgnore]
        public int LineNumber { get; set; }

        /// <summary>
        /// Field tag.
        /// </summary>
        [CanBeNull]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Index method.
        /// </summary>
        [JsonProperty("method")]
        public FstIndexMethod Method { get; set; }

        /// <summary>
        /// Format itself.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse one line from the stream.
        /// </summary>
        [CanBeNull]
        public static FstLine ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string line;
            while (true)
            {
                line = reader.ReadLine();
                if (ReferenceEquals(line, null))
                {
                    return null;
                }
                line = line.Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    break;
                }
            }

            line = line.Replace('\x1A', ' ');
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            char[] delimiters = {' ', '\t'};

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

            string[] parts = line.Split
                (
                    delimiters,
                    3,
                    StringSplitOptions.RemoveEmptyEntries
                );

#else
            // TODO Implement it properly

            string[] parts = line.Split
                (
                    delimiters
                );

#endif

            if (parts.Length != 3)
            {
                Log.Error
                    (
                        "FstLine::ParseStream: "
                        + "bad line: "
                        + line.NullableToVisibleString()
                    );

                throw new FormatException
                    (
                        "Bad FST line: "
                        + line.NullableToVisibleString()
                    );
            }

            FstIndexMethod method = (FstIndexMethod) int.Parse
                (
                    parts[1]
                );
            FstLine result = new FstLine
            {
                Tag = parts[0],
                Method = method,
                Format = parts[2]
            };

            return result;
        }

        /// <summary>
        /// Convert line to the IRBIS format.
        /// </summary>
        [NotNull]
        public string ToFormat()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "mpl,'{0}',/,",
                    Tag
                );
            result.Append
                (
                    IrbisFormat.PrepareFormat(Format)
                );
            result.Append(",'\x07'");

            return result.ToString();
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Restore object state from specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            LineNumber = reader.ReadPackedInt32();
            Tag = reader.ReadNullableString();
            Method = (FstIndexMethod) reader.ReadPackedInt32();
            Format = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(LineNumber);
            writer.WriteNullable(Tag);
            writer.WritePackedInt32((int)Method);
            writer.WriteNullable(Format);
        }

        #endregion

        #region Object members

        #endregion
    }
}
