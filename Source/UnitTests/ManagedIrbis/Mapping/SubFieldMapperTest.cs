using System;

using ManagedIrbis;
using ManagedIrbis.Mapping;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Mapping
{
    [TestClass]
    public class SubFieldMapperTest
    {
        [TestMethod]
        public void SubFieldMapper_ToBoolean_1()
        {
            SubField subField = new SubField('a', "Some value");
            Assert.IsTrue(SubFieldMapper.ToBoolean(subField));
            subField = new SubField('a');
            Assert.IsFalse(SubFieldMapper.ToBoolean(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToBoolean_2()
        {
            RecordField field = new RecordField(100, new SubField('a', "Some value"));
            Assert.IsTrue(SubFieldMapper.ToBoolean(field, 'a'));
            field = new RecordField(100, new SubField('a'));
            Assert.IsFalse(SubFieldMapper.ToBoolean(field, 'a'));
            Assert.IsFalse(SubFieldMapper.ToBoolean(field, 'b'));
        }

        [TestMethod]
        public void SubFieldMapper_ToChar_1()
        {
            SubField subField = new SubField('a', "Some value");
            Assert.AreEqual('S', SubFieldMapper.ToChar(subField));
            subField = new SubField('a');
            Assert.AreEqual('\0', SubFieldMapper.ToChar(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToChar_2()
        {
            RecordField field = new RecordField(100, new SubField('a', "Some value"));
            Assert.AreEqual('S', SubFieldMapper.ToChar(field, 'a'));
            field = new RecordField(100, new SubField('a'));
            Assert.AreEqual('\0', SubFieldMapper.ToChar(field, 'a'));
            Assert.AreEqual('\0', SubFieldMapper.ToChar(field, 'b'));
        }

        [TestMethod]
        public void SubFieldMapper_ToDateTime_1()
        {
            SubField subField = new SubField('a', "20180321");
            Assert.AreEqual(new DateTime(2018, 3, 21), SubFieldMapper.ToDateTime(subField));
            subField = new SubField('a');
            Assert.AreEqual(DateTime.MinValue, SubFieldMapper.ToDateTime(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToDateTime_2()
        {
            RecordField field = new RecordField(100, new SubField('a', "20180321"));
            DateTime? expected = new DateTime(2018, 3, 21);
            Assert.AreEqual(expected, SubFieldMapper.ToDateTime(field, 'a'));
            field = new RecordField(100, new SubField('a'));
            expected = DateTime.MinValue;
            Assert.AreEqual(expected, SubFieldMapper.ToDateTime(field, 'a'));
            expected = null;
            Assert.AreEqual(expected, SubFieldMapper.ToDateTime(field, 'b'));
        }

        [TestMethod]
        public void SubFieldMapper_ToDecimal_1()
        {
            SubField subField = new SubField('a', "123.456");
            Assert.AreEqual(123.456m, SubFieldMapper.ToDecimal(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual(0.0m, SubFieldMapper.ToDecimal(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToDouble_1()
        {
            SubField subField = new SubField('a', "123.456");
            Assert.AreEqual(123.456, SubFieldMapper.ToDouble(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual(0.0, SubFieldMapper.ToDouble(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToSingle_1()
        {
            SubField subField = new SubField('a', "123.456");
            Assert.AreEqual(123.456f, SubFieldMapper.ToSingle(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual(0.0f, SubFieldMapper.ToDouble(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToInt16_1()
        {
            SubField subField = new SubField('a', "123");
            Assert.AreEqual((short)123, SubFieldMapper.ToInt16(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual((short)0, SubFieldMapper.ToInt16(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToInt32_1()
        {
            SubField subField = new SubField('a', "123");
            Assert.AreEqual(123, SubFieldMapper.ToInt32(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual(0, SubFieldMapper.ToInt32(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToInt32_2()
        {
            RecordField field = new RecordField(100, new SubField('a', "123"));
            int? expected = 123;
            Assert.AreEqual(expected, SubFieldMapper.ToInt32(field, 'a'));
            expected = 0;
            field = new RecordField(100, new SubField('a'));
            Assert.AreEqual(expected, SubFieldMapper.ToInt32(field, 'a'));
            expected = null;
            Assert.AreEqual(expected, SubFieldMapper.ToInt32(field, 'b'));
        }

        [TestMethod]
        public void SubFieldMapper_ToInt64_1()
        {
            SubField subField = new SubField('a', "123");
            Assert.AreEqual(123L, SubFieldMapper.ToInt64(subField));
            subField = new SubField('a', "Wrong");
            Assert.AreEqual(0L, SubFieldMapper.ToInt64(subField));
        }

        [TestMethod]
        public void SubFieldMapper_ToInt64_2()
        {
            RecordField field = new RecordField(100, new SubField('a', "123"));
            long? expected = 123;
            Assert.AreEqual(expected, SubFieldMapper.ToInt64(field, 'a'));
            expected = 0;
            field = new RecordField(100, new SubField('a'));
            Assert.AreEqual(expected, SubFieldMapper.ToInt64(field, 'a'));
            expected = null;
            Assert.AreEqual(expected, SubFieldMapper.ToInt64(field, 'b'));
        }

        [TestMethod]
        public void SubFieldMapper_ToString_1()
        {
            string value = "Some value";
            SubField subField = new SubField('a', value);
            Assert.AreSame(value, SubFieldMapper.ToString(subField));

            value = null;
            subField = new SubField('a', value);
            Assert.AreSame(value, SubFieldMapper.ToString(subField));
        }
    }
}
