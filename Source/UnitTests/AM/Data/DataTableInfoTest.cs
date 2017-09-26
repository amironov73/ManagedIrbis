using AM.Data;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class DataTableInfoTest
    {
        [NotNull]
        private DataTableInfo _GetTable()
        {
            return new DataTableInfo
            {
                Name = "Readers",
                Columns =
                {
                    new DataColumnInfo
                    {
                        Name = "FIO",
                        Title = "ФИО",
                        Width = 100,
                        Type = "System.String",
                        Frozen = true
                    },
                    new DataColumnInfo
                    {
                        Name = "Age",
                        Title = "Возраст",
                        Width = 30,
                        Type = "System.Int32"
                    },
                    new DataColumnInfo
                    {
                        Name = "Ticket",
                        Title = "Билет",
                        Width = 50,
                        Type = "System.String",
                        ReadOnly = true
                    }
                }
            };
        }

        [TestMethod]
        public void DataTableInfo_Construction_1()
        {
            DataTableInfo table = new DataTableInfo();
            Assert.IsNull(table.Name);
            Assert.AreEqual(0, table.Columns.Count);
            Assert.IsNull(table.UserData);
        }

        [TestMethod]
        public void DataTableInfo_TableName_1()
        {
            const string expected = "Users";
            DataTableInfo table = new DataTableInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, table.Name);
        }

        [TestMethod]
        public void DataTableInfo_ToString_1()
        {
            const string expected = "Users";
            DataTableInfo table = new DataTableInfo
            {
                Name = expected
            };
            Assert.AreEqual(expected, table.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] DataTableInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            DataTableInfo second = bytes.RestoreObjectFromMemory<DataTableInfo>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Columns.Count, second.Columns.Count);
            for (int i = 0; i < first.Columns.Count; i++)
            {
                Assert.AreEqual(first.Columns[i].Name, second.Columns[i].Name);
                Assert.AreEqual(first.Columns[i].Title, second.Columns[i].Title);
                Assert.AreEqual(first.Columns[i].Width, second.Columns[i].Width);
                Assert.AreEqual(first.Columns[i].Type, second.Columns[i].Type);
                Assert.AreEqual(first.Columns[i].DefaultValue, second.Columns[i].DefaultValue);
                Assert.AreEqual(first.Columns[i].Frozen, second.Columns[i].Frozen);
                Assert.AreEqual(first.Columns[i].Invisible, second.Columns[i].Invisible);
                Assert.AreEqual(first.Columns[i].ReadOnly, second.Columns[i].ReadOnly);
                Assert.AreEqual(first.Columns[i].Sorted, second.Columns[i].Sorted);
                Assert.IsNull(second.UserData);
            }
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void DataTableInfo_Serialization_1()
        {
            DataTableInfo table = new DataTableInfo();
            _TestSerialization(table);

            table.UserData = "User data";
            _TestSerialization(table);

            table = _GetTable();
            _TestSerialization(table);
        }

        [TestMethod]
        public void DataTableInfo_Verify_1()
        {
            DataTableInfo table = new DataTableInfo();
            Assert.IsFalse(table.Verify(false));

            table = _GetTable();
            Assert.IsTrue(table.Verify(false));
        }

        [TestMethod]
        public void DataTableInfo_ToXml_1()
        {
            DataTableInfo table = new DataTableInfo();
            Assert.AreEqual("<table />", XmlUtility.SerializeShort(table));

            table = _GetTable();
            Assert.AreEqual("<table name=\"Readers\"><column name=\"FIO\" title=\"ФИО\" width=\"100\" type=\"System.String\" frozen=\"true\" /><column name=\"Age\" title=\"Возраст\" width=\"30\" type=\"System.Int32\" /><column name=\"Ticket\" title=\"Билет\" width=\"50\" type=\"System.String\" readOnly=\"true\" /></table>", XmlUtility.SerializeShort(table));
        }

        [TestMethod]
        public void DataTableInfo_ToJson_1()
        {
            DataTableInfo table = new DataTableInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(table));

            table = _GetTable();
            Assert.AreEqual("{'columns':[{'name':'FIO','title':'ФИО','width':100,'type':'System.String','frozen':true},{'name':'Age','title':'Возраст','width':30,'type':'System.Int32'},{'name':'Ticket','title':'Билет','width':50,'type':'System.String','readOnly':true}],'name':'Readers'}", JsonUtility.SerializeShort(table));
        }
    }
}

