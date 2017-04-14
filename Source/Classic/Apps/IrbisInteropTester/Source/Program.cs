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
using AM.Runtime;
using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Server;

#endregion

// ReSharper disable LocalizableElement

namespace IrbisInteropTester
{
    class Program
    {
        static void DumpAddress
            (
                IntPtr pointer,
                int count
            )
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Marshal.ReadByte(pointer, i);
            }
            DumpUtility.Dump(Console.Out, bytes);
        }

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

        static string FromBytes
            (
                Encoding encoding,
                byte[] bytes
            )
        {
            int pos;
            for (pos = 0; pos < bytes.Length; pos++)
            {
                if (bytes[pos] == 0)
                {
                    break;
                }
            }
            string result = encoding.GetString(bytes, 0, pos);

            return result;
        }

        static void Test1(string[] args)
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

                int interopVersion = Irbis65Dll.InteropVersion();
                Console.WriteLine("InteropVersion={0}", interopVersion);

                StringBuilder dllVersion = new StringBuilder(100);
                Irbis65Dll.IrbisDllVersion(dllVersion, dllVersion.Capacity);
                Console.WriteLine("Irbis64.dll version={0}", dllVersion);

                string uctab = serverIni.UpperCaseTable;
                string lctab = string.Empty;
                string actab = serverIni.AlphabetTablePath;
                string execDir = systemPath;
                string dataDir = dataPath;
                int retCode = Irbis65Dll.IrbisUatabInit(uctab, lctab, actab, execDir, dataDir);
                Console.WriteLine("IrbisUatabInit={0}", retCode);
                HandleRetCode(retCode);
                //DumpAddress(new IntPtr(retCode), 192);

                string depositPath = Path.GetFullPath
                    (
                        Path.Combine
                        (
                            dataPath,
                            "Deposit"
                        )
                    );
                Console.WriteLine("DepositPath={0}", depositPath);
                retCode = Irbis65Dll.IrbisInitDeposit(depositPath);
                Console.WriteLine("IrbisInitDeposit({0})={1}", depositPath, retCode);
                HandleRetCode(retCode);

                Irbis65Dll.IrbisSetOptions(-1, 0, 0);
                Console.WriteLine("IrbisSetOptions(-1,0,0)");

                IntPtr space = Irbis65Dll.IrbisInit();
                Console.WriteLine("IrbisInit=0x{0:X8}", space.ToInt32());
                //DumpAddress(space, 256);

                string mainIni = Path.GetFullPath
                    (
                        Path.Combine
                        (
                            systemPath,
                            "irbisc.ini"
                        )
                    );
                Irbis65Dll.IrbisMainIniInit(mainIni);
                Console.WriteLine("IrbisMainIniInit({0})", mainIni);

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

                retCode = Irbis65Dll.IrbisIsDbLocked(space);
                Console.WriteLine("IrbisIsDbLocked={0}", retCode);
                HandleRetCode(retCode);

                int maxMfn = Irbis65Dll.IrbisMaxMfn(space);
                Console.WriteLine("MaxMFN={0}", maxMfn);
                HandleRetCode(maxMfn);

                int mfn = 1;
                retCode = Irbis65Dll.IrbisRecord(space, 0, mfn);
                Console.WriteLine("IrbisRecord({0})={1}", mfn, retCode);
                HandleRetCode(retCode);

                mfn = Irbis65Dll.IrbisMfn(space, 0);
                Console.WriteLine("IrbisMfn={0}", mfn);
                HandleRetCode(mfn);

                retCode = Irbis65Dll.IrbisNFields(space, 0);
                Console.WriteLine("IrbisNFields={0}", retCode);
                HandleRetCode(retCode);

                int isLocked = Irbis65Dll.IrbisIsLocked(space, 0);
                int isDeleted = Irbis65Dll.IrbisIsDeleted(space, 0);
                int isActualized = Irbis65Dll.IrbisIsActualized(space, 0);
                Console.WriteLine("Locked={0}, Deleted={1}, Actualized={2}",
                    isLocked, isDeleted, isActualized);
                HandleRetCode(isLocked);
                HandleRetCode(isDeleted);
                HandleRetCode(isActualized);

                isLocked = Irbis65Dll.IrbisIsRealyLocked(space, mfn);
                Console.WriteLine("IrbisIsReallyLocked={0}", isLocked);
                HandleRetCode(isLocked);

                int version = Irbis65Dll.IrbisReadVersion(space, mfn);
                Console.WriteLine("IrbisReadVersion={0}", version);
                HandleRetCode(version);

                //string rawRecordText = Irbis65Dll.GetRawRecordText(space);
                //Console.WriteLine(rawRecordText);

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

                Irbis65Dll.IrbisInitUactab(space);
                Console.WriteLine("IrbisInitUactab");

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

                //string formattedRecord = Irbis65Dll.GetFormattedRecord(space);
                //Console.WriteLine(formattedRecord);

                Encoding utf = Encoding.UTF8;
                byte[] term = new byte[512];
                string text = "K=БЕТОН";
                utf.GetBytes(text, 0, text.Length, term, 0);
                retCode = Irbis65Dll.IrbisFind(space, term);
                Console.WriteLine("IrbisFind={0}", retCode);
                for (int i = 0; i < 10; i++)
                {
                    retCode = Irbis65Dll.IrbisNextTerm(space, term);
                    if (retCode < 0)
                    {
                        break;
                    }
                    text = FromBytes(utf, term);
                    int nposts = Irbis65Dll.IrbisNPosts(space);
                    Console.Write("{0,-8} {1}:", nposts, text);
                    for (int j = 0; j < nposts; j++)
                    {
                        Irbis65Dll.IrbisNextPost(space);
                        mfn = Irbis65Dll.IrbisPosting(space, 1);
                        Console.Write
                            (
                                "{0}{1}",
                                j == 0 ? " " : ", ",
                                mfn
                            );
                    }
                    Console.WriteLine();
                }

                Irbis65Dll.IrbisClose(space);
                Console.WriteLine("Closed");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static void Test2(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("IrbisInteropTester <irbis_server.ini>");
                return;
            }

            try
            {
                ServerConfiguration configuration
                    = ServerConfiguration.FromIniFile(args[0]);
                using (Irbis64Dll irbis = new Irbis64Dll(configuration))
                {
                    Console.WriteLine
                        (
                            "Irbis64.dll version={0}",
                            Irbis64Dll.GetDllVersion()
                        );

                    irbis.UseDatabase("ibis");
                    Console.WriteLine
                        (
                            "Max MFN={0}",
                            irbis.GetMaxMfn()
                        );

                    string briefPft = irbis.GetPftPath("brief");
                    irbis.SetFormat("@" + briefPft);

                    for (int mfn = 10; mfn < 200; mfn++)
                    {
                        irbis.ReadRecord(mfn);
                        Console.WriteLine("Read record MFN={0}", mfn);

                        NativeRecord record = irbis.GetRecord();
                        Console.WriteLine(record);

                        string text = irbis.FormatRecord();
                        Console.WriteLine(text);

                        Console.WriteLine();
                    }

                    Console.WriteLine
                        (
                            "Record offset={0}, formatted offset={1}",
                            irbis.Layout.Value.RecordOffset,
                            irbis.Layout.Value.FormattedOffset
                        );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static void Main(string[] args)
        {
            //Test1(args);
            Test2(args);
        }
    }
}
