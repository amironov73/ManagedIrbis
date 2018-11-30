using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;
using AM.Text;
using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordFieldCollectionTest
    {
        [TestMethod]
        public void RecordFieldCollection_Construction_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
                {
                    new RecordField(),
                    new RecordField(200),
                    new RecordField(300, "Value")
                };
            Assert.AreEqual(3, collection.Count);
        }

        private void _TestSerialization
            (
                params RecordField[] subFields
            )
        {
            RecordFieldCollection collection1 = new RecordFieldCollection();
            collection1.AddRange(subFields);

            byte[] bytes = collection1.SaveToMemory();

            RecordFieldCollection collection2 = bytes
                    .RestoreObjectFromMemory<RecordFieldCollection>();

            Assert.AreEqual(collection1.Count, collection2.Count);

            for (int i = 0; i < collection1.Count; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        RecordField.Compare
                            (
                                collection1[i],
                                collection2[i]
                            )
                    );
            }
        }

        [TestMethod]
        public void RecordFieldCollection_Serialization_1()
        {
            _TestSerialization();

            _TestSerialization
                (
                    new RecordField(),
                    new RecordField(200),
                    new RecordField(300, "Value")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RecordFieldCollection_NotNull_1()
        {
            RecordFieldCollection collection =
                new RecordFieldCollection
                {
                    new RecordField(),
                    null,
                    new RecordField(200)
                };
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void RecordFieldCollection_ReadOnly_1()
        {
            RecordFieldCollection collection =
                new RecordFieldCollection
                {
                    new RecordField(),
                    new RecordField(200),
                    new RecordField(300, "Value")
                }.AsReadOnly();

            collection.Add(new RecordField());
        }

        [TestMethod]
        public void RecordFieldCollection_AddCapacity()
        {
            RecordFieldCollection collection = new RecordFieldCollection();
            collection.AddCapacity(10);
            Assert.IsTrue(collection.Capacity >= 10);
        }

        [TestMethod]
        public void RecordFieldCollection_BeginUpdate_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection();
            collection.BeginUpdate();
            collection.Add(new RecordField(100, "Field100#1"));
            collection.Add(new RecordField(100, "Field100#2"));
            collection.Add(new RecordField(100, "Field100#3"));
            Assert.AreEqual("100/0", collection[0].Path);
            Assert.AreEqual("100/0", collection[1].Path);
            Assert.AreEqual("100/0", collection[2].Path);
        }

        [TestMethod]
        public void RecordFieldCollection_Clear_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100),
                new RecordField(200),
                new RecordField(300)
            };
            Assert.AreEqual(3, collection.Count);
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void RecordFieldCollection_EndUpdate_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection();
            collection.BeginUpdate();
            collection.Add(new RecordField(100, "Field100#1"));
            collection.Add(new RecordField(100, "Field100#2"));
            collection.Add(new RecordField(100, "Field100#3"));
            collection.EndUpdate();
            Assert.AreEqual("100/1", collection[0].Path);
            Assert.AreEqual("100/2", collection[1].Path);
            Assert.AreEqual("100/3", collection[2].Path);
        }

        [TestMethod]
        public void RecordFieldCollection_EnsureCapacity_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection();
            collection.EnsureCapacity(10);
            Assert.IsTrue(collection.Capacity >= 10);
        }

        [TestMethod]
        public void RecordFieldCollection_SetRecord_1()
        {
            MarcRecord record = new MarcRecord();
            Assert.AreSame(record, record.Fields.Record);
        }

        [TestMethod]
        public void RecordFieldCollection_SetRecord_2()
        {
            MarcRecord first = new MarcRecord()
                .AddField(100, "Field100")
                .AddField(200, "Field200")
                .AddField(300, "Field300");
            MarcRecord second = first.Clone();
            Assert.AreSame(second, second.Fields.Record);
        }

        [TestMethod]
        public void RecordFieldCollection_ApplyFieldValue_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            string newValue = "NewField200";
            collection.ApplyFieldValue(200, newValue);
            Assert.AreEqual(newValue, collection[1].Value);
        }

        [TestMethod]
        public void RecordFieldCollection_ApplyFieldValue_2()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            collection.ApplyFieldValue(200, null);
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void RecordFieldCollection_ApplyFieldValue_4()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            string value = "Field400";
            collection.ApplyFieldValue(400, value);
            Assert.AreEqual(4, collection.Count);
            Assert.AreEqual(value, collection[3].Value);
        }

        [TestMethod]
        public void RecordFieldCollection_ApplyFieldValue_5()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            collection.ApplyFieldValue(400, null);
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void RecordFieldCollection_Find_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            RecordField found = collection.Find(field => field.Tag > 100);
            Assert.IsNotNull(found);
            Assert.AreEqual(200, found.Tag);

            found = collection.Find(field => field.Tag < 100);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void RecordFieldCollection_FindAll_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            RecordField[] found = collection.FindAll(field => field.Tag > 100);
            Assert.AreEqual(2, found.Length);

            found = collection.FindAll(field => field.Tag < 100);
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void RecordFieldCollection_SetItem_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            collection[1] = new RecordField(220, "Field220");
            Assert.AreEqual(220, collection[1].Tag);
        }

        [TestMethod]
        public void RecordFieldCollection_ToString_1()
        {
            RecordFieldCollection collection = new RecordFieldCollection
            {
                new RecordField(100, "Field100"),
                new RecordField(200, "Field200"),
                new RecordField(300, "Field300")
            };
            string actual = collection.ToString().DosToUnix();
            string expected = "100#Field100\n200#Field200\n300#Field300";
            Assert.AreEqual(expected, actual);
        }
    }
}
