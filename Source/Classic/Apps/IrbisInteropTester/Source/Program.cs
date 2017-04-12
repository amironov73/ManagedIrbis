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
                
                Environment.CurrentDirectory = systemPath;
                Console.WriteLine("Directory changed to {0}", Environment.CurrentDirectory);

                int interopVersion = Irbis65Dll.InteropVersion();
                Console.WriteLine("InteropVersion={0}", interopVersion);

                StringBuilder dllVersion = new StringBuilder(100);
                Irbis65Dll.IrbisDllVersion(dllVersion, dllVersion.Capacity);
                Console.WriteLine("Irbis64.dll version={0}", dllVersion);

                IntPtr space = Irbis65Dll.IrbisInit();
                Console.WriteLine("Initialized");

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
