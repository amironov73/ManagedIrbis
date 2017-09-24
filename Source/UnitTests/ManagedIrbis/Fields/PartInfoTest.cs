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
    public class PartInfoTest
    {
        private PartInfo _GetPart()
        {
            return new PartInfo
            {
                SecondLevelNumber = "Ч. 2",
                SecondLevelTitle = "Отрочество"
            };
        }

        private RecordField _GetField()
        {
            return new RecordField(PartInfo.Tag)
                .AddSubField('h', "Ч. 2")
                .AddSubField('i', "Отрочество");
        }

        [TestMethod]
        public void PartInfo_Construction_1()
        {
            PartInfo part = new PartInfo();
            Assert.IsNull(part.SecondLevelNumber);
            Assert.IsNull(part.SecondLevelTitle);
            Assert.IsNull(part.ThirdLevelNumber);
            Assert.IsNull(part.ThirdLevelTitle);
            Assert.IsNull(part.Role);
            Assert.IsNull(part.UnknownSubFields);
            Assert.IsNull(part.Field);
            Assert.IsNull(part.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] PartInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            PartInfo second = bytes.RestoreObjectFromMemory<PartInfo>();
            Assert.AreEqual(first.SecondLevelNumber, second.SecondLevelNumber);
            Assert.AreEqual(first.SecondLevelTitle, second.SecondLevelTitle);
            Assert.AreEqual(first.ThirdLevelNumber, second.ThirdLevelNumber);
            Assert.AreEqual(first.ThirdLevelTitle, second.ThirdLevelTitle);
            Assert.AreEqual(first.Role, second.Role);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void PartInfo_Serialization_1()
        {
            PartInfo part = new PartInfo();
            _TestSerialization(part);

            part.Field = new RecordField();
            part.UserData = "User data";
            _TestSerialization(part);

            part = _GetPart();
            _TestSerialization(part);
        }

        [TestMethod]
        public void PartInfo_ParseField_1()
        {
            RecordField field = _GetField();
            PartInfo part = PartInfo.ParseField(field);
            Assert.AreSame(field, part.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), part.SecondLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('i'), part.SecondLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), part.ThirdLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), part.ThirdLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('u'), part.Role);
            Assert.IsNotNull(part.UnknownSubFields);
            Assert.AreEqual(0, part.UnknownSubFields.Length);
            Assert.IsNull(part.UserData);
        }

        [TestMethod]
        public void PartInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            PartInfo[] part = PartInfo.ParseRecord(record);
            Assert.AreEqual(1, part.Length);
            Assert.AreSame(field, part[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), part[0].SecondLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('i'), part[0].SecondLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), part[0].ThirdLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), part[0].ThirdLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('u'), part[0].Role);
            Assert.IsNotNull(part[0].UnknownSubFields);
            Assert.AreEqual(0, part[0].UnknownSubFields.Length);
            Assert.IsNull(part[0].UserData);
        }

        [TestMethod]
        public void PartInfo_ToField_1()
        {
            PartInfo part = _GetPart();
            RecordField field = part.ToField();
            Assert.AreEqual(PartInfo.Tag, field.Tag);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual(part.SecondLevelNumber, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(part.SecondLevelTitle, field.GetFirstSubFieldValue('i'));
            Assert.AreEqual(part.ThirdLevelNumber, field.GetFirstSubFieldValue('k'));
            Assert.AreEqual(part.ThirdLevelTitle, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(part.Role, field.GetFirstSubFieldValue('u'));
        }

        [TestMethod]
        public void PartInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(PartInfo.Tag)
                .AddSubField('h', "???")
                .AddSubField('i', "???");
            PartInfo part = _GetPart();
            part.ApplyToField(field);
            Assert.AreEqual(PartInfo.Tag, field.Tag);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual(part.SecondLevelNumber, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(part.SecondLevelTitle, field.GetFirstSubFieldValue('i'));
            Assert.AreEqual(part.ThirdLevelNumber, field.GetFirstSubFieldValue('k'));
            Assert.AreEqual(part.ThirdLevelTitle, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(part.Role, field.GetFirstSubFieldValue('u'));
        }

        [TestMethod]
        public void PartInfo_Verify_1()
        {
            PartInfo part = new PartInfo();
            Assert.IsFalse(part.Verify(false));

            part = _GetPart();
            Assert.IsTrue(part.Verify(false));
        }

        [TestMethod]
        public void PartInfo_ToXml_1()
        {
            PartInfo part = new PartInfo();
            Assert.AreEqual("<part />", XmlUtility.SerializeShort(part));

            part = _GetPart();
            Assert.AreEqual("<part><secondLevelNumber>Ч. 2</secondLevelNumber><secondLevelTitle>Отрочество</secondLevelTitle></part>", XmlUtility.SerializeShort(part));
        }

        [TestMethod]
        public void PartInfo_ToJson_1()
        {
            PartInfo part = new PartInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(part));

            part = _GetPart();
            Assert.AreEqual("{'secondLevelNumber':'Ч. 2','secondLevelTitle':'Отрочество'}", JsonUtility.SerializeShort(part));
        }

        [TestMethod]
        public void PartInfo_ToString_1()
        {
            PartInfo part = new PartInfo();
            Assert.AreEqual("(empty)", part.ToString());

            part = _GetPart();
            Assert.AreEqual("Ч. 2 -- Отрочество", part.ToString());
        }
    }
}
