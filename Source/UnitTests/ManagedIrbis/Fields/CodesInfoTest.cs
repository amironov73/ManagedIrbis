using AM.Json;
using AM.Runtime;
using AM.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CodesInfoTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private CodesInfo _GetCodes()
        {
            return new CodesInfo
            {
                DocumentType = "a",
                DocumentKind = "05",
                DocumentCharacter1 = "454",
                DocumentCharacter2 = "19",
                PurposeCode1 = "m",
                PurposeCode2 = "e"
            };
        }

        [NotNull]
        private RecordField _GetField()
        {
            return new RecordField(CodesInfo.Tag, "^Ta^B05^C454^219^Xm^Ye");
        }

        private void _Compare
            (
                [NotNull] CodesInfo first,
                [NotNull] CodesInfo second
            )
        {
            Assert.AreEqual(first.DocumentType, second.DocumentType);
            Assert.AreEqual(first.DocumentKind, second.DocumentKind);
            Assert.AreEqual(first.DocumentCharacter1, second.DocumentCharacter1);
            Assert.AreEqual(first.DocumentCharacter2, second.DocumentCharacter2);
            Assert.AreEqual(first.DocumentCharacter3, second.DocumentCharacter3);
            Assert.AreEqual(first.DocumentCharacter4, second.DocumentCharacter4);
            Assert.AreEqual(first.DocumentCharacter5, second.DocumentCharacter5);
            Assert.AreEqual(first.DocumentCharacter6, second.DocumentCharacter6);
            Assert.AreEqual(first.PurposeCode1, second.PurposeCode1);
            Assert.AreEqual(first.PurposeCode2, second.PurposeCode2);
            Assert.AreEqual(first.PurposeCode3, second.PurposeCode3);
            Assert.AreEqual(first.AgeRestrictions, second.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_Constructor_1()
        {
            CodesInfo codes = new CodesInfo();
            Assert.IsNull(codes.DocumentType);
            Assert.IsNull(codes.DocumentKind);
            Assert.IsNull(codes.DocumentCharacter1);
            Assert.IsNull(codes.DocumentCharacter2);
            Assert.IsNull(codes.DocumentCharacter3);
            Assert.IsNull(codes.DocumentCharacter4);
            Assert.IsNull(codes.DocumentCharacter5);
            Assert.IsNull(codes.DocumentCharacter6);
            Assert.IsNull(codes.PurposeCode1);
            Assert.IsNull(codes.PurposeCode2);
            Assert.IsNull(codes.PurposeCode3);
            Assert.IsNull(codes.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_Parse_1()
        {
            RecordField field = _GetField();
            CodesInfo actual = CodesInfo.Parse(field);
            CodesInfo expected = _GetCodes();
            _Compare(expected, actual);
        }

        [TestMethod]
        public void CodesInfo_ToField_1()
        {
            CodesInfo codes = _GetCodes();
            RecordField actual = codes.ToField();
            RecordField expected = _GetField();
            CompareFields(expected, actual);
        }

        [TestMethod]
        public void CodesInfo_ApplyToField_1()
        {
            RecordField actual = new RecordField()
                .AddSubField('t', "o")
                .AddSubField('b', "03")
                .AddSubField('c', "111")
                .AddSubField('2', "91")
                .AddSubField('x', "l")
                .AddSubField('y', "a");
            CodesInfo codes = _GetCodes();
            codes.ApplyToField(actual);
            RecordField expected = _GetField();
            CompareFields(expected, actual);
        }

        private void _TestSerialization
            (
                [NotNull] CodesInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            CodesInfo second = bytes.RestoreObjectFromMemory<CodesInfo>();
            _Compare(first, second);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void CodesInfo_Serialization_1()
        {
            CodesInfo codes = new CodesInfo();
            _TestSerialization(codes);

            codes = _GetCodes();
            codes.Field = new RecordField();
            codes.UserData = "User data";
            _TestSerialization(codes);
        }

        [TestMethod]
        public void CodesInfo_Verify_1()
        {
            CodesInfo codes = new CodesInfo();
            Assert.IsFalse(codes.Verify(false));

            codes = _GetCodes();
            Assert.IsTrue(codes.Verify(false));
        }

        [TestMethod]
        public void CodesInfo_ToXml_1()
        {
            CodesInfo codes = new CodesInfo();
            Assert.AreEqual("<codes />", XmlUtility.SerializeShort(codes));

            codes = _GetCodes();
            Assert.AreEqual("<codes type=\"a\" kind=\"05\" character1=\"454\" character2=\"19\" purpose1=\"m\" purpose2=\"e\" />", XmlUtility.SerializeShort(codes));
        }

        [TestMethod]
        public void CodesInfo_ToJson_1()
        {
            CodesInfo codes = new CodesInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(codes));

            codes = _GetCodes();
            Assert.AreEqual("{'type':'a','kind':'05','character1':'454','character2':'19','purpose1':'m','purpose2':'e'}", JsonUtility.SerializeShort(codes));
        }

        [TestMethod]
        public void CodesInfo_ToString_1()
        {
            CodesInfo codes = new CodesInfo();
            Assert.AreEqual("DocumentType: (null), DocumentKind: (null), DocumentCharacter1: (null)", codes.ToString());

            codes = _GetCodes();
            Assert.AreEqual("DocumentType: a, DocumentKind: 05, DocumentCharacter1: 454", codes.ToString());
        }
    }
}
