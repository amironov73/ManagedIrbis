/* WriteRecordTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class WriteRecordTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private IrbisRecord _GetRecord()
        {
            IrbisRecord result = new IrbisRecord();

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

            field = new RecordField("210");
            field.AddSubField('a', "Иркутск");
            field.AddSubField('d', "2016");
            result.Fields.Add(field);

            field = new RecordField("215");
            field.AddSubField('a', "123");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            field = new RecordField("920", "PAZK");
            result.Fields.Add(field);

            return result;
        }

        #endregion

        #region Public methods

        [TestMethod]
        public void TestWriteRecord()
        {
            IrbisRecord record = _GetRecord();
            record = Connection.WriteRecord(record);
            Console.Write
                (
                    record.ToPlainText().Substring(0,50).Trim()
                );
        }

        #endregion
    }
}
