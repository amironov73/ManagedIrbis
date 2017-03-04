using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class FieldFilterTest
    {
        private FieldFilter _GetFilter()
        {
            AbstractClient client = new LocalClient();
            FieldFilter result = new FieldFilter
                (
                    client,
                    "v200^a = 'Второе'"
                );

            return result;
        }

        [TestMethod]
        public void FieldFilter_Constructor_1()
        {
            FieldFilter filter = _GetFilter();

            Assert.IsNotNull(filter.Formatter);
            Assert.IsNotNull(filter.Formatter.Program);
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(new RecordField("100", "^aПервое"));
            result.Fields.Add(new RecordField("100", "^aВторое"));
            result.Fields.Add(new RecordField("100", "^aТретье"));
            result.Fields.Add(new RecordField("200", "^aПервое"));
            result.Fields.Add(new RecordField("200", "^aВторое"));
            result.Fields.Add(new RecordField("200", "^aТретье"));
            result.Fields.Add(new RecordField("300", "^aПервое"));
            result.Fields.Add(new RecordField("300", "^aВторое"));
            result.Fields.Add(new RecordField("300", "^aТретье"));

            return result;
        }

        [TestMethod]
        public void FieldFilter_CheckField_1()
        {
            FieldFilter filter = _GetFilter();

            RecordField field100_1 = new RecordField("100", "^aПервое");
            RecordField field100_2 = new RecordField("100", "^aВторое");
            RecordField field200_1 = new RecordField("200", "^aПервое");
            RecordField field200_2 = new RecordField("200", "^aВторое");

            Assert.IsFalse(filter.CheckField(field100_1));
            Assert.IsFalse(filter.CheckField(field100_2));
            Assert.IsFalse(filter.CheckField(field200_1));
            Assert.IsTrue(filter.CheckField(field200_2));
        }

        [TestMethod]
        public void FieldFilter_AllFields_1()
        {
            FieldFilter filter = _GetFilter();
            MarcRecord record = _GetRecord();

            Assert.IsFalse(filter.AllFields(record.Fields));
        }

        [TestMethod]
        public void FieldFilter_AnyField_1()
        {
            FieldFilter filter = _GetFilter();
            MarcRecord record = _GetRecord();

            Assert.IsTrue(filter.AnyField(record.Fields));
        }

        [TestMethod]
        public void FieldFilter_FilterFields_1()
        {
            FieldFilter filter = _GetFilter();
            MarcRecord record = _GetRecord();

            RecordField[] filtered = filter.FilterFields
                (
                    record.Fields
                );
            Assert.AreEqual(1, filtered.Length);
            Assert.AreEqual
                (
                    "^aВторое",
                    filtered[0].ToText()
                );
        }

        [TestMethod]
        public void FieldFilter_First_1()
        {
            FieldFilter filter = _GetFilter();
            MarcRecord record = _GetRecord();

            RecordField found = filter.First(record.Fields);
            Assert.IsNotNull(found);
            Assert.AreEqual
                (
                    "^aВторое",
                    found.ToText()
                );
        }

        [TestMethod]
        public void FieldFilter_Last_1()
        {
            FieldFilter filter = _GetFilter();
            MarcRecord record = _GetRecord();

            RecordField found = filter.Last(record.Fields);
            Assert.IsNotNull(found);
            Assert.AreEqual
                (
                    "^aВторое",
                    found.ToText()
                );
        }
    }
}
