using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable PossiblyMistakenUseOfParamsMethod
// ReSharper disable ConvertToLocalFunction
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordFieldUtilityTest
    {
        [NotNull]
        private RecordField _GetField()
        {
            return new RecordField()
                .AddSubField('a', "SubA")
                .AddSubField('b', "SubB")
                .AddSubField('c', "SubC")
                .AddSubField('d', "SubD")
                .AddSubField('e', "SubE");
        }

        [NotNull]
        private RecordFieldCollection _GetFieldCollection()
        {
            return new RecordFieldCollection
            {
                new RecordField(100)
                    .AddSubField('a', "SubA")
                    .AddSubField('b', "SubB")
                    .AddSubField('c', "SubC")
                    .AddSubField('d', "SubD")
                    .AddSubField('e', "SubE"),
                new RecordField(101)
                    .AddSubField('f', "SubF")
                    .AddSubField('g', "SubG")
                    .AddSubField('h', "SubH")
                    .AddSubField('i', "SubI")
                    .AddSubField('j', "SubJ"),
                new RecordField(200)
                    .AddSubField('k', "SubK")
                    .AddSubField('l', "SubL")
                    .AddSubField('m', "SubM")
                    .AddSubField('n', "SubN")
                    .AddSubField('o', "SubO"),
                new RecordField(200)
                    .AddSubField('p', "SubP")
                    .AddSubField('q', "SubQ")
                    .AddSubField('r', "SubR")
                    .AddSubField('s', "SubS")
                    .AddSubField('t', "SubT")
            };
        }

        [NotNull]
        private IEnumerable<RecordField> _GetFieldEnumeration()
        {
            yield return new RecordField(100)
                .AddSubField('a', "SubA")
                .AddSubField('b', "SubB")
                .AddSubField('c', "SubC")
                .AddSubField('d', "SubD")
                .AddSubField('e', "SubE");
            yield return new RecordField(101)
                .AddSubField('f', "SubF")
                .AddSubField('g', "SubG")
                .AddSubField('h', "SubH")
                .AddSubField('i', "SubI")
                .AddSubField('j', "SubJ");
            yield return new RecordField(200)
                .AddSubField('k', "SubK")
                .AddSubField('l', "SubL")
                .AddSubField('m', "SubM")
                .AddSubField('n', "SubN")
                .AddSubField('o', "SubO");
            yield return new RecordField(200)
                .AddSubField('p', "SubP")
                .AddSubField('q', "SubQ")
                .AddSubField('r', "SubR")
                .AddSubField('s', "SubS")
                .AddSubField('t', "SubT");
        }

        [NotNull]
        [ItemNotNull]
        private SubField[] _GetSubFieldArray()
        {
            return new[]
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
                new SubField('d', "SubD"),
                new SubField('e', "SubE"),
            };
        }

        [NotNull]
        [ItemNotNull]
        private List<SubField> _GetSubFieldList()
        {
            return new List<SubField>
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
                new SubField('d', "SubD"),
                new SubField('e', "SubE"),
            };
        }

        [NotNull]
        [ItemNotNull]
        private IEnumerable<SubField> _GetSubFieldEnumeration()
        {
            yield return new SubField('a', "SubA1");
            yield return new SubField('b', "SubB1");
            yield return new SubField('c', "SubC1");
            yield return new SubField('a', "SubA2");
            yield return new SubField('b', "SubB2");
        }

        [TestMethod]
        public void RecordFieldUtility_EmptyArray_1()
        {
            Assert.IsNotNull(RecordFieldUtility.EmptyArray);
            Assert.AreEqual(0, RecordFieldUtility.EmptyArray.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_AddNonEmptySubField_1()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.AddNonEmptySubField(field1, 'a', "Hello");
            Assert.AreSame(field1, field2);
            Assert.AreEqual(1, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddNonEmptySubField_2()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.AddNonEmptySubField(field1, 'a', "");
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddNonEmptySubField_3()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.AddNonEmptySubField(field1, 'a', null);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_1()
        {
            RecordField field1 = new RecordField();
            SubField[] subFields =
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
            };
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(3, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_2()
        {
            RecordField field1 = new RecordField();
            SubField[] subFields = new SubField[0];
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_3()
        {
            RecordField field1 = new RecordField();
            SubField[] subFields = null;
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_4()
        {
            RecordField field1 = new RecordField();
            List<SubField> subFields = new List<SubField>
            {
                new SubField('a', "SubA"),
                new SubField('b', "SubB"),
                new SubField('c', "SubC"),
            };
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(3, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_5()
        {
            RecordField field1 = new RecordField();
            List<SubField> subFields = new List<SubField>();
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AddSubFields_6()
        {
            RecordField field1 = new RecordField();
            List<SubField> subFields = null;
            RecordField field2 = RecordFieldUtility.AddSubFields(field1, subFields);
            Assert.AreSame(field1, field2);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_AllSubFields_1()
        {
            RecordField field1 = new RecordField()
                .AddSubField('a', "SubA")
                .AddSubField('b', "SubB")
                .AddSubField('c', "SubC");
            RecordField field2 = new RecordField()
                .AddSubField('d', "SubD")
                .AddSubField('e', "SubE")
                .AddSubField('f', "SubF");
            RecordField[] fields = { field1, field2 };
            SubField[] subFields = RecordFieldUtility.AllSubFields(fields);
            Assert.AreEqual(6, subFields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_ApplySubField_1()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    "SubA"
                );
            Assert.AreSame(field1, field2);
            Assert.AreEqual(1, field1.SubFields.Count);

            RecordField field3 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    (string)null
                );
            Assert.AreSame(field1, field3);
            Assert.AreEqual(0, field1.SubFields.Count);

            RecordField field4 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    '\0',
                    "Sub0"
                );
            Assert.AreSame(field1, field4);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_ApplySubField_2()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    (object)"SubA"
                );
            Assert.AreSame(field1, field2);
            Assert.AreEqual(1, field1.SubFields.Count);

            RecordField field3 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    (object)null
                );
            Assert.AreSame(field1, field3);
            Assert.AreEqual(0, field1.SubFields.Count);

            RecordField field4 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    '\0',
                    (object)"Sub0"
                );
            Assert.AreSame(field1, field4);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_ApplySubField_3()
        {
            RecordField field1 = new RecordField();
            RecordField field2 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    true,
                    "SubA"
                );
            Assert.AreSame(field1, field2);
            Assert.AreEqual(1, field1.SubFields.Count);

            RecordField field3 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    'a',
                    false,
                    "SubA"
                );
            Assert.AreSame(field1, field3);
            Assert.AreEqual(0, field1.SubFields.Count);

            RecordField field4 = RecordFieldUtility.ApplySubField
                (
                    field1,
                    '\0',
                    true,
                    "Sub0"
                );
            Assert.AreSame(field1, field4);
            Assert.AreEqual(0, field1.SubFields.Count);
        }

        [TestMethod]
        public void RecordFieldUtility_FilterSubFields_1()
        {
            IEnumerable<SubField> source = _GetSubFieldEnumeration();
            SubField[] result = RecordFieldUtility.FilterSubFields(source, 'a', 'b');
            Assert.AreEqual(4, result.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_FilterSubFields_2()
        {
            RecordField source = _GetField();
            SubField[] result = RecordFieldUtility.FilterSubFields(source, 'a', 'b');
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_1()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField[] fields = RecordFieldUtility.GetField(collection, 100);
            Assert.AreEqual(1, fields.Length);

            fields = RecordFieldUtility.GetField(collection, 1000);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_2()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField field = RecordFieldUtility.GetField(collection, 100, 0);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetField(collection, 100, 1);
            Assert.IsNull(field);

            field = RecordFieldUtility.GetField(collection, 1000, 0);
            Assert.IsNull(field);

            field = RecordFieldUtility.GetField(collection, 200, 0);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetField(collection, 200, 1);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetField(collection, 200, 2);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, 100);
            Assert.AreEqual(1, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = RecordFieldUtility.GetField(enumeration, 1000);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_4()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = RecordFieldUtility.GetField(enumeration, 100, 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, 100, 1);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, 1000, 0);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, 200, 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, 200, 1);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, 200, 2);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_5()
        {
            int count = 0;
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Action<RecordField> action = field => { count++; };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, action);
            Assert.AreEqual(4, fields.Length);
            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_6()
        {
            int fieldCount = 0, subFieldCount = 0;
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Action<RecordField> fieldAction = field => { fieldCount++; };
            Action<SubField> subFieldAction = suField => { subFieldCount++; };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, fieldAction, subFieldAction);
            Assert.AreEqual(4, fields.Length);
            Assert.AreEqual(4, fieldCount);
            Assert.AreEqual(20, subFieldCount);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_7()
        {
            int subFieldCount = 0;
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Action<SubField> action = suField => { subFieldCount++; };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, action);
            Assert.AreEqual(4, fields.Length);
            Assert.AreEqual(20, subFieldCount);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_8()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Func<SubField, bool> predicate = subField => subField.Code == 'a';
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, predicate);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_9()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            char[] codes = { 'a', 'b' };
            Func<SubField, bool> predicate = subField => subField.Value.Contains("Sub");
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, codes, predicate);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_10()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            char[] codes = { 'a', 'b' };
            string[] values = { "SubA" };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, codes, values);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_11()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            char[] codes = { 'a', 'b' };
            string[] values = { "SubA" };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, codes, values);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_12()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, 'a', "SubA");
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_13()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100 };
            char[] codes = { 'a', 'b' };
            string[] values = { "SubA" };
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, tags, codes, values);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_14()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Func<RecordField, bool> fieldPredicate = field => field.Tag < 200;
            Func<SubField, bool> subFieldPredicate = subField => subField.Code < 'c';
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, fieldPredicate, subFieldPredicate);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_15()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Func<RecordField, bool> fieldPredicate = field => field.Tag < 200;
            RecordField[] fields = RecordFieldUtility.GetField(enumeration, fieldPredicate);
            Assert.AreEqual(2, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_16()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            RecordField field = RecordFieldUtility.GetField(enumeration, tags, 0);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetField(enumeration, tags, 10);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_17()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            Func<RecordField, bool> predicate = field => field.HaveSubField('a');
            RecordField[] fields = RecordFieldUtility.GetField(collection, predicate);
            Assert.AreEqual(1, fields.Length);

            predicate = field => field.HaveSubField('j');
            fields = RecordFieldUtility.GetField(collection, predicate);
            Assert.AreEqual(1, fields.Length);

            predicate = field => field.HaveSubField('x');
            fields = RecordFieldUtility.GetField(collection, predicate);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetField_18()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            int[] tags = { 100, 200 };
            RecordField field = RecordFieldUtility.GetField(collection, tags, 1);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetField(collection, tags, 10);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldCount_1()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            Assert.AreEqual(1, RecordFieldUtility.GetFieldCount(collection, 100));
            Assert.AreEqual(2, RecordFieldUtility.GetFieldCount(collection, 200));
            Assert.AreEqual(0, RecordFieldUtility.GetFieldCount(collection, 300));
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldCount_2()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            Assert.AreEqual(1, RecordFieldUtility.GetFieldCount(enumeration, 100));

            enumeration = _GetFieldEnumeration();
            Assert.AreEqual(2, RecordFieldUtility.GetFieldCount(enumeration, 200));

            enumeration = _GetFieldEnumeration();
            Assert.AreEqual(0, RecordFieldUtility.GetFieldCount(enumeration, 300));
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_1()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField[] fields = RecordFieldUtility.GetFieldRegex(enumeration, "^[12]");
            Assert.AreEqual(4, fields.Length);

            enumeration = _GetFieldEnumeration();
            fields = RecordFieldUtility.GetFieldRegex(enumeration, "^[89]");
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_2()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = RecordFieldUtility.GetFieldRegex(enumeration, "^[12]", 2);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFieldRegex(enumeration, "^[12]", 5);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFieldRegex(enumeration, "^[89]", 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            char[] codes = { 'a', 'b' };
            string textRegex = "^Sub";
            RecordField[] fields = RecordFieldUtility.GetFieldRegex(enumeration, tags, codes, textRegex);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_4()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            char[] codes = { 'a', 'b' };
            string textRegex = "^Sub";
            RecordField field = RecordFieldUtility.GetFieldRegex(enumeration, tags, codes, textRegex, 0);
            Assert.IsNotNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_5()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            string textRegex = "^Sub";
            RecordField[] fields = RecordFieldUtility.GetFieldRegex(enumeration, tags, textRegex);
            Assert.AreEqual(0, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_6()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            int[] tags = { 100, 200 };
            string textRegex = "^Sub";
            RecordField field = RecordFieldUtility.GetFieldRegex(enumeration, tags, textRegex, 0);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_7()
        {
            IEnumerable<RecordField> enumeration = new []
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            int[] tags = { 100, 200 };
            string textRegex = "^Field";
            RecordField[] fields = RecordFieldUtility.GetFieldRegex(enumeration, tags, textRegex);
            Assert.AreEqual(1, fields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldRegex_8()
        {
            IEnumerable<RecordField> enumeration = new []
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            int[] tags = { 100, 200 };
            string textRegex = "^Field";
            RecordField field = RecordFieldUtility.GetFieldRegex(enumeration, tags, textRegex, 0);
            Assert.IsNotNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_1()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] values = RecordFieldUtility.GetFieldValue(enumeration);
            Assert.AreEqual(0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_2()
        {
            RecordFieldCollection enumeration = _GetFieldCollection();
            string[] values = RecordFieldUtility.GetFieldValue(enumeration);
            Assert.AreEqual(0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_3()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            string[] values = RecordFieldUtility.GetFieldValue(enumeration, 100);
            Assert.AreEqual(0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_4()
        {
            RecordFieldCollection enumeration = _GetFieldCollection();
            string[] values = RecordFieldUtility.GetFieldValue(enumeration, 100);
            Assert.AreEqual(0, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_5()
        {
            RecordFieldCollection enumeration = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            string[] values = RecordFieldUtility.GetFieldValue(enumeration, 100);
            Assert.AreEqual(1, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFieldValue_6()
        {
            RecordFieldCollection enumeration = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            string[] values = RecordFieldUtility.GetFieldValue(enumeration);
            Assert.AreEqual(2, values.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstField_1()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = RecordFieldUtility.GetFirstField(enumeration, 100);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFirstField(enumeration, 200);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFirstField(enumeration, 900);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstField_2()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            RecordField field = RecordFieldUtility.GetFirstField(enumeration, 100, 200);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFirstField(enumeration, 200);
            Assert.IsNotNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFirstField(enumeration, 900, 1000);
            Assert.IsNull(field);

            enumeration = _GetFieldEnumeration();
            field = RecordFieldUtility.GetFirstField(enumeration, 900, 200);
            Assert.IsNotNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstField_3()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField field = RecordFieldUtility.GetFirstField(collection, 100);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetFirstField(collection, 200);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetFirstField(collection, 900);
            Assert.IsNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstField_4()
        {
            RecordFieldCollection collection = _GetFieldCollection();
            RecordField field = RecordFieldUtility.GetFirstField(collection, 100, 200);
            Assert.IsNotNull(field);

            field = RecordFieldUtility.GetFirstField(collection, 900, 1000);
            Assert.IsNull(field);

            field = RecordFieldUtility.GetFirstField(collection, 900, 200);
            Assert.IsNotNull(field);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstFieldValue_1()
        {
            RecordFieldCollection enumeration = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            string value = RecordFieldUtility.GetFirstFieldValue(enumeration, 100);
            Assert.IsNotNull(value);

            value = RecordFieldUtility.GetFirstFieldValue(enumeration, 200);
            Assert.IsNotNull(value);

            value = RecordFieldUtility.GetFirstFieldValue(enumeration, 900);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstFieldValue_2()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "TheField200"),
            };
            string value = RecordFieldUtility.GetFirstFieldValue(collection, 100);
            Assert.IsNotNull(value);

            value = RecordFieldUtility.GetFirstFieldValue(collection, 200);
            Assert.IsNotNull(value);

            value = RecordFieldUtility.GetFirstFieldValue(collection, 900);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void RecordFieldUtility_GetFirstSubField_1()
        {
            RecordField field = _GetField();
            SubField subField = RecordFieldUtility.GetFirstSubField(field, 'a');
            Assert.IsNotNull(subField);

            subField = RecordFieldUtility.GetFirstSubField(field, 'b');
            Assert.IsNotNull(subField);

            subField = RecordFieldUtility.GetFirstSubField(field, 'x');
            Assert.IsNull(subField);
        }

        [TestMethod]
        public void RecordFieldUtility_GetSubField_1()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField[] subFields = RecordFieldUtility.GetSubField(enumeration, 'a');
            Assert.AreEqual(1, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = RecordFieldUtility.GetSubField(enumeration, 'b');
            Assert.AreEqual(1, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = RecordFieldUtility.GetSubField(enumeration, 'x');
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetSubField_2()
        {
            IEnumerable<RecordField> enumeration = _GetFieldEnumeration();
            SubField[] subFields = RecordFieldUtility.GetSubField(enumeration, 'a', 'b');
            Assert.AreEqual(2, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = RecordFieldUtility.GetSubField(enumeration, 'b', 'z');
            Assert.AreEqual(1, subFields.Length);

            enumeration = _GetFieldEnumeration();
            subFields = RecordFieldUtility.GetSubField(enumeration, 'x', 'z');
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_GetSubField_3()
        {
            RecordField field = _GetField();
            SubField[] subFields = RecordFieldUtility.GetSubField(field, 'a');
            Assert.AreEqual(1, subFields.Length);

            subFields = RecordFieldUtility.GetSubField(field, 'b');
            Assert.AreEqual(1, subFields.Length);

            subFields = RecordFieldUtility.GetSubField(field, 'x');
            Assert.AreEqual(0, subFields.Length);
        }

        [TestMethod]
        public void RecordFieldUtility_HaveNotSubField_1()
        {
            RecordField source = _GetField();
            Assert.IsTrue(RecordFieldUtility.HaveNotSubField(source, '0'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'a'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'b'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'c'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'd'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'e'));
            Assert.IsTrue(RecordFieldUtility.HaveNotSubField(source, 'f'));
        }

        [TestMethod]
        public void RecordFieldUtility_HaveNotSubField_2()
        {
            RecordField source = _GetField();
            Assert.IsTrue(RecordFieldUtility.HaveNotSubField(source, '0', '1'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'a', 'b'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'b', '0'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'c', '1'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'd', 'f'));
            Assert.IsFalse(RecordFieldUtility.HaveNotSubField(source, 'e', 'q'));
            Assert.IsTrue(RecordFieldUtility.HaveNotSubField(source, 'f', 'g'));
        }

        [TestMethod]
        public void RecordFieldUtility_HaveSubField_1()
        {
            RecordField source = _GetField();
            Assert.IsFalse(RecordFieldUtility.HaveSubField(source, '0'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'a'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'b'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'c'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'd'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'e'));
            Assert.IsFalse(RecordFieldUtility.HaveSubField(source, 'f'));
        }

        [TestMethod]
        public void RecordFieldUtility_HaveSubField_2()
        {
            RecordField source = _GetField();
            Assert.IsFalse(RecordFieldUtility.HaveSubField(source, '0', '1'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'a', 'b'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'b', '0'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'c', '1'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'd', 'f'));
            Assert.IsTrue(RecordFieldUtility.HaveSubField(source, 'e', 'q'));
            Assert.IsFalse(RecordFieldUtility.HaveSubField(source, 'f', 'g'));
        }
    }
}
