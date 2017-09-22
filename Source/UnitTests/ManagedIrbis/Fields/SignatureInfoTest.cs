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
    public class SignatureInfoTest
    {
        private SignatureInfo _GetSignatureInfo()
        {
            SignatureInfo result = new SignatureInfo
            {
                Position = "Директор НТБ",
                Signature = "Т. А. Клеменкова"
            };

            return result;
        }

        [TestMethod]
        public void SignatureInfo_Construction_1()
        {
            SignatureInfo signature = new SignatureInfo();
            Assert.IsNull(signature.Position);
            Assert.IsNull(signature.Signature);
            Assert.IsNull(signature.UnknownSubFields);
            Assert.IsNull(signature.Field);
            Assert.IsNull(signature.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] SignatureInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            SignatureInfo second = bytes.RestoreObjectFromMemory<SignatureInfo>();
            Assert.AreEqual(first.Position, second.Position);
            Assert.AreEqual(first.Signature, second.Signature);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void SignatureInfo_Serialization_1()
        {
            SignatureInfo signature = new SignatureInfo();
            _TestSerialization(signature);

            signature.UserData = "User data";
            signature.Field = new RecordField();
            _TestSerialization(signature);

            signature = _GetSignatureInfo();
            _TestSerialization(signature);
        }

        [TestMethod]
        public void SignatureInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(SignatureInfo.Tag)
                .AddSubField('a', "Зам. директора")
                .AddSubField('b', "Е. Н. Башкирцева");
            SignatureInfo signature = _GetSignatureInfo();
            signature.ApplyToField(field);
            Assert.AreEqual("Директор НТБ", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("Т. А. Клеменкова", field.GetFirstSubFieldValue('b'));
        }

        [TestMethod]
        public void SignatureInfo_ParseField_1()
        {
            RecordField field = new RecordField(SignatureInfo.Tag)
                .AddSubField('a', "Директор НТБ")
                .AddSubField('b', "Т. А. Клеменкова");
            SignatureInfo signature = SignatureInfo.ParseField(field);
            Assert.AreEqual("Директор НТБ", signature.Position);
            Assert.AreEqual("Т. А. Клеменкова", signature.Signature);
        }

        [TestMethod]
        public void SignatureInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field1 = new RecordField(SignatureInfo.Tag)
                .AddSubField('a', "Директор НТБ")
                .AddSubField('b', "Т. А. Клеменкова");
            record.Fields.Add(field1);
            RecordField field2 = new RecordField(SignatureInfo.Tag)
                .AddSubField('a', "Зам. директора")
                .AddSubField('b', "Е. Н. Башкирцева");
            record.Fields.Add(field2);
            SignatureInfo[] signatures = SignatureInfo.ParseRecord(record);
            Assert.AreEqual(2, signatures.Length);
            Assert.AreEqual("Директор НТБ", signatures[0].Position);
            Assert.AreEqual("Т. А. Клеменкова", signatures[0].Signature);
            Assert.AreEqual("Зам. директора", signatures[1].Position);
            Assert.AreEqual("Е. Н. Башкирцева", signatures[1].Signature);
        }

        [TestMethod]
        public void SignatureInfo_ToField_1()
        {
            SignatureInfo signature = _GetSignatureInfo();
            RecordField field = signature.ToField();
            Assert.AreEqual(SignatureInfo.Tag, field.Tag);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual("Директор НТБ", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("Т. А. Клеменкова", field.GetFirstSubFieldValue('b'));
        }

        [TestMethod]
        public void SignatureInfo_ToXml_1()
        {
            SignatureInfo signature = new SignatureInfo();
            Assert.AreEqual("<signatureInfo />", XmlUtility.SerializeShort(signature));

            signature = _GetSignatureInfo();
            Assert.AreEqual("<signatureInfo position=\"Директор НТБ\" signature=\"Т. А. Клеменкова\" />", XmlUtility.SerializeShort(signature));
        }

        [TestMethod]
        public void SignatureInfo_ToJson_1()
        {
            SignatureInfo signature = new SignatureInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(signature));

            signature = _GetSignatureInfo();
            Assert.AreEqual("{'position':'Директор НТБ','signature':'Т. А. Клеменкова'}", JsonUtility.SerializeShort(signature));
        }

        [TestMethod]
        public void SignatureInfo_ToString_1()
        {
            SignatureInfo signature = new SignatureInfo();
            Assert.AreEqual("(null) (null)", signature.ToString());

            signature = _GetSignatureInfo();
            Assert.AreEqual("Директор НТБ Т. А. Клеменкова", signature.ToString());
        }

        [TestMethod]
        public void SignatureInfo_Verify_1()
        {
            SignatureInfo signature = new SignatureInfo();
            Assert.IsFalse(signature.Verify(false));

            signature = _GetSignatureInfo();
            Assert.IsTrue(signature.Verify(false));
        }
    }
}
