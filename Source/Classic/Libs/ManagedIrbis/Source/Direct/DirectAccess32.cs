// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DirectReader32.cs -- direct reading IRBIS32 databases
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Direct reading IRBIS32 databases.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DirectAccess32
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        [NotNull]
        public MstFile32 Mst { get; private set; }

        /// <summary>
        /// Cross-reference file.
        /// </summary>
        [NotNull]
        public XrfFile32 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        [NotNull]
        public InvertedFile32 InvertedFile { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DirectAccess32
            (
                [NotNull] string masterFile
            )
        {
            Code.NotNullNorEmpty(masterFile, "masterFile");

            Database = Path.GetFileNameWithoutExtension
                (
                    masterFile
                );
            Mst = new MstFile32
                (
                    Path.ChangeExtension
                        (
                            masterFile,
                            ".mst"
                        )
                );
            Xrf = new XrfFile32
                (
                    Path.ChangeExtension
                    (
                        masterFile,
                        ".xrf"
                    )
                );
            InvertedFile = new InvertedFile32
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
        /// Get maximal MFN for the database.
        /// </summary>
        /// <returns></returns>
        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

        /// <summary>
        /// Read given record.
        /// </summary>
        /// <returns><c>null</c> if no such record
        /// or record physically deleted.</returns>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            if (mfn > GetMaxMfn())
            {
                return null;
            }

            XrfRecord32 xrfRecord = Xrf.ReadRecord(mfn);
            if ((xrfRecord.Status & RecordStatus.PhysicallyDeleted)
                != 0)
            {
                return null;
            }

            MstRecord32 mstRecord = Mst.ReadRecord2
                (
                    xrfRecord.AbsoluteOffset
                );
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
                    MstRecord32 mstRecord = Mst.ReadRecord2(offset);
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
        /// Simple search by the key.
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
        /// Simple search and read records by the key.
        /// </summary>
        public MarcRecord[] SearchReadSimple
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            int[] found = InvertedFile.SearchSimple(key);
            List<MarcRecord> result = new List<MarcRecord>();

            foreach (int mfn in found)
            {
                try
                {
                    XrfRecord32 xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        MstRecord32 mstRecord = Mst.ReadRecord2(xrfRecord.AbsoluteOffset);
                        if (!mstRecord.Deleted)
                        {
                            MarcRecord marcRecord
                                = mstRecord.DecodeRecord();
                            marcRecord.Database = Database;
                            result.Add(marcRecord);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                    (
                        "DirectReader32::SearchReadSimple",
                        exception
                    );
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Mst.Dispose();
            Xrf.Dispose();
            InvertedFile.Dispose();
        }

        #endregion
    }
}

