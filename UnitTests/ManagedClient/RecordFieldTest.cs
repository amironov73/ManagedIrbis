using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;
using Newtonsoft.Json.Linq;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class RecordFieldTest
    {
        [TestMethod]
        public void TestRecordFieldConstruction1()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(RecordField.NoTag, field.Tag);
            Assert.AreEqual(null, field.Value);
            Assert.AreEqual(0,field.SubFields.Count);
        }

        [TestMethod]
        public void TestRecordFieldConstruction2()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(field, field.SubFields.Field);
        }

        [TestMethod]
        public void TestRecordFieldAddSubField()
        {
            RecordField field = new RecordField();
            field.AddSubField('a', "Value");
            Assert.AreEqual(field, field.SubFields[0].Field);
        }

        private void _TestSerialization
            (
                RecordField field1
            )
        {
            byte[] bytes = field1.SaveToMemory();

            RecordField field2 = bytes
                .RestoreObjectFromMemory<RecordField>();

            Assert.AreEqual
                (
                    0,
                    RecordField.Compare
                    (
                        field1,
                        field2
                    )
                );
        }

        [TestMethod]
        public void TestRecordFieldSerialization()
        {
            _TestSerialization
                (
                    new RecordField()
                );
            _TestSerialization
                (
                    new RecordField("100")
                );
            _TestSerialization
                (
                    new RecordField("199", "Hello")
                );
            _TestSerialization
                (
                    new RecordField
                        (
                            "200",
                            new SubField('a', "Hello")
                        )
                );
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField("200", "Значение");

            result.AddSubField('a', "Заглавие");
            result.AddSubField('e', "подзаголовочные");
            result.AddSubField('f', "об ответственности");

            return result;
        }

        [TestMethod]
        public void TestRecordFieldToString()
        {
            RecordField field = _GetField();

            string actual = field.ToString();
            int result = string.CompareOrdinal
                (
                    "200#Значение^aЗаглавие^eподзаголовочные^fоб ответственности",
                    actual
                );

            Assert.AreEqual
                (
                    0,
                    result
                );
        }

        [TestMethod]
        public void TestRecordFieldToText()
        {
            RecordField field = _GetField();

            string actual = field.ToText();
            Assert.AreEqual
                (
                    "Значение^aЗаглавие^eподзаголовочные^fоб ответственности",
                    actual
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestRecordFieldReadOnly1()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            field.Value = "New value";
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestRecordFieldReadOnly2()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            field.AddSubField('a', "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestRecordFieldReadOnly3()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            SubField subField = field.SubFields[0];
            subField.Value = "New value";
        }

        [TestMethod]
        public void TestRecordFieldToJObject()
        {
            RecordField field = _GetField();

            JObject jObject = field.ToJObject();

            Assert.AreEqual("200", jObject["tag"]);
            Assert.AreEqual("Заглавие", jObject["subfields"][0]["value"]);
        }

        [TestMethod]
        public void TestRecordFieldToJson()
        {
            RecordField field = _GetField();

            string actual = field.ToJson()
                .Replace("\r","").Replace("\n","")
                .Replace("\"", "'");
            const string expected = "{'tag':'200','value':'Значение','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочные'},{'code':'f','value':'об ответственности'}]}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRecordFieldFromJObject()
        {
            JObject jObject = JObject.Parse(@"{""tag"": ""200"","
+@"""subfields"": []}");

            RecordField field = RecordFieldUtility.FromJObject(jObject);

            Assert.AreEqual("200", field.Tag);
        }

        [TestMethod]
        public void TestRecordFieldFromJson()
        {
            string text = "{'tag':'200', 'indicator1':'1', 'value':'Значение','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочные'},{'code':'f','value':'об ответственности'}]}"
                .Replace("'", "\"");


            RecordField field = RecordFieldUtility.FromJson(text);

            Assert.AreEqual("200", field.Tag);
            Assert.AreEqual(3, field.SubFields.Count);
        }

        [TestMethod]
        public void TestRecordFieldToXml()
        {
            RecordField field = _GetField();

            string actual = field.ToXml()
                .Replace("\r", "").Replace("\n","");
            const string expected = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<field xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" tag=""200"" value=""Значение"">"
+@"  <subfield code=""a"" value=""Заглавие"" />"
+@"  <subfield code=""e"" value=""подзаголовочные"" />"
+@"  <subfield code=""f"" value=""об ответственности"" />"
+@"</field>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRecordFieldFromXml()
        {
            const string text = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<field xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" tag=""200"" value=""Значение"">"
+@"  <subfield code=""a"" value=""Заглавие"" />"
+@"  <subfield code=""e"" value=""подзаголовочные"" />"
+@"  <subfield code=""f"" value=""об ответственности"" />"
+@"</field>";
            RecordField field = RecordFieldUtility.FromXml(text);

            Assert.AreEqual("200", field.Tag);
            Assert.AreEqual(3, field.SubFields.Count);
        }
    }
}
