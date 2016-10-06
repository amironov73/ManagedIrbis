using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisConnectionTest
    {
        const string ConnectionString 
            = "host=127.0.0.1;port=6666;user=1;password=1;";

        [TestMethod]
        public void TestIrbisConnection_Constructor()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                Assert.IsNotNull(client);
            }
        }

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

        private void _TestSerialization
            (
                IrbisConnection first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisConnection second = bytes
                .RestoreObjectFromMemory<IrbisConnection>();

            Assert.IsNotNull(second);
        }

        [TestMethod]
        public void TestIrbisConnection_Serialization()
        {
            IrbisConnection client = new IrbisConnection();
            _TestSerialization(client);
        }

        [TestMethod]
        public void TestIrbisConnection_Clone()
        {
            IrbisConnection first = new IrbisConnection();
            first.ParseConnectionString(ConnectionString);

            IrbisConnection second = first.Clone(false);

            Assert.AreEqual (first.Host, second.Host);
            Assert.AreEqual(first.Port, second.Port);
            Assert.AreEqual(first.Username, second.Username);
            Assert.AreEqual(first.Password, second.Password);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Workstation, second.Workstation);
        }
    }
}
