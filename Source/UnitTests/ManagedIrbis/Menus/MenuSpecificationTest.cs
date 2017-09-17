using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class MenuSpecificationTest
    {
        private static void _TestSerialization
            (
                [NotNull] MenuSpecification first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MenuSpecification second = bytes
                .RestoreObjectFromMemory<MenuSpecification>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Path, second.Path);
            Assert.AreEqual(first.SortMode, second.SortMode);
        }
        [TestMethod]
        public void MenuSpecification_Serialization_1()
        {
            MenuSpecification specification = new MenuSpecification();
            _TestSerialization(specification);
            specification.FileName = "123.mnu";
            _TestSerialization(specification);
            specification.Database = "IBIS";
            _TestSerialization(specification);
            specification.Path = IrbisPath.MasterFile;
            _TestSerialization(specification);
            specification.SortMode = 2;
            _TestSerialization(specification);
        }

        private static void _TestJson
            (
                MenuSpecification specification,
                string expected
            )
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, specification);
            string actual = writer.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuSpecification_ToJson_1()
        {
            MenuSpecification entry = new MenuSpecification
            {
                FileName = "123.mnu",
                Database = "IBIS",
                Path = IrbisPath.MasterFile,
                SortMode = 2
            };
            _TestJson(entry, "{\"file\":\"123.mnu\",\"db\":\"IBIS\",\"path\":2,\"sort\":2}");
        }

        [TestMethod]
        public void MenuSpecification_ToJson_2()
        {
            MenuSpecification entry = new MenuSpecification
            {
                FileName = "123.mnu",
                Path = IrbisPath.MasterFile,
            };
            _TestJson(entry, "{\"file\":\"123.mnu\",\"path\":2}");
        }

        private static void _TestXml
            (
                MenuSpecification specification,
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
                XmlSerializer serializer = new XmlSerializer(typeof(MenuSpecification));
                serializer.Serialize(writer, specification, namespaces);
            }
            string actual = output.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MenuSpecification_ToXml_1()
        {
            MenuSpecification specification = new MenuSpecification
            {
                FileName = "123.mnu",
                Database = "IBIS",
                Path = IrbisPath.MasterFile,
                SortMode = 2
            };
            _TestXml(specification, "<menu file=\"123.mnu\" db=\"IBIS\" path=\"MasterFile\" sort=\"2\" />");
        }

        [TestMethod]
        public void MenuSpecification_ToXml_2()
        {
            MenuSpecification specification = new MenuSpecification
            {
                FileName = "123.mnu",
                Path = IrbisPath.MasterFile,
            };
            _TestXml(specification, "<menu file=\"123.mnu\" path=\"MasterFile\" />");
        }

        [TestMethod]
        public void MenuSpecification_FromFileSpecification_1()
        {
            FileSpecification fileSpecification
                = new FileSpecification(IrbisPath.MasterFile, "IBIS", "123.mnu");
            MenuSpecification menuSpecification
                = MenuSpecification.FromFileSpecification(fileSpecification);
            Assert.AreEqual(fileSpecification.FileName, menuSpecification.FileName);
            Assert.AreEqual(fileSpecification.Database, menuSpecification.Database);
            Assert.AreEqual(fileSpecification.Path, menuSpecification.Path);
            Assert.AreEqual(0, menuSpecification.SortMode);
        }

        [TestMethod]
        public void MenuSpecification_FromFileSpecification_2()
        {
            FileSpecification fileSpecification
                = new FileSpecification(IrbisPath.System, "123.mnu");
            MenuSpecification menuSpecification
                = MenuSpecification.FromFileSpecification(fileSpecification);
            Assert.AreEqual(fileSpecification.FileName, menuSpecification.FileName);
            Assert.IsNull(menuSpecification.Database);
            Assert.AreEqual(fileSpecification.Path, menuSpecification.Path);
            Assert.AreEqual(0, menuSpecification.SortMode);
        }

        [TestMethod]
        public void MenuSpecification_ToFileSpecification_1()
        {
            MenuSpecification menuSpecification
                = new MenuSpecification
                {
                    FileName = "123.mnu",
                    Database = "IBIS",
                    Path = IrbisPath.MasterFile,
                    SortMode = 2
                };
            FileSpecification fileSpecification
                = menuSpecification.ToFileSpecification();
            Assert.AreEqual(menuSpecification.FileName, fileSpecification.FileName);
            Assert.AreEqual(menuSpecification.Database, fileSpecification.Database);
            Assert.AreEqual(menuSpecification.Path, fileSpecification.Path);
        }

        [TestMethod]
        public void MenuSpecification_ToFileSpecification_2()
        {
            MenuSpecification menuSpecification
                = new MenuSpecification
                {
                    FileName = "123.mnu",
                    Path = IrbisPath.System
                };
            FileSpecification fileSpecification
                = menuSpecification.ToFileSpecification();
            Assert.AreEqual(menuSpecification.FileName, fileSpecification.FileName);
            Assert.IsNull(fileSpecification.Database);
            Assert.AreEqual(menuSpecification.Path, fileSpecification.Path);
        }

        [TestMethod]
        public void MenuSpecification_Parse_1()
        {
            MenuSpecification specification
                = MenuSpecification.Parse("123.mnu");
            Assert.AreEqual("123.mnu", specification.FileName);
            Assert.IsNull(specification.Database);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual(0, specification.SortMode);
        }

        [TestMethod]
        public void MenuSpecification_Parse_2()
        {
            MenuSpecification specification
                = MenuSpecification.Parse("123.mnu\\IBIS");
            Assert.AreEqual("123.mnu", specification.FileName);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual(0, specification.SortMode);
        }

        [TestMethod]
        public void MenuSpecification_Parse_3()
        {
            MenuSpecification specification
                = MenuSpecification.Parse("123.mnu\\IBIS\\2");
            Assert.AreEqual("123.mnu", specification.FileName);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual(2, specification.SortMode);
        }

        [TestMethod]
        public void MenuSpecification_Verify_1()
        {
            MenuSpecification specification = new MenuSpecification();
            Assert.IsFalse(specification.Verify(false));
            specification.FileName = "123.mnu";
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void MenuSpecification_ToString_1()
        {
            MenuSpecification specification = new MenuSpecification();
            Assert.AreEqual("(null)", specification.ToString());
            specification.FileName = "123.mnu";
            Assert.AreEqual("123.mnu", specification.ToString());
        }
    }
}
