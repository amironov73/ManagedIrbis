// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataSetInfo.cs -- information about dataset
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Data
{
    /// <summary>
    /// Information about <see cref="DataSet"/>.
    /// </summary>
    [PublicAPI]
    [XmlRoot("dataset")]
    [MoonSharpUserData]
    public class DataSetInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [CanBeNull]
        [XmlElement("connectionString")]
        [JsonProperty("connectionString", NullValueHandling = NullValueHandling.Ignore)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the dataset is read only.
        /// </summary>
        [XmlAttribute("readOnly")]
        [JsonProperty("readOnly", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the select command text.
        /// </summary>
        /// <value>The select command text.</value>
        [CanBeNull]
        [XmlElement("selectCommand")]
        [JsonProperty("selectCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectCommandText { get; set; }

        /// <summary>
        /// Gets the table list.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlElement("table")]
        [JsonProperty("tables")]
        public NonNullCollection<DataTableInfo> Tables
        {
            get; private set;
        }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataSetInfo()
        {
            Tables = new NonNullCollection<DataTableInfo>();
        }

        #endregion

        #region Public methods

#if !NETCORE

        /// <summary>
        /// Loads <see cref="DataSetInfo"/> from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static DataSetInfo Load
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            XmlSerializer serializer = new XmlSerializer(typeof(DataSetInfo));
            using (Stream stream = File.OpenRead(fileName))
            {
                return (DataSetInfo)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Saves this instance into the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            XmlSerializer serializer = new XmlSerializer(typeof(DataSetInfo));
            using (Stream stream = File.Create(fileName))
            {
                serializer.Serialize(stream, this);
            }
        }

#endif

        /// <summary>
        /// Should serialize the <see cref="ReadOnly"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReadOnly()
        {
            return ReadOnly;
        }

        /// <summary>
        /// Should serialize the <see cref="Tables"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTables()
        {
            return Tables.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            ConnectionString = reader.ReadNullableString();
            ReadOnly = reader.ReadBoolean();
            SelectCommandText = reader.ReadNullableString();
            Tables = reader.ReadNonNullCollection<DataTableInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(ConnectionString);
            writer.Write(ReadOnly);
            writer.WriteNullable(SelectCommandText);
            writer.WriteCollection(Tables);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<DataSetInfo> verifier
                = new Verifier<DataSetInfo>(this, throwOnError);

            foreach (DataTableInfo table in Tables)
            {
                verifier.VerifySubObject(table, "Table");
            }

            return verifier.Result;
        }

        #endregion
    }
}
