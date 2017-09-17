using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AM.Runtime;

using JetBrains.Annotations;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Menus;

using Newtonsoft.Json;

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class MenuUtilityTest
    {
        [TestMethod]
        public void MenuUtility_Add_1()
        {
            MenuFile menu = new MenuFile();
            menu.Add("a", 1);
            Assert.AreEqual(1, menu.GetValue("a", 0));
        }

        [TestMethod]
        public void MenuUtility_Add_2()
        {
            MenuFile menu = new MenuFile();
            menu.Add<object>("a", null);
            Assert.AreEqual(string.Empty, menu.GetString("a"));
        }

        [TestMethod]
        public void MenuUtility_CollectStrings_1()
        {
            MenuFile menu = new MenuFile();
            string[] actual = menu.CollectStrings("a");
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void MenuUtility_CollectStrings_2()
        {
            MenuFile menu = new MenuFile();
            menu.Add("a", "first");
            string[] actual = menu.CollectStrings("a");
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("first", actual[0]);
        }

        [TestMethod]
        public void MenuUtility_CollectStrings_3()
        {
            MenuFile menu = new MenuFile();
            menu.Add("a", "first");
            menu.Add("a", "second");
            string[] actual = menu.CollectStrings("a");
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("first", actual[0]);
            Assert.AreEqual("second", actual[1]);
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
        public void MenuUtility_ToJson_1()
        {
            MenuFile menu = new MenuFile();
            string actual = menu.ToJson();
            Assert.AreEqual("[]", actual);
        }

        [TestMethod]
        public void MenuUtility_ToJson_2()
        {
            MenuFile menu = _GetMenu();
            string actual = menu.ToJson();
            Assert.AreEqual("[{\"code\":\"a\",\"comment\":\"Comment for a\"},{\"code\":\"b\",\"comment\":\"Comment for b\"},{\"code\":\"c\",\"comment\":\"Comment for c\"}]", actual);
        }

        [TestMethod]
        public void MenuUtility_GetValueSensitive_1()
        {
            MenuFile menu = new MenuFile();
            menu.Add("a", 1);
            Assert.AreEqual(1, menu.GetValueSensitive("a", 0));
        }

        [TestMethod]
        public void MenuUtility_FromJson_1()
        {
            MenuFile menu = MenuUtility.FromJson("[{\"code\":\"a\",\"comment\":\"Comment for a\"},{\"code\":\"b\",\"comment\":\"Comment for b\"},{\"code\":\"c\",\"comment\":\"Comment for c\"}]");
            Assert.AreEqual(3, menu.Entries.Count);
            Assert.AreEqual("a", menu.Entries[0].Code);
            Assert.AreEqual("Comment for a", menu.Entries[0].Comment);
            Assert.AreEqual("b", menu.Entries[1].Code);
            Assert.AreEqual("Comment for b", menu.Entries[1].Comment);
            Assert.AreEqual("c", menu.Entries[2].Code);
            Assert.AreEqual("Comment for c", menu.Entries[2].Comment);
        }

        [TestMethod]
        public void MenuUtility_ToXml_1()
        {
            MenuFile menu = _GetMenu();
            string actual = menu.ToXml();
            Assert.AreEqual("<menu><entry code=\"a\" comment=\"Comment for a\" /><entry code=\"b\" comment=\"Comment for b\" /><entry code=\"c\" comment=\"Comment for c\" /></menu>", actual);
        }

        [TestMethod]
        public void MenuUtility_ToTree_1()
        {
            MenuFile menu = new MenuFile();
            menu.Add("1", "First");
            menu.Add("1.1", "First first");
            menu.Add("1.1.1", "First first first");
            menu.Add("1.1.2", "First first second");
            menu.Add("1.2", "First second");
            menu.Add("1.2.1", "First second first");
            menu.Add("2", "Second");
            IrbisTreeFile tree = menu.ToTree();
            Assert.AreEqual(2, tree.Roots.Count);
        }
        [TestMethod]
        public void MenuUtility_ToTree_2()
        {
            MenuFile menu = new MenuFile();
            menu.Add("1", "First");
            menu.Add("1.1", "First first");
            menu.Add("1.1.1", "First first first");
            menu.Add("1.1.2", "First first second");
            menu.Add("1.2", "First second");
            menu.Add("1.2.1", "First second first");
            menu.Add("2", "Second");
            menu.Add("1", "Another first");
            IrbisTreeFile tree = menu.ToTree();
            Assert.AreEqual(3, tree.Roots.Count);
        }

    }
}
