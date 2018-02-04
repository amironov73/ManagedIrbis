// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MappedAccess64.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !FW35 && !UAP && !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Direct database access with <see cref="MemoryMappedFile"/>
    /// Прямой доступ к БД с помощью MemoryMappedFile.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MappedAccess64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Master file.
        /// </summary>
        [NotNull]
        public MappedMstFile64 Mst { get; private set; }

        /// <summary>
        /// Cross-reference file.
        /// </summary>
        [NotNull]
        public MappedXrfFile64 Xrf { get; private set; }

        /// <summary>
        /// Inverted (index) file.
        /// </summary>
        [NotNull]
        public MappedInvertedFile64 InvertedFile { get; private set; }

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
        public MappedAccess64
            (
                [NotNull] string masterFile
            )
        {
            Code.NotNullNorEmpty(masterFile, "masterFile");

            Database = Path.GetFileNameWithoutExtension(masterFile);
            Mst = new MappedMstFile64(Path.ChangeExtension(masterFile, ".mst"));
            Xrf = new MappedXrfFile64(Path.ChangeExtension(masterFile, ".xrf"));
            InvertedFile = new MappedInvertedFile64(Path.ChangeExtension(masterFile, ".ifp"));
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
        /// Read raw record.
        /// </summary>
        [CanBeNull]
        public MstRecord64 ReadRawRecord
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

            MstRecord64 result = Mst.ReadRecord(xrfRecord.Offset);

            return result;
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
            if (xrfRecord.Offset == 0
                || (xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
            {
                return null;
            }

            MstRecord64 mstRecord = Mst.ReadRecord(xrfRecord.Offset);
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
                    MstRecord64 mstRecord = Mst.ReadRecord(offset);
                    MarcRecord previousVersion = mstRecord.DecodeRecord();
                    previousVersion.Database = lastVersion.Database;
                    previousVersion.Mfn = lastVersion.Mfn;
                    result.Add(previousVersion);
                    lastVersion = previousVersion;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Read links for the term.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public TermLink[] ReadLinks
            (
                [NotNull] string key
            )
        {
            Code.NotNull(key, "key");

            return InvertedFile.SearchExact(key);
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public TermInfo[] ReadTerms
            (
                [NotNull] TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            TermInfo[] result = InvertedFile.ReadTerms(parameters);

            return result;
        }

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
            List<MarcRecord> result = new List<MarcRecord>(mfns.Length);
            foreach (int mfn in mfns)
            {
                try
                {
                    XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        MstRecord64 mstRecord = Mst.ReadRecord(xrfRecord.Offset);
                        if (!mstRecord.Deleted)
                        {
                            MarcRecord irbisRecord = mstRecord.DecodeRecord();
                            irbisRecord.Database = Database;
                            result.Add(irbisRecord);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "MappedAccess64::SearchReadSimple",
                            exception
                        );
                }
            }
            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            InvertedFile.Dispose();
            Xrf.Dispose();
            Mst.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
