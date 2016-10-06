/* NodeLeader.cs -- leader in N01/L01
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
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
    /// Лидер записи в N01, L01
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Number={Number}, Previous={Previous}, Next={Next}, "
        + "TermCount={TermCount}, FreeOffset={FreeOffset}")]
#endif
    public sealed class NodeLeader
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
        /// Считывание из потока.
        /// </summary>
        [NotNull]
        public static NodeLeader Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            NodeLeader result = new NodeLeader
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
