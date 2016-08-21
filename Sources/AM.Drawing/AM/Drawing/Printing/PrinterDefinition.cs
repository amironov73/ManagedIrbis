/* PrinterDefinition.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.IO;
using AM.Json;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace AM.Drawing.Printing
{
    /// <summary>
    /// Printer definition for saving/restoring.
    /// </summary>
    [PublicAPI]
    [XmlRoot("printer")]
    public sealed class PrinterDefinition
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Как Windows называет принтер.
        /// </summary>
        [CanBeNull]
        [XmlElement("name")]
        [JsonProperty("name")]
        [DisplayName("Принтер")]
        public string Name { get; set; }

        /// <summary>
        /// Ширина страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("width")]
        [JsonProperty("width")]
        [DisplayName("Ширина страницы")]
        public int PageWidth { get; set; }

        /// <summary>
        /// Высота страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("height")]
        [JsonProperty("height")]
        [DisplayName("Высота страницы")]
        public int PageHeight { get; set; }

        /// <summary>
        /// Landscape or portrait?
        /// </summary>
        [XmlElement("landscape")]
        [JsonProperty("landscape")]
        [DisplayName("Портретная ориентация")]
        public bool Landscape { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get Fargo Card printer.
        /// </summary>
        [NotNull]
        public static PrinterDefinition GetFargoPrinter()
        {
            PrinterDefinition result = new PrinterDefinition
            {
                Name = "HDP5000 Card Printer",
                PageWidth = 220,
                PageHeight = 345,
                Landscape = true
            };

            return result;
        }

        /// <summary>
        /// Load <see cref="PrinterDefinition"/>
        /// from the JSON-file.
        /// </summary>
        [NotNull]
        public static PrinterDefinition LoadJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            PrinterDefinition result
                = JsonUtility.ReadObjectFromFile<PrinterDefinition>
                (
                    fileName
                );

            return result;
        }

        /// <summary>
        /// Load <see cref="PrinterDefinition"/>
        /// from the XML file.
        /// </summary>
        [NotNull]
        public static PrinterDefinition LoadXml
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (Stream stream = File.OpenRead(fileName))
            {
                XmlSerializer serializer
                    = new XmlSerializer(typeof(PrinterDefinition));

                return (PrinterDefinition)serializer
                    .Deserialize(stream);
            }
        }

        /// <summary>
        /// Save the <see cref="PrinterDefinition"/>
        /// to the JSON file.
        /// </summary>
        public void SaveJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            JsonUtility.SaveObjectToFile
                (
                    this,
                    fileName
                );
        }

        /// <summary>
        /// Save the <see cref="PrinterDefinition"/>
        /// to the XML file.
        /// </summary>
        public void SaveXml
            (
                string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (Stream stream = File.Create(fileName))
            {
                XmlSerializer serializer
                    = new XmlSerializer(typeof(PrinterDefinition));
                serializer.Serialize(stream, this);
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            PageWidth = reader.ReadPackedInt32();
            PageHeight = reader.ReadPackedInt32();
            Landscape = reader.ReadBoolean();
        }

        /// <summary>
        /// Save the object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WritePackedInt32(PageWidth)
                .WritePackedInt32(PageHeight)
                .Write(Landscape);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<PrinterDefinition> verifier
                = new Verifier<PrinterDefinition>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .Assert(PageWidth > 0, "PageWidth")
                .Assert(PageHeight > 0, "PageHeight");

            return verifier.Result;
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
                    "{0}: {1} x {2}",
                    Name,
                    PageWidth,
                    PageHeight
                );
        }

        #endregion
    }
}
