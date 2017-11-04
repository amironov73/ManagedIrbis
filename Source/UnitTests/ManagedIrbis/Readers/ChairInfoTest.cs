using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Readers;

using Moq;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ChairInfoTest
    {
        [TestMethod]
        public void ChairInfo_Construction_1()
        {
            ChairInfo chair = new ChairInfo();
            Assert.IsNull(chair.Code);
            Assert.IsNull(chair.Title);
        }

        [TestMethod]
        public void ChairInfo_Construction_2()
        {
            const string code = "АБ";
            ChairInfo chair = new ChairInfo(code);
            Assert.AreEqual(code, chair.Code);
            Assert.IsNull(chair.Title);
        }

        [TestMethod]
        public void ChairInfo_Construction_3()
        {
            const string code = "АБ", title = "Абонемент";
            ChairInfo chair = new ChairInfo(code, title);
            Assert.AreEqual(code, chair.Code);
            Assert.AreEqual(title, chair.Title);
        }

        [NotNull]
        private string _GetMenuText()
        {
            return "ЧЗ\r\nЧитальный зал\r\n" +
                   "АБ\r\nАбонемент\r\n" +
                   "ИБО\r\nБиблиографический отдел\r\n" +
                   "НМО\r\nМетодический отдел\r\n" +
                   "КХ\r\nКнигохранилище\r\n" +
                   "*****\r\n";
        }

        [TestMethod]
        public void ChairInfo_Parse_1()
        {
            ChairInfo[] chairs = ChairInfo.Parse(_GetMenuText(), true);
            Assert.AreEqual(6, chairs.Length);
            Assert.AreEqual("*", chairs[0].Code);
            Assert.AreEqual("Все подразделения", chairs[0].Title);
            Assert.AreEqual("ЧЗ", chairs[chairs.Length - 1].Code);
            Assert.AreEqual("Читальный зал", chairs[chairs.Length - 1].Title);
        }

        [TestMethod]
        public void ChairInfo_Read_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(_GetMenuText());
            IIrbisConnection connection = mock.Object;
            ChairInfo[] chairs = ChairInfo.Read(connection, "kv.mnu", true);
            Assert.AreEqual(6, chairs.Length);
            Assert.AreEqual("*", chairs[0].Code);
            Assert.AreEqual("Все подразделения", chairs[0].Title);
            Assert.AreEqual("ЧЗ", chairs[chairs.Length - 1].Code);
            Assert.AreEqual("Читальный зал", chairs[chairs.Length - 1].Title);
            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ChairInfo_Read_1a_Exception()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(String.Empty);
            IIrbisConnection connection = mock.Object;
            ChairInfo.Read(connection, "kv.mnu", true);
        }

        [TestMethod]
        public void ChairInfo_Read_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(_GetMenuText());
            IIrbisConnection connection = mock.Object;
            ChairInfo[] chairs = ChairInfo.Read(connection);
            Assert.AreEqual(6, chairs.Length);
            Assert.AreEqual("*", chairs[0].Code);
            Assert.AreEqual("Все подразделения", chairs[0].Title);
            Assert.AreEqual("ЧЗ", chairs[chairs.Length - 1].Code);
            Assert.AreEqual("Читальный зал", chairs[chairs.Length - 1].Title);
            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        private void _TestSerialization
            (
                [NotNull] ChairInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ChairInfo second = bytes
                .RestoreObjectFromMemory<ChairInfo>();

            Assert.AreEqual(first.Code, second.Code);
            Assert.AreEqual(first.Title, second.Title);
        }

        [TestMethod]
        public void ChairInfo_Serialization_1()
        {
            ChairInfo chair = new ChairInfo();
            _TestSerialization(chair);

            chair.Code = "АБ";
            chair.Title = "Абонемент";
            _TestSerialization(chair);
        }

        [TestMethod]
        public void IsbnInfo_ToXml_1()
        {
            ChairInfo chair = new ChairInfo();
            Assert.AreEqual("<chair />", XmlUtility.SerializeShort(chair));

            chair = new ChairInfo("АБ");
            Assert.AreEqual("<chair code=\"АБ\" />", XmlUtility.SerializeShort(chair));

            chair = new ChairInfo("АБ", "Абонемент");
            Assert.AreEqual("<chair code=\"АБ\" title=\"Абонемент\" />", XmlUtility.SerializeShort(chair));
        }

        [TestMethod]
        public void IsbnInfo_ToJson_1()
        {
            ChairInfo chair = new ChairInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(chair));

            chair = new ChairInfo("АБ");
            Assert.AreEqual("{'code':'АБ'}", JsonUtility.SerializeShort(chair));

            chair = new ChairInfo("АБ", "Абонемент");
            Assert.AreEqual("{'code':'АБ','title':'Абонемент'}", JsonUtility.SerializeShort(chair));
        }

        [TestMethod]
        public void ChairInfo_ToString_1()
        {
            ChairInfo chair = new ChairInfo();
            Assert.AreEqual("(null)", chair.ToString());

            chair = new ChairInfo("АБ");
            Assert.AreEqual("АБ", chair.ToString());

            chair = new ChairInfo("АБ", "Абонемент");
            Assert.AreEqual("АБ - Абонемент", chair.ToString());
        }
    }
}
