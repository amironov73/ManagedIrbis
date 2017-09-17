using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AM.Runtime;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Menus;

using Newtonsoft.Json;

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class MenuEntryTest
    {
        [TestMethod]
        public void MenuEntry_OtherEntry_1()
        {
            MenuEntry thisEntry = new MenuEntry();
            Assert.IsNull(thisEntry.OtherEntry);
            MenuEntry otherEntry = new MenuEntry();
            thisEntry.OtherEntry = otherEntry;
            Assert.AreEqual(otherEntry, thisEntry.OtherEntry);
        }

        private static void _TestSerialization
            (
                [NotNull] MenuEntry first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MenuEntry second = bytes
                .RestoreObjectFromMemory<MenuEntry>();

            Assert.AreEqual(first.Code, second.Code);
            Assert.AreEqual(first.Comment, second.Comment);
            Assert.IsNull(second.OtherEntry);
        }

        [TestMethod]
        public void MenuEntry_Serialization_1()
        {
            MenuEntry entry = new MenuEntry();
            _TestSerialization(entry);
            entry.Code = "A";
            _TestSerialization(entry);
            entry.Comment = "B";
            _TestSerialization(entry);
        }

        private static void _TestJson
            (
                MenuEntry entry,
                string expected
            )
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, entry);
            string actual = writer.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuEntry_ToJson_1()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code",
                Comment = "The comment"
            };
            _TestJson(entry, "{\"code\":\"The code\",\"comment\":\"The comment\"}");
        }

        [TestMethod]
        public void MenuEntry_ToJson_2()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code"
            };
            _TestJson(entry, "{\"code\":\"The code\"}");
        }

        [TestMethod]
        public void MenuEntry_ToJson_3()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code",
                OtherEntry = new MenuEntry()
            };
            _TestJson(entry, "{\"code\":\"The code\"}");
        }

        private static void _TestXml
            (
                MenuEntry entry,
                string expected
            )
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };
            StringBuilder output = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MenuEntry));
                serializer.Serialize(writer, entry, namespaces);
            }
            string actual = output.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuEntry_ToXml_1()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code",
                Comment = "The comment"
            };
            _TestXml(entry, "<entry code=\"The code\" comment=\"The comment\" />");
        }

        [TestMethod]
        public void MenuEntry_ToXml_2()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code"
            };
            _TestXml(entry, "<entry code=\"The code\" />");
        }

        [TestMethod]
        public void MenuEntry_ToXml_3()
        {
            MenuEntry entry = new MenuEntry
            {
                Code = "The code",
                OtherEntry = new MenuEntry()
            };
            _TestXml(entry, "<entry code=\"The code\" />");
        }

        [TestMethod]
        public void MenuEntry_ToString_1()
        {
            MenuEntry entry = new MenuEntry();
            Assert.AreEqual("Code: (null), Comment: (null)", entry.ToString());
            entry.Code = "the code";
            Assert.AreEqual("Code: the code, Comment: (null)", entry.ToString());
            entry.Comment = "the comment";
            Assert.AreEqual("Code: the code, Comment: the comment", entry.ToString());
            entry.OtherEntry = new MenuEntry();
            Assert.AreEqual("Code: the code, Comment: the comment", entry.ToString());
        }
    }
}
