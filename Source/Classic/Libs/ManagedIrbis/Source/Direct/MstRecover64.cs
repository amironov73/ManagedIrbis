// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecover64.cs -- MST file recover
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// MST file recover.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class MstRecover64
    {
        #region Public methods

        /// <summary>
        /// Build ISO 2709 file from the found records.
        /// </summary>
        public static void BuildIso
            (
                [NotNull] FoundRecord[] foundRecords,
                [NotNull] string sourcePath,
                [NotNull] string destinationPath,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(foundRecords, "foundRecords");
            Code.FileExists(sourcePath, "sourcePath");
            Code.NotNullNorEmpty(destinationPath, "destinationPath");
            Code.NotNull(encoding, "encoding");

            FileUtility.DeleteIfExists(destinationPath);
            using (Stream output = File.Create(destinationPath))
            using (MstFile64 mstFile = new MstFile64(sourcePath, DirectAccessMode.ReadOnly))
            {
                foreach (FoundRecord foundRecord in foundRecords)
                {
                    MstRecord64 mstRecord = mstFile.ReadRecord(foundRecord.Position);
                    MarcRecord marcRecord = mstRecord.DecodeRecord();
                    Iso2709.WriteIso(marcRecord, output, encoding);
                }
            }
        }

        /// <summary>
        /// Rebuild MST file from the found records.
        /// </summary>
        public static void BuildMst
            (
                [NotNull] FoundRecord[] records,
                [NotNull] string sourcePath,
                [NotNull] string destinationPath
            )
        {
            Code.NotNull(records, "records");
            Code.FileExists(sourcePath, "sourcePath");
            Code.NotNullNorEmpty(destinationPath, "destinationPath");

            FileUtility.DeleteIfExists(destinationPath);
        }

        /// <summary>
        /// Build text file from the found records.
        /// </summary>
        public static void BuildText
            (
                [NotNull] FoundRecord[] foundRecords,
                [NotNull] string sourcePath,
                [NotNull] string destinationPath,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(foundRecords, "foundRecords");
            Code.FileExists(sourcePath, "sourcePath");
            Code.NotNullNorEmpty(destinationPath, "destinationPath");
            Code.NotNull(encoding, "encoding");

            FileUtility.DeleteIfExists(destinationPath);
            using (StreamWriter output = TextWriterUtility.Create(destinationPath, encoding))
            using (MstFile64 mstFile = new MstFile64(sourcePath, DirectAccessMode.ReadOnly))
            {
                foreach (FoundRecord foundRecord in foundRecords)
                {
                    MstRecord64 mstRecord = mstFile.ReadRecord(foundRecord.Position);
                    MarcRecord marcRecord = mstRecord.DecodeRecord();
                    PlainText.WriteRecord(output, marcRecord);
                }
            }
        }

        /// <summary>
        /// Rebuild XRF file from the found records.
        /// </summary>
        public static void BuildXrf
            (
                [NotNull] FoundRecord[] records,
                [NotNull] string xrfPath
            )
        {
            Code.NotNull(records, "records");
            Code.NotNullNorEmpty(xrfPath, "xrfPath");

            FileUtility.DeleteIfExists(xrfPath);
            Dictionary<int, FoundRecord> dictionary
                = records.ToDictionary(record => record.Mfn);
            int maxMfn = records.Max(record => record.Mfn);
            using (Stream stream = File.Create(xrfPath))
            {
                for (int mfn = 1; mfn <= maxMfn; mfn++)
                {
                    long offset;
                    int status;
                    if (dictionary.ContainsKey(mfn))
                    {
                        FoundRecord found = dictionary[mfn];
                        offset = found.Position;
                        status = found.Flags & (int)~RecordStatus.Last;
                    }
                    else
                    {
                        offset = 0;
                        status = (int)RecordStatus.PhysicallyDeleted;
                    }
                    stream.WriteInt64Network(offset);
                    stream.WriteInt32Network(status);
                }
            }
        }

        /// <summary>
        /// Check XRF and MST.
        /// </summary>
        [NotNull]
        public static string[] CheckXrf
            (
                [NotNull] string mstPath,
                [NotNull] string xrfPath
            )
        {
            Code.FileExists(mstPath, "mstPath");
            Code.FileExists(xrfPath, "xrfPath");

            List<string> result = new List<string>();
            FileInfo xrfInfo = new FileInfo(xrfPath);
            using (XrfFile64 xrf = new XrfFile64(xrfPath, DirectAccessMode.ReadOnly))
            using (Stream mstStream = File.OpenRead(mstPath))
            {
                long mstLength = mstStream.Length;
                // TODO в конце XRF могут быть нули, их надо игнорировать
                int maxMfn = (int)(xrfInfo.Length / XrfRecord64.RecordSize);
                MstControlRecord64 control = MstControlRecord64.Read(mstStream);
                if (maxMfn != control.NextMfn)
                {
                    result.Add("Length mismatch");
                }

                for (int mfn = 1; mfn < maxMfn; mfn++)
                {
                    XrfRecord64 xrfRecord = xrf.ReadRecord(mfn);
                    if ((xrfRecord.Status & RecordStatus.PhysicallyDeleted) == 0)
                    {
                        if (xrfRecord.Offset >= mstLength)
                        {
                            result.Add(mfn.ToInvariantString());
                        }
                        else
                        {
                            MstRecordLeader64 leader = MstRecordLeader64.Read(mstStream);
                            if (leader.Mfn != mfn)
                            {
                                result.Add(mfn.ToInvariantString());
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Find records.
        /// </summary>
        [NotNull]
        public static FoundRecord[] FindRecords
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            Dictionary<int, FoundRecord> dictionary = new Dictionary<int, FoundRecord>();
            using (Stream stream = File.OpenRead(fileName))
            {
                long fileLength = stream.Length;
                //MstControlRecord64 control = MstControlRecord64.Read(stream);
                //long position = MstControlRecord64.RecordSize;
                long position = 0x24;
                stream.Position = position;
                //int nextMfn = control.NextMfn;

                while (position < fileLength)
                {
                    try
                    {
                        MstRecordLeader64 leader = MstRecordLeader64.Read(stream);
                        if (leader.Mfn == 0)
                        {
                            break;
                        }

                        int mfn = leader.Mfn;
                        int length = leader.Length;
                        FoundRecord record = new FoundRecord
                        {
                            Mfn = mfn,
                            Position = position,
                            Length = length,
                            FieldCount = leader.Nvf,
                            Version = leader.Version,
                            Flags = leader.Status
                        };
                        dictionary[mfn] = record;

                        position += length;
                        stream.Position = position;
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return dictionary.Values.ToArray();
        }

        #endregion
    }
}
