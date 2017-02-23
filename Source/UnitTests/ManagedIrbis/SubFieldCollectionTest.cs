using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

namespace UnitTests
{
    [TestClass]
    public class SubFieldCollectionTest
    {
        [TestMethod]
        public void SubFieldCollection_Constructor1()
        {
            SubFieldCollection collection =
                new SubFieldCollection
                {
                    new SubField(),
                    new SubField('a'),
                    new SubField('b', "Value")
                };
            Assert.AreEqual(3, collection.Count);
        }

        private void _TestSerialization
            (
                params SubField[] subFields
            )
        {
            SubFieldCollection collection1 = new SubFieldCollection();
            collection1.AddRange(subFields);

            //collection1.SaveToFile("collection1.bin");
            //collection1.SaveToZipFile("collection1.biz");

            byte[] bytes = collection1.SaveToMemory();

            SubFieldCollection collection2 = bytes
                    .RestoreObjectFromMemory <SubFieldCollection>();

            Assert.AreEqual(collection1.Count, collection2.Count);

            for (int i = 0; i < collection1.Count; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare
                        (
                            collection1[i],
                            collection2[i]
                        )
                    );
            }
        }

        [TestMethod]
        public void SubFieldCollection_Serialization1()
        {
            _TestSerialization();

            _TestSerialization
                (
                    new SubField(),
                    new SubField('a'),
                    new SubField('b', "Hello")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubFieldCollection_NotNull1()
        {
            SubFieldCollection collection =
                new SubFieldCollection
                {
                    new SubField(),
                    null,
                    new SubField('a')
                };
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubFieldCollection_ReadOnly1()
        {
            RecordField field = new RecordField().AsReadOnly();
            field.SubFields.Add(new SubField());
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubFieldCollection_ReadOnly2()
        {
            SubFieldCollection collection = new SubFieldCollection();
            collection.SetReadOnly();            
            collection.Add(new SubField());
        }

        private SubFieldCollection _GetCollection()
        {
            SubFieldCollection result = new SubFieldCollection
            {
                new SubField('a', "Subfield A"),
                new SubField('b', "Subfield B"),
                new SubField('c', "Subfield C")
            };

            return result;
        }

        [TestMethod]
        public void SubFieldCollection_ToJson1()
        {
            SubFieldCollection collection = _GetCollection();

            string actual = collection.ToJson()
                .Replace("\r","").Replace("\n","");
            const string expected = @"["
+@"  {"
+@"    ""code"": ""a"","
+@"    ""value"": ""Subfield A"""
+@"  },"
+@"  {"
+@"    ""code"": ""b"","
+@"    ""value"": ""Subfield B"""
+@"  },"
+@"  {"
+@"    ""code"": ""c"","
+@"    ""value"": ""Subfield C"""
+@"  }"
+@"]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubFieldCollection_FromJson1()
        {
            const string text = @"["
+@"  {"
+@"    ""code"": ""a"","
+@"    ""value"": ""Subfield A"""
+@"  },"
+@"  {"
+@"    ""code"": ""b"","
+@"    ""value"": ""Subfield B"""
+@"  },"
+@"  {"
+@"    ""code"": ""c"","
+@"    ""value"": ""Subfield C"""
+@"  }"
+@"]";
            SubFieldCollection collection
                = SubFieldCollection.FromJson(text);

            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual('a', collection[0].Code);
            Assert.AreEqual("Subfield A", collection[0].Value);
            Assert.AreEqual('b', collection[1].Code);
            Assert.AreEqual("Subfield B", collection[1].Value);
            Assert.AreEqual('c', collection[2].Code);
            Assert.AreEqual("Subfield C", collection[2].Value);
        }

        [TestMethod]
        public void SubFieldCollection_Assign1()
        {
            SubFieldCollection source = _GetCollection();
            SubFieldCollection target = new SubFieldCollection();
            target.Assign(source);

            Assert.AreEqual(source.Field, target.Field);
            Assert.AreEqual(source.Count, target.Count);
            for (int i = 0; i < source.Count; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare
                        (
                            source[i],
                            target[i]
                        )
                    );
            }
        }

        [TestMethod]
        public void SubFieldCollection_AssignClone1()
        {
            SubFieldCollection source = _GetCollection();
            SubFieldCollection target = new SubFieldCollection();
            target.AssignClone(source);

            Assert.AreEqual(source.Field, target.Field);
            Assert.AreEqual(source.Count, target.Count);
            for (int i = 0; i < source.Count; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare
                        (
                            source[i],
                            target[i]
                        )
                    );
            }
        }

        [TestMethod]
        public void SubFieldCollection_SetField1()
        {
            RecordField field = new RecordField("200");
            SubField subFieldA = new SubField('a', "Title1");
            field.SubFields.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            field.SubFields.Add(subFieldE);
            field.SetSubField('a', "Title2");
            Assert.AreEqual("Title2", subFieldA.Value);
            Assert.AreEqual("Subtitle", subFieldE.Value);
        }

        [TestMethod]
        public void SubFieldCollection_Clone1()
        {
            SubFieldCollection collection = new SubFieldCollection();
            SubField subFieldA = new SubField('a', "Title1");
            collection.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            collection.Add(subFieldE);

            SubFieldCollection clone = collection.Clone();
            Assert.AreEqual(collection.Count, clone.Count);
        }

        [TestMethod]
        public void SubFieldCollection_Find1()
        {
            SubFieldCollection collection = new SubFieldCollection();
            SubField subFieldA = new SubField('a', "Title1");
            collection.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            collection.Add(subFieldE);

            SubField found = collection.Find
                (
                    x => x.Value.SameString("Subtitle")
                );
            Assert.AreEqual(subFieldE, found);

            found = collection.Find
                (
                    x => x.Value.SameString("Notitle")
                );
            Assert.IsNull(found);
        }

        [TestMethod]
        public void SubFieldCollection_FindAll1()
        {
            SubFieldCollection collection = new SubFieldCollection();
            SubField subFieldA = new SubField('a', "Title1");
            collection.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            collection.Add(subFieldE);

            SubField[] found = collection.FindAll
                (
                    x => x.Value.SameString("Subtitle")
                );
            Assert.AreEqual(1, found.Length);

            found = collection.FindAll
                (
                    x => x.Value.SameString("Notitle")
                );
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubFieldCollection_SetReadOnly1()
        {
            SubFieldCollection collection = new SubFieldCollection();
            SubField subFieldA = new SubField('a', "Title1");
            collection.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            collection.Add(subFieldE);

            collection.SetReadOnly();

            subFieldA.Value = "New value";
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubFieldCollection_AsReadOnly1()
        {
            SubFieldCollection collection = new SubFieldCollection();
            SubField subFieldA = new SubField('a', "Title1");
            collection.Add(subFieldA);
            SubField subFieldE = new SubField('e', "Subtitle");
            collection.Add(subFieldE);

            collection = collection.AsReadOnly();
            collection.Add(new SubField());
        }

        [TestMethod]
        public void SubFieldCollection_ClearItems1()
        {
            SubFieldCollection collection = _GetCollection();
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void SubFieldCollection_InsertItem1()
        {
            SubFieldCollection collection = _GetCollection();
            Assert.AreEqual(3, collection.Count);
            SubField subField = new SubField('d', "Subfield D");
            collection.Insert(1, subField);
            Assert.AreEqual(subField, collection[1]);
            Assert.AreEqual(4, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubFieldCollection_InsertItem_Exception()
        {
            SubFieldCollection collection = _GetCollection();
            collection.Insert(1, null);
        }

        [TestMethod]
        public void SubFieldCollection_RemoveItem1()
        {
            SubFieldCollection collection = _GetCollection();
            Assert.AreEqual(3, collection.Count);
            collection.Remove(collection[1]);
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void SubFieldCollection_SetItem1()
        {
            SubFieldCollection collection = _GetCollection();
            Assert.AreEqual(3, collection.Count);
            SubField subField = new SubField('d', "Subfield D");
            collection[1] = subField;
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(subField, collection[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubFieldCollection_SetItem_Exception()
        {
            SubFieldCollection collection = _GetCollection();
            collection[1] = null;
        }
    }
}
