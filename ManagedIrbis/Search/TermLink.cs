/* TermLink.cs -- term link
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */


#region Using directives

using System;
using System.IO;
using System.Xml.Serialization;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

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
    public sealed class TermLink
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
                    "Mfn: {0}, Tag: {1}, "
                    + "Occurrence: {2}, Index: {3}", 
                    Mfn, 
                    Tag, 
                    Occurrence, 
                    Index
                );
        }

        /// <summary>
        /// Compares this term link with another.
        /// </summary>
        private bool Equals
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
