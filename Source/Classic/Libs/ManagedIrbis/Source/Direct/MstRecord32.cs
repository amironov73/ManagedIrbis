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

            RecordField result
                = RecordFieldUtility.Parse(concatenated);

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
                //PreviousOffset = Leader.Previous,
                //Version = Leader.Version
            };

            foreach (MstDictionaryEntry32 entry in Dictionary)
            {
                RecordField field = DecodeField(entry);
                result.Fields.Add(field);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
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
