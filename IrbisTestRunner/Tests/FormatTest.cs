/* FormatTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using ManagedClient;
using ManagedClient.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class FormatTest
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
        public void TestFormatOneRecord()
        {
            string actual = Connection.FormatRecord
                (
                    "@brief",
                    1
                );
            Console.Write(actual);
        }

        [TestMethod]
        public void TestFormatVirtualRecord()
        {
            IrbisRecord record = _GetRecord();
            string actual = Connection.FormatRecord
                (
                    "@brief",
                    record
                );
            Console.Write(actual);
        }

        [TestMethod]
        public void TestFormatThreeRecords()
        {
            string[] actual = Connection.FormatRecords
                (
                    "@brief",
                    new[] { 1, 2, 3 }
                );
            Console.Write(string.Join(Environment.NewLine, actual));
        }

        #endregion
    }
}
