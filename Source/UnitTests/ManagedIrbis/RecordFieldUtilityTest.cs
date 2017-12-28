using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
