/* ReadRecordTest.cs
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
    class ReadRecordTest
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
        public void ReadRecord_SimpleRead()
        {
            MarcRecord record = Connection.ReadRecord(1);
            Write
                (
                    record.ToPlainText().Substring(0,50).Trim()
                );
        }

        [TestMethod]
        public void ReadRecord_ReadAndFormat()
        {

            MarcRecord record = Connection.ReadRecord
                (
                    "IBIS",
                    1,
                    false,
                    "@brief"
                );
            Write
                (
                    record.ToPlainText().Substring(0, 50).Trim()
                );
            Write(" | ");
            Write(record.Description);
        }

        [TestMethod]
        public void ReadRecord_MultipleRecords()
        {
            MarcRecord[] records = Connection.ReadRecords
                (
                    "IBIS",
                    new[] { 1, 2, 3 }
                );
            Write
                (
                    string.Join
                    (
                        " | ",
                        records.Select(r=>r.ToPlainText()
                            .SafeSubstring(0,50).Trim())
                    )
                );
        }

        #endregion
    }
}
