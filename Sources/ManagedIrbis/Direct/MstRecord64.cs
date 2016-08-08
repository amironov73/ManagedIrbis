/* MstRecord64.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Direct
{
    [DebuggerDisplay("Leader={Leader}")]
    public sealed class MstRecord64
    {
        #region Constants

        #endregion

        #region Properties

        public MstRecordLeader64 Leader { get; set; }

        public List<MstDictionaryEntry64> Dictionary { get; set; }

        public bool Deleted
        {
            get { return ((Leader.Status & (int)(RecordStatus.LogicallyDeleted | RecordStatus.PhysicallyDeleted)) != 0); }
        }

        #endregion

        #region Private members

        private string _DumpDictionary ( )
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

        public RecordField DecodeField(MstDictionaryEntry64 entry)
        {
            string catenated = string.Format
                (
                    "{0}#{1}",
                    entry.Tag,
                    entry.Text
                );

            RecordField result = RecordFieldUtility.Parse(catenated);

            return result;
        }

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

        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Leader: {0}\r\nDictionary: {1}", 
                    Leader,
                    _DumpDictionary ()
                );
        }

        #endregion
    }
}
