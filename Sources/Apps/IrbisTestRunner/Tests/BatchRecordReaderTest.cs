/* BatchRecordReaderTest.cs --
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
using ManagedIrbis.Batch;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class BatchRecordReaderTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [TestMethod]
        public void TestBatchRecordReader_ManyRecords()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            BatchRecordReader reader = new BatchRecordReader
                (
                    connection,
                    "IBIS",
                    100,
                    Enumerable.Range(1, 150)
                );
            List<MarcRecord> records = reader.ReadAll();
            string text = StringUtility.Join
                (
                    ", ",
                    records.Select(r => r.Mfn)
                );
            Write("MFN: {0}", text);
        }

        [TestMethod]
        public void TestBatchRecordReader_OneRecord()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            BatchRecordReader reader = new BatchRecordReader
                (
                    connection,
                    "IBIS",
                    100,
                    new[] { 1 }
                );
            List<MarcRecord> records = reader.ReadAll();
            string text = StringUtility.Join
                (
                    ", ",
                    records.Select(r => r.Mfn)
                );
            Write("MFN: {0}", text);
        }

        [TestMethod]
        public void TestBatchRecordReader_NoRecords()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            BatchRecordReader reader = new BatchRecordReader
                (
                    connection,
                    "IBIS",
                    100,
                    new int[0]
                );
            List<MarcRecord> records = reader.ReadAll();
            Write("readed: {0}", records.Count);
        }

        #endregion
    }
}
