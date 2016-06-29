/* MarcSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Xml.Linq;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class MarcSchema
    {
        #region Properties

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
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

        #region Object members

        #endregion
    }
}
