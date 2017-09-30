using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordComparerTest
    {
        [NotNull]
        private MarcRecord _GetFirstRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = 123,
                Index = "И 21/12345",
                Description = "Иванов. Заглавие: подзаголовочное / И. И. Иванов, П. П. Петров",
                SortKey = "Иванов. Заглавие: подзаголовочное / И. И. Иванов, П. П. Петров"
            };

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            field = new RecordField(903, "И 21/12345");
            result.Fields.Add(field);

            return result;
        }

        [NotNull]
        private MarcRecord _GetSecondRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = 321,
                Index = "П 30/56789",
                Description = "Петров. Другое заглавие: подзаголовочное / П. П. Петров, И. И. Иванов",
                SortKey = "Петров. Другое заглавие: подзаголовочное / П. П. Петров, И. И. Иванов"
            };

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Другое заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "П. П. Петров, И. И. Иванов");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);

            field = new RecordField(903, "П 30/56789");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void RecordComparer_ByMfn_1()
        {
            MarcRecord first = _GetFirstRecord();
            MarcRecord second = _GetSecondRecord();
            IComparer<MarcRecord> comparer = RecordComparer.ByMfn();
            Assert.IsTrue(comparer.Compare(first, second) < 0); 
        }

        [TestMethod]
        public void RecordComparer_ByIndex_1()
        {
            MarcRecord first = _GetFirstRecord();
            MarcRecord second = _GetSecondRecord();
            IComparer<MarcRecord> comparer = RecordComparer.ByIndex();
            Assert.IsTrue(comparer.Compare(first, second) < 0); 
        }

        [TestMethod]
        public void RecordComparer_ByDescription_1()
        {
            MarcRecord first = _GetFirstRecord();
            MarcRecord second = _GetSecondRecord();
            IComparer<MarcRecord> comparer = RecordComparer.ByDescription();
            Assert.IsTrue(comparer.Compare(first, second) < 0); 
        }

        [TestMethod]
        public void RecordComparer_BySortKey_1()
        {
            MarcRecord first = _GetFirstRecord();
            MarcRecord second = _GetSecondRecord();
            IComparer<MarcRecord> comparer = RecordComparer.BySortKey();
            Assert.IsTrue(comparer.Compare(first, second) < 0); 
        }
    }
}
