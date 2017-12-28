using System;
using System.IO;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable LocalizableElement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class StandardFunctionsFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetExistingFile
            (
                [NotNull] string fileName
            )
        {
            return Path.Combine
                (
                    TestDataPath,
                    fileName
                );
        }

        [NotNull]
        private string _GetTemporaryFile()
        {
            return Path.GetTempFileName();
        }

        private void _Test
            (
                [NotNull] string source,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            PftFormatter formatter = new PftFormatter(context);
            formatter.ParseProgram(source);
            formatter.Program.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StandardFunctions_ReadAll_1()
        {
            string fileName = _GetExistingFile("ibis.par");
            string source = string.Format("$f=openRead('{0}'); readAll($f), close($f)", fileName);
            _Test(source, "1=.\\datai\\ibis\\\n2=.\\datai\\ibis\\\n3=.\\datai\\ibis\\\n4=.\\datai\\ibis\\\n5=.\\datai\\ibis\\\n6=.\\datai\\ibis\\\n7=.\\datai\\ibis\\\n8=.\\datai\\ibis\\\n9=.\\datai\\ibis\\\n10=.\\datai\\ibis\\\n11=.\\datai\\ibis\\\n");
        }

        [TestMethod]
        public void StandardFunctions_ReadLine_1()
        {
            string fileName = _GetExistingFile("ibis.par");
            string source = string.Format("$f=openRead('{0}'); readLine($f), close($f)", fileName);
            _Test(source, "1=.\\datai\\ibis\\");
        }

        [TestMethod]
        public void StandardFunctions_IsOpen_1()
        {
            string fileName = _GetExistingFile("ibis.par");
            string source = string.Format("$f=openRead('{0}'); isOpen($f) / close($f) isOpen($f)", fileName);
            _Test(source, "1\n0");
        }

        [TestMethod]
        public void StandardFunctions_Write_1()
        {
            string expected = "Test text";
            string fileName = _GetTemporaryFile();
            string source = string.Format("$f=openWrite('{0}'); write($f;'{1}'), close($f)", fileName, expected);
            _Test(source, "");
            string actual = File.ReadAllText(fileName);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StandardFunctions_WriteLine_1()
        {
            string expected = "Test text";
            string fileName = _GetTemporaryFile();
            string source = string.Format("$f=openWrite('{0}'); writeLine($f;'{1}'), close($f)", fileName, expected);
            _Test(source, "");
            string actual = File.ReadAllText(fileName);
            Assert.AreEqual(expected + "\n", actual.DosToUnix());
        }

        [TestMethod]
        public void StandardFunctions_OpenAppend_1()
        {
            string fileName = _GetTemporaryFile();
            File.WriteAllText(fileName, "Line1" + Environment.NewLine);
            string source = string.Format("$f=openAppend('{0}'); write($f;'{1}'), close($f)", fileName, "Line2");
            _Test(source, "");
            string expected = "Line1\nLine2";
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }
    }
}
