// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RevisionInfo.cs -- данные о редактировании записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Stage={Stage} Date={Date} Name={Name}")]
    public sealed class RevisionInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 907;

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("stage")]
        [JsonProperty("stage")]
        public string Stage { get; set; }

        /// <summary>
        /// Дата. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// ФИО оператора. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static RevisionInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull (field, "field");

            // TODO: support for unknown subfields

            RevisionInfo result = new RevisionInfo
                {
                    Date = field.GetFirstSubFieldValue('a'),
                    Name = field.GetFirstSubFieldValue('b'),
                    Stage = field.GetFirstSubFieldValue('c')
                };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RevisionInfo[] Parse
            (
                [NotNull] MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
                .ToArray();
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RevisionInfo[] Parse
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
        /// Should serialize <see cref="Date"/> field?
        /// </summary>
#if FW4
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeDate()
        {
            return !ReferenceEquals(Date, null);
        }

        /// <summary>
        /// Should serialize <see cref="Name"/> field?
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeName()
        {
            return !ReferenceEquals(Name, null);
        }

        /// <summary>
        /// Should serialize <see cref="Stage"/> field?
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeStage()
        {
            return !ReferenceEquals(Stage, null);
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Name)
                .AddNonEmptySubField('c', Stage);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Stage = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            Name = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Stage)
                .WriteNullable(Date)
                .WriteNullable(Name);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Stage: {0}, Date: {1}, Name: {2}",
                    Stage,
                    Date,
                    Name
                );
        }

        #endregion
    }
}
