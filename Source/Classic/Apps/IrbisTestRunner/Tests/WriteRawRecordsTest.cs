/* WriteRawRecordsTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class WriteRawRecordsTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private MarcRecord _GetRecord
            (
                int number
            )
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
            field.AddSubField('a', "Заглавие " + number);
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
        public void WriteRawRecords_Test1()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");
            MarcRecord[] records =
            {
                _GetRecord(1),
                _GetRecord(2),
                _GetRecord(3)
            };

            string[] rawRecords =
            {
                ProtocolText.EncodeRecord(records[0]),
                ProtocolText.EncodeRecord(records[1]),
                ProtocolText.EncodeRecord(records[2])
            };
            string[] result = connection.WriteRawRecords
                (
                    "IBIS",
                    rawRecords,
                    false,
                    true
                );

            string text = string.Join
                (
                    Environment.NewLine,
                    result
                );

            Write(text);
        }

        #endregion
    }
}
