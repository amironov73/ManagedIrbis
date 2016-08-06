using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisParFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                IrbisParFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisParFile second = bytes
                .RestoreObjectFromMemory<IrbisParFile>();

            Assert.AreEqual(first.AnyPath, second.AnyPath);
            Assert.AreEqual(first.CntPath, second.CntPath);
            Assert.AreEqual(first.ExtPath, second.ExtPath);
            Assert.AreEqual(first.IfpPath, second.IfpPath);
            Assert.AreEqual(first.L01Path, second.L01Path);
            Assert.AreEqual(first.L02Path, second.L02Path);
            Assert.AreEqual(first.MstPath, second.MstPath);
            Assert.AreEqual(first.N01Path, second.N01Path);
            Assert.AreEqual(first.N02Path, second.N02Path);
            Assert.AreEqual(first.PftPath, second.PftPath);
            Assert.AreEqual(first.XrfPath, second.XrfPath);
        }

        [TestMethod]
        public void TestIrbisParFileSerialization()
        {
            IrbisParFile parFile = new IrbisParFile();
            _TestSerialization(parFile);

            parFile.MstPath = @".\datai\ibis\";
            _TestSerialization(parFile);
        }

        private IrbisParFile _GetParFile ()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "ibis.par"
                );

            IrbisParFile result = IrbisParFile.ParseFile(fileName);

            return result;
        }

        [TestMethod]
        public void TestIrbisParFileParseFile()
        {
            IrbisParFile parFile = _GetParFile();
            Assert.AreEqual(@".\datai\ibis\", parFile.MstPath);
            _TestSerialization(parFile);
        }

        [TestMethod]
        public void TestIrbisParFileWriteText()
        {
            IrbisParFile parFile = _GetParFile();

            StringWriter writer = new StringWriter();
            parFile.WriteText(writer);
            string text = writer.ToString();
            Assert.IsTrue(text.Length > 0);
        }
    }
}
