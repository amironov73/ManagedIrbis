using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;
using AM.Text;
using JetBrains.Annotations;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldCollectionTest
    {
        [NotNull]
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
        public void SubFieldCollection_Constructor_1()
        {
            SubFieldCollection collection = new SubFieldCollection
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
        public void SubFieldCollection_Serialization_1()
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
        public void SubFieldCollection_NotNull_1()
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
        public void SubFieldCollection_ReadOnly_1()
        {
            RecordField field = new RecordField().AsReadOnly();
            field.SubFields.Add(new SubField());
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubFieldCollection_ReadOnly_2()
        {
            SubFieldCollection collection = new SubFieldCollection();
            collection.SetReadOnly();
            collection.Add(new SubField());
        }

        [TestMethod]
        public void SubFieldCollection_ToJson_1()
        {
            SubFieldCollection collection = _GetCollection();

            string actual = collection.ToJson().DosToUnix();
            const string expected = "[\n  {\n    \"code\": \"a\",\n    \"value\": \"Subfield A\"\n  },\n  {\n    \"code\": \"b\",\n    \"value\": \"Subfield B\"\n  },\n  {\n    \"code\": \"c\",\n    \"value\": \"Subfield C\"\n  }\n]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubFieldCollection_FromJson_1()
        {
            const string text = "[\n  {\n    \"code\": \"a\",\n    \"value\": \"Subfield A\"\n  },\n  {\n    \"code\": \"b\",\n    \"value\": \"Subfield B\"\n  },\n  {\n    \"code\": \"c\",\n    \"value\": \"Subfield C\"\n  }\n]";
            SubFieldCollection collection = SubFieldCollection.FromJson(text);

            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual('a', collection[0].Code);
            Assert.AreEqual("Subfield A", collection[0].Value);
            Assert.AreEqual('b', collection[1].Code);
            Assert.AreEqual("Subfield B", collection[1].Value);
            Assert.AreEqual('c', collection[2].Code);
            Assert.AreEqual("Subfield C", collection[2].Value);
        }

        [TestMethod]
        public void SubFieldCollection_Assign_1()
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
        public void SubFieldCollection_AssignClone_1()
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
        public void SubFieldCollection_SetField_1()
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
        public void SubFieldCollection_Clone_1()
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
        public void SubFieldCollection_Find_1()
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
        public void SubFieldCollection_FindAll_1()
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
        public void SubFieldCollection_SetReadOnly_1()
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
        public void SubFieldCollection_AsReadOnly_1()
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
        public void SubFieldCollection_ClearItems_1()
        {
            SubFieldCollection collection = _GetCollection();
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void SubFieldCollection_InsertItem_1()
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
        public void SubFieldCollection_InsertItem_2()
        {
            SubFieldCollection collection = _GetCollection();
            collection.Insert(1, null);
        }

        [TestMethod]
        public void SubFieldCollection_RemoveItem_1()
        {
            SubFieldCollection collection = _GetCollection();
            Assert.AreEqual(3, collection.Count);
            collection.Remove(collection[1]);
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void SubFieldCollection_SetItem_1()
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
        public void SubFieldCollection_SetItem_2()
        {
            SubFieldCollection collection = _GetCollection();
            collection[1] = null;
        }
    }
}
