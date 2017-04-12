// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
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
using AM.IO;

using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Server;

#endregion

// ReSharper disable LocalizableElement

namespace IrbisInteropTester
{
    class Program
    {
        static void HandleRetCode
            (
                int retCode
            )
        {
            if (retCode < 0)
            {
                throw new Exception("RetCode=" + retCode);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("IrbisInteropTester <irbis_server.ini>");
                return;
            }

            try
            {
                string fileName = args[0];
                Encoding encoding = IrbisEncoding.Ansi;
                IniFile iniFile = new IniFile
                (
                    fileName,
                    encoding,
                    false
                );
                ServerIniFile serverIni = new ServerIniFile
                (
                    iniFile
                );

                string systemPath = serverIni.SystemPath
                    .ThrowIfNull("systemPath not set");
                Console.WriteLine("SystemPath={0}", systemPath);

                string dataPath = serverIni.DataPath
                    .ThrowIfNull("dataPath not set");
                Console.WriteLine("DataPath={0}", dataPath);

                //Environment.CurrentDirectory = systemPath;
                //Console.WriteLine("Directory changed to {0}", Environment.CurrentDirectory);

                int interopVersion = Irbis65Dll.InteropVersion();
                Console.WriteLine("InteropVersion={0}", interopVersion);

                StringBuilder dllVersion = new StringBuilder(100);
                Irbis65Dll.IrbisDllVersion(dllVersion, dllVersion.Capacity);
                Console.WriteLine("Irbis64.dll version={0}", dllVersion);

                IntPtr space = Irbis65Dll.IrbisInit();
                Console.WriteLine("Initialized");

                string uctab = Path.Combine(systemPath, "isisucw");
                string lctab = Path.Combine(systemPath, "isisacw");
                string actab = Path.Combine(systemPath, "isisacw");
                string execDir = systemPath;
                string dataDir = dataPath;
                int retCode = Irbis65Dll.IrbisUatabInit(uctab, lctab, actab, execDir, dataDir);
                Console.WriteLine("IrbisUatabInit={0}", retCode);
                HandleRetCode(retCode);
                IntPtr uaPtr = new IntPtr(retCode);
                byte[] bytes = new byte[192];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Marshal.ReadByte(uaPtr, i);
                }
                DumpUtility.Dump(Console.Out, bytes);

                string ibisParPath = Path.Combine
                    (
                        dataPath,
                        "ibis.par"
                    );
                ParFile ibisPar = ParFile.ParseFile(ibisParPath);
                string mstPath = ibisPar.MstPath
                    .ThrowIfNull("mstPath not set");
                mstPath = Path.GetFullPath
                    (
                        Path.Combine(systemPath, mstPath)
                    );
                mstPath = Path.Combine
                    (
                        mstPath,
                        "ibis"
                    );

                retCode = Irbis65Dll.IrbisInitMst(space, mstPath, 1);
                Console.WriteLine("IrbisInitMst({0})={1}", mstPath, retCode);
                HandleRetCode(retCode);

                string termPath = ibisPar.IfpPath
                    .ThrowIfNull("ibisPar.IfpPath not set");
                termPath = Path.GetFullPath
                (
                    Path.Combine(systemPath, termPath)
                );
                termPath = Path.Combine
                (
                    termPath,
                    "ibis"
                );
                retCode = Irbis65Dll.IrbisInitTerm(space, termPath);
                Console.WriteLine("IrbisInitTerm({0})={1}", termPath, retCode);
                HandleRetCode(retCode);

                int maxMfn = Irbis65Dll.IrbisMaxMfn(space);
                Console.WriteLine("MaxMFN={0}", maxMfn);
                HandleRetCode(maxMfn);

                int mfn = 1;
                retCode = Irbis65Dll.IrbisRecord(space, 0, mfn);
                Console.WriteLine("IrbisRecord({0})={1}", mfn, retCode);
                HandleRetCode(retCode);

                string pftPath = ibisPar.PftPath
                    .ThrowIfNull("pftPath not set");
                pftPath = Path.GetFullPath
                    (
                        Path.Combine
                            (
                                systemPath,
                                pftPath
                            )
                    );
                Console.WriteLine("PftPath={0}", pftPath);
                string briefPath = Path.Combine
                    (
                        pftPath,
                        "brief"
                    );

                int retcode = Irbis65Dll.IrbisInitPft(space, "@" + briefPath);
                Console.WriteLine("IrbisInitPft({0})={1}", briefPath, retcode);
                HandleRetCode(retcode);

                retcode = Irbis65Dll.IrbisFormat
                    (
                        space,
                        0 /*номер полки*/,
                        1,
                        0,
                        32000 /*размер буфера*/,
                        "IRBIS64"
                    );
                Console.WriteLine("IrbisFormat={0}", retcode);
                HandleRetCode(retcode);

                string formattedRecord = Irbis65Dll.GetFormattedRecord(space);
                Console.WriteLine(formattedRecord);

                Irbis65Dll.IrbisClose(space);
                Console.WriteLine("Closed");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
