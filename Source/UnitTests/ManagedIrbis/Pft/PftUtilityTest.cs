using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftUtilityTest
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
        public void PftUtility_GetFieldCount_1()
        {
            MarcRecord record = _GetRecord();
            PftContext context = new PftContext(null)
            {
                Record = record
            };

            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 701));
            Assert.AreEqual(0, PftUtility.GetFieldCount(context, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 701));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300, 700));
        }
    }
}
