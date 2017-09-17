using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Menus;

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class MenuFileTest
        : Common.CommonUnitTest
    {
        private void _CompareMenu
            (
                MenuFile first,
                MenuFile second
            )
        {
            Assert.AreEqual(first.FileName, second.FileName);

            MenuEntry[] firstEntries
                = first.SortEntries(MenuSort.None);
            MenuEntry[] secondEntries
                = second.SortEntries(MenuSort.None);

            Assert.AreEqual(firstEntries.Length, secondEntries.Length);
            for (int i = 0; i < firstEntries.Length; i++)
            {
                MenuEntry entry1 = firstEntries[i];
                MenuEntry entry2 = secondEntries[i];

                Assert.AreEqual(entry1.Code, entry2.Code);
                Assert.AreEqual(entry1.Comment, entry2.Comment);
            }
        }

        private void _TestSerialization
            (
                MenuFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            MenuFile second = bytes
                .RestoreObjectFromMemory<MenuFile>();

            _CompareMenu(first, second);
        }

        private MenuFile _GetMenu()
        {
            MenuFile result = new MenuFile();

            result
                .Add("a", "Comment for a")
                .Add("b", "Comment for b")
                .Add("c", "Comment for c");

            return result;
        }

        [TestMethod]
        public void MenuFile_Constructor_1()
        {
            MenuFile menu = _GetMenu();

            Assert.AreEqual(3, menu.Entries.Count);
            string actual = menu.GetString("c");
            Assert.AreEqual("Comment for c", actual);

            _TestSerialization(menu);
        }

        [TestMethod]
        public void MenuFile_LoadLocalFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "ORG.MNU"
                );

            MenuFile menu = MenuFile
                .ParseLocalFile(fileName);

            Assert.AreEqual(9, menu.Entries.Count);

            string actual = menu.GetString("1");
            Assert.AreEqual("RU", actual);
        }

        [TestMethod]
        public void MenuFile_Serialization_1()
        {
            MenuFile menu = new MenuFile();

            _TestSerialization(menu);
        }

        [TestMethod]
        public void MenuFile_ToJson_1()
        {
            MenuFile menu = _GetMenu();

            string actual = menu.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            string expected = "[{'code':'a','comment':'Comment for a'},{'code':'b','comment':'Comment for b'},{'code':'c','comment':'Comment for c'}]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuFile_FromJson_1()
        {
            string text = "[{'code':'a','comment':'Comment for a'},{'code':'b','comment':'Comment for b'},{'code':'c','comment':'Comment for c'}]"
                .Replace("'", "\"");

            MenuFile second = MenuUtility.FromJson(text);
            MenuFile first = _GetMenu();

            _CompareMenu(first,second);
        }

        [TestMethod]
        public void MenuFile_ParseLocalJsonFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test-menu.json"
                );

            MenuFile first = _GetMenu();
            MenuFile second = MenuUtility
                .ParseLocalJsonFile(fileName);

            _CompareMenu(first, second);
        }

        [TestMethod]
        public void MenuFile_ToXml_1()
        {
            MenuFile menu = _GetMenu();

            string actual = menu.ToXml()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            string expected = "<menu><entry code='a' comment='Comment for a' /><entry code='b' comment='Comment for b' /><entry code='c' comment='Comment for c' /></menu>";

            Assert.AreEqual(expected, actual);
        }
    }
}
