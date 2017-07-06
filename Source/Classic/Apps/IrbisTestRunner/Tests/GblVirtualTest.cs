/* GblVirtualTest.cs --
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
using ManagedIrbis.Gbl;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class GblVirtualTest
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
        public void GblVirtualRecord_Test1()
        {
            MarcRecord record = _GetRecord();
            IrbisConnection connection = Connection.ThrowIfNull();
            GblBuilder builder = new GblBuilder()
                .Add("4000", "'Field 4000'");

            MarcRecord result = connection.CorrectVirtualRecord
                (
                    "IBIS",
                    record,
                    builder.ToStatements()
                );
            Write(result);
        }

        [TestMethod]
        public void GblVirtualRecord_Test2()
        {
            MarcRecord record = _GetRecord();
            IrbisConnection connection = Connection.ThrowIfNull();

            MarcRecord result = connection.CorrectVirtualRecord
                (
                    "IBIS",
                    record,
                    "@test"
                );
            Write(result);
        }

        #endregion
    }
}
