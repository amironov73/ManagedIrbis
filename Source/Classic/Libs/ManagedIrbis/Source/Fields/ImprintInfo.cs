// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ImprintInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Выходные данные, поле 210.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ImprintInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "1acltxy";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "210";

        #endregion

        #region Properties

        /// <summary>
        /// Издательство (издающая организация), подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("publisher")]
        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        /// <summary>
        /// Издательство на издании, подполе l.
        /// </summary>
        [CanBeNull]
        [SubField('l')]
        [XmlAttribute("printedPublisher")]
        [JsonProperty("printedPublisher")]
        public string PrintedPublisher { get; set; }

        /// <summary>
        /// Город1, подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("city1")]
        [JsonProperty("city1")]
        public string City1 { get; set; }

        /// <summary>
        /// Город2, подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("city2")]
        [JsonProperty("city2")]
        public string City2 { get; set; }

        /// <summary>
        /// Город3, подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlAttribute("city3")]
        [JsonProperty("city3")]
        public string City3 { get; set; }

        /// <summary>
        /// Год издания, подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Место печати, подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("place")]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Типография, подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("printingHouse")]
        [JsonProperty("printingHouse")]
        public string PrintingHouse { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImprintInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImprintInfo
            (
                [CanBeNull] string publisher,
                [CanBeNull] string city1,
                [CanBeNull] string year
            )
        {
            Publisher = publisher;
            City1 = city1;
            Year = year;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static ImprintInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO: support for unknown subfields

            ImprintInfo result = new ImprintInfo
            {
                Publisher = field.GetFirstSubFieldValue('c'),
                PrintedPublisher = field.GetFirstSubFieldValue('l'),
                City1 = field.GetFirstSubFieldValue('a'),
                City2 = field.GetFirstSubFieldValue('x'),
                City3 = field.GetFirstSubFieldValue('y'),
                Year = field.GetFirstSubFieldValue('d'),
                Place = field.GetFirstSubFieldValue('1'),
                PrintingHouse = field.GetFirstSubFieldValue('t')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ImprintInfo[] Parse
            (
                [NotNull] MarcRecord record,
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ImprintInfo[] Parse
        (
            [NotNull] MarcRecord record
        )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Should serialize <see cref="Publisher"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePublisher()
        {
            return !ReferenceEquals(Publisher, null);
        }

        /// <summary>
        /// Should serialize <see cref="PrintedPublisher"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePrintedPublisher()
        {
            return !ReferenceEquals(PrintedPublisher, null);
        }

        /// <summary>
        /// Should serialize <see cref="City1"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity1()
        {
            return !ReferenceEquals(City1, null);
        }

        /// <summary>
        /// Should serialize <see cref="City2"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity2()
        {
            return !ReferenceEquals(City2, null);
        }

        /// <summary>
        /// Should serialize <see cref="City3"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity3()
        {
            return !ReferenceEquals(City3, null);
        }

        /// <summary>
        /// Should serialize <see cref="Year"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeYear()
        {
            return !ReferenceEquals(Year, null);
        }

        /// <summary>
        /// Should serialize <see cref="Place"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePlace()
        {
            return !ReferenceEquals(Place, null);
        }

        /// <summary>
        /// Should serialize <see cref="PrintingHouse"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePrintingHouse()
        {
            return !ReferenceEquals(PrintingHouse, null);
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('c', Publisher)
                .AddNonEmptySubField('l', PrintedPublisher)
                .AddNonEmptySubField('a', City1)
                .AddNonEmptySubField('x', City2)
                .AddNonEmptySubField('y', City3)
                .AddNonEmptySubField('d', Year)
                .AddNonEmptySubField('1', Place)
                .AddNonEmptySubField('t', PrintingHouse);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Publisher = reader.ReadNullableString();
            PrintedPublisher = reader.ReadNullableString();
            City1 = reader.ReadNullableString();
            City2 = reader.ReadNullableString();
            City3 = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            PrintingHouse = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Publisher)
                .WriteNullable(PrintedPublisher)
                .WriteNullable(City1)
                .WriteNullable(City2)
                .WriteNullable(City3)
                .WriteNullable(Year)
                .WriteNullable(Place)
                .WriteNullable(PrintingHouse);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
            (
                "City1: {0}, Publisher: {1}, Year: {2}",
                City1.ToVisibleString(),
                Publisher.ToVisibleString(),
                Year.ToVisibleString()
            );
        }

        #endregion
    }
}
