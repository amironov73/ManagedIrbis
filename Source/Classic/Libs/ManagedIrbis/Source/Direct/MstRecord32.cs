// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecord32.cs -- MST file record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    public sealed class MstRecord32
    {
        #region Constants

        /// <summary>
        /// Block size of MST file.
        /// </summary>
        //public const int MstBlockSize = 2048;
        public const int MstBlockSize = 512;

        #endregion

        #region Properties

        /// <summary>
        /// Leader.
        /// </summary>
        public MstRecordLeader32 Leader { get; set; }

        /// <summary>
        /// Dictionary.
        /// </summary>
        public List<MstDictionaryEntry32> Dictionary { get; set; }

        /// <summary>
        /// Whether the record deleted.
        /// </summary>
        public bool Deleted
        {
            get {
                    return (Leader.Status &
                          (int)(RecordStatus.LogicallyDeleted
                                  | RecordStatus.PhysicallyDeleted)
                                )
                            != 0;
                }
        }

        #endregion

        #region Private members

        private string _DumpDictionary()
        {
            StringBuilder result = new StringBuilder();

            foreach (MstDictionaryEntry32 entry in Dictionary)
            {
                result.AppendLine(entry.ToString());
            }

            return result.ToString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Decode the field.
        /// </summary>
        [NotNull]
        public static RecordField DecodeField
            (
                [NotNull] MstDictionaryEntry32 entry
            )
        {
            string concatenated = string.Format
                (
                    "{0}#{1}",
                    entry.Tag,
                    entry.Text
                );

            RecordField result = RecordFieldUtility.Parse(concatenated);

            return result;
        }

        /// <summary>
        /// Decode record.
        /// </summary>
        [NotNull]
        public MarcRecord DecodeRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = Leader.Mfn,
                Status = (RecordStatus)Leader.Status,
                PreviousOffset = Leader.PreviousOffset,
            };

            foreach (MstDictionaryEntry32 entry in Dictionary)
            {
                RecordField field = DecodeField(entry);
                result.Fields.Add(field);
            }

            return result;
        }

        /// <summary>
        /// Encode the field.
        /// </summary>
        public static MstDictionaryEntry32 EncodeField
            (
                [NotNull] RecordField field
            )
        {
            MstDictionaryEntry32 result = new MstDictionaryEntry32
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
        public static MstRecord32 EncodeRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MstRecordLeader32 leader = new MstRecordLeader32
            {
                Mfn = record.Mfn,
                Status = (int) record.Status,
            };

            MstRecord32 result = new MstRecord32
            {
                Leader = leader
            };

            if (result.Dictionary.Capacity < record.Fields.Count)
            {
                result.Dictionary.Capacity = record.Fields.Count;
            }

            foreach (RecordField field in record.Fields)
            {
                MstDictionaryEntry32 entry = EncodeField(field);
                result.Dictionary.Add(entry);
            }

            return result;
        }


        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Leader: {0}{2}Dictionary: {1}",
                    Leader,
                    _DumpDictionary(),
                    Environment.NewLine
                );
        }

        #endregion
    }
}
