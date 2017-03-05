// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RevisionInfo.cs -- данные о редактировании записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

#if FW4

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#endif

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Stage={Stage} Date={Date} Name={Name}")]
#endif
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
        public const string Tag = "907";

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
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


            return record.Fields
                .GetField(tag)

#if !WINMOBILE && !PocketPC

                .Select(Parse)

#else

                .Select(field => Parse(field))

#endif

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
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public bool ShouldSerializeDate()
        {
            return !ReferenceEquals(Date, null);
        }

        /// <summary>
        /// Should serialize <see cref="Name"/> field?
        /// </summary>
#if FW4
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public bool ShouldSerializeName()
        {
            return !ReferenceEquals(Name, null);
        }

        /// <summary>
        /// Should serialize <see cref="Stage"/> field?
        /// </summary>
#if FW4
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
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

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Stage = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            Name = reader.ReadNullableString();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
