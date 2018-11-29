using System;

using AM;
using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordFieldTest
    {
        class MyClass
        {
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        [TestMethod]
        public void RecordField_Constructor_1()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(RecordField.NoTag, field.Tag);
            Assert.AreEqual(null, field.Value);
            Assert.AreEqual(0,field.SubFields.Count);
        }

        [TestMethod]
        public void RecordField_Constructor_2()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(field, field.SubFields.Field);
        }

        [TestMethod]
        public void RecordField_Constructor_3()
        {
            const string expected = "Value";
            MarcRecord record = new MarcRecord();
            RecordField field = new RecordField(100, expected, true, record);
            Assert.AreEqual(100, field.Tag);
            Assert.AreEqual(expected, field.Value);
            Assert.IsTrue(field.ReadOnly);
            Assert.AreSame(record, field.Record);
        }

        [TestMethod]
        public void RecordField_AddSubField_1()
        {
            RecordField field = new RecordField();
            field.AddSubField('a', "Value");
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual(field, field.SubFields[0].Field);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_1()
        {
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', null)
                .AddNonEmptySubField('b', new MyClass{Text = "SubfieldB"});
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("SubfieldB", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_2()
        {
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', false, "SubfieldA")
                .AddNonEmptySubField('b', true, "SubfieldB");
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("SubfieldB", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_3()
        {
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', 0)
                .AddNonEmptySubField('b', 1);
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("1", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_4()
        {
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', 0L)
                .AddNonEmptySubField('b', 1L);
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("1", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_5()
        {
            DateTime? first = null;
            DateTime? second = new DateTime(2017, 9, 30);
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', first)
                .AddNonEmptySubField('b', second);
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("20170930", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_6()
        {
            DateTime first = DateTime.MinValue;
            DateTime second = new DateTime(2017, 9, 30);
            RecordField field = new RecordField()
                .AddNonEmptySubField('a', first)
                .AddNonEmptySubField('b', second);
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("20170930", field.SubFields[0].Value);
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
        public void RecordField_Serialization_1()
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
                            200,
                            new SubField('a', "Hello")
                        )
                );
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField(200, "Значение");

            result.AddSubField('a', "Заглавие");
            result.AddSubField('e', "подзаголовочные");
            result.AddSubField('f', "об ответственности");

            return result;
        }

        [TestMethod]
        public void RecordField_ToString_1()
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
        public void RecordField_ToText_1()
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
        public void RecordField_ReadOnly_1()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            field.Value = "New value";
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void RecordField_ReadOnly_2()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            field.AddSubField('a', "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void RecordField_ReadOnly_3()
        {
            RecordField field = _GetField().AsReadOnly();
            Assert.IsTrue(field.ReadOnly);
            SubField subField = field.SubFields[0];
            subField.Value = "New value";
        }

        [TestMethod]
        public void RecordField_ToJObject_1()
        {
            RecordField field = _GetField();

            JObject jObject = field.ToJObject();

            Assert.AreEqual(200, jObject["tag"]);
            Assert.AreEqual("Заглавие", jObject["subfields"][0]["value"]);
        }

        [TestMethod]
        public void RecordField_ToJson_1()
        {
            RecordField field = _GetField();

            string actual = field.ToJson()
                .Replace("\r","").Replace("\n","")
                .Replace("\"", "'");
            const string expected = "{'tag':200,'value':'Значение','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочные'},{'code':'f','value':'об ответственности'}]}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordField_FromJObject_1()
        {
            JObject jObject = JObject.Parse(@"{""tag"": ""200"","
+@"""subfields"": []}");

            RecordField field = RecordFieldUtility.FromJObject(jObject);

            Assert.AreEqual(200, field.Tag);
        }

        [TestMethod]
        public void RecordField_FromJson_1()
        {
            string text = "{'tag':'200', 'indicator1':'1', 'value':'Значение','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочные'},{'code':'f','value':'об ответственности'}]}"
                .Replace("'", "\"");

            RecordField field = RecordFieldUtility.FromJson(text);

            Assert.AreEqual(200, field.Tag);
            Assert.AreEqual(3, field.SubFields.Count);
        }

        [TestMethod]
        public void RecordField_ToXml_1()
        {
            RecordField field = _GetField();

            string actual = field.ToXml()
                .Replace("\r", "").Replace("\n","");
            const string expected = "<field tag=\"200\" value=\"Значение\"><subfield code=\"a\" value=\"Заглавие\" /><subfield code=\"e\" value=\"подзаголовочные\" /><subfield code=\"f\" value=\"об ответственности\" /></field>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordField_FromXml_1()
        {
            const string text = @"<?xml version=""1.0"" encoding=""utf-16""?>"
+@"<field xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" tag=""200"" value=""Значение"">"
+@"  <subfield code=""a"" value=""Заглавие"" />"
+@"  <subfield code=""e"" value=""подзаголовочные"" />"
+@"  <subfield code=""f"" value=""об ответственности"" />"
+@"</field>";
            RecordField field = RecordFieldUtility.FromXml(text);

            Assert.AreEqual(200, field.Tag);
            Assert.AreEqual(3, field.SubFields.Count);
        }

        [TestMethod]
        public void RecordField_Clone_1()
        {
            RecordField source = _GetField();
            RecordField target = source.Clone();

            Assert.AreEqual
                (
                    0,
                    RecordField.Compare
                    (
                        source,
                        target
                    )
                );
        }

        [TestMethod]
        public void RecordField_IsFixed_1()
        {
            RecordField field = new RecordField(100);
            Assert.IsFalse(field.IsFixed);

            field = new RecordField(5);
            Assert.IsTrue(field.IsFixed);
        }

        [TestMethod]
        public void RecordField_SetValue_1()
        {
            bool save = RecordField.BreakOnValueContainDelimiters;
            RecordField.BreakOnValueContainDelimiters = false;
            try
            {
                RecordField field = new RecordField(200);
                field.SetValue("^aSubfieldA^bSubfieldB");
                Assert.AreEqual(2, field.SubFields.Count);
                Assert.AreEqual("SubfieldA", field.GetFirstSubFieldValue('a'));
                Assert.AreEqual("SubfieldB", field.GetFirstSubFieldValue('b'));
                Assert.IsNull(field.Value);
            }
            finally
            {
                RecordField.BreakOnValueContainDelimiters = save;
            }
        }

        [TestMethod]
        public void RecordField_SetValue_2()
        {
            bool save = RecordField.BreakOnValueContainDelimiters;
            RecordField.BreakOnValueContainDelimiters = false;
            try
            {
                RecordField field = new RecordField(200);
                field.SetValue("Value^aSubfieldA^bSubfieldB");
                Assert.AreEqual(2, field.SubFields.Count);
                Assert.AreEqual("SubfieldA", field.GetFirstSubFieldValue('a'));
                Assert.AreEqual("SubfieldB", field.GetFirstSubFieldValue('b'));
                Assert.AreEqual("Value", field.Value);
            }
            finally
            {
                RecordField.BreakOnValueContainDelimiters = save;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RecordField_SetValue_3()
        {
            bool save = RecordField.BreakOnValueContainDelimiters;
            RecordField.BreakOnValueContainDelimiters = true;
            try
            {
                RecordField field = new RecordField(200);
                field.SetValue("^aSubfieldA^bSubfieldB");
            }
            finally
            {
                RecordField.BreakOnValueContainDelimiters = save;
            }
        }

        [TestMethod]
        public void RecordField_SetValue_4()
        {
            const string expected = "Value";
            RecordField field = new RecordField(200);
            field.SetValue(expected);
            Assert.AreEqual(expected, field.Value);
        }

        [TestMethod]
        public void RecordField_SetValue_5()
        {
            bool save = RecordField.TrimValue;
            RecordField.TrimValue = true;
            try
            {
                RecordField field = new RecordField(200);
                field.SetValue(" Value ");
                Assert.AreEqual("Value", field.Value);
            }
            finally
            {
                RecordField.TrimValue = save;
            }
        }

        [TestMethod]
        public void RecordField_SetValue_6()
        {
            const string expected = " Value ";
            bool save = RecordField.TrimValue;
            RecordField.TrimValue = false;
            try
            {
                RecordField field = new RecordField(200);
                field.SetValue(expected);
                Assert.AreEqual(expected, field.Value);
            }
            finally
            {
                RecordField.TrimValue = save;
            }
        }

        [TestMethod]
        public void RecordField_SetTag_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = new RecordField(100);
            record.Fields.Add(field);
            field.SetTag(200);
            Assert.AreSame(field, record.Fields.GetFirstField(200));
            Assert.IsNull(record.Fields.GetFirstField(100));
        }

        [TestMethod]
        public void RecordField_SetSubField_1()
        {
            const string expected = "SubFieldA1";
            RecordField field = new RecordField(200)
                .AddSubField('a', "SubfieldA");
            field.SetSubField('a', expected);
            Assert.AreEqual(expected, field.GetFirstSubFieldValue('a'));
        }

        [TestMethod]
        public void RecordField_SetSubField_2()
        {
            const string expected = "SubFieldB";
            RecordField field = new RecordField(200)
                .AddSubField('a', "SubfieldA");
            field.SetSubField('b', expected);
            Assert.AreEqual(expected, field.GetFirstSubFieldValue('b'));
        }

        [TestMethod]
        public void RecordField_ReplaceSubField_1()
        {
            const string previous = "SubfieldA";
            const string expected = "SubFieldA1";
            RecordField field = new RecordField(200)
                .AddSubField('a', previous);
            field.ReplaceSubField('a', previous, expected);
            Assert.AreEqual(expected, field.GetFirstSubFieldValue('a'));
        }

        [TestMethod]
        public void RecordField_RemoveSubField_1()
        {
            RecordField field = new RecordField(200)
                .AddSubField('a', "SubfieldA")
                .AddSubField('b', "SubfieldB");
            Assert.AreEqual(2, field.SubFields.Count);
            field.RemoveSubField('a');
            Assert.AreEqual(1, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("SubfieldB", field.SubFields[0].Value);
        }

        [TestMethod]
        public void RecordField_RemoveSubField_2()
        {
            RecordField field = new RecordField(200)
                .AddSubField('a', "SubfieldA")
                .AddSubField('b', "SubfieldB");
            Assert.AreEqual(2, field.SubFields.Count);
            field.RemoveSubField('c');
            Assert.AreEqual(2, field.SubFields.Count);
        }

        [TestMethod]
        public void RecordField_Parse_1()
        {
            RecordField field = RecordField.Parse("200#Value^aSubfieldA^bSubfieldB");
            Assert.IsNotNull(field);
            Assert.AreEqual(200, field.Tag);
            Assert.AreEqual("Value", field.Value);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual('a', field.SubFields[0].Code);
            Assert.AreEqual("SubfieldA", field.SubFields[0].Value);
            Assert.AreEqual('b', field.SubFields[1].Code);
            Assert.AreEqual("SubfieldB", field.SubFields[1].Value);
        }

        [TestMethod]
        public void RecordField_Parse_2()
        {
            RecordField field = RecordField.Parse("200#Value");
            Assert.IsNotNull(field);
            Assert.AreEqual(200, field.Tag);
            Assert.AreEqual("Value", field.Value);
            Assert.AreEqual(0, field.SubFields.Count);
        }

        [TestMethod]
        public void RecordField_Parse_3()
        {
            RecordField field = RecordField.Parse("200#^aSubfieldA^bSubfieldB");
            Assert.IsNotNull(field);
            Assert.AreEqual(200, field.Tag);
            Assert.IsNull(field.Value);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual('a', field.SubFields[0].Code);
            Assert.AreEqual("SubfieldA", field.SubFields[0].Value);
            Assert.AreEqual('b', field.SubFields[1].Code);
            Assert.AreEqual("SubfieldB", field.SubFields[1].Value);
        }

        [TestMethod]
        public void RecordField_Parse_4()
        {
            RecordField field = RecordField.Parse(null);
            Assert.IsNull(field);

            field = RecordField.Parse(string.Empty);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordField_AssignFrom_1()
        {
            RecordField source = new RecordField(100, "Value")
                .AddSubField('a', "SubfieldA")
                .AddSubField('b', "SubfieldB");
            RecordField target = new RecordField(200);
            target.AssignFrom(source);
            Assert.AreEqual(200, target.Tag);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual(source.SubFields.Count, target.SubFields.Count);
            for (int i = 0; i < source.SubFields.Count; i++)
            {
                Assert.AreEqual(source.SubFields[i].Code, target.SubFields[i].Code);
                Assert.AreEqual(source.SubFields[i].Value, target.SubFields[i].Value);
            }
        }

        [TestMethod]
        public void RecordField_Compare_1()
        {
            RecordField first = new RecordField(100, "Value1");
            RecordField second = new RecordField(200, "Value2");
            Assert.IsTrue(RecordField.Compare(first, second) < 0);
        }

        [TestMethod]
        public void RecordField_Compare_2()
        {
            RecordField first = new RecordField(100, "Value1");
            RecordField second = new RecordField(100, "Value2");
            Assert.IsTrue(RecordField.Compare(first, second) < 0);
        }

        [TestMethod]
        public void RecordField_Compare_3()
        {
            RecordField first = new RecordField(100, "Value1");
            RecordField second = new RecordField(100, "Value1")
                .AddSubField('a', "SubfieldA");
            Assert.IsTrue(RecordField.Compare(first, second) < 0);
        }

        [TestMethod]
        public void RecordField_Compare_4()
        {
            RecordField first = new RecordField(100, "Value1")
                .AddSubField('a', "SubfieldA1");
            RecordField second = new RecordField(100, "Value1")
                .AddSubField('a', "SubfieldA2");
            Assert.IsTrue(RecordField.Compare(first, second) < 0);
        }

        [TestMethod]
        public void RecordField_Compare_5()
        {
            RecordField first = new RecordField(100, "Value1")
                .AddSubField('a', "SubfieldA");
            RecordField second = new RecordField(100, "Value1")
                .AddSubField('a', "SubfieldA");
            Assert.IsTrue(RecordField.Compare(first, second) == 0);
        }

        [TestMethod]
        public void RecordField_HaveSubField_1()
        {
            RecordField field = new RecordField()
                .AddSubField('a', "SubfieldA");
            Assert.IsTrue(field.HaveSubField('a'));
            Assert.IsFalse(field.HaveSubField('b'));
        }

        [TestMethod]
        public void RecordField_HaveSubField_2()
        {
            RecordField field = new RecordField()
                .AddSubField('a', "SubfieldA");
            Assert.IsTrue(field.HaveSubField('a', 'b'));
            Assert.IsFalse(field.HaveSubField('b', 'c'));
        }

        [TestMethod]
        public void RecordField_HaveNotSubField_1()
        {
            RecordField field = new RecordField()
                .AddSubField('a', "SubfieldA");
            Assert.IsFalse(field.HaveNotSubField('a'));
            Assert.IsTrue(field.HaveNotSubField('b'));
        }

        [TestMethod]
        public void RecordField_HaveNotSubField_2()
        {
            RecordField field = new RecordField()
                .AddSubField('a', "SubfieldA");
            Assert.IsFalse(field.HaveNotSubField('a', 'b'));
            Assert.IsTrue(field.HaveNotSubField('b', 'c'));
        }

        [TestMethod]
        public void RecordField_GetSubField_1()
        {
            RecordField field = new RecordField();
            SubField subField = new SubField('a', "SubfieldA");
            field.SubFields.Add(subField);
            SubField[] found = field.GetSubField('a');
            Assert.AreEqual(1, found.Length);
            Assert.AreSame(subField, found[0]);

            found = field.GetSubField('b');
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void RecordField_GetSubField_2()
        {
            RecordField field = new RecordField();
            SubField subField1 = new SubField('a', "SubfieldA1");
            field.SubFields.Add(subField1);
            SubField subField2 = new SubField('a', "SubfieldA2");
            field.SubFields.Add(subField2);
            SubField found = field.GetSubField('a', 0);
            Assert.AreSame(subField1, found);
            found = field.GetSubField('a', 1);
            Assert.AreEqual(subField2, found);
            found = field.GetSubField('a', 2);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void RecordField_GetSubFieldValue_1()
        {
            const string expected1 = "SubfieldA1";
            const string expected2 = "SubfieldA2";
            RecordField field = new RecordField();
            SubField subField1 = new SubField('a', expected1);
            field.SubFields.Add(subField1);
            SubField subField2 = new SubField('a', expected2);
            field.SubFields.Add(subField2);
            Assert.AreEqual(expected1, field.GetSubFieldValue('a', 0));
            Assert.AreEqual(expected2, field.GetSubFieldValue('a', 1));
            Assert.IsNull(field.GetSubFieldValue('a', 2));
            Assert.IsNull(field.GetSubFieldValue('b', 0));
        }

        [TestMethod]
        public void RecordField_GetValueOrFirstSubField_1()
        {
            const string expected0 = "Value";
            const string expected1 = "SubfieldA1";
            const string expected2 = "SubfieldA2";
            RecordField field = new RecordField(200, expected0)
                .AddSubField('a', expected1)
                .AddSubField('a', expected2);
            Assert.AreEqual(expected0, field.GetValueOrFirstSubField());
            field.Value = null;
            Assert.AreEqual(expected1, field.GetValueOrFirstSubField());
            field.SubFields.RemoveAt(0);
            Assert.AreEqual(expected2, field.GetValueOrFirstSubField());
            field.SubFields.Clear();
            Assert.IsNull(field.GetValueOrFirstSubField());
        }

        [TestMethod]
        public void RecordField_IsEmpty_1()
        {
            RecordField field = new RecordField(200);
            Assert.IsTrue(field.IsEmpty);

            field = new RecordField(200, "Field200");
            Assert.IsFalse(field.IsEmpty);

            field = new RecordField(200, new SubField('a', "SubfieldA"));
            Assert.IsFalse(field.IsEmpty);
        }
    }
}
