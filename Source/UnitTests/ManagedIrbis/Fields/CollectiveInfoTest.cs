using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CollectiveInfoTest
    {
        [NotNull]
        private CollectiveInfo _GetCollective()
        {
            return new CollectiveInfo
            {
                Title = "Иркутский государственный университет",
                Country = "RU",
                Abbreviation = "ИГУ",
                City1 = "Иркутск",
                Department = "Биолого-почвенный факультет",
                Gost = "М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак."
            };
        }

        // ^AИркутский государственный университет^SRU^RИГУ^CИркутск
        // ^BБиолого-почвенный факультет
        // ^7М-во образования и науки Рос. Федерации, ФГБОУ ВПО "Иркут. гос. ун-т", Биол.-почв. фак.
        [NotNull]
        private RecordField _GetField()
        {
            return new RecordField(711)
                .AddSubField('a', "Иркутский государственный университет")
                .AddSubField('s', "RU")
                .AddSubField('r', "ИГУ")
                .AddSubField('c', "Иркутск")
                .AddSubField('b', "Биолого-почвенный факультет")
                .AddSubField('7', "М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак.");
        }

        [TestMethod]
        public void CollectiveInfo_Construction_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            Assert.IsNull(collective.Title);
            Assert.IsNull(collective.Country);
            Assert.IsNull(collective.Abbreviation);
            Assert.IsNull(collective.Number);
            Assert.IsNull(collective.Date);
            Assert.IsNull(collective.City1);
            Assert.IsNull(collective.Department);
            Assert.IsFalse(collective.Characteristic);
            Assert.IsNull(collective.Gost);
            Assert.IsNull(collective.UnknownSubFields);
            Assert.IsNull(collective.Field);
            Assert.IsNull(collective.UserData);
        }

        [TestMethod]
        public void CollectiveInfo_Construction_2()
        {
            CollectiveInfo collective = new CollectiveInfo("Иркутский государственный университет");
            Assert.AreEqual("Иркутский государственный университет", collective.Title);
            Assert.IsNull(collective.Country);
            Assert.IsNull(collective.Abbreviation);
            Assert.IsNull(collective.Number);
            Assert.IsNull(collective.Date);
            Assert.IsNull(collective.City1);
            Assert.IsNull(collective.Department);
            Assert.IsFalse(collective.Characteristic);
            Assert.IsNull(collective.Gost);
            Assert.IsNull(collective.UnknownSubFields);
            Assert.IsNull(collective.Field);
            Assert.IsNull(collective.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] CollectiveInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            CollectiveInfo second = bytes.RestoreObjectFromMemory<CollectiveInfo>();
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Country, second.Country);
            Assert.AreEqual(first.Abbreviation, second.Abbreviation);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Date, second.Date);
            Assert.AreEqual(first.City1, second.City1);
            Assert.AreEqual(first.Department, second.Department);
            Assert.AreEqual(first.Characteristic, second.Characteristic);
            Assert.AreEqual(first.Gost, second.Gost);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void CollectiveInfo_Serialization_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            _TestSerialization(collective);

            collective.Field = new RecordField();
            collective.UserData = "User data";
            _TestSerialization(collective);

            collective = _GetCollective();
            _TestSerialization(collective);
        }

        [TestMethod]
        public void CollectiveInfo_ParseField_1()
        {
            RecordField field = _GetField();
            CollectiveInfo collective = CollectiveInfo.ParseField(field);
            Assert.AreSame(field, collective.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), collective.Title);
            Assert.AreEqual(field.GetFirstSubFieldValue('s'), collective.Country);
            Assert.AreEqual(field.GetFirstSubFieldValue('r'), collective.Abbreviation);
            Assert.AreEqual(field.GetFirstSubFieldValue('n'), collective.Number);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), collective.Date);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), collective.City1);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), collective.Department);
            Assert.IsFalse(collective.Characteristic);
            Assert.AreEqual(field.GetFirstSubFieldValue('7'), collective.Gost);
            Assert.IsNotNull(collective.UnknownSubFields);
            Assert.AreEqual(0, collective.UnknownSubFields.Length);
            Assert.IsNull(collective.UserData);
        }

        [TestMethod]
        public void CollectiveInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            CollectiveInfo[] collective = CollectiveInfo.ParseRecord(record, CollectiveInfo.KnownTags);
            Assert.AreSame(field, collective[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), collective[0].Title);
            Assert.AreEqual(field.GetFirstSubFieldValue('s'), collective[0].Country);
            Assert.AreEqual(field.GetFirstSubFieldValue('r'), collective[0].Abbreviation);
            Assert.AreEqual(field.GetFirstSubFieldValue('n'), collective[0].Number);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), collective[0].Date);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), collective[0].City1);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), collective[0].Department);
            Assert.IsFalse(collective[0].Characteristic);
            Assert.AreEqual(field.GetFirstSubFieldValue('7'), collective[0].Gost);
            Assert.IsNotNull(collective[0].UnknownSubFields);
            Assert.AreEqual(0, collective[0].UnknownSubFields.Length);
            Assert.IsNull(collective[0].UserData);
        }

        [TestMethod]
        public void CollectiveInfo_ToField_1()
        {
            CollectiveInfo collective = _GetCollective();
            RecordField field = collective.ToField(711);
            Assert.AreEqual(6, field.SubFields.Count);
            Assert.AreEqual(711, field.Tag);
            Assert.AreEqual(collective.Title, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(collective.Country, field.GetFirstSubFieldValue('s'));
            Assert.AreEqual(collective.Abbreviation, field.GetFirstSubFieldValue('r'));
            Assert.AreEqual(collective.Number, field.GetFirstSubFieldValue('n'));
            Assert.AreEqual(collective.Date, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(collective.City1, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(collective.Department, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(collective.Gost, field.GetFirstSubFieldValue('7'));
            Assert.AreEqual(collective.Characteristic, field.HaveSubField('x'));
        }

        [TestMethod]
        public void CollectiveInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(711)
                .AddSubField('a', "???")
                .AddSubField('b', "???");
            CollectiveInfo collective = _GetCollective();
            collective.ApplyToField(field);
            Assert.AreEqual(6, field.SubFields.Count);
            Assert.AreEqual(711, field.Tag);
            Assert.AreEqual(collective.Title, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(collective.Country, field.GetFirstSubFieldValue('s'));
            Assert.AreEqual(collective.Abbreviation, field.GetFirstSubFieldValue('r'));
            Assert.AreEqual(collective.Number, field.GetFirstSubFieldValue('n'));
            Assert.AreEqual(collective.Date, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(collective.City1, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(collective.Department, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(collective.Gost, field.GetFirstSubFieldValue('7'));
            Assert.AreEqual(collective.Characteristic, field.HaveSubField('x'));
        }

        [TestMethod]
        public void CollectiveInfo_Verify_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            Assert.IsFalse(collective.Verify(false));

            collective = _GetCollective();
            Assert.IsTrue(collective.Verify(false));
        }

        [TestMethod]
        public void CollectiveInfo_ToXml_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            Assert.AreEqual("<collective />", XmlUtility.SerializeShort(collective));

            collective = _GetCollective();
            Assert.AreEqual("<collective><title>Иркутский государственный университет</title><country>RU</country><abbreviation>ИГУ</abbreviation><city>Иркутск</city><department>Биолого-почвенный факультет</department><gost>М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак.</gost></collective>", XmlUtility.SerializeShort(collective));
        }

        [TestMethod]
        public void CollectiveInfo_ToJson_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(collective));

            collective = _GetCollective();
            Assert.AreEqual("{\'title\':\'Иркутский государственный университет\',\'country\':\'RU\',\'abbreviation\':\'ИГУ\',\'city\':\'Иркутск\',\'department\':\'Биолого-почвенный факультет\',\'gost\':\'М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак.\'}", JsonUtility.SerializeShort(collective));
        }

        [TestMethod]
        public void CollectiveInfo_ToString_1()
        {
            CollectiveInfo collective = new CollectiveInfo();
            Assert.AreEqual("(null)", collective.ToString());

            collective = _GetCollective();
            Assert.AreEqual("Иркутский государственный университет", collective.ToString());
        }
    }
}
