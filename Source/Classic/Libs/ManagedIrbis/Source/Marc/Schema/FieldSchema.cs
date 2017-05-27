// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT

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

#endregion

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("field")]
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class FieldSchema
        : IHandmadeSerializable
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
        [ItemNotNull]
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
        [NotNull]
        [ItemNotNull]
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
        [ItemNotNull]
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

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the given stream
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Description = reader.ReadNullableString();
            Display = reader.ReadBoolean();
            reader.ReadCollection(Examples);
            Indicator1.RestoreFromStream(reader);
            Indicator2.RestoreFromStream(reader);
            Mandatory = reader.ReadBoolean();
            MandatoryText = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            reader.ReadCollection(RelatedFields);
            Repeatable = reader.ReadBoolean();
            RepeatableText = reader.ReadNullableString();
            reader.ReadCollection(SubFields);
            Tag = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Description);
            writer.Write(Display);
            writer.WriteCollection(Examples);
            Indicator1.SaveToStream(writer);
            Indicator2.SaveToStream(writer);
            writer.Write(Mandatory);
            writer.WriteNullable(MandatoryText);
            writer.WriteNullable(Name);
            writer.WriteCollection(RelatedFields);
            writer.Write(Repeatable);
            writer.WriteNullable(RepeatableText);
            writer.WriteCollection(SubFields);
            writer.WriteNullable(Tag);
        }

        #endregion


        #region Object members

        #endregion
    }
}

#endif

