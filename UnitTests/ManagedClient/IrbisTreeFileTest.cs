using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisTreeFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestIrbisTreeFileConstruction()
        {
            IrbisTreeFile tree = new IrbisTreeFile();

            Assert.AreEqual(0, tree.Roots.Count);

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileParseLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "II.TRE"
                );

            IrbisTreeFile tree = IrbisTreeFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            Assert.AreEqual(4, tree.Roots.Count);

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileSave()
        {
            
        }

        private void _TestSerialization
            (
                IrbisTreeFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisTreeFile second = bytes
                .RestoreObjectFromMemory<IrbisTreeFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Roots.Count, second.Roots.Count);
        }

        [TestMethod]
        public void TestIrbisTreeFileSerialization()
        {
            IrbisTreeFile tree = new IrbisTreeFile();
            _TestSerialization(tree);
        }
    }
}
