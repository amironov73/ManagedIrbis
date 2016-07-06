using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class SubFieldCollectionTest
    {
        [TestMethod]
        public void TestSubFieldCollectionConstruction()
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
        public void TestSubFieldCollectionSerialization()
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
        public void TestSubFieldCollectionNotNull()
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
        public void TestSubFieldCollectionReadOnly()
        {
            RecordField field = new RecordField().AsReadOnly();
            field.SubFields.Add(new SubField());
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
        public void TestSubFieldCollectionToJson()
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
        public void TestSubFieldCollectionFromJson()
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
        public void TestSubFieldCollectionAssign()
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
        public void TestSubFieldCollectionAssignClone()
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
    }
}
