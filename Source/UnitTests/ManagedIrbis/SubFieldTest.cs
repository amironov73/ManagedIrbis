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
        public void SubField_Constructor()
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
            Assert.AreEqual("^aThe value", subField.ToString());

            SubField clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.CodeString, clone.CodeString);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^aThe value", clone.ToString());
            Assert.AreEqual(0, SubField.Compare(subField, clone));

            subField.SetValue("New value");
            Assert.AreEqual("New value", subField.Value);
            subField.SetValue(null);
            Assert.AreEqual(null, subField.Value);
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
        public void SubField_Serialization()
        {
            _TestSerialization(new SubField[0]);
            _TestSerialization(new SubField());
            _TestSerialization(new SubField(), new SubField());
            _TestSerialization(new SubField('a'), new SubField('b'));
            _TestSerialization(new SubField('a', "Hello"),
                new SubField('b', "World"));
        }

        [TestMethod]
        public void SubField_SetValue1()
        {
            SubField subField = new SubField('a')
            {
                Value = "Right Value"
            };
            Assert.AreEqual("Right Value", subField.Value);
        }

        [TestMethod]
        public void SubField_SetValue2()
        {
            SubField subField = new SubField('a')
            {
                Value = "  Right Value  "
            };
            Assert.AreEqual("Right Value", subField.Value);
        }

        [TestMethod]
        public void SubField_SetValue3()
        {
            SubField subField = new SubField('a')
            {
                Value = "Right\nValue"
            };
            Assert.AreEqual("Right Value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void SubField_SetValue_Exception()
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
        public void SubField_ReadOnly()
        {
            SubField subField = new SubField('a', "Value", true, null);
            Assert.AreEqual("Value", subField.Value);
            subField.Value = "New value";
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubField_AsReadOnly()
        {
            SubField subField = new SubField('a', "Value")
                .AsReadOnly();
            Assert.AreEqual("Value", subField.Value);
            subField.Value = "New value";
            Assert.AreEqual("Value", subField.Value);
        }


        [TestMethod]
        public void SubField_ToJObject()
        {
            SubField subField = new SubField('a', "Value");

            JObject jObject = subField.ToJObject();
            Assert.AreEqual("a", jObject["code"].ToString());
            Assert.AreEqual("Value", jObject["value"].ToString());
        }

        [TestMethod]
        public void SubField_ToJson()
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
        public void SubField_FromJObject()
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
        public void SubField_FromJson()
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
        public void SubField_ToXml()
        {
            SubField subField = new SubField('a', "Value");
            string actual = subField.ToXml()
                .Replace("\r", "").Replace("\n","");
            const string expected = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<subfield xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" code=""a"" value=""Value"" />";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubField_FromXml()
        {
            const string text = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<subfield xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" code=""a"" value=""Value"" />";

            SubField subField = SubFieldUtility.FromXml(text);

            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void SubField_Field1()
        {
            SubField subField = new SubField('a', "Title");
            Assert.IsNull(subField.Field);

            RecordField field = new RecordField("200");
            field.SubFields.Add(subField);
            Assert.AreEqual(field, subField.Field);
        }

        [TestMethod]
        public void SubField_Path1()
        {
            SubField subField = new SubField();
            Assert.AreEqual(string.Empty, subField.Path);

            subField = new SubField('a', "Title");
            Assert.AreEqual("^a", subField.Path);

            RecordField field = new RecordField("200");
            field.SubFields.Add(subField);
            Assert.AreEqual("200/0^a", subField.Path);
        }

        [TestMethod]
        public void SubField_Verify1()
        {
            SubField subField = new SubField();
            Assert.IsFalse(subField.Verify(false));

            subField = new SubField('a');
            Assert.IsTrue(subField.Verify(false));

            subField = new SubField('a', "Title");
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        public void SubField_Compare1()
        {
            SubField subField1 = new SubField('a');
            SubField subField2 = new SubField('b');
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) < 0
                );

            subField1 = new SubField('a', "Title1");
            subField2 = new SubField('a', "Title2");
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) < 0
                );

            subField1 = new SubField('a', "Title");
            subField2 = new SubField('a', "Title");
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) == 0
                );
        }

        [TestMethod]
        public void SubField_SetModified1()
        {
            SubField subField = new SubField('a', "Title1");
            Assert.IsFalse(subField.Modified);
            subField.Value = "Title2";
            Assert.IsTrue(subField.Modified);
            subField.NotModified();
            Assert.IsFalse(subField.Modified);
        }

        [TestMethod]
        public void SubField_SetModified2()
        {
            RecordField field = new RecordField("200");
            Assert.IsFalse(field.Modified);
            SubField subField = new SubField('a', "Title1");
            Assert.IsFalse(subField.Modified);
            field.SubFields.Add(subField);
            field.NotModified();
            subField.Value = "Title2";
            Assert.IsTrue(subField.Modified);
            Assert.IsTrue(field.Modified);
            subField.NotModified();
            Assert.IsFalse(subField.Modified);
        }

        [TestMethod]
        public void SubField_UserData()
        {
            SubField subField = new SubField();
            Assert.IsNull(subField.UserData);

            subField.UserData = "User data";
            Assert.AreEqual("User data", subField.UserData);
        }

        [TestMethod]
        public void SubField_ToString1()
        {
            SubField subField = new SubField();
            Assert.AreEqual("^\0", subField.ToString());

            subField = new SubField('a');
            Assert.AreEqual("^a", subField.ToString());

            subField = new SubField('A');
            Assert.AreEqual("^a", subField.ToString());

            subField = new SubField('a', "Title");
            Assert.AreEqual("^aTitle", subField.ToString());
        }
    }
}
