using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Collections;
using UnsafeAM.IO;
using UnsafeAM.Runtime;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.UnsafeAM.IO
{
    [TestClass]
    public class BinaryWriterUtilityTest
    {
        private class Dummy
            : IHandmadeSerializable
        {
            public int Value { get; set; }

            public void RestoreFromStream(BinaryReader reader)
            {
                Value = reader.ReadInt32();
            }

            public void SaveToStream(BinaryWriter writer)
            {
                writer.Write(Value);
            }
        }

        // ==========================================================

        [TestMethod]
        public void BinaryWriterUtility_Write_CollectionT_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            NonNullCollection<Dummy> expected = new NonNullCollection<Dummy>
            {
                new Dummy { Value = 123 },
                new Dummy { Value = 456 },
                new Dummy { Value = 789 }
            };
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            NonNullCollection<Dummy> actual
                = BinaryReaderUtility.ReadNonNullCollection<Dummy>(reader);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].Value, actual[0].Value);
            Assert.AreEqual(expected[1].Value, actual[1].Value);
            Assert.AreEqual(expected[2].Value, actual[2].Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Byte_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            byte? expected = 123;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            byte? actual = BinaryReaderUtility.ReadNullableByte(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Byte_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            byte? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            byte? actual = BinaryReaderUtility.ReadNullableByte(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_DateTime_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            DateTime? expected = new DateTime(2018, 2, 13, 12, 8, 0);
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            DateTime? actual = BinaryReaderUtility.ReadNullableDateTime(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_DateTime_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            DateTime? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            DateTime? actual = BinaryReaderUtility.ReadNullableDateTime(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Decimal_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            decimal? expected = 123.45m;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            decimal? actual = BinaryReaderUtility.ReadNullableDecimal(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Decimal_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            decimal? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            decimal? actual = BinaryReaderUtility.ReadNullableDecimal(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Double_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            double? expected = 123.45;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            double? actual = BinaryReaderUtility.ReadNullableDouble(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Double_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            double? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            double? actual = BinaryReaderUtility.ReadNullableDouble(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int16_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            short? expected = 123;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            short? actual = BinaryReaderUtility.ReadNullableInt16(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int16_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            short? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            short? actual = BinaryReaderUtility.ReadNullableInt16(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int32_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            int? expected = 123;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int? actual = BinaryReaderUtility.ReadNullableInt32(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int32_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            int? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int? actual = BinaryReaderUtility.ReadNullableInt32(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int64_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            long? expected = 123;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            long? actual = BinaryReaderUtility.ReadNullableInt64(reader);
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_Nullable_Int64_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            long? expected = null;
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            long? actual = BinaryReaderUtility.ReadNullableInt64(reader);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void BinaryWriterUtility_Write_DateTime_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            DateTime expected = new DateTime(2018, 2, 13, 11, 55, 0);
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            DateTime actual = BinaryReaderUtility.ReadDateTime(reader);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinaryWriterUtility_WritePackedInt32_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            int[] values = {0, 1, 100, 127, 256, 1000,
                1000000, 20000030, 2012345678,
                -1, -2, -100, -127, -128, -256, -1000,
                -1000000, -20000030, -2012345678 };
            foreach (int value in values)
            {
                writer.WritePackedInt32(value);
            }

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            for (int i = 0; i < values.Length; i++)
            {
                int value = reader.ReadPackedInt32();
                Assert.AreEqual(values[i], value);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WritePackedInt64_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            long[] values = {0, 1, 100, 127, 256, 1000,
                1000000, 20000030, 2012345678, 2012345678901,
                2012345678901234 };
            foreach (long value in values)
            {
                writer.WritePackedInt64(value);
            }

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            for (int i = 0; i < values.Length; i++)
            {
                long value = reader.ReadPackedInt64();
                Assert.AreEqual(values[i], value);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteListT_1()
        {
            List<Dummy> list1 = new List<Dummy>();
            for (int i = 0; i < 10; i++)
            {
                Dummy item = new Dummy { Value = i };
                list1.Add(item);
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.WriteList(list1);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            List<Dummy> list2 = reader.ReadList<Dummy>();

            Assert.AreEqual(list1.Count, list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Dummy item1 = list1[i];
                Dummy item2 = list2[i];
                Assert.AreEqual(item1.Value, item2.Value);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArrayT_1()
        {
            Dummy[] expected = new Dummy[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = new Dummy { Value = i };
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            Dummy[] actual = BinaryReaderUtility.ReadArray<Dummy>(reader);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Dummy item1 = expected[i];
                Dummy item2 = actual[i];
                Assert.AreEqual(item1.Value, item2.Value);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArray_Byte_1()
        {
            byte[] expected = new byte[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = (byte) i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            byte[] actual = BinaryReaderUtility.ReadByteArray(reader);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArray_Int16_1()
        {
            short[] expected = new short[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = (short) i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            short[] actual = BinaryReaderUtility.ReadInt16Array(reader);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArray_Int32_1()
        {
            int[] expected = new int[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int[] actual = BinaryReaderUtility.ReadInt32Array(reader);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArray_Int64_1()
        {
            long[] expected = new long[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            long[] actual = BinaryReaderUtility.ReadInt64Array(reader);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteArray_String_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            string[] expected = { "Hello", "World" };
            BinaryWriterUtility.WriteArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            string[] actual = BinaryReaderUtility.ReadStringArray(reader);

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullable_String_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            string expected = "Hello";
            BinaryWriterUtility.WriteNullable(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            string actual = BinaryReaderUtility.ReadNullableString(reader);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullable_String_2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            string expected = null;
            BinaryWriterUtility.WriteNullable(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            string actual = BinaryReaderUtility.ReadNullableString(reader);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArray_Int32_1()
        {
            int[] expected = new int[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int[] actual = BinaryReaderUtility.ReadNullableInt32Array(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArray_Int32_2()
        {
            int[] expected = null;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int[] actual = BinaryReaderUtility.ReadNullableInt32Array(reader);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArray_String_1()
        {
            string[] expected = new string[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = "Item" + i;
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            string[] actual = BinaryReaderUtility.ReadNullableStringArray(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArray_String_2()
        {
            string[] expected = null;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            string[] actual = BinaryReaderUtility.ReadNullableStringArray(reader);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArrayT_1()
        {
            Dummy[] expected = new Dummy[10];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = new Dummy { Value = i };
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            Dummy[] actual = BinaryReaderUtility.ReadNullableArray<Dummy>(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Dummy item1 = expected[i];
                Dummy item2 = actual[i];
                Assert.AreEqual(item1.Value, item2.Value);
            }
        }

        [TestMethod]
        public void BinaryWriterUtility_WriteNullableArrayT_2()
        {
            Dummy[] expected = null;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryWriterUtility.WriteNullableArray(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            Dummy[] actual = BinaryReaderUtility.ReadNullableArray<Dummy>(reader);
            Assert.IsNull(actual);
        }
    }
}
