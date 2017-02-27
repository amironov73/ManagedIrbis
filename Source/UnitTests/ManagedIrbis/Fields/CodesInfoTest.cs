using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CodesInfoTest
    {
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
            Assert.IsNull(codes.PurposeCode1);
            Assert.IsNull(codes.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_Parse_1()
        {
            RecordField field = new RecordField("900", "^Ta^B05^C454^219^Xm^Ye");
            CodesInfo codes = CodesInfo.Parse(field);
            Assert.AreEqual("a", codes.DocumentType);
            Assert.AreEqual("05", codes.DocumentKind);
            Assert.AreEqual("454", codes.DocumentCharacter1);
            Assert.AreEqual("19", codes.DocumentCharacter2);
            Assert.IsNull(codes.DocumentCharacter3);
            Assert.IsNull(codes.DocumentCharacter4);
            Assert.IsNull(codes.DocumentCharacter5);
            Assert.IsNull(codes.DocumentCharacter6);
            Assert.AreEqual("m", codes.PurposeCode1);
            Assert.AreEqual("e", codes.PurposeCode2);
            Assert.IsNull(codes.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_ToField_1()
        {
            CodesInfo codes = new CodesInfo
            {
                DocumentType = "a",
                DocumentKind = "05",
                DocumentCharacter1 = "454",
                DocumentCharacter2 = "19",
                PurposeCode1 = "m",
                PurposeCode2 = "e"
            };
            RecordField field = codes.ToField();
            const string expected = "^ta^b05^c454^219^xm^ye";
            string actual = field.ToText();
            Assert.AreEqual(expected, actual);
        }
    }
}
