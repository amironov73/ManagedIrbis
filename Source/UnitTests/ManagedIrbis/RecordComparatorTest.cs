using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordComparatorTest
    {
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void RecordComparator_FindDifference_1()
        {
            MarcRecord first = _GetRecord();
            MarcRecord second = _GetRecord();

            RecordDifferenceResult result
                = RecordComparator.FindDifference
                (
                    first,
                    second
                );
            Assert.AreEqual(0, result.FirstOnly.Count);
            Assert.AreEqual(0, result.SecondOnly.Count);
            Assert.AreEqual(first.Fields.Count, result.Both.Count);
        }

        [TestMethod]
        public void RecordComparator_FindDifference_2()
        {
            MarcRecord first = new MarcRecord();
            MarcRecord second = _GetRecord();

            RecordDifferenceResult result
                = RecordComparator.FindDifference
                (
                    first,
                    second
                );
            Assert.AreEqual(0, result.FirstOnly.Count);
            Assert.AreEqual(second.Fields.Count, result.SecondOnly.Count);
            Assert.AreEqual(0, result.Both.Count);
        }

        [TestMethod]
        public void RecordComparator_FindDifference_3()
        {
            MarcRecord first = _GetRecord();
            MarcRecord second = new MarcRecord();

            RecordDifferenceResult result
                = RecordComparator.FindDifference
                (
                    first,
                    second
                );
            Assert.AreEqual(first.Fields.Count, result.FirstOnly.Count);
            Assert.AreEqual(0, result.SecondOnly.Count);
            Assert.AreEqual(0, result.Both.Count);
        }
    }
}
