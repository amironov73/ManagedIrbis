using System;
using System.Collections.Generic;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class FoundItemTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void FoundItem_Construction_1()
        {
            FoundItem item = new FoundItem();
            Assert.IsNull(item.Text);
            Assert.AreEqual(0, item.Mfn);
        }

        [TestMethod]
        public void FoundItem_ToString_1()
        {
            FoundItem item = new FoundItem();
            Assert.AreEqual
                (
                    "[0] (null)",
                    item.ToString()
                );

            item.Text = string.Empty;
            Assert.AreEqual
                (
                    "[0] (empty)",
                    item.ToString()
                );

            item.Text = "Hello";
            Assert.AreEqual
                (
                    "[0] Hello",
                    item.ToString()
                );
        }

        [TestMethod]
        public void FoundItem_ParseLine_1()
        {
            FoundItem item = FoundItem.ParseLine("123#Hello");
            Assert.AreEqual(123, item.Mfn);
            Assert.AreEqual("Hello", item.Text);
        }

        [TestMethod]
        public void FoundItem_ConvertToMfn_1()
        {
            List<FoundItem> items = new List<FoundItem>()
            {
                new FoundItem {Mfn = 1, Text = "One"},
                new FoundItem {Mfn = 2, Text = "Two"},
                new FoundItem {Mfn = 3, Text = "Three"},
                new FoundItem {Mfn = 4, Text = "Four"},
                new FoundItem {Mfn = 5, Text = "Five"}
            };
            int[] mfns = FoundItem.ConvertToMfn(items);
            Assert.AreEqual(5, mfns.Length);
        }

        [TestMethod]
        public void FoundItem_ConvertToText_1()
        {
            List<FoundItem> items = new List<FoundItem>()
            {
                new FoundItem {Mfn = 1, Text = "One"},
                new FoundItem {Mfn = 2, Text = "Two"},
                new FoundItem {Mfn = 3, Text = "Three"},
                new FoundItem {Mfn = 4, Text = "Four"},
                new FoundItem {Mfn = 5, Text = "Five"}
            };
            string[] lines = FoundItem.ConvertToText(items);
            Assert.AreEqual(5, lines.Length);
        }

        [TestMethod]
        public void FoundItem_Verify_1()
        {
            FoundItem item = new FoundItem();
            Assert.IsFalse(item.Verify(false));

            item.Mfn = 1;
            Assert.IsFalse(item.Verify(false));

            item.Text = "Hello";
            Assert.IsTrue(item.Verify(true));
        }

        private void _TestSerialization
            (
                FoundItem first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FoundItem second
                = bytes.RestoreObjectFromMemory<FoundItem>();

            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Record, second.Record); // Hmmm...
            Assert.AreEqual(first.Selected, second.Selected);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void FoundItem_Serialization_1()
        {
            FoundItem item = new FoundItem();
            _TestSerialization(item);

            item.UserData = "User data";
            _TestSerialization(item);

            item.Mfn = 1;
            _TestSerialization(item);

            item.Text = "Hello";
            _TestSerialization(item);
        }

        [TestMethod]
        public void FoundItem_ToXml_1()
        {
            FoundItem item = new FoundItem();
            Assert.AreEqual("<item mfn=\"0\" />", XmlUtility.SerializeShort(item));

            item.Mfn = 1;
            Assert.AreEqual("<item mfn=\"1\" />", XmlUtility.SerializeShort(item));

            item.Text = "Hello";
            Assert.AreEqual("<item text=\"Hello\" mfn=\"1\" />", XmlUtility.SerializeShort(item));
        }

        [TestMethod]
        public void FoundItem_ToJson_1()
        {
            FoundItem item = new FoundItem();
            Assert.AreEqual("{'mfn':0}", JsonUtility.SerializeShort(item));

            item.Mfn = 1;
            Assert.AreEqual("{'mfn':1}", JsonUtility.SerializeShort(item));

            item.Text = "Hello";
            Assert.AreEqual("{'text':'Hello','mfn':1}", JsonUtility.SerializeShort(item));
        }

        [TestMethod]
        public void FoundItem_ParseServerResponse_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                //.AppendUtf("0")
                //.NewLine()
                //.AppendUtf("26544")
                //.NewLine()
                .AppendUtf("47#Еремеева, Т. В. Освоение генофонда некоторых видов Armeniaca Scop. в Предбайкалье : Автореф. дисс. ... канд. наук / Т. В. Еремеева, 2000. - 19 с.")
                .NewLine()
                .AppendUtf("208#Д'Оссон К. История монголов : От Чингиз-хана до Тамерлана. Т. 1, 1937. - 252 с.")
                .NewLine()
                .AppendUtf("3762#Памятники архитектуры пригородов Ленинграда : [альбом-монография] / А. Н. Петров [и др.] ; авт. предисл. Г. Н. Булдаков ; илл. М. Е. Васильева ; науч. ред. И. А. Бартенев, 1983. - 615 с. (Введено оглавление)")
                .NewLine()
                .AppendUtf("4142#Николай Михайлович Карамзин : Указатель трудов, литературы о жизни и творчестве. 1883-1993 / Отв. ред. А.А.Либерман, 1999. - 447 с.")
                .NewLine()
                .AppendUtf("4163#Lamb, Sydney M. Pathways of the Brain : The Neurocognitive Basis of Language / S. M.Lamb, 1999. - 416 p.")
                .NewLine()
                .AppendUtf("4464#Doulton, Anne-Marie. The Arts Funding Guide / A.-M. Doulton, 1994. - 348 р")
                .NewLine()
                .AppendUtf("4808#Офис-97 [Электронный ресурс]  : сборник. Т.2 (русский). - 1 CD-ROM (Введено оглавление)")
                .NewLine()
                .AppendUtf("5072#Елманова, Н. Технологии IBM. Часть 4. IBM WebSphere Application Server / Н. Елманова // Компьютер пресс. - 2001. - № : 12.- C.171-174.")
                .NewLine()
                .AppendUtf("5311#Лабораторный практикум по информатике для студентов очной, очно-заочной и заочной форм обучения. Ч. 1. Windows 95 (98), Word, Excel, Access / Ю.А. Агафонов, Т.П. Бояринцева, Т.Н. Сержант и др, 2001. - 69 с.")
                .NewLine()
                .AppendUtf("5695#Джиджилава, З. Lankia. Тени предков / З. Джиджилава // Авторевю. - 2001. - № : 24.- С.44-45.")
                .NewLine();
            byte[][] rawRequest = { new byte[0], new byte[0] };
            byte[] rawAnswer = builder.Encode();
            IrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, rawAnswer, rawRequest, true);
            List<FoundItem> found = FoundItem.ParseServerResponse(response, 0);
            Assert.AreEqual(10, found.Count);
            Assert.AreEqual(47, found[0].Mfn);
            Assert.AreEqual("Еремеева, Т. В. Освоение генофонда некоторых видов Armeniaca Scop. в Предбайкалье : Автореф. дисс. ... канд. наук / Т. В. Еремеева, 2000. - 19 с.", found[0].Text);
        }
    }
}
