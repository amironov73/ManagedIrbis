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
        public void Search_Few()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            int[] found = connection.Search("T=A$");
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
        public void Search_Many()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string saveDatabase = connection.Database;
            try
            {
                // connection.Database = "ISTU";
                int[] found = connection.Search("K=А$");
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
                connection.Database = saveDatabase;
            }
        }

        [TestMethod]
        public void Search_Format()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            FoundItem[] found = connection.SearchFormat
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
        public void Search_Format_Utf8()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            FoundItem[] found = connection.SearchFormatUtf8
                (
                    "T=A$",
                    "v200^a, ' Привет, мир!'"
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
        public void Search_Count()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            int result = connection.SearchCount("T=A$");

            Write("Found: " + result);
        }

        [TestMethod]
        public void Search_Read()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            MarcRecord[] records = connection.SearchRead("T=A$");

            Write(records.Length);
        }

        [TestMethod]
        public void Search_ReadOneRecord()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            MarcRecord record = connection.SearchReadOneRecord("T=A$");

            Write(record.NullableToVisibleString().Substring(0,50));
        }

        #endregion
    }
}
