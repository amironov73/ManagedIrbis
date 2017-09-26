using AM;
using AM.Data;
using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class DataColumnInfoTest
    {
        [NotNull]
        private DataColumnInfo _GetColumn()
        {
            return new DataColumnInfo
            {
                Name = "Ticket",
                Title = "Читательский билет",
                Width = 100,
                Type = "System.String"
            };
        }

        [TestMethod]
        public void DataColumnInfo_Construction_1()
        {
            DataColumnInfo info = new DataColumnInfo();
            Assert.IsNull(info.Name);
            Assert.IsNull(info.Title);
            Assert.AreEqual(0, info.Width);
            Assert.IsNull(info.Type);
            Assert.IsNull(info.DefaultValue);
            Assert.IsFalse(info.Frozen);
            Assert.IsFalse(info.Invisible);
            Assert.IsFalse(info.ReadOnly);
            Assert.IsFalse(info.Sorted);
            Assert.IsNull(info.UserData);
        }

        [TestMethod]
        public void DataColulnInfo_Name_1()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.Name);
        }

        [TestMethod]
        public void DataColulnInfo_Title_1()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Title = expected
            };
            Assert.AreEqual(expected, info.Title);
        }

        [TestMethod]
        public void DataColulnInfo_Type_1()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                Type = expected
            };
            Assert.AreEqual(expected, info.Type);
        }

        [TestMethod]
        public void DataColulnInfo_DefaultValue_1()
        {
            const string expected = "Hello";
            DataColumnInfo info = new DataColumnInfo
            {
                DefaultValue = expected
            };
            Assert.AreEqual(expected, info.DefaultValue);
        }

        [TestMethod]
        public void DataColulnInfo_Width_1()
        {
            const int expected = 123;
            DataColumnInfo info = new DataColumnInfo
            {
                Width = expected
            };
            Assert.AreEqual(expected, info.Width);
        }

        [TestMethod]
        public void DataColulnInfo_Frozen_1()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Frozen = expected
            };
            Assert.AreEqual(expected, info.Frozen);
        }

        [TestMethod]
        public void DataColulnInfo_Invisible_1()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Invisible = expected
            };
            Assert.AreEqual(expected, info.Invisible);
        }

        [TestMethod]
        public void DataColulnInfo_ReadOnly_1()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                ReadOnly = expected
            };
            Assert.AreEqual(expected, info.ReadOnly);
        }

        [TestMethod]
        public void DataColulnInfo_Sorted_1()
        {
            const bool expected = true;
            DataColumnInfo info = new DataColumnInfo
            {
                Sorted = expected
            };
            Assert.AreEqual(expected, info.Sorted);
        }

        [TestMethod]
        public void DataColumnInfo_ToString_1()
        {
            const string expected = "Users";
            DataColumnInfo info = new DataColumnInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, info.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] DataColumnInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            DataColumnInfo second = bytes.RestoreObjectFromMemory<DataColumnInfo>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Width, second.Width);
            Assert.AreEqual(first.Type, second.Type);
            Assert.AreEqual(first.DefaultValue, second.DefaultValue);
            Assert.AreEqual(first.Frozen, second.Frozen);
            Assert.AreEqual(first.Invisible, second.Invisible);
            Assert.AreEqual(first.ReadOnly, second.ReadOnly);
            Assert.AreEqual(first.Sorted, second.Sorted);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void DataColumnInfo_Serialization_1()
        {
            DataColumnInfo column = new DataColumnInfo();
            _TestSerialization(column);

            column.UserData = "User data";
            _TestSerialization(column);

            column = _GetColumn();
            _TestSerialization(column);
        }

        [TestMethod]
        public void DataColumnInfo_Verify_1()
        {
            DataColumnInfo column = new DataColumnInfo();
            Assert.IsFalse(column.Verify(false));

            column = _GetColumn();
            Assert.IsTrue(column.Verify(false));
        }

        [TestMethod]
        public void DataColumnInfo_ToXml_1()
        {
            DataColumnInfo column = new DataColumnInfo();
            Assert.AreEqual("<column />", XmlUtility.SerializeShort(column));

            column = _GetColumn();
            Assert.AreEqual("<column name=\"Ticket\" title=\"Читательский билет\" width=\"100\" type=\"System.String\" />", XmlUtility.SerializeShort(column));
        }

        [TestMethod]
        public void DataColumnInfo_ToJson_1()
        {
            DataColumnInfo column = new DataColumnInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(column));

            column = _GetColumn();
            Assert.AreEqual("{'name':'Ticket','title':'Читательский билет','width':100,'type':'System.String'}", JsonUtility.SerializeShort(column));
        }
    }
}
