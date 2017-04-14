// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Irbis64Dll.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Server;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Irbis64Dll
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Configuration.
        /// </summary>
        [NotNull]
        public ServerConfiguration Configuration { get; internal set; }

        /// <summary>
        /// Current database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; private set; }

        /// <summary>
        /// Memory layout.
        /// </summary>
        public NonNullValue<SpaceLayout> Layout { get; set; }

        /// <summary>
        /// Current database parameters.
        /// </summary>
        [CanBeNull]
        public ParFile Parameters { get; private set; }

        /// <summary>
        /// Current shelf number.
        /// </summary>
        public int Shelf { get; set; }

        /// <summary>
        /// Pointer to IrbisSpace structure.
        /// </summary>
        public IntPtr Space { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public Irbis64Dll
            (
                [NotNull] ServerConfiguration configuration
            )
        {
            Code.NotNull(configuration, "configuration");

            Layout = new SpaceLayout();

            Configuration = configuration;
            _Initialize();
        }

        #endregion

        #region Private members

        static void _HandleRetCode
            (
                string methodName,
                int retCode
            )
        {
            if (retCode < 0)
            {
                throw new IrbisException
                    (
                        methodName
                        + " return code="
                        + retCode
                    );
            }
        }

        private void _Initialize()
        {
            string systemPath = Configuration.SystemPath
                .ThrowIfNull("systemPath not set");
            string dataPath = Configuration.DataPath
                .ThrowIfNull("dataPath not set");
            string uctab = Configuration.UpperCaseTable
                .ThrowIfNull("UpperCaseTable not set");
            string lctab = string.Empty;
            string actab = Configuration.AlphabetTablePath
                .ThrowIfNull("AplhabetTablePath not set");
            string execDir = systemPath;
            string dataDir = dataPath;
            int retCode = Irbis65Dll.IrbisUatabInit
                (
                    uctab,
                    lctab,
                    actab,
                    execDir,
                    dataDir
                );
            _HandleRetCode("IrbisUatabInit", retCode);

            string depositPath = Path.GetFullPath
            (
                Path.Combine
                (
                    dataPath,
                    "Deposit"
                )
            );
            retCode = Irbis65Dll.IrbisInitDeposit(depositPath);
            _HandleRetCode("IrbisInitDeposit", retCode);

            Irbis65Dll.IrbisSetOptions(-1, 0, 0);
            Space = Irbis65Dll.IrbisInit();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs simple search for exact term match.
        /// </summary>
        [NotNull]
        public int[] ExactSearch
            (
                [NotNull] string term
            )
        {
            Code.NotNullNorEmpty(term, "term");

            Encoding utf = IrbisEncoding.Utf8;
            byte[] buffer = new byte[512];
            utf.GetBytes(term, 0, term.Length, buffer, 0);
            int retCode = Irbis65Dll.IrbisFind(Space, buffer);
            if (retCode < 0)
            {
                return new int[0];
            }

            int nposts = Irbis65Dll.IrbisNPosts(Space);
            _HandleRetCode("IrbisNPosts", nposts);

            int[] result = new int[nposts];
            for (int i = 0; i < nposts; i++)
            {
                retCode = Irbis65Dll.IrbisNextPost(Space);
                _HandleRetCode("IrbisNextPost", retCode);

                int mfn = Irbis65Dll.IrbisPosting(Space, 1);
                _HandleRetCode("IrbisPosting", mfn);
                result[i] = mfn;
            }

            return result;
        }

        /// <summary>
        /// Get full path for the PFT file (without extension).
        /// </summary>
        [NotNull]
        public string GetPftPath
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            ParFile parameters = Parameters
                    .ThrowIfNull("paramters not set");
            string pftPath = parameters.PftPath
                .ThrowIfNull("pftPath not set");
            string systemPath = Configuration.SystemPath
                .ThrowIfNull("systemPath not set");
            pftPath = Path.GetFullPath
            (
                Path.Combine
                (
                    systemPath,
                    pftPath
                )
            );
            string result = Path.Combine
            (
                pftPath,
                fileName
            );

            return result;
        }

        /// <summary>
        /// Format current record.
        /// </summary>
        [NotNull]
        public string FormatRecord()
        {
            int retcode = Irbis65Dll.IrbisFormat
                (
                    Space,
                    0 /*номер полки*/,
                    1,
                    0,
                    32000 /*размер буфера*/,
                    "IRBIS64"
                );
            _HandleRetCode("IrbisFormat", retcode);

            string result = GetFormattedText();

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public string FormatRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            ReadRecord(mfn);
            string result = FormatRecord();

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public string FormatRecord
            (
                string format,
                int mfn
            )
        {
            Code.NotNullNorEmpty(format, "format");
            Code.Positive(mfn, "mfn");

            ReadRecord(mfn);
            SetFormat(format);
            string result = FormatRecord();

            return result;
        }

        /// <summary>
        /// Get current mfn.
        /// </summary>
        public int GetCurrentMfn()
        {
            int result = Irbis65Dll.IrbisMfn(Space, Shelf);
            _HandleRetCode("IrbisMfn", result);

            return result;
        }

        /// <summary>
        /// Get version of IRBIS64.dll.
        /// </summary>
        public static string GetDllVersion()
        {
            StringBuilder result = new StringBuilder(255);
            Irbis65Dll.IrbisDllVersion(result, result.Capacity);

            return result.ToString();
        }

        /// <summary>
        /// Get interop version for irbis65.dll.
        /// </summary>
        public static int GetInteropVersion()
        {
            return Irbis65Dll.InteropVersion();
        }

        /// <summary>
        /// Get max MFN for current database.
        /// </summary>
        public int GetMaxMfn()
        {
            int result = Irbis65Dll.IrbisMaxMfn(Space);
            _HandleRetCode("IrbisMaxMfn", result);

            return result;
        }

        /// <summary>
        /// Get formatted text.
        /// </summary>
        [NotNull]
        public string GetFormattedText()
        {
            SpaceLayout layout = Layout;

            if (layout.FormattedOffset == 0)
            {
                throw new IrbisException("formattedOffset not set");
            }

            IntPtr textPointer = Space.GetPointer32
            (
                layout.FormattedOffset
            );
            Encoding encoding = IrbisEncoding.Utf8;
            string result = InteropUtility.GetZeroTerminatedString
                (
                    textPointer,
                    encoding,
                    32000
                );

            return result;
        }

        /// <summary>
        /// Get native record from memory.
        /// </summary>
        [NotNull]
        public NativeRecord GetRecord()
        {
            byte[] memory = GetRecordMemory();
            NativeRecord result 
                = NativeRecord.ParseMemory(memory);

            return result;
        }

        /// <summary>
        /// Get memory block for current record.
        /// </summary>
        public byte[] GetRecordMemory()
        {
            SpaceLayout layout = Layout;

            IntPtr recordPointer = Space.GetPointer32
                (
                    layout.RecordOffset
                );
            int recordLength 
                = Marshal.ReadInt32(recordPointer, 4);
            byte[] memory = recordPointer.GetBlock(recordLength);

            return memory;
        }

        /// <summary>
        /// Determines version for given record.
        /// </summary>
        public int GetRecordVersion
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            int result = Irbis65Dll.IrbisReadVersion(Space, mfn);
            _HandleRetCode("IrbisReadVersion", result);

            return result;
        }

        /// <summary>
        /// Grab block by pointer.
        /// </summary>
        [NotNull]
        public byte[] GrabBlockByPointer
            (
                int offset,
                int length
            )
        {
            IntPtr pointer = Space.GetPointer32(offset);
            byte[] result = new byte[length];
            Marshal.Copy(pointer, result,0, length);

            return result;
        }

        /// <summary>
        /// Grab string by pointer.
        /// </summary>
        [NotNull]
        public string GrabStringByPointer
            (
                [NotNull] Encoding encoding,
                int offset,
                int maxLength
            )
        {
            Code.NotNull(encoding, "encoding");

            IntPtr pointer = Space.GetPointer32(offset);
            string result
                = InteropUtility.GetZeroTerminatedString
                    (
                        pointer,
                        encoding,
                        maxLength
                    );

            return result;
        }

        /// <summary>
        /// Determines whether the database is locked.
        /// </summary>
        public bool IsDatabaseLocked()
        {
            int result = Irbis65Dll.IrbisIsDbLocked(Space);
            _HandleRetCode("IrbisIsDbLocked", result);

            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Determines whether the record is actualized.
        /// </summary>
        public bool IsRecordActualized()
        {
            int result = Irbis65Dll.IrbisIsActualized(Space, Shelf);
            _HandleRetCode("IrbisIsActualized", result);

            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Determines whether the record is deleted.
        /// </summary>
        public bool IsRecordDeleted()
        {
            int result = Irbis65Dll.IrbisIsDeleted(Space, Shelf);
            _HandleRetCode("IrbisIsDeleted", result);

            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Determines whether the record is locked.
        /// </summary>
        public bool IsRecordLocked()
        {
            int result = Irbis65Dll.IrbisIsLocked(Space, Shelf);
            _HandleRetCode("IrbisIsLocked", result);

            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Determines whether the record is locked.
        /// </summary>
        public bool IsRecordLocked
            (
                int mfn
            )
        {
            int result = Irbis65Dll.IrbisIsRealyLocked
                (
                    Space,
                    mfn
                );
            _HandleRetCode("IrbisIsReallyLocked", result);

            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Read record with specified MFN.
        /// </summary>
        public void ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            int result = Irbis65Dll.IrbisRecord(Space, Shelf, mfn);
            _HandleRetCode("IrbisRecord", result);

            if (Layout.Value.RecordOffset == 0)
            {
                Layout.Value.FindTheRecord
                (
                    Space,
                    mfn,
                    0x4000
                );
            }
        }

        /// <summary>
        /// Setup the format.
        /// </summary>
        public void SetFormat
            (
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(format, "format");

            if (Layout.Value.FormattedOffset == 0)
            {
                Layout.Value.FindTheFormattedText
                    (
                        Space,
                        0x4000
                    );
            }

            int result = Irbis65Dll.IrbisInitPft(Space, format);
            _HandleRetCode("IrbisInitPft", result);

            Irbis65Dll.IrbisInitUactab(Space);
        }

        /// <summary>
        /// Use specified database.
        /// </summary>
        public void UseDatabase
            (
                [NotNull] string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            if (Database.SameString(databaseName))
            {
                return;
            }

            string parPath = Path.Combine
                (
                    Configuration.DataPath,
                    databaseName + ".par"
                );
            ParFile parFile = ParFile.ParseFile(parPath);
            Parameters = parFile;
            string mstPath = parFile.MstPath
                .ThrowIfNull("mstPath not set");
            mstPath = Path.GetFullPath
                (
                    Path.Combine
                    (
                        Configuration.SystemPath,
                        mstPath
                    )
                );
            mstPath = Path.Combine
                (
                    mstPath,
                    databaseName
                );

            int retCode = Irbis65Dll.IrbisInitMst
                (
                    Space,
                    mstPath, 
                    5
                );
            _HandleRetCode("IrbisInitMst", retCode);

            string termPath = parFile.IfpPath
                .ThrowIfNull("ibisPar.IfpPath not set");
            termPath = Path.GetFullPath
                (
                    Path.Combine
                        (
                            Configuration.SystemPath, 
                            termPath
                        )
                );
            termPath = Path.Combine
                (
                    termPath,
                    databaseName
                );
            retCode = Irbis65Dll.IrbisInitTerm
                (
                    Space,
                    termPath
                );
            _HandleRetCode("IrbisInitTerm", retCode);

            Database = databaseName;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        public void Dispose()
        {
            if (Space.ToInt32() != 0)
            {
                Irbis65Dll.IrbisClose(Space);
            }
        }

        #endregion
    }
}
