using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class DataColumnInfoTest
    {
        [TestMethod]
        public void DataColumnInfo_Construction()
        {
            DataColumnInfo info = new DataColumnInfo();
            Assert.AreEqual(null, info.Name);
            Assert.AreEqual(null, info.Title);
            Assert.AreEqual(0, info.Width);
            Assert.AreEqual(null, info.Type);
            Assert.AreEqual(null, info.DefaultValue);
            Assert.AreEqual(false, info.Frozen);
            Assert.AreEqual(false, info.Invisible);
            Assert.AreEqual(false, info.ReadOnly);
            Assert.AreEqual(false, info.Sorted);
        }

        [TestMethod]
        public void DataColulnInfo_Name()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.Name);
        }

        [TestMethod]
        public void DataColulnInfo_Title()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Title = expected
            };
            Assert.AreEqual(expected, info.Title);
        }

        [TestMethod]
        public void DataColulnInfo_Type()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Type = expected
            };
            Assert.AreEqual(expected, info.Type);
        }

        [TestMethod]
        public void DataColulnInfo_DefaultValue()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                DefaultValue = expected
            };
            Assert.AreEqual(expected, info.DefaultValue);
        }

        [TestMethod]
        public void DataColulnInfo_Width()
        {
            const int expected = 123;
            DataColumnInfo info = new DataColumnInfo
            {
                Width = expected
            };
            Assert.AreEqual(expected, info.Width);
        }

        [TestMethod]
        public void DataColulnInfo_Frozen()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Frozen = expected
            };
            Assert.AreEqual(expected, info.Frozen);
        }

        [TestMethod]
        public void DataColulnInfo_Invisible()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Invisible = expected
            };
            Assert.AreEqual(expected, info.Invisible);
        }

        [TestMethod]
        public void DataColulnInfo_ReadOnly()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                ReadOnly = expected
            };
            Assert.AreEqual(expected, info.ReadOnly);
        }

        [TestMethod]
        public void DataColulnInfo_Sorted()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Sorted = expected
            };
            Assert.AreEqual(expected, info.Sorted);
        }

        [TestMethod]
        public void DataColumnInfo_ToString()
        {
            const string expected = "Users";
            DataColumnInfo info = new DataColumnInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.ToString());
        }
    }
}
