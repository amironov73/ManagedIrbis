// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataSetInfo.cs -- information about dataset
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Data;
using System.IO;
using System.Xml.Serialization;

using AM.Collections;

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
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [CanBeNull]
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the dataset is read only.
        /// </summary>
        [XmlAttribute("readOnly")]
        [JsonProperty("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the select command text.
        /// </summary>
        /// <value>The select command text.</value>
        [CanBeNull]
        [XmlElement("selectCommand")]
        [JsonProperty("selectCommand")]
        public string SelectCommandText { get; set; }

        /// <summary>
        /// Gets the table list.
        /// </summary>
        [NotNull]
        [XmlElement("table")]
        [JsonProperty("tables")]
        public NonNullCollection<DataTableInfo> Tables
        {
            get; private set;
        }

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

#endregion
    }
}
