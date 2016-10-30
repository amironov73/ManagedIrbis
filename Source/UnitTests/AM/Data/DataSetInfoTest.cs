using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class DataSetInfoTest
    {
        [TestMethod]
        public void DataSetInfo_Construction()
        {
            DataSetInfo info = new DataSetInfo();
            Assert.AreEqual(null, info.ConnectionString);
            Assert.AreEqual(false, info.ReadOnly);
            Assert.AreEqual(null, info.SelectCommandText);
            Assert.AreEqual(0, info.Tables.Count);
        }

        [TestMethod]
        public void DataSetInfo_ConnectionString()
        {
            const string expected = "server=(local)";
            DataSetInfo info = new DataSetInfo
            {
                ConnectionString = expected
            };
            Assert.AreEqual(expected, info.ConnectionString);
        }

        [TestMethod]
        public void DataSetInfo_ReadOnly()
        {
            const bool expected = true;
            DataSetInfo info = new DataSetInfo
            {
                ReadOnly = expected
            };
            Assert.AreEqual(expected, info.ReadOnly);
        }

        [TestMethod]
        public void DataSetInfo_SelectCommandText()
        {
            const string expected = "select * from users";
            DataSetInfo info = new DataSetInfo
            {
                SelectCommandText = expected
            };
            Assert.AreEqual(expected, info.SelectCommandText);
        }

        [TestMethod]
        public void DataSetInfo_Tables()
        {
            DataSetInfo info = new DataSetInfo
            {
                Tables =
                {
                    new DataTableInfo
                    {
                        Name = "Users"
                    }
                }
            };
            Assert.AreEqual(1, info.Tables.Count);
        }

        [TestMethod]
        public void DataSetInfo_Load_Save()
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
    }
}
