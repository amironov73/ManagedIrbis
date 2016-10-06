using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ParFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                ParFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ParFile second = bytes
                .RestoreObjectFromMemory<ParFile>();

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
        public void ParFile_Serialization()
        {
            ParFile parFile = new ParFile();
            _TestSerialization(parFile);

            parFile.MstPath = @".\datai\ibis\";
            _TestSerialization(parFile);
        }

        private ParFile _GetParFile ()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "ibis.par"
                );

            ParFile result = ParFile.ParseFile(fileName);

            return result;
        }

        [TestMethod]
        public void ParFile_ParseFile()
        {
            ParFile parFile = _GetParFile();
            Assert.AreEqual(@".\datai\ibis\", parFile.MstPath);
            _TestSerialization(parFile);
        }

        [TestMethod]
        public void ParFile_WriteText()
        {
            ParFile parFile = _GetParFile();

            StringWriter writer = new StringWriter();
            parFile.WriteText(writer);
            string text = writer.ToString();
            Assert.IsTrue(text.Length > 0);
        }
    }
}
