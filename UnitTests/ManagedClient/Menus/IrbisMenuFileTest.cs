using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient.Menus;

namespace UnitTests.ManagedClient.Menus
{
    [TestClass]
    public class IrbisMenuFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                IrbisMenuFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisMenuFile second = bytes
                .RestoreObjectFromMemory<IrbisMenuFile>();

            Assert.AreEqual(first.FileName, second.FileName);

            IrbisMenuFile.Entry[] firstEntries
                = first.SortEntries(IrbisMenuFile.Sort.None);
            IrbisMenuFile.Entry[] secondEntries
                = second.SortEntries(IrbisMenuFile.Sort.None);

            Assert.AreEqual(firstEntries.Length, secondEntries.Length);
            for (int i = 0; i < firstEntries.Length; i++)
            {
                IrbisMenuFile.Entry entry1 = firstEntries[i];
                IrbisMenuFile.Entry entry2 = secondEntries[i];

                Assert.AreEqual(entry1.Code, entry2.Code);
                Assert.AreEqual(entry1.Comment, entry2.Comment);
            }
        }

        [TestMethod]
        public void TestIrbisMenuFileConstruction()
        {
            IrbisMenuFile menu = new IrbisMenuFile();

            menu
                .Add("a", "Comment for a")
                .Add("b", "Comment for b")
                .Add("c", "Comment for c");

            Assert.AreEqual(3, menu.Entries.Count);
            string actual = menu.GetString("c");
            Assert.AreEqual("Comment for c", actual);

            _TestSerialization(menu);
        }

        [TestMethod]
        public void TestIrbisMenuFileLoadLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "ORG.MNU"
                );

            IrbisMenuFile menu = IrbisMenuFile
                .ParseLocalFile(fileName);

            Assert.AreEqual(9, menu.Entries.Count);

            string actual = menu.GetString("1");
            Assert.AreEqual("RU", actual);
        }

        [TestMethod]
        public void TestIrbisMenuFileSerialization()
        {
            IrbisMenuFile menu = new IrbisMenuFile();

            _TestSerialization(menu);
        }
    }
}
