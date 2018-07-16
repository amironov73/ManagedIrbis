using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Moq;

// ReSharper disable ExpressionIsAlwaysNull

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

            MenuEntry[] firstEntries = first.SortEntries(MenuSort.None);
            MenuEntry[] secondEntries = second.SortEntries(MenuSort.None);

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
        public void MenuFile_Static_Constructor_1()
        {
            Assert.IsNotNull(MenuFile.MenuSeparators);
        }

        [TestMethod]
        public void MenuFile_TrimCode_1()
        {
            Assert.AreEqual("Abc", MenuFile.TrimCode("Abc"));
            Assert.AreEqual("Abc", MenuFile.TrimCode("Abc-123"));
            Assert.AreEqual("Abc", MenuFile.TrimCode("Abc 123"));
        }

        [TestMethod]
        public void MenuFile_FindEntrySensitive_1()
        {
            MenuFile menu = _GetMenu();
            Assert.IsNotNull(menu.FindEntrySensitive("a"));
            Assert.IsNull(menu.FindEntrySensitive("A"));
        }

        [TestMethod]
        public void MenuFile_GetEntry_1()
        {
            MenuFile menu = _GetMenu();

            Assert.IsNotNull(menu.GetEntry("a"));
            Assert.IsNull(menu.GetEntry("e"));
            Assert.IsNotNull(menu.GetEntry("A"));
            Assert.IsNull(menu.GetEntry("E"));
            Assert.IsNotNull(menu.GetEntry(" a"));
            Assert.IsNull(menu.GetEntry(" e"));
            Assert.IsNotNull(menu.GetEntry("a-123"));
            Assert.IsNull(menu.GetEntry("e-123"));
            Assert.IsNotNull(menu.GetEntry("A-123"));
            Assert.IsNull(menu.GetEntry("E-123"));
        }

        [TestMethod]
        public void MenuFile_GetEntrySensitive_1()
        {
            MenuFile menu = _GetMenu();

            Assert.IsNotNull(menu.GetEntrySensitive("a"));
            Assert.IsNull(menu.GetEntrySensitive("e"));
            Assert.IsNull(menu.GetEntrySensitive("A"));
            Assert.IsNull(menu.GetEntrySensitive("E"));
            Assert.IsNotNull(menu.GetEntrySensitive(" a"));
            Assert.IsNull(menu.GetEntrySensitive(" e"));
            Assert.IsNull(menu.GetEntrySensitive(" A"));
            Assert.IsNull(menu.GetEntrySensitive(" E"));
            Assert.IsNotNull(menu.GetEntrySensitive("a-123"));
            Assert.IsNull(menu.GetEntrySensitive("e-123"));
            Assert.IsNull(menu.GetEntrySensitive("A-123"));
            Assert.IsNull(menu.GetEntrySensitive("E-123"));
        }

        [TestMethod]
        public void MenuFile_GetStringSensitive_1()
        {
            string defaultValue = "default value";
            MenuFile menu = _GetMenu();

            Assert.AreEqual("Comment for a", menu.GetStringSensitive("a"));
            Assert.AreEqual(defaultValue, menu.GetStringSensitive("d", defaultValue));
        }

        [TestMethod]
        public void MenuFile_ParseLocalFile_1()
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
        public void MenuFile_ParseServerResponse_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendAnsi("a").NewLine()
                .AppendAnsi("Comment for a").NewLine()
                .AppendAnsi("b").NewLine()
                .AppendAnsi("Comment for b").NewLine()
                .AppendAnsi("c").NewLine()
                .AppendAnsi("Comment for c").NewLine()
                .AppendAnsi(MenuFile.StopMarker).NewLine();
            IrbisConnection connection = new IrbisConnection();
            byte[][] query = { new byte[0], new byte[0] };
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );
            MenuFile menu = MenuFile.ParseServerResponse(response);
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("Comment for a", menu.GetString("a"));
            Assert.AreEqual("Comment for b", menu.GetString("b"));
            Assert.AreEqual("Comment for c", menu.GetString("c"));
            Assert.IsNull(menu.GetString("q"));
        }

        [TestMethod]
        public void MenuFile_ParseServerResponse_2()
        {
            string response = "a\nComment for a\nb\nComment for b\nc\nComment for c\n*****";
            MenuFile menu = MenuFile.ParseServerResponse(response);
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("Comment for a", menu.GetString("a"));
            Assert.AreEqual("Comment for b", menu.GetString("b"));
            Assert.AreEqual("Comment for c", menu.GetString("c"));
            Assert.IsNull(menu.GetString("q"));
        }

        [TestMethod]
        public void MenuFile_ParseServerResponse_3()
        {
            string response = "a\nComment for a\nb\nComment for b\nc\nComment for c\n";
            MenuFile menu = MenuFile.ParseServerResponse(response);
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("Comment for a", menu.GetString("a"));
            Assert.AreEqual("Comment for b", menu.GetString("b"));
            Assert.AreEqual("Comment for c", menu.GetString("c"));
            Assert.IsNull(menu.GetString("q"));
        }

        [TestMethod]
        public void MenuFile_ParseServerResponse_4()
        {
            string response = "a\nComment for a\nb\nComment for b\nc\nComment for c";
            MenuFile menu = MenuFile.ParseServerResponse(response);
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("Comment for a", menu.GetString("a"));
            Assert.AreEqual("Comment for b", menu.GetString("b"));
            Assert.AreEqual("Comment for c", menu.GetString("c"));
            Assert.IsNull(menu.GetString("q"));
        }

        [TestMethod]
        public void MenuFile_ReadFromServer_1()
        {
            string response = "a\nComment for a\nb\nComment for b\nc\nComment for c\n*****";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(response);
            IIrbisConnection connection = mock.Object;
            FileSpecification specification
                = new FileSpecification(IrbisPath.MasterFile, "IBIS", "any.mnu");
            MenuFile menu = MenuFile.ReadFromServer(connection, specification);
            Assert.IsNotNull(menu);
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("Comment for a", menu.GetString("a"));
            Assert.AreEqual("Comment for b", menu.GetString("b"));
            Assert.AreEqual("Comment for c", menu.GetString("c"));
            Assert.IsNull(menu.GetString("q"));
        }

        [TestMethod]
        public void MenuFile_ReadFromServer_2()
        {
            string response = null;
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(response);
            IIrbisConnection connection = mock.Object;
            FileSpecification specification
                = new FileSpecification(IrbisPath.MasterFile, "IBIS", "any.mnu");
            MenuFile menu = MenuFile.ReadFromServer(connection, specification);
            Assert.IsNull(menu);
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

        [TestMethod]
        public void MenuFile_SortEntries_1()
        {
            MenuFile menu = new MenuFile()
                .Add("2", "2")
                .Add("1", "3")
                .Add("3", "1");

            MenuEntry[] sorted = menu.SortEntries(MenuSort.None);
            Assert.AreEqual(3, sorted.Length);
            Assert.AreEqual("2", sorted[0].Code);
            Assert.AreEqual("1", sorted[1].Code);
            Assert.AreEqual("3", sorted[2].Code);

            sorted = menu.SortEntries(MenuSort.ByCode);
            Assert.AreEqual(3, sorted.Length);
            Assert.AreEqual("1", sorted[0].Code);
            Assert.AreEqual("2", sorted[1].Code);
            Assert.AreEqual("3", sorted[2].Code);

            sorted = menu.SortEntries(MenuSort.ByComment);
            Assert.AreEqual(3, sorted.Length);
            Assert.AreEqual("3", sorted[0].Code);
            Assert.AreEqual("2", sorted[1].Code);
            Assert.AreEqual("1", sorted[2].Code);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MenuFile_SortEntries_2()
        {
            MenuFile menu = _GetMenu();
            menu.SortEntries((MenuSort)333);
        }

        [TestMethod]
        public void MenuFile_ToText_1()
        {
            MenuFile menu = _GetMenu();
            string expected = "a\nComment for a\nb\nComment for b\nc\nComment for c\n*****\n";
            string actual = menu.ToText().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuFile_ToString_1()
        {
            string fileName = "menu.mnu";
            MenuFile menu = new MenuFile
            {
                FileName = fileName
            };
            Assert.AreEqual(fileName, menu.ToString());
        }
    }
}
