using System;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class SenderInfoTest
    {
        private SenderInfo _GetSender()
        {
            return new SenderInfo
            {
                Sender1 = "Иркутская областная государственная универсальная научная библиотека им. И.И. Молчанова-Сибирского",
                Sender2 = "Отдел комплектования",
                Street = "ул. Лермонтова",
                House = "253",
                City = "г. Иркутск",
                Country = "Россия",
                Postcode = "664033",
                Phone = "+7 (3952) 48-66-80"
            };
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField(SenderInfo.Tag)
                .AddSubField('f', "Иркутская областная государственная универсальная научная библиотека им. И.И. Молчанова-Сибирского")
                .AddSubField('g', "Отдел комплектования")
                .AddSubField('d', "ул. Лермонтова")
                .AddSubField('e', "253")
                .AddSubField('c', "г. Иркутск")
                .AddSubField('b', "Россия")
                .AddSubField('a', "664033")
                .AddSubField('k', "+7 (3952) 48-66-80");

            return result;
        }

        [TestMethod]
        public void SenderInfo_Construction_1()
        {
            SenderInfo sender = new SenderInfo();
            Assert.IsNull(sender.Sender1);
            Assert.IsNull(sender.Sender2);
            Assert.IsNull(sender.Street);
            Assert.IsNull(sender.House);
            Assert.IsNull(sender.City);
            Assert.IsNull(sender.Country);
            Assert.IsNull(sender.Postcode);
            Assert.IsNull(sender.Phone);
            Assert.IsNull(sender.UnknownSubFields);
            Assert.IsNull(sender.Field);
            Assert.IsNull(sender.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] SenderInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            SenderInfo second = bytes.RestoreObjectFromMemory<SenderInfo>();
            Assert.AreEqual(first.Sender1, second.Sender1);
            Assert.AreEqual(first.Sender2, second.Sender2);
            Assert.AreEqual(first.Street, second.Street);
            Assert.AreEqual(first.House, second.House);
            Assert.AreEqual(first.City, second.City);
            Assert.AreEqual(first.Country, second.Country);
            Assert.AreEqual(first.Postcode, second.Postcode);
            Assert.AreEqual(first.Phone, second.Phone);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void SenderInfo_Serialization_1()
        {
            SenderInfo sender = new SenderInfo();
            _TestSerialization(sender);

            sender.UserData = "User data";
            _TestSerialization(sender);

            sender = _GetSender();
            _TestSerialization(sender);
        }

        [TestMethod]
        public void SenderInfo_ParseField_1()
        {
            RecordField field = _GetField();
            SenderInfo sender = SenderInfo.ParseField(field);
            Assert.AreSame(field, sender.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('f'), sender.Sender1);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), sender.Sender2);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), sender.Street);
            Assert.AreEqual(field.GetFirstSubFieldValue('e'), sender.House);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), sender.City);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), sender.Country);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), sender.Postcode);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), sender.Phone);
            Assert.IsNotNull(sender.UnknownSubFields);
        }

        [TestMethod]
        public void SenderInfo_ParseRecord_1()
        {
            RecordField field = _GetField();
            MarcRecord record = new MarcRecord();
            record.Fields.Add(field);
            SenderInfo[] sender = SenderInfo.ParseRecord(record);
            Assert.AreEqual(1, sender.Length);
            Assert.AreSame(field, sender[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('f'), sender[0].Sender1);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), sender[0].Sender2);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), sender[0].Street);
            Assert.AreEqual(field.GetFirstSubFieldValue('e'), sender[0].House);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), sender[0].City);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), sender[0].Country);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), sender[0].Postcode);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), sender[0].Phone);
            Assert.IsNotNull(sender[0].UnknownSubFields);
        }

        [TestMethod]
        public void SenderInfo_ToField_1()
        {
            SenderInfo sender = _GetSender();
            RecordField field = sender.ToField();
            Assert.AreEqual(SenderInfo.Tag, field.Tag);
            Assert.AreEqual(8, field.SubFields.Count);
            Assert.AreEqual(sender.Sender1, field.GetFirstSubFieldValue('f'));
            Assert.AreEqual(sender.Sender2, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(sender.Street, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(sender.House, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(sender.City, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(sender.Country, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(sender.Postcode, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(sender.Phone, field.GetFirstSubFieldValue('k'));
        }

        [TestMethod]
        public void SenderInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(SenderInfo.Tag)
                .AddSubField('a', "???")
                .AddSubField('b', "???");
            SenderInfo sender = _GetSender();
            sender.ApplyToField(field);
            Assert.AreEqual(8, field.SubFields.Count);
            Assert.AreEqual(sender.Sender1, field.GetFirstSubFieldValue('f'));
            Assert.AreEqual(sender.Sender2, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(sender.Street, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(sender.House, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(sender.City, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(sender.Country, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(sender.Postcode, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(sender.Phone, field.GetFirstSubFieldValue('k'));
        }

        [TestMethod]
        public void SenderInfo_Verify_1()
        {
            SenderInfo sender = new SenderInfo();
            Assert.IsFalse(sender.Verify(false));

            sender = _GetSender();
            Assert.IsTrue(sender.Verify(false));
        }

        [TestMethod]
        public void SenderInfo_ToXml_1()
        {
            SenderInfo sender = new SenderInfo();
            Assert.AreEqual("<sender />", XmlUtility.SerializeShort(sender));

            sender = _GetSender();
            Assert.AreEqual("<sender><sender1>Иркутская областная государственная универсальная научная библиотека им. И.И. Молчанова-Сибирского</sender1><sender2>Отдел комплектования</sender2><street>ул. Лермонтова</street><house>253</house><city>г. Иркутск</city><country>Россия</country><postcode>664033</postcode><phone>+7 (3952) 48-66-80</phone></sender>", XmlUtility.SerializeShort(sender));
        }

        [TestMethod]
        public void SenderInfo_ToJson_1()
        {
            SenderInfo sender = new SenderInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(sender));

            sender = _GetSender();
            Assert.AreEqual("{'sender1':'Иркутская областная государственная универсальная научная библиотека им. И.И. Молчанова-Сибирского','sender2':'Отдел комплектования','street':'ул. Лермонтова','house':'253','city':'г. Иркутск','country':'Россия','postcode':'664033','phone':'+7 (3952) 48-66-80'}", JsonUtility.SerializeShort(sender));
        }

        [TestMethod]
        public void SenderInfo_ToString_1()
        {
            SenderInfo sender = new SenderInfo();
            Assert.AreEqual
                (
                    "Индекс: (null)\nГород: (null)\nУлица: (null)\nДом: (null)\nОтправитель: (null)",
                    sender.ToString().DosToUnix()
                );

            sender = _GetSender();
            Assert.AreEqual
                (
                    "Индекс: 664033\nГород: г. Иркутск\nУлица: ул. Лермонтова\nДом: 253\nОтправитель: Иркутская областная государственная универсальная научная библиотека им. И.И. Молчанова-Сибирского",
                    sender.ToString().DosToUnix()
                );
        }
    }
}
