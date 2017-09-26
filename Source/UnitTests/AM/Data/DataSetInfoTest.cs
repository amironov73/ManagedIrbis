using System;
using System.IO;

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
    public class DataSetInfoTest
    {
        [NotNull]
        private DataSetInfo _GetDataSet()
        {
            return new DataSetInfo
            {
                ConnectionString = "DataSource=(local);Initial Catalog=Library;",
                Tables =
                {
                    new DataTableInfo
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
                    },
                    new DataTableInfo
                    {
                        Name = "Books",
                        Columns =
                        {
                            new DataColumnInfo
                            {
                                Name = "Author",
                                Title = "Автор",
                                Width = 100,
                                Type = "System.String",
                                Frozen = true
                            },
                            new DataColumnInfo
                            {
                                Name = "Title",
                                Title = "Заглавие",
                                Width = 150,
                                Type = "System.String",
                                ReadOnly = true
                            },
                            new DataColumnInfo
                            {
                                Name = "Price",
                                Title = "Цена",
                                Width = 30,
                                Type = "System.Decimal"
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void DataSetInfo_Construction_1()
        {
            DataSetInfo dataSet = new DataSetInfo();
            Assert.IsNull(dataSet.ConnectionString);
            Assert.IsFalse(dataSet.ReadOnly);
            Assert.IsNull(dataSet.SelectCommandText);
            Assert.AreEqual(0, dataSet.Tables.Count);
            Assert.IsNull(dataSet.UserData);
        }

        [TestMethod]
        public void DataSetInfo_ConnectionString_1()
        {
            const string expected = "server=(local)";
            DataSetInfo dataSet = new DataSetInfo
            {
                ConnectionString = expected
            };
            Assert.AreEqual(expected, dataSet.ConnectionString);
        }

        [TestMethod]
        public void DataSetInfo_ReadOnly_1()
        {
            const bool expected = true;
            DataSetInfo dataSet = new DataSetInfo
            {
                ReadOnly = expected
            };
            Assert.AreEqual(expected, dataSet.ReadOnly);
        }

        [TestMethod]
        public void DataSetInfo_SelectCommandText_1()
        {
            const string expected = "select * from users";
            DataSetInfo dataSet = new DataSetInfo
            {
                SelectCommandText = expected
            };
            Assert.AreEqual(expected, dataSet.SelectCommandText);
        }

        private void _TestSerialization
            (
                [NotNull] DataSetInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            DataSetInfo second = bytes.RestoreObjectFromMemory<DataSetInfo>();
            Assert.AreEqual(first.ConnectionString, second.ConnectionString);
            Assert.AreEqual(first.ReadOnly, second.ReadOnly);
            Assert.AreEqual(first.SelectCommandText, second.SelectCommandText);
            Assert.AreEqual(first.Tables.Count, second.Tables.Count);
            for (int i = 0; i < first.Tables.Count; i++)
            {
                Assert.AreEqual(first.Tables[i].Name, second.Tables[i].Name);
                Assert.AreEqual(first.Tables[i].Columns.Count, second.Tables[i].Columns.Count);
                for (int j = 0; j < first.Tables[i].Columns.Count; j++)
                {
                    Assert.AreEqual(first.Tables[i].Columns[j].Name, second.Tables[i].Columns[j].Name);
                    Assert.AreEqual(first.Tables[i].Columns[j].Title, second.Tables[i].Columns[j].Title);
                    Assert.AreEqual(first.Tables[i].Columns[j].Width, second.Tables[i].Columns[j].Width);
                    Assert.AreEqual(first.Tables[i].Columns[j].Type, second.Tables[i].Columns[j].Type);
                    Assert.AreEqual(first.Tables[i].Columns[j].DefaultValue, second.Tables[i].Columns[j].DefaultValue);
                    Assert.AreEqual(first.Tables[i].Columns[j].Frozen, second.Tables[i].Columns[j].Frozen);
                    Assert.AreEqual(first.Tables[i].Columns[j].Invisible, second.Tables[i].Columns[j].Invisible);
                    Assert.AreEqual(first.Tables[i].Columns[j].ReadOnly, second.Tables[i].Columns[j].ReadOnly);
                    Assert.AreEqual(first.Tables[i].Columns[j].Sorted, second.Tables[i].Columns[j].Sorted);
                    Assert.IsNull(second.UserData);
                }
                Assert.IsNull(second.Tables[i].UserData);
            }
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void DataSetInfo_Serialization_1()
        {
            DataSetInfo dataSet = new DataSetInfo();
            _TestSerialization(dataSet);

            dataSet.UserData = "User data";
            _TestSerialization(dataSet);

            dataSet = _GetDataSet();
            _TestSerialization(dataSet);
        }

        [TestMethod]
        public void DataSetInfo_Load_Save_1()
        {
            string fileName = Path.GetTempFileName();

            DataSetInfo info1 = new DataSetInfo
            {
                ConnectionString = "host=(local)",
                ReadOnly = true,
                SelectCommandText = "select * from users",
                Tables =
                {
                    new DataTableInfo
                    {
                        Name = "Users",
                        Columns =
                        {
                            new DataColumnInfo
                            {
                                Name ="ID",
                                Type = "int"
                            },
                            new DataColumnInfo
                            {
                                Name="FirstName",
                                Type = "string"
                            },
                            new DataColumnInfo
                            {
                                Name = "LatName",
                                Type = "string"
                            }
                        }
                    }
                }
            };

            info1.Save(fileName);

            DataSetInfo info2 = DataSetInfo.Load(fileName);
            Assert.AreEqual(info1.ConnectionString, info2.ConnectionString);
            Assert.AreEqual(info1.ReadOnly, info2.ReadOnly);
            Assert.AreEqual(info1.SelectCommandText, info2.SelectCommandText);
            Assert.AreEqual(info1.Tables.Count, info2.Tables.Count);
            Assert.AreEqual(info1.Tables[0].Name, info2.Tables[0].Name);
            Assert.AreEqual(info1.Tables[0].Columns.Count, info2.Tables[0].Columns.Count);
            for (int i = 0; i < info1.Tables[0].Columns.Count; i++)
            {
                DataColumnInfo column1 = info1.Tables[0].Columns[i];
                DataColumnInfo column2 = info2.Tables[0].Columns[i];
                Assert.AreEqual(column1.DefaultValue, column2.DefaultValue);
                Assert.AreEqual(column1.Invisible, column2.Invisible);
                Assert.AreEqual(column1.Name, column2.Name);
                Assert.AreEqual(column1.ReadOnly, column2.ReadOnly);
                Assert.AreEqual(column1.Sorted, column2.Sorted);
                Assert.AreEqual(column1.Title, column2.Title);
                Assert.AreEqual(column1.Type, column2.Type);
                Assert.AreEqual(column1.Width, column2.Width);
                Assert.AreEqual(column1.Frozen, column2.Frozen);
            }
        }

        [TestMethod]
        public void DataSetInfo_Verify_1()
        {
            DataSetInfo dataSet = new DataSetInfo();
            Assert.IsTrue(dataSet.Verify(false));

            dataSet = _GetDataSet();
            Assert.IsTrue(dataSet.Verify(false));
        }

        [TestMethod]
        public void DataSetInfo_ToXml_1()
        {
            DataSetInfo dataSet = new DataSetInfo();
            Assert.AreEqual("<dataset />", XmlUtility.SerializeShort(dataSet));

            dataSet = _GetDataSet();
            Assert.AreEqual("<dataset><connectionString>DataSource=(local);Initial Catalog=Library;</connectionString><table name=\"Readers\"><column name=\"FIO\" title=\"ФИО\" width=\"100\" type=\"System.String\" frozen=\"true\" /><column name=\"Age\" title=\"Возраст\" width=\"30\" type=\"System.Int32\" /><column name=\"Ticket\" title=\"Билет\" width=\"50\" type=\"System.String\" readOnly=\"true\" /></table><table name=\"Books\"><column name=\"Author\" title=\"Автор\" width=\"100\" type=\"System.String\" frozen=\"true\" /><column name=\"Title\" title=\"Заглавие\" width=\"150\" type=\"System.String\" readOnly=\"true\" /><column name=\"Price\" title=\"Цена\" width=\"30\" type=\"System.Decimal\" /></table></dataset>", XmlUtility.SerializeShort(dataSet));
        }

        [TestMethod]
        public void DataSetInfo_ToJson_1()
        {
            DataSetInfo dataSet = new DataSetInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(dataSet));

            dataSet = _GetDataSet();
            Assert.AreEqual("{'connectionString':'DataSource=(local);Initial Catalog=Library;','tables':[{'columns':[{'name':'FIO','title':'ФИО','width':100,'type':'System.String','frozen':true},{'name':'Age','title':'Возраст','width':30,'type':'System.Int32'},{'name':'Ticket','title':'Билет','width':50,'type':'System.String','readOnly':true}],'name':'Readers'},{'columns':[{'name':'Author','title':'Автор','width':100,'type':'System.String','frozen':true},{'name':'Title','title':'Заглавие','width':150,'type':'System.String','readOnly':true},{'name':'Price','title':'Цена','width':30,'type':'System.Decimal'}],'name':'Books'}]}", JsonUtility.SerializeShort(dataSet));
        }
    }
}
