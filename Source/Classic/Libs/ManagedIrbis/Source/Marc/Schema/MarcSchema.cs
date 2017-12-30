// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MarcSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("schema")]
    [DebuggerDisplay("Fields = {Fields.Count}")]
    public sealed class MarcSchema
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [JsonProperty("fields")]
        [XmlElement("field")]
        public NonNullCollection<FieldSchema> Fields
        {
            get { return _fields; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MarcSchema()
        {
            _fields = new NonNullCollection<FieldSchema>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<FieldSchema> _fields;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse given XML document.
        /// </summary>
        [NotNull]
        public static MarcSchema ParseDocument
            (
                [NotNull] XDocument document
            )
        {
            Code.NotNull(document, "document");

            MarcSchema result = new MarcSchema();

            var elements = document.Descendants("FIELD");
            foreach (XElement element in elements)
            {
                FieldSchema field = FieldSchema.ParseElement(element);
                result.Fields.Add(field);
            }

            return result;
        }

        /// <summary>
        /// Parse local XML file.
        /// </summary>
        [NotNull]
        public static MarcSchema ParseLocalXml
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            XDocument document = XDocument.Load(fileName);
            MarcSchema result = ParseDocument(document);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the given stream
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            reader.ReadCollection(Fields);
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteCollection(Fields);
        }

        #endregion


        #region Object members

        #endregion
    }
}

