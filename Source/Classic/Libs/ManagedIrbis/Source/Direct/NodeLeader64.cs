// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeLeader64.cs -- record leader in N01/L01
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Record leader of L01/N01 file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Number={Number}, Previous={Previous}, Next={Next}, "
        + "TermCount={TermCount}, FreeOffset={FreeOffset}")]
    public sealed class NodeLeader64
    {
        #region Properties

        /// <summary>
        /// Номер записи (начиная с 1; в N01 номер первой записи
        /// равен номеру корневой записи дерева
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public int Number { get; set; }

        /// <summary>
        /// Номер предыдущей записи (-1, если нет)
        /// </summary>
        [XmlAttribute("previous")]
        [JsonProperty("previous")]
        public int Previous { get; set; }

        /// <summary>
        /// Номер следующей записи (-1, если нет)
        /// </summary>
        [XmlAttribute("next")]
        [JsonProperty("previous")]
        public int Next { get; set; }

        /// <summary>
        /// Число ключей в записи
        /// </summary>
        [XmlAttribute("term-count")]
        [JsonProperty("term-count")]
        public int TermCount { get; set; }

        /// <summary>
        /// Смещение на свободную позицию в записи
        /// (от начала записи)
        /// </summary>
        [XmlAttribute("free-offset")]
        [JsonProperty("free-offset")]
        public int FreeOffset { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Dump the leader.
        /// </summary>
        public void Dump
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteLine("NUMBER: {0}", Number);
            writer.WriteLine("PREV  : {0}", Previous);
            writer.WriteLine("NEXT  : {0}", Next);
            writer.WriteLine("TERMS : {0}", TermCount);
            writer.WriteLine("FREE  : {0}", FreeOffset);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        [NotNull]
        public static NodeLeader64 Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            NodeLeader64 result = new NodeLeader64
                {
                    Number = stream.ReadInt32Network(),
                    Previous = stream.ReadInt32Network(),
                    Next = stream.ReadInt32Network(),
                    TermCount = stream.ReadInt32Network(),
                    FreeOffset = stream.ReadInt32Network()
                };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Previous: {1}, "
                    + "Next: {2}, TermCount: {3}, "
                    + "FreeOffset: {4}",
                    Number,
                    Previous,
                    Next,
                    TermCount,
                    FreeOffset
                );
        }

        #endregion
    }
}
