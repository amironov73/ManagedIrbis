using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;
using Newtonsoft.Json.Linq;

namespace UnitTests
{
    [TestClass]
    public class SubFieldTest
    {
        [TestMethod]
        public void TestSubFieldConstructor()
        {
            SubField subField = new SubField();
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.AreEqual(SubField.NoCodeString, subField.CodeString);
            Assert.AreEqual(null, subField.Value);
            Assert.AreEqual("^\0", subField.ToString());

            subField = new SubField('A', "The value");
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual("A", subField.CodeString);
            Assert.AreEqual("The value", subField.Value);
            Assert.AreEqual("^AThe value", subField.ToString());

            SubField clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.CodeString, clone.CodeString);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^AThe value", clone.ToString());
            Assert.AreEqual(0, SubField.Compare(subField, clone));

            subField.SetValue("New value");
            Assert.AreEqual("New value", subField.Value);
            subField.SetValue(null);
            Assert.AreEqual("New value", subField.Value);
        }

        private void _TestSerialization
            (
                params SubField[] subFields
            )
        {
            SubField[] array1 = subFields;
            byte[] bytes = array1.SaveToMemory();

            SubField[] array2 = bytes
                    .RestoreArrayFromMemory<SubField>();

            Assert.AreEqual(array1.Length, array2.Length);
            for (int i = 0; i < array1.Length; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare(array1[i], array2[i])
                    );
            }
        }

        [TestMethod]
        public void TestSubFieldSerialization()
        {
            _TestSerialization(new SubField[0]);
            _TestSerialization(new SubField());
            _TestSerialization(new SubField(), new SubField());
            _TestSerialization(new SubField('a'), new SubField('b'));
            _TestSerialization(new SubField('a', "Hello"),
                new SubField('b', "World"));
        }

        [TestMethod]
        public void TestSubFieldSetValue1()
        {
            SubField subField = new SubField('a')
            {
                Value = "Right Value"
            };
            Assert.AreEqual("Right Value", subField.Value);
        }



        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void TestSubField_SetValue_Exception()
        {
            bool save = SubFieldValue.ThrowOnVerify;
            SubFieldValue.ThrowOnVerify = true;
            try
            {
                SubField subField = new SubField('a')
                {
                    Value = "Wrong^Value"
                };
                Assert.AreEqual("Wrong", subField.Value);
            }
            finally
            {
                SubFieldValue.ThrowOnVerify = save;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestSubFieldReadOnly()
        {
            SubField subField = new SubField('a', "Value", true, null);
            Assert.AreEqual("Value", subField.Value);
            subField.Value = "New value";
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void TestSubFieldToJObject()
        {
            SubField subField = new SubField('a', "Value");

            JObject jObject = subField.ToJObject();
            Assert.AreEqual("a", jObject["code"].ToString());
            Assert.AreEqual("Value", jObject["value"].ToString());
        }

        [TestMethod]
        public void TestSubFieldToJson()
        {
            SubField subField = new SubField('a', "Value");

            string actual = subField.ToJson()
                .Replace("\r","").Replace("\n","");
            const string expected = @"{"
+@"  ""code"": ""a"","
+@"  ""value"": ""Value"""
+@"}";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSubFieldFromJObject()
        {
            JObject jObject = new JObject
                (
                    new JProperty("code", "a"),
                    new JProperty("value", "Value")
                );

            SubField subField = SubFieldUtility.FromJObject(jObject);
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void TestSubFieldFromJson()
        {
            const string text = @"{"
+@"  ""code"": ""a"","
+@"  ""value"": ""Value"""
+@"}";

            SubField subField = SubFieldUtility.FromJson(text);

            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void TestSubFieldToXml()
        {
            SubField subField = new SubField('a', "Value");
            string actual = subField.ToXml()
                .Replace("\r", "").Replace("\n","");
            const string expected = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<subfield xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" code=""a"" value=""Value"" />";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSubFieldFromXml()
        {
            const string text = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<subfield xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" code=""a"" value=""Value"" />";

            SubField subField = SubFieldUtility.FromXml(text);

            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }
    }
}
