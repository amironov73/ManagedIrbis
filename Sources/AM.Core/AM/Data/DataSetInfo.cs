/* DataSetInfo.cs -- information about dataset
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

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

        private List<DataTableInfo> _tables
            = new List<DataTableInfo>();

        /// <summary>
        /// Gets the table list.
        /// </summary>
        [NotNull]
        [XmlElement("table")]
        [JsonProperty("tables")]
        public List<DataTableInfo> Tables
        {
            [DebuggerStepThrough]
            get
            {
                return _tables;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads <see cref="DataSetInfo"/> from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static DataSetInfo Load
            (
                string fileName
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
        public void Save(string fileName)
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            XmlSerializer serializer = new XmlSerializer(typeof(DataSetInfo));
            using (Stream stream = File.Create(fileName))
            {
                serializer.Serialize(stream, this);
            }
        }

        #endregion
    }
}