using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.AM.IO
{
    [TestClass]
    public class StreamUtilityTest
    {
        [TestMethod]
        public void StreamUtility_AppendTo_1()
        {
            byte[] sourceArray = { 1, 2, 3, 4 };
            MemoryStream sourceStream = new MemoryStream(sourceArray);
            MemoryStream destinationStream = new MemoryStream();
            StreamUtility.AppendTo(sourceStream, destinationStream, -1);
            byte[] destinationArray = destinationStream.ToArray();
            Assert.AreEqual(4, destinationArray.Length);
            Assert.AreEqual(1, destinationArray[0]);
            Assert.AreEqual(2, destinationArray[1]);
            Assert.AreEqual(3, destinationArray[2]);
            Assert.AreEqual(4, destinationArray[3]);
        }

        private int _Compare
            (
                [NotNull] byte[] firstArray,
                [NotNull] byte[] secondArray
            )
        {
            MemoryStream firstStream = new MemoryStream(firstArray);
            MemoryStream secondStream = new MemoryStream(secondArray);
            int result = StreamUtility.CompareTo(firstStream, secondStream);

            return result;
        }

        [TestMethod]
        public void StreamUtility_CompareTo_1()
        {
            byte[] firstArray = { 1, 2, 3, 4 };
            byte[] secondArray = { 1, 2, 3, 4 };
            Assert.IsTrue(_Compare(firstArray, secondArray) == 0);

            secondArray = new byte[] { 1, 2, 3 };
            Assert.IsTrue(_Compare(firstArray, secondArray) > 0);

            secondArray = new byte[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(_Compare(firstArray, secondArray) < 0);

            secondArray = new byte[] { 1, 2, 3, 5 };
            Assert.IsTrue(_Compare(firstArray, secondArray) < 0);
        }

        [TestMethod]
        public void StreamUtility_ReadAsMuchAsPossible_1()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadAsMuchAsPossible(stream, 1000);
            Assert.AreEqual(4, readed.Length);
            Assert.AreEqual(1, readed[0]);
            Assert.AreEqual(2, readed[1]);
            Assert.AreEqual(3, readed[2]);
            Assert.AreEqual(4, readed[3]);
        }

        [TestMethod]
        public void StreamUtility_ReadAsMuchAsPossible_2()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadAsMuchAsPossible(stream, 3);
            Assert.AreEqual(3, readed.Length);
            Assert.AreEqual(1, readed[0]);
            Assert.AreEqual(2, readed[1]);
            Assert.AreEqual(3, readed[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StreamUtility_ReadAsMuchAsPossible_3()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            MemoryStream stream = new MemoryStream(buffer);
            StreamUtility.ReadAsMuchAsPossible(stream, -3);
        }

        [TestMethod]
        public void StreamUtility_ReadAsMuchAsPossible_4()
        {
            byte[] buffer = new byte[0];
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadAsMuchAsPossible(stream, 4);
            Assert.AreEqual(0, readed.Length);
        }

        [TestMethod]
        public void StreamUtility_ReadBoolean_1()
        {
            byte[] buffer = { 0, 1, 2 };
            MemoryStream stream = new MemoryStream(buffer);
            Assert.IsFalse(StreamUtility.ReadBoolean(stream));
            Assert.IsTrue(StreamUtility.ReadBoolean(stream));
            Assert.IsTrue(StreamUtility.ReadBoolean(stream));
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void StreamUtility_ReadBoolean_2()
        {
            byte[] buffer = new byte[0];
            MemoryStream stream = new MemoryStream(buffer);
            StreamUtility.ReadBoolean(stream);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_1()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadBytes(stream, 2);
            Assert.IsNotNull(readed);
            Assert.AreEqual(2, readed.Length);
            Assert.AreEqual(1, readed[0]);
            Assert.AreEqual(2, readed[1]);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_2()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadBytes(stream, 6);
            Assert.IsNotNull(readed);
            Assert.AreEqual(4, readed.Length);
            Assert.AreEqual(1, readed[0]);
            Assert.AreEqual(2, readed[1]);
            Assert.AreEqual(3, readed[2]);
            Assert.AreEqual(4, readed[3]);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_3()
        {
            byte[] buffer = new byte[0];
            MemoryStream stream = new MemoryStream(buffer);
            byte[] readed = StreamUtility.ReadBytes(stream, 6);
            Assert.IsNull(readed);
        }

        [TestMethod]
        public void StreamUtility_ReadDateTime_1()
        {
            MemoryStream stream = new MemoryStream();
            DateTime expected = new DateTime(2017, 12, 4, 12, 29, 0);
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            DateTime actual = StreamUtility.ReadDateTime(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadDecimal_1()
        {
            MemoryStream stream = new MemoryStream();
            decimal expected = 123.45m;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            decimal actual = StreamUtility.ReadDecimal(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadDouble_1()
        {
            MemoryStream stream = new MemoryStream();
            double expected = 123.45;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            double actual = StreamUtility.ReadDouble(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16_1()
        {
            MemoryStream stream = new MemoryStream();
            short expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            short actual = StreamUtility.ReadInt16(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32_1()
        {
            MemoryStream stream = new MemoryStream();
            int expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            int actual = StreamUtility.ReadInt32(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64_1()
        {
            MemoryStream stream = new MemoryStream();
            long expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            long actual = StreamUtility.ReadInt64(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt16_1()
        {
            MemoryStream stream = new MemoryStream();
            ushort expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            ushort actual = StreamUtility.ReadUInt16(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt32_1()
        {
            MemoryStream stream = new MemoryStream();
            uint expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            uint actual = StreamUtility.ReadUInt32(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt64_1()
        {
            MemoryStream stream = new MemoryStream();
            ulong expected = 123;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            ulong actual = StreamUtility.ReadUInt64(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Array_1()
        {
            MemoryStream stream = new MemoryStream();
            short[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            short[] actual = StreamUtility.ReadInt16Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt16Array_1()
        {
            MemoryStream stream = new MemoryStream();
            ushort[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            ushort[] actual = StreamUtility.ReadUInt16Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Array_1()
        {
            MemoryStream stream = new MemoryStream();
            int[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            int[] actual = StreamUtility.ReadInt32Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt32Array_1()
        {
            MemoryStream stream = new MemoryStream();
            uint[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            uint[] actual = StreamUtility.ReadUInt32Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadSingle_1()
        {
            MemoryStream stream = new MemoryStream();
            float expected = 123.45f;
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            float actual = StreamUtility.ReadSingle(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadString_1()
        {
            MemoryStream stream = new MemoryStream();
            string expected = "Hello, wolrd!";
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            string actual = StreamUtility.ReadString(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadString_2()
        {
            MemoryStream stream = new MemoryStream();
            string expected = "Hello, wolrd!";
            Encoding encoding = Encoding.ASCII;
            StreamUtility.Write(stream, expected, encoding);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            string actual = StreamUtility.ReadString(stream, encoding);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadStringArray_1()
        {
            MemoryStream stream = new MemoryStream();
            string[] expected = {"Hello", "wolrd!"};
            StreamUtility.Write(stream, expected);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            string[] actual = StreamUtility.ReadStringArray(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadStringArray_2()
        {
            MemoryStream stream = new MemoryStream();
            string[] expected = {"Hello", "wolrd!"};
            Encoding encoding = Encoding.ASCII;
            StreamUtility.Write(stream, expected, encoding);
            byte[] buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            string[] actual = StreamUtility.ReadStringArray(stream, encoding);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork16_1()
        {
            byte[] array = {1, 2};
            StreamUtility.HostToNetwork16(array, 0);
            Assert.AreEqual(2, array[0]);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork32_1()
        {
            byte[] array = {1, 2, 3, 4};
            StreamUtility.HostToNetwork32(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork64_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            StreamUtility.HostToNetwork64(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
            Assert.AreEqual(8, array[4]);
            Assert.AreEqual(7, array[5]);
            Assert.AreEqual(6, array[6]);
            Assert.AreEqual(5, array[7]);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Network_1()
        {
            byte[] array = {1, 2};
            MemoryStream stream = new MemoryStream(array);
            short actual = StreamUtility.ReadInt16Network(stream);
            Assert.AreEqual(0x0102, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Network_1()
        {
            byte[] array = {1, 2, 3, 4};
            MemoryStream stream = new MemoryStream(array);
            int actual = StreamUtility.ReadInt32Network(stream);
            Assert.AreEqual(0x01020304, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64Network_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            MemoryStream stream = new MemoryStream(array);
            long actual = StreamUtility.ReadInt64Network(stream);
            Assert.AreEqual(0x0506070801020304L, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Host_1()
        {
            byte[] array = {1, 2};
            MemoryStream stream = new MemoryStream(array);
            short actual = StreamUtility.ReadInt16Host(stream);
            Assert.AreEqual(0x0201, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Host_1()
        {
            byte[] array = {1, 2, 3, 4};
            MemoryStream stream = new MemoryStream(array);
            int actual = StreamUtility.ReadInt32Host(stream);
            Assert.AreEqual(0x04030201, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64Host_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            MemoryStream stream = new MemoryStream(array);
            long actual = StreamUtility.ReadInt64Host(stream);
            Assert.AreEqual(0x0807060504030201L, actual);
        }

        [TestMethod]
        public void StreamUtility_Write16Network_1()
        {
            MemoryStream stream = new MemoryStream();
            StreamUtility.WriteInt16Network(stream, 0x0102);
            byte[] array = stream.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }

        [TestMethod]
        public void StreamUtility_Write32Network_1()
        {
            MemoryStream stream = new MemoryStream();
            StreamUtility.WriteInt32Network(stream, 0x01020304);
            byte[] array = stream.ToArray();
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void StreamUtility_Write64Network_1()
        {
            MemoryStream stream = new MemoryStream();
            StreamUtility.WriteInt64Network(stream, 0x0102030405060708L);
            byte[] array = stream.ToArray();
            Assert.AreEqual(8, array.Length);
            Assert.AreEqual(5, array[0]);
            Assert.AreEqual(6, array[1]);
            Assert.AreEqual(7, array[2]);
            Assert.AreEqual(8, array[3]);
            Assert.AreEqual(1, array[4]);
            Assert.AreEqual(2, array[5]);
            Assert.AreEqual(3, array[6]);
            Assert.AreEqual(4, array[7]);
        }

        [TestMethod]
        public void StreamUtility_WriteBoolean_1()
        {
            MemoryStream stream = new MemoryStream();
            StreamUtility.Write(stream, false);
            StreamUtility.Write(stream, true);
            byte[] array = stream.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public void StreamUtility_ReadExact_1()
        {
            byte[] buffer = {1, 2, 3, 4, 5};
            MemoryStream stream = new MemoryStream(buffer);
            byte[] actual = StreamUtility.ReadExact(stream, 4);
            Assert.AreEqual(4, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(2, actual[1]);
            Assert.AreEqual(3, actual[2]);
            Assert.AreEqual(4, actual[3]);
            Assert.AreEqual(4L, stream.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void StreamUtility_ReadExact_2()
        {
            byte[] buffer = {1, 2};
            MemoryStream stream = new MemoryStream(buffer);
            StreamUtility.ReadExact(stream, 4);
        }

        [TestMethod]
        public void StreamUtility_ReadToEnd_1()
        {
            byte[] buffer = {1, 2, 3, 4, 5, 6, 7, 8};
            MemoryStream stream = new MemoryStream(buffer);
            StreamUtility.ReadExact(stream, 4);
            byte[] actual = StreamUtility.ReadToEnd(stream);
            Assert.AreEqual(4, actual.Length);
            Assert.AreEqual(5, actual[0]);
            Assert.AreEqual(6, actual[1]);
            Assert.AreEqual(7, actual[2]);
            Assert.AreEqual(8, actual[3]);
        }
    }
}
