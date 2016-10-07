using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Caching;
using AM.IO;
using AM.Runtime;

namespace UnitTests.AM.Caching
{
    [TestClass]
    public class FileCacheTest
    {

        class DummyMan
            : IHandmadeSerializable
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public void RestoreFromStream(BinaryReader reader)
            {
                Name = reader.ReadNullableString();
                Age = reader.ReadPackedInt32();
            }

            public void SaveToStream(BinaryWriter writer)
            {
                writer
                    .WriteNullable(Name)
                    .WritePackedInt32(Age);
            }
        }

        [TestMethod]
        public void TestFileCache()
        {
            DummyMan[] sourceArray =
            {
                new DummyMan {Name = "Peter", Age = 10},
                new DummyMan {Name = "John", Age = 11},
                new DummyMan {Name = "Alice", Age = 12},
            };

            FileCache<int, DummyMan> cache = new FileCache<int, DummyMan>();
            for (int i = 0; i < sourceArray.Length; i++)
            {
                cache.Add(i, sourceArray[i]);
            }

            DummyMan[] restoredArray = new DummyMan[sourceArray.Length];

            for (int i = 0; i < sourceArray.Length; i++)
            {
                restoredArray[i] = cache.Get(i);
                Assert.IsNotNull(restoredArray[i]);
                Assert.AreEqual(sourceArray[i].Name, restoredArray[i].Name);
                Assert.AreEqual(sourceArray[i].Age, restoredArray[i].Age);
            }

            cache.Dispose();
        }
    }
}
