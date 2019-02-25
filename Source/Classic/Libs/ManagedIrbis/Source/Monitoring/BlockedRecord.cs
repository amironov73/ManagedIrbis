// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BlockedRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    /// Информация о заблокированной записи.
    /// </summary>
    [PublicAPI]
    [XmlRoot("blocked")]
    [MoonSharpUserData]
    public sealed class BlockedRecord
        : IHandmadeSerializable,
        IVerifiable,
        IEquatable<BlockedRecord>
    {
        #region Properties

        /// <summary>
        /// Database.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mfn { get; set; }

        /// <summary>
        /// Сколько раз запись обнаруживалась заблокированной
        /// системой мониторинга.
        /// </summary>
        [XmlAttribute("count")]
        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Count { get; set; }

        /// <summary>
        /// Since the moment.
        /// </summary>
        [XmlAttribute("since")]
        [JsonProperty("since", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Since { get; set; }

        /// <summary>
        /// Пометка.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Marked { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Merge the database info.
        /// </summary>
        [NotNull]
        public static List<BlockedRecord> Merge
            (
                [NotNull] List<BlockedRecord> list,
                [NotNull] DatabaseInfo[] databases
            )
        {
            Code.NotNull(list, "list");
            Code.NotNull(databases, "databases");

            foreach (BlockedRecord record in list)
            {
                record.Marked = false;
            }

            foreach (DatabaseInfo database in databases)
            {
                int[] lockedRecords = database.LockedRecords;
                if (ReferenceEquals(lockedRecords, null))
                {
                    continue;
                }

                foreach (int mfn in lockedRecords)
                {
                    BlockedRecord found = null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        BlockedRecord record = list[i];
                        if (record.Database == database.Name
                            && record.Mfn == mfn)
                        {
                            found = record;
                            found.Marked = true;
                            found.Count++;
                        }
                    }

                    if (ReferenceEquals(found, null))
                    {
                        BlockedRecord record = new BlockedRecord
                        {
                            Database = database.Name,
                            Mfn = mfn,
                            Count = 1,
                            Since = DateTime.Now,
                            Marked = true
                        };
                        list.Add(record);
                    }
                }
            }

            BlockedRecord[] records = list.Where(r => !r.Marked).ToArray();
            foreach (BlockedRecord blocked in records)
            {
                list.Remove(blocked);
            }

            return list;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Count = reader.ReadPackedInt32();
            long ticks = reader.ReadInt64();
            Since = new DateTime(ticks);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Database)
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Count)
                .Write(Since.Ticks);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BlockedRecord> verifier
                = new Verifier<BlockedRecord>(this, throwOnError);

            verifier
                .Assert(Mfn != 0, "Mfn != 0");

            return verifier.Result;
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                BlockedRecord other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Database == other.Database
                && Mfn == other.Mfn;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (ReferenceEquals(obj, this))
            {
                return true;
            }
            BlockedRecord record = obj as BlockedRecord;
            if (ReferenceEquals(record, null))
            {
                return false;
            }
            return Equals(record);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            int result = Mfn;
            if (!string.IsNullOrEmpty(Database))
            {
                result = result * 17 + Database.GetHashCode();
            }

            return result;
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Database.ToVisibleString()
                + ":"
                + Mfn.ToInvariantString()
                + ":"
                + Count.ToInvariantString();
        }

        #endregion
    }
}
