/* FileSpecification.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Irbis file name.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Path={Path} Database={Database} FileName={FileName}")]
    public sealed class FileSpecification
        : IHandmadeSerializable,
        IVerifiable,
        IEquatable<FileSpecification>
    {
        #region Properties

        /// <summary>
        /// Path.
        /// </summary>
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        #endregion

        #region Private members

        private static bool _CompareDatabases
            (
                string first,
                string second
            )
        {
            if (string.IsNullOrEmpty(first)
                && string.IsNullOrEmpty(second))
            {
                return true;
            }

            return string.Compare
                (
                    first,
                    second,
                    StringComparison.OrdinalIgnoreCase
                ) == 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSpecification()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSpecification
            (
                IrbisPath path,
                string fileName
            )
        {
            Path = path;
            FileName = fileName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSpecification
            (
                IrbisPath path,
                string database,
                string fileName
            )
        {
            Path = path;
            Database = database;
            FileName = fileName;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object stat from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Path = (IrbisPath) reader.ReadPackedInt32();
            Database = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
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
                .WritePackedInt32((int) Path)
                .WriteNullable(Database)
                .WriteNullable(FileName);
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
            return true;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Compare with other <see cref="FileSpecification"/>.
        /// </summary>
        public bool Equals
            (
                [NotNull] FileSpecification other
            )
        {
            Code.NotNull(other, "other");

            return Path == other.Path
                   && _CompareDatabases(Database, other.Database)
                   && FileName.SameString(other.FileName);
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with
        /// the current object.</param>
        /// <returns><c>true</c> if the specified
        /// <see cref="System.Object" /> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is FileSpecification
                && Equals((FileSpecification) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance,
        /// suitable for use in hashing algorithms
        /// and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int) Path;
                hashCode = (hashCode*397)
                    ^ (Database != null ? Database.GetHashCode() : 0);
                hashCode = (hashCode*397)
                    ^ (FileName != null ? FileName.GetHashCode() : 0);

                return hashCode;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            string result = string.Format
                (
                    "{0}.{1}.{2}",
                    (int)Path,
                    Database,
                    FileName
                );

            return result;
        }

        #endregion
    }
}
