// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermLink.cs -- term link
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Term link.
    /// </summary>
    [PublicAPI]
    [XmlRoot("term-link")]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Mfn}] {Tag}/{Occurrence} {Index}")]
#endif
    public sealed class TermLink
        : IHandmadeSerializable
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

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="TermLink"/>.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public TermLink Clone()
        {
            return (TermLink) MemberwiseClone();
        }

        /// <summary>
        /// Чтение ссылки из файла.
        /// </summary>
        [NotNull]
        public static TermLink Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            TermLink result = new TermLink
            {
                Mfn = stream.ReadInt32Network(),
                Tag = stream.ReadInt32Network(),
                Occurrence = stream.ReadInt32Network(),
                Index = stream.ReadInt32Network()
            };
            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Index);
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
                    "[{0}] {1}/{2} {3}",
                    Mfn, 
                    Tag, 
                    Occurrence, 
                    Index
                );
        }

        /// <summary>
        /// Compares this term link with another.
        /// </summary>
        public bool Equals
            (
                [NotNull] TermLink other
            )
        {
            Code.NotNull(other, "other");

            return (Mfn == other.Mfn) 
                && (Tag == other.Tag) 
                && (Occurrence == other.Occurrence) 
                && (Index == other.Index);
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="System.Object" />
        /// is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare
        /// with the current object.</param>
        /// <returns><c>true</c> if the specified
        /// <see cref="System.Object" /> is equal to
        /// this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return (obj is TermLink) 
                && Equals((TermLink) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance,
        /// suitable for use in hashing algorithms
        /// and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Mfn;
                hashCode = (hashCode*397) ^ Tag;
                hashCode = (hashCode*397) ^ Occurrence;
                hashCode = (hashCode*397) ^ Index;
                return hashCode;
            }
        }

        #endregion
    }
}
