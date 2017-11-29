using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Worksheet;

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class WsFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private WsFile _GetFile()
        {
            return new WsFile
            {
                Name = "pazk31.ws",
                Pages =
                {
                    new WorksheetPage
                    {
                        Name = "Дублетность",
                        Items =
                        {
                            new WorksheetItem
                            {
                                Tag = "700",
                                Title = "1-й  автор - Заголовок описания",
                                EditMode = "5",
                                InputInfo = "701d.wss"
                            },
                            new WorksheetItem
                            {
                                Tag = "710",
                                Title = "1-й коллектив (организация) - заголовок описания",
                                EditMode = "5",
                                InputInfo = "710dk.wss"
                            }
                        }
                    },
                    new WorksheetPage
                    {
                        Name = "Расширенное",
                        Items =
                        {
                            new WorksheetItem
                            {
                                Tag = "300",
                                Title = "Общие примечания",
                                EditMode = "0"
                            },
                            new WorksheetItem
                            {
                                Tag = "912",
                                Title = "Примечания о языке",
                                EditMode = "0"
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void WsFile_Construction_1()
        {
            WsFile file = new WsFile();
            Assert.IsNull(file.Name);
            Assert.IsNotNull(file.Pages);
            Assert.AreEqual(0, file.Pages.Count);
        }

        private void _TestSerialization
            (
                [NotNull] WsFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            WsFile second = bytes
                .RestoreObjectFromMemory<WsFile>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Pages.Count, second.Pages.Count);
            for (int i = 0; i < first.Pages.Count; i++)
            {
                Assert.AreEqual(first.Pages[i].Name, second.Pages[i].Name);
                Assert.AreEqual(first.Pages[i].Items.Count, second.Pages[i].Items.Count);
            }
        }

        [TestMethod]
        public void WsFile_ReadLocalFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "pazk31.ws"
                );
            WsFile file = WsFile.ReadLocalFile(fileName);

            Assert.IsNotNull(file.Pages);
            Assert.AreEqual("pazk31.ws", file.Name);
            Assert.AreEqual(9, file.Pages.Count);

            _TestSerialization(file);
        }

        [TestMethod]
        public void WsFile_ReadLocalFile_2()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "pazk42.ws"
                );
            WsFile file = WsFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            file = WsFile.FixupLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi,
                    file
                );

            Assert.IsNotNull(file.Pages);
            Assert.AreEqual("pazk42.ws", file.Name);
            Assert.AreEqual(12, file.Pages.Count);

            _TestSerialization(file);
        }

        [TestMethod]
        public void WsFile_ReadFromServer_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "pazk31.ws"
                    );
                WsFile file = WsFile.ReadFromServer(provider, specification);
                Assert.IsNotNull(file);
                Assert.AreEqual("pazk31.ws", file.Name);
                Assert.AreEqual(9, file.Pages.Count);
            }
        }

        [TestMethod]
        public void WsFile_ReadFromServer_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "nosuchfile.ws"
                    );
                WsFile file = WsFile.ReadFromServer(provider, specification);
                Assert.IsNull(file);
            }
        }

        [TestMethod]
        public void WsFile_ReadFromServer_3()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "pazk42.ws"
                    );
                WsFile file = WsFile.ReadFromServer(provider, specification);
                Assert.IsNotNull(file);
                Assert.AreEqual("pazk42.ws", file.Name);
                Assert.AreEqual(12, file.Pages.Count);
            }
        }

        [TestMethod]
        public void WsFile_Verify_1()
        {
            WsFile file = new WsFile();
            Assert.IsTrue(file.Verify(false));

            file = _GetFile();
            Assert.IsTrue(file.Verify(false));
        }

        [TestMethod]
        public void WsFile_ToXml_1()
        {
            WsFile file = new WsFile();
            Assert.AreEqual("<worksheet />", XmlUtility.SerializeShort(file));

            file = _GetFile();
            Assert.AreEqual("<worksheet name=\"pazk31.ws\"><pages><page><name>Дублетность</name><item><tag>700</tag><title>1-й  автор - Заголовок описания</title><input-mode>5</input-mode><input-info>701d.wss</input-info></item><item><tag>710</tag><title>1-й коллектив (организация) - заголовок описания</title><input-mode>5</input-mode><input-info>710dk.wss</input-info></item></page><page><name>Расширенное</name><item><tag>300</tag><title>Общие примечания</title><input-mode>0</input-mode></item><item><tag>912</tag><title>Примечания о языке</title><input-mode>0</input-mode></item></page></pages></worksheet>", XmlUtility.SerializeShort(file));
        }

        [TestMethod]
        public void WsFile_ToJson_1()
        {
            WsFile file = new WsFile();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(file));

            file = _GetFile();
            Assert.AreEqual("{'name':'pazk31.ws','pages':[{'name':'Дублетность','items':[{'tag':'700','title':'1-й  автор - Заголовок описания','input-mode':'5','input-info':'701d.wss'},{'tag':'710','title':'1-й коллектив (организация) - заголовок описания','input-mode':'5','input-info':'710dk.wss'}]},{'name':'Расширенное','items':[{'tag':'300','title':'Общие примечания','input-mode':'0'},{'tag':'912','title':'Примечания о языке','input-mode':'0'}]}]}", JsonUtility.SerializeShort(file));
        }

        [TestMethod]
        public void WsFile_ToString_1()
        {
            WsFile file = new WsFile();
            Assert.AreEqual("(null)", file.ToString());

            file = _GetFile();
            Assert.AreEqual("pazk31.ws", file.ToString());
        }
    }
}
