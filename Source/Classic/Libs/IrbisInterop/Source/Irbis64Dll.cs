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
        /// Get native record from memory.
        /// </summary>
        [NotNull]
        public NativeRecord GetRecord()
        {
            IntPtr recordPointer = Space.GetPointer32(626);
            int recordLength = Marshal.ReadInt32(recordPointer, 4);
            byte[] memory = recordPointer.GetBlock(recordLength);
            NativeRecord result = NativeRecord.ParseMemory(memory);

            return result;
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

            string parPath = Path.Combine
                (
                    Configuration.DataPath,
                    databaseName + ".par"
                );
            ParFile parFile = ParFile.ParseFile(parPath);
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
            retCode = Irbis65Dll.IrbisInitTerm(Space, termPath);
            _HandleRetCode("IrbisInitTerm", retCode);
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
