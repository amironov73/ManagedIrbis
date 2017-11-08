using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class DatabaseInfoTest
    {
        [NotNull]
        private DatabaseInfo _GetDatabase()
        {
            return new DatabaseInfo
            {
                Name = "IBIS",
                Description = "Электронный каталог"
            };
        }

        [TestMethod]
        public void DatabaseInfo_Constructor_1()
        {
            DatabaseInfo database = new DatabaseInfo();
            Assert.IsNull(database.Name);
            Assert.IsNull(database.Description);
            Assert.AreEqual(0, database.MaxMfn);
            Assert.IsNull(database.LogicallyDeletedRecords);
            Assert.IsNull(database.PhysicallyDeletedRecords);
            Assert.IsNull(database.NonActualizedRecords);
            Assert.IsNull(database.LogicallyDeletedRecords);
            Assert.IsFalse(database.DatabaseLocked);
            Assert.IsFalse(database.ReadOnly);
        }

        [TestMethod]
        public void DatabaseInfo_Describe_1()
        {
            DatabaseInfo database = _GetDatabase();

            string expected = "Name: IBIS\nDescription: Электронный каталог\nMax MFN: 0\nRead-only: False\nDatabase locked: False\n";
            string actual = database.Describe().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DatabaseInfo_Describe_2()
        {
            DatabaseInfo database = _GetDatabase();
            database.LogicallyDeletedRecords = new[] {1, 2, 3};
            database.PhysicallyDeletedRecords = new[] {4, 5, 6};
            database.LockedRecords = new[] {7, 8, 9};
            database.NonActualizedRecords = new[] {10, 11, 12};
            database.MaxMfn = 1000;

            string expected = "Name: IBIS\nDescription: Электронный каталог\nLogically deleted records: 1-3\nPhysically deleted records: 4-6\nNon-actualized records: 10-12\nLocked records: 7-9\nMax MFN: 1000\nRead-only: False\nDatabase locked: False\n";
            string actual = database.Describe().DosToUnix();
            Assert.AreEqual(expected, actual);
        }


        private void _TestSerialization
            (
                DatabaseInfo source
            )
        {
            byte[] bytes = source.SaveToMemory();

            DatabaseInfo target 
                = bytes.RestoreObjectFromMemory<DatabaseInfo>();

            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.Description, target.Description);
            Assert.AreEqual(source.MaxMfn, target.MaxMfn);
            Assert.AreEqual(source.DatabaseLocked, target.DatabaseLocked);
            Assert.AreEqual(source.ReadOnly, target.ReadOnly);
        }

        [TestMethod]
        public void DatabaseInfo_Serialization_1()
        {
            DatabaseInfo database = new DatabaseInfo();
            _TestSerialization(database);

            database = new DatabaseInfo
                {
                    Name = "IBIS",
                    Description = "Catalog"
                };
            _TestSerialization(database);
        }

        [TestMethod]
        public void DatabaseInfo_ParseMenu_1()
        {
            string[] menu =
            {
                "IBIS",
                "Catalog",
                "-RDR",
                "Readers",
                "*****"
            };

            DatabaseInfo[] databases = DatabaseInfo.ParseMenu(menu);
            Assert.AreEqual(2, databases.Length);
            Assert.AreEqual("IBIS", databases[0].Name);
            Assert.AreEqual("Catalog", databases[0].Description);
            Assert.IsFalse(databases[0].ReadOnly);
            Assert.AreEqual("RDR", databases[1].Name);
            Assert.AreEqual("Readers", databases[1].Description);
            Assert.IsTrue(databases[1].ReadOnly);
        }

        [TestMethod]
        public void DatabaseInfo_ToString_1()
        {
            DatabaseInfo database = new DatabaseInfo();
            string expected = "(null)";
            string actual = database.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DatabaseInfo_ToString_2()
        {
            DatabaseInfo database = new DatabaseInfo
            {
                Name = "IBIS"
            };
            string expected = "IBIS";
            string actual = database.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DatabaseInfo_ToString_3()
        {
            DatabaseInfo database = new DatabaseInfo
            {
                Name = "IBIS",
                Description = "Catalog"
            };
            string expected = "IBIS - Catalog";
            string actual = database.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DatabaseInfo_ParseServerResponse_1()
        {
            ResponseBuilder builder = new ResponseBuilder();

            // Physically deleted records
            builder.AppendUtf("1\x001E2\x001E3").NewLine()

            // Logically deleted records
            .AppendUtf("4\x001E5\x001E6").NewLine()

            // Non-actualized records
            .AppendUtf("7\x001E8\x001E9").NewLine()

            // Locked records
            .AppendUtf("10\x001E11\x001E12").NewLine()

            // Maximal MFN
            .AppendUtf("1000").NewLine()

            // Database locked?
            .AppendUtf("0").NewLine();

            IrbisConnection connection = new IrbisConnection();
            byte[] query = new byte[0];
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );

            DatabaseInfo database = DatabaseInfo.ParseServerResponse(response);
            Assert.AreEqual
                (
                    "Name: (null)\nDescription: (null)\nLogically deleted records: 4-6\nPhysically deleted records: 1-3\nNon-actualized records: 7-9\nLocked records: 10-12\nMax MFN: 1000\nRead-only: False\nDatabase locked: False\n",
                    database.Describe().DosToUnix()
                );
        }

        [TestMethod]
        public void DatabaseInfo_ParseServerResponse_2()
        {
            ResponseBuilder builder = new ResponseBuilder();

            // Physically deleted records
            builder.NewLine()

                // Logically deleted records
                .NewLine()

                // Non-actualized records
                .AppendUtf("7\x001E8\x001E9").NewLine()

                // Locked records
                .AppendUtf("10\x001E11\x001E12").NewLine()

                // Maximal MFN
                .AppendUtf("1000").NewLine()

                // Database locked?
                .AppendUtf("0").NewLine();

            IrbisConnection connection = new IrbisConnection();
            byte[] query = new byte[0];
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );

            DatabaseInfo database = DatabaseInfo.ParseServerResponse(response);
            Assert.AreEqual
                (
                    "Name: (null)\nDescription: (null)\nLogically deleted records: \nPhysically deleted records: \nNon-actualized records: 7-9\nLocked records: 10-12\nMax MFN: 1000\nRead-only: False\nDatabase locked: False\n",
                    database.Describe().DosToUnix()
                );
        }
    }
}
