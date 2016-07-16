/* IrbisFileName.cs -- 
 * Ars Magna project, http://arsmagna.ru
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
using AM.Runtime;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Irbis file name.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Path={Path} Database={Database} FileName={FileName}")]
    public sealed class IrbisFileSpecification
        : IHandmadeSerializable,
        IVerifiable
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

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisFileSpecification()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisFileSpecification
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
        public IrbisFileSpecification
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
