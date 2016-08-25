/* DirectReader64.cs -- direct reading IRBIS64 databases
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Direct reading IRBIS64 databases.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DirectReader64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        [NotNull]
        public MstFile64 Mst { get; private set; }

        /// <summary>
        /// Cross-references file.
        /// </summary>
        [NotNull]
        public XrfFile64 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        [NotNull]
        public InvertedFile64 InvertedFile { get; private set; }

        /// <summary>
        /// Database path.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DirectReader64
            (
                [NotNull] string masterFile,
                bool inMemory
            )
        {
            Code.NotNullNorEmpty(masterFile, "masterFile");

            Database = Path.GetFileNameWithoutExtension(masterFile);
            Mst = new MstFile64
                (
                    Path.ChangeExtension
                        (
                            masterFile,
                            ".mst"
                        )
                );
            Xrf = new XrfFile64
                (
                    Path.ChangeExtension
                    (
                        masterFile,
                        ".xrf"
                    ),
                    inMemory
                );
            InvertedFile = new InvertedFile64
                (
                    Path.ChangeExtension
                    (
                        masterFile,
                        ".ifp"
                    )
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get max MFN for database. Not next MFN!
        /// </summary>
        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

        /// <summary>
        /// Read record with given MFN.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
            if (xrfRecord.Offset == 0)
            {
                return null;
            }
            MstRecord64 mstRecord = Mst.ReadRecord2(xrfRecord.Offset);
            MarcRecord result = mstRecord.DecodeRecord();
            result.Database = Database;

            return result;
        }

        /// <summary>
        /// Read all versions of the record.
        /// </summary>
        [NotNull]
        public MarcRecord[] ReadAllRecordVersions
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            List<MarcRecord> result = new List<MarcRecord>();
            MarcRecord lastVersion = ReadRecord(mfn);
            if (lastVersion != null)
            {
                result.Add(lastVersion);
                while (true)
                {
                    long offset = lastVersion.PreviousOffset;
                    if (offset == 0)
                    {
                        break;
                    }
                    MstRecord64 mstRecord = Mst.ReadRecord2(offset);
                    MarcRecord previousVersion = mstRecord.DecodeRecord();
                    if (previousVersion != null)
                    {
                        result.Add(previousVersion);
                        lastVersion = previousVersion;
                    }
                }
            }

            return result.ToArray();
        }

        //public IrbisRecord ReadRecord2
        //    (
        //        int mfn
        //    )
        //{
        //    XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
        //    MstRecord32 mstRecord = Mst.ReadRecord2(xrfRecord.Offset);
        //    IrbisRecord result = mstRecord.DecodeRecord();
        //    result.Database = Database;
        //    return result;
        //}

        /// <summary>
        /// Simple search.
        /// </summary>
        [NotNull]
        public int[] SearchSimple
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            int[] found = InvertedFile.SearchSimple(key);
            List<int> result = new List<int>();
            foreach (int mfn in found)
            {
                if (!Xrf.ReadRecord(mfn).Deleted)
                {
                    result.Add(mfn);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Simple search and read records.
        /// </summary>
        [NotNull]
        public MarcRecord[] SearchReadSimple
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            int[] mfns = InvertedFile.SearchSimple(key);
            List<MarcRecord> result = new List<MarcRecord>();
            foreach (int mfn in mfns)
            {
                try
                {
                    XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        MstRecord64 mstRecord = Mst.ReadRecord2(xrfRecord.Offset);
                        if (!mstRecord.Deleted)
                        {
                            MarcRecord irbisRecord
                                = mstRecord.DecodeRecord();
                            irbisRecord.Database = Database;
                            result.Add(irbisRecord);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {

            Mst.Dispose();
            Xrf.Dispose();
            InvertedFile.Dispose();
        }

        #endregion
    }
}
