// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogDelta.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("catalogDelta")]
    public sealed class CatalogDelta
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// First date.
        /// </summary>
        [XmlAttribute("firstDate")]
        [JsonProperty("firstDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime FirstDate { get; set; }

        /// <summary>
        /// Second date.
        /// </summary>
        [XmlAttribute("secondDate")]
        [JsonProperty("secondDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime SecondDate { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database { get; set; }

        /// <summary>
        /// New records.
        /// </summary>
        [CanBeNull]
        [JsonProperty("new")]
        [XmlArray("new")]
        [XmlArrayItem("mfn")]
        public int[] NewRecords { get; set; }

        /// <summary>
        /// Deleted records.
        /// </summary>
        [CanBeNull]
        [XmlArray("deleted")]
        [XmlArrayItem("mfn")]
        [JsonProperty("deleted", NullValueHandling = NullValueHandling.Ignore)]
        public int[] DeletedRecords { get; set; }

        /// <summary>
        /// Altered records.
        /// </summary>
        [CanBeNull]
        [XmlArray("altered")]
        [XmlArrayItem("mfn")]
        [JsonProperty("altered", NullValueHandling = NullValueHandling.Ignore)]
        public int[] AlteredRecords { get; set; }

        #endregion

        #region Private members

        private static void _AppendRecords
            (
                [NotNull] StringBuilder builder,
                [CanBeNull] int[] records,
                [NotNull] string name
            )
        {
            if (!ReferenceEquals(records, null)
                && records.Length != 0)
            {
                builder.AppendLine
                    (
                        string.Format
                        (
                            "{0}: {1}",
                            name,
                            StringUtility.Join
                                (
                                    ", ",
                                    records
                                )
                        )
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create delta for two catalog states.
        /// </summary>
        [NotNull]
        public static CatalogDelta Create
            (
                [NotNull] CatalogState first,
                [NotNull] CatalogState second
            )
        {
            Code.NotNull(first, "first");
            Code.NotNull(second, "second");

            RecordState[] firstRecords = first.Records
                .ThrowIfNull("first.Records");
            RecordState[] secondRecords = second.Records
                .ThrowIfNull("second.Records");

            int[] firstDeleted = first.LogicallyDeleted
                .ThrowIfNull("first.LogicallyDeleted");
            int[] secondDeleted = second.LogicallyDeleted
                .ThrowIfNull("second.LogicallyDeleted");

            // TODO compare first.Database with second.Database?

            CatalogDelta result = new CatalogDelta
            {
                FirstDate = first.Date,
                SecondDate = second.Date,
                Database = first.Database,
                NewRecords = secondRecords.Except
                    (
                        firstRecords,
                        new RecordStateComparer.ByMfn()
                    )
                    .Select(state => state.Mfn)
                    .Where(mfn => mfn != 0)
                    .ToArray()
            };


            result.AlteredRecords = secondRecords.Except
                (
                    firstRecords,
                    new RecordStateComparer.ByVersion()
                )
                .Select(state => state.Mfn)
                .Where(mfn => mfn != 0)
                .Except(result.NewRecords.ThrowIfNull("result.NewRecords"))
                .Except(secondDeleted)
                .ToArray();

            result.DeletedRecords 
                = secondDeleted.Except(firstDeleted)
                .Where(mfn => mfn != 0)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="FirstDate"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeFirstDate()
        {
            return FirstDate != DateTime.MinValue;
        }

        /// <summary>
        /// Should serialize the <see cref="SecondDate"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeSecondDate()
        {
            return SecondDate != DateTime.MinValue;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Id = reader.ReadPackedInt32();
            FirstDate = reader.ReadDateTime();
            SecondDate = reader.ReadDateTime();
            Database = reader.ReadNullableString();
            NewRecords = reader.ReadNullableInt32Array();
            DeletedRecords = reader.ReadNullableInt32Array();
            AlteredRecords = reader.ReadNullableInt32Array();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Id)
                .Write(FirstDate)
                .Write(SecondDate)
                .WriteNullable(Database)
                .WriteNullableArray(NewRecords)
                .WriteNullableArray(DeletedRecords)
                .WriteNullableArray(AlteredRecords);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            _AppendRecords(result, NewRecords, "New");
            _AppendRecords(result, DeletedRecords, "Deleted");
            _AppendRecords(result, AlteredRecords, "Altered");

            return result.ToString();
        }

        #endregion
    }
}
