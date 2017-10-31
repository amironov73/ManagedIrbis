using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus9Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus9_GetCharacter_1()
        {
            Execute("+9F33", "!");

            // Обработка ошибок
            Execute("+9F", "");
        }

        [TestMethod]
        public void UniforPlus9_GetDirectoryName_1()
        {
            Execute(@"+92C:\Windows\System32\esent.dll", @"C:\Windows\System32\");

            // Обработка ошибок
            Execute("+92", "");
            Execute("+92C:", "");
        }

        [TestMethod]
        public void UniforPlus9_GetDrive_1()
        {
            Execute(@"+94C:\Windows\System32\esent.dll", "C:");

            // Обработка ошибок
            Execute("+94", "");
            Execute(@"+92\Windows", @"\");
            Execute(@"+92Windows", "");
        }

        [TestMethod]
        public void UniforPlus9_GetExtension_1()
        {
            Execute(@"+93C:\Windows\System32\esent.dll", ".dll");
            Execute(@"+93Windows.exe.dll", ".dll");

            // Обработка ошибок
            Execute("+93", "");
            Execute(@"+93\Windows", "");
            Execute("+93Windows", "");
        }

        [TestMethod]
        public void UniforPlus9_GetFileContent_1()
        {
            Execute("+9C2,IBIS,_test_hello.pft", "'Hello'");

            // Обработка ошибок
            Execute("+9C", "");
            Execute("+9C2,", "");
            Execute("+9C2,IBIS,", "");
            Execute("+9C2,IBIS,notexist.pft", "");
        }

        [TestMethod]
        public void UniforPlus9_FileExist_1()
        {
            Execute("+9L2,IBIS,_test_hello.pft", "1");
            Execute("+9L2,IBIS,notexist.pft", "0");

            Execute("+9L0,,http://google.com", "1");
            Execute("+9L0,,http://google.c", "0");

            // Обработка ошибок
            Execute("+9L", "");
            Execute("+9L2,", "");
            Execute("+9L2,IBIS,", "");
        }

        [TestMethod]
        public void UniforPlus9_GetFileName_1()
        {
            Execute(@"+91C:\Windows\System32\esent.dll", "esent.dll");
            Execute(@"+91Windows.exe.dll", "Windows.exe.dll");
            Execute("+91.dll", ".dll");

            // Обработка ошибок
            Execute("+91", "");
            Execute(@"+91\", "");
        }

        [TestMethod]
        public void UniforPlus9_GetFileSize_1()
        {
            const string fileName = @"C:\Windows\System32\esent.dll";
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                long fileSize = fileInfo.Length;
                Execute(@"+9A" + fileName, fileSize.ToInvariantString());
            }
            Execute("+9A.dll", "0");

            // Обработка ошибок
            Execute("+9A", "");
        }

        [TestMethod]
        public void UniforPlus9_GetIndex_1()
        {
            Execute(null, 0, "+90", "0");
            Execute(null, 1, "+90", "1");
        }

        [TestMethod]
        public void UniforPlus9_GetGeneration_1()
        {
            Execute("+9V", "64");
        }

        [TestMethod]
        public void UniforPlus9_StringLength_1()
        {
            Execute("+95", "0");
            Execute("+95Hello", "5");
            Execute("+95Привет", "6");
        }
    }
}
