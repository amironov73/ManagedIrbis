/* FileSpecification.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: good
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
    [DebuggerDisplay("Path={Path} Database={Database} " +
                     "FileName={FileName}")]
    public sealed class FileSpecification
        : IHandmadeSerializable,
        IVerifiable,
        IEquatable<FileSpecification>
    {
        #region Properties

        /// <summary>
        /// Is the file binary or text?
        /// </summary>
        public bool BinaryFile { get; set; }

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

        /// <summary>
        /// File contents (when we want write the file).
        /// </summary>
        [CanBeNull]
        public string Contents { get; set; }

        #endregion

        #region Construction

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
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            Path = path;
            FileName = fileName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSpecification
            (
                IrbisPath path,
                [CanBeNull] string database,
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            Path = path;
            Database = database;
            FileName = fileName;
        }

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
            BinaryFile = reader.ReadBoolean();
            Path = (IrbisPath)reader.ReadPackedInt32();
            Database = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
            Contents = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(BinaryFile);
            writer
                .WritePackedInt32((int)Path)
                .WriteNullable(Database)
                .WriteNullable(FileName)
                .WriteNullable(Contents);
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
            Verifier<FileSpecification> verifier
                = new Verifier<FileSpecification>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(FileName, "FileName");

            if (Path == IrbisPath.MasterFile
                || Path == IrbisPath.InvertedFile)
            {
                verifier.NotNullNorEmpty(Database, "Database");
            }

            return verifier.Result;
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
                && Equals((FileSpecification)obj);
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
                int hashCode = (int)Path;
                hashCode = (hashCode * 397)
                    ^ (Database != null ? Database.GetHashCode() : 0);
                hashCode = (hashCode * 397)
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
            string fileName = FileName;
            if (BinaryFile)
            {
                fileName = "@" + fileName;
            }
            else
            {
                if (!ReferenceEquals(Contents, null))
                {
                    fileName = "&" + fileName;
                }
            }

            string result;

            switch (Path)
            {
                case IrbisPath.System:
                case IrbisPath.Data:
                    result = string.Format
                        (
                            "{0}..{1}",
                            (int)Path,
                            fileName
                        );
                    break;
                default:
                    result = string.Format
                        (
                            "{0}.{1}.{2}",
                            (int)Path,
                            Database,
                            fileName
                        );
                    break;
            }

            if (!ReferenceEquals(Contents, null))
            {
                result = result
                    + "&"
                    + IrbisText.WindowsToIrbis(Contents);
            }

            return result;
        }

        #endregion
    }
}
