/* FieldSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class FieldSchema
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Display.
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Examples.
        /// </summary>
        [NotNull]
        public NonNullCollection<Example> Examples
        {
            get { return _examples; }
        }

            /// <summary>
        /// First indicator.
        /// </summary>
        [NotNull]
        public IndicatorSchema Indicator1
        {
            get { return _indicator1; }
        }

        /// <summary>
        /// Second indicator.
        /// </summary>
        [NotNull]
        public IndicatorSchema Indicator2
        {
            get { return _indicator2; }
        }

        /// <summary>
        /// Mandatory?
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MandatoryText { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Related fields
        /// </summary>
        public NonNullCollection<RelatedField> RelatedFields
        {
            get { return _relatedFields; }
        }

        /// <summary>
        /// Repeatable?
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RepeatableText { get; set; }

        /// <summary>
        /// Subfields.
        /// </summary>
        [NotNull]
        public NonNullCollection<SubFieldSchema> SubFields
        {
            get { return _subFields; }
        }

        /// <summary>
        /// Tag.
        /// </summary>
        public string Tag { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldSchema()
        {
            _examples = new NonNullCollection<Example>();
            _indicator1 = new IndicatorSchema();
            _indicator2 = new IndicatorSchema();
            _relatedFields = new NonNullCollection<RelatedField>();
            _subFields = new NonNullCollection<SubFieldSchema>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<Example> _examples;

        private IndicatorSchema _indicator1, _indicator2;

        private readonly NonNullCollection<RelatedField> _relatedFields;

        private readonly NonNullCollection<SubFieldSchema> _subFields;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse given XML element.
        /// </summary>
        [NotNull]
        public static FieldSchema ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            FieldSchema result = new FieldSchema
            {
                Tag = element.GetAttributeText("tag"),
                Name = element.GetAttributeText("name"),
                Mandatory = element.GetAttributeBoolean("mandatory"),
                MandatoryText = element.GetAttributeText("nm", null),
                Repeatable = element.GetAttributeBoolean("repeatable"),
                RepeatableText = element.GetAttributeText("nr", null),
                Description = element.GetInnerXml("DESCRIPTION"),
                Display = element.GetAttributeBoolean("display", false)
            };

            foreach (XElement subElement in element.Elements("SUBFIELD"))
            {
                SubFieldSchema subField
                    = SubFieldSchema.ParseElement(subElement);
                result.SubFields.Add(subField);
            }

            foreach (XElement subElement in element.Elements("RELATED"))
            {
                RelatedField relatedField
                    = RelatedField.ParseElement(subElement);
                result.RelatedFields.Add(relatedField);
            }

            XElement examples = element.Element("EXAMPLES");
            if (!ReferenceEquals(examples, null))
            {
                foreach (XElement subElement in examples.Elements("EX"))
                {
                    Example example = Example.ParseElement(subElement);
                    result.Examples.Add(example);
                }
            }

            XElement indicator1 = element.Element("IND1");
            if (!ReferenceEquals(indicator1, null))
            {
                result._indicator1 = IndicatorSchema.ParseElement(indicator1);
            }

            XElement indicator2 = element.Element("IND2");
            if (!ReferenceEquals(indicator2, null))
            {
                result._indicator2 = IndicatorSchema.ParseElement(indicator2);
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
