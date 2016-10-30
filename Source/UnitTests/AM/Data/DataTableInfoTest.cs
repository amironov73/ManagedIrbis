using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class DataTableInfoTest
    {
        [TestMethod]
        public void DataTableInfo_Construction()
        {
            DataTableInfo info = new DataTableInfo();
            Assert.AreEqual(null, info.Name);
            Assert.AreEqual(0, info.Columns.Count);
        }

        [TestMethod]
        public void DataTableInfo_TableName()
        {
            const string expected = "Users";
            DataTableInfo info = new DataTableInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.Name);
        }

        [TestMethod]
        public void DataTableInfo_Columns()
        {
            DataTableInfo info = new DataTableInfo
            {
                Columns = { new DataColumnInfo() }
            };
            Assert.AreEqual(1, info.Columns.Count);
        }

        [TestMethod]
        public void DataTableInfo_ToString()
        {
            const string expected = "Users";
            DataTableInfo info = new DataTableInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.ToString());
        }
    }
}
