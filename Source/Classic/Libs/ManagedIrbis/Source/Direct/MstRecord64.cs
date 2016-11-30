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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Leader={Leader}")]
#endif
    public sealed class MstRecord64
    {
        #region Constants

        #endregion

        #region Properties

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
                return (Leader.Status &
                      (int)(RecordStatus.LogicallyDeleted
                              | RecordStatus.PhysicallyDeleted)
                            )
                        != 0;
            }
        }

        #endregion

        #region Private members

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

            foreach (MstDictionaryEntry64 entry in Dictionary)
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
