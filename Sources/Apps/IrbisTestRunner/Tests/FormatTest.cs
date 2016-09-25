/* FormatTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using AM;

using ManagedIrbis;
using ManagedIrbis.Testing;

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
        public void Format_Verbatim()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string actual = connection.FormatRecord
                (
                    "'Привет, мир!'",
                    1
                );
            Write(actual);
        }

        [TestMethod]
        public void Format_OneRecord_Test1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string actual = connection.FormatRecord
                (
                    "@brief",
                    1
                );
            Write(actual);
        }

        [TestMethod]
        public void Format_OneRecord_Test2()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string actual = connection.FormatRecord
                (
                    "v200^a,/,v200^e",
                    1
                );
            Write(actual);
        }

        [TestMethod]
        public void Format_VirtualRecord()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            MarcRecord record = _GetRecord();
            string actual = connection.FormatRecord
                (
                    "@brief",
                    record
                );
            Write(actual);
        }

        [TestMethod]
        public void Format_ThreeRecords()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string[] actual = connection.FormatRecords
                (
                    "IBIS",
                    "@brief",
                    new[] { 1, 2, 3 }
                );
            Write(string.Join(Environment.NewLine, actual));
        }

        [TestMethod]
        public void Format_Optimized()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string actual = connection.FormatRecord
                (
                    "@",
                    1
                );
            Write(actual);
        }

        [TestMethod]
        public void Format_Bang()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string actual = connection.FormatRecord
                (
                    "!'Привет, мир!'",
                    1
                );
            Write(actual);
        }

        #endregion
    }
}
