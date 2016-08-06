/* SearchTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Search;
using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class SearchTest
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
        public void TestSearch_Few()
        {
            int[] found = Connection.Search("T=A$");
            Write
                (
                    string.Join
                    (
                        ", ",
                        found.Select(mfn => mfn.ToInvariantString())
                    )
                    .Substring(0,50)
                );
        }

        [TestMethod]
        public void TestSearch_Many()
        {
            string saveDatabase = Connection.Database;
            try
            {
                //Connection.Database = "ISTU";
                int[] found = Connection.Search("K=А$");
                Write
                    (
                        "Found: " + found.Length + ": "
                        + string.Join
                            (
                                ", ",
                                found.Select
                                (
                                    mfn => mfn.ToInvariantString()
                                )
                            )
                            .Substring(0, 50)
                    );
            }
            finally
            {
                Connection.Database = saveDatabase;
            }
        }

        [TestMethod]
        public void TestSearch_Format()
        {
            FoundItem[] found = Connection.SearchFormat
                (
                    "T=A$",
                    IrbisFormat.Brief
                );
            Write
                (
                    "Found: " + found.Length + ": "
                    + string.Join
                        (
                            "| ",
                            found.Select
                                (
                                    item => item.ToString()
                                        .SafeSubstring(0, 20)
                                )
                        )
                        .SafeSubstring(0, 100)
                );
        }

        [TestMethod]
        public void TestSearch_Count()
        {
            int result = Connection.SearchCount("T=A$");

            Write("Found: " + result);
        }

        [TestMethod]
        public void TestSearch_Read()
        {
            MarcRecord[] records = Connection.SearchRead("T=A$");

            Write(records.Length);
        }

        [TestMethod]
        public void TestSearch_ReadOneRecord()
        {
            MarcRecord record = Connection.SearchReadOneRecord("T=A$");

            Write(record.NullableToVisibleString().Substring(0,50));
        }

        #endregion
    }
}
