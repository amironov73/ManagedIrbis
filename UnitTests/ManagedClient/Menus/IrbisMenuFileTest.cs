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
        private void _CompareMenu
            (
                IrbisMenuFile first,
                IrbisMenuFile second
            )
        {
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

        private void _TestSerialization
            (
                IrbisMenuFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisMenuFile second = bytes
                .RestoreObjectFromMemory<IrbisMenuFile>();

            _CompareMenu(first, second);
        }

        private IrbisMenuFile _GetMenu()
        {
            IrbisMenuFile result = new IrbisMenuFile();

            result
                .Add("a", "Comment for a")
                .Add("b", "Comment for b")
                .Add("c", "Comment for c");

            return result;
        }

        [TestMethod]
        public void TestIrbisMenuFileConstruction()
        {
            IrbisMenuFile menu = _GetMenu();

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

        [TestMethod]
        public void TestIrbisMenuFileToJson()
        {
            IrbisMenuFile menu = _GetMenu();

            string actual = menu.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            string expected = "[{'code':'a','comment':'Comment for a'},{'code':'b','comment':'Comment for b'},{'code':'c','comment':'Comment for c'}]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIrbisMenuFileFromJson()
        {
            string text = "[{'code':'a','comment':'Comment for a'},{'code':'b','comment':'Comment for b'},{'code':'c','comment':'Comment for c'}]"
                .Replace("'", "\"");

            IrbisMenuFile second = IrbisMenuUtility.FromJson(text);
            IrbisMenuFile first = _GetMenu();

            _CompareMenu(first,second);
        }

        [TestMethod]
        public void TestIrbisMenuFileParseLocalJsonFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test-menu.json"
                );

            IrbisMenuFile first = _GetMenu();
            IrbisMenuFile second = IrbisMenuUtility
                .ParseLocalJsonFile(fileName);

            _CompareMenu(first, second);
        }

        [TestMethod]
        public void TestIrbisMenuToXml()
        {
            IrbisMenuFile menu = _GetMenu();

            string actual = menu.ToXml()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            string expected = "<?xml version='1.0' encoding='utf-16'?><menu xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  <entry code='a' comment='Comment for a' />  <entry code='b' comment='Comment for b' />  <entry code='c' comment='Comment for c' /></menu>";

            Assert.AreEqual(expected, actual);
        }
    }
}
