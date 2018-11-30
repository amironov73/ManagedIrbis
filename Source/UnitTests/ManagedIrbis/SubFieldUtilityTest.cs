using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JetBrains.Annotations;

using ManagedIrbis;

using Newtonsoft.Json.Linq;

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ConvertToLocalFunction
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldUtilityTest
    {
        [NotNull]
        private SubField[] _GetArray()
        {
            return new[]
            {
                new SubField('a', "SubFieldA1"),
                new SubField('b', "SubFieldB1"),
                new SubField('c', "SubFieldC1"),
                new SubField('a', "SubFieldA2"),
                new SubField('b', "SubFieldB2"),
                new SubField('c', "SubFieldC2"),
            };
        }

        [NotNull]
        private SubFieldCollection _GetCollection()
        {
            SubFieldCollection result = new SubFieldCollection
            {
                new SubField('a', "SubFieldA1"),
                new SubField('b', "SubFieldB1"),
                new SubField('c', "SubFieldC1"),
                new SubField('a', "SubFieldA2"),
                new SubField('b', "SubFieldB2"),
                new SubField('c', "SubFieldC2"),
            };

            return result;
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_1()
        {
            SubField[] array = _GetArray();
            SubField found = SubFieldUtility.GetFirstSubField(array, 'a');
            Assert.AreEqual("SubFieldA1", found.Value);

            found = SubFieldUtility.GetFirstSubField(array, 'c');
            Assert.AreEqual("SubFieldC1", found.Value);

            found = SubFieldUtility.GetFirstSubField(array, 'd');
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_2()
        {
            SubFieldCollection collection = _GetCollection();
            SubField found = SubFieldUtility.GetFirstSubField(collection, 'a');
            Assert.AreEqual("SubFieldA1", found.Value);

            found = SubFieldUtility.GetFirstSubField(collection, 'c');
            Assert.AreEqual("SubFieldC1", found.Value);

            found = SubFieldUtility.GetFirstSubField(collection, 'd');
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_3()
        {
            SubField[] array = _GetArray();
            SubField found = SubFieldUtility.GetFirstSubField(array, 'a', 'b');
            Assert.AreEqual("SubFieldA1", found.Value);

            found = SubFieldUtility.GetFirstSubField(array, 'c', 'd');
            Assert.AreEqual("SubFieldC1", found.Value);

            found = SubFieldUtility.GetFirstSubField(array, 'd', 'e');
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_4()
        {
            SubFieldCollection collection = _GetCollection();
            SubField found = SubFieldUtility.GetFirstSubField(collection, 'a', 'b');
            Assert.AreEqual("SubFieldA1", found.Value);

            found = SubFieldUtility.GetFirstSubField(collection, 'c', 'd');
            Assert.AreEqual("SubFieldC1", found.Value);

            found = SubFieldUtility.GetFirstSubField(collection, 'd', 'e');
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_5()
        {
            SubField[] array = _GetArray();
            SubField found = SubFieldUtility.GetFirstSubField(array, 'a', "SubFieldA2");
            Assert.AreEqual("SubFieldA2", found.Value);

            found = SubFieldUtility.GetFirstSubField(array, 'c', "SubFieldC3");
            Assert.IsNull(found);

            found = SubFieldUtility.GetFirstSubField(array, 'd', "No such subfield");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetFirstSubField_6()
        {
            SubFieldCollection collection = _GetCollection();
            SubField found = SubFieldUtility.GetFirstSubField(collection, 'a', "SubFieldA2");
            Assert.AreEqual("SubFieldA2", found.Value);

            found = SubFieldUtility.GetFirstSubField(collection, 'c', "SubFieldC3");
            Assert.IsNull(found);

            found = SubFieldUtility.GetFirstSubField(collection, 'd', "No such subfield");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_1()
        {
            SubField[] array = _GetArray();
            SubField[] found = SubFieldUtility.GetSubField(array, 'a');
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldA2", found[1].Value);

            found = SubFieldUtility.GetSubField(array, 'd');
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_2()
        {
            SubFieldCollection collection = _GetCollection();
            SubField[] found = SubFieldUtility.GetSubField(collection, 'a');
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldA2", found[1].Value);

            found = SubFieldUtility.GetSubField(collection, 'd');
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_3()
        {
            SubField[] array = _GetArray();
            SubField[] found = SubFieldUtility.GetSubField(array, 'a', 'b');
            Assert.AreEqual(4, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldB1", found[1].Value);
            Assert.AreEqual("SubFieldA2", found[2].Value);
            Assert.AreEqual("SubFieldB2", found[3].Value);

            found = SubFieldUtility.GetSubField(array, 'c', 'd');
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("SubFieldC1", found[0].Value);
            Assert.AreEqual("SubFieldC2", found[1].Value);

            found = SubFieldUtility.GetSubField(array, 'd', 'e');
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_4()
        {
            SubFieldCollection collection = _GetCollection();
            SubField[] found = SubFieldUtility.GetSubField(collection, 'a', 'b');
            Assert.AreEqual(4, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldB1", found[1].Value);
            Assert.AreEqual("SubFieldA2", found[2].Value);
            Assert.AreEqual("SubFieldB2", found[3].Value);

            found = SubFieldUtility.GetSubField(collection, 'c', 'd');
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("SubFieldC1", found[0].Value);
            Assert.AreEqual("SubFieldC2", found[1].Value);

            found = SubFieldUtility.GetSubField(collection, 'd', 'e');
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_5()
        {
            SubField[] array = _GetArray();
            int counter = 0;
            Action<SubField> action = sub => counter++;
            SubField[] found = SubFieldUtility.GetSubField(array, action);
            Assert.AreEqual(6, found.Length);
            Assert.AreEqual(6, counter);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_6()
        {
            SubFieldCollection collection = _GetCollection();
            int counter = 0;
            Action<SubField> action = sub => counter++;
            SubField[] found = SubFieldUtility.GetSubField(collection, action);
            Assert.AreEqual(6, found.Length);
            Assert.AreEqual(6, counter);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_7()
        {
            RecordField[] fields =
            {
                new RecordField(100)
                {
                    SubFields =
                    {
                        new SubField('a', "100a"),
                        new SubField('b', "100b")
                    }
                },
                new RecordField(200)
                {
                    SubFields =
                    {
                        new SubField('a', "200a1"),
                        new SubField('b', "200b"),
                        new SubField('a', "200a2")
                    }
                }
            };
            Func<RecordField, bool> fieldPredicate = field => field.Tag == 200;
            Func<SubField, bool> subfieldPredicate = sub => sub.Code == 'a';
            SubField[] found = SubFieldUtility.GetSubField(fields, fieldPredicate, subfieldPredicate);
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("200a1", found[0].Value);
            Assert.AreEqual("200a2", found[1].Value);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubField_8()
        {
            RecordField[] fields =
            {
                new RecordField(100)
                {
                    SubFields =
                    {
                        new SubField('a', "100a"),
                        new SubField('b', "100b")
                    }
                },
                new RecordField(200)
                {
                    SubFields =
                    {
                        new SubField('a', "200a1"),
                        new SubField('b', "200b"),
                        new SubField('a', "200a2")
                    }
                }
            };
            int[] tags = { 200 };
            char[] codes = { 'a' };
            SubField[] found = SubFieldUtility.GetSubField(fields, tags, codes);
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("200a1", found[0].Value);
            Assert.AreEqual("200a2", found[1].Value);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubFieldRegex_1()
        {
            SubField[] array = _GetArray();
            SubField[] found = SubFieldUtility.GetSubFieldRegex(array, "[ab]");
            Assert.AreEqual(4, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldB1", found[1].Value);
            Assert.AreEqual("SubFieldA2", found[2].Value);
            Assert.AreEqual("SubFieldB2", found[3].Value);

            found = SubFieldUtility.GetSubFieldRegex(array, "[x-z]");
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubFieldRegex_2()
        {
            SubField[] array = _GetArray();
            char[] codes = { 'a', 'b', 'c', 'd' };
            SubField[] found = SubFieldUtility.GetSubFieldRegex(array, codes, "[AB][12]");
            Assert.AreEqual(4, found.Length);
            Assert.AreEqual("SubFieldA1", found[0].Value);
            Assert.AreEqual("SubFieldB1", found[1].Value);
            Assert.AreEqual("SubFieldA2", found[2].Value);
            Assert.AreEqual("SubFieldB2", found[3].Value);

            found = SubFieldUtility.GetSubFieldRegex(array, codes, "[AB][34]");
            Assert.AreEqual(0, found.Length);

            codes = new[] { 'x', 'y', 'z' };
            found = SubFieldUtility.GetSubFieldRegex(array, codes, "[AB][12]");
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubFieldRegex_3()
        {
            RecordField[] fields =
            {
                new RecordField(100)
                {
                    SubFields =
                    {
                        new SubField('a', "100a"),
                        new SubField('b', "100b")
                    }
                },
                new RecordField(200)
                {
                    SubFields =
                    {
                        new SubField('a', "200a1"),
                        new SubField('b', "200b"),
                        new SubField('a', "200a2")
                    }
                }
            };
            int[] tags = { 200 };
            char[] codes = { 'a' };
            SubField[] found = SubFieldUtility.GetSubFieldRegex(fields, tags, codes, "a2");
            Assert.AreEqual(1, found.Length);
            Assert.AreEqual("200a2", found[0].Value);
        }

        [TestMethod]
        public void SubFieldUtility_GetSubFieldValue_1()
        {
            SubField subField = new SubField('a', "Value");
            Assert.AreEqual("Value", SubFieldUtility.GetSubFieldValue(subField));

            subField = null;
            Assert.AreEqual(null, SubFieldUtility.GetSubFieldValue(subField));
        }

        [TestMethod]
        public void SubFieldUtility_GetSubFieldValue_2()
        {
            SubField[] array =
            {
                new SubField('a', "ValueA"),
                new SubField('b', "ValueB"),
                null,
                new SubField('c', null),
                new SubField('d', "ValueD")
            };
            string[] values = SubFieldUtility.GetSubFieldValue(array);
            Assert.AreEqual(3, values.Length);
            Assert.AreEqual("ValueA", values[0]);
            Assert.AreEqual("ValueB", values[1]);
            Assert.AreEqual("ValueD", values[2]);
        }

        [TestMethod]
        public void SubFieldUtility_ToJObject_1()
        {
            SubField subField = new SubField('a', "Value");
            JObject jobject = SubFieldUtility.ToJObject(subField);
            Assert.AreEqual('a', Extensions.Value<char>(jobject["code"]));
            Assert.AreEqual("Value", Extensions.Value<string>(jobject["value"]));
        }

        [TestMethod]
        public void SubFieldUtility_ToJson_1()
        {
            SubField subField = new SubField('a', "Value");
            Assert.AreEqual
                (
                    "{'code':'a','value':'Value'}",
                    SubFieldUtility.ToJson(subField)
                );
        }

        [TestMethod]
        public void SubFieldUtility_FromJObject_1()
        {
            JObject jObject = new JObject
            {
                {"code", "a"},
                {"value", "Value"}
            };
            SubField subField = SubFieldUtility.FromJObject(jObject);
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void SubFieldUtility_FromJObject_2()
        {
            JObject jObject = new JObject
            {
                {"code", "a"}
            };
            SubField subField = SubFieldUtility.FromJObject(jObject);
            Assert.AreEqual('a', subField.Code);
            Assert.IsNull(subField.Value);
        }

        [TestMethod]
        public void SubFieldUtility_FromJson_1()
        {
            SubField subField = SubFieldUtility.FromJson("{'code':'a','value':'Value'}");
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void SubFieldUtility_ToSourceCode_1()
        {
            SubField subField = new SubField();
            Assert.AreEqual
                (
                    "new SubField('\\0', null)",
                    SubFieldUtility.ToSourceCode(subField)
                );
        }

        [TestMethod]
        public void SubFieldUtility_ToSourceCode_2()
        {
            SubField subField = new SubField('a');
            Assert.AreEqual
                (
                    "new SubField('a', null)",
                    SubFieldUtility.ToSourceCode(subField)
                );
        }

        [TestMethod]
        public void SubFieldUtility_ToSourceCode_3()
        {
            SubField subField = new SubField('a', "Some text");
            Assert.AreEqual
                (
                    "new SubField('a', \"Some text\")",
                    SubFieldUtility.ToSourceCode(subField)
                );
        }
    }
}
