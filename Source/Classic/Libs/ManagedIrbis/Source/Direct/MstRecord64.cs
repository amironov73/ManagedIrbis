// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecord64.cs -- MST file record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AM;
using AM.IO;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// MST file record.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class MstRecord64
    {

        #region Properties

        /// <summary>
        /// MST file offset of the record.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Leader.
        /// </summary>
        public MstRecordLeader64 Leader { get; set; }

        /// <summary>
        /// Dictionary.
        /// </summary>
        public List<MstDictionaryEntry64> Dictionary { get; set; }

        /// <summary>
        /// Whether the record deleted.
        /// </summary>
        public bool Deleted
        {
            get
            {
                const int badStatus
                    = (int)(RecordStatus.LogicallyDeleted
                    | RecordStatus.PhysicallyDeleted);

                return (Leader.Status & badStatus) != 0;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MstRecord64()
        {
            Leader = new MstRecordLeader64();
            Dictionary = new List<MstDictionaryEntry64>();
        }

        #endregion

        #region Private members

        [NotNull]
        private string _DumpDictionary ()
        {
            StringBuilder result = new StringBuilder();

            foreach ( MstDictionaryEntry64 entry in Dictionary )
            {
                result.AppendLine ( entry.ToString () );
            }

            return result.ToString ();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Decode the field.
        /// </summary>
        [NotNull]
        public RecordField DecodeField
            (
                [NotNull] MstDictionaryEntry64 entry
            )
        {
            RecordField result = RecordFieldUtility.Parse
                (
                    entry.Tag.ToInvariantString(),
                    entry.Text
                );

            return result;
        }

        /// <summary>
        /// Decode the record.
        /// </summary>
        [NotNull]
        public MarcRecord DecodeRecord()
        {
            MarcRecord result = new MarcRecord
                {
                    Mfn = Leader.Mfn,
                    Status = (RecordStatus) Leader.Status,
                    PreviousOffset = Leader.Previous,
                    Version = Leader.Version
                };

            result.Fields.BeginUpdate();
            result.Fields.EnsureCapacity(Dictionary.Count);

            foreach (MstDictionaryEntry64 entry in Dictionary)
            {
                RecordField field = DecodeField(entry);
                result.Fields.Add(field);
            }

            result.Fields.EndUpdate();

            return result;
        }

        /// <summary>
        /// Encode the field.
        /// </summary>
        [NotNull]
        public static MstDictionaryEntry64 EncodeField
            (
                [NotNull] RecordField field
            )
        {
            MstDictionaryEntry64 result = new MstDictionaryEntry64
            {
                Tag = field.Tag,
                Text = field.ToText()
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static MstRecord64 EncodeRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MstRecord64 result = new MstRecord64
            {
                Leader =
                {
                    Mfn = record.Mfn,
                    Status = (int) record.Status,
                    Previous = record.PreviousOffset,
                    Version = record.Version
                }
            };

            if (result.Dictionary.Capacity < record.Fields.Count)
            {
                result.Dictionary.Capacity = record.Fields.Count;
            }
            foreach (RecordField field in record.Fields)
            {
                MstDictionaryEntry64 entry = EncodeField(field);
                result.Dictionary.Add(entry);
            }

            return result;
        }

        /// <summary>
        /// Prepare the record for serialization.
        /// </summary>
        public void Prepare()
        {
            Encoding encoding = IrbisEncoding.Utf8;
            Leader.Nvf = Dictionary.Count;
            int recordSize = MstRecordLeader64.LeaderSize
                + Dictionary.Count * MstDictionaryEntry64.EntrySize;
            Leader.Base = recordSize;
            int position = 0;
            foreach (MstDictionaryEntry64 entry in Dictionary)
            {
                entry.Position = position;
                entry.Bytes = encoding.GetBytes(entry.Text);
                int length = entry.Bytes.Length;
                entry.Length = length;
                recordSize += length;
                position += length;
            }

            if (recordSize % 2 != 0)
            {
                recordSize++;
            }
            Leader.Length = recordSize;
        }

        /// <summary>
        /// Write the record to specified stream.
        /// </summary>
        public void Write
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            Leader.Write(stream);
            foreach (MstDictionaryEntry64 entry in Dictionary)
            {
                stream.WriteInt32Network(entry.Tag);
                stream.WriteInt32Network(entry.Position);
                stream.WriteInt32Network(entry.Length);
            }
            foreach (MstDictionaryEntry64 entry in Dictionary)
            {
                stream.Write(entry.Bytes, 0, entry.Bytes.Length);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Leader: {0}{2}Dictionary: {1}", 
                    Leader,
                    _DumpDictionary (),
                    Environment.NewLine
                );
        }

        #endregion
    }
}
