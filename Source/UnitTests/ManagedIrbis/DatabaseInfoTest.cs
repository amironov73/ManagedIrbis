using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class DatabaseInfoTest
    {
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
            DatabaseInfo database = new DatabaseInfo
            {
                Name = "IBIS",
                Description = "Catalog"
            };

            string expected = @"Name: IBIS
Description: Catalog
Max MFN: 0
Read-only: False
Database locked: False
";
            string actual = database.Describe();
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
        public void DatabaseInfo_Serialization()
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

            DatabaseInfo[] databases 
                = DatabaseInfo.ParseMenu(menu);
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
    }
}
